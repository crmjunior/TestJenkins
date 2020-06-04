using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Newtonsoft.Json;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class QuestaoBusiness
    {
        private readonly IQuestaoData _questaoRepository;
        private readonly IImagemData _imagemRepository;
        private readonly IVideoData _videoRepository;
        private readonly IEspecialidadeData _especialidadeRepository;
        private readonly IFuncionarioData _funcionarioRepository;
        private readonly ISimuladoData _simuladoRepository;
        private readonly ILogOperacoesConcursoData _logRepository;
        private readonly INotificacaoData _notificacaoRepository;
		private readonly IMontaProvaData _montaProvaRepository;

        public QuestaoBusiness(IQuestaoData questaoRepository, IImagemData imagemRepository, IVideoData videoRepository, IEspecialidadeData especialidadeRepository)
        {
            _questaoRepository = questaoRepository;
            _imagemRepository = imagemRepository;
            _videoRepository = videoRepository;
            _especialidadeRepository = especialidadeRepository;
        }

        public QuestaoBusiness(IQuestaoData questaoRepository, IImagemData imagemRepository, IVideoData videoRepository, IEspecialidadeData especialidadeRepository, IFuncionarioData funcionarioRepository)
        {
            _questaoRepository = questaoRepository;
            _imagemRepository = imagemRepository;
            _videoRepository = videoRepository;
            _especialidadeRepository = especialidadeRepository;
            _funcionarioRepository = funcionarioRepository;
        }

        public QuestaoBusiness(IQuestaoData questaoRepository, IImagemData imagemRepository, IVideoData videoRepository, IEspecialidadeData especialidadeRepository, IFuncionarioData funcionarioRepository, ISimuladoData simuladoRepository)
        {
            _questaoRepository = questaoRepository;
            _imagemRepository = imagemRepository;
            _videoRepository = videoRepository;
            _especialidadeRepository = especialidadeRepository;
            _funcionarioRepository = funcionarioRepository;
            _simuladoRepository = simuladoRepository;
        }

		public QuestaoBusiness(IQuestaoData questaoRepository, IImagemData imagemRepository, IVideoData videoRepository, IEspecialidadeData especialidadeRepository, IFuncionarioData funcionarioRepository, ISimuladoData simuladoRepository, IMontaProvaData montaProvaRepository)
		{
			_questaoRepository = questaoRepository;
			_imagemRepository = imagemRepository;
			_videoRepository = videoRepository;
			_especialidadeRepository = especialidadeRepository;
			_funcionarioRepository = funcionarioRepository;
			_simuladoRepository = simuladoRepository;
			_montaProvaRepository = montaProvaRepository;
		}

		public QuestaoBusiness(QuestaoEntity questaoRepository)
        {
            _questaoRepository = questaoRepository;
        }

        public QuestaoBusiness(IQuestaoData questaoRepository, IFuncionarioData funcionarioRepository)
        {
            _questaoRepository = questaoRepository;
            _funcionarioRepository = funcionarioRepository;

        }

        public QuestaoBusiness()
        {

        }

        public QuestaoBusiness(QuestaoEntity questaoEntity, LogOperacoesConcursoEntity logOperacoesConcursoEntity)
        {
            _questaoRepository = questaoEntity;
            _logRepository = logOperacoesConcursoEntity;
        }

        public QuestaoBusiness(IQuestaoData questaoRepository, INotificacaoData notificacaoRepository)
        {
            _questaoRepository = questaoRepository;
            _notificacaoRepository = notificacaoRepository;
        }

        public int SetRespostaObjetiva(RespostaObjetivaPost resp)
        {
            using(MiniProfiler.Current.Step("Incluindo resposta objetiva"))
            {
                InserirLogSetRespostaObjetiva(resp);

                var exercicioHistorico = _questaoRepository.GetExercicioHistorico(resp.HistoricoId);

                if (exercicioHistorico == null)
                    return 0;

                var tipoProva = exercicioHistorico.intTipoProva == null ? 0 : exercicioHistorico.intTipoProva.Value;
                if (tipoProva == (int)TipoProvaEnum.ModoOnline)
                {
                    var ultimaMarcacao = _questaoRepository.GetUltimaMarcacao_SimuladoOnline(resp.QuestaoId, resp.ExercicioTipoId, resp.Matricula);
                    var retorno = _questaoRepository.SetRespostaObjetivaSimuladoOnline(resp, ultimaMarcacao);
                    new Task(() => verificarProvasQuestao(resp, exercicioHistorico, ultimaMarcacao, tipoProva)).Start();
                    return retorno;
                }
                else
                {
                    var ultimaMarcacao = _questaoRepository.GetUltimaMarcacaobyIntExercicioHistoricoID(resp.QuestaoId, resp.ExercicioTipoId, resp.Matricula, exercicioHistorico.intTipoProva,  resp.HistoricoId);
                    var retorno = _questaoRepository.SetRespostaObjetiva(resp, ultimaMarcacao);
                    new Task(() => verificarProvasQuestao(resp, exercicioHistorico, ultimaMarcacao, tipoProva)).Start();
                    return retorno;
                }
            }
        }

        public int SetRespostaObjetivaSimuladoAgendado(RespostaObjetivaPost resp)
        {
            using(MiniProfiler.Current.Step("Gravando resposta objetiva simulado agendado"))
            {
                InserirLogSetRespostaObjetiva(resp);
                var ultimaMarcacao = _questaoRepository.GetUltimaMarcacao_SimuladoOnline(resp.QuestaoId, resp.ExercicioTipoId, resp.Matricula);
                return _questaoRepository.SetRespostaObjetivaSimuladoOnline(resp, ultimaMarcacao);
            }
        }

        private void InserirLogSetRespostaObjetiva(RespostaObjetivaPost resp)
        {
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = resp.Matricula,
                IdApp = 0,
                Tela = Util.Log.MsProLog_Tela.RealizaProvaQuestao,
                Acao = Util.Log.MsProLog_Acao.Abriu,
                Obs = string.Format("Exercício tipo: {0} - ID Questão: {1} - Alternativa: {2} - Id Histórico: {3}", resp.ExercicioTipoId, resp.QuestaoId, resp.Alterantiva, resp.HistoricoId)
            });
        }

        		public void verificarProvasQuestao(RespostaObjetivaPost resp, ExercicioHistoricoDTO exercicioHistorico, MarcacoesObjetivaDTO ultimaMarcacao, int? tipoProva)
		{
			try
			{
				var selecionadaAnterior = ultimaMarcacao != null ? ultimaMarcacao.Resposta : null;
				var selecionadaAtual = resp.Alterantiva;

				if (selecionadaAtual != selecionadaAnterior)
				{
					var provas = _montaProvaRepository.ObterIdProvasPorQuestao(resp.QuestaoId);
					var contadores = _montaProvaRepository.ObterContadorDeQuestoes(resp.Matricula);
					var provasContadores = contadores.Join(provas, c => c.ID, p => p, (x, y) => x).ToList();

					if (provasContadores.Any())
					{
						provasContadores.ForEach(provaContador =>
						{
							var questoes = _montaProvaRepository.GetQuestoesProva(provaContador);

							if (questoes.Any())
							{
								var questoesSimulado = new MontaProvaBusiness(new MontaProvaEntity()).ObterProvaSimulado(resp.Matricula, questoes);
								var questoesConcurso = new MontaProvaBusiness(new MontaProvaEntity()).ObterProvaConcurso(resp.Matricula, questoes);

								provaContador.NaoRealizadas = questoesSimulado.NaoRealizadas + questoesConcurso.NaoRealizadas;
								provaContador.Acertos = questoesSimulado.Acertos + questoesConcurso.Acertos;
								provaContador.Erros = questoesSimulado.Erros + questoesConcurso.Erros;

								_montaProvaRepository.AtualizarContadorDeQuestoes(provaContador);
							}
						});
					}
				}
			}
			catch { }
		}

        public QuestaoApostilaDownload GetQuestaoApostilaDownload(int QuestaoID, int ClientID, int ApplicationID, string appVersion = "")
        {
            var entidadeQuestao = ObterDetalhesQuestaoConcurso(QuestaoID, ClientID, ApplicationID, appVersion);

            entidadeQuestao.MediaComentario.Imagens = null;
            entidadeQuestao.MediaComentario.Video = null;

            var lstImagens = _imagemRepository.GetImagensQuestaoConcurso(QuestaoID);
            var LstImagens64 = new List<String>();

            if (lstImagens.Any())
            {
                foreach (var imgID in lstImagens)
                {
                    var imagemBase64 = _imagemRepository.GetConcursoBase64(imgID);
                    LstImagens64.Add(imagemBase64);
                }
            }

            entidadeQuestao.MediaComentario.Imagens = LstImagens64;

            var forumRecurso = _questaoRepository.GetForumQuestaoRecurso(QuestaoID, ClientID);

            var estatisticas = _questaoRepository.GetEstatistica(QuestaoID, (int)Exercicio.tipoExercicio.CONCURSO);

            var questaoApostilaDownload = new QuestaoApostilaDownload
            {
                Questao = entidadeQuestao,
                ForumRecurso = forumRecurso,
                Estatistica = estatisticas
            };

            return questaoApostilaDownload;
        }

        public List<QuestaoApostilaDownload> GetListaQuestaoApostilaDownload(QuestoesDownloadRequestDTO questoesDownloadRequest, string appVersion = "")
        {
            List<QuestaoApostilaDownload> questoes = new List<QuestaoApostilaDownload>();


            Parallel.ForEach(questoesDownloadRequest.QuestoesIds, questaoId =>
            {
                var questao = GetQuestaoApostilaDownload(questaoId, questoesDownloadRequest.ClientId, questoesDownloadRequest.ApplicationId, appVersion);
                questao.Questao.Id = questaoId;
                questoes.Add(questao);

            });
            return questoes;
        }

        
        private void InserirLogAberturaQuestao(int QuestaoID, int ClientID, int ApplicationID)
        {
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = ClientID,
                IdApp = (Aplicacoes)ApplicationID,
                Tela = Util.Log.MsProLog_Tela.RealizaProvaQuestao,
                Acao = Util.Log.MsProLog_Acao.Abriu,
                Obs = string.Format("CI - ID: {0}", QuestaoID)
            });
        }

        public Questao ObterDetalhesQuestaoConcurso(int QuestaoID, int ClientID, int ApplicationID, string appVersion = "")
        {
            var tipoUsuario = _funcionarioRepository.GetTipoPerfilUsuario(ClientID);
            var isAcademico = tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador || tipoUsuario == EnumTipoPerfil.Master;

            InserirLogAberturaQuestao(QuestaoID, ClientID, ApplicationID);

            var entidadeQuestao = new Questao();

            var questao = _questaoRepository.ObterQuestaoConcurso(QuestaoID);

            var gabarito = _questaoRepository.ObterRespostaQuestaoConcurso(QuestaoID);

            bool questaoPossuiTextoDeGabaritoNoEnunciado = questao.txtEnunciado.ToUpper().Contains("GABARITO");

            if (questaoPossuiTextoDeGabaritoNoEnunciado)
            {
                if (string.IsNullOrEmpty(gabarito))
                    gabarito = questao.txtEnunciado.Substring(questao.txtEnunciado.ToUpper().IndexOf("GABARITO"));
                questao.txtEnunciado = questao.txtEnunciado.Remove(questao.txtEnunciado.ToUpper().IndexOf("GABARITO"));
            }

            if (string.IsNullOrEmpty(gabarito))
                gabarito = "Não há gabarito oficial para esta questão";

            var alternativas = new List<Alternativa>();

            var alternativasQuery = _questaoRepository.ObterAlternativasQuestaoConcurso(QuestaoID);

            if (alternativasQuery.Any())
            {
                alternativas = alternativasQuery.Select(a => new Alternativa()
                {
                    Letra = Convert.ToChar(a.txtLetraAlternativa),
                    Nome = a.txtAlternativa,
                    Id = a.intAlternativaID,
                    Correta = Convert.ToBoolean(a.bitCorreta),
                    CorretaPreliminar = Convert.ToBoolean(a.bitCorretaPreliminar)
                }).ToList();

                if (alternativas.Any(a => a.Correta || a.CorretaPreliminar))
                {
                    if (!alternativas.Any(a => a.Correta))
                        alternativas.FirstOrDefault(a => a.CorretaPreliminar).Correta = true;
                }
                else
                    alternativas.FirstOrDefault().Gabarito = gabarito;
            }
            else
            {
                var primeiraAlternativasQuery = _questaoRepository.ObterPrimeiraAlternativa(QuestaoID);
                if (alternativas.Count == 0)
                    alternativas.Add(new Alternativa
                    {
                        Id = primeiraAlternativasQuery.intAlternativaID,
                        Gabarito = gabarito,
                        Nome = "Resposta: ",
                        Correta = true
                    });
                else
                    alternativas[0].Gabarito = gabarito;
            }

            var exercicio = new ExercicioEntity();

            var queryQuestaoConcurso = _questaoRepository.ObterQuestaoConcurso(QuestaoID);

            if (queryQuestaoConcurso == null)
                return new Questao();

            var ExercicioID = queryQuestaoConcurso.intProvaID ?? 0;

            var respostasDiscursivas = _questaoRepository.ObterRespostasDiscursivas(QuestaoID, ClientID);

            bool Respondida = false;

            if (respostasDiscursivas.Count() > 0)
            {
                foreach (var r in respostasDiscursivas)
                {
                    for (int k = 0; k < alternativas.Count(); k++)
                    {
                        if (r.intDicursivaId == alternativas[k].Id)
                        {
                            alternativas[k].Resposta = r.txtResposta;
                            Respondida = true;
                        }
                    }
                }
            }

            var LstImagensId = new List<String>();
            LstImagensId.AddRange(_imagemRepository.GetImagensQuestaoConcurso(QuestaoID).Select(x => x.ToString()));

            var lstImagensComentario = new Imagens();
            lstImagensComentario.AddRange(_imagemRepository.GetConcursoImagemComentario(QuestaoID));

            var lstVideos = _videoRepository.GetVideoQuestaoConcurso(QuestaoID, ApplicationID, appVersion);
            var videoAddress = string.Empty;
            if (lstVideos.Any())
                videoAddress = lstVideos[0].Url;


            entidadeQuestao = PreencherMidiaQuestao(questao, alternativas, Respondida, LstImagensId, videoAddress);

            if (isAcademico && questao.intEmployeeComentarioID > 0)
            {
                var funcionario = _funcionarioRepository.GetById(questao.intEmployeeComentarioID.Value);
                entidadeQuestao.ProfessorComentario = funcionario.Nome;
            }

            entidadeQuestao.Comentario = HttpUtility.HtmlDecode(entidadeQuestao.Comentario);

            entidadeQuestao.Ordem = questao.intOrder ?? questao.intQuestaoID;

            var marcacao = _questaoRepository.GetAnotacaoQuestaoConcurso(questao.intQuestaoID, ClientID);
            if (marcacao != null)
            {
                var anotacoes = new List<QuestaoAnotacao>
                {
                    marcacao
                };
                entidadeQuestao.Anotacoes = anotacoes;
            }

            var resposta = _questaoRepository.ObterRespostaAlternativa(QuestaoID, ClientID);

            var LetraAlternativa = "";

            if (resposta != null)
                LetraAlternativa = resposta.txtLetraAlternativa;

            foreach (Alternativa a in entidadeQuestao.Alternativas)
                if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                    a.Selecionada = true;

            var correta = _questaoRepository.ObterAlternativaCorreta(QuestaoID, ClientID);

            if (resposta != null)
                entidadeQuestao.Respondida = true;

            if (correta.Count() > 0)
                entidadeQuestao.Correta = true;

            if (questao.bitAnulada == true || questao.bitAnuladaPosRecurso == true)
                entidadeQuestao.Anulada = true;
            else
                entidadeQuestao.Anulada = false;

            entidadeQuestao.Especialidades = _especialidadeRepository.GetByFilters(QuestaoID);
            entidadeQuestao.Titulo = GeraTituloEnunciado(entidadeQuestao, Exercicio.tipoExercicio.CONCURSO).Replace("0", "");
            entidadeQuestao.ImagensComentario = lstImagensComentario;

            entidadeQuestao.Enunciado = isAcademico ? GetEnunciadoComentado(entidadeQuestao, questao, QuestaoID) : questao.txtEnunciado;
           
            var prova = _questaoRepository.ObterProvaConcurso(queryQuestaoConcurso);


            entidadeQuestao.Cabecalho = string.Concat(prova.Ano, " ", prova.Nome);
            entidadeQuestao.Prova = new Prova { TipoProva = new TipoProva { Tipo = prova.Tipo } };
            return entidadeQuestao;
        }

        private string GeraTituloEnunciado(Questao questao, Exercicio.tipoExercicio tipoExercicio)
        {
            string TitEnunciado;

            switch (tipoExercicio)
            {
                case Exercicio.tipoExercicio.SIMULADO:
                    TitEnunciado = string.Format("Questão {0}\n{1}", questao.Ordem, GetEspecialidades(questao));
                    return TitEnunciado;

                case Exercicio.tipoExercicio.CONCURSO:
                    TitEnunciado = string.Format("Questão {0}\n", GetEspecialidades(questao));
                    return TitEnunciado;
            }

            return string.Empty;
        }

        private string GetEspecialidades(Questao questao)
        {
            var texto = questao.Especialidades.Count() > 1 ? "Especialidades: " : "Especialidade: ";
            foreach (var ep in questao.Especialidades)
                texto += string.Concat(ep.Descricao, "/");
            texto = texto.Substring(0, texto.Length - 1);

            return texto;
        }

        private Questao PreencherMidiaQuestao(tblConcursoQuestoes questao, List<Alternativa> alternativas, bool Respondida, List<string> LstImagensId, string videoAddress)
        {
            Media mediaComentario = new Media();
            mediaComentario.Imagens = LstImagensId;
            mediaComentario.Video = videoAddress;
            var entidadeQuestao = new Questao
            {
                Enunciado = questao.txtEnunciado,
                Alternativas = alternativas,
                Comentario = string.IsNullOrEmpty(questao.txtComentario)
                                      ? "Esta questão ainda não possui comentário em texto. Comentário em produção pela equipe acadêmica."
                                      : Utilidades.RemoveHtml(questao.txtComentario),
                MediaComentario = mediaComentario,
                Respondida = Respondida,
                Tipo = Convert.ToBoolean(questao.bitDiscursiva) ? 2 : 1
            };
            return entidadeQuestao;
        }

        public string GetEnunciadoComentado(Questao entidadeQuestao, tblConcursoQuestoes questaoConcurso, int questaoID)
        {
            var enunciadoComentado = Utilidades.FormatarEnunciadoComentario(entidadeQuestao.Enunciado, Utilidades.RemoveHtml(entidadeQuestao.Comentario), questaoID.ToString());
            if(questaoConcurso != null)
                enunciadoComentado += FormataComentarioRecurso(questaoConcurso);

            return enunciadoComentado;
        }

        public string FormataComentarioRecurso(tblConcursoQuestoes questaoConcurso)
        {
            string comentarioRecursos = "";
            const string TITULO_MEDGRUPO = "<br/><br/><b>Parecer MEDGRUPO (Recursos):</b><br/><br/>";
            const string CABE_RECURSO = "<br/><b>CABE RECURSO</b><br/><br/>";
            const string NAO_CABE_RECURSO = "<br/><b>NÃO CABE RECURSO</b><br/><br/>";
            const string TITULO_BANCA = "<br/><br/><b>Parecer da Banca (Recursos):</b><br/>";

            var existeAnaliseProfessor = questaoConcurso.bitComentarioAtivo ?? false;
            var recursoQuestaoConcursoDTO = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO { IdRecursoStatusBanca = questaoConcurso.intStatus_Banca_Recurso },
                Questao = new QuestaoConcursoRecursoDTO { Anulada = questaoConcurso.bitAnulada, AnuladaPosRecurso = questaoConcurso.bitAnuladaPosRecurso }
            };

            var parecerBanca = DecisaoBancaQuestaoCabeRecurso(recursoQuestaoConcursoDTO);

            if (existeAnaliseProfessor)
            {
                comentarioRecursos += string.Concat(TITULO_MEDGRUPO, Utilidades.RemoveHtml(HttpUtility.HtmlDecode(questaoConcurso.txtRecurso)));
                var parecerProfessor = DecisaoAnaliseProfessorQuestaoCabeRecurso(new ForumRecursoDTO { IdAnaliseProfessorStatus = questaoConcurso.ID_CONCURSO_RECURSO_STATUS }) ?? false;
                comentarioRecursos += parecerProfessor ? CABE_RECURSO : NAO_CABE_RECURSO;
            }
            if (parecerBanca.HasValue)
            {
                comentarioRecursos += string.Concat(TITULO_BANCA, Utilidades.RemoveHtml(HttpUtility.HtmlDecode(questaoConcurso.txtComentario_banca_recurso)));
                comentarioRecursos += parecerBanca.Value ? CABE_RECURSO : NAO_CABE_RECURSO;
            }
            return comentarioRecursos;
        }

        private bool? DecisaoAnaliseProfessorQuestaoCabeRecurso(ForumRecursoDTO forumRecurso)
        {
            bool? cabeRecurso = null;
            int status = forumRecurso.IdAnaliseProfessorStatus ?? -1;
            var decisoesValidas = new[] { (int)QuestaoRecurso.StatusQuestao.CabeRecurso, (int)QuestaoRecurso.StatusQuestao.NaoCabeRecurso };

            if (forumRecurso.ExisteAnaliseProfessor && decisoesValidas.Contains(status))
            {
                cabeRecurso = (status == (int)QuestaoRecurso.StatusQuestao.CabeRecurso);
            }
            return cabeRecurso;
        }

        private bool? DecisaoBancaQuestaoCabeRecurso(RecursoQuestaoConcursoDTO questaoRecurso)
        {
            var forumRecurso = questaoRecurso.ForumRecurso;
            var questao = questaoRecurso.Questao;
            bool? cabeRecurso = null;
            var decisoesBanca = new[] { (int)QuestaoRecurso.StatusBancaAvaliadora.Sim, (int)QuestaoRecurso.StatusBancaAvaliadora.Nao };

            if (forumRecurso.IdRecursoStatusBanca.HasValue && decisoesBanca.Contains(forumRecurso.IdRecursoStatusBanca.Value))
            {
                cabeRecurso = forumRecurso.IdRecursoStatusBanca.Value == ((int)QuestaoRecurso.StatusBancaAvaliadora.Sim);
            }
            else if (questao.Anulada || (questao.AnuladaPosRecurso.HasValue && questao.AnuladaPosRecurso.Value))
            {
                cabeRecurso = true;
            }

            return cabeRecurso;
        }

        private void InserirLogGetTipoSimulado(int ClientID, int ApplicationID, int QuestaoID)
        {
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = ClientID,
                IdApp = (Aplicacoes)ApplicationID,
                Tela = Util.Log.MsProLog_Tela.RealizaProvaQuestao,
                Acao = Util.Log.MsProLog_Acao.Abriu,
                Obs = string.Format("Simulado - ID: {0}", QuestaoID)
            });
        }

        public Questao GetTipoSimulado(Int32 QuestaoID, Int32 ClientID, Int32 ApplicationID)
        {
            using(MiniProfiler.Current.Step("Obtendo tipo simulado"))
            {
                InserirLogGetTipoSimulado(ClientID, ApplicationID, QuestaoID);
                var idQuestao = QuestaoID.ToString();
                var tipoUsuario = _funcionarioRepository.GetTipoPerfilUsuario(ClientID);
                var isAcademico = tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador || tipoUsuario == EnumTipoPerfil.Master;

                var questao = _questaoRepository.CacheQuestao(QuestaoID);

                var alternativas = _questaoRepository.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);

                if (Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)))
                {
                    foreach (var indice in alternativas.ToList())
                    {
                        if (indice.Nome.Equals("Resposta: "))
                        {
                            alternativas.Remove(indice);
                        }
                    }
                }

                var questaoSimulado = _questaoRepository.GetQuestao_tblQuestaoSimulado(QuestaoID);
                var simulado = _simuladoRepository.GetSimulado(questaoSimulado.intSimuladoID);

                if (questaoSimulado.intSimuladoID == 0)
                    return new Questao();

                var ExercicioID = questaoSimulado.intSimuladoID;

                var respostasDiscursivas = _questaoRepository.ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO);

                bool Respondida = false;

                if (respostasDiscursivas.Count > 0)
                {
                    foreach (var r in respostasDiscursivas)
                        for (int k = 0; k < alternativas.Count; k++)
                            if (r.intDicursivaId == alternativas[k].Id)
                            {
                                alternativas[k].Resposta = r.txtResposta;
                                Respondida = true;
                            }
                }

                var imagens = _imagemRepository.GetImagensQuestaoSimulado(QuestaoID).Select(x => x.ToString()).ToList();
                var videos = _videoRepository.GetVideoQuestaoSimulado(QuestaoID);

                var videoAddress = "";
                if (videos != null && videos.Count > 0)
                    videoAddress = videos[0].Nome;

                var imagensComentario = _questaoRepository.GetComantarioImagemSimulado(QuestaoID);

                var imgComentario = new Imagens();
                imgComentario.AddRange(imagensComentario);

                var mediaComentario = new Media()
                {
                    Imagens = imagens,
                    Video = videoAddress
                };

                if (isAcademico && questao.intQuestaoConcursoID.HasValue)
                {
                    idQuestao = questao.intQuestaoConcursoID.ToString();
                }

                Questao entidadeQuestao = new Questao
                {
                    Enunciado = isAcademico ? Utilidades.FormatarEnunciadoComentario(questao.txtEnunciado, Utilidades.RemoveHtml(questao.txtComentario), idQuestao) : questao.txtEnunciado,
                    Alternativas = alternativas,
                    Comentario = string.IsNullOrEmpty(questao.txtComentario)
                                        ? "Esta questão ainda não possui comentário em texto. Comentário em produção pela equipe acadêmica."
                                        : questao.txtComentario,
                    MediaComentario = mediaComentario,
                    ImagensComentario = imgComentario,
                    Respondida = Respondida,
                    Tipo = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? 2 : 1
                };

                var simuladoVersaoOrdem = _questaoRepository.GetSimuladoVersao(QuestaoID);

                entidadeQuestao.Ordem = simuladoVersaoOrdem != null
                        ? simuladoVersaoOrdem.intOrdem
                        : entidadeQuestao.Id;

                var isSimuladoOnline = _questaoRepository.GetSimuladoIsOnline(ClientID, QuestaoID);

                var resposta = _questaoRepository.GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline);

                var LetraAlternativa = !string.IsNullOrEmpty(resposta) ? resposta : "";

                entidadeQuestao.RespostaAluno = LetraAlternativa;
                entidadeQuestao.Respondida = !string.IsNullOrEmpty(resposta);


                foreach (var a in entidadeQuestao.Alternativas)
                {
                    if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                    {
                        a.Selecionada = true;
                        break;
                    }
                }


                var isCorreta = _questaoRepository.GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline);

                var marcacao = _questaoRepository.GetQuestaoMarcacao(QuestaoID, ClientID);

                if (marcacao != null)
                {
                    var lqa = new List<QuestaoAnotacao>();
                    var qa = new QuestaoAnotacao();

                    qa.Favorita = marcacao.bitFlagFavorita;
                    qa.Duvida = marcacao.bitFlagEmDuvida;
                    qa.Anotacao = marcacao.txtAnotacao;

                    lqa.Add(qa);

                    entidadeQuestao.Anotacoes = lqa;
                }

                entidadeQuestao.Respondida = !string.IsNullOrEmpty(resposta);

                entidadeQuestao.Cabecalho = simulado.ExercicioName;

                entidadeQuestao.Correta = isCorreta;

                entidadeQuestao.Anulada = questao.bitAnulada;
                entidadeQuestao.Especialidades = _especialidadeRepository.GetByQuestaoSimulado(QuestaoID, ExercicioID);

                entidadeQuestao.Titulo = GeraTituloEnunciado(entidadeQuestao, Exercicio.tipoExercicio.SIMULADO).Replace("0", "");

                return entidadeQuestao;
            }
        }

        public string ObterLetraStatusQuestao(RecursoQuestaoConcursoDTO questao)
        {
            var status = QuestaoRecurso.StatusQuestao.NaoSolicitado;
            var statusQuestao = questao.ForumRecurso.IdAnaliseProfessorStatus ?? -1;
            var statusProfessor = new int[]
            {
                (int)QuestaoRecurso.StatusQuestao.CabeRecurso,
                (int)QuestaoRecurso.StatusQuestao.NaoCabeRecurso
            };
            var statusAnalise = new int[]
            {
                (int)QuestaoRecurso.StatusQuestao.EmAnalise,
                (int)QuestaoRecurso.StatusQuestao.EmAnaliseBloqueada
            };

            var forumPre = questao.ForumRecurso.ForumPreAnalise;

            if (questao.ForumRecurso.BancaCabeRecurso.HasValue && questao.ForumRecurso.BancaCabeRecurso.Value)
            {
                status = QuestaoRecurso.StatusQuestao.AlteradaPelaBanca;
            }
            else if (forumPre.AnaliseProfessorCabeRecurso.HasValue)
            {
                status = (forumPre.AnaliseProfessorCabeRecurso.Value)
                    ? QuestaoRecurso.StatusQuestao.CabeRecurso : QuestaoRecurso.StatusQuestao.NaoCabeRecurso;
            }
            else if (!questao.ForumRecurso.ExisteAnaliseProfessor
                && statusProfessor.Contains(statusQuestao))
            {
                status = QuestaoRecurso.StatusQuestao.EmAnalise;
            }
            else if (statusAnalise.Contains(statusQuestao))
            {
                status = QuestaoRecurso.StatusQuestao.EmAnalise;
            }

            return status.GetDescription();
        }

        public int EnviarComentarioVotoForumPos(int idQuestao, int matricula, string votoAluno, string texto)
        {
            var tipoForum = QuestaoRecurso.TipoForumRecurso.Pos;

            var result = _questaoRepository.EnviarVotoComentarioForum(
                idQuestao, matricula, votoAluno, texto, tipoForum
                );

            if (result > 0)
            {
                _questaoRepository.EnvioEmailComentarioForumPosAsync(
                    idQuestao, matricula, texto, votoAluno == QuestaoRecurso.StatusOpiniao.Favor.GetDescription()
                    );
            }

            return result;
        }

        private bool ForumQuestaoNaoEstaIniciado(int numeroStatus)
        {
            var statusIniciais = new int[]
            {
                (int)QuestaoRecurso.StatusQuestao.NaoSolicitado,
                (int)QuestaoRecurso.StatusQuestao.NaoExiste,
                (int)QuestaoRecurso.StatusQuestao.Invalido
            };

            return statusIniciais.Contains(numeroStatus);
        }

        private void IniciarForumQuestaoConcurso(tblConcursoQuestoes questao)
        {
            questao.ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.EmAnalise;
            _questaoRepository.UpdateQuestaoConcurso(questao);
        }

        public int EnviarComentarioVotoForumPre(int idQuestao, int matricula, string votoAluno, string texto)
        {
            var tipoForum = QuestaoRecurso.TipoForumRecurso.Pre;

            var questao = _questaoRepository.GetQuestaoConcursoById(idQuestao);
            var status = questao.ID_CONCURSO_RECURSO_STATUS ?? 0;

            if (ForumQuestaoNaoEstaIniciado(status) && votoAluno == QuestaoRecurso.StatusOpiniao.Favor.GetDescription())
            {
                if (!_questaoRepository.IsQuestaoProvaRMais(idQuestao))
                {
                    IniciarForumQuestaoConcurso(questao);
                }
            }

            var result = _questaoRepository.EnviarVotoComentarioForum(
                idQuestao, matricula, votoAluno, texto, tipoForum
                );

            if (result > 0)
            {
                _questaoRepository.EnvioEmailComentarioForumPreAsync(
                    idQuestao, matricula, texto, votoAluno == QuestaoRecurso.StatusOpiniao.Favor.GetDescription()
                    );

                if(questao.intEmployeeID.HasValue && questao.intEmployeeID.Value > default(int))
                {
                    NotificarProfessorComentarioForumQuestaoAsync(questao.intEmployeeID.Value, questao);
                }
            }
            return result;
        }

        private Task<int> NotificarProfessorComentarioForumQuestaoAsync(int matricula, tblConcursoQuestoes questao)
        {
            return Task.Factory.StartNew(() => NotificarProfessorComentarioForumQuestao(matricula, questao));
        }

        public string EnviarAnaliseMedgrupoAluno(int idQuestao, int matricula)
        {
            var emailAluno = string.Empty;
            List<Attachment> anexos = null;
            var forum = _questaoRepository.GetQuestaoConcursoRecurso(idQuestao, matricula);
            var imagensAnalise = ObterAnexosAnaliseProfessorRecurso(idQuestao);
            var email = _questaoRepository.GetEmailEnvioAnaliseQuestaoAluno(matricula);
            emailAluno = email;



            if (ConfigurationProvider.Get("Settings:enviaEmailParaAluno") != "SIM")
            {
                email = ConfigurationProvider.Get("Settings:emailDesenv");
            }

            var titulo = string.Format(
                "[MEDGRUPO] {0} / Questão {1} - Análise de recurso",
                forum.Prova.Nome, forum.Questao.Numero
                );

            if (imagensAnalise != null && imagensAnalise.Any())
            {
                anexos = Utilidades.ImageUrlToAttachment(imagensAnalise.Select(a => a.UrlImagem).ToArray());
            }

            Utilidades.SendMailDirect(
                email, forum.ForumRecurso.ForumPreAnalise.TextoAnaliseProfessor,
                titulo, Constants.DEFAULT_EMAIL_PROFILE, anexos: anexos
                );
            return emailAluno;
        }

        private int NotificarProfessorComentarioForumQuestao(int matricula, tblConcursoQuestoes questao)
        {
            var prova = _questaoRepository.ObterProvaConcurso(questao);
            var metadado = new MetadadoNotificacaoDTO
            {
                Acao = "abrir_pagina",
                Pagina = string.Format("provas/prova/{0}/questao/{1}", questao.intProvaID, questao.intQuestaoID)
            };

            var notificacao = _notificacaoRepository.Get((int)Constants.Notificacoes.Recursos.AvisaProfessorComentarioPre);
            var mensagem = new NotificacaoPosEventoDTO
            {
                Matricula = matricula,
                IdNotificacao = notificacao.IdNotificacao,
                Titulo = notificacao.Titulo.Replace("#CONCURSO", prova.Nome + Constants.DASH + prova.UF)
                    .Replace("#NUM", (questao.intOrder ?? 0).ToString()),
                Mensagem = notificacao.Texto,
                Metadados = JsonConvert.SerializeObject(metadado),
                Ativa = false
            };

            var lista = _notificacaoRepository.InserirNotificacoesPosEvento(mensagem);
            return AtualizarMetadadosAtivarNotificacoes(lista, metadado);;
        }

        public bool DesabilitaAcertosQuestaoConcurso(int matricula, int idProva, int idEmployee)
        {
            var qtd = _questaoRepository.DesabilitaAcertosQuestaoConcurso(matricula, idProva, idEmployee);
            return qtd > default(int);
        }

        public ProvaQuestoesConcursoDTO GetQuestoesProvaConcursoComStatus(int idProva, int matricula)
        {
            var questoes = _questaoRepository.GetQuestoesProvaConcurso(idProva);
            DTO.ProvaConcursoDTO prova = null;

            var temQuestao = questoes != null && questoes.Any();
            if (temQuestao)
            {
                var primeiraQuestao = questoes.First();
                prova = primeiraQuestao.Prova;
            }
            else
            {
                prova = _questaoRepository.GetProvaConcurso(idProva);
            }

            if (matricula != default(int))
            {
                prova.AlunoTemEnvioRankAcertos = _questaoRepository.AlunoTemRankAcertos(idProva, matricula);
                prova.AlunoTemRespostaSelecionada = _questaoRepository.AlunoSelecionouAlternativaQuestaoProva(idProva, matricula);
                prova.AlunoViuAvisoComentarioRelevante = _questaoRepository.AlunoViuAvisoComentarioRecurso(matricula);
            }

            prova.ComunicadoHabilitado = matricula == default(int)
                || _questaoRepository.AlunoComunicadoHabilitado(idProva, matricula);

            if ((prova.Bloqueada = IsProvaConcursoBloqueado(prova)))
            {
                questoes.Clear();
            }

            if (!prova.ComunicadoAtivo || prova.DataLimiteComunicado < DateTime.Now)
            {
                prova.Comunicado = string.Empty;
            }

            var live = _questaoRepository.GetLiveProva(idProva);
            if(live != null && live.ExibirLiveRecursos)
            {
                prova.UrlLive = live.UrlLive;
                if(live.DataLive != DateTime.MinValue)
                {
                    prova.DataLive = live.DataLive.ToString("dd/MM");
                    prova.HoraLive =
                        live.DataLive.Minute == default(int)
                            ? string.Format("{0}h", live.DataLive.ToString("HH"))
                            : string.Format("{0}h{1}", live.DataLive.ToString("HH"), live.DataLive.ToString("mm"));
                }
            }

            foreach (var q in questoes)
            {
                q.ForumRecurso.ForumPreAnalise = new ForumPreRecursoDTO
                {
                    AnaliseProfessorCabeRecurso = DecisaoAnaliseProfessorQuestaoCabeRecurso(q.ForumRecurso)
                };

                q.ForumRecurso.BancaCabeRecurso = DecisaoBancaQuestaoCabeRecurso(q);
                q.Questao.StatusQuestao = ObterLetraStatusQuestao(q);
            }

            return new ProvaQuestoesConcursoDTO
            {
                Prova = prova,
                Questoes = questoes.Select(q => q.Questao).OrderBy(q => q.Numero)
            };
        }

        private bool IsProvaConcursoBloqueado(DTO.ProvaConcursoDTO prova)
        {
            return prova.StatusProva == (int)QuestaoRecurso.StatusProva.Bloqueada;
        }

        private int AtualizarMetadadosAtivarNotificacoes(List<tblNotificacaoEvento> notificacoes, MetadadoNotificacaoDTO metadado)
        {
            if (notificacoes != null)
            {
                foreach (var notificacao in notificacoes)
                {
                    metadado.IdNotificacao = notificacao.intNotificacaoEvento;
                    notificacao.Metadados = JsonConvert.SerializeObject(metadado);
                    notificacao.bitAtivo = true;
                }
            }
            return _notificacaoRepository.AtualizarNotificacoesPosEvento(notificacoes);
        }

        public int SelecionarAlternativaQuestaoConcurso(int idQuestao, int matricula, string alternativaSelecionada)
        {
            var favorita = _questaoRepository.GetConcursoQuestoesAlunoFavorita(idQuestao, matricula);
            var alternativas = _questaoRepository.ObterAlternativasComEstatisticaFavorita(idQuestao);
            var letrasGabarito = ObterLetraGabaritoQuestaoAlternativas(alternativas);
            var letraCorreta = (letrasGabarito != null) && letrasGabarito.Contains(alternativaSelecionada);

            var qtd = 0;

            if (favorita == null)
            {
                favorita = new tblConcursoQuestoes_Aluno_Favoritas
                {
                    intQuestaoID = idQuestao,
                    intClientID = matricula,
                    charResposta = alternativaSelecionada,
                    bitResultadoResposta = letraCorreta,
                    dteDate = DateTime.Now,
                    bitDuvida = false,
                    bitVideo = false,
                    bitActive = false
                };
                qtd = _questaoRepository.InserirQuestaoConcursoAlunoFavoritas(favorita);
            }
            else
            {
                favorita.charRespostaNova = alternativaSelecionada;
                qtd = _questaoRepository.UpdateQuestoesConcursoAlunoFavoritas(favorita);
            }

            return qtd;
        }      

        public RecursoQuestaoConcursoDTO GetQuestaoConcursoRecurso(int idQuestao, int matricula)
        {
            var tarefasComentarios = new List<Task>();
            var acessoAluno = matricula != default(int);
            
            var recursoQuestao = _questaoRepository.GetQuestaoConcursoRecurso(idQuestao, matricula);

            tarefasComentarios.Add(PreencherComentarioPreForumRecursoAsync(recursoQuestao, matricula));
            tarefasComentarios.Add(PreencherComentarioPosForumRecursoAsync(recursoQuestao, matricula));

            if (recursoQuestao.Questao.Discursiva)
            {
                var gabarito = _questaoRepository.GetGabaritoDiscursiva(idQuestao);
                recursoQuestao.Questao.GabaritoDiscursiva = gabarito.GabaritoDiscursiva;
                recursoQuestao.Alternativas = new List<AlternativaQuestaoConcursoDTO>();
            }
            else
            {
                recursoQuestao.Alternativas = _questaoRepository.ObterAlternativasComEstatisticaFavorita(idQuestao);
                recursoQuestao.Questao.TotalRespostas = recursoQuestao.Alternativas != null ? recursoQuestao.Alternativas.Sum(a => a.QtdResponderam) : 0;
                recursoQuestao.Questao.GabaritoLetras = ObterLetraGabaritoQuestao(recursoQuestao);

                if (recursoQuestao.Questao.GabaritoLetras != null && recursoQuestao.Questao.GabaritoLetras.Any())
                {
                    recursoQuestao.Questao.GabaritoLetra = recursoQuestao.Questao.GabaritoLetras.First();
                }
            }

            recursoQuestao.Questao.EnunciadoImagensId = _imagemRepository.GetImagensQuestaoConcurso(idQuestao).ToArray();
            recursoQuestao.Questao.GabaritoTipo = ObterLetraStatusGabarito(recursoQuestao);

            if (acessoAluno && !recursoQuestao.Questao.Discursiva)
            {
                var alternativaFavorita = _questaoRepository.GetAlternativaFavoritaQuestaoConcurso(idQuestao, matricula);

                if (alternativaFavorita != null)
                {
                    recursoQuestao.Questao.AlternativaSelecionada = alternativaFavorita.LetraUltimaAlternativaSelecionada ?? alternativaFavorita.LetraAlternativaSelecionada;
                }
            }

            recursoQuestao.ForumRecurso.ForumPreAnalise.AnaliseProfessorCabeRecurso = DecisaoAnaliseProfessorQuestaoCabeRecurso(recursoQuestao.ForumRecurso);
            recursoQuestao.ForumRecurso.BancaCabeRecurso = DecisaoBancaQuestaoCabeRecurso(recursoQuestao);
            
            Task.WaitAll(tarefasComentarios.ToArray());

            recursoQuestao.ForumRecurso.ForumPreAnalise.ForumFechado = DefineForumPreRecursosForumFechado(recursoQuestao);
            recursoQuestao.ForumRecurso.ForumPosAnalise.ForumFechado = DefineForumPosRecursosForumFechado(recursoQuestao);
            
            recursoQuestao.ForumRecurso.ForumPreAnalise.AnexosAnalise = ObterAnexosAnaliseProfessorRecurso(idQuestao);
            recursoQuestao.Questao.StatusQuestao = ObterLetraStatusQuestao(recursoQuestao);

            return recursoQuestao;
        }

        public IEnumerable<ImagemDTO> ObterAnexosAnaliseProfessorRecurso(int idQuestao)
        {
            IEnumerable<ImagemDTO> lista = null;
            var imagensBase = _questaoRepository.ObterImagensComentarioRecurso(idQuestao);

            if (imagensBase != null)
            {
                lista = imagensBase.Where(i => !string.IsNullOrEmpty(i.txtLabel))
                    .Select(i => new ImagemDTO
                    {
                        IdImagem = i.intID,
                        Descricao = i.txtName,
                        NomeArquivo = i.txtLabel,
                        UrlImagem = Constants.LINK_STATIC_ANEXO_ANALISERECURSOS + i.txtLabel
                    });
            }
            return lista;
        }

        public bool DefineForumPreRecursosForumFechado(RecursoQuestaoConcursoDTO recurso)
        {
            var statusRecursoFechado = new[]
            {
                (int)ProvaRecurso.StatusProva.RecursosExpirados,
                (int)ProvaRecurso.StatusProva.BloqueadoParaNovosRecursos
            };
            return statusRecursoFechado.Contains(recurso.Prova.StatusProva)
                || (recurso.ForumRecurso.ExisteAnaliseProfessor || recurso.ForumRecurso.BancaCabeRecurso.HasValue);
        }

        public bool DefineForumPosRecursosForumFechado(RecursoQuestaoConcursoDTO recurso)
        {
            var possuiComentarioFinal = false;
            var comentarios = recurso.ForumRecurso.ForumPosAnalise.Comentarios;
            if (comentarios != null && comentarios.Any())
            {
                possuiComentarioFinal = comentarios.Any(c => c.Professor && c.EncerraForum);
            }
            return possuiComentarioFinal || !recurso.ForumRecurso.ExisteAnaliseProfessor || recurso.ForumRecurso.BancaCabeRecurso.HasValue;
        }

        private Task PreencherComentarioPreForumRecursoAsync(RecursoQuestaoConcursoDTO recursoQuestao, int matricula)
        {
            return Task.Factory.StartNew(() =>
            {
                var comentariosPre = _questaoRepository.ObterComentariosForumConcursoPre(
                    recursoQuestao.Questao.Id, matricula
                    );
                if (comentariosPre != null)
                {
                    PreencherComentarioPreForumRecurso(recursoQuestao, comentariosPre.ToList());
                }
            });
        }

        private void PreencherComentarioPreForumRecurso(RecursoQuestaoConcursoDTO recursoQuestao, List<ForumComentarioDTO> comentariosPre)
        {
            recursoQuestao.ForumRecurso.ForumPreAnalise.Comentarios = comentariosPre;

            if (comentariosPre != null)
            {
                recursoQuestao.ForumRecurso.ForumPreAnalise.QtdCabe = GetQuantidadeUltimoVotoAlunos(comentariosPre, true);
                recursoQuestao.ForumRecurso.ForumPreAnalise.QtdNaocabe = GetQuantidadeUltimoVotoAlunos(comentariosPre, false);

                if (comentariosPre.Any(c => c.Autor))
                {
                    var comentario = comentariosPre.OrderByDescending(c => c.DataInclusao).First(c => c.Autor);
                    var status = comentario.Afirma ? QuestaoRecurso.StatusOpiniao.Favor : QuestaoRecurso.StatusOpiniao.Contra;
                    recursoQuestao.ForumRecurso.ForumPreAnalise.VotoAluno = status.GetDescription();
                }
            }
        }

        private int GetQuantidadeUltimoVotoAlunos(List<ForumComentarioDTO> comentarios, bool afirma)
        {
            var quantidade = default(int);
            if (comentarios != null && comentarios.Any())
            {
                var agrupamento = (from c in comentarios
                                   group c by new { c.Matricula } into g
                                   let maxDate = g.Max(c => c.DataInclusao)
                                   select new { g.Key.Matricula, DataUltimo = maxDate }
                                ).ToList();

                var ultimos = (from c in comentarios
                               from g in agrupamento
                               where c.Matricula == g.Matricula && c.DataInclusao == g.DataUltimo
                               select c).ToList();

                quantidade = ultimos.Count(c => c.Afirma == afirma && !c.Professor);
            }
            return quantidade;
        }

        private Task PreencherComentarioPosForumRecursoAsync(RecursoQuestaoConcursoDTO recursoQuestao, int matricula)
        {
            List<ForumComentarioDTO> comentarios = null;
            return Task.Factory.StartNew(() =>
            {
                var comentariosPos = _questaoRepository.ObterComentariosForumConcursoPos(
                    recursoQuestao.Questao.Id, matricula
                    );

                if(comentariosPos != null)
                {
                    comentarios = comentariosPos.ToList();
                }

                var comentariosProfessor = ObterComentarioForumPosProfessor(recursoQuestao.Questao.Id);

                if(comentariosProfessor != null)
                {
                    if(comentarios != null)
                    {
                        comentarios.AddRange(comentariosProfessor);
                    }
                    else
                    {
                        comentarios = comentariosProfessor.ToList();
                    }
                    comentarios = comentarios.OrderByDescending(c => c.DataInclusao).ToList();
                }

                if (comentarios != null)
                {
                    PreencherComentarioPosForumRecurso(recursoQuestao, comentarios);
                }
            });
        }

        private void PreencherComentarioPosForumRecurso(RecursoQuestaoConcursoDTO recursoQuestao, List<ForumComentarioDTO> comentariosPos)
        {
            recursoQuestao.ForumRecurso.ForumPosAnalise.Comentarios = comentariosPos;

            if (comentariosPos != null)
            {
                recursoQuestao.ForumRecurso.ForumPosAnalise.QtdConcordo = GetQuantidadeUltimoVotoAlunos(comentariosPos, true);
                recursoQuestao.ForumRecurso.ForumPosAnalise.QtdDiscordo = GetQuantidadeUltimoVotoAlunos(comentariosPos, false);

                if (comentariosPos.Any(c => c.Autor))
                {
                    var comentario = comentariosPos.OrderByDescending(c => c.DataInclusao).First(c => c.Autor);
                    var status = comentario.Afirma ? QuestaoRecurso.StatusOpiniao.Favor : QuestaoRecurso.StatusOpiniao.Contra;
                    recursoQuestao.ForumRecurso.ForumPosAnalise.VotoAluno = status.GetDescription();
                }
            }
        }

        private List<ForumComentarioDTO> ObterComentarioForumPosProfessor(int idQuestao)
        {
            var comentarios = _questaoRepository.ObterComentarioForumPosProfessor(idQuestao);
            if(comentarios != null)
            {
                foreach(var c in comentarios)
                {
                    if(c.Matricula == Constants.MATRICULA_ACADEMICORECURSOS)
                    {
                        c.Nome = "Equipe Acadêmica";
                        c.UrlAvatar = Constants.AVATAR_ACADEMICORECURSOS;
                    }
                    else
                    {
                        c.Especialidade = "Professor Medgrupo";
                        c.UrlAvatar = string.Concat(
                            Constants.URLDIRETORIOAVATARPROFESSOR, c.Matricula, ".jpg"
                        );
                    }
                }
            }
            return comentarios != null ? comentarios.ToList() : null;
        }

        private List<string> ObterLetraGabaritoQuestao(RecursoQuestaoConcursoDTO recursoQuestao)
        {
            List<string> letras = null;

            if (recursoQuestao.Alternativas != null && (!recursoQuestao.Questao.AnuladaPosRecurso.HasValue
                || !recursoQuestao.Questao.AnuladaPosRecurso.Value) && !recursoQuestao.Questao.Anulada)
            {
                letras = ObterLetraGabaritoQuestaoAlternativas(recursoQuestao.Alternativas);
            }
            return letras;
        }

        private List<string> ObterLetraGabaritoQuestaoAlternativas(IEnumerable<AlternativaQuestaoConcursoDTO> alternativas)
        {
            List<string> letras = null;

            if (alternativas.Any(a => a.CorretaOficial))
            {
                letras = alternativas.Where(a => a.CorretaOficial).Select(a => a.Letra).ToList();
            }
            else if (alternativas.Any(a => a.CorretaPreliminar))
            {
                letras = alternativas.Where(a => a.CorretaPreliminar).Select(a => a.Letra).ToList();
            }

            return letras;
        }

        private string ObterLetraStatusGabarito(RecursoQuestaoConcursoDTO questao)
        {
            QuestaoRecurso.StatusGabarito? status = null;

            if (questao.Questao.AnuladaPosRecurso.HasValue && questao.Questao.AnuladaPosRecurso.Value)
            {
                status = QuestaoRecurso.StatusGabarito.AnuladaAposRecurso;
            }
            else if (questao.Questao.Anulada)
            {
                status = QuestaoRecurso.StatusGabarito.Anulada;
            }
            else if (questao.Alternativas.Any(a => a.CorretaOficial))
            {
                status = QuestaoRecurso.StatusGabarito.GabaritoOficial;
            }
            else if (questao.Alternativas.Any(a => a.CorretaPreliminar))
            {
                status = QuestaoRecurso.StatusGabarito.GabaritoPreliminar;
            }

            return status.HasValue ? status.Value.GetDescription() : null;
        }

        public List<Questao> GetRelatorioQuestoesPublicadas(string siglaConcurso = "TODOS", int anoQuestao = 0, int anoQuestaoPublicada = 0)
        {
            var questoes = _questaoRepository.ListConcursoQuestao(siglaConcurso, anoQuestao, anoQuestaoPublicada)
                .OrderBy(a => a.SiglaConcurso)
                .ThenBy(b => b.OrdemQuestao);

            var lstQuestao = new List<Questao>();

            foreach (var item in questoes)
            {
                var q = new Questao();
                q.Concurso = new Concurso { Ano = item.AnoConcurso, Sigla = item.SiglaConcurso };
                q.Prova = new Prova { Nome = item.NomeProva };
                q.Id = item.IdQuestao;
                q.Ordem = item.OrdemQuestao;
                q.Ano = item.AnoQuestao;
                q.DataQuestao = Utilidades.DateTimeToUnixTimestamp(item.DataQuestao ?? DateTime.MinValue);

                q.VideoQuestao = new Video { Duracao = _videoRepository.GetDuracao(item.IdQuestao) };

                q.ProtocoladaPara = _questaoRepository.GetProtocoladaPara(item.IdQuestao, item.AnoQuestao);
                q.PrimeiroComentario = _questaoRepository.GetPrimeiroComentario(item.IdQuestao);
                q.UltimoComentario = _questaoRepository.GetUltimoComentario(item.IdQuestao);

                lstQuestao.Add(q);
            }

            return lstQuestao;
        }

        public Questao GetQuestaoTipoApostila(int QuestaoID, int ClientID, int ApplicationID)
        {
            using(MiniProfiler.Current.Step("Obter questão tipo apostila"))
            {
                var tipoUsuario = _funcionarioRepository.GetTipoPerfilUsuario(ClientID);
                var isAcademico = tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador || tipoUsuario == EnumTipoPerfil.Master;

                var questao = _questaoRepository.GetTipoApostila(QuestaoID, ClientID, ApplicationID);
                var questaoConcurso = _questaoRepository.ObterQuestaoConcurso(QuestaoID);

                questao.Enunciado = isAcademico ? GetEnunciadoComentado(questao, questaoConcurso, QuestaoID) : questao.Enunciado;

                return questao;
            }
        }

        public QuestaoSimuladoAgendadoDTO GetQuestaoSimuladoAgendado(int QuestaoID, int ClientID, int ExercicioID, int ExercicioHistoricoID = 0)
        {
            using(MiniProfiler.Current.Step("Obtendo questao simulado agendado"))
            {
                var tipoUsuario = _funcionarioRepository.GetTipoPerfilUsuario(ClientID);
                var isAcademico = tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador || tipoUsuario == EnumTipoPerfil.Master;

                var questao = _questaoRepository.CacheQuestao(QuestaoID);
                var alternativas = _questaoRepository.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);
                var simulado = _simuladoRepository.GetSimulado(ExercicioID);

                if (Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)))
                {
                    foreach (var indice in alternativas.ToList())
                    {
                        if (indice.Nome.Equals("Resposta: "))
                        {
                            alternativas.Remove(indice);
                        }
                    }
                }

                var respostasDiscursivas = _questaoRepository.GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID);

                bool Respondida = VerificarRespostaDiscursivaSimuladoAgendado(respostasDiscursivas, alternativas);
                var imagens = _imagemRepository.GetImagensQuestaoSimulado(QuestaoID).Select(x => x.ToString()).ToList();

                var mediaComentario = new Media()
                {
                    Imagens = imagens,
                    Video = ""
                };

                var simuladoVersaoOrdem = _questaoRepository.GetSimuladoVersao(QuestaoID);

                Questao entidadeQuestao = new Questao
                {
                    Enunciado = isAcademico ? Utilidades.FormatarEnunciadoComentario(questao.txtEnunciado, Utilidades.RemoveHtml(questao.txtComentario), questao.intQuestaoID.ToString()) : questao.txtEnunciado,
                    Alternativas = alternativas,
                    Respondida = Respondida,
                    Tipo = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? (int)Questao.tipoQuestao.DISCURSIVA : (int)Questao.tipoQuestao.OBJETIVA,
                    Cabecalho = simulado.ExercicioName,
                    Anulada = questao.bitAnulada,
                    MediaComentario = mediaComentario
                };

                entidadeQuestao.Ordem = simuladoVersaoOrdem != null
                        ? simuladoVersaoOrdem.intOrdem
                        : entidadeQuestao.Id;

                var resposta = _questaoRepository.GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID);

                var LetraAlternativa = !string.IsNullOrEmpty(resposta) ? resposta : "";

                entidadeQuestao.RespostaAluno = LetraAlternativa;
                entidadeQuestao.Respondida = !string.IsNullOrEmpty(resposta) || Respondida ? true : false;

                foreach (var a in entidadeQuestao.Alternativas)
                {
                    if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                    {
                        a.Selecionada = true;
                        break;
                    }
                }

                entidadeQuestao.Especialidades = _especialidadeRepository.GetByQuestaoSimulado(QuestaoID, ExercicioID);

                entidadeQuestao.Titulo = GeraTituloEnunciado(entidadeQuestao, Exercicio.tipoExercicio.SIMULADO).Replace("0", "");

                var listaAlternativas = new List<AlternativaSimualdoAgendadoDTO>();

                foreach (var alt in entidadeQuestao.Alternativas)
                {
                    listaAlternativas.Add(new AlternativaSimualdoAgendadoDTO()
                    {
                        Id = alt.Id,
                        Nome = alt.Nome,
                        Letra = alt.Letra.ToString(),
                        Editar = alt.Editar,  
                        Selecionada = alt.Selecionada
                    });
                }

                var listaEspecialidades = new List<EspecialidadeDTO>();
                foreach (var esp in entidadeQuestao.Especialidades)
                {
                    listaEspecialidades.Add(new EspecialidadeDTO()
                    {
                        ID = esp.Id,
                        Nome = esp.Descricao
                    });
                }

                var entidadeQuestaoSimulado = new QuestaoSimuladoAgendadoDTO()
                {
                    Id = entidadeQuestao.Id,
                    Alternativas = listaAlternativas,
                    Anulada = entidadeQuestao.Anulada,
                    Cabecalho = entidadeQuestao.Cabecalho,
                    Enunciado = entidadeQuestao.Enunciado,
                    Especialidades = listaEspecialidades,
                    ExercicioTipoID = entidadeQuestao.ExercicioTipoID,
                    MediaComentario = entidadeQuestao.MediaComentario,
                    Ordem = entidadeQuestao.Ordem,
                    Respondida = entidadeQuestao.Respondida,
                    RespostaAluno = entidadeQuestao.RespostaAluno,
                    Tipo = entidadeQuestao.Tipo,
                    Titulo = entidadeQuestao.Titulo
                };


                return entidadeQuestaoSimulado;
            }
        }

        private bool VerificarRespostaDiscursivaSimuladoAgendado(List<CartaoRespostaDiscursivaDTO> respostas, List<Alternativa> alternativas)
        {
            var respondida = false;
            if (respostas.Count > 0)
            {
                foreach (var r in respostas)
                    for (int k = 0; k < alternativas.Count; k++)
                        if (r.intDicursivaId == alternativas[k].Id)
                        {
                            alternativas[k].Resposta = r.txtResposta;
                            respondida = true;
                        }
            }

            return respondida;
        }
    }
}