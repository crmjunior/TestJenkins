using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Newtonsoft.Json;

namespace MedCore_DataAccess.Business
{
    public class NotificacaoRecursosBusiness
    {
        private readonly INotificacaoData _notificacaoRepository;
        private readonly IExercicioData _exercicioRepository;
        private readonly IQuestaoData _questaoRepository;
        private readonly IConcursoData _concursoRepository;

        public NotificacaoRecursosBusiness(INotificacaoData notificacaoRepository, IExercicioData exercicioRepository,
            IQuestaoData questaoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
            _exercicioRepository = exercicioRepository;
            _questaoRepository = questaoRepository;
        }

        public NotificacaoRecursosBusiness(INotificacaoData notificacaoRepository, IExercicioData exercicioRepository,
            IQuestaoData questaoRepository, IConcursoData concursoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
            _exercicioRepository = exercicioRepository;
            _questaoRepository = questaoRepository;
            _concursoRepository = concursoRepository;
        }

        public int CriarNotificacoesMudancaStatusProva(int idProva, ProvaRecurso.StatusProva status)
        {
            var qtd = default(int);

            if (StatusProvaNotificaAluno(status))
            {
                qtd = CriarNotificacoesAlunosFavoritaramProva(
                        idProva, Constants.Notificacoes.Recursos.StatusProvaFavoritos,
                        "provas/prova/{0}", status
                    );
            }
            return qtd;
        }

        private bool StatusQuestaoPrecisaAprovacaoCoordenacao(QuestaoRecurso.StatusQuestao status)
        {
            var statusAprovar = new QuestaoRecurso.StatusQuestao[]
            {
                QuestaoRecurso.StatusQuestao.CabeRecurso,
                QuestaoRecurso.StatusQuestao.NaoCabeRecurso,
            };
            return statusAprovar.Contains(status);
        }

        public bool StatusQuestaoNotificaAluno(QuestaoRecurso.StatusQuestao status, bool questaoRMais)
        {
            var statusNotificados = new QuestaoRecurso.StatusQuestao[]
            {
                QuestaoRecurso.StatusQuestao.CabeRecurso,
                QuestaoRecurso.StatusQuestao.AlteradaPelaBanca
            };
            var statusNotificadosApenasRMais = new QuestaoRecurso.StatusQuestao[]
            {
                QuestaoRecurso.StatusQuestao.NaoCabeRecurso,
                QuestaoRecurso.StatusQuestao.EmAnalise
            };

            return statusNotificados.Contains(status) ||
                (questaoRMais && statusNotificadosApenasRMais.Contains(status));
        }

        private bool StatusProvaNotificaAluno(ProvaRecurso.StatusProva status)
        {
            var statusNotificados = new ProvaRecurso.StatusProva[]
            {
                ProvaRecurso.StatusProva.RecursosEmAnalise,
                ProvaRecurso.StatusProva.RecursosPróximos
            };
            return statusNotificados.Contains(status);
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

        public List<NotificacaoInternaDTO> GetNotificacoesAluno(int matricula)
        {
            var notificacoes = _notificacaoRepository.GetNotificacoesAlunoPosEvento(matricula, Aplicacoes.Recursos)
                ?? new List<NotificacaoPosEventoDTO>();

            return notificacoes.Select(n => new NotificacaoInternaDTO
            {
                IdNotificacao = n.IdNotificacao,
                Titulo = n.Titulo,
                Subtitulo = n.Mensagem,
                Parametros = n.Metadados,
                Data = n.Data,
                Lida = n.Lida
            }).OrderByDescending(a => a.Data).ToList();
        }

        private string TrocarParametrosMensagemQuestaoProva(string mensagem, string concurso, string status, string numero)
        {
            return mensagem.Replace("#CONCURSO", concurso).Replace("#STATUS", status).Replace("#NUM", numero);
        }

        public int NotificarMudancaStatusQuestao(int idQuestao, QuestaoRecurso.StatusQuestao status)
        {
            var qtd = default(int);
            if (!StatusQuestaoPrecisaAprovacaoCoordenacao(status))
            {
                qtd = CriarNotificacoesMudancaStatusQuestao(idQuestao, status);
            }
            else if(_questaoRepository.IsQuestaoProvaRMais(idQuestao))
            {
                qtd = CriarNotificacoesMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.EmAnalise);
            }
            return qtd;
        }

        public int CriarNotificacoesMudancaStatusQuestao(int idQuestao, QuestaoRecurso.StatusQuestao status)
        {
            var qtd = default(int);
            var questaoRMais = _questaoRepository.IsQuestaoProvaRMais(idQuestao);

            if (!StatusQuestaoNotificaAluno(status, questaoRMais))
            {
                return qtd;
            }

            var questao = _questaoRepository.GetQuestaoConcursoById(idQuestao);
            ProvaAlunosFavoritoDTO alunos = null;

            if(status == QuestaoRecurso.StatusQuestao.NaoCabeRecurso)
            {
                alunos = _questaoRepository.GetAlunosVotaramForumPre(idQuestao);
            }
            else
            {
                alunos = _exercicioRepository.GetAlunosFavoritaramProva(questao.intProvaID ?? 0);
            }

            if (alunos.MatriculasFavoritaram != null && alunos.MatriculasFavoritaram.Any())
            {
                var metadado = new MetadadoNotificacaoDTO
                {
                    Acao = "abrir_pagina",
                    Pagina = string.Format("provas/prova/{0}/questao/{1}", questao.intProvaID ?? 0, idQuestao)
                };

                var notificacao = _notificacaoRepository.Get((int)Constants.Notificacoes.Recursos.StatusQuestaoFavoritos);
                var notificacoesEnviar = new List<NotificacaoPosEventoDTO>();
                foreach (var matricula in alunos.MatriculasFavoritaram)
                {
                    notificacoesEnviar.Add(new NotificacaoPosEventoDTO
                    {
                        Matricula = matricula,
                        IdNotificacao = notificacao.IdNotificacao,
                        Titulo = TrocarParametrosMensagemQuestaoProva(
                            notificacao.Titulo, alunos.Prova.Nome, string.Empty, (questao.intOrder ?? 0).ToString()
                            ),
                        Mensagem = TrocarParametrosMensagemQuestaoProva(
                            notificacao.Texto, alunos.Prova.Nome, ObterMensagemStatusQuestao(status), (questao.intOrder ?? 0).ToString()
                            ),
                        Metadados = JsonConvert.SerializeObject(metadado),
                        Ativa = false
                    });
                }
                var lista = _notificacaoRepository.InserirNotificacoesPosEvento(notificacoesEnviar.ToArray());
                qtd = AtualizarMetadadosAtivarNotificacoes(lista, metadado);
            }
            return qtd;
        }

        private string ObterMensagemStatusQuestao(QuestaoRecurso.StatusQuestao status)
        {
            var statusInt = (int)status;
            var statusMensagem = ((Constants.Messages.StatusQuestaoNotificacao)statusInt);
            return statusMensagem.GetDescription();
        }

        public int CriarNotificacoesAprovacaoAnaliseQuestao(int idQuestao, bool aprovada)
        {
            var qtd = default(int);
            if (aprovada)
            {
                var questao = _questaoRepository.GetQuestaoConcursoById(idQuestao);
                var status = questao.ID_CONCURSO_RECURSO_STATUS ?? 0;
                if(status == (int)QuestaoRecurso.StatusQuestao.CabeRecurso || status == (int)QuestaoRecurso.StatusQuestao.NaoCabeRecurso)
                {
                    qtd = CriarNotificacoesMudancaStatusQuestao(idQuestao, (QuestaoRecurso.StatusQuestao)status);
                }
            }
            return qtd;
        }

        public int NotificarMudancaRankingAcertos(int idProva, bool liberado)
        {
            var qtd = default(int);
            if (liberado)
            {
                qtd = CriarNotificacoesAlunosFavoritaramProva(
                    idProva, Constants.Notificacoes.Recursos.RankingAcertosLiberado,
                    "provas/prova/{0}/forum/ranking"
                    );
            }
            return qtd;
        }

        public void NotificarQuestaoAlteradoBanca(int idQuestao, int status)
        {
            var statusNotifica = new[]
            {
                (int)QuestaoRecurso.StatusBancaAvaliadora.Sim
            };

            var statusAtual = _questaoRepository.ObterStatusRecursoBanca(idQuestao);
            if(statusNotifica.Contains(status) && statusAtual != status)
            {
                CriarNotificacoesMudancaStatusQuestaoAsync(idQuestao, 
                    QuestaoRecurso.StatusQuestao.AlteradaPelaBanca
                    );
            }
        }

        public Task<int> CriarNotificacoesMudancaStatusQuestaoAsync(int idQuestao, QuestaoRecurso.StatusQuestao status)
        {
            return Task.Factory.StartNew(() => CriarNotificacoesMudancaStatusQuestao(idQuestao, status));
        }

        public int NotificaMudancaComunicadoLiberado(int idProva, bool liberado)
        {
            var qtd = default(int);
            if (liberado)
            {
                qtd = CriarNotificacoesAlunosFavoritaramProva(
                    idProva, Constants.Notificacoes.Recursos.ComunicadoLiberado,
                    "provas/prova/{0}"
                    );
            }
            return qtd;
        }
        
        private int CriarNotificacoesAlunosFavoritaramProva(int idProva, Constants.Notificacoes.Recursos tipo, string pagina,
            ProvaRecurso.StatusProva status = ProvaRecurso.StatusProva.Inexistente)
        {
            var qtd = default(int);
            var alunos = _exercicioRepository.GetAlunosFavoritaramProva(idProva);

            if (alunos.MatriculasFavoritaram != null && alunos.MatriculasFavoritaram.Any())
            {
                var metadado = new MetadadoNotificacaoDTO
                {
                    Acao = "abrir_pagina",
                    Pagina = string.Format(pagina, idProva)
                };

                var notificacao = _notificacaoRepository.Get((int)tipo);
                var notificacoesEnviar = new List<NotificacaoPosEventoDTO>();
                foreach (var matricula in alunos.MatriculasFavoritaram)
                {
                    notificacoesEnviar.Add(new NotificacaoPosEventoDTO
                    {
                        Matricula = matricula,
                        IdNotificacao = notificacao.IdNotificacao,
                        Titulo = TrocarParametrosMensagemQuestaoProva(
                            notificacao.Titulo, alunos.Prova.Nome, status.GetDescription(), string.Empty
                            ),
                        Mensagem = TrocarParametrosMensagemQuestaoProva(
                            notificacao.Texto, alunos.Prova.Nome, status.GetDescription(), string.Empty
                            ),
                        Metadados = JsonConvert.SerializeObject(metadado),
                        Ativa = false
                    });
                }
                var lista = _notificacaoRepository.InserirNotificacoesPosEvento(notificacoesEnviar.ToArray());
                qtd = AtualizarMetadadosAtivarNotificacoes(lista, metadado);
            }
            return qtd;
        }

        public int NotificaConclusaoEstadoQuestoesProva(int idProva, Constants.Notificacoes.Recursos tipo)
        {
            var qtd = default(int);
            if (StatusConclusaoNotificaExternamente(tipo))
            {
                qtd = CriarNotificacoesAlunosFavoritaramProva(idProva, tipo, "provas/prova/{0}");
                _concursoRepository.InserirConfiguracaoProvaAluno(new tblProvaAlunoConfiguracoes
                {
                    intContactID = Utilidades.UsuarioSistema,
                    dteAtualizacao = DateTime.Now,
                    dteCriacao = DateTime.Now,
                    intProvaID = idProva,
                    bitComunicadoAtivo = true,
                    intConfigNotificacao = (int)tipo
                });
            }
            return qtd;
        }

        public bool StatusConclusaoNotificaExternamente(Constants.Notificacoes.Recursos tipo)
        {
            var statusNotifica = new[]
            {
                Constants.Notificacoes.Recursos.ConclusaoAnaliseAcademica,
                Constants.Notificacoes.Recursos.ConclusaoAnaliseBancaQuestoes
            };
            return statusNotifica.Contains(tipo);
        }
    }
}