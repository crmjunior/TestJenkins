using System;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using System.Linq;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;
using System.Threading;

namespace MedCore_DataAccess.Business
{
    public class ExercicioBusiness
    {
        private IExercicioData _exercicioRepository;
        private IRankingSimuladoData _rankingSimuladoRepository;
        private ISimuladoData _simuladoRepository;
        private ICartaoRespostaData _cartaoRespostaRepository;
        private IQuestaoData _questaoRepository;
        private IAulaEntityData _aulaRepository;
        private IClienteEntity _clienteRepository;

        private ICartaoRespostaBusiness _cartaoRespostaBusiness;

        public ExercicioBusiness(IExercicioData exercicioRepository, IRankingSimuladoData rankingSimuladoRepository, ISimuladoData simuladoRepository, ICartaoRespostaData cartaoRespostaRepository, IQuestaoData questaoRepository, IAulaEntityData aulaRepository)
        {
            _exercicioRepository = exercicioRepository;
            _rankingSimuladoRepository = rankingSimuladoRepository;
            _simuladoRepository = simuladoRepository;
            _cartaoRespostaRepository = cartaoRespostaRepository;
            _questaoRepository = questaoRepository;
            _aulaRepository = aulaRepository;

            _cartaoRespostaBusiness = new CartaoRespostaBusiness(_cartaoRespostaRepository, _questaoRepository, _aulaRepository);
        }

        public ExercicioBusiness(IExercicioData exercicioRepository, IRankingSimuladoData rankingSimuladoRepository, ISimuladoData simuladoRepository, 
            ICartaoRespostaData cartaoRespostaRepository, IQuestaoData questaoRepository, IAulaEntityData aulaRepository, IClienteEntity clienteRepository)
        {
            _exercicioRepository = exercicioRepository;
            _rankingSimuladoRepository = rankingSimuladoRepository;
            _simuladoRepository = simuladoRepository;
            _cartaoRespostaRepository = cartaoRespostaRepository;
            _questaoRepository = questaoRepository;
            _aulaRepository = aulaRepository;
            _clienteRepository = clienteRepository;

            _cartaoRespostaBusiness = new CartaoRespostaBusiness(_cartaoRespostaRepository, _questaoRepository, _aulaRepository);
        }
        public int SetFinalizaExercicio(Exercicio exercicio, bool finalizarPorTempo = false)
        {
            try
            {
                using(MiniProfiler.Current.Step("Finalizando exercício"))
                {
                    var exercicioHistorico = _exercicioRepository.ObterExercicio(exercicio.HistoricoId);

                    if (exercicioHistorico != null)
                    {
                        if (finalizarPorTempo)
                        {
                            if (exercicioHistorico.bitRealizadoOnline != null && (bool)exercicioHistorico.bitRealizadoOnline)
                            {
                                var simuladoAtual = _simuladoRepository.GetSimulado(exercicioHistorico.intExercicioID);
                                var tempo = exercicioHistorico.dteDateInicio.AddMinutes(simuladoAtual.Duracao);
                                if (tempo >= DateTime.Now)
                                {
                                    return 0;
                                }
                            }
                        }

                        var naoPossuiRealizacaoOnline = !_exercicioRepository.AlunoJaRealizouSimuladoOnline(exercicioHistorico.intClientID, exercicioHistorico.intExercicioID);

                        if (exercicioHistorico.intExercicioTipo == 1 && naoPossuiRealizacaoOnline)
                        {
                            var simulado = _exercicioRepository.ObterSimulado(exercicioHistorico.intExercicioID);

                            if (simulado != null && simulado.bitOnline)
                            {
                                var alunoExcecao = _exercicioRepository.ObterSimuladoAlunoExcecao(exercicioHistorico.intClientID, exercicioHistorico.intExercicioID);

                                var dataInicioSimulado = alunoExcecao != null ? alunoExcecao.dteDataHoraInicioWEB : simulado.dteDataHoraInicioWEB;
                                var dataFimSimulado = alunoExcecao != null ? alunoExcecao.dteDataHoraTerminoWEB : simulado.dteDataHoraTerminoWEB;

                                if (exercicioHistorico.dteDateInicio >= dataInicioSimulado && exercicioHistorico.dteDateInicio <= dataFimSimulado)
                                {
                                    _exercicioRepository.RegistrarSimuladoOnline(exercicio.HistoricoId);

                                    var questoes = _exercicioRepository.ObterQuestoesOnline(exercicioHistorico.intHistoricoExercicioID);

                                    _exercicioRepository.InserirQuestoesSimulado(questoes, exercicioHistorico.intClientID);

                                    _exercicioRepository.ReplicarSimuladoOnlineTabelasMGE(exercicioHistorico.intClientID, exercicioHistorico.intExercicioID);
                                }
                            }
                        }

                        _exercicioRepository.FinalizarExercicio(exercicio);
                    }
                    else
                        return 0;

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public Exercicio GetModalSimuladoOnline(int idSimulado, int matricula, int idAplicacao)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo modal simulado online"))
                {
                    var exerc = new Exercicio();
                    var cr = _cartaoRespostaBusiness.GetCartaoRespostaSimuladoAgendado(matricula, idSimulado, 0);

                    var simu = _simuladoRepository.GetSimulado(idSimulado);
                    exerc.Duracao = simu.Duracao;
                    exerc.ExercicioName = simu.ExercicioName;
                    exerc.Online = 1;

                    exerc.Questoes = new List<Questao>();
                    foreach (var questao in cr.Questoes)
                        exerc.Questoes.Add
                            (new Questao()
                            {
                                Id = questao.Id,
                                Enunciado = questao.Enunciado,
                                ExercicioTipoID = questao.ExercicioTipoID,
                                MediaComentario = questao.MediaComentario,
                                Respondida = questao.Respondida,
                                RespostaAluno = questao.RespostaAluno,
                                Tipo = questao.Tipo

                            });

                    return exerc;
                }
            }
            catch
            {
                throw;
            }
        }

        public SimuladosEmAndamentoDTO GetSimuladosIniciados(int matricula, int idAplicacao)
        {

            var listaSimuladoEmAndamento = new List<SimuladosEmAndamentoDTO>();

            using(MiniProfiler.Current.Step("Obtendo simulados iniciados"))
            {
                var exerciciosAbertos = _exercicioRepository.ObterExerciciosEmAndamento(matricula, idAplicacao);

                for (int i = 0; i < exerciciosAbertos.Count; i++)
                {
                    var exercicio = exerciciosAbertos[i];
                    var simulado = _exercicioRepository.ObterSimulado(exercicio.intExercicioID);
                    if (simulado != null)
                    {
                        var fimSimulado = exercicio.dteDateInicio.AddMinutes(simulado.intDuracaoSimulado);
                        if ((exercicio.intTipoProva == (int)TipoProvaEnum.ModoOnline) && (fimSimulado < DateTime.Now))
                        {
                            SetFinalizaExercicio(new Exercicio() { HistoricoId = exercicio.intHistoricoExercicioID }, true);
                        }
                        else
                        {
                            listaSimuladoEmAndamento.Add(new SimuladosEmAndamentoDTO()
                            {
                                IdHistorico = exercicio.intHistoricoExercicioID,
                                TipoProva = exercicio.intTipoProva == (int)TipoProvaEnum.ModoOnline ? "Simulado Agendado" : "Modo Prova",
                                NomeSimulado = simulado.txtSimuladoName,
                                IdSimulado = simulado.intSimuladoID,
                                TipoSimulado = simulado.intTipoSimuladoID
                            });
                        }
                    }
                }

                return (listaSimuladoEmAndamento.Count > 0) ? listaSimuladoEmAndamento[0] : null;
            }
        }

        public Simulado GetSimuladoModoProvaConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion = "0.0.0")
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo configuração simulado modo prova"))
                {
                    var simu = _exercicioRepository.ObterSimulado(idSimulado);
                    var exercicioHistorico = _exercicioRepository.ObterUltimoExercicioSimuladoModoProva(matricula, idSimulado);
                    var jaInicouModoProva = exercicioHistorico != null;

                    var versaoMinimaComparacao = Utilidades.VersaoMinimaImpressaoSimulado(idAplicacao);

                    var simulado = new Simulado
                    {
                        IdExercicio = idSimulado,
                        Nome = simu.txtSimuladoDescription,
                        QtdQuestoes = Convert.ToInt32(simu.intQtdQuestoes),
                        QtdQuestoesDiscursivas = Convert.ToInt32(simu.intQtdQuestoesCasoClinico),
                        PermissaoProva = new PermissaoProva
                        {
                            Cronometro = 1,
                            Impressao = Utilidades.VersaoMenorOuIgual(appVersion, versaoMinimaComparacao) ? 0 : 1
                        }
                    };

                    if (jaInicouModoProva)
                    {
                        var tempoRestante = exercicioHistorico.dteDateInicio.AddMinutes(simu.intDuracaoSimulado) - DateTime.Now;
                        var tempoRestanteSeg = exercicioHistorico.dteDateInicio.AddSeconds(simu.intDuracaoSimulado * 60) - DateTime.Now;

                        this.ConfigurarDuracaoSimulado(simulado, Convert.ToInt32(tempoRestante.TotalMinutes), Convert.ToInt32(tempoRestante.TotalSeconds));

                        simulado.CartoesResposta = new CartoesResposta { HistoricoId = exercicioHistorico.intHistoricoExercicioID };
                    }
                    else
                    {
                        this.ConfigurarDuracaoSimulado(simulado, simu.intDuracaoSimulado, simu.intDuracaoSimulado * 60);

                        var historico = _exercicioRepository.InserirExercicioSimulado(idSimulado, matricula, idAplicacao, TipoProvaEnum.ModoProva);

                        simulado.CartoesResposta = new CartoesResposta { HistoricoId = historico.intHistoricoExercicioID };
                    }

                    return simulado;
                }

            }
            catch
            {
                throw;
            }
        }

        private void ConfigurarDuracaoSimulado(Simulado simulado, int duracao, int duracaoEmSegundos)
        {
            simulado.DuracaoEmSeg = duracaoEmSegundos;
            simulado.Duracao = duracao;
        }

        public List<ComboSimuladoDTO> GetComboSimuladosRealizados(int matricula, int simuladoId, int idAplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo combo simulados realizados"))
            {
                var retorno = new List<ComboSimuladoDTO>();

                var exercicios = _exercicioRepository.GetComboSimuladosRealizados(matricula, simuladoId, idAplicacao).OrderByDescending(x => x.dteDateInicio).ToList();

                var simuladoOnline = exercicios.Where(x => (TipoProvaEnum)x.intTipoProva == TipoProvaEnum.ModoOnline).FirstOrDefault();

                if (simuladoOnline != null)
                {
                    retorno.Add(new ComboSimuladoDTO()
                    {
                        DataRealizacao = simuladoOnline.dteDateInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                        HistoricoID = simuladoOnline.intHistoricoExercicioID,
                        TipoProva = "Simulado Agendado"
                    });
                }

                exercicios.ForEach(x => {
                    if ((TipoProvaEnum)x.intTipoProva != TipoProvaEnum.ModoOnline)
                    {
                        retorno.Add(new ComboSimuladoDTO()
                        {
                            DataRealizacao = x.dteDateInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                            HistoricoID = x.intHistoricoExercicioID,
                            TipoProva = "Modo Prova"
                        });
                    }
                });


                return retorno;
            }
        }

        public RankingSimuladoAluno GetResultadoRankingModoProva(int matricula, int idSimulado, int idHistorico, int idAplicacao, string especialidade = "", string unidades = "", string localidade = "")
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo resultado ranking modo prova"))
                {
                    var result = new RankingSimuladoAluno();

                    result.EstatisticasAlunoRankingModoProva = _rankingSimuladoRepository.GetEstatisticaAlunoSimuladoModoProva(matricula, idSimulado, idHistorico);
                    result.EstatisticasAlunoRankingEstudo = _rankingSimuladoRepository.GetEstatisticaAlunoSimulado(matricula, idSimulado, false);

                    var local = string.IsNullOrEmpty(localidade) ? string.Empty : (localidade.IndexOf('(') > 0 ? localidade.Substring(0, localidade.IndexOf('(')).Trim() : localidade);

                    this.ObterPosicaoAlunoNoRankingModoProva(result, idSimulado, idHistorico, especialidade, unidades, localidade);

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private RankingSimuladoAluno ObterPosicaoAlunoNoRankingModoProva(RankingSimuladoAluno rankingAluno, int idSimulado, int idHistorico, string especialidade, string unidades, string localidade)
        {
            var addAlunoAoRanking = 1;
            var ranking = _exercicioRepository.ObterRankingPorSimulado(idSimulado, especialidade, unidades, localidade);
            var qtdQuestoes = _exercicioRepository.ObterQuantidadeQuestoesSimuladoOnline(idSimulado);
            var posicaoRankingAluno = ranking.Where(x => x.Acertos > rankingAluno.EstatisticasAlunoRankingModoProva.Acertos).Count() + addAlunoAoRanking;

            if (ranking.Count == 0)
                rankingAluno.Posicao = "1º";
            else
                rankingAluno.Posicao = posicaoRankingAluno.ToString() + "º";

            rankingAluno.Nota = ((10d / (double)qtdQuestoes) * rankingAluno.EstatisticasAlunoRankingModoProva.Acertos).ToString("0.#");
            rankingAluno.DataRealizacao = _exercicioRepository.ObterExercicio(idHistorico).dteDateInicio;
            rankingAluno.QuantidadeParticipantes = ranking.Count > 0 ? (ranking.Count + addAlunoAoRanking) : 1;

            return rankingAluno;
        }

        public Exercicio GetSimuladoOnlineCorrente()
        {
            var simulado = _exercicioRepository.GetSimuladoOnlineCorrente();
            return simulado;
        }

        // public int EnviarComentarioForumProva(int idProva, string texto, int matricula, string ip)
        // {
        //     var idEspecialidade = _exercicioRepository.GetIdEspecialidade(matricula);

        //     var comentario = new ForumProva.Comentario
        //     {
        //         ComentarioTexto = texto,
        //         Especialidade = new Especialidade { Id = idEspecialidade }
        //     };

        //     var enviou = _exercicioRepository.SetComentarioForumProva(new ForumProva
        //     {
        //         Matricula = matricula,
        //         Ip = ip,
        //         Prova = new Exercicio { ID = idProva },
        //         Comentarios = new List<ForumProva.Comentario> { comentario }
        //     });

        //     if (enviou > 0)
        //     {
        //         EnviarEmailComentarioForumProvaAsync(idProva, texto, matricula);
        //     }

        //     return enviou;
        // }

        // private void EnviarEmailComentarioForumProvaAsync(int idProva, string texto, int matricula)
        // {
        //     var thread = new Thread(() => EnviarEmailComentarioForumProva(idProva, texto, matricula));
        //     thread.Start();
        // }

        // private void EnviarEmailComentarioForumProva(int idProva, string texto, int matricula)
        // {
        //     throw new NotImplementedException();
            // var enviarAluno = WebConfigurationManager.AppSettings["enviaEmailParaAluno"].ToString();
            // var emailDesenv = WebConfigurationManager.AppSettings["emailDesenv"].ToString();

            // var prova = _questaoRepository.GetProvaConcurso(idProva);

            // if(prova != null)
            // {
            //     var siglaUf = prova.Nome.Split('-').Select(a => a.Trim());

            //     var avatar = _clienteRepository.GetClienteAvatar(matricula);
            //     var aluno = _clienteRepository.GetDadosBasicos(matricula);

            //     var corpoEmail = Utilidades.CorpoEmailComentarioForumProva(
            //             siglaUf.ElementAt(0), siglaUf.ElementAt(1), prova.Ano ?? 0, avatar.Caminho,
            //             aluno.Nome, matricula, prova.NomeCompleto, texto
            //         );

            //     var tituloEmail = string.Format(
            //             "Forum MEDGRUPO {0} - {1} {2}",
            //             siglaUf.ElementAt(0), siglaUf.ElementAt(1), prova.Ano ?? 0
            //         );

            //     var destinatarios = new List<string>();
            //     if (enviarAluno == "SIM")
            //     {
            //         destinatarios.Add(Constants.EMAIL_RECURSOS);
            //     }
            //     else
            //     {
            //         tituloEmail = "[Homologação] " + tituloEmail;
            //         destinatarios.Add(Constants.EMAIL_RECURSOS);
            //         destinatarios.Add(emailDesenv);
            //     }

            //     Utilidades.SendMailDirect(string.Join(",", destinatarios), corpoEmail, tituloEmail, "medmaster");
            // }
        // }
    }
}