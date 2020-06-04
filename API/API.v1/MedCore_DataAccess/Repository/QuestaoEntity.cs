using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Enums;
using System.Data.SqlClient;
using MedCore_DataAccess.Util;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Medgrupo.DataAccessEntity;
using System.Web;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using StackExchange.Profiling;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace MedCore_DataAccess.Repository
{
    public class QuestaoEntity : IQuestaoData
    {
        private string TitEnunciado { get; set; }

        public ILogOperacoesConcursoData LogOperacoesConcurso { get; set; }

        public QuestaoEntity()
        {
            LogOperacoesConcurso = new LogOperacoesConcursoEntity();
        }

        public QuestaoAnotacao GetAnotacaoQuestaoConcurso(int QuestaoID, int ClientId)
        {
            using(MiniProfiler.Current.Step("Obtendo as anotações referentes a uma questão e um cliente"))
            {
                using (var ctx = new AcademicoContext())
                {
                    List<int> ListaQuestaoId;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        ListaQuestaoId = (from qs in ctxMatDir.tblConcursoQuestoes
                                        where qs.intQuestaoID == QuestaoID
                                        select qs.intQuestaoID).ToList();
                    }

                    var marcacao = (from m in ctx.tblQuestao_Marcacao
                                    where m.intClientID == ClientId
                                    && m.intQuestaoID == QuestaoID
                                    && ListaQuestaoId.Contains(m.intQuestaoID)
                                    orderby m.dtAnotacao descending
                                    select new QuestaoAnotacao
                                    {
                                        Anotacao = m.txtAnotacao,
                                        Favorita = m.bitFlagFavorita

                                    }).FirstOrDefault();

                    return marcacao;
            }

            }
        }

        public Dictionary<string, string> GetEstatistica(int idQuestao, int idTipoExercicio)
        {
            var ctx = new DesenvContext();

            if (idTipoExercicio == Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA)) idTipoExercicio = Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO);

            IList<tblQuestao_Estatistica> estatisticasQuestao;
            using(MiniProfiler.Current.Step("Obtendo estastisticas de uma questão"))
            {
                estatisticasQuestao = (
                from Qe in ctx.tblQuestao_Estatistica
                where Qe.intQuestaoID == idQuestao && Qe.intExercicioTipo == idTipoExercicio
                select Qe
                ).ToList();
            }
            IQueryable<tblConcursoQuestoes> questao;
            using(MiniProfiler.Current.Step("Obtendo dados da questão"))
            {
                questao = (
                    from Q in ctx.tblConcursoQuestoes
                    where Q.intQuestaoID == idQuestao
                    select Q
                    );
            }

            const string respostaInvalida = "-1";
            var respostaCorreta = respostaInvalida;
            if (questao.Any() && !(questao.First().bitAnulada) && estatisticasQuestao.Where(x => x.bitCorreta == true).Count() == 1)
            {
                respostaCorreta = estatisticasQuestao.Where(x => x.bitCorreta == true).First().txtLetraAlternativa;
            }

            using(MiniProfiler.Current.Step("Obtendo lista de Alternativas e liga a suas estastíticas"))
            {
                var alternativas = new List<string>()
                {
                    "A", "B", "C", "D", "E", "F"
                };

                estatisticasQuestao = estatisticasQuestao.Select(a => new tblQuestao_Estatistica { txtLetraAlternativa = a.txtLetraAlternativa, dblPercent = a.dblPercent }).Distinct().ToList();

                var result = (from a in alternativas
                          join l in estatisticasQuestao on a equals l.txtLetraAlternativa.ToString().Trim() into g
                          from aleft in g.DefaultIfEmpty()
                          select new { txtLetraAlternativa = a, dblPercent = (aleft == null ? 0 : aleft.dblPercent) }
                          );

                var dicAlternativas = new Dictionary<string, string>();

                foreach (var estatistica in result)
                {
                    dicAlternativas.Add(estatistica.txtLetraAlternativa, Convert.ToString(Math.Round((Decimal)estatistica.dblPercent * 100, 2)));
                }

                dicAlternativas.Add("Correta", respostaCorreta);

                return dicAlternativas;
            }
        }

        public int GetExercicioHistorico(int intSimuladoId, int matricula)
        {
            throw new NotImplementedException();
        }

        public ExercicioHistoricoDTO GetExercicioHistorico(int ExercicioHistoricoID)
        {
            using(MiniProfiler.Current.Step("Obtendo histórico de exercícios"))
            {
                using (var ctx = new AcademicoContext())
                {
                    return (from eh in ctx.tblExercicio_Historico
                            where eh.intHistoricoExercicioID == ExercicioHistoricoID
                            select new ExercicioHistoricoDTO()
                            {
                                intHistoricoExercicioID = eh.intHistoricoExercicioID,
                                bitPresencial = eh.bitPresencial,
                                bitRanqueado = eh.bitRanqueado,
                                bitRealizadoOnline = eh.bitRealizadoOnline,
                                dteDateFim = eh.dteDateFim,
                                dteDateInicio = eh.dteDateInicio,
                                intApplicationID = eh.intApplicationID,
                                intClientID = eh.intClientID,
                                intExercicioID = eh.intExercicioID,
                                intTempoExcedido = eh.intTempoExcedido,
                                intExercicioTipo = eh.intExercicioTipo,
                                intTipoProva = eh.intTipoProva,
                                intVersaoID = eh.intVersaoID
                            }).FirstOrDefault();
                }
            }
        }

        public MarcacoesObjetivaDTO GetUltimaMarcacaobyIntExercicioHistoricoID(int QuestaoID, int ExercicioTipoId, int Matricula, int? TipoProva, int IntExercicioHistoricoID)
        {
            using (var ctx = new AcademicoContext())
            {
                MarcacoesObjetivaDTO ultimaMarcacao;
                if (!TipoProva.HasValue || ((TipoProvaEnum)TipoProva == TipoProvaEnum.ModoEstudo))
                    using(MiniProfiler.Current.Step("Obtendo última marcação - Histórico dos execícios"))
                    {
                        ultimaMarcacao = (from o in ctx.tblCartaoResposta_objetiva
                                      where o.intQuestaoID == QuestaoID
                                            && o.intExercicioTipoId == ExercicioTipoId
                                            && o.intClientID == Matricula
                                            && o.intHistoricoExercicioID == IntExercicioHistoricoID
                                      select new MarcacoesObjetivaDTO
                                      {
                                          ID = o.intID,
                                          Resposta = o.txtLetraAlternativa
                                      }).FirstOrDefault();
                    }
                else
                { 
                    using(MiniProfiler.Current.Step("Obtendo última marcação  - Histórico dos execícios"))
                    {
                        var exercicio = (from h in ctx.tblExercicio_Historico
                                        where  h.dteDateFim == null
                                        && h.intClientID == Matricula
                                        && h.intTipoProva == (int)TipoProvaEnum.ModoProva
                                        && h.intHistoricoExercicioID == IntExercicioHistoricoID
                                        select h).Any();

                        ultimaMarcacao = (from o in ctx.tblCartaoResposta_objetiva
                                            where o.intQuestaoID == QuestaoID
                                                && o.intExercicioTipoId == ExercicioTipoId
                                                && o.intHistoricoExercicioID == IntExercicioHistoricoID
                                                && exercicio
                                            select new MarcacoesObjetivaDTO
                                            {
                                                ID = o.intID,
                                                Resposta = o.txtLetraAlternativa
                                            }).FirstOrDefault();
                    }
                }

                return ultimaMarcacao;
            }
        }

        public MarcacoesObjetivaDTO GetUltimaMarcacao_SimuladoOnline(int QuestaoID, int ExercicioTipoId, int Matricula)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Obtendo última marcação  - Simulado Online"))
                {
                    return (from h in ctx.tblExercicio_Historico
                            join o in ctx.tblCartaoResposta_objetiva_Simulado_Online on h.intHistoricoExercicioID equals o.intHistoricoExercicioID
                            where o.intQuestaoID == QuestaoID
                                && o.intExercicioTipoId == ExercicioTipoId
                                && h.intClientID == Matricula
                            select new MarcacoesObjetivaDTO
                            {
                                ID = o.intID,
                                Resposta = o.txtLetraAlternativa,
                            })
                        .FirstOrDefault();
                }
            }
        }

        public List<tblConcursoQuestoes_Alternativas> ObterAlternativaCorreta(int QuestaoID, int ClientID)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    List<tblConcursoQuestoes_Alternativas> correta;

                    using(MiniProfiler.Current.Step("Obtendo alternativas cartão resposta  - Obter alternativa correta"))
                    {
                        var alternativasCartaoResposta = (from c in ctx.tblCartaoResposta_objetiva
                                                        where
                                                            c.intQuestaoID == QuestaoID
                                                            && c.intClientID == ClientID
                                                        select new { c.txtLetraAlternativa }).ToList();
                    

                        var listaConcursoQuestoesAlternativas = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                                where
                                                                    a.intQuestaoID == QuestaoID
                                                                select a).ToList();

                        correta = (from a in listaConcursoQuestoesAlternativas
                                    join c in alternativasCartaoResposta on a.txtLetraAlternativa equals c.txtLetraAlternativa
                                    select a).ToList();
                    }

                    return correta;
                }
            }
        }

        public List<tblConcursoQuestoes_Alternativas> ObterAlternativasQuestaoConcurso(int QuestaoID)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo alternativas de questao de concurso"))
                {
                    var alternativasQuery = (from a in ctx.tblConcursoQuestoes_Alternativas
                                            where a.intQuestaoID == QuestaoID
                                            select a).ToList();
                    return alternativasQuery;
                }
            }
        }

        public tblConcursoQuestoes_Alternativas ObterPrimeiraAlternativa(int QuestaoID)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo primeira alternativas"))
                {
                    var primeiraAlternativasQuery = ctx.tblConcursoQuestoes_Alternativas.Where(d => d.intQuestaoID == QuestaoID).OrderBy(d => d.intAlternativaID).FirstOrDefault();
                    return primeiraAlternativasQuery;
                }
            }
        }

        public ProvaConcursoDTO ObterProvaConcurso(tblConcursoQuestoes queryQuestaoConcurso)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo prova de concurso"))
                {
                    var prova = (from p in ctx.tblConcurso_Provas
                                join c in ctx.tblConcurso on p.ID_CONCURSO equals c.ID_CONCURSO
                                join t in ctx.tblConcurso_Provas_Tipos on p.intProvaTipoID equals t.intProvaTipoID
                                where p.intProvaID == queryQuestaoConcurso.intProvaID
                                select new DTO.ProvaConcursoDTO { Ano = c.VL_ANO_CONCURSO, Nome = c.SG_CONCURSO, Tipo = p.txtName, UF = c.CD_UF }).FirstOrDefault();
                                
                    return prova;
                }
            }
        }

        public tblConcursoQuestoes ObterQuestaoConcurso(int QuestaoID)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo questao de concurso"))
                {
                    var questao = (from q in ctx.tblConcursoQuestoes where q.intQuestaoID == QuestaoID select q).FirstOrDefault();
                    return questao;
                }    
            }
        }

        public CartaoRespostaObjetivaDTO ObterRespostaAlternativa(int QuestaoID, int ClientID)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    IList<tblCartaoResposta_objetiva> cartaoResposta;
                    using(MiniProfiler.Current.Step("Obtendo cartão resposta objetiva"))
                    {
                        var respostas = (from c in ctx.tblCartaoResposta_objetiva
                                        where c.intClientID == ClientID && c.intQuestaoID == QuestaoID
                                        group c by c.intQuestaoID into g
                                        select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                        cartaoResposta = (from r in respostas
                                            join c in ctx.tblCartaoResposta_objetiva on r.ID equals c.intID
                                            select c).ToList();
                    }

                    using(MiniProfiler.Current.Step("Obtendo resposta alternativa"))
                    {
                        Func<tblCartaoResposta_objetiva, CartaoRespostaObjetivaDTO> seletor = c => (
                            from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                            where a.intQuestaoID == c.intQuestaoID
                            select new CartaoRespostaObjetivaDTO()
                            {
                                intID = c.intID,
                                intQuestaoID = c.intQuestaoID,
                                intHistoricoExercicioID = c.intHistoricoExercicioID,
                                txtLetraAlternativa = c.txtLetraAlternativa,
                                guidQuestao = c.guidQuestao,
                                intExercicioTipoId = c.intExercicioTipoId,
                                dteCadastro = c.dteCadastro
                            }).FirstOrDefault();

                        return cartaoResposta.Select(x => seletor(x)).FirstOrDefault();
                    }
                }
            }
        }

        public string ObterRespostaQuestaoConcurso(int QuestaoID)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo resposta questão de concurso"))
                {
                    var gabarito = (from q in ctx.tblConcursoQuestoes_Alternativas where q.intQuestaoID == QuestaoID && !string.IsNullOrEmpty(q.txtResposta) select q.txtResposta).FirstOrDefault();
                    return gabarito;
                }
            }
        }

        public List<CartaoRespostaDiscursivaDTO> ObterRespostasDiscursivas(int QuestaoID, int ClientID, int TipoExercicio = 2)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Obtendo resposta discursiva"))
                {
                    var respostasDiscursivas = (from r in ctx.tblCartaoResposta_Discursiva
                                                join h in ctx.tblExercicio_Historico on r.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                where r.intQuestaoDiscursivaID == QuestaoID
                                                    && h.intClientID == ClientID
                                                    && r.intExercicioTipoId == TipoExercicio
                                                orderby r.dteCadastro
                                                select new CartaoRespostaDiscursivaDTO()
                                                {
                                                    intID = r.intID,
                                                    intQuestaoDiscursivaID = r.intQuestaoDiscursivaID,
                                                    intHistoricoExercicioID = r.intHistoricoExercicioID,
                                                    txtResposta = r.txtResposta,
                                                    intExercicioTipoId = r.intExercicioTipoId,
                                                    intDicursivaId = r.intDicursivaId,
                                                    dteCadastro = r.dteCadastro,
                                                    dblNota = r.dblNota
                                                }).ToList();
                    return respostasDiscursivas;
                }
            }
        }

        public int SetRespostaObjetiva(RespostaObjetivaPost resp, MarcacoesObjetivaDTO ultimaMarcacao)
        {
            using (var ctx = new AcademicoContext())
            {
                if (ultimaMarcacao != null)
                {
                    using(MiniProfiler.Current.Step("Removendo questão objetiva - SetRespostaObjetiva"))
                    {
                        var questaoObjetiva = ctx.tblCartaoResposta_objetiva
                            .FirstOrDefault(o => o.intID == ultimaMarcacao.ID);

                        ctx.tblCartaoResposta_objetiva.Remove(questaoObjetiva);
                    }
                }

                using(MiniProfiler.Current.Step("Adicionando resposta objectiva - SetRespostaObjetiva"))
                {
                    ctx.tblCartaoResposta_objetiva.Add(new tblCartaoResposta_objetiva
                    {
                        intExercicioTipoId = resp.ExercicioTipoId,
                        intHistoricoExercicioID = resp.HistoricoId,
                        intQuestaoID = resp.QuestaoId,
                        txtLetraAlternativa = resp.Alterantiva,
                        dteCadastro = DateTime.Now,
                        intClientID = resp.Matricula
                    });
                }
                ctx.SaveChanges();
            }
            return 1;
        }

        public int SetRespostaObjetivaSimuladoOnline(RespostaObjetivaPost resp, MarcacoesObjetivaDTO ultimaMarcacao)
        {
            using (var ctx = new AcademicoContext())
            {
                if (ultimaMarcacao != null)
                {
                    using(MiniProfiler.Current.Step("Removendo questão objetiva - SetRespostaObjetivaSimuladoOnline"))
                    {
                        var questaoObjetiva = ctx.tblCartaoResposta_objetiva_Simulado_Online
                            .FirstOrDefault(o => o.intID == ultimaMarcacao.ID);

                        ctx.tblCartaoResposta_objetiva_Simulado_Online.Remove(questaoObjetiva);
                    }

                }

                using(MiniProfiler.Current.Step("Adicionando resposta objectiva simulado online"))
                {
                    ctx.tblCartaoResposta_objetiva_Simulado_Online.Add(new tblCartaoResposta_objetiva_Simulado_Online
                    {
                        intExercicioTipoId = resp.ExercicioTipoId,
                        intHistoricoExercicioID = resp.HistoricoId,
                        intQuestaoID = resp.QuestaoId,
                        txtLetraAlternativa = resp.Alterantiva,
                        dteCadastro = DateTime.Now
                    });
                }
                ctx.SaveChanges();
            }
            return 1;
        }

        public ForumQuestaoRecurso GetForumQuestaoRecurso(int idQuestao, int matricula, bool visitente = false)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var forum = new ForumQuestaoRecurso();
                    var forumPre = new ForumQuestaoRecurso.Pre();
                    var forumPos = new ForumQuestaoRecurso.Pos();

                    using(MiniProfiler.Current.Step("Obtendo alternativa selecionada - GetForumQuestaoRecurso"))
                    {
                        var alternaSelecionada = (from f in ctx.tblConcursoQuestoes_Aluno_Favoritas
                                                where f.intClientID == matricula && f.intQuestaoID == idQuestao
                                                select f).OrderByDescending(a => a.dteDate);

                        if (alternaSelecionada.Any())
                            forum.AlternativaSelecionada = alternaSelecionada.FirstOrDefault().charResposta.ToString();
                    }

                    using(MiniProfiler.Current.Step("Obter recurso questão do forum - GetForumQuestaoRecurso"))
                    {
                        var questao = (
                            from cq in ctx.tblConcursoQuestoes
                            from p in ctx.tblPersons.Where(p => p.intContactID == cq.intEmployeeID).DefaultIfEmpty()
                            where cq.intQuestaoID == idQuestao
                            select new
                            {
                                idProva = cq.intProvaID,
                                pre_analiseAcademica = cq.txtRecurso,
                                pre_analiseAtiva = cq.bitComentarioAtivo,
                                pre_nomeProfessor = p.txtName,
                                pre_idProfessor = (int?)p.intContactID,
                                pre_statusAnaliseAcademica = cq.ID_CONCURSO_RECURSO_STATUS,
                                pos_statusBanca = cq.intStatus_Banca_Recurso,
                                pos_veredito = cq.txtComentario_banca_recurso,
                            }).FirstOrDefault();



                        #region FORUM PRÉ ============================================================================================

                        var parametros = new SqlParameter[] {
                                                                new SqlParameter("intQuestaoID", idQuestao),
                                                                new SqlParameter("intTipo", 1),
                                                                new SqlParameter("intContactId", matricula)
                                                            };
                        var ds = new DBQuery().ExecuteStoredProcedure("msp_RecursosListaComentarios", parametros);

                        if (!visitente)
                            if (Convert.ToBoolean(questao.pre_analiseAtiva))
                                if (questao.pre_analiseAcademica != null)
                                {
                                    var imagens = ctx.tblConcursoQuestoes_recursosComentario_Imagens.Where(q => q.intQuestao == idQuestao).ToList();

                                    forumPre.Analises = new List<AnaliseAcademica> {
                                                                                    new AnaliseAcademica {
                                                                                                                Texto = Utilidades.RemoveHtmlEMantemNegritoEPuloDeLinha(questao.pre_analiseAcademica),
                                                                                                                UrlAvatarProfessor = String.Concat("http://arearestrita.medgrupo.com.br/_static/images/professores/", questao.pre_idProfessor, ".jpg"),
                                                                                                                NomeProfessor = GetRecursosNomeProfessorFormatado(questao.pre_nomeProfessor.Trim()),
                                                                                                                UrlImagens = imagens.Count > 0 ? imagens.Select(q => string.Concat("http://static.medgrupo.com.br/static/imagens/RecursosComentario/", q.txtLabel)).ToList() : null
                                                                                                            }
                                                                                };
                                }

                        int? status = null;
                        if (!visitente)
                        {
                            if (questao.pre_statusAnaliseAcademica == 5 || questao.pre_statusAnaliseAcademica == 7)
                                status = 2;

                            if (questao.pre_statusAnaliseAcademica == 8 || questao.pre_statusAnaliseAcademica == 0)
                                status = 3;


                            if (Convert.ToBoolean(questao.pre_analiseAtiva))
                            {
                                switch (questao.pre_statusAnaliseAcademica)
                                {
                                    case 3:
                                        status = 0;
                                        break;

                                    case 4:
                                        status = 1;
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (questao.pre_statusAnaliseAcademica)
                                {
                                    case 3:
                                    case 4:
                                        status = 2;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            if (status != null)
                                forumPre.StatusAnaliseAcademica = status;
                        }
                        else
                            status = -1;

                        forumPre.TextoStatusAnaliseAcademica = GetTextoAnaliseAcademicaRecurso(Convert.ToInt32(status));

                        var statusProva = ctx.tblConcurso_Provas.Where(p => p.intProvaID == questao.idProva).FirstOrDefault().ID_CONCURSO_RECURSO_STATUS;
                        var statusAnalise = Convert.ToInt32(questao.pre_statusAnaliseAcademica);

                        if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts) || visitente)
                            forumPre.ForumFechado = true;
                        else if (
                            statusProva != 1 ||
                            (new[] { 3, 4 }.Contains(statusAnalise) && Convert.ToBoolean(questao.pre_analiseAtiva)) ||
                            statusAnalise == 7

                        )
                            forumPre.ForumFechado = true;

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            var queryAluno = from coment in ds.Tables[0].AsEnumerable()
                                            where coment.Field<int>("intContactID") == matricula
                                                && coment.Field<int>("intTipo") == 1
                                            orderby coment.Field<DateTime>("data1")
                                            select coment;

                            forumPre.AlunoOpinou = false;
                            if (queryAluno.Any())
                                forumPre.AlunoOpinouCabe = queryAluno.LastOrDefault().Field<string>("bitOpiniao").Trim() == "Cabe Recurso:";
                            forumPre.QtdCabeRecuso = Convert.ToInt32(ds.Tables[0].Rows[0]["totalSIM"]);
                            forumPre.QtdNaoCabeRecuso = Convert.ToInt32(ds.Tables[0].Rows[0]["totalNAO"]);
                            forumPre.Comentarios = new List<ForumQuestaoRecurso.ForumComentarioRecurso>();

                            foreach (DataRow dRow in ds.Tables[0].Rows)
                                forumPre.Comentarios.Add(
                                    new ForumQuestaoRecurso.ForumComentarioRecurso
                                    {
                                        Nome = dRow["txtName"].ToString(),
                                        Uf = dRow["txtUF"].ToString(),
                                        Opiniao = dRow["bitOpiniao"].ToString().Replace(":", string.Empty),
                                        ComentarioTexto = Utilidades.RemoveHtmlEMantemNegritoEPuloDeLinha(dRow["comentario"].ToString()),
                                        DataAmigavel = dRow["dataAmigavel"].ToString(),
                                        DataDecorrida = Utilidades.GetTempoDecorridoPorExtenso(Convert.ToDateTime(dRow["data1"].ToString())),
                                        Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() },
                                        Matricula = Convert.ToInt32(dRow["intContactID"].ToString()),
                                        IsProfessor = Convert.ToBoolean(Convert.ToInt32(dRow["bitProfessor"].ToString()))
                                    });
                            var avatares = new List<Avatar>();
                            var listaMatriculasComentario = forumPre.Comentarios.Select(o => o.Matricula).Distinct().ToList();
                            foreach (var m in listaMatriculasComentario)
                                avatares.Add(new ClienteEntity().GetClienteAvatar(m));

                            foreach (var avatar in avatares)
                            {
                                if (!string.IsNullOrEmpty(avatar.Caminho))
                                    forumPre.Comentarios.Where(c => c.Matricula == avatar.Matricula).ToList()
                                        .ForEach(c => c.Avatar = avatar.Caminho);
                                else
                                    forumPre.Comentarios.Where(c => c.Matricula == avatar.Matricula).ToList()
                                        .ForEach(c => c.Avatar = avatar.CaminhoImagemPadrao);
                            }
                        }
                        #endregion


                        if (!visitente)
                        {
                            #region FORUM PÓS ============================================================================================
                            forumPos.ForumFechado = true;
                            var idProva = ctx.tblConcursoQuestoes.Where(q => q.intQuestaoID == idQuestao).FirstOrDefault().intProvaID;
                            var temQuestaoAlterada = ctx.tblConcursoQuestoes.Where(q => q.intProvaID == idProva).Where(q => new[] { 11, 12 }.Contains(q.intStatus_Banca_Recurso ?? 0)).Any();


                            bool? statusBanca = null;
                            if (questao.pos_statusBanca == 11)
                                statusBanca = true;
                            else if (questao.pos_statusBanca == 12 || temQuestaoAlterada)
                                statusBanca = false;

                            forumPos.RecursoConcedidoPelaBanca = statusBanca;
                            forumPos.VereditoBanca = questao.pos_veredito;

                            var textoRecursoConcedido = string.Empty;
                            if (statusBanca != null)
                                textoRecursoConcedido = Convert.ToBoolean(statusBanca) ? "DEFERIDO" : "INDEFERIDO";
                            forumPos.Resultado = textoRecursoConcedido;

                            if (forumPre.ForumFechado)
                            {

                                var dsComentFinal = new DBQuery().ExecuteStoredProcedure("msp_Recursos_ComentariosFinaisAcad", new SqlParameter[] { new SqlParameter("intQuestaoID", idQuestao) });
                                if (dsComentFinal.Tables[0].Rows.Count > 0)
                                    forumPos.ComentarioFinal = Utilidades.RemoveHtmlEMantemNegritoEPuloDeLinha(dsComentFinal.Tables[0].Rows[0]["comentario"].ToString());

                                if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts) || visitente)
                                    forumPos.ForumFechado = true;
                                else
                                    forumPos.ForumFechado = (
                                        forumPos.ComentarioFinal != null
                                        || new[] { 11, 12 }.Contains(Convert.ToInt32(questao.pos_statusBanca))
                                        || !Convert.ToBoolean(questao.pre_analiseAtiva)
                                        || temQuestaoAlterada
                                    );

                                var parametros2 = new SqlParameter[] {
                                                                        new SqlParameter("intQuestaoID", idQuestao),
                                                                        new SqlParameter("intTipo", 2),
                                                                        new SqlParameter("intContactId", matricula)
                                                                    };

                                var ds2 = new DBQuery().ExecuteStoredProcedure("msp_RecursosListaComentarios", parametros2);

                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    var queryAluno = from coment in ds2.Tables[0].AsEnumerable()
                                                    where coment.Field<int>("intContactID") == matricula
                                                        && coment.Field<int>("intTipo") == 2
                                                    orderby coment.Field<DateTime>("data1")
                                                    select coment;
                                    forumPos.AlunoOpinou = false;
                                    if (queryAluno.Any())
                                        forumPos.AlunoOpinouConcordo = queryAluno.LastOrDefault().Field<string>("bitOpiniao").Trim() == "Concordo:";
                                    forumPos.QtdConcordo = Convert.ToInt32(ds2.Tables[0].Rows[0]["totalSIM"]);
                                    forumPos.QtdNaoConcordo = Convert.ToInt32(ds2.Tables[0].Rows[0]["totalNAO"]);
                                    forumPos.Comentarios = new List<ForumQuestaoRecurso.ForumComentarioRecurso>();
                                    foreach (DataRow dRow in ds2.Tables[0].Rows)
                                        forumPos.Comentarios.Add(
                                            new ForumQuestaoRecurso.ForumComentarioRecurso
                                            {
                                                Nome = dRow["txtName"].ToString(),
                                                Uf = dRow["txtUF"].ToString(),
                                                Opiniao = dRow["bitOpiniao"].ToString().Replace(":", string.Empty),
                                                ComentarioTexto = Utilidades.RemoveHtmlEMantemNegritoEPuloDeLinha(dRow["comentario"].ToString()),
                                                DataAmigavel = dRow["dataAmigavel"].ToString(),
                                                DataDecorrida = Utilidades.GetTempoDecorridoPorExtenso(Convert.ToDateTime(dRow["data1"].ToString())),
                                                Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() },
                                                Matricula = Convert.ToInt32(dRow["intContactID"].ToString()),
                                                IsProfessor = Convert.ToBoolean(Convert.ToInt32(dRow["bitProfessor"].ToString()))
                                            });
                                    var avatares = new List<Avatar>();
                                    var listaMatriculasComentarioPos = forumPos.Comentarios.Select(o => o.Matricula).Distinct().ToList();
                                    foreach (var m in listaMatriculasComentarioPos)
                                        avatares.Add(new ClienteEntity().GetClienteAvatar(m));

                                    foreach (var avatar in avatares)
                                    {
                                        if (!string.IsNullOrEmpty(avatar.Caminho))
                                            forumPos.Comentarios.Where(c => c.Matricula == avatar.Matricula).ToList()
                                                .ForEach(c => c.Avatar = avatar.Caminho);
                                        else
                                            forumPos.Comentarios.Where(c => c.Matricula == avatar.Matricula).ToList()
                                                .ForEach(c => c.Avatar = avatar.CaminhoImagemPadrao);
                                    }

                                }

                                #endregion
                            }

                        }
                        else
                        {
                            if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts) || visitente)
                                forumPos.ForumFechado = true;
                        }



                        forum.ForumPreQuestao = forumPre;
                        forum.ForumPosQuestao = forumPos;

                        forum.RecursoLido = RecursoLido(idQuestao, matricula, forum, ctx);


                        return forum;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private string GetTextoAnaliseAcademicaRecurso(int status)
        {
            var analise = string.Empty;
            switch (status)
            {
                case 0:
                    analise = "Não";
                    break;

                case 1:
                    analise = "Sim";
                    break;

                case 2:
                    analise = "Em Análise";
                    break;

                case 3:
                    analise = string.Empty;
                    break;

                default:
                    break;
            }
            return analise;
        }

        private bool RecursoLido(int idQuestao, int matricula, ForumQuestaoRecurso forum, DesenvContext ctx)
        {
            using(MiniProfiler.Current.Step("Verificando se recurso foi lido"))
            {
                var recursoLog = ctx.tblLogRecursoAluno.FirstOrDefault(x => x.intClientID == matricula && x.intQuestaoID == idQuestao);

                var recursoConcedidoBanca = forum.ForumPosQuestao.RecursoConcedidoPelaBanca ?? false;
                var recursoAcademico = (forum.ForumPreQuestao.StatusAnaliseAcademica ?? -1) == 1;

                var bancaLido = recursoConcedidoBanca && (recursoLog != null && recursoLog.bitVisualizadoBanca);
                var academicaLido = recursoAcademico && (recursoLog != null && recursoLog.bitVisualizadoMedgrupo);


                return bancaLido || academicaLido;
            }

        }

        public string GetRecursosNomeProfessorFormatado(string nomeCompleto)
        {
            try
            {
                if (nomeCompleto == null) return string.Empty;

                var palavras = nomeCompleto.Trim().Split(' ');
                var nomeFormatado = nomeCompleto.ToString();

                if (palavras.Count() > 2)
                    nomeFormatado = string.Concat(palavras[0], " ", palavras[palavras.Count() - 1]);

                return nomeFormatado;
            }
            catch
            {
                throw;
            }
        }

        public int SetRecursoQuestaoAluno(int idQuestao, int matricula, RecursoAlunoLog recursoAlunoLog)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Criando recurso questão por aluno"))
                {
                    var log = ctx.tblLogRecursoAluno.FirstOrDefault(x => x.intQuestaoID == idQuestao && x.intClientID == matricula);

                    if (log != null)
                    {
                        log.dteDataCriacao = DateTime.Now;
                        log.bitVisualizadoBanca = recursoAlunoLog.RecursoConcedidoPelaBanca;
                        log.bitVisualizadoMedgrupo = recursoAlunoLog.StatusAnaliseAcademica == 1;

                    }
                    else
                    {

                        var logRecurso = new tblLogRecursoAluno
                        {
                            intQuestaoID = idQuestao,
                            dteDataCriacao = DateTime.Now,
                            intClientID = matricula,
                            bitVisualizadoBanca = recursoAlunoLog.RecursoConcedidoPelaBanca,
                            bitVisualizadoMedgrupo = recursoAlunoLog.StatusAnaliseAcademica == 1

                        };

                        ctx.tblLogRecursoAluno.Add(logRecurso);
                    }


                    return ctx.SaveChanges();
                }
            }
        }

        public int SetRespostaDiscursiva(RespostaDiscursivaPost resp)
        {
            try
            {
                using(MiniProfiler.Current.Step("Inserindo resposta discursiva"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        if (resp.Resposta.Length > 1000) return 0;
                        if (!ctx.tblExercicio_Historico.Any(h => h.intHistoricoExercicioID == resp.HistoricoId)) return 0;

                        var existeResposta = ctx.tblCartaoResposta_Discursiva.FirstOrDefault(
                            q => q.intQuestaoDiscursivaID == resp.QuestaoId &&
                                q.intExercicioTipoId == resp.ExercicioTipoId &&
                                q.intHistoricoExercicioID == resp.HistoricoId &&
                                q.intDicursivaId == resp.DiscursivaId);

                        if (existeResposta == null)
                        {
                            ctx.tblCartaoResposta_Discursiva.Add(
                                new tblCartaoResposta_Discursiva
                                {
                                    intExercicioTipoId = resp.ExercicioTipoId,
                                    intHistoricoExercicioID = resp.HistoricoId,
                                    intQuestaoDiscursivaID = resp.QuestaoId,
                                    intDicursivaId = resp.DiscursivaId,
                                    txtResposta = string.IsNullOrWhiteSpace(resp.Resposta) ? null : resp.Resposta,
                                    dteCadastro = DateTime.Now
                                });
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(resp.Resposta))
                                existeResposta.txtResposta = null;
                            else
                                existeResposta.txtResposta = resp.Resposta;
                        }

                        ctx.SaveChanges();
                    }
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int SetAnotacaoAlunoQuestao(int QuestaoId, int ClientID, string Comentario, int tipoExercicio)
        {
            try
            {
                using(MiniProfiler.Current.Step("Salvando anotação do aluno na questão"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        var mcc = new tblQuestao_Marcacao();
                        if (tipoExercicio == 1)
                        {
                            if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.SIMULADO))
                                return 0;

                            mcc = (from m in ctx.tblQuestao_Marcacao
                                join qs in ctx.tblQuestao_Simulado on m.intQuestaoID equals qs.intQuestaoID
                                where m.intClientID == ClientID && m.intQuestaoID == QuestaoId
                                orderby m.dtAnotacao descending
                                select m).FirstOrDefault();
                        }
                        else if (tipoExercicio == 2 || tipoExercicio == 3)
                        {

                            if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.CONCURSO))
                                return 0;

                            List<int> listaConcursoQuestoes;
                            using (var ctxMatDir = new DesenvContext())
                            {
                                listaConcursoQuestoes = (from qc in ctxMatDir.tblConcursoQuestoes
                                                        where qc.intQuestaoID == QuestaoId
                                                        select qc.intQuestaoID).ToList();
                            }

                            mcc = (from m in ctx.tblQuestao_Marcacao
                                where m.intClientID == ClientID
                                    && m.intQuestaoID == QuestaoId
                                    && listaConcursoQuestoes.Contains(m.intQuestaoID)
                                orderby m.dtAnotacao descending
                                select m).FirstOrDefault();
                        }

                        if (mcc == null)
                            ctx.tblQuestao_Marcacao.Add(new tblQuestao_Marcacao
                            {
                                intQuestaoID = QuestaoId,
                                intTipoExercicioID = tipoExercicio,
                                intClientID = ClientID,
                                dtAnotacao = DateTime.Now,
                                txtAnotacao = Comentario
                            });

                        else
                            mcc.txtAnotacao = Comentario;


                        ctx.SaveChanges();
                        return 1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public bool isQuestao(int QuestaoId, Exercicio.tipoExercicio TipoExercicio)
        {
            switch (TipoExercicio)
            {
                case Exercicio.tipoExercicio.SIMULADO:
                    return new QuestaoEntity().isQuestaoSimulado(QuestaoId);
                case Exercicio.tipoExercicio.CONCURSO:
                    return isQuestaoConcurso(QuestaoId);
                case Exercicio.tipoExercicio.APOSTILA:
                    return isQuestaoConcurso(QuestaoId);
                case Exercicio.tipoExercicio.MONTAPROVA:
                    return false;
                default:
                    return false;
            }
        }

        public bool isQuestaoSimulado(int QuestaoId)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Verificando se questão é de simulado"))
                {
                    var query = (from q in ctx.tblQuestao_Simulado where q.intQuestaoID == QuestaoId select q).FirstOrDefault();
                    return query != null;
                }
            }
        }

        public bool isQuestaoConcurso(int QuestaoId)
        {
            using(MiniProfiler.Current.Step("Verificando se questão é de concurso"))
            {
                var ctx = new DesenvContext();
                var query = (from q in ctx.tblConcursoQuestoes where q.intQuestaoID == QuestaoId select q).FirstOrDefault();
                return query != null;
            }
        }

        public int SetFavoritaQuestaoConcurso(int QuestaoId, int ClientID, bool Favorita)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Adicionando questão de concurso aos favoritos"))
                {
                    List<int> ListaQuestaoId;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        ListaQuestaoId = (from qs in ctxMatDir.tblConcursoQuestoes
                                        where qs.intQuestaoID == QuestaoId
                                        select qs.intQuestaoID
                                        ).ToList();
                    }

                    var mcc = (from m in ctx.tblQuestao_Marcacao
                            where m.intClientID == ClientID
                                && m.intQuestaoID == QuestaoId
                                && ListaQuestaoId.Contains(m.intQuestaoID)
                            orderby m.dtAnotacao descending
                            select m).FirstOrDefault();

                    if (mcc == null)
                    {
                        var marcacao = new tblQuestao_Marcacao
                        {
                            intQuestaoID = QuestaoId,
                            intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO),
                            intClientID = ClientID,
                            dtAnotacao = DateTime.Now,
                            bitFlagFavorita = Favorita
                        };
                        ctx.tblQuestao_Marcacao.Add(marcacao);
                    }
                    else
                    {

                        mcc.bitFlagFavorita = Favorita;

                    }
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }

                    return 1;
                }
            }
        }

        public int GetPermissaoAvaliacao(int questaoId, int exercicioTipoId, int tipoComentario, int matricula)
        {
             try
            {
                using(MiniProfiler.Current.Step("Obtendo permissão avaliação"))
                {
                    using (var ctx = new DesenvContext())
                    {
                        var jaAvaliou = ctx.tblAvaliacaoConteudoQuestao.Any(
                            a => a.intQuestaoId == questaoId
                                && a.intTipoExercicioId == exercicioTipoId
                                && a.intClientId == matricula
                                && a.intTipoComentario == tipoComentario);

                        return Convert.ToInt32(!jaAvaliou);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<QuestaoAvaliacaoComentarioOpcoes> GetOpcoesAvaliacaoNegativaComentarioQuestao()
        {
            using(MiniProfiler.Current.Step("Obtendo alternativas negativas na avaliação de comentario de questão"))
            {
                using (var ctx = new DesenvContext())
                {
                    return ctx.tblAvaliacaoConteudoQuestaoAlternativas
                        .Where(x => x.bitAtiva)
                        .Select(y => new QuestaoAvaliacaoComentarioOpcoes { ID = y.intID, Nome = y.txtDescricao }).ToList();
                }
            }
        }

        public int GetAvaliacaoRealizada(int questaoId, int exercicioTipoId, int tipoComentario, int matricula)
        {
            try
            {
                
                using (var ctx = new DesenvContext())
                {
                    using(MiniProfiler.Current.Step("Obtendo avaliação realizada"))
                    {           
                        var avaliacao = ctx.tblAvaliacaoConteudoQuestao.FirstOrDefault(
                            a => a.intQuestaoId == questaoId
                                && a.intTipoExercicioId == exercicioTipoId
                                && a.intClientId == matricula
                                && a.intTipoComentario == tipoComentario);

                        return avaliacao == null ? 0 : avaliacao.intNota;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

         public int SetDuvidaQuestaoConcurso(int QuestaoId, int ClientID, bool Duvida)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Inserindo duvida em questão de concurso"))
                {   
                    List<int> ListaQuestaoId;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        ListaQuestaoId = (from qs in ctxMatDir.tblConcursoQuestoes
                                        where qs.intQuestaoID == QuestaoId
                                        select qs.intQuestaoID).ToList();
                    }


                    var mcc = (from m in ctx.tblQuestao_Marcacao
                            where m.intClientID == ClientID
                                && m.intQuestaoID == QuestaoId
                                && ListaQuestaoId.Contains(m.intQuestaoID)
                            orderby m.dtAnotacao descending
                            select m).FirstOrDefault();

                    if (mcc == null)
                    {
                        var marcacao = new tblQuestao_Marcacao
                        {
                            intQuestaoID = QuestaoId,
                            intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO),
                            intClientID = ClientID,
                            dtAnotacao = DateTime.Now,
                            bitFlagEmDuvida = Duvida
                        };
                        ctx.tblQuestao_Marcacao.Add(marcacao);
                    }
                    else
                    {

                        mcc.bitFlagEmDuvida = Duvida;

                    }
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }

                    return 1;
                }
            }
        }

        public int SetAvaliacao(QuestaoAvaliacaoComentario questaoAvaliacaoComentario)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Inserindo avaliação"))
                {
                    var retorno = 1;
                    try
                    {
                        var avaliacao = new tblAvaliacaoConteudoQuestao();
                        avaliacao.intQuestaoId = questaoAvaliacaoComentario.QuestaoId;
                        avaliacao.intTipoExercicioId = questaoAvaliacaoComentario.ExercicioTipoId;
                        avaliacao.intClientId = questaoAvaliacaoComentario.AlunoMatricula;
                        avaliacao.intNota = questaoAvaliacaoComentario.Nota;
                        avaliacao.txtComentarioAvaliacao = questaoAvaliacaoComentario.ComentarioAvaliacao;
                        avaliacao.intTipoComentario = questaoAvaliacaoComentario.TipoComentario;
                        avaliacao.dteCadastro = DateTime.Now;
                        avaliacao.bitActive = true;

                        var objComentario = ctx.tblLogConcursoQuestaoComentario
                            .Where(c => c.intQuestaoID == questaoAvaliacaoComentario.QuestaoId)
                            .OrderByDescending(q => q.id).FirstOrDefault();

                        if (avaliacao.intTipoComentario == 2 || objComentario == null)
                        {
                            avaliacao.intComentarioLogId = null;
                            avaliacao.intAlternativaReprova = questaoAvaliacaoComentario.OpcaoComentarioNegativo;
                        }

                        else
                            avaliacao.intComentarioLogId = objComentario.id;


                        ctx.tblAvaliacaoConteudoQuestao.Add(avaliacao);
                        ctx.SaveChanges();

                        return retorno;
                    }
                    catch
                    {
                        retorno = 0;
                        return retorno;
                    }
                }
            }
        }

        public List<PPQuestao> GetQuestoesComComentarioApostilaCache(int EntidadeID)
        {
            using(MiniProfiler.Current.Step("Obtendo questões com comentario apostila em cache"))
            {
                try
                {
                    if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetQuestoesComComentarioApostila))
                        return GetQuestoesComComentarioApostila(EntidadeID);

                    var key = String.Format("{0}:{1}", RedisCacheConstants.Questao.KeyGetQuestoesComComentarioApostila, EntidadeID);
                    var ppQuestoes = RedisCacheManager.GetItemObject<List<PPQuestao>>(key);

                    if (ppQuestoes != null)
                        return ppQuestoes;

                    var ret = GetQuestoesComComentarioApostila(EntidadeID);

                    RedisCacheManager.SetItemObject(key, ret);

                    return ret;
                }
                catch
                {
                    return GetQuestoesComComentarioApostila(EntidadeID);
                }
            }
        }

        public List<PPQuestao> GetQuestoesComComentarioApostila(int EntidadeID)
        {
            using(MiniProfiler.Current.Step("Obtendo questões com comentario apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var etapa = PPQuestao.EtapaPortal.SomenteComentario;
                    var r1r3 = PPQuestao.R1R3.Todos;
                    var intEmployeeID = 0;
                    var maxRegistros = 2147483647;
                    var pagina = 1;
                    var areas = 0;
                    var idQuestao = 0;
                    var idProva = 0;
                    var somenteID = true;
                    var idApostila = 0;
                    var idApostilaEntidade = EntidadeID;
                    var concursoPremium = 0;
                    var concurso = string.Empty;
                    var anoImpressao = 0;
                    var tipoFiltroPalavraChave = 0;
                    var palavraChave = string.Empty;
                    var somenteQtdMaxima = false;
                    var labels = string.Empty;
                    var consideraConcursoBloqueado = true;
                    var idProtocolo = 0;
                    var excluirProtocoladas = false;
                    var excluirFavoritas = false;
                    var ano = 0;

                    var productGroup = (from b in ctx.tblBooks
                                        join pr in ctx.tblProducts on b.intBookID equals pr.intProductID
                                        where b.intBookEntityID == idApostilaEntidade && pr.intProductGroup1 == 4 && pr.intProductGroup2 != 47
                                        orderby pr.intProductGroup2 descending
                                        select pr.intProductGroup2
                    ).Distinct().FirstOrDefault();

                    var produto = Extend.GetProductByCourse((Produto.Cursos)productGroup);

                    var pp = new PPQuestao()
                    {
                        FiltroLabels = Utilidades.ConvertStringToListInt((labels)),
                        FiltroEtapa = etapa,
                        FiltroR1R3 = r1r3,
                        Ano = ano,
                        FiltroProduto = produto,
                        FiltroIntEmployeeID = intEmployeeID,
                        FiltroArea = (GrandeArea.EspeciaisFlags)areas,
                        FiltroIDQuestao = idQuestao,
                        FiltroProva = idProva,
                        FiltroApostila = idApostila,
                        FiltroApostilaEntidade = idApostilaEntidade,
                        FiltroBitConcursoPremium = concursoPremium,
                        FiltroConcurso = concurso,
                        FiltroAnoImpressao = anoImpressao,
                        TipoFiltroPalavraChave = (PPQuestao.TipoFiltroPalavraChaveLogico)tipoFiltroPalavraChave,
                        FiltroPalavraChave = HttpUtility.UrlDecode(palavraChave),
                        FiltroConcursoBloqueado = consideraConcursoBloqueado,
                        FiltroProtocolo = idProtocolo,
                        FiltroExcluirProtocolada = excluirProtocoladas,
                        FiltroExcluirFavorita = excluirFavoritas
                    };

                    var ppQuestoes = new PortalProfessorEntity().GetQuestoesPorEtapaPortal(pp, maxRegistros, pagina, somenteID, somenteQtdMaxima);
                    ppQuestoes.ForEach(x => x.PossuiComentario = true);

                    var apostilaEntidade = new ApostilaEntidadeEntity().GetAll().FirstOrDefault(x => x.ID == EntidadeID);
                    if (apostilaEntidade.Nome.Contains("R3") || apostilaEntidade.Nome.Contains("R4"))
                    {
                        etapa = PPQuestao.EtapaPortal.SemComentario;
                        pp.FiltroEtapa = etapa;
                        var ppQuestoesSemComentario = new PortalProfessorEntity().GetQuestoesPorEtapaPortal(pp, maxRegistros, pagina, somenteID, somenteQtdMaxima);
                        ppQuestoesSemComentario.ForEach(x => x.PossuiComentario = false);
                        ppQuestoes.AddRange(ppQuestoesSemComentario);
                    }

                    return ppQuestoes;
                }
            }
        }

        public IQueryable<int> GetQuestoesPorTipo(PPQuestao.R1R3 tipoQuestao, ref DesenvContext ctx)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo questões por tipo"))
                {
                    /*Regra do R3:
                    * 
                    * As questões são classificadas em R1 ou R3 de acordo com suas provas. Questões R1 são de provas R1 e R3 de provas R3.
                    * Existe uma regra de tratamento:
                    * 
                    * Se estivermos validando as questões para MED, só consideramos as questões R3 como sendo as questões de provas R3 de concurso 2011 em diante
                    * Se a validação for em MEDCURSO, as questões tem que ser de concursos de 2012 em diante
                    * E no caso de MEDELETRO, as questões tem que ser de concursos 2013 em diante.
                    * 
                    * Assim sendo, qualquer questão R3 que não bater na regra acima precisa ser tratada como R1.
                    * 
                    * Motivo: existem lugares em que não exibimos questões R3, mas essa regra foi sendo implementada por produtos em anos diferentes.
                    * Como as questões antes de ter a regra implementada já foram vistas pelos alunos, elas continuam em exibição.
                    * 
                    * 28/11/2018: A regra acima foi removida com a criação de um produto do tipo R3/R4. Agora nao faremos nenhum filtro por ano, retornaremos de acordo com a 
                    * classificação. Posteriormente, criaremos um campo "tipo", para pararmos de utilizar a string de descrição para filtrar o tipo de prova
                    * */

                    IQueryable<int> consulta;

                    switch (tipoQuestao)
                    {
                        case PPQuestao.R1R3.R1:
                            consulta = (from questao in ctx.tblConcursoQuestoes
                                        join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                                        join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                                        where !tipo.txtDescription.ToUpper().Contains("R3") && !tipo.txtDescription.ToUpper().Contains("R4")
                                        select new
                                        {
                                            questao.intQuestaoID
                                        }).Select(questao => questao.intQuestaoID);
                            break;
                        case PPQuestao.R1R3.R3:
                            consulta = (from questao in ctx.tblConcursoQuestoes
                                        join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                                        join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                                        where tipo.txtDescription.ToUpper().Contains("R3") || tipo.txtDescription.ToUpper().Contains("R4")
                                        select new
                                        {
                                            questao.intQuestaoID
                                        }).Select(questao => questao.intQuestaoID);
                            break;
                        case PPQuestao.R1R3.Todos:
                        default:
                            consulta = (from questao in ctx.tblConcursoQuestoes
                                        join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                                        join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                                        select new
                                        {
                                            questao.intQuestaoID
                                        }).Select(questao => questao.intQuestaoID);
                            break;
                    }

                    return consulta;
                }
            }
            catch
            {
                throw;
            }
        }

        public Professor GetPrimeiroComentario(int idQuestao)
        {
            using(MiniProfiler.Current.Step("Obtendo primeiro comentario"))
            {
                using (var ctx = new DesenvContext())
                {
                    var resp = Utilidades.GetResponsibilitiesAcademic();

                    var academico = (from e in ctx.tblEmployees
                                    where resp.Contains(e.intResponsabilityID)
                                    select (e.intEmployeeID)
                                    ).ToList();

                    var prof = (from log in ctx.tblLogConcursoQuestaoComentario
                                join p in ctx.tblPersons on log.intEmployeeAlterou equals p.intContactID
                                where log.intQuestaoID == idQuestao
                                    && !academico.Contains((int)log.intEmployeeAlterou)
                                orderby log.dteDateAlteracao ascending
                                select new
                                {
                                    ID = p.intContactID,
                                    Nome = p.txtName.Trim(),
                                    DataAcao = log.dteDateAlteracao
                                }).FirstOrDefault();

                    var professor = new Professor();
                    if (prof != null)
                    {
                        professor.ID = prof.ID;
                        professor.Nome = prof.Nome.Trim();
                        professor.DataAcao = prof.DataAcao;
                    }

                    return professor;
                }
            }
        }

        public Professor GetUltimoComentario(int idQuestao)
        {
            using(MiniProfiler.Current.Step("Obtendo último comentario"))
            {
                using (var ctx = new DesenvContext())
                {
                    var resp = Utilidades.GetResponsibilitiesAcademic();

                    var academico = (from e in ctx.tblEmployees
                                    where resp.Contains(e.intResponsabilityID)
                                    select (e.intEmployeeID)
                                    ).ToList();

                    var prof = (from log in ctx.tblLogConcursoQuestaoComentario
                                join p in ctx.tblPersons on log.intEmployeeAlterou equals p.intContactID
                                where log.intQuestaoID == idQuestao
                                    && !academico.Contains((int)log.intEmployeeAlterou)
                                orderby log.dteDateAlteracao descending
                                select new
                                {
                                    ID = p.intContactID,
                                    Nome = p.txtName.Trim(),
                                    DataAcao = log.dteDateAlteracao
                                }).FirstOrDefault();

                    var professor = new Professor();
                    if (prof != null)
                    {
                        professor.ID = prof.ID;
                        professor.Nome = prof.Nome.Trim();
                        professor.DataAcao = prof.DataAcao;
                    }

                    return professor;
                }
            }
        }

        public QuestaoDTO CacheQuestao(int QuestaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                try
                {
                    if (RedisCacheManager.CannotCache(RedisCacheConstants.Simulado.QuestaoSimulado))
                    {
                        return (from q in ctx.tblQuestoes
                                where q.intQuestaoID == QuestaoID
                                select new QuestaoDTO
                                {
                                    intQuestaoID = q.intQuestaoID,
                                    intOldQuestaoID = q.intOldQuestaoID,
                                    intInstituicaoID = q.intInstituicaoID,
                                    intNivelDeDificuladeID = q.intNivelDeDificuladeID,
                                    bitCasoClinico = q.bitCasoClinico,
                                    bitConceitual = q.bitConceitual,
                                    intGrandeAreaID = q.intGrandeAreaID,
                                    intEspecialidadeID = q.intEspecialidadeID,
                                    intSubEspecialidadeID = q.intSubEspecialidadeID,
                                    ID_CONCURSO = q.ID_CONCURSO,
                                    txtEnunciado = q.txtEnunciado,
                                    txtComentario = q.txtComentario,
                                    txtObservacao = q.txtObservacao,
                                    txtRecurso = q.txtRecurso,
                                    dteQuestao = q.dteQuestao,
                                    intFonteID = q.intFonteID,
                                    txtFonteTipo = q.txtFonteTipo,
                                    txtOrigem = q.txtOrigem,
                                    intYear = q.intYear,
                                    intProfessorAutor = q.intProfessorAutor,
                                    intProfessorFilmagem = q.intProfessorFilmagem,
                                    dteFilmagem = q.dteFilmagem,
                                    guidQuestaoID = q.guidQuestaoID,
                                    bitAnulada = q.bitAnulada,
                                    intEmployeeComentarioID = q.intEmployeeComentarioID,
                                    dteLimite = q.dteLimite,
                                    intQuestaoConcursoID = q.intQuestaoConcursoID
                                }).FirstOrDefault();
                    }
                    else
                    {
                        var key = String.Format("{0}:{1}", RedisCacheConstants.Simulado.QuestaoSimulado, QuestaoID);
                        var questao = RedisCacheManager.GetItemObject<QuestaoDTO>(key);

                        if (questao == null)
                        {
                            questao = (from q in ctx.tblQuestoes
                                       where q.intQuestaoID == QuestaoID
                                       select new QuestaoDTO()
                                       {
                                           intQuestaoID = q.intQuestaoID,
                                           intOldQuestaoID = q.intOldQuestaoID,
                                           intInstituicaoID = q.intInstituicaoID,
                                           intNivelDeDificuladeID = q.intNivelDeDificuladeID,
                                           bitCasoClinico = q.bitCasoClinico,
                                           bitConceitual = q.bitConceitual,
                                           intGrandeAreaID = q.intGrandeAreaID,
                                           intEspecialidadeID = q.intEspecialidadeID,
                                           intSubEspecialidadeID = q.intSubEspecialidadeID,
                                           ID_CONCURSO = q.ID_CONCURSO,
                                           txtEnunciado = q.txtEnunciado,
                                           txtComentario = q.txtComentario,
                                           txtObservacao = q.txtObservacao,
                                           txtRecurso = q.txtRecurso,
                                           dteQuestao = q.dteQuestao,
                                           intFonteID = q.intFonteID,
                                           txtFonteTipo = q.txtFonteTipo,
                                           txtOrigem = q.txtOrigem,
                                           intYear = q.intYear,
                                           intProfessorAutor = q.intProfessorAutor,
                                           intProfessorFilmagem = q.intProfessorFilmagem,
                                           dteFilmagem = q.dteFilmagem,
                                           guidQuestaoID = q.guidQuestaoID,
                                           bitAnulada = q.bitAnulada,
                                           intEmployeeComentarioID = q.intEmployeeComentarioID,
                                           dteLimite = q.dteLimite,
                                           intQuestaoConcursoID = q.intQuestaoConcursoID
                                       }).FirstOrDefault();
                            if (questao != null)
                            {
                                var timeoutHour = 6;
                                RedisCacheManager.SetItemObject(key, questao, TimeSpan.FromHours(timeoutHour));
                            }
                        }

                        return questao;
                    }
                }
                catch (Exception)
                {
                    return (from q in ctx.tblQuestoes
                            where q.intQuestaoID == QuestaoID
                            select new QuestaoDTO()
                            {
                                intQuestaoID = q.intQuestaoID,
                                intOldQuestaoID = q.intOldQuestaoID,
                                intInstituicaoID = q.intInstituicaoID,
                                intNivelDeDificuladeID = q.intNivelDeDificuladeID,
                                bitCasoClinico = q.bitCasoClinico,
                                bitConceitual = q.bitConceitual,
                                intGrandeAreaID = q.intGrandeAreaID,
                                intEspecialidadeID = q.intEspecialidadeID,
                                intSubEspecialidadeID = q.intSubEspecialidadeID,
                                ID_CONCURSO = q.ID_CONCURSO,
                                txtEnunciado = q.txtEnunciado,
                                txtComentario = q.txtComentario,
                                txtObservacao = q.txtObservacao,
                                txtRecurso = q.txtRecurso,
                                dteQuestao = q.dteQuestao,
                                intFonteID = q.intFonteID,
                                txtFonteTipo = q.txtFonteTipo,
                                txtOrigem = q.txtOrigem,
                                intYear = q.intYear,
                                intProfessorAutor = q.intProfessorAutor,
                                intProfessorFilmagem = q.intProfessorFilmagem,
                                dteFilmagem = q.dteFilmagem,
                                guidQuestaoID = q.guidQuestaoID,
                                bitAnulada = q.bitAnulada,
                                intEmployeeComentarioID = q.intEmployeeComentarioID,
                                dteLimite = q.dteLimite,
                                intQuestaoConcursoID = q.intQuestaoConcursoID
                            }).FirstOrDefault();
                }

            }

        }

        public List<Alternativa> GetAlternativasQuestao(int QuestaoID, string isCasoClinico)
        {
            using (var ctx = new AcademicoContext())
            {
                var alternativas = (from a in ctx.tblQuestaoAlternativas where a.intQuestaoID == QuestaoID select a)
                    .ToList()
                    .Select(a => new Alternativa()
                    {
                        Correta = a.bitCorreta ?? false,
                        Letra = Convert.ToChar(a.txtLetraAlternativa),
                        Nome = Convert.ToBoolean(Convert.ToInt32(isCasoClinico)) ?
                            (string.IsNullOrEmpty(a.txtAlternativa) ? "Resposta: " : a.txtAlternativa)
                            : a.txtAlternativa,

                        Gabarito = Convert.ToBoolean(Convert.ToInt32(isCasoClinico)) ?
                            (string.IsNullOrEmpty(a.txtResposta) ? "(Sem Gabarito)" : a.txtResposta)
                            : a.txtResposta,
                        Id = a.intAlternativaID
                    })
                    .ToList();

                return alternativas;
            }
        }

        public TabelaQuestaoSimuladoDTO GetQuestao_tblQuestaoSimulado(int QuestaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var questao = (from qs in ctx.tblQuestao_Simulado
                               where qs.intQuestaoID == QuestaoID
                               select new TabelaQuestaoSimuladoDTO()
                               {
                                   intQuestaoID = qs.intQuestaoID,
                                   intSimuladoID = qs.intSimuladoID,
                                   txtCodigoCorrecao = qs.txtCodigoCorrecao,
                                   bitAnulada = qs.bitAnulada
                               }).FirstOrDefault();
                return questao;
            }
        }

        public List<Imagem> GetComantarioImagemSimulado(int QuestaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var imagensComentario = (from i in ctx.tblQuestoesSimuladoImagem_Comentario
                                         where i.intQuestaoID == QuestaoID
                                         select new Imagem
                                         {
                                             ID = i.intImagemComentarioID,
                                             Url = Constants.URLCOMENTARIOIMAGEMSIMULADOMSCROSS.Replace("IDCOMENTARIOIMAGEM", i.intImagemComentarioID.ToString().Trim()),
                                             Nome = i.txtName
                                         }).ToList();

                return imagensComentario;
            }
        }

        public SimuladoVersaoDTO GetSimuladoVersao(int QuestaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var simuladoVersao = (from o in ctx.tblSimuladoOrdenacao
                                      where o.intQuestaoID == QuestaoID
                                      select new SimuladoVersaoDTO()
                                      {
                                          intSimuladoID = o.intSimuladoID,
                                          intQuestaoID = o.intQuestaoID,
                                          intVersaoID = 1,
                                          intOrdem = o.intOrdem ?? 0
                                      }).Union(
                    from v in ctx.tblSimuladoVersao
                    where v.intQuestao == QuestaoID && v.intVersaoID == 1
                    select new SimuladoVersaoDTO()
                    {
                        intSimuladoID = v.intSimuladoID,
                        intQuestaoID = v.intQuestaoID,
                        intVersaoID = v.intVersaoID,
                        intOrdem = v.intQuestao
                    }
                ).FirstOrDefault();

                return simuladoVersao;
            }
        }

        public bool GetSimuladoIsOnline(int ClientID, int QuestaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (from h in ctx.tblExercicio_Historico
                        join qs in ctx.tblQuestao_Simulado on h.intExercicioID equals qs.intSimuladoID
                        where h.intClientID == ClientID
                        && qs.intQuestaoID == QuestaoID
                        && h.bitRealizadoOnline == true
                        && h.dteDateFim == null
                        select h.intTipoProva).Any();
            }
        }

        public string GetRespostaObjetivaSimulado(int QuestaoID, int ClientID, bool isSimuladoOnline)
        {
            using (var ctx = new AcademicoContext())
            {
                if (isSimuladoOnline)
                {
                    return (from h in ctx.tblExercicio_Historico
                            join c in ctx.tblCartaoResposta_objetiva_Simulado_Online on h.intHistoricoExercicioID equals c.intHistoricoExercicioID
                            where h.intClientID == ClientID && c.intQuestaoID == QuestaoID && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                            orderby c.dteCadastro descending
                            select c.txtLetraAlternativa).ToList().FirstOrDefault();
                }
                else
                {
                    return (from h in ctx.tblExercicio_Historico
                            join c in ctx.tblCartaoResposta_objetiva on h.intHistoricoExercicioID equals c.intHistoricoExercicioID
                            where h.intClientID == ClientID && c.intQuestaoID == QuestaoID && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                            orderby c.dteCadastro descending
                            select c.txtLetraAlternativa).ToList().FirstOrDefault();
                }
            }
        }

        public bool GetQuestaoSimuladoIsCorreta(int ClientID, int QuestaoID, bool isSimuladoOnline)
        {
            using (var ctx = new AcademicoContext())
            {

                if (isSimuladoOnline)
                {
                    return (from h in ctx.tblExercicio_Historico
                            join c in ctx.tblCartaoResposta_objetiva_Simulado_Online on h.intHistoricoExercicioID equals c.intHistoricoExercicioID
                            join a in ctx.tblQuestaoAlternativas on c.txtLetraAlternativa equals a.txtLetraAlternativa
                            where
                            c.intQuestaoID == QuestaoID
                            && h.intClientID == ClientID
                            && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                            && a.intQuestaoID == QuestaoID
                            select a).ToList().Any();
                }
                else
                {
                    return (from h in ctx.tblExercicio_Historico
                            join c in ctx.tblCartaoResposta_objetiva on h.intHistoricoExercicioID equals c.intHistoricoExercicioID
                            join a in ctx.tblQuestaoAlternativas on c.txtLetraAlternativa equals a.txtLetraAlternativa
                            where
                            c.intQuestaoID == QuestaoID
                            && h.intClientID == ClientID
                            && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                            && a.intQuestaoID == QuestaoID
                            select a).ToList().Any();
                }

            }
        }

        public QuestaoMarcacaoDTO GetQuestaoMarcacao(int QuestaoID, int ClientID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (from m in ctx.tblQuestao_Marcacao
                        where m.intClientID == ClientID && m.intQuestaoID == QuestaoID
                        orderby m.dtAnotacao descending
                        select new QuestaoMarcacaoDTO()
                        {
                            intID = m.intID,
                            bitFlagEmDuvida = m.bitFlagEmDuvida,
                            bitFlagFavorita = m.bitFlagFavorita,
                            dtAnotacao = m.dtAnotacao,
                            intClientID = m.intClientID,
                            intQuestaoID = m.intQuestaoID,
                            txtAnotacao = m.txtAnotacao,
                            intTipoExercicioID = m.intTipoExercicioID
                        }).FirstOrDefault();
            }
        }

        public Questao GetAtualizacaoDados(String guid)
        {
            try
            {
                new Guid(guid);

                Questao entidadeQuestao = new Questao();

                DBQuery q = new DBQuery("msp_Medsoft_SelectAtualizacao_Dados 'QUESTAO', '" + guid + "'");
                System.Data.Common.DbDataReader rs = q.reader;

                if (rs.NextResult())
                {
                    if (rs.Read())
                    {
                        entidadeQuestao.Guid = rs["guidQuestaoID"] == DBNull.Value ? "" : rs["guidQuestaoID"].ToString();
                        entidadeQuestao.Id = rs["intQuestaoID"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intQuestaoID"]);
                        entidadeQuestao.Enunciado = rs["txtEnunciado"] == DBNull.Value ? "" : rs["txtEnunciado"].ToString();
                        entidadeQuestao.Comentario = rs["txtComentario"] == DBNull.Value ? "" : rs["txtComentario"].ToString();
                        entidadeQuestao.Anulada = rs["bitAnulada"] == DBNull.Value ? false : Convert.ToBoolean(rs["bitAnulada"]);
                        entidadeQuestao.CodigoCorrecao = rs["txtCodigoCorrecao"] == DBNull.Value ? "" : rs["txtCodigoCorrecao"].ToString();
                        entidadeQuestao.ExercicioTipoID = rs["intExercicioTipo"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intExercicioTipo"]);
                        entidadeQuestao.ExercicioTipo = rs["txtExercicioTipo"] == DBNull.Value ? "" : rs["txtExercicioTipo"].ToString();
                        entidadeQuestao.Duracao = rs["intDuration"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intDuration"]);
                        entidadeQuestao.Tipo = rs["intTipo"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intTipo"]);
                        entidadeQuestao.Especialidades = new List<Especialidade>(){
                                                                                      new Especialidade()
                                                                                      {
                                                                                          Id = rs["intEspecialidadeID"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intEspecialidadeID"])
                                                                                      }
                                                                                  };

                        List<Alternativa> alternativas = new List<Alternativa>();
                        if (rs.NextResult())
                        {

                            while (rs.Read())
                            {

                                Alternativa alt = new Alternativa()
                                {
                                    Correta = Convert.ToBoolean(rs["bitCorreta"]),
                                    Letra = Convert.ToChar(rs["txtLetraAlternativa"]),
                                    Nome = rs["txtAlternativa"].ToString()
                                };

                                alternativas.Add(alt);
                            }
                        }
                        entidadeQuestao.Alternativas = alternativas;

                        List<QuestaoImagem> imagens = new List<QuestaoImagem>();
                        if (rs.NextResult())
                        {

                            while (rs.Read())
                            {
                                QuestaoImagem imagem = new QuestaoImagem()
                                {
                                    Id = Convert.ToInt32(rs["intID"]),
                                    Imagem = (byte[])rs["imgImagem"],
                                    Label = rs["txtLabel"].ToString(),
                                    Nome = rs["txtName"].ToString(),
                                    QuestaoId = Convert.ToInt32(rs["intQuestaoID"])
                                };

                                imagens.Add(imagem);
                            }

                        }
                        entidadeQuestao.Imagens = imagens;

                        var gabarito = string.Empty;
                        if (entidadeQuestao.ExercicioTipoID == 2 && entidadeQuestao.Tipo == 2 && entidadeQuestao.Enunciado.ToUpper().Contains("GABARITO"))
                        {
                            gabarito = entidadeQuestao.Enunciado.Substring(entidadeQuestao.Enunciado.ToUpper().IndexOf("GABARITO"));
                            entidadeQuestao.Enunciado = entidadeQuestao.Enunciado.Remove(entidadeQuestao.Enunciado.ToUpper().IndexOf("GABARITO"));
                        }


                        List<QuestaoDiscursiva> discursivas = new List<QuestaoDiscursiva>();

                        if (rs.NextResult())
                        {
                            while (rs.Read())
                            {
                                QuestaoDiscursiva d = new QuestaoDiscursiva()
                                {
                                    Id = rs["intQuestaodiscursivaID"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intQuestaodiscursivaID"]),
                                    Enunciado = rs["txtEnunciado"] == DBNull.Value ? null : Convert.ToString(rs["txtEnunciado"]),
                                    Resposta = !string.IsNullOrEmpty(gabarito) ? gabarito : rs["txtResposta"] == DBNull.Value ? null : Convert.ToString(rs["txtResposta"]),
                                    ExercicioTipo = rs["intExercicioTipo"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intExercicioTipo"]),
                                    Tempo = rs["intTempo"] == DBNull.Value ? 0 : Convert.ToInt32(rs["intTempo"])
                                };

                                discursivas.Add(d);
                            }
                        }
                        entidadeQuestao.Discursivas = discursivas;

                        List<QuestaoAnotacao> anotacoes = new List<QuestaoAnotacao>();

                        if (rs.NextResult())
                        {
                            while (rs.Read())
                            {
                                QuestaoAnotacao a = new QuestaoAnotacao()
                                {
                                    Favorita = Convert.ToBoolean(rs["intQuestaoID"]),
                                    Duvida = Convert.ToBoolean(rs["intQuestaoID"]),
                                    DataHora = Utilidades.FormatDataTime(Convert.ToDateTime(rs["intQuestaoID"])),
                                    Anotacao = Convert.ToString(rs["intQuestaoID"])
                                };
                                anotacoes.Add(a);
                            }

                        }
                        entidadeQuestao.Anotacoes = anotacoes;

                        var especs = new Especialidades();
                        if (rs.NextResult())
                            while (rs.Read())
                            {
                                Especialidade espec = new Especialidade()
                                {
                                    Id = Convert.ToInt32(rs["intEspecialidadeId"])
                                };
                                especs.Add(espec);
                            }

                        entidadeQuestao.Especialidades = especs;


                    }
                }

                return entidadeQuestao;
            }
            catch (FormatException)
            {
                return new Questao();
            }
            catch
            {
                throw;
            }
        }

        public ForumQuestao.Coluna1 GetForumQuestaoCol1(int idQuestao, int idTipoQuestao, int matricula)
        {
            var forum = new ForumQuestao.Coluna1();
            var parametros = new SqlParameter[] {
                                                    new SqlParameter("intQuestaoID", idQuestao),
                                                    new SqlParameter("col", 1),
                                                    new SqlParameter("intClientID", matricula),
                                                    new SqlParameter("intTipoQuestao", idTipoQuestao)
                                                };

            var ds = new DBQuery().ExecuteStoredProcedure("msp_Medsoft_SelectQuestao_Forum", parametros);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var naoCabe = ds.Tables[0].AsEnumerable().Count(r => r["bitOpiniao"].ToString().ToUpper().Contains("NÃO"));
                var cabe = ds.Tables[0].Rows.Count - naoCabe;

                forum.QtdCabeRecuso = cabe;
                forum.QtdNaoCabeRecuso = naoCabe;

                forum.Comentarios = new List<ForumQuestaComentario>();
                foreach (DataRow dRow in ds.Tables[0].Rows)
                    forum.Comentarios.Add(
                        new ForumQuestaComentario
                        {
                            Nome = dRow["txtName"].ToString(),
                            Uf = dRow["txtUF"].ToString(),
                            Opiniao = dRow["bitOpiniao"].ToString().Replace(":", string.Empty),
                            ComentarioTexto = dRow["comentario"].ToString(),
                            DataAmigavel = dRow["dataAmigavel"].ToString(),
                            Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() }
                        });
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                forum.StatusAnaliseAcademica = GetTextoAnaliseAcademicaRecurso(Convert.ToInt32(ds.Tables[1].Rows[0]["bitStatus"]));
                forum.Analises = new List<AnaliseAcademica>();
                foreach (DataRow dRow in ds.Tables[1].Rows)
                    forum.Analises.Add(new AnaliseAcademica
                    {
                        NomeProfessor = dRow["txtNomeProf"].ToString(),
                        UrlAvatarProfessor = dRow["txtProfPhotoPath"].ToString(),
                        Texto = dRow["txtAnaliseMedgrupo"].ToString()
                    });
            }
            return forum;
        }

        public ForumQuestao.Coluna2 GetForumQuestaoCol2(int idQuestao, int idTipoQuestao, int matricula)
        {
            var forum = new ForumQuestao.Coluna2();
            var parametros = new SqlParameter[] {
                                                    new SqlParameter("intQuestaoID", idQuestao),
                                                    new SqlParameter("col", 2),
                                                    new SqlParameter("intClientID", matricula),
                                                    new SqlParameter("intTipoQuestao", idTipoQuestao)
                                                };

            var ds = new DBQuery().ExecuteStoredProcedure("msp_Medsoft_SelectQuestao_Forum", parametros);

            if (ds.Tables[0].Rows.Count > 0)
            {
                forum.Comentarios = new List<ForumQuestaComentario>();
                foreach (DataRow dRow in ds.Tables[0].Rows)
                    forum.Comentarios.Add(
                        new ForumQuestaComentario
                        {
                            Nome = dRow["txtName"].ToString(),
                            Uf = dRow["txtUF"].ToString(),
                            Opiniao = dRow["bitOpiniao"].ToString().Replace(":", string.Empty),
                            ComentarioTexto = dRow["comentario"].ToString(),
                            DataAmigavel = dRow["dataAmigavel"].ToString(),
                            Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() }
                        });
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                forum.AnalisesFinais = new List<AnaliseAcademica>();
                foreach (DataRow dRow in ds.Tables[1].Rows)
                    forum.AnalisesFinais.Add(new AnaliseAcademica
                    {
                        NomeProfessor = dRow["txtName"].ToString(),
                        UrlAvatarProfessor = dRow["txtProfPhotoPath"].ToString(),
                        Texto = dRow["comentario"].ToString()
                    });
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                var dRow = ds.Tables[2].Rows[0];
                forum.VereditoBanca = dRow["txtComentarioBanca"].ToString();
                forum.RecursoConcedidoPelaBanca = string.IsNullOrEmpty(dRow["bitStatus"].ToString()) ? false : Convert.ToBoolean(dRow["bitStatus"]);
            }
            return forum;
        }

        public ForumQuestao.Coluna3 GetForumQuestaoCol3(int idQuestao, int idTipoQuestao, int matricula)
        {
            var forum = new ForumQuestao.Coluna3();
            var parametros = new SqlParameter[] {
                                                    new SqlParameter("intQuestaoID", idQuestao),
                                                    new SqlParameter("col", 3),
                                                    new SqlParameter("intClientID", matricula),
                                                    new SqlParameter("intTipoQuestao", idTipoQuestao)
                                                };

            var ds = new DBQuery().ExecuteStoredProcedure("msp_Medsoft_SelectQuestao_Forum", parametros);

            if (ds.Tables[0].Rows.Count > 0)
            {
                forum.Comentarios = new List<ForumQuestaComentario>();
                foreach (DataRow dRow in ds.Tables[0].Rows)
                    forum.Comentarios.Add(
                        new ForumQuestaComentario
                        {
                            Nome = dRow["txtName"].ToString(),
                            Uf = dRow["txtUF"].ToString(),

                            ComentarioTexto = dRow["comentario"].ToString(),
                            DataAmigavel = dRow["dataAmigavel"].ToString(),
                            Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() }
                        });
            }
            return forum;
        }

        public List<Estatistica> GetEstatisticaExercicio(int idQuestao, int idTipoExercicio)
        {
            var estatisticas = new List<Estatistica>();
            var est = new Dictionary<string, string>();

            if (idTipoExercicio == 1)
                est = GetEstatistica(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO));
            else if (idTipoExercicio == 2)
                est = GetEstatistica(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO));

            foreach (var e in est)
            {
                Double Num;
                bool isNum = double.TryParse(e.Value, out Num);
                if (isNum)
                    estatisticas.Add(new Estatistica { Letra = e.Key, Valor = Convert.ToDouble(e.Value) });
                else
                {
                    foreach (var x in estatisticas)
                    {
                        if (x.Letra == e.Value)
                            x.Correta = true;
                    }
                }
            }

            return estatisticas;
        }

        public int SetFavoritaQuestaoSimulado(int QuestaoId, int ClientID, bool Favorita)
        {
            using(MiniProfiler.Current.Step("Gravando questão favorita simulado"))
            {
                using (var ctx = new AcademicoContext())
                {
                    if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.SIMULADO))
                        return 0;

                    var mcc = (from m in ctx.tblQuestao_Marcacao
                            join qs in ctx.tblQuestao_Simulado on m.intQuestaoID equals qs.intQuestaoID
                            where m.intClientID == ClientID && m.intQuestaoID == QuestaoId
                            orderby m.dtAnotacao descending
                            select m).FirstOrDefault();

                    if (mcc == null)
                    {
                        var marcacao = new tblQuestao_Marcacao
                        {
                            intQuestaoID = QuestaoId,
                            intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO),
                            intClientID = ClientID,
                            dtAnotacao = DateTime.Now,
                            bitFlagFavorita = Favorita
                        };
                        ctx.tblQuestao_Marcacao.Add(marcacao);
                    }
                    else
                    {
                        mcc.bitFlagFavorita = Favorita;
                    }
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }

                    return 1;
                }
            }
        }

        public int SetDuvidaQuestaoSimulado(int QuestaoId, int ClientID, bool Duvida)
        {
            try
            {
                using(MiniProfiler.Current.Step("Gravando dúvida questão simulado"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.SIMULADO))
                            return 0;

                        var mcc = (from m in ctx.tblQuestao_Marcacao
                                join qs in ctx.tblQuestao_Simulado on m.intQuestaoID equals qs.intQuestaoID
                                where m.intClientID == ClientID && m.intQuestaoID == QuestaoId
                                orderby m.dtAnotacao descending
                                select m).FirstOrDefault();

                        if (mcc == null)
                            ctx.tblQuestao_Marcacao.Add(new tblQuestao_Marcacao
                            {
                                intQuestaoID = QuestaoId,
                                intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO),
                                intClientID = ClientID,
                                dtAnotacao = DateTime.Now,
                                bitFlagEmDuvida = Duvida
                            });

                        else
                            mcc.bitFlagEmDuvida = Duvida;

                        ctx.SaveChanges();
                        return 1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetRespostaAluno(Int32 QuestaoID, Int32 ExercicioID, Int32 ExercicioTipo, Int32 ApplicationID, Int32 ClientID, String LetraAlternativa, String Resposta, Exercicio.tipoExercicio ExercicioTipoId)
        {
            using (var ctx = new AcademicoContext())
            {
                var exercicio = new ExercicioEntity();
                Int32 HistoricoID = exercicio.GetHistoricoID(ExercicioID, ExercicioTipo, ApplicationID, ClientID);

                if (!isQuestao(QuestaoID, Exercicio.tipoExercicio.SIMULADO))
                    return 0;

                tblCartaoResposta_Discursiva questao = (from q in ctx.tblCartaoResposta_Discursiva
                                                            join a in ctx.tblQuestaoAlternativas on q.intDicursivaId equals a.intAlternativaID
                                                            where q.intQuestaoDiscursivaID == QuestaoID
                                                                  && q.intHistoricoExercicioID == HistoricoID
                                                                  && a.txtLetraAlternativa == LetraAlternativa
                                                            select q).FirstOrDefault();

                if (questao == null && Resposta != "")
                {

                    var queryDiscursiva = from alt in ctx.tblQuestaoAlternativas where alt.txtLetraAlternativa == LetraAlternativa && alt.intQuestaoID == QuestaoID select alt;
                    var queryDiscursivaId = queryDiscursiva.ToList();
                    if (queryDiscursivaId.Count() > 0)
                    {
                        int intDiscursivaId = (queryDiscursivaId).FirstOrDefault().intAlternativaID;

                        var newQuestao = new tblCartaoResposta_Discursiva
                        {
                            intQuestaoDiscursivaID = QuestaoID,
                            intHistoricoExercicioID = HistoricoID,
                            txtResposta = Resposta,
                            intDicursivaId = intDiscursivaId,
                            intExercicioTipoId = Convert.ToInt32(ExercicioTipoId),
                            dteCadastro = DateTime.Now
                        };

                        ctx.tblCartaoResposta_Discursiva.Add(newQuestao);
                        ctx.SaveChanges();
                    }

                }
                else
                {
                    if (Resposta != "")
                    {
                        questao.dteCadastro = DateTime.Now;
                        questao.txtResposta = Resposta;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        if (questao != null)
                        {
                            ctx.tblCartaoResposta_Discursiva.Remove(questao);
                            ctx.SaveChanges();
                        }
                    }
                }

                return 1;
            }
        }

        public int SetRespostaAluno(Int32 QuestaoID, Int32 ExercicioID, Int32 ExercicioTipo, Int32 ApplicationID, Int32 ClientID, String LetraAlternativa, bool Selecionada, Exercicio.tipoExercicio ExercicioTipoId)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var exercicio = new ExercicioEntity();
                    Int32 HistoricoID = exercicio.GetHistoricoID(ExercicioID, ExercicioTipo, ApplicationID, ClientID);

                    if (!isQuestao(QuestaoID, ExercicioTipoId))
                        return 0;

                    var questao = (from q in ctx.tblCartaoResposta_objetiva where q.intQuestaoID == QuestaoID && q.intExercicioTipoId == ExercicioTipo && q.intHistoricoExercicioID == HistoricoID select q).FirstOrDefault();

                    if (questao == null)
                    {
                        if (Selecionada)
                        {
                            var newQuestao2 = new tblCartaoResposta_objetiva
                            {
                                intQuestaoID = QuestaoID,
                                intHistoricoExercicioID = HistoricoID,
                                txtLetraAlternativa = LetraAlternativa,
                                guidQuestao = (from qst in ctx.tblQuestoes where qst.intQuestaoID == QuestaoID select qst.guidQuestaoID).FirstOrDefault(),
                                intExercicioTipoId = ExercicioTipo
                            };

                            var newQuestao = new tblCartaoResposta_objetiva();
                            newQuestao.intQuestaoID = QuestaoID;
                            newQuestao.intHistoricoExercicioID = HistoricoID;
                            newQuestao.txtLetraAlternativa = LetraAlternativa;
                            newQuestao.intExercicioTipoId = ExercicioTipo;
                            newQuestao.dteCadastro = DateTime.Now;
                            if (ExercicioTipo == 1)
                                newQuestao.guidQuestao = (from qst in ctx.tblQuestoes where qst.intQuestaoID == QuestaoID select qst.guidQuestaoID).FirstOrDefault();
                            else
                                newQuestao.guidQuestao = (from qst in ctxMatDir.tblConcursoQuestoes where qst.intQuestaoID == QuestaoID select qst.guidQuestaoID).FirstOrDefault();

                            ctx.tblCartaoResposta_objetiva.Add(newQuestao);
                        }
                    }
                    else
                    {
                        if (Selecionada)
                        {
                            questao.txtLetraAlternativa = LetraAlternativa;
                            questao.dteCadastro = DateTime.Now;
                        }
                        else
                            ctx.tblCartaoResposta_objetiva.Remove(questao);
                    }

                    ctx.SaveChanges();
                }
            }

            return 1;
        }

        public int GetQuestaoSemMarcacao(int clientId)
        {
            using (var ctx = new AcademicoContext())
            {
                var questoesMarcadas = ctx.tblQuestao_Marcacao.Where(q => q.intClientID == clientId).Select(x => x.intQuestaoID).ToList();

                tblConcursoQuestoes questaoNaoMarcada = null;
                using (var ctxMatDir = new DesenvContext())
                {
                    questaoNaoMarcada = ctxMatDir.tblConcursoQuestoes.Where(q => !questoesMarcadas.Contains(q.intQuestaoID)).OrderByDescending(x => x.intQuestaoID).FirstOrDefault();
                }

                return questaoNaoMarcada.intQuestaoID;

            }
        }

        public int GetQuestaoSimuladoSemMarcacao(int clientId)
        {
            using (var ctx = new AcademicoContext())
            {
                var questoesMarcadas = ctx.tblQuestao_Marcacao.Where(q => q.intClientID == clientId && q.intTipoExercicioID == (int)Exercicio.tipoExercicio.SIMULADO).Select(x => x.intQuestaoID).ToList();

                var questaoNaoMarcada = ctx.tblQuestao_Simulado.Where(x => !questoesMarcadas.Contains(x.intQuestaoID)).FirstOrDefault();

                return questaoNaoMarcada.intQuestaoID;

            }
        }

        public int SetDuvida(QuestaoDuvida questaoDuvida)
        {
            using(MiniProfiler.Current.Step("Salvando duvida de questão em concurso"))
            {
                using (var ctx = new DesenvContext())
                {
                    var retorno = 0;
                    try
                    {
                        if (!string.IsNullOrEmpty(questaoDuvida.TextoPergunta))
                        {
                            var duvida = new tblQuestao_Duvida();
                            duvida.intQuestaoId = questaoDuvida.Questao.Id;
                            duvida.intTipoExercicioId = questaoDuvida.Questao.ExercicioTipoID;
                            duvida.intClientId = questaoDuvida.Aluno.ID;
                            duvida.txtPergunta = questaoDuvida.TextoPergunta;
                            duvida.dtePergunta = DateTime.Now;
                            duvida.intApplicationId = questaoDuvida.AplicacaoId;
                            duvida.bitActive = true;

                            ctx.tblQuestao_Duvida.Add(duvida);
                            ctx.SaveChanges();

                            SetAdminDuvidaEncaminhar(new QuestaoDuvidaEncaminhamento()
                            {
                                QuestaoDuvidaID = duvida.intQuestaoDuvidaId,
                                TipoExercicioID = questaoDuvida.Questao.ExercicioTipoID,
                                QuestaoID = questaoDuvida.Questao.Id
                            });

                            retorno = 1;

                        }
                        return retorno;
                    }
                    catch (Exception)
                    {
                        return retorno;
                    }
                }
            }

        }

        public int SetAdminDuvidaEncaminhar(QuestaoDuvidaEncaminhamento aRegistro)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    if (aRegistro.DestinatarioID == 0)
                    {
                        if (aRegistro.TipoExercicioID == 1)
                        {
                            using (var ctxAcad = new AcademicoContext())
                            {
                                int? intEmployeeComentarioID = (from q in ctxAcad.tblQuestoes
                                                                where q.intQuestaoID == aRegistro.QuestaoID
                                                                select q.intEmployeeComentarioID).FirstOrDefault();

                                var professor = (from e in ctx.tblEmployees
                                                 where intEmployeeComentarioID == e.intEmployeeID
                                                 select new
                                                 {
                                                     Id = e.bitActiveEmployee.Value ? intEmployeeComentarioID :
                                                                    !e.bitActiveEmployee.Value && e.intGestorID != null ? e.intGestorID :
                                                                        0
                                                 }).DefaultIfEmpty().FirstOrDefault();

                                aRegistro.DestinatarioID = professor != null ? professor.Id.Value : 0;
                            }
                        }

                        if (aRegistro.TipoExercicioID == 2)
                        {
                            var professor = (from q in ctx.tblConcursoQuestoes
                                             join e1 in ctx.tblEmployees on q.intEmployeeComentarioID equals e1.intEmployeeID into e2
                                             from e in e2.DefaultIfEmpty()
                                             where q.intQuestaoID == aRegistro.QuestaoID
                                             select new
                                             {
                                                 Id = q.intEmployeeComentarioID == null ? 0 :
                                                            e.bitActiveEmployee.Value ? q.intEmployeeComentarioID :
                                                                !e.bitActiveEmployee.Value && e.intGestorID != null ? e.intGestorID :
                                                                    0
                                             }).FirstOrDefault();
                            aRegistro.DestinatarioID = professor != null ? professor.Id.Value : 0;
                        }
                    }

                    if (aRegistro.DestinatarioID > 0)
                    {
                        var encaminhamentoAnterior = ctx.tblQuestao_Duvida_Encaminhamento.FirstOrDefault(q => q.intQuestaoDuvidaID == aRegistro.QuestaoDuvidaID && q.bitAtivo);

                        if (encaminhamentoAnterior != null)
                        {
                            encaminhamentoAnterior.bitAtivo = false;
                        }

                        var encaminhamento = new tblQuestao_Duvida_Encaminhamento()
                        {
                            intQuestaoDuvidaID = aRegistro.QuestaoDuvidaID,
                            intRemetenteID = (aRegistro.RemetenteID.HasValue && (aRegistro.RemetenteID.Value == 0)) ? null : aRegistro.RemetenteID,
                            intDestinatarioID = aRegistro.DestinatarioID,
                            dteEncaminhamento = DateTime.Now,
                            bitAtivo = true
                        };

                        ctx.tblQuestao_Duvida_Encaminhamento.Add(encaminhamento);
                        ctx.SaveChanges();
                    }

                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<QuestaoDuvida> GetQuestoesDuvida(QuestaoDuvida questaoDuvida)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var listaQuestaoDuvida = new List<QuestaoDuvida>();

                    var lista = (from q in ctx.tblQuestao_Duvida
                                 join dm in ctx.tblQuestao_Duvida_Moderada on q.intQuestaoDuvidaId equals dm.intQuestaoDuvidaId
                                 into dm1
                                 from dm2 in dm1.DefaultIfEmpty()
                                 join p in ctx.tblPersons on q.intClientId equals p.intContactID
                                 join c in ctx.tblCities on p.intCityID equals c.intCityID
                                 join s in ctx.tblStates on c.intState equals s.intStateID
                                 where (q.intQuestaoId == questaoDuvida.Questao.Id
                                        && q.intTipoExercicioId == questaoDuvida.Questao.ExercicioTipoID
                                        && q.intClientId == questaoDuvida.Aluno.ID
                                        && q.intTipoExercicioId == questaoDuvida.Questao.ExercicioTipoID
                                        && q.intQuestaoId == questaoDuvida.Questao.Id)


                                 select new
                                 {
                                     Id = q.intQuestaoDuvidaId,
                                     QuestaoId = q.intQuestaoId,
                                     AlunoId = q.intClientId,
                                     AlunoNome = p.txtName,
                                     AlunoUF = s.txtCaption,
                                     txtPergunta = q.txtPergunta,
                                     dtePergunta = q.dtePergunta,
                                     txtResposta = q.txtResposta,
                                     dteResposta = q.dteResposta,
                                     bitActive = q.bitActive
                                 }).ToList();


                    var professorId = 0;
                    var professorNome = String.Empty;
                    if (questaoDuvida.Questao.ExercicioTipoID == 1)
                    {
                        using (var ctxAcad = new AcademicoContext())
                        {
                            var professor = ctxAcad.tblQuestoes.Where(q => q.intQuestaoID == questaoDuvida.Questao.Id);
                            if (professor.Any(p => p.intEmployeeComentarioID != null))
                            {
                                professorId = Convert.ToInt32(professor.FirstOrDefault().intEmployeeComentarioID);
                                professorNome = ctx.tblPersons.Where(p => p.intContactID == professorId).FirstOrDefault().txtName;
                            }
                        }
                    }

                    if (questaoDuvida.Questao.ExercicioTipoID == 2)
                    {
                        var professor = ctx.tblConcursoQuestoes.Where(c => c.intQuestaoID == questaoDuvida.Questao.Id);
                        if (professor.Any(p => p.intEmployeeComentarioID != null))
                        {
                            professorId = Convert.ToInt32(professor.FirstOrDefault().intEmployeeComentarioID);
                            professorNome = ctx.tblPersons.Where(p => p.intContactID == professorId).FirstOrDefault().txtName;
                        }
                    }




                    foreach (var questao in lista)
                    {
                        var item = new QuestaoDuvida();

                        item.Id = questaoDuvida.Id;
                        item.Questao = new Questao { Id = questao.QuestaoId };
                        item.Aluno = new Aluno { ID = questao.AlunoId, Nome = questao.AlunoNome.Trim(), Uf = questao.AlunoUF };
                        item.TextoPergunta = questao.txtPergunta;
                        item.DataPergunta = Utilidades.DateTimeToUnixTimestamp(questao.dtePergunta ?? DateTime.MinValue);
                        item.Professor = new Pessoa { ID = professorId, Nome = professorNome.Trim() };
                        item.TextoResposta = questao.txtResposta;
                        item.DataResposta = questaoDuvida.DataResposta != null ? Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(questao.dteResposta)) : questaoDuvida.DataResposta;
                        item.AplicacaoId = questaoDuvida.AplicacaoId;
                        item.Ativo = questao.bitActive;


                        listaQuestaoDuvida.Add(item);
                    }



                    return listaQuestaoDuvida;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task EnvioEmailComentarioForumPosAsync(int idQuestao, int matricula, string comentario, bool voto)
        {
            var tipo = QuestaoRecurso.TipoForumRecurso.Pos;
            var votoAluno = Convert.ToInt32(voto);

            return Task.Factory.StartNew(() => envioEmailComentarioForumPos((int)tipo, new ForumQuestaoRecurso.Pos
            {
                Comentarios = new List<ForumQuestaoRecurso.ForumComentarioRecurso>
                {
                    new ForumQuestaoRecurso.ForumComentarioRecurso
                    {
                        ComentarioTexto = comentario,
                        Questao = new Questao { Id = idQuestao },
                        Matricula = matricula,
                        Opiniao = votoAluno.ToString()
                    }
                }
            }));
        }

        private void envioEmailComentarioForumPos(int tipoForum, ForumQuestaoRecurso.Pos forum)
        {
            using (var ctx = new DesenvContext())
            {
                var e = new Email();
                var mailTo = new StringBuilder();
                var producao = ConfigurationProvider.Get("Settings:enviaEmailParaAluno");
                var emailHomol = ConfigurationProvider.Get("Settings:emailDesenv");
                var strConteudo = new StringBuilder();
                var questaoId = Convert.ToInt32(forum.Comentarios[0].Questao.Id);
                var matricula = Convert.ToInt32(forum.Comentarios[0].Matricula);
                var questao = GetQuestaoConcurso(questaoId);
                var escolha = "";

                var alternativasCorretas = GetAlternativasCorretas(questaoId);

                questao.Prova = new Prova()
                {
                    ID = Convert.ToInt32((from qc in ctx.tblConcursoQuestoes where qc.intQuestaoID == questaoId select qc.intProvaID).FirstOrDefault())
                };

                var concurso = (from p in ctx.tblConcurso_Provas
                                join c in ctx.tblConcursos on p.ID_CONCURSO equals c.intConcursoID
                                where p.intProvaID == questao.Prova.ID
                                select new Concurso()
                                {
                                    Nome = p.txtName,
                                    Sigla = c.txtSigla
                                }
                ).FirstOrDefault();


                var nomeAluno = (from p in ctx.tblPersons
                                 where p.intContactID == matricula
                                 select p.txtName).FirstOrDefault();

                if (Convert.ToBoolean(Convert.ToInt32(forum.Comentarios[0].Opiniao)))
                {
                    escolha = "SIM";
                }
                else
                {
                    escolha = "NÃO";
                }

                mailTo.Append("forum.recursos@medgrupo.com.br,mmarinho@medgrupo.com.br,marcio@medgrupo.com.br");

                var emailProfessor =
                (from q in ctx.tblConcursoQuestoes
                 join prof in ctx.tblPersons on q.intEmployeeID equals prof.intContactID
                 where q.intQuestaoID == questaoId
                 select prof.txtEmail1).FirstOrDefault();

                if (!String.IsNullOrEmpty(emailProfessor))
                {
                    mailTo.Append(", ");
                    mailTo.Append(emailProfessor);
                }

                strConteudo.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                strConteudo.Append("<style type=\"text/css\">a { color: #777} td { font:15px \"Trebuchet MS\";margin-left:15px;} </style></head>");
                strConteudo.Append("<body style=\"background:#fff; color:#000; font:14px 'Trebuchet MS';\"><h1 style=\" font-size: 20px;\">");
                strConteudo.Append("<img src=\"http://recursos.medgrupo.com.br/images/logo.jpg\" alt=\"Medgrupo\" /></h1>");
                strConteudo.AppendFormat("<h2 style=\" font-size: 12px; color: #777\">Solicitação / Comentário de Recursos ({0})</h2>", DateTime.Now);
                strConteudo.Append("<table cellspacing=\"0\" cellpadding=\"4\">");
                strConteudo.AppendFormat("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\" colspan=\"2\"><strong>Concorda?</strong> {0}</td></tr>", escolha);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Aluno</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0} - Matrícula {1}</td></tr>", nomeAluno, matricula);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Concurso</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0} - {1}</td></tr><tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\">", concurso.Sigla, concurso.Nome);
                strConteudo.AppendFormat("<strong>Questão</strong></td><td valign=\"top\">{0} (ID: {1})", questao.Ordem, questao.Id);
                strConteudo.AppendFormat(" - {0}</td></tr><tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Gabarito</strong></td>", questao.Enunciado);
                strConteudo.AppendFormat("<td valign=\"top\">{0} - {1}</td></tr>", alternativasCorretas[0].Letra, alternativasCorretas[0].Nome);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Justificativa</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0}</td></tr></table>", forum.Comentarios[0].ComentarioTexto);
                strConteudo.Append("<a href=\"http://ctrlpanel2.medgrupo.com.br/\" style=\"\" font-size: 15px;\"><br>Administrar</a>");
                strConteudo.Append("</div><p style=\"font-size: 11px;\"><strong>Medgrupo Participa&ccedil;&otilde;es</strong><br />+55 (21) 2131-0031</p></body></html>");

                if (producao == "SIM")
                {
                    e.mailTo = mailTo.ToString();
                }
                else
                {
                    e.mailTo = emailHomol;
                }

                e.mailSubject = "Comentário sobre veredito";
                e.mailBody = strConteudo.ToString();
                Utilidades.SendMailDirect(e.mailTo, e.mailBody, e.mailSubject, "Extensivo");
            }
        }

        public Questao GetQuestaoConcurso(int idQuestao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var questao = new Questao();

                    var queryQuestao = (from cq in ctx.tblConcursoQuestoes
                                        where cq.intQuestaoID == idQuestao
                                        select new Questao
                                        {
                                            Id = cq.intQuestaoID,
                                            Enunciado = cq.txtEnunciado,
                                            Comentario = cq.txtComentario,
                                            Ordem = cq.intOrder.Value
                                        }).FirstOrDefault();

                    if (queryQuestao == null)
                    {
                        var retorno = new Questao();
                        return retorno;
                    }


                    var queryEspecialidades = (from cqc in ctx.tblConcursoQuestao_Classificacao
                                               join cqcc in ctx.tblConcursoQuestaoCatologoDeClassificacoes on cqc.intClassificacaoID equals cqcc.intClassificacaoID
                                               where (new[] { 2, 3, 9 }).Contains(cqc.intTipoDeClassificacao) && cqc.intQuestaoID == idQuestao
                                               select new Especialidade
                                               {
                                                   Id = cqc.intClassificacaoID,
                                                   Descricao = cqcc.intParent != null ? string.Concat(cqcc.txtTipoDeClassificacao, " - ", cqcc.txtSubTipoDeClassificacao) : cqcc.txtSubTipoDeClassificacao,
                                               });

                    var queryImagens = (from img in ctx.tblQuestaoConcurso_Imagem
                                        where img.intQuestaoID == idQuestao
                                        select new PPQuestaoImagem
                                        {
                                            Id = img.intID,
                                            Url = Constants.URLIMAGEMQUESTAO.Replace("IDQUESTAOIMAGEM", img.intID.ToString().Trim())
                                        });

                    var queryAlternativas = from qa in ctx.tblConcursoQuestoes_Alternativas
                                            where qa.intQuestaoID == idQuestao
                                            select new Alternativa
                                            {
                                                LetraStr = qa.txtLetraAlternativa,
                                                Nome = qa.txtAlternativa,
                                                Correta = qa.bitCorreta ?? false
                                            };

                    if (queryQuestao != null)
                    {
                        VideoEntity videoEntity = new VideoEntity();
                        List<Video> videos = videoEntity.GetVideoQuestaoConcurso(queryQuestao.Id);
                        if (videos.Any())
                            questao.VideoQuestao = new Video { Url = videos[0].Url };

                        questao.Id = queryQuestao.Id;
                        questao.Enunciado = queryQuestao.Enunciado;
                        questao.Comentario = queryQuestao.Comentario;
                        questao.Ordem = queryQuestao.Ordem;
                        questao.Especialidades = new List<Especialidade>(queryEspecialidades);
                        questao.Imagens = new List<PPQuestaoImagem>(queryImagens);
                        questao.Alternativas = new List<Alternativa>(queryAlternativas);
                    }

                    return questao;
                }
            }
            catch
            {
                throw;
            }
        }

        private List<Alternativa> GetAlternativasCorretas(int idQuestao)
        {
            using (var ctx = new DesenvContext())
            {
                var retorno = new List<Alternativa>();
                var questao = ctx.tblConcursoQuestoes.Where(q => q.intQuestaoID == idQuestao).FirstOrDefault();
                var gabaritoPosLiberado = questao.bitGabaritoPosRecursoLiberado ?? false;
                var semGabaritoOficial = questao.bitSemGabarito;

                var alternativa = ctx.tblConcursoQuestoes_Alternativas.Where(a => a.intQuestaoID == idQuestao
                                                                                  && ((gabaritoPosLiberado && !semGabaritoOficial && (a.bitCorreta ?? false)) || (a.bitCorretaPreliminar ?? false)));

                foreach (var alt in alternativa)
                    retorno.Add(new Alternativa
                    {
                        Letra = Convert.ToChar(alt.txtLetraAlternativa),
                        Id = alt.intAlternativaID,
                        Nome = alt.txtAlternativa
                    });

                return retorno;
            }
        }

        private void envioEmailComentarioForumPre(int tipoForum, ForumQuestaoRecurso.Pre forum)
        {
            using (var ctx = new DesenvContext())
            {
                var e = new Email();
                var mailTo = new StringBuilder();
                
                
                
                var producao = ConfigurationProvider.Get("Settings:enviaEmailParaAluno");
                var emailHomol = ConfigurationProvider.Get("Settings:emailDesenv");
                var strConteudo = new StringBuilder();
                var questaoId = Convert.ToInt32(forum.Comentarios[0].Questao.Id);
                var matricula = Convert.ToInt32(forum.Comentarios[0].Matricula);
                var questao = GetQuestaoConcurso(questaoId);
                var escolha = "";

                var alternativasCorretas = GetAlternativasCorretas(questaoId);

                questao.Prova = new Prova()
                {
                    ID = Convert.ToInt32((from qc in ctx.tblConcursoQuestoes where qc.intQuestaoID == questaoId select qc.intProvaID).FirstOrDefault())
                };

                var concurso = (from p in ctx.tblConcurso_Provas
                                join c in ctx.tblConcursos on p.ID_CONCURSO equals c.intConcursoID
                                where p.intProvaID == questao.Prova.ID
                                select new Concurso()
                                {
                                    Nome = p.txtName,
                                    Sigla = c.txtSigla
                                }
                ).FirstOrDefault();

                var nomeAluno = (from p in ctx.tblPersons
                                 where p.intContactID == matricula
                                 select p.txtName).FirstOrDefault();

                if (Convert.ToBoolean(Convert.ToInt32(forum.Comentarios[0].Opiniao)))
                {
                    escolha = "SIM";
                }
                else
                {
                    escolha = "NÃO";
                }

                mailTo.Append("forum.recursos@medgrupo.com.br");

                var emailProfessor =
                (from q in ctx.tblConcursoQuestoes
                 join prof in ctx.tblPersons on q.intEmployeeID equals prof.intContactID
                 where q.intQuestaoID == questaoId
                 select prof.txtEmail1).FirstOrDefault();

                if (!String.IsNullOrEmpty(emailProfessor))
                {
                    mailTo.Append(", ");
                    mailTo.Append(emailProfessor);
                }

                strConteudo.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                strConteudo.Append("<style type=\"text/css\">a { color: #777} td { font:15px \"Trebuchet MS\";margin-left:15px;} </style></head>");
                strConteudo.Append("<body style=\"background:#fff; color:#000; font:14px 'Trebuchet MS';\"><h1 style=\" font-size: 20px;\">");
                strConteudo.Append("<img src=\"http://recursos.medgrupo.com.br/images/logo.jpg\" alt=\"Medgrupo\" /></h1>");
                strConteudo.AppendFormat("<h2 style=\" font-size: 12px; color: #777\">Solicitação / Comentário de Recursos ({0})</h2>", DateTime.Now);
                strConteudo.Append("<table cellspacing=\"0\" cellpadding=\"4\">");
                strConteudo.AppendFormat("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\" colspan=\"2\"><strong>Concorda?</strong> {0}</td></tr>", escolha);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Aluno</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0} - Matrícula {1}</td></tr>", nomeAluno, matricula);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Concurso</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0} - {1}</td></tr><tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\">", concurso.Sigla, concurso.Nome);
                strConteudo.AppendFormat("<strong>Questão</strong></td><td valign=\"top\">{0} (ID: {1})", questao.Ordem, questao.Id);
                strConteudo.AppendFormat(" - {0}</td></tr><tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Gabarito</strong></td>", questao.Enunciado);
                strConteudo.AppendFormat("<td valign=\"top\">{0} - {1}</td></tr>", alternativasCorretas[0].Letra, alternativasCorretas[0].Nome);
                strConteudo.Append("<tr><td align=\"left\" valign=\"top\" nowrap=\"nowrap\"><strong>Justificativa</strong></td>");
                strConteudo.AppendFormat("<td valign=\"top\">{0}</td></tr></table>", forum.Comentarios[0].ComentarioTexto);
                strConteudo.Append("<a href=\"http://ctrlpanel2.medgrupo.com.br/\" style=\"\" font-size: 15px;\"><br>Administrar</a>");
                strConteudo.Append("</div><p style=\"font-size: 11px;\"><strong>Medgrupo Participa&ccedil;&otilde;es</strong><br />+55 (21) 2131-0031</p></body></html>");

                if (producao == "SIM")
                {
                    e.mailTo = mailTo.ToString();
                }
                else
                {
                    e.mailTo = emailHomol;
                }
                e.mailSubject = "Solicitação de Recursos";
                e.mailBody = strConteudo.ToString();
                Utilidades.SendMailDirect(e.mailTo, e.mailBody, e.mailSubject, "Extensivo");

            }
        }

        public int EnviarVotoComentarioForum(int idQuestao, int matricula, string votoAluno, string texto, QuestaoRecurso.TipoForumRecurso tipoForum)
        {
            var ds = new DBQuery().ExecuteStoredProcedure(
                "msp_RecursoAlunoComentario",
                    new SqlParameter[] {
                        new SqlParameter("intQuestaoID", idQuestao),
                        new SqlParameter("intClientID", matricula),
                        new SqlParameter("intTipo", (int)tipoForum),
                        new SqlParameter("txtRecurso_Comentario", texto),
                        new SqlParameter("bitActive", 1),
                        new SqlParameter("bitOpiniao", votoAluno == QuestaoRecurso.StatusOpiniao.Favor.GetDescription()),
                        new SqlParameter("op", 1)
                });

            return (ds != null && ds.Tables != null) ? 1 : 0;
        }

        public RecursoQuestaoConcursoDTO GetQuestaoConcursoRecurso(int idQuestao, int matricula)
        {
            RecursoQuestaoConcursoDTO questaoRecurso = null;
            using (var ctx = new DesenvContext())
            {
                questaoRecurso = (
                       from cq in ctx.tblConcursoQuestoes
                       join prova in ctx.tblConcurso_Provas on cq.intProvaID equals prova.intProvaID
                       join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                       join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                       from p in ctx.tblPersons.Where(p => p.intContactID == cq.intEmployeeID).DefaultIfEmpty()
                       from cc in ctx.tblConcurso_ProvaCasoClinico.Where(c => c.intCasoClinicoID == cq.intCasoClinicoID).DefaultIfEmpty()
                       where cq.intQuestaoID == idQuestao
                       select new RecursoQuestaoConcursoDTO
                       {
                           Questao = new QuestaoConcursoRecursoDTO
                           {
                               Id = cq.intQuestaoID,
                               Enunciado = cq.txtEnunciadoConcursoSiteRecursos,
                               Numero = cq.intOrder ?? 0,
                               Anulada = cq.bitAnulada,
                               AnuladaPosRecurso = cq.bitAnuladaPosRecurso,
                               Discursiva = cq.bitDiscursiva == true,
                               CasoClinico = cc != null ? cc.txtTexto : null
                           },
                           Prova = new DTO.ProvaConcursoDTO
                           {
                               IdProva = prova.intProvaID,
                               Ano = concurso.VL_ANO_CONCURSO,
                               Tipo = prova.txtTipoProva,
                               DataRecursoAte = (prova.dteExpiracao ?? concurso.PRAZO_RECURSO_ATE),
                               PainelAviso = prova.txtDescription,
                               Nome = concurso.SG_CONCURSO + Constants.DASH + concurso.CD_UF,
                               RMais = tipo.txtDescription.StartsWith(Constants.R3) || tipo.txtDescription.StartsWith(Constants.R4),
                               StatusProva = prova.ID_CONCURSO_RECURSO_STATUS
                           },
                           Concurso = new ConcursoDTO
                           {
                               Sigla = concurso.SG_CONCURSO,
                               SiglaEstado = concurso.CD_UF,
                               Descricao = concurso.NM_CONCURSO
                           },
                           ForumRecurso = new ForumRecursoDTO
                           {
                               ExisteAnaliseProfessor = cq.bitComentarioAtivo ?? false,
                               JustificativaBanca = cq.txtComentario_banca_recurso,
                               ForumPreAnalise = new ForumPreRecursoDTO
                               {
                                   TextoAnaliseProfessor = cq.txtRecurso,
                                   Professor = new ProfessorDTO
                                   {
                                       Id = p != null ? p.intContactID : 0,
                                       Nome = p != null ? p.txtName : null
                                   }
                               },
                               ForumPosAnalise = new ForumPosRecursoDTO
                               {
                                   Oculto = tipo.txtDescription.StartsWith(Constants.R3) || tipo.txtDescription.StartsWith(Constants.R4)
                               },
                               IdAnaliseProfessorStatus = cq.ID_CONCURSO_RECURSO_STATUS,
                               IdRecursoStatusBanca = cq.intStatus_Banca_Recurso
                           }
                       }).FirstOrDefault();

                questaoRecurso.Prova.DataFinalRecurso = questaoRecurso.Prova.DataRecursoAte.HasValue
                    ? questaoRecurso.Prova.DataRecursoAte.Value.ToString("dd/MM/yyyy") : string.Empty;

                var idProfessor = questaoRecurso.ForumRecurso.ForumPreAnalise.Professor.Id;
                if (idProfessor != default(int))
                {
                    questaoRecurso.ForumRecurso.ForumPreAnalise.Professor.UrlAvatar = string.Concat(
                        Constants.URLDIRETORIOAVATARPROFESSOR, idProfessor, ".jpg"
                        );

                    var nome = Utilidades.GetNomeResumido(questaoRecurso.ForumRecurso.ForumPreAnalise.Professor.Nome);
                    questaoRecurso.ForumRecurso.ForumPreAnalise.Professor.Nome = nome;
                }
            }
            return questaoRecurso;
        }

        public int UpdateQuestaoConcurso(tblConcursoQuestoes questao)
        {
            using (var ctx = new DesenvContext())
            {
                ctx.tblConcursoQuestoes.Add(questao);
                ctx.Entry(questao).State = EntityState.Modified;
                return ctx.SaveChanges();
            }
        }

        public tblConcursoQuestoes GetQuestaoConcursoById(int idQuestao)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcursoQuestoes.SingleOrDefault(q => q.intQuestaoID == idQuestao);
            }
        }

        public bool IsQuestaoProvaRMais(int idQuestao)
        {
            using (var ctx = new DesenvContext())
            {
                return (from cq in ctx.tblConcursoQuestoes
                        join prova in ctx.tblConcurso_Provas on cq.intProvaID equals prova.intProvaID
                        join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                        where cq.intQuestaoID == idQuestao && 
                            (tipo.txtDescription.StartsWith(Constants.R3) || tipo.txtDescription.StartsWith(Constants.R4))
                        select cq.intQuestaoID).Any();
            }
        }

        public Task EnvioEmailComentarioForumPreAsync(int idQuestao, int matricula, string comentario, bool voto)
        {
            var tipo = QuestaoRecurso.TipoForumRecurso.Pre;
            var votoAluno = Convert.ToInt32(voto);

            return Task.Factory.StartNew(() => envioEmailComentarioForumPre((int)tipo, new ForumQuestaoRecurso.Pre
            {
                Comentarios = new List<ForumQuestaoRecurso.ForumComentarioRecurso>
                {
                    new ForumQuestaoRecurso.ForumComentarioRecurso
                    {
                        ComentarioTexto = comentario,
                        Questao = new Questao { Id = idQuestao },
                        Matricula = matricula,
                        Opiniao = votoAluno.ToString()
                    }
                }
            }));
        }

        public tblConcursoQuestoes_Aluno_Favoritas GetConcursoQuestoesAlunoFavorita(int idQuestao, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcursoQuestoes_Aluno_Favoritas.FirstOrDefault(
                    f => f.intQuestaoID == idQuestao && f.intClientID == matricula
                    );
            }
        }

        public IEnumerable<AlternativaQuestaoConcursoDTO> ObterAlternativasComEstatisticaFavorita(int idQuestao)
        {
            var alternativaBase = ObterAlternativasQuestaoConcurso(idQuestao);
            IEnumerable<AlternativaQuestaoConcursoDTO> alternativaLista = null;

            if (alternativaBase != null)
            {
                alternativaBase = alternativaBase.Where(
                    a => !string.IsNullOrEmpty(a.txtAlternativa) || !string.IsNullOrEmpty(a.txtImagemOtimizada ?? a.txtImagem)
                    ).ToList();

                alternativaLista = alternativaBase.Select(a => new AlternativaQuestaoConcursoDTO
                {
                    IdAlternativa = a.intAlternativaID,
                    Descricao = a.txtAlternativa,
                    Letra = a.txtLetraAlternativa,
                    CorretaOficial = a.bitCorreta ?? false,
                    CorretaPreliminar = a.bitCorretaPreliminar ?? false,
                    UrlImagem = !string.IsNullOrEmpty(a.txtImagemOtimizada ?? a.txtImagem)
                                    ? Constants.STATIC_MEDGRUPO + (a.txtImagem ?? a.txtImagemOtimizada) : null
                }).ToList();
            }

            if (alternativaLista != null)
            {
                using (var ctx = new DesenvContext())
                {
                    var group = (from f in ctx.tblConcursoQuestoes_Aluno_Favoritas
                                 where f.intQuestaoID == idQuestao
                                 group f by new { f.intQuestaoID, f.charResposta } into g
                                 select new { Key = g.Key.charResposta, Count = g.Count() });

                    foreach (var g in group)
                    {
                        var existe = alternativaLista.Any(a => a.Letra == g.Key);

                        if (existe)
                        {
                            alternativaLista.First(a => a.Letra == g.Key).QtdResponderam = g.Count;
                        }
                    }
                }
            }
            return alternativaLista;
        }

        public int InserirQuestaoConcursoAlunoFavoritas(tblConcursoQuestoes_Aluno_Favoritas alternativa)
        {
            using (var ctx = new DesenvContext())
            {
                ctx.tblConcursoQuestoes_Aluno_Favoritas.Add(alternativa);
                return ctx.SaveChanges();
            }
        }

        public Questao GetGabaritoDiscursiva(int idQuestao, string enunciado = "")
        {
            using (var ctx = new DesenvContext())
            {
                var resposta = string.Empty;
                var alternativa = ctx.tblConcursoQuestoes_Alternativas.Where(a => a.txtLetraAlternativa == "A" && a.intQuestaoID == idQuestao).FirstOrDefault();

                if(alternativa != null)
                {
                    resposta = alternativa.txtResposta;
                }
                
                var enunciadoCompleto = ctx.tblConcursoQuestoes.Where(c => c.intQuestaoID == idQuestao).FirstOrDefault().txtEnunciado.Trim();

                var splitEnunciado = string.IsNullOrEmpty(enunciado) ? Regex.Split(enunciadoCompleto, "gabarito", RegexOptions.IgnoreCase) : Regex.Split(enunciado, "gabarito", RegexOptions.IgnoreCase);
                var gabEnunciado = splitEnunciado.Count() > 1 ? String.Concat("Gabarito", String.Join("Gabarito", splitEnunciado.Where(i => i != splitEnunciado[0]))) : string.Empty;

                var enun = splitEnunciado[0];

                var questao = new Questao();
                questao.Enunciado = enun;
                questao.Id = idQuestao;
                questao.GabaritoDiscursiva = string.IsNullOrEmpty(resposta) ? gabEnunciado : resposta.Trim();

                return questao;
            }
        }

        public QuestaoConcursoAlternativaFavoritaDTO GetAlternativaFavoritaQuestaoConcurso(int idQuestao, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from f in ctx.tblConcursoQuestoes_Aluno_Favoritas
                        where f.intClientID == matricula && f.intQuestaoID == idQuestao
                        select new QuestaoConcursoAlternativaFavoritaDTO
                        {
                            IdQuestao = f.intQuestaoID,
                            LetraAlternativaSelecionada = f.charResposta,
                            LetraUltimaAlternativaSelecionada = f.charRespostaNova,
                            Data = f.dteDate
                        }).OrderByDescending(a => a.Data)
                        .FirstOrDefault();
            }
        }

        public int UpdateQuestoesConcursoAlunoFavoritas(tblConcursoQuestoes_Aluno_Favoritas alternativa)
        {
            using (var ctx = new DesenvContext())
            {
                var alternativaBase = ctx.tblConcursoQuestoes_Aluno_Favoritas.FirstOrDefault(
                    f => f.intClientID == alternativa.intClientID && f.intQuestaoID == alternativa.intQuestaoID
                    );

                if (alternativaBase != null)
                {
                    alternativaBase.charResposta = alternativa.charResposta;
                    alternativaBase.charRespostaNova = alternativa.charRespostaNova;
                    alternativaBase.bitActive = alternativa.bitActive;
                    alternativaBase.bitDuvida = alternativa.bitDuvida;
                    alternativaBase.bitVideo = alternativa.bitVideo;
                    alternativaBase.bitResultadoResposta = alternativa.bitResultadoResposta;
                    ctx.Entry(alternativaBase).State = EntityState.Modified;
                }
                return alternativaBase != null ? ctx.SaveChanges() : 0;
            }
        }

        public IEnumerable<tblConcursoQuestoes_recursosComentario_Imagens> ObterImagensComentarioRecurso(int idQuestao)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcursoQuestoes_recursosComentario_Imagens.Where(
                    a => a.intQuestao == idQuestao
                    ).ToList();
            }
        }

        public IEnumerable<ForumComentarioDTO> ObterComentariosForumConcursoPre(int idQuestao, int matricula)
        {
            var comentarioPre = GetComentariosModeradosAlunosForumConcurso(idQuestao, matricula, QuestaoRecurso.TipoForumRecurso.Pre);

            if (comentarioPre != null)
            {
                foreach (var opiniao in comentarioPre)
                {
                    var status = opiniao.Afirma ? QuestaoRecurso.StatusOpiniao.Favor : QuestaoRecurso.StatusOpiniao.Contra;
                    opiniao.VotoAluno = status.GetDescription();

                    opiniao.UrlAvatar = DefinirUrlAvatarComentarioForum(opiniao);
                }
            }
            return comentarioPre;
        }

        public string DefinirUrlAvatarComentarioForum(ForumComentarioDTO opiniao)
        {
            var url = Constants.LINK_STATIC_AVATAR_PADRAO;

            if (!string.IsNullOrEmpty(opiniao.PathPerfil))
            {
                url = Constants.LINK_STATIC_FOTOS + opiniao.PathPerfil;
            }
            else if (!string.IsNullOrEmpty(opiniao.PathAvatar))
            {
                url = Constants.LINK_STATIC_AVATARES + opiniao.PathAvatar;
            }
            return url;
        }

        public IEnumerable<ForumComentarioDTO> ObterComentariosForumConcursoPos(int idQuestao, int matricula)
        {
            var comentarioPos = GetComentariosModeradosAlunosForumConcurso(idQuestao, matricula, QuestaoRecurso.TipoForumRecurso.Pos);

            if (comentarioPos != null)
            {
                foreach (var opiniao in comentarioPos)
                {
                    var status = opiniao.Afirma ? QuestaoRecurso.StatusOpiniao.Favor : QuestaoRecurso.StatusOpiniao.Contra;
                    opiniao.VotoAluno = status.GetDescription();

                    opiniao.UrlAvatar = DefinirUrlAvatarComentarioForum(opiniao);
                }
            }
            return comentarioPos;
        }

        private List<ForumComentarioDTO> GetComentariosModeradosAlunosForumConcurso(int idQuestao, int matricula, QuestaoRecurso.TipoForumRecurso tipo)
        {
            List<ForumComentarioDTO> comentariosForum = null;
            var setorProfessor = (int)Pessoa.EnumTipoPessoa.Professor;

            using (var ctx = new DesenvContext())
            {
                using (var ctxAcad = new AcademicoContext())
                {
                    var comentariosConcurso = (from ca in ctx.tblConcurso_Recurso_Aluno
                                               join p in ctx.tblPersons on ca.intContactID equals p.intContactID
                                               from e in ctx.tblEmployee_Sector.Where(
                                                   es => ca.intContactID == es.intEmployeeID && es.intSectorID == setorProfessor
                                                   ).DefaultIfEmpty()
                                               from c in ctx.tblClients.Where(c => c.intClientID == ca.intContactID).DefaultIfEmpty()
                                               from pic in ctx.tblPersonsPicture.Where(
                                                   x => p.intContactID == x.intContactID && x.bitActive.Value && x.intPictureTypeID == 1
                                                   ).DefaultIfEmpty()
                                               from pa in ctx.tblPersonsAvatar.Where(
                                                  z => p.intContactID == z.intContactID && z.bitActive.Value
                                                  ).DefaultIfEmpty()
                                               from a in ctx.tblAvatar.Where(
                                                   v => pa.intAvatarID == v.intAvatarID && v.bitActive.Value
                                                   ).DefaultIfEmpty()
                                               where ca.intQuestaoID == idQuestao && ca.intTipo == (int)tipo
                                                   && (ca.bitActive == true || ca.intContactID == matricula)
                                                   && e == null
                                               select new
                                               {
                                                   Nome = p.txtName,
                                                   Matricula = ca.intContactID,
                                                   Texto = ca.txtRecurso_Comentario,
                                                   DataComentario = ca.dteCadastro,
                                                   Opiniao = ca.bitOpiniao,
                                                   Picture = pic.txtPicturePath,
                                                   Avatar = a.txtAvatarPath,
                                                   intEspecialidadeId = c.intEspecialidadeID
                                               }).ToList();

                    List<int?> listaEspecialidadeId = comentariosConcurso.Select(x => x.intEspecialidadeId).ToList();

                    var especialidade = (from e in ctxAcad.tblEspecialidades
                                         where listaEspecialidadeId.Contains(e.intEspecialidadeID)
                                         select new
                                         {
                                             e.intEspecialidadeID,
                                             e.DE_ESPECIALIDADE
                                         }).ToList();

                    var comentarios = (from c in comentariosConcurso
                                       from e in especialidade.Where(x => x.intEspecialidadeID == c.intEspecialidadeId).DefaultIfEmpty()
                                       select new
                                       {
                                           Nome = c.Nome,
                                           Matricula = c.Matricula,
                                           Texto = c.Texto,
                                           DataComentario = c.DataComentario,
                                           Opiniao = c.Opiniao,
                                           Especialidade = e.DE_ESPECIALIDADE,
                                           Picture = c.Picture,
                                           Avatar = c.Avatar
                                       }).ToList();


                    if (comentarios != null)
                    {
                        comentariosForum = comentarios.Select(c => new ForumComentarioDTO
                        {
                            Nome = Utilidades.GetNomeResumido(c.Nome),
                            Matricula = c.Matricula,
                            Texto = Utilidades.RemoveHtmlETrocaBrPorQuebraDeLinha(c.Texto),
                            Especialidade = c.Especialidade,
                            DataDecorrida = Utilidades.GetTempoDecorridoPorExtenso(c.DataComentario),
                            DataInclusao = c.DataComentario,
                            Afirma = c.Opiniao.HasValue && c.Opiniao.Value,
                            Autor = matricula == c.Matricula,
                            PathAvatar = c.Avatar != null ? c.Avatar.Trim() : null,
                            PathPerfil = c.Picture != null ? c.Picture.Trim() : null
                        }).OrderByDescending(c => c.DataInclusao).ToList();
                    }
                }
            }

            return comentariosForum;
        }

        public IEnumerable<ForumComentarioDTO> ObterComentarioForumPosProfessor(int idQuestao)
        {
            List<ForumComentarioDTO> comentarios = null;
            using (var ctx = new DesenvContext())
            {
                var result = (from c in ctx.tblConcurso_Recurso_MEDGRUPO
                              join p in ctx.tblPersons on c.intContactID equals p.intContactID
                              where c.bitActive == true && c.intQuestaoID == idQuestao
                               && c.IdCoordenador.HasValue && c.IdCoordenador.Value > 0
                               && c.intTipo == (int)QuestaoRecurso.TipoForumRecurso.Pos
                              select new
                              {
                                  Nome = p.txtName,
                                  Matricula = c.intContactID,
                                  Texto = c.txtRecurso_Comentario,
                                  DataInclusao = c.dteCadastro,
                                  EncerraForum = c.bitSelarForum
                              }).ToList();

                if(result != null && result.Any())
                {
                    comentarios = result.Select(c => new ForumComentarioDTO
                    {
                        Nome = Utilidades.GetNomeResumido(c.Nome),
                        Matricula = c.Matricula ?? 0,
                        Texto = Utilidades.RemoveHtmlETrocaBrPorQuebraDeLinha(c.Texto),
                        DataDecorrida = Utilidades.GetTempoDecorridoPorExtenso(c.DataInclusao ?? DateTime.MinValue),
                        DataInclusao = c.DataInclusao ?? DateTime.MinValue,
                        EncerraForum = c.EncerraForum,
                        Professor = true,
                        Afirma = true
                    }).ToList();
                }
                return comentarios;
            }
        }

        public string GetEmailEnvioAnaliseQuestaoAluno(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from a in ctx.tblPersons
                        where a.intContactID == matricula
                        select a.txtEmail1 ?? a.txtEmail2 ?? a.txtEmail3)
                        .FirstOrDefault();
            }
        }

        public List<RecursoQuestaoConcursoDTO> GetQuestoesProvaConcurso(int idProva)
        {
            List<RecursoQuestaoConcursoDTO> questoes = null;

            using (var ctx = new DesenvContext())
            {
                questoes = (
                       from cq in ctx.tblConcursoQuestoes
                       join prova in ctx.tblConcurso_Provas on cq.intProvaID equals prova.intProvaID
                       join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                       join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                       where prova.intProvaID == idProva
                       select new RecursoQuestaoConcursoDTO
                       {
                           Prova = new DTO.ProvaConcursoDTO
                           {
                               Nome = concurso.CD_UF + Constants.DASH + concurso.SG_CONCURSO,
                               NomeCompleto = concurso.NM_CONCURSO,
                               Ano = concurso.VL_ANO_CONCURSO,
                               Tipo = prova.txtTipoProva,
                               DataRecursoAte = (prova.dteExpiracao ?? concurso.PRAZO_RECURSO_ATE),
                               PainelAviso = prova.txtDescription,
                               PainelAvisoTitulo = prova.txtTituloPainelAvisos,
                               DataLimiteComunicado = prova.dteLightboxExpirationDate,
                               Comunicado = prova.txtLightBox,
                               ComunicadoAtivo = prova.bitActiveLightBox == true,
                               StatusProva = prova.ID_CONCURSO_RECURSO_STATUS,
                               RankLiberado = prova.bitRecursoForumAcertosLiberado == true,
                               RMais = tipo.txtDescription.StartsWith(Constants.R3) || tipo.txtDescription.StartsWith(Constants.R4),
                               QtdQuestoes = prova.intRecursoForumAcertosQtdQuestoes ?? 0
                           },
                           Questao = new QuestaoConcursoRecursoDTO
                           {
                               Id = cq.intQuestaoID,
                               Enunciado = cq.txtEnunciado,
                               Numero = cq.intOrder ?? 0,
                               Anulada = cq.bitAnulada,
                               AnuladaPosRecurso = cq.bitAnuladaPosRecurso
                           },
                           ForumRecurso = new ForumRecursoDTO
                           {
                               ExisteAnaliseProfessor = cq.bitComentarioAtivo ?? false,
                               JustificativaBanca = cq.txtComentario_banca_recurso,
                               IdAnaliseProfessorStatus = cq.ID_CONCURSO_RECURSO_STATUS,
                               IdRecursoStatusBanca = cq.intStatus_Banca_Recurso
                           }
                       }).ToList();

                if(questoes == null)
                {
                    questoes = new List<RecursoQuestaoConcursoDTO>();
                }

                questoes.ForEach(q =>
                {
                    q.Prova.DataFinalRecurso = q.Prova.DataRecursoAte.HasValue && q.Prova.DataRecursoAte != Constants.SqlDefaultDateTimeValue
                        ? q.Prova.DataRecursoAte.Value.ToString("dd/MM/yyyy") : string.Empty;
                    q.Prova.PainelAviso = Utilidades.RemoveHtmlETrocaBrPorQuebraDeLinha(q.Prova.PainelAviso);
                });
            }
            return questoes;
        }

        public int DesabilitaAcertosQuestaoConcurso(int matricula, int idProva, int idEmployee)
        {
            var qtd = default(int);
            using (var ctx = new DesenvContext())
            {
                var notas = ctx.tblConcurso_Provas_Acertos.Where(
                    a => a.intContactID == matricula && a.intProvaID == idProva
                    ).ToList();

                if(notas != null)
                {
                    notas.ForEach(x =>
                    {
                        x.bitActive = false;
                        ctx.Entry(x).State = EntityState.Modified;
                    });
                    qtd = ctx.SaveChanges();
                }
            }

            if(qtd > default(int))
            {
                LogOperacoesConcurso.InserirLogAsync(
                    TipoOperacoesConcursoEnum.RemoveuRankingAcerto, idEmployee: idEmployee, dados: new object[] { matricula, idProva }
                    );
            }
            return qtd;
        }

        public ProvaConcursoDTO GetProvaConcurso(int idProva)
        {
            using (var ctx = new DesenvContext())
            {
                return (from prova in ctx.tblConcurso_Provas
                        join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                        join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                        where prova.intProvaID == idProva
                        select new DTO.ProvaConcursoDTO
                        {
                            Nome = concurso.CD_UF + Constants.DASH + concurso.SG_CONCURSO,
                            NomeCompleto = concurso.NM_CONCURSO,
                            Ano = concurso.VL_ANO_CONCURSO,
                            Tipo = prova.txtTipoProva,
                            DataRecursoAte = (prova.dteExpiracao ?? concurso.PRAZO_RECURSO_ATE),
                            PainelAviso = prova.txtDescription,
                            PainelAvisoTitulo = prova.txtTituloPainelAvisos,
                            DataLimiteComunicado = prova.dteLightboxExpirationDate,
                            Comunicado = prova.txtLightBox,
                            ComunicadoAtivo = prova.bitActiveLightBox.HasValue && prova.bitActiveLightBox.Value,
                            StatusProva = prova.ID_CONCURSO_RECURSO_STATUS,
                            RankLiberado = prova.bitRecursoForumAcertosLiberado == true,
                            RMais = tipo.txtDescription.StartsWith(Constants.R3) || tipo.txtDescription.StartsWith(Constants.R4),
                            QtdQuestoes = prova.intRecursoForumAcertosQtdQuestoes ?? 0
                        }).FirstOrDefault();
            }
        }

        public bool AlunoTemRankAcertos(int idProva, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from f in ctx.tblConcurso_Provas_Acertos
                        where f.bitActive == true && f.intProvaID == idProva
                            && f.intContactID == matricula
                        select f.intProvaAcertosID).Any();
            }
        }

        public bool AlunoSelecionouAlternativaQuestaoProva(int idProva, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from f in ctx.tblConcursoQuestoes_Aluno_Favoritas
                        join q in ctx.tblConcursoQuestoes on f.intQuestaoID equals q.intQuestaoID
                        where f.intClientID == matricula && q.intProvaID == idProva
                        select f.intQuestaoID).Any();
            }
        }

        public bool AlunoViuAvisoComentarioRecurso(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblProvaAlunoConfiguracoes.Any(
                    c => c.intContactID == matricula && c.bitVisualizouModalRecurso == true
                    );
            }
        }

        public bool AlunoComunicadoHabilitado(int idProva, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return !ctx.tblProvaAlunoConfiguracoes.Where(
                    c => c.intContactID == matricula && c.intProvaID == idProva && c.bitComunicadoAtivo == false
                    ).Any();
            }
        }

        public ProvaRecursoLive GetLiveProva(int idProva)
        {
            using (var ctx = new DesenvContext())
            {
                return (from a in ctx.tblConcurso_ProvasLivesRecurso
                        where a.intProvaID == idProva
                        orderby a.dteData descending
                        select new ProvaRecursoLive
                        {
                            UrlLive = a.txtUrl,
                            DataLive = a.dteData,
                            ExibirLiveRecursos = a.bitExibirFacebookLivePortalRecursos
                        }).FirstOrDefault();
            }
        }

        public bool AlunoJaVotouForumQuestao(int idQuestao, int matricula, QuestaoRecurso.TipoForumRecurso tipoForum)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcurso_Recurso_Aluno.Any(
                    a => a.intQuestaoID == idQuestao && a.intContactID == matricula
                    && a.intTipo == (int)tipoForum
                    );
            }
        }

        public List<QuestaoConcursoVotosDTO> GetQtdCabeQuestoesConcurso(params int[] idQuestaoList)
        {
            var tipoForum = (int)QuestaoRecurso.TipoForumRecurso.Pre;
            using (var ctx = new DesenvContext())
            {
                return (from c in ctx.tblConcurso_Recurso_Aluno
                        where c.intTipo == tipoForum
                            && c.bitOpiniao == true
                            && idQuestaoList.Contains(c.intQuestaoID)
                        group c by c.intQuestaoID into g
                        select new QuestaoConcursoVotosDTO
                        {
                            IdQuestao = g.Key,
                            QtdCabeRecurso = g.Count()
                        }).ToList();
            }
        }

        public int SetRespostaRecursos(QuestaoRecursosPost questao)
        {
            try
            {
                var parametros = new SqlParameter[] {
                                                        new SqlParameter("idQuestao", questao.IdQuestao),
                                                        new SqlParameter("idCliente", questao.Matricula),
                                                        new SqlParameter("charResposta", questao.AlternativaSelecionada),
                                                        new SqlParameter("bitResultadoResposta", questao.AlternativaCorreta)
                                                    };

                var ds = new DBQuery().ExecuteStoredProcedure("emed_insert_questoes_resposta_favoritas_v2", parametros);
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int SetComentarioForumQuestaoPre(ForumQuestaoRecurso.Pre forumPre)
        {
            try
            {
                var op = 1;
                var tipoForumPre = 1;
                using (var ctx = new DesenvContext())
                {
                    var questaoId = forumPre.Comentarios[0].Questao.Id;

                    if (ctx.tblConcursoQuestoes
                        .Where(q => q.intQuestaoID == questaoId)
                        .Select(q => q.ID_CONCURSO_RECURSO_STATUS)
                        .Any(q => q == 8 || q == 0 || q == -1))
                        op = 4;
                }
                var parametros = new SqlParameter[] {
                                                        new SqlParameter("@op", op),
                                                        new SqlParameter("@intQuestaoID", forumPre.Comentarios[0].Questao.Id),
                                                        new SqlParameter("@intClientID", forumPre.Comentarios[0].Matricula),
                                                        new SqlParameter("@txtRecurso_Comentario", forumPre.Comentarios[0].ComentarioTexto),
                                                        new SqlParameter("@Recupera_IP", forumPre.Comentarios[0].Ip),
                                                        new SqlParameter("@bitActive", 1),
                                                        new SqlParameter("@bitOpiniao", Convert.ToBoolean(Convert.ToInt32(forumPre.Comentarios[0].Opiniao))),
                                                        new SqlParameter("@intTipo", 1),
                                                        new SqlParameter("@bitIsModerada", 0)
                                                    };
                var ds = new DBQuery().ExecuteStoredProcedure("msp_RecursoAlunoComentario", parametros);

                envioEmailComentarioForumPre(tipoForumPre, forumPre);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int SetComentarioForumQuestaoPos(ForumQuestaoRecurso.Pos forumPos)
        {
            try
            {
                var op = 1;
                var tipoForumPos = 2;
                using (var ctx = new DesenvContext())
                {
                    var questaoId = forumPos.Comentarios[0].Questao.Id;

                    if (ctx.tblConcursoQuestoes
                        .Where(q => q.intQuestaoID == questaoId)
                        .Select(q => q.ID_CONCURSO_RECURSO_STATUS)
                        .Any(q => q == 8 || q == 0))
                        op = 4;
                }
                var parametros = new SqlParameter[] {
                                                        new SqlParameter("@op", op),
                                                        new SqlParameter("@intQuestaoID", forumPos.Comentarios[0].Questao.Id),
                                                        new SqlParameter("@intClientID", forumPos.Comentarios[0].Matricula),
                                                        new SqlParameter("@txtRecurso_Comentario", forumPos.Comentarios[0].ComentarioTexto),
                                                        new SqlParameter("@Recupera_IP", forumPos.Comentarios[0].Ip),
                                                        new SqlParameter("@bitActive", 1),
                                                        new SqlParameter("@bitOpiniao", Convert.ToBoolean(Convert.ToInt32(forumPos.Comentarios[0].Opiniao))),
                                                        new SqlParameter("@intTipo", 2),
                                                        new SqlParameter("@bitIsModerada", 0)
                                                    };
                var ds = new DBQuery().ExecuteStoredProcedure("msp_RecursoAlunoComentario", parametros);

                envioEmailComentarioForumPos(tipoForumPos, forumPos);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public List<string> GetDuvidaQuestaoUrlsImagens(int idDuvida)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var retornoImagens = new List<string>();

                    var imagens = ctx.tblQuestao_Duvida_Imagem.Where(i => i.intQuestaoDuvidaId == idDuvida).ToList();
                    if (imagens.Any())
                    {
                        var hostS3 = Constants.DOMAINS3.Replace("BUCKET", Constants.DUVIDAQUESTAOBUCKET);
                        var accessKey = Constants.ACCESSKEY;
                        var secretKey = Constants.SECRETKEY;
                        var r = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 3600) / 60.0;
                        var expires = Math.Round(r) * 60;
                        var enc = Encoding.ASCII;
                        var encUtf = Encoding.UTF8;
                        var hmac = new HMACSHA1(encUtf.GetBytes(secretKey));
                        hmac.Initialize();
                        var VERB = "GET";

                        var url = string.Empty;

                        foreach (var imagem in imagens)
                        {
                            url = Path.Combine(hostS3, String.Concat(imagem.txtNomeImagem, ".", Constants.FORMATOIMAGEM));
                            var resource = (new Uri(url)).AbsolutePath;
                            var host = (new Uri(url)).AbsoluteUri.Replace(resource, "");
                            var signatureString = String.Concat(VERB, "\n", "\n", "\n", expires, "\n", resource);
                            var buffer = encUtf.GetBytes(signatureString);
                            var signature = System.Convert.ToBase64String(hmac.ComputeHash(buffer));

                            var urlImagem = String.Concat(host, resource, "?AWSAccessKeyId=", accessKey, "&Expires=", expires, "&Signature=", HttpUtility.UrlEncode(signature));

                            retornoImagens.Add(urlImagem);
                        }
                    }

                    return retornoImagens;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetDuvidaResposta(QuestaoDuvida questaoDuvida)
        {
            using (var ctx = new DesenvContext())
            {
                var retorno = 0;
                try
                {
                    var valoresValidos = (!string.IsNullOrEmpty(questaoDuvida.TextoResposta) &&
                                          questaoDuvida.Id > 0);

                    if (valoresValidos)
                    {
                        var encaminhamento = ctx.tblQuestao_Duvida_Encaminhamento
                            .Where(e => e.intQuestaoDuvidaID == questaoDuvida.Id
                                        && e.bitAtivo).ToList();

                        if (encaminhamento.Any())
                        {
                            var idsEncaminhamento = encaminhamento.Select(e => e.intQuestaoDuvidaEncaminhamentoID);
                            ctx.tblQuestao_Duvida_Resposta
                                .Where(r => idsEncaminhamento.Contains(r.intEncaminhamentoId))
                                .ToList()
                                .ForEach(r => r.bitActive = false);

                            var encaminha = encaminhamento
                                .OrderByDescending(e => e.intQuestaoDuvidaEncaminhamentoID)
                                .Take(1)
                                .FirstOrDefault();
                            ctx.tblQuestao_Duvida_Resposta.Add(new tblQuestao_Duvida_Resposta
                            {
                                intEncaminhamentoId = encaminha.intQuestaoDuvidaEncaminhamentoID,
                                txtResposta = questaoDuvida.TextoResposta,
                                bitActive = true,
                                dteResposta = DateTime.Now
                            });

                            ctx.SaveChanges();
                            retorno = 1;
                        }
                    }
                    return retorno;
                }
                catch (Exception)
                {
                    return retorno;
                }
            }
        }

        public List<QuestaoDuvida> GetQuestaoDuvidas(QuestaoDuvida questaoDuvida, int duvidaIdScroll)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var totalDuvidasLista = 20;
                    var listaQuestaoDuvida = new List<QuestaoDuvida>();


                    var todasDuvidas = (from d in ctx.tblQuestao_Duvida
                                        join p in ctx.tblPersons on d.intClientId equals p.intContactID
                                        join c in ctx.tblCities on p.intCityID equals c.intCityID
                                        join s in ctx.tblStates on c.intState equals s.intStateID
                                        where d.intQuestaoId == questaoDuvida.Questao.Id && d.intTipoExercicioId == questaoDuvida.Questao.ExercicioTipoID
                                        select new { idDuvida = d.intQuestaoDuvidaId, matricula = d.intClientId, nomeAluno = p.txtName, pergunta = d.txtPergunta, dtPergunta = d.dtePergunta, alunoUf = s.txtCaption, }).ToList().OrderByDescending(d => d.dtPergunta);

                    var lstAvatares = new List<Avatar>();
                    foreach (var matricula in todasDuvidas.Select(m => m.matricula).Distinct().ToList())
                        lstAvatares.Add(new ClienteEntity().GetClienteAvatar(matricula));


                    var todasRespostasDuvidas = (from e in ctx.tblQuestao_Duvida_Encaminhamento
                                                 join p in ctx.tblPersons on e.intDestinatarioID equals p.intContactID
                                                 join d in ctx.tblQuestao_Duvida on e.intQuestaoDuvidaID equals d.intQuestaoDuvidaId
                                                 join r in ctx.tblQuestao_Duvida_Resposta on e.intQuestaoDuvidaEncaminhamentoID equals r.intEncaminhamentoId
                                                 where e.bitAtivo && r.bitActive
                                                       && d.intQuestaoId == questaoDuvida.Questao.Id && d.intTipoExercicioId == questaoDuvida.Questao.ExercicioTipoID
                                                 select new
                                                 {
                                                     idDuvida = d.intQuestaoDuvidaId,
                                                     reposta = r.txtResposta,
                                                     dataResposta = r.dteResposta,
                                                     profId = e.intDestinatarioID,
                                                     prof = p.txtName.Trim(),
                                                     idEncaminhamento = e.intQuestaoDuvidaEncaminhamentoID
                                                 }).ToList();

                    var lstAvataresProf = new List<Avatar>();
                    foreach (var matricula in todasRespostasDuvidas.Where(p => p.profId != 0).Select(m => m.profId).Distinct().ToList())
                        lstAvataresProf.Add(new ClienteEntity().GetClienteAvatar(matricula));

                    var duvidasModeradas = (from m in ctx.tblQuestao_Duvida_Moderada
                                            join d in ctx.tblQuestao_Duvida on m.intQuestaoDuvidaId equals d.intQuestaoDuvidaId
                                            where d.intQuestaoId == questaoDuvida.Questao.Id && d.intTipoExercicioId == questaoDuvida.Questao.ExercicioTipoID
                                            select new { idDuvida = m.intQuestaoDuvidaId, dataModeracao = m.dteCadastro, ativo = m.bitActive }).ToList();

                    var avatar = new Avatar();
                    var avatarProf = new Avatar();

                    foreach (var duvida in todasDuvidas)
                    {
                        avatar = lstAvatares.FirstOrDefault(a => a.Matricula == duvida.matricula);
                        var duvidaCompleta = new QuestaoDuvida
                        {
                            Id = duvida.idDuvida,
                            TextoPergunta = duvida.pergunta,
                            Ativo = true,
                            AplicacaoId = questaoDuvida.AplicacaoId,
                            Questao = new Questao { Id = questaoDuvida.Questao.Id },
                            DataPergunta = Utilidades.ToUnixTimespan(Convert.ToDateTime(duvida.dtPergunta)),
                            Aluno = new Aluno
                            {
                                ID = duvida.matricula,
                                Nome = duvida.matricula == questaoDuvida.Aluno.ID ? "Mim" : duvida.nomeAluno.Trim(),
                                Foto = (string.IsNullOrEmpty(avatar.CaminhoImagemPadrao) ? avatar.Caminho : avatar.CaminhoImagemPadrao),
                                Uf = duvida.alunoUf
                            }
                        };

                        if (duvida.matricula == questaoDuvida.Aluno.ID)
                        {
                            var resposta = todasRespostasDuvidas.Where(r => r.idDuvida == duvida.idDuvida);
                            if (resposta.Any())
                            {
                                avatarProf = lstAvataresProf.FirstOrDefault(p => p.Matricula == resposta.FirstOrDefault().profId);

                                duvidaCompleta.TextoResposta = resposta.FirstOrDefault().reposta;
                                duvidaCompleta.DataResposta = Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(resposta.FirstOrDefault().dataResposta));
                                duvidaCompleta.Professor = new Pessoa
                                {
                                    Nome = resposta.FirstOrDefault().prof,
                                    Foto = string.IsNullOrEmpty(avatarProf.Caminho)
                                                                   ? avatarProf.CaminhoImagemPadrao : avatar.Caminho
                                };
                            }

                            listaQuestaoDuvida.Add(duvidaCompleta);
                        }
                        else
                        {
                            var moderacoesDuvida = duvidasModeradas.Where(m => m.idDuvida == duvida.idDuvida).ToList();

                            if (moderacoesDuvida.Any())
                            {

                                if (moderacoesDuvida.OrderByDescending(m => m.dataModeracao).Take(1).FirstOrDefault().ativo)
                                {
                                    var resposta = todasRespostasDuvidas.Where(r => r.idDuvida == duvida.idDuvida).ToList();
                                    if (resposta.Any())
                                    {
                                        avatarProf = lstAvataresProf.FirstOrDefault(p => p.Matricula == resposta.FirstOrDefault().profId);
                                        duvidaCompleta.TextoResposta = resposta.FirstOrDefault().reposta;
                                        duvidaCompleta.DataResposta = Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(resposta.FirstOrDefault().dataResposta));
                                        duvidaCompleta.Professor = new Pessoa
                                        {
                                            Nome = resposta.FirstOrDefault().prof,
                                            Foto = string.IsNullOrEmpty(avatarProf.Caminho)
                                                                           ? avatarProf.CaminhoImagemPadrao : avatar.Caminho
                                        };
                                    }

                                    listaQuestaoDuvida.Add(duvidaCompleta);
                                }
                            }
                        }
                    }

                    if (duvidaIdScroll > 0)
                        listaQuestaoDuvida = listaQuestaoDuvida.Where(l => l.Id < duvidaIdScroll).Take(totalDuvidasLista).ToList();

                    return listaQuestaoDuvida.Take(totalDuvidasLista).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestaoDuvidaDTO> GetAdminListaDuvida(ParamQuestaoDuvida aParam)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var professor = (from e in ctx.tblQuestao_Duvida_Encaminhamento
                                     join p in ctx.tblPersons on e.intDestinatarioID equals p.intContactID
                                     where e.bitAtivo
                                     select new { idEncaminhamento = e.intQuestaoDuvidaEncaminhamentoID, idQuestaoDuvida = e.intQuestaoDuvidaID, Nome = p.txtName, IdProf = p.intContactID });

                    var moderado = (from m in ctx.tblQuestao_Duvida_Moderada
                                    group m by m.intQuestaoDuvidaId into g
                                    select new { intQuestaoDuvidaId = g.Key, MaxData = g.Max(m => m.dteCadastro) });

                    var Ultimo_moderado = (from d in ctx.tblQuestao_Duvida_Moderada
                                           join u in moderado on d.intQuestaoDuvidaId equals u.intQuestaoDuvidaId
                                           where (d.dteCadastro == u.MaxData)
                                           select d);

                    var moderador = (from u in Ultimo_moderado
                                     join p in ctx.tblPersons on u.intEmployeeId equals p.intContactID
                                     select new { idQuestaoDuvida = u.intQuestaoDuvidaId, Data = u.dteCadastro, Nome = p.txtName, IdModerador = p.intContactID, Aprovado = u.bitActive });

                    var lista = (from d in ctx.tblQuestao_Duvida
                                 join p in ctx.tblPersons on d.intClientId equals p.intContactID
                                 join prof1 in professor on d.intQuestaoDuvidaId equals prof1.idQuestaoDuvida into prof2
                                 from prof in prof2.DefaultIfEmpty()
                                 join res1 in ctx.tblQuestao_Duvida_Resposta on prof.idEncaminhamento equals res1.intEncaminhamentoId into res2
                                 from res in res2.DefaultIfEmpty()
                                 join mod1 in moderador on d.intQuestaoDuvidaId equals mod1.idQuestaoDuvida into mod2
                                 from mod in mod2.DefaultIfEmpty()
                                 where (d.intQuestaoId == aParam.QuestaoDuvida.IdQuestao || aParam.QuestaoDuvida.IdQuestao == 0) &&
                                       (d.intClientId == aParam.QuestaoDuvida.IdCliente || aParam.QuestaoDuvida.IdCliente == 0) &&
                                       (d.intTipoExercicioId == aParam.QuestaoDuvida.IdTipoExercicio || aParam.QuestaoDuvida.IdTipoExercicio == 0)
                                 select new QuestaoDuvidaDTO
                                 {
                                     Id = d.intQuestaoDuvidaId,
                                     IdQuestao = d.intQuestaoId,
                                     IdTipoExercicio = d.intTipoExercicioId,
                                     IdCliente = p.intContactID,
                                     Cliente = p.txtName.Trim(),
                                     Pergunta = d.txtPergunta,
                                     PerguntaData = d.dtePergunta,
                                     Respondido = true,
                                     Resposta = res.txtResposta,
                                     RespostaData = res.dteResposta,
                                     Ativo = d.bitActive,
                                     Moderado = true,
                                     IdModerador = mod.IdModerador,
                                     Moderador = mod.Nome.Trim(),
                                     ModeradorData = mod.Data,
                                     Encaminhado = true,
                                     IdProfessor = prof.IdProf,
                                     Professor = prof.Nome.Trim(),
                                     Aprovado = mod.Aprovado,
                                     Lida = ctx.tblQuestao_Duvida_Lida.Any(l => l.intEmployeeID == aParam.UsuarioLogado && l.intQuestaoDuvidaID == d.intQuestaoDuvidaId && l.bitLido),
                                     Encaminhamentos = (from e in ctx.tblQuestao_Duvida_Encaminhamento
                                                        where e.intQuestaoDuvidaID == d.intQuestaoDuvidaId
                                                        join rem1 in ctx.tblPersons on e.intRemetenteID equals rem1.intContactID into rem2
                                                        from rem in rem2.DefaultIfEmpty()
                                                        join dest in ctx.tblPersons on e.intDestinatarioID equals dest.intContactID
                                                        orderby e.dteEncaminhamento descending
                                                        select new QuestaoDuvidaEncaminhamento
                                                        {
                                                            Id = e.intQuestaoDuvidaEncaminhamentoID,
                                                            QuestaoDuvidaID = e.intQuestaoDuvidaID,
                                                            RemetenteID = e.intRemetenteID,
                                                            RemetenteNome = rem.txtName.Trim(),
                                                            DestinatarioID = e.intDestinatarioID,
                                                            DestinatarioNome = dest.txtName.Trim(),
                                                            DataDuvidaEncaminhada = e.dteEncaminhamento,
                                                            Ativo = e.bitAtivo
                                                        })
                                 });

                    return GetAdminDuvidasFiltradas(ctx, lista, aParam);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<QuestaoDuvidaDTO> GetAdminDuvidasFiltradas(DesenvContext ctx, IQueryable<QuestaoDuvidaDTO> aLista, ParamQuestaoDuvida aParam)
        {
            try
            {
                bool usuarioMaster = (aParam.UsuarioPerfil == EnumTipoPerfil.Master);
                bool usuarioCoordenador = (aParam.UsuarioPerfil == EnumTipoPerfil.Coordenador);

                var listaSubordinados = (from e in ctx.tblEmployees where e.intGestorID == aParam.UsuarioLogado && e.bitActiveEmployee.Value && usuarioCoordenador select e.intEmployeeID);

                switch (aParam.TipoDuvida)
                {
                    case EnumQuestaoDuvidaTipo.NaoModerados:
                        aLista = aLista.Where(l => !l.Moderado && l.Ativo && (usuarioMaster || l.IdProfessor == aParam.UsuarioLogado || listaSubordinados.Contains(l.IdProfessor.Value)));
                        break;

                    case EnumQuestaoDuvidaTipo.Moderados:
                        aLista = aLista.Where(l => l.Moderado && l.Ativo && (usuarioMaster || l.IdProfessor == aParam.UsuarioLogado || listaSubordinados.Contains(l.IdProfessor.Value)));
                        break;

                    case EnumQuestaoDuvidaTipo.ModeradosPorMim:
                        aLista = aLista.Where(l => l.IdModerador == aParam.UsuarioLogado && l.Ativo);
                        break;

                    case EnumQuestaoDuvidaTipo.NaoRespondidas:
                        aLista = aLista.Where(l => l.Resposta == null && l.Ativo && (usuarioMaster || l.IdProfessor == aParam.UsuarioLogado || listaSubordinados.Contains(l.IdProfessor.Value)));
                        break;

                    case EnumQuestaoDuvidaTipo.Respondidas:
                        aLista = aLista.Where(l => l.Respondido && l.Ativo && (usuarioMaster || l.IdProfessor == aParam.UsuarioLogado || listaSubordinados.Contains(l.IdProfessor.Value)));
                        break;

                    case EnumQuestaoDuvidaTipo.RespondidasPorMim:
                        aLista = aLista.Where(l => l.Respondido && l.Ativo && l.IdProfessor == aParam.UsuarioLogado);
                        break;

                    case EnumQuestaoDuvidaTipo.NaoEncaminhadas:
                        aLista = aLista.Where(l => !l.Encaminhado && l.Ativo && (usuarioMaster || usuarioCoordenador));
                        break;

                    case EnumQuestaoDuvidaTipo.Encaminhadas:
                        aLista = aLista.Where(l => l.Encaminhado && l.Ativo && !l.Moderado && (usuarioMaster || l.IdProfessor == aParam.UsuarioLogado || listaSubordinados.Contains(l.IdProfessor.Value)));
                        break;

                    case EnumQuestaoDuvidaTipo.EncaminhadosParaMim:
                        aLista = aLista.Where(l => !l.Moderado && l.Ativo && (l.IdProfessor == aParam.UsuarioLogado));
                        break;

                    case EnumQuestaoDuvidaTipo.Descartadas:
                        aLista = aLista.Where(l => !l.Ativo);
                        break;

                    case EnumQuestaoDuvidaTipo.Todos:
                        aLista = aLista.Where(l => l.Ativo);
                        break;
                }

                if (!string.IsNullOrEmpty(aParam.QuestaoDuvida.Cliente))
                    aLista = aLista.Where(l => l.Cliente.ToUpper().Contains(aParam.QuestaoDuvida.Cliente.ToUpper()));

                if (!string.IsNullOrEmpty(aParam.QuestaoDuvida.Professor))
                    aLista = aLista.Where(l => l.Professor.ToUpper().Contains(aParam.QuestaoDuvida.Professor.ToUpper()));

                if (aParam.QuestaoDuvida.IdModerador > 0)
                    aLista = aLista.Where(l => l.IdModerador == aParam.QuestaoDuvida.IdModerador);

                if (aParam.LabelAluno > 0)
                {
                    var lista = new LabelEntity().ListarItemMarcados(aParam.LabelAluno, aParam.UsuarioLogado);
                    aLista = aLista.Where(l => lista.Contains(l.IdCliente));
                }

                if (aParam.LabelQuestaoDuvida > 0)
                {
                    var lista = new LabelEntity().ListarItemMarcados(aParam.LabelQuestaoDuvida, aParam.UsuarioLogado);
                    aLista = aLista.Where(l => lista.Contains(l.Id));
                }

                if (aParam.DataPerguntaIni != null && aParam.DataPerguntaFim != null)
                {
                    DateTime dteInicial = Convert.ToDateTime(aParam.DataPerguntaIni, new CultureInfo("pt-BR"));
                    DateTime dteFinal = Convert.ToDateTime(aParam.DataPerguntaFim, new CultureInfo("pt-BR"));
                    aLista = aLista.Where(l => l.PerguntaData >= dteInicial && l.PerguntaData <= dteFinal);
                }

                if (aParam.DataModeracaoIni != null && aParam.DataModeracaoFim != null)
                {
                    DateTime dteInicial = Convert.ToDateTime(aParam.DataModeracaoIni, new CultureInfo("pt-BR"));
                    DateTime dteFinal = Convert.ToDateTime(aParam.DataModeracaoFim, new CultureInfo("pt-BR"));
                    aLista = aLista.Where(l => l.ModeradorData >= dteInicial && l.ModeradorData <= dteFinal);
                }

                if (aParam.DataRespostaIni != null && aParam.DataRespostaFim != null)
                {
                    DateTime dteInicial = Convert.ToDateTime(aParam.DataRespostaIni, new CultureInfo("pt-BR"));
                    DateTime dteFinal = Convert.ToDateTime(aParam.DataRespostaFim, new CultureInfo("pt-BR"));
                    aLista = aLista.Where(l => l.RespostaData >= dteInicial && l.RespostaData <= dteFinal);
                }

                return aLista
                    .OrderByDescending(l => l.PerguntaData)
                    .Skip((aParam.PageNumber - 1) * aParam.PageSize)
                    .Take(aParam.PageSize)
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SetAdminDuvidaModerar(QuestaoDuvidaModeracao aRegistro)
        {
            var retorno = 0;
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var duvida = ctx.tblQuestao_Duvida.FirstOrDefault(d => d.intQuestaoDuvidaId == aRegistro.QuestaoDuvidaID);

                    if (duvida != null)
                    {
                        var moderacao = new tblQuestao_Duvida_Moderada()
                        {
                            intQuestaoDuvidaId = aRegistro.QuestaoDuvidaID,
                            intEmployeeId = aRegistro.EmployeeID,
                            dteCadastro = DateTime.Now,
                            bitActive = aRegistro.Ativo
                        };

                        ctx.tblQuestao_Duvida_Moderada.Add(moderacao);
                        ctx.SaveChanges();

                        retorno = 1;
                    }
                    return retorno;
                }
            }
            catch (Exception)
            {
                return retorno;
            }
        }

        public Professor GetProtocoladaPara(int idQuestao, int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var prof = (from pq in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                            join p1 in ctx.tblPersons on pq.intEmployeeID equals p1.intContactID into p2
                            from p in p2.DefaultIfEmpty()
                            join cq1 in ctx.tblConcursoQuestoes on pq.intQuestaoID equals cq1.intQuestaoID into cq2
                            from cq in cq2.DefaultIfEmpty()
                            where pq.intEmployeeID != null && pq.intQuestaoID == idQuestao && cq.intYear == ano
                            orderby pq.intProtocoloID descending
                            select new Professor
                            {
                                ID = p.intContactID,
                                Nome = p.txtName.Trim()
                            }).FirstOrDefault();

                return prof;
            }
        }

        public List<int> GetAnos(string sigla = "TODOS")
        {
            using (var ctx = new DesenvContext())
            {

                var list = new List<int>();

                var anos = (from cq in ctx.tblConcursoQuestoes
                            join cp in ctx.tblConcurso_Provas on cq.intProvaID equals cp.intProvaID
                            join c in ctx.tblConcurso on cp.ID_CONCURSO equals c.ID_CONCURSO
                            where (c.SG_CONCURSO.ToUpper().Trim() == sigla.ToUpper().Trim() || sigla == "TODOS")
                            select (int)cq.intYear).Distinct().ToList();

                return anos;
            }
        }

        public List<int> GetAnosPublicadas(string sigla = "TODOS")
        {
            using (var ctx = new DesenvContext())
            {

                var list = new List<int>();

                var anos = (from cq in ctx.tblConcursoQuestoes
                            join cp in ctx.tblConcurso_Provas on cq.intProvaID equals cp.intProvaID
                            join c in ctx.tblConcurso on cp.ID_CONCURSO equals c.ID_CONCURSO
                            where (c.SG_CONCURSO.ToUpper().Trim() == sigla.ToUpper().Trim() || sigla == "TODOS")
                            select cq.dteQuestao.Value.Year).Distinct().ToList();

                return anos;
            }
        }

        public List<ConcursoQuestaoDTO> ListConcursoQuestao(string siglaConcurso, int anoQuestao, int anoQuestaoPublicada)
        {
            using (var ctx = new DesenvContext())
            {
                var qry = (from c in ctx.tblConcurso
                           join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
                           join cq in ctx.tblConcursoQuestoes on cp.intProvaID equals cq.intProvaID
                           where (c.SG_CONCURSO == siglaConcurso || siglaConcurso == "TODOS")
                                && (anoQuestao == 0 || cq.intYear == anoQuestao)
                                && (anoQuestaoPublicada == 0 || cq.dteQuestao.Value.Year == anoQuestaoPublicada)
                           select new ConcursoQuestaoDTO
                           {
                               AnoConcurso = (int)c.VL_ANO_CONCURSO,
                               SiglaConcurso = c.SG_CONCURSO,
                               NomeProva = cp.txtName,
                               IdQuestao = cq.intQuestaoID,
                               OrdemQuestao = (int)cq.intOrder,
                               AnoQuestao = (int)cq.intYear,
                               DataQuestao = cq.dteQuestao

                           }
                );

                return qry.ToList();
            }
        }

        public int GetQuestaoForumPreLiberado()
        {
            using (var ctx = new DesenvContext())
            {
                var intQuestaoID = (from cp in ctx.tblConcurso_Provas
                                    join cq in ctx.tblConcursoQuestoes on cp.intProvaID equals cq.intProvaID
                                    where cp.ID_CONCURSO_RECURSO_STATUS == 1 && !(cq.bitComentarioAtivo ?? false)
                                    select cq.intQuestaoID).FirstOrDefault();

                return intQuestaoID;

            }
        }

        public int GetQuestaoForumPosLiberado()
        {
            using (var ctx = new DesenvContext())
            {
                var temQuestaoAlterada = ctx.tblConcursoQuestoes.Where(q => q.intProvaID == 924474).Where(q => new[] { 11, 12 }.Contains(q.intStatus_Banca_Recurso ?? 0)).Any();
                var decisaoBancaLiberada = new[] { (int)QuestaoRecurso.StatusBancaAvaliadora.Sim, (int)QuestaoRecurso.StatusBancaAvaliadora.Nao }.ToList();
                var intQuestaoID = (from cp in ctx.tblConcurso_Provas
                                    join cq in ctx.tblConcursoQuestoes on cp.intProvaID equals cq.intProvaID
                                    where cp.ID_CONCURSO_RECURSO_STATUS == (int)ProvaRecurso.StatusProva.RecursosEmAnalise
                                    && (cq.bitComentarioAtivo ?? false)
                                    && !decisaoBancaLiberada.Contains(cq.intStatus_Banca_Recurso ?? -1)
                                    && !temQuestaoAlterada
                                    select cq.intQuestaoID).DefaultIfEmpty(0).FirstOrDefault();

                return intQuestaoID;

            }
        }

        public List<Questao> GetDiscursivasSimulado(Int32 idProva, Int32 matricula)
        {
            var isNotaLiberada = new RankingSimuladoEntity().IsFaseFinalLiberado(idProva);
            var entidadeQuestao = new Questao();

            using (var ctx = new AcademicoContext())
            {
                var lstQuestao = (from s in ctx.tblQuestao_Simulado
                                  where s.intSimuladoID == idProva && s.txtCodigoCorrecao.Contains("Caso")
                                  select new Questao()
                                  {
                                      Id = s.intQuestaoID
                                  }).ToList();

                foreach (var q in lstQuestao)
                {
                    var lstAlternativas = (from a in ctx.tblQuestaoAlternativas where a.intQuestaoID == q.Id && string.IsNullOrEmpty(a.txtAlternativa) != true select a).ToList().Select(a => new Alternativa()
                    {
                        Correta = a.bitCorreta ?? false,
                        Letra = Convert.ToChar(a.txtLetraAlternativa),
                        Nome = (string.IsNullOrEmpty(a.txtAlternativa) ? "Resposta: " : a.txtAlternativa),
                        Gabarito = (string.IsNullOrEmpty(a.txtResposta) ? "(Sem Gabarito)" : a.txtResposta),
                        Id = a.intAlternativaID,
                        Nota = (from d in ctx.tblCartaoResposta_Discursiva
                                join h in ctx.tblExercicio_Historico on d.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                where d.intDicursivaId == a.intAlternativaID && h.intClientID == matricula
                                select d.dblNota).FirstOrDefault(),
                        Resposta = (from d in ctx.tblCartaoResposta_Discursiva
                                    join h in ctx.tblExercicio_Historico on d.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                    where d.intDicursivaId == a.intAlternativaID && h.intClientID == matricula
                                    select d.txtResposta).FirstOrDefault()
                    }).ToList();

                    foreach (var a in lstAlternativas)
                    {
                        if (!isNotaLiberada)
                            a.Nota = null;
                    }
                    q.Alternativas = new List<Alternativa>();
                    q.Alternativas = lstAlternativas;
                }

                return lstQuestao;
            }
        }

        public Questao GetTipoApostila(int QuestaoID, int ClientID, int ApplicationID)
        {
            var entidadeQuestao = new Questao();
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {

                    var questao = (from q in ctxMatDir.tblConcursoQuestoes where q.intQuestaoID == QuestaoID select q).FirstOrDefault();
                    var gabarito = (from q in ctxMatDir.tblConcursoQuestoes_Alternativas where q.intQuestaoID == QuestaoID && !string.IsNullOrEmpty(q.txtResposta) select q.txtResposta).FirstOrDefault();

                    if (questao.txtEnunciado.ToUpper().Contains("GABARITO"))
                    {
                        if (string.IsNullOrEmpty(gabarito))
                        {
                            gabarito = questao.txtEnunciado.Substring(questao.txtEnunciado.ToUpper().IndexOf("GABARITO"));
                        }

                        questao.txtEnunciado = questao.txtEnunciado.Remove(questao.txtEnunciado.ToUpper().IndexOf("GABARITO"));
                    }

                    var alternativas = new List<Alternativa>();
                    var alternativasQuery = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas where a.intQuestaoID == QuestaoID && a.txtAlternativa.Trim() != "" select a).ToList();
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
                            {
                                alternativas.FirstOrDefault(a => a.CorretaPreliminar).Correta = true;
                            }
                        }
                        else
                        {
                            alternativas.FirstOrDefault().Gabarito = gabarito;
                        }

                    }
                    else
                    {
                        var primeiraAlternativasQuery = ctxMatDir.tblConcursoQuestoes_Alternativas.Where(d => d.intQuestaoID == QuestaoID).OrderBy(d => d.intAlternativaID).FirstOrDefault();
                        if (alternativas.Count == 0)
                            alternativas.Add(new Alternativa
                            {
                                Id = primeiraAlternativasQuery.intAlternativaID,
                                Gabarito = gabarito,
                                Nome = "Resposta: ",
                                Correta = true
                            });
                        else
                        {
                            alternativas[0].Gabarito = gabarito;
                        }
                    }

                    var queryQuestaoConcurso = (from qc in ctxMatDir.tblConcursoQuestoes where qc.intQuestaoID == QuestaoID select qc).FirstOrDefault();

                    if (queryQuestaoConcurso == null)
                        return new Questao();

                    var respostasDiscursivas = (from r in ctx.tblCartaoResposta_Discursiva
                                                join historico in ctx.tblExercicio_Historico on r.intHistoricoExercicioID equals historico.intHistoricoExercicioID
                                                where r.intQuestaoDiscursivaID == QuestaoID
                                                      && historico.intClientID == ClientID
                                                select r)
                        .ToList();

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

                    var imagemEntity = new ImagemEntity();
                    var LstImagensId = new List<String>();

                    var lstImagens = imagemEntity.GetImagensQuestaoConcursoCache(QuestaoID);
                    foreach (var i in lstImagens)
                        LstImagensId.Add(i.ToString());

                    var lstImagensComentario = new Imagens();
                    lstImagensComentario.AddRange(imagemEntity.GetConcursoImagemComentario(QuestaoID));

                    var videoEntity = new VideoEntity();
                    var lstVideos = videoEntity.GetVideoQuestaoConcurso(QuestaoID);
                    var videoAddress = string.Empty;

                    if (lstVideos.Any())
                        videoAddress = lstVideos[0].Url;

                    Media mediaComentario = new Media();
                    mediaComentario.Imagens = LstImagensId;
                    mediaComentario.Video = videoAddress;
                    entidadeQuestao = new Questao
                    {
                        Enunciado = questao.txtEnunciado,
                        Alternativas = alternativas,
                        Comentario = Utilidades.RemoveHtml(questao.txtComentario),
                        MediaComentario = mediaComentario,
                        Respondida = Respondida,
                        Tipo = Convert.ToBoolean(questao.bitDiscursiva) ? (int)Questao.tipoQuestao.DISCURSIVA : (int)Questao.tipoQuestao.OBJETIVA
                    };

                    entidadeQuestao.Comentario = System.Web.HttpUtility.HtmlDecode(entidadeQuestao.Comentario);

                    entidadeQuestao.Ordem = questao.intOrder ?? questao.intQuestaoID;

                    var respostas = (from c in ctx.tblCartaoResposta_objetiva
                                     join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                     where h.intClientID == ClientID && c.intQuestaoID == QuestaoID
                                     group c by c.intQuestaoID into g
                                     select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                    var concursoQuestoesAlternativasIntQuestaoID = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                                    where a.intQuestaoID == QuestaoID
                                                                    select a.intQuestaoID).FirstOrDefault();

                    var resposta = (from r in respostas
                                    join c in ctx.tblCartaoResposta_objetiva on r.ID equals c.intID
                                    where c.intQuestaoID == concursoQuestoesAlternativasIntQuestaoID
                                    select c).FirstOrDefault();

                    var LetraAlternativa = "";

                    if (resposta != null)
                        LetraAlternativa = resposta.txtLetraAlternativa;

                    foreach (Alternativa a in entidadeQuestao.Alternativas)
                        if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                            a.Selecionada = true;

                    var cartaoRespostaLetraAlternativa = (from c in ctx.tblCartaoResposta_objetiva
                                                          join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                          where
                                                          c.intQuestaoID == QuestaoID
                                                          && h.intClientID == ClientID
                                                          select c.txtLetraAlternativa).ToList();

                    var correta = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                   where
                                   a.intQuestaoID == QuestaoID
                                   && cartaoRespostaLetraAlternativa.Contains(a.txtLetraAlternativa)
                                   select a).ToList();

                    var concursoQuestoesIntQuestaoID = (from qs in ctxMatDir.tblConcursoQuestoes
                                                        where qs.intQuestaoID == QuestaoID
                                                        select qs.intQuestaoID).FirstOrDefault();

                    var marcacao = (from m in ctx.tblQuestao_Marcacao
                                    where m.intClientID == ClientID
                                        && m.intQuestaoID == QuestaoID
                                        && m.intQuestaoID == concursoQuestoesIntQuestaoID
                                    orderby m.dtAnotacao descending
                                    select m).ToList().FirstOrDefault();

                    if (resposta != null)
                        entidadeQuestao.Respondida = true;

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

                    if (correta.Count() > 0)
                        entidadeQuestao.Correta = true;

                    if (questao.bitAnulada == true || questao.bitAnuladaPosRecurso == true)
                        entidadeQuestao.Anulada = true;
                    else
                        entidadeQuestao.Anulada = false;

                    entidadeQuestao.Especialidades = (new EspecialidadeEntity()).GetByFilters(QuestaoID);
                    entidadeQuestao.Titulo = GeraTituloEnunciado(entidadeQuestao, Exercicio.tipoExercicio.CONCURSO).Replace("0", "");
                    entidadeQuestao.ImagensComentario = lstImagensComentario;

                    var prova = (from p in ctxMatDir.tblConcurso_Provas
                                 join c in ctxMatDir.tblConcurso on p.ID_CONCURSO equals c.ID_CONCURSO
                                 where p.intProvaID == queryQuestaoConcurso.intProvaID
                                 select new { ano = c.VL_ANO_CONCURSO, nome = c.SG_CONCURSO, tipo = p.txtName }).FirstOrDefault();

                    entidadeQuestao.Cabecalho = string.Concat(prova.ano, " ", prova.nome);
                    entidadeQuestao.Prova = new Prova { TipoProva = new TipoProva { Tipo = prova.tipo } };
                }
            }
            return entidadeQuestao;
        }

        public Questao GetTipoMontaProva(int QuestaoID, int ClientID, int ApplicationID)
        {
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = ClientID,
                IdApp = (Aplicacoes)ApplicationID,
                Tela = Util.Log.MsProLog_Tela.RealizaProvaQuestao,
                Acao = Util.Log.MsProLog_Acao.Abriu,
                Obs = string.Format("Monta Prova - ID: {0}", QuestaoID)
            });

            var entidadeQuestao = new Questao();
            using (var ctx = new AcademicoContext())
            {
                var questao = (from q in ctx.tblQuestoes where q.intQuestaoID == QuestaoID select q).FirstOrDefault();
                var alternativas = (from a in ctx.tblQuestaoAlternativas where a.intQuestaoID == QuestaoID select a).ToList().Select(a => new Alternativa()
                {
                    Correta = a.bitCorreta ?? false,
                    Letra = Convert.ToChar(a.txtLetraAlternativa),
                    Nome = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ?
                        (string.IsNullOrEmpty(a.txtAlternativa) ? "Resposta: " : a.txtAlternativa)
                        : a.txtAlternativa,

                    Gabarito = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ?
                        (string.IsNullOrEmpty(a.txtResposta) ? "(Sem Gabarito)" : a.txtResposta)
                        : a.txtResposta,
                    Id = a.intAlternativaID
                }).ToList();

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

                var exercicio = new ExercicioEntity();
                var queryQuestaoSimlado = (from qs in ctx.tblQuestao_Simulado where qs.intQuestaoID == QuestaoID select qs).FirstOrDefault();

                if (queryQuestaoSimlado == null)
                    return new Questao();

                var ExercicioID = queryQuestaoSimlado.intSimuladoID;

                var respostasDiscursivas = (from r in ctx.tblCartaoResposta_Discursiva
                                            join h in ctx.tblExercicio_Historico on r.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                            where r.intQuestaoDiscursivaID == QuestaoID
                                                  && h.intClientID == ClientID
                                            select r).ToList();

                bool Respondida = false;

                if (respostasDiscursivas.Count() > 0)
                    foreach (var r in respostasDiscursivas)
                        for (int k = 0; k < alternativas.Count(); k++)
                            if (r.intDicursivaId == alternativas[k].Id)
                            {
                                alternativas[k].Resposta = r.txtResposta;
                                Respondida = true;
                            }

                var imagemEntity = new ImagemEntity();

                var imagemIds = imagemEntity.GetImagensQuestaoSimulado(QuestaoID);
                var imagens = new List<String>();
                foreach (var i in imagemIds)
                    imagens.Add(i.ToString());

                var videoEntity = new VideoEntity();
                var videos = videoEntity.GetVideoQuestaoSimulado(QuestaoID);

                var videoAddress = "";
                if (videos.Count() > 0)
                    videoAddress = videos[0].Nome;

                var imagensComentario = new List<Imagem>();


                imagensComentario = (from i in ctx.tblQuestoesSimuladoImagem_Comentario
                                     where i.intQuestaoID == questao.intQuestaoID
                                     select new Imagem
                                     {
                                         ID = i.intImagemComentarioID,
                                         Url = Constants.URLCOMENTARIOIMAGEMSIMULADOMSCROSS.Replace("IDCOMENTARIOIMAGEM", i.intImagemComentarioID.ToString().Trim()),
                                         Nome = i.txtName

                                     }).ToList();

                var imgComentario = new Imagens();
                imgComentario.AddRange(imagensComentario);




                var mediaComentario = new Media();
                mediaComentario.Imagens = imagens;
                mediaComentario.Video = videoAddress;
                entidadeQuestao = new Questao
                {
                    Enunciado = questao.txtEnunciado,
                    Alternativas = alternativas,
                    Comentario = string.IsNullOrEmpty(questao.txtComentario)
                                          ? "Esta questão ainda não possui comentário em texto. Comentário em produção pela equipe acadêmica."
                                          : questao.txtComentario,
                    MediaComentario = mediaComentario,
                    ImagensComentario = imgComentario,
                    Respondida = Respondida,
                    Tipo = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? 2 : 1
                };

                var queryOrdem = (from o in ctx.tblSimuladoOrdenacao
                                  where o.intQuestaoID == QuestaoID
                                  select new
                                  {
                                      intSimuladoID = o.intSimuladoID,
                                      intQuestaoID = o.intQuestaoID,
                                      intVersaoID = 1,
                                      intOrdem = o.intOrdem ?? 0
                                  }).Union(
                    from v in ctx.tblSimuladoVersao
                    where v.intQuestao == QuestaoID && v.intVersaoID == 1
                    select new
                    {
                        intSimuladoID = v.intSimuladoID,
                        intQuestaoID = v.intQuestaoID,
                        intVersaoID = v.intVersaoID,
                        intOrdem = v.intQuestao
                    }
                ).FirstOrDefault();

                entidadeQuestao.Ordem = queryOrdem != null
                    ? queryOrdem.intOrdem
                    : entidadeQuestao.Id;

                var respostas = (from c in ctx.tblCartaoResposta_objetiva
                                 join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                 where h.intClientID == ClientID && c.intQuestaoID == QuestaoID
                                 group c by c.intQuestaoID into g
                                 select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                var resposta = (from r in respostas
                                join c in ctx.tblCartaoResposta_objetiva on r.ID equals c.intID
                                join a in ctx.tblQuestaoAlternativas on c.intQuestaoID equals a.intQuestaoID
                                select c).FirstOrDefault();

                var LetraAlternativa = "";

                if (resposta != null)
                    LetraAlternativa = resposta.txtLetraAlternativa;

                foreach (Alternativa a in entidadeQuestao.Alternativas)
                    if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                        a.Selecionada = true;

                var correta = (from a in ctx.tblQuestaoAlternativas
                               join c in ctx.tblCartaoResposta_objetiva on a.txtLetraAlternativa equals c.txtLetraAlternativa
                               join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                               where
                               c.intQuestaoID == QuestaoID
                               && h.intClientID == ClientID
                               && a.intQuestaoID == QuestaoID
                               select a).ToList();

                var marcacao = (from m in ctx.tblQuestao_Marcacao
                                join qs in ctx.tblQuestao_Simulado on m.intQuestaoID equals qs.intQuestaoID
                                where m.intClientID == ClientID && m.intQuestaoID == QuestaoID
                                orderby m.dtAnotacao descending
                                select m).ToList().FirstOrDefault();

                if (resposta != null)
                    entidadeQuestao.Respondida = true;

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

                if (correta.Count() > 0)
                    entidadeQuestao.Correta = true;

                entidadeQuestao.Anulada = questao.bitAnulada;
                entidadeQuestao.Especialidades = (new EspecialidadeEntity()).GetByQuestaoSimulado(QuestaoID, ExercicioID);

                entidadeQuestao.Titulo = GeraTituloEnunciado(entidadeQuestao, Exercicio.tipoExercicio.MONTAPROVA).Replace("0", "");
            }
            return entidadeQuestao;
        }

        protected string GeraTituloEnunciado(Questao questao, Exercicio.tipoExercicio tipoExercicio)
        {
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

        private static string GetEspecialidades(Questao questao)
        {
            var texto = questao.Especialidades.Count() > 1 ? "Especialidades: " : "Especialidade: ";
            foreach (var ep in questao.Especialidades)
                texto += string.Concat(ep.Descricao, "/");
            texto = texto.Substring(0, texto.Length - 1);

            return texto;
        }

        public QuestaoAnotacao GetAnotacoesAluno(int idQuestao, int matricula)
        {
            var questaoAnotacao = new QuestaoAnotacao();
            using (var ctx = new AcademicoContext())
            {
                var marcacao = (from m in ctx.tblQuestao_Marcacao
                                join qs in ctx.tblQuestao_Simulado on m.intQuestaoID equals qs.intQuestaoID
                                where m.intClientID == matricula && m.intQuestaoID == idQuestao
                                orderby m.dtAnotacao descending
                                select m).FirstOrDefault();

                if (marcacao != null)
                {
                    questaoAnotacao.Favorita = marcacao.bitFlagFavorita;
                    questaoAnotacao.Duvida = marcacao.bitFlagEmDuvida;
                    questaoAnotacao.Anotacao = marcacao.txtAnotacao;
                }
            }

            return questaoAnotacao;
        }

        public List<int> GetQuestaoComAlternativaNaoRespondida(int idTipoExercicio)
        {
            using (var ctx = new DesenvContext())
            {
                List<int> Lista = new List<int>();

                var questaoEstatistica = (from qe in ctx.tblQuestao_Estatistica
                                          where qe.intExercicioTipo == idTipoExercicio
                                          select qe
                                           ).Distinct();

                var result = ctx.tblConcursoQuestoes_Alternativas
                    .Where(qa => !questaoEstatistica.Where(qe => qe.intQuestaoID == qa.intQuestaoID && qe.txtLetraAlternativa.Trim() == qa.txtLetraAlternativa.Trim()).Any())
                    .Select(x => x.intQuestaoID).Distinct().ToList();

                return result;
            }
        }

        public List<int> GetQuestoesAnuladas()
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcursoQuestoes.Where(a => a.bitAnulada).Select(x => x.intQuestaoID).ToList();
            }
        }

        public List<int> GetQuestaoComMaisDeUmaAlternativaCorreta(int idTipoExercicio)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_Estatistica.Where(a => a.bitCorreta == true && a.intExercicioTipo == idTipoExercicio).GroupBy(c => c.intQuestaoID).Where(b => b.Count() > 1).Select(b => b.Key).Distinct().ToList();
            }
        }

        public List<CartaoRespostaDiscursivaDTO> GetRespostasDiscursivasSimuladoAgendado(int QuestaoID, int ExercicioHistoricoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var respostasDiscursivas = (from r in ctx.tblCartaoResposta_Discursiva
                                            where r.intQuestaoDiscursivaID == QuestaoID
                                                  && r.intHistoricoExercicioID == ExercicioHistoricoID
                                            orderby r.dteCadastro
                                            select new CartaoRespostaDiscursivaDTO()
                                            {
                                                intID = r.intID,
                                                intQuestaoDiscursivaID = r.intQuestaoDiscursivaID,
                                                intHistoricoExercicioID = r.intHistoricoExercicioID,
                                                txtResposta = r.txtResposta,
                                                intExercicioTipoId = r.intExercicioTipoId,
                                                intDicursivaId = r.intDicursivaId,
                                                dteCadastro = r.dteCadastro,
                                                dblNota = r.dblNota
                                            }).ToList();
                return respostasDiscursivas;
            }
        }

        public string GetRespostaObjetivaSimuladoAgendado(int QuestaoID, int ExercicioHistoricoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var respostas = (from c in ctx.tblCartaoResposta_objetiva_Simulado_Online
                                 where c.intQuestaoID == QuestaoID
                                 && c.intHistoricoExercicioID == ExercicioHistoricoID
                                 orderby c.dteCadastro descending
                                 select c.txtLetraAlternativa).ToList().FirstOrDefault();
                return respostas;
            }
        }

        public List<Questao> GetTipoSimuladoAll(Int32 idSimulado, Int32 ClientID, Int32 ApplicationID)
        {
            List<Questao> lstQuestao = new List<Questao>();
            var entidadeQuestao = new Questao();
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Obtendo tipos todos os simulados"))
                {
                    var queryQuestaoSimlado = ctx.tblQuestao_Simulado.Where(x => x.intSimuladoID == idSimulado).ToList();

                    foreach (var item in queryQuestaoSimlado)
                    {

                        var questao = (from q in ctx.tblQuestoes where q.intQuestaoID == item.intQuestaoID select q).FirstOrDefault();
                        var alternativas = (from a in ctx.tblQuestaoAlternativas where a.intQuestaoID == item.intQuestaoID select a).ToList().Select(a => new Alternativa()
                        {
                            Letra = Convert.ToChar(a.txtLetraAlternativa),
                            Nome = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? (string.IsNullOrEmpty(a.txtAlternativa) ? "Resposta: " : a.txtAlternativa) : a.txtAlternativa,
                            Gabarito = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? (string.IsNullOrEmpty(a.txtResposta) ? "(Sem Gabarito)" : a.txtResposta) : a.txtResposta,
                            Id = a.intAlternativaID
                        }).ToList();


                        var exercicio = new ExercicioEntity();
                        var ExercicioID = item.intSimuladoID;

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

                        Int32 historicoExercicioID = exercicio.GetHistoricoID(ExercicioID, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), ApplicationID, ClientID);

                        var respostasDiscursivas = (from r in ctx.tblCartaoResposta_Discursiva
                                                    join h in ctx.tblExercicio_Historico on r.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                    where r.intQuestaoDiscursivaID == item.intQuestaoID
                                                    && h.intClientID == ClientID
                                                    select r).ToList();

                        bool Respondida = false;

                        if (respostasDiscursivas.Count() > 0)
                            foreach (var r in respostasDiscursivas)
                                for (int k = 0; k < alternativas.Count(); k++)
                                    if (r.intDicursivaId == alternativas[k].Id)
                                    {
                                        alternativas[k].Resposta = r.txtResposta;
                                        Respondida = true;
                                    }

                        entidadeQuestao = new Questao
                        {

                            Id = questao.intQuestaoID,
                            Alternativas = alternativas,
                            Respondida = Respondida,
                            Tipo = Convert.ToBoolean(Convert.ToInt32(questao.bitCasoClinico)) ? 2 : 1,
                            ExercicioTipoID = 1
                        };



                        var respostas = (from c in ctx.tblCartaoResposta_objetiva
                                        join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                        where h.intClientID == ClientID && c.intQuestaoID == item.intQuestaoID
                                        group c by c.intQuestaoID into g
                                        select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                        var resposta = (from r in respostas
                                        join c in ctx.tblCartaoResposta_objetiva on r.ID equals c.intID
                                        join a in ctx.tblQuestaoAlternativas on c.intQuestaoID equals a.intQuestaoID
                                        select c).FirstOrDefault();

                        var LetraAlternativa = "";

                        if (resposta != null)
                            LetraAlternativa = resposta.txtLetraAlternativa;

                        foreach (Alternativa a in entidadeQuestao.Alternativas)
                            if (a.Letra.ToString().ToLower() == LetraAlternativa.ToLower())
                                a.Selecionada = true;

                        entidadeQuestao.Anulada = questao.bitAnulada;

                        lstQuestao.Add(entidadeQuestao);
                    }
                }
            }
            return lstQuestao;
        }

        public int SetFavoritaQuestaoApostila(int QuestaoId, int ClientID, bool Favorita)
        {
            using(MiniProfiler.Current.Step("Define questão apostila como favorita"))
            {
                using (var ctx = new AcademicoContext())
                {
                    if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.APOSTILA))
                        return 0;

                    List<int> ListQuestaoId;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        ListQuestaoId = (from qs in ctxMatDir.tblConcursoQuestoes
                                        where qs.intQuestaoID == QuestaoId
                                        select qs.intQuestaoID).ToList();
                    }


                    var mcc = (from m in ctx.tblQuestao_Marcacao
                            where m.intClientID == ClientID
                                && m.intQuestaoID == QuestaoId
                                && ListQuestaoId.Contains(m.intQuestaoID)
                            orderby m.dtAnotacao descending
                            select m).FirstOrDefault();

                    if (mcc == null)
                    {
                        var marcacao = new tblQuestao_Marcacao
                        {
                            intQuestaoID = QuestaoId,
                            intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA),
                            intClientID = ClientID,
                            dtAnotacao = DateTime.Now,
                            bitFlagFavorita = Favorita
                        };
                        ctx.tblQuestao_Marcacao.Add(marcacao);
                    }
                    else
                    {
                        mcc.bitFlagFavorita = Favorita;
                    }
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }

                    return 1;
                }
            }
        }

        public int SetDuvidaQuestaoApostila(int QuestaoId, int ClientID, bool Duvida)
        {
            using(MiniProfiler.Current.Step("Define dúvida de questão de apostila"))
            {
                using (var ctx = new AcademicoContext())
                {
                    if (!isQuestao(QuestaoId, Exercicio.tipoExercicio.APOSTILA))
                        return 0;

                    List<int> ListaQuestaoID;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        ListaQuestaoID = (from qs in ctxMatDir.tblConcursoQuestoes
                                        where qs.intQuestaoID == QuestaoId
                                        select qs.intQuestaoID).ToList();
                    }

                    var mcc = (from m in ctx.tblQuestao_Marcacao
                            where m.intClientID == ClientID
                                && m.intQuestaoID == QuestaoId
                                && ListaQuestaoID.Contains(m.intQuestaoID)
                            orderby m.dtAnotacao descending
                            select m).FirstOrDefault();

                    if (mcc == null)
                    {
                        var marcacao = new tblQuestao_Marcacao
                        {
                            intQuestaoID = QuestaoId,
                            intTipoExercicioID = Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA),
                            intClientID = ClientID,
                            dtAnotacao = DateTime.Now,
                            bitFlagEmDuvida = Duvida
                        };
                        ctx.tblQuestao_Marcacao.Add(marcacao);
                    }
                    else
                    {
                        mcc.bitFlagEmDuvida = Duvida;
                    }
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }

                    return 1;
                }
            }
        }

        public List<QuestaoFiltroDTO> GetMarcacoesQuestoesAluno(int matricula)
        {

            var favoritas = new List<QuestaoFiltroDTO>();

            using (var ctx = new AcademicoContext())
            {
                favoritas = ctx.tblQuestao_Marcacao.Where(x => x.intClientID == matricula && (x.bitFlagFavorita || x.txtAnotacao != null))
                    .Select(
                    q => new QuestaoFiltroDTO
                    {
                        QuestaoId = q.intQuestaoID,
                        TipoExercicioId = q.intTipoExercicioID,
                        Favorita = q.bitFlagFavorita,
                        Anotacao = q.txtAnotacao
                    }
                    ).ToList();

            }
            return favoritas;
        }

        public List<QuestaoFiltroDTO> GetQuestoesIds(int[] ids)
        {
            using (var ctx = new DesenvContext())
            {
                var retorno = (from q in ctx.tblConcursoQuestoes
                               join p in ctx.tblConcurso_Provas on q.intProvaID equals p.intProvaID
                               join c in ctx.tblConcurso on p.ID_CONCURSO equals c.ID_CONCURSO
                               join e in ctx.tblStates on c.CD_UF equals e.txtCaption
                               select new QuestaoFiltroDTO
                               {
                                   QuestaoId = q.intQuestaoID,
                                   Ano = c.VL_ANO_CONCURSO ?? 0,
                                   Estado = e.txtDescription,
                                   ConcursoSigla = c.SG_CONCURSO
                               }).ToList();

                var questoes = (from a in retorno
                                join b in ids on a.QuestaoId equals b
                                select a).ToList();

                return questoes;
            }

        }

        public ProvaAlunosFavoritoDTO GetAlunosVotaramForumPre(int idQuestao)
        {
            var alunos = new ProvaAlunosFavoritoDTO();
            using (var ctx = new DesenvContext())
            {
                var result = (from c in ctx.tblConcurso
                              join p in ctx.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
                              join q in ctx.tblConcursoQuestoes on p.intProvaID equals q.intProvaID
                              join v in ctx.tblConcurso_Recurso_Aluno on q.intQuestaoID equals v.intQuestaoID
                              where q.intQuestaoID == idQuestao && v.intTipo == (int)QuestaoRecurso.TipoForumRecurso.Pre
                              select new
                              {
                                  NomeProva = c.SG_CONCURSO + Constants.DASH + c.CD_UF,
                                  Matricula = v.intContactID
                              }).ToList();

                if (result != null && result.Any())
                {
                    alunos.MatriculasFavoritaram = result.Select(m => m.Matricula).Distinct().ToList();
                    alunos.Prova = new ProvaConcursoDTO
                    {
                        Nome = result.ElementAt(0).NomeProva
                    };
                }
                return alunos;
            }
        }

        public int ObterStatusRecursoBanca(int idQuestao)
        {
            using (var ctx = new DesenvContext())
            {
                var questao = ctx.tblConcursoQuestoes.FirstOrDefault(x => x.intQuestaoID == idQuestao);
                return questao != null && questao.intStatus_Banca_Recurso.HasValue ? 
                        questao.intStatus_Banca_Recurso.Value : default(int);
            }
        }
    }
}