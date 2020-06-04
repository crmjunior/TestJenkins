using System;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System.Collections.Generic;
using System.Data;
using StackExchange.Profiling;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Medgrupo.DataAccessEntity;

namespace MedCore_DataAccess.Repository
{
    public class RankingSimuladoEntity : IRankingSimuladoData
    {
        const int MEDREADER = 183;

        public AlunoConcursoEstatistica GetEstatisticaAlunoSimulado(int idMatricula, int idExercicio, bool isOnline)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo estatística aluno simulado"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        var questoes = (from sq in ctx.tblQuestao_Simulado
                                        join q in ctx.tblQuestoes
                                        on sq.intQuestaoID equals q.intQuestaoID
                                        join qa in ctx.tblQuestaoAlternativas
                                        on sq.intQuestaoID equals qa.intQuestaoID
                                        where sq.intSimuladoID == idExercicio
                                        && (q.bitCasoClinico != "1")
                                        select new
                                        {
                                            questaoId = qa.intQuestaoID,
                                            alternativa = qa.txtLetraAlternativa,
                                            alternativaCorreta = qa.bitCorreta,
                                            anulada = q.bitAnulada
                                        }
                                        ).ToList();

                        var qtdeQuestoesAnuladas = questoes.Where(x => x.anulada).Select(x => x.questaoId).Distinct().Count();
                        var questoesNaoAnuladas = questoes.Where(x => x.anulada == false && x.alternativaCorreta == true).ToList();

                        var estatistica = new AlunoConcursoEstatistica();

                        var respostasAluno = new List<object>()
                        .Select(t => new { questaoId = default(int), alternativa = default(string) }).ToList();

                        if (isOnline)
                        {
                            // respostasAluno = (from eh in ctx.tblExercicio_Historico
                            //                 join cro in ctx.tblCartaoResposta_objetiva_Simulado_Online
                            //                 on eh.intHistoricoExercicioID equals cro.intHistoricoExercicioID
                            //                 join sv in ctx.tblSimuladoVersao
                            //                 on new { cro.intQuestaoID, eh.intExercicioID, eh.intVersaoID } equals new { sv.intQuestaoID, intExercicioID = sv.intSimuladoID, sv.intVersaoID }
                            //                 join sr in ctx.tblSimuladoRespostas on new { sv.intSimuladoID, eh.intClientID, sv.intQuestao, sv.intVersaoID } equals new { sr.intSimuladoID, sr.intClientID, sr.intQuestao, sr.intVersaoID }
                            //                 where eh.intExercicioID == idExercicio && eh.intClientID == idMatricula && eh.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
                            //                 && (eh.bitRealizadoOnline == true || eh.bitPresencial == true)
                            //                 select new { questaoId = cro.intQuestaoID, alternativa = sr.txtLetraResposta, DataRealizacao = cro.dteCadastro }
                            //                 ).OrderByDescending(x => x.DataRealizacao)
                            //                 .GroupBy(x => x.questaoId)
                            //                 .Select(y => new { questaoId = y.FirstOrDefault().questaoId, alternativa = y.FirstOrDefault().alternativa }).ToList();

                            var query = (from eh in ctx.tblExercicio_Historico
                                join cro in ctx.tblCartaoResposta_objetiva_Simulado_Online
                                on eh.intHistoricoExercicioID equals cro.intHistoricoExercicioID
                                join sv in ctx.tblSimuladoVersao
                                on new { cro.intQuestaoID, eh.intExercicioID, eh.intVersaoID } equals new { sv.intQuestaoID, intExercicioID = sv.intSimuladoID, sv.intVersaoID }
                                join sr in ctx.tblSimuladoRespostas on new { sv.intSimuladoID, eh.intClientID, sv.intQuestao, sv.intVersaoID } equals new { sr.intSimuladoID, sr.intClientID, sr.intQuestao, sr.intVersaoID }
                                where eh.intExercicioID == idExercicio && eh.intClientID == idMatricula && eh.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
                                && (eh.bitRealizadoOnline == true || eh.bitPresencial == true)
                                select new { questaoId = cro.intQuestaoID, alternativa = sr.txtLetraResposta, DataRealizacao = cro.dteCadastro }).AsNoTracking().ToList();

                                respostasAluno = query.OrderByDescending(x => x.DataRealizacao)
                                    .GroupBy(x => x.questaoId)
                                    .Select(y => new { questaoId = y.FirstOrDefault().questaoId, alternativa = y.FirstOrDefault().alternativa }).ToList();
                                            


                        }
                        else
                        {
                            var lista = new DBQuery().ExecuteQuery(@"select 
                                    x.intQuestaoID,
                                    x.txtLetraAlternativa,
                                    x.intHistoricoExercicioID
                                from (
                                select 
                                    cro.intQuestaoID,
                                    max(cro.txtLetraAlternativa) as txtLetraAlternativa,
                                    max(eh.intHistoricoExercicioID) as intHistoricoExercicioID
                                from
                                tblExercicio_Historico eh
                                inner join tblCartaoResposta_objetiva cro on eh.intHistoricoExercicioID = cro.intHistoricoExercicioID
                                where eh.intClientID = " + idMatricula + @" and eh.intExercicioTipo = 1
                                group by cro.intQuestaoID) x
                                order by x.intHistoricoExercicioID desc
                            ", Utilidades.GetChaveamento());

                            if (lista.Tables[0].Rows.Count > 0)
                                foreach (DataRow dRow in lista.Tables[0].Rows)
                                    respostasAluno.Add(new
                                    {
                                        questaoId = Convert.ToInt32(dRow["intQuestaoID"]),
                                        alternativa = dRow["txtLetraAlternativa"].ToString()
                                    });
                        }

                        var respostasAlunoNaoAnuladas = (from ra in respostasAluno
                                                        join q in questoesNaoAnuladas
                                                        on ra.questaoId equals q.questaoId
                                                        where q.anulada == false
                                                        select new { questaoId = ra.questaoId, alternativa = ra.alternativa }
                                                        ).Distinct().ToList();

                        estatistica.TotalQuestoes = questoes.GroupBy(x => x.questaoId).Count();
                        if(estatistica.TotalQuestoes == 0 && !isOnline)
                        {
                            estatistica.TotalQuestoes = (from sq in ctx.tblQuestao_Simulado
                                            join q in ctx.tblQuestoes
                                            on sq.intQuestaoID equals q.intQuestaoID
                                            where sq.intSimuladoID == idExercicio
                                            select q.intQuestaoID
                                        ).Distinct().Count();
                        }
                        estatistica.NaoRealizadas = estatistica.TotalQuestoes - (respostasAlunoNaoAnuladas.Count() + qtdeQuestoesAnuladas);
                        estatistica.Acertos = ((from q in questoesNaoAnuladas
                                                join r in respostasAlunoNaoAnuladas
                                                on q.questaoId equals r.questaoId
                                                where q.alternativa == r.alternativa
                                                select q.questaoId
                                                ).Count() + qtdeQuestoesAnuladas);

                        estatistica.Erros = (from q in questoesNaoAnuladas
                                            join r in respostasAlunoNaoAnuladas
                                            on q.questaoId equals r.questaoId
                                            where q.alternativa != r.alternativa
                                            select q.questaoId
                                            ).Count();

                        estatistica.Nota = estatistica.Acertos;



                        return estatistica;

                    }
                }

            }
            catch
            {

                throw;
            }

        }

        public RankingSimuladoAluno GetRankingObjetiva(int idClient, int idSimulado, string especialidade, string unidades, string localidade = "")
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxDir = new DesenvContext())
                    {
                        using(MiniProfiler.Current.Step("Obtendo ranking objetiva"))
                        {       
                            var ranking = new RankingSimuladoAluno();

                            var aluno = ctxDir.tblPersons.FirstOrDefault(p => p.intContactID == idClient);

                            ranking.NickName = string.IsNullOrEmpty(aluno.txtNickName)
                                ? Utilidades.GetNickName(aluno.txtName.Trim())
                                : aluno.txtNickName;

                            ranking.Especialidade = ctx.tblSimulado.FirstOrDefault(s => s.intSimuladoID == idSimulado).txtSimuladoDescription.Split('-')[1].Trim();
                            ranking.Simulado = GetSimuladoRealizacao(idClient, idSimulado);
                            ranking.Nota = "-";
                            ranking.Posicao = "-";

                            var lista = GetRankingParcial(
                                idSimulado,
                                unidades ?? string.Empty,
                                localidade ?? string.Empty,
                                especialidade ?? string.Empty,
                                string.Empty,
                                idClient);

                            ranking.QuantidadeParticipantes = lista.Count();

                            var rankingSimuladoAluno = lista.Where(x => x.intClientID == idClient)
                                .Select(x => new RankingSimulado()
                                {
                                    Acertos = x.intAcertos ?? 0,
                                    Especialidade = x.txtEspecialidade ?? string.Empty,
                                    Filial = x.txtUnidade ?? string.Empty,
                                    NickName = x.txtNickName != null ? x.txtNickName.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "") : string.Empty,
                                    NotaFinal = x.dblNotaFinal != null ? Math.Round(x.dblNotaFinal.Value, 2) : 0.0,
                                    Posicao = x.txtPosicao ?? string.Empty,
                                    IdUf = x.intStateID ?? 0,
                                    Uf = string.IsNullOrEmpty(x.txtUnidade) || x.txtUnidade.IndexOf('(') < 0 ? x.txtUnidade : x.txtUnidade.Substring(x.txtUnidade.IndexOf('(')).Replace("(", "").Replace(")", ""),
                                    Id = x.intClientID ?? 0
                                }).FirstOrDefault();

                            var al = rankingSimuladoAluno;
                            if (al != null)
                            {
                                ranking.Nota = al.NotaObjetiva == 0 ? al.NotaFinal.ToString() : al.NotaObjetiva.ToString();
                                ranking.Posicao = al.Posicao;
                            }

                            return ranking;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Simulado GetSimuladoRealizacao(int idClient, int idSimulado)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    var simuladoRealizado = new Simulado();

                    var historico = ctx.tblExercicio_Historico.Where(p => p.intClientID == idClient && p.intExercicioID == idSimulado && (p.bitRealizadoOnline == true || p.bitPresencial == true)).FirstOrDefault();
                    if (historico == null) return simuladoRealizado;

                    simuladoRealizado.DtHoraInicio = historico.dteDateInicio;
                    simuladoRealizado.DtHoraFim = historico.dteDateFim;

                    return simuladoRealizado;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsFaseFinalLiberado(int idSimulado)
        {
            using (var ctx = new AcademicoContext())
            {
                using(MiniProfiler.Current.Step("Verificando se fase final está liberada"))
                {
                    var isLiberado = ctx.tblSimuladoRanking_Fase02.Where(p => p.intSimuladoID == idSimulado).Any();
                    return isLiberado;
                }
            }
        }

        public RankingSimuladoAluno GetRankingObjetivaCache(int idClient, int idSimulado, string especialidade, string unidades, string localidade = "")
        {
            try
            {
                var key = String.Format("{0}:{1}:{2}:{3}:{4}:{5}", RedisCacheConstants.Simulado.KeyRankingSimulado, idClient, idSimulado, especialidade, unidades, localidade);
                var ranking = RedisCacheManager.GetItemObject<RankingSimuladoAluno>(key);

                if (ranking == null)
                {
                    ranking = GetRankingObjetiva(idClient, idSimulado, especialidade, unidades, localidade);
                    if (ranking != null)
                    {
                        var timeoutRankingHour = 1;
                        RedisCacheManager.SetItemObject(key, ranking, TimeSpan.FromHours(timeoutRankingHour));
                    }
                }

                return ranking;
            }
            catch (Exception)
            {
                return GetRankingObjetiva(idClient, idSimulado, especialidade, unidades, localidade);
            }
        }

        public List<RankingDTO> GetRankingParcial(int idSimulado, string txtUnidade = "", string txtLocal = "", string txtEspecialidade = "", string txtStore = "", int matricula = 0)
        {
            using (var ctx = new AcademicoContext())
            {
                int[] unidade = string.IsNullOrEmpty(txtUnidade) || txtUnidade.ToUpper() == "TODOS" ? new int[0] : txtUnidade.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                int[] intStoreID = string.IsNullOrEmpty(txtStore) || txtStore.ToUpper() == "TODOS" ? null : Array.ConvertAll(txtStore.Split(','), Int32.Parse);

                var rankingFase01 = (from rf1 in ctx.tblSimuladoRanking_Fase01
                                     join s in ctx.tblSimulado
                                       on rf1.intSimuladoID equals s.intSimuladoID
                                     where s.intSimuladoID == idSimulado
                                     && (txtLocal.ToUpper() == "TODOS" || txtLocal == "" || (txtLocal == "Rio de Janeiro" ? (rf1.txtLocal.Contains("Barra") || rf1.txtLocal.Contains("Tijuca")) : rf1.txtLocal.Contains(txtLocal)) || rf1.intClientID == matricula)
                                     && (txtUnidade.ToUpper() == "TODOS" || txtUnidade == "" || unidade.Contains(rf1.intStateID ?? 0) || rf1.intClientID == matricula)
                                     && (txtEspecialidade.ToUpper() == "TODOS" || txtEspecialidade == "" || rf1.txtEspecialidade == txtEspecialidade || rf1.intClientID == matricula)
                                     select new
                                     {
                                         rf1.intSimuladoID,
                                         rf1.txtPosicao,
                                         rf1.intAcertos,
                                         rf1.dblNotaProvaDiscursiva,
                                         rf1.dblNotaObjetiva,
                                         rf1.dblNotaDiscursiva,
                                         dblNotaFinal = rf1.dblNotaFinal * ((s.intPesoProvaObjetiva ?? 100) * 0.01),
                                         rf1.intClientID,
                                         rf1.txtUnidade,
                                         rf1.txtLocal,
                                         rf1.txtName,
                                         rf1.txtEspecialidade,
                                         rf1.intStateID
                                     }).ToList();

                using (var ctxMatDir = new DesenvContext())
                {
                    var listIntClientID = rankingFase01.Select(x => x.intClientID).Distinct().ToList();
                    var listUnidade = rankingFase01.Select(x => x.txtUnidade).Distinct().ToList();

                    var stores = (from s in ctxMatDir.tblStores
                                  where listUnidade.Contains(s.txtStoreName)
                                  select s).ToList();

                    int posicao = 1;
                    var ranking = (from r in rankingFase01
                                   join s in stores
                                     on r.txtUnidade.ToUpper() equals s.txtStoreName.ToUpper() 
                                   where
                                     txtStore == null 
                                     || txtStore == "" 
                                     || intStoreID.Contains(s.intStoreID)
                                     || r.intClientID == matricula
                                   orderby r.dblNotaFinal descending, r.intClientID
                                   select new 
                                   {
                                       intSimuladoID = r.intSimuladoID,
                                       intPosicao = posicao++,
                                       intAcertos = r.intAcertos,
                                       dblNotaProvaDiscursiva = r.dblNotaProvaDiscursiva,
                                       dblNotaObjetiva = r.dblNotaObjetiva,
                                       dblNotaDiscursiva = r.dblNotaDiscursiva,
                                       dblNotaFinal = r.dblNotaFinal,
                                       intClientID = r.intClientID,
                                       txtUnidade = r.txtUnidade.Replace("EAD Assinaturas", "Ensino A  Distância (EAD)"),
                                       txtLocal = r.txtLocal,
                                       txtName = r.txtName,
                                       txtEspecialidade = r.txtEspecialidade,
                                       intStateID = r.intStateID,
                                       txtNickName = String.Empty 
                                   })
                                   .ToList();

                    var notasAgrupadas = ranking
                        .GroupBy(x => x.dblNotaFinal)
                        .Select(x => new { dblNotaFinal = x.Key, minPosicao = x.Min(r => r.intPosicao) })
                        .ToList();

                    var rankingCorrigido = (
                        from r in ranking
                        join agrupadas in notasAgrupadas on r.dblNotaFinal equals agrupadas.dblNotaFinal
                        select new RankingDTO()
                        {
                            intSimuladoID = r.intSimuladoID,
                            txtPosicao = agrupadas.minPosicao.ToString() + "º",
                            intAcertos = r.intAcertos,
                            dblNotaProvaDiscursiva = r.dblNotaProvaDiscursiva,
                            dblNotaObjetiva = r.dblNotaObjetiva,
                            dblNotaDiscursiva = r.dblNotaDiscursiva,
                            dblNotaFinal = r.dblNotaFinal,
                            intClientID = r.intClientID,
                            txtUnidade = r.txtUnidade,
                            txtLocal = r.txtLocal,
                            txtName = r.txtName,
                            txtEspecialidade = r.txtEspecialidade,
                            intStateID = r.intStateID,
                            txtNickName = r.txtNickName
                        }
                    ).ToList();

                    return rankingCorrigido;

                }                
            }
        }

        public AlunoConcursoEstatistica GetSimuladoConsolidado(int matricula, int idSimulado)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblSimuladoOnline_Consolidado
                          .Where(x => x.intClientID == matricula && x.intSimuladoID == idSimulado)
                          .Select(x => new AlunoConcursoEstatistica
                          {
                              Acertos = x.intCertas,
                              Erros = x.intErradas,
                              NaoRealizadas = x.intNaoRealizadas,
                              Nota = x.intCertas,
                              TotalQuestoes = x.intCertas + x.intErradas + x.intNaoRealizadas
                          })
                          .FirstOrDefault();
            }
        }

        public void InsertSimuladoConsolidado(int matricula, int idSimulado, AlunoConcursoEstatistica estatisticasAlunoRankingOnline)
        {
            var entity = new tblSimuladoOnline_Consolidado()
            {
                intClientID = matricula,
                intSimuladoID = idSimulado,
                intCertas = estatisticasAlunoRankingOnline.Acertos,
                intErradas = estatisticasAlunoRankingOnline.Erros,
                intNaoRealizadas = estatisticasAlunoRankingOnline.NaoRealizadas,
                dteDataCriacao = DateTime.Now
            };

            using (var ctx = new AcademicoContext())
            {
                ctx.tblSimuladoOnline_Consolidado.Add(entity);
                ctx.SaveChanges();
            }
        }

        public RankingSimuladoAluno GetRanking(int matricula, int idSimulado, string especialidade, string unidades, int idAplicacao)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obter ranking simulado."))
                {
                    tblPersons aluno;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        aluno = ctxMatDir.tblPersons.FirstOrDefault(p => p.intContactID == matricula);
                    }

                    using (var ctx = new AcademicoContext())
                    {
                        var ranking = new RankingSimuladoAluno();

                        ranking.NickName = string.IsNullOrEmpty(aluno.txtNickName)
                            ? Utilidades.GetNickName(aluno.txtName.Trim())
                            : aluno.txtNickName;

                        ranking.Especialidade = ctx.tblSimulado.FirstOrDefault(s => s.intSimuladoID == idSimulado).txtSimuladoDescription.Split('-')[1].Trim();
                        ranking.Simulado = new Simulado { Nome = ctx.tblSimulado.FirstOrDefault(s => s.intSimuladoID == idSimulado).txtSimuladoName };
                        ranking.Nota = "-";
                        ranking.Posicao = "-";

                        var lstRankingSimulado = GetRankingParcial(
                            idSimulado,
                            string.IsNullOrEmpty(unidades) ? string.Empty : unidades,
                            string.Empty,
                            string.IsNullOrEmpty(especialidade) ? string.Empty : especialidade,
                            string.Empty,
                            matricula);

                        ranking.RankingSimulado = lstRankingSimulado.Select(x => new RankingSimulado()
                        {
                            Id = x.intClientID ?? 0,
                            Especialidade = x.txtEspecialidade ?? string.Empty,
                            NickName = x.txtNickName != null ? x.txtNickName.ToString().Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "") : string.Empty,
                            NotaObjetiva = x.dblNotaFinal != null ? Math.Round(x.dblNotaFinal.Value, 2) : 0.0,
                            Posicao = x.txtPosicao ?? string.Empty,
                            IdUf = x.intStateID ?? 0,

                            Uf = !string.IsNullOrEmpty(x.txtUnidade) && x.txtUnidade.IndexOf('(') > 0 ? x.txtUnidade.Substring(x.txtUnidade.IndexOf('(')).Replace("(", "").Replace(")", "") : x.txtUnidade,
                        }).ToList();

                        if (ranking.RankingSimulado != null)
                        {
                            var al = ranking.RankingSimulado.FirstOrDefault(r => r.Id == matricula);
                            if (al != null)
                            {
                                ranking.Nota = al.NotaObjetiva.ToString();
                                ranking.Posicao = al.Posicao;
                            }
                        }

                        return ranking;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public RankingSimuladoAluno GetRankingFinal(int idClient, int idSimulado, string especialidade, string unidades)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo ranking final"))
                {
                    tblPersons aluno;
                    using (var ctxMatDir = new DesenvContext())
                    {
                        aluno = ctxMatDir.tblPersons.FirstOrDefault(p => p.intContactID == idClient);
                    }

                    using (var ctx = new AcademicoContext())
                    {
                        var ranking = new RankingSimuladoAluno();

                        ranking.NickName = string.IsNullOrEmpty(aluno.txtNickName)
                                        ? Utilidades.GetNickName(aluno.txtName.Trim())
                                        : aluno.txtNickName;


                        ranking.Especialidade = ctx.tblSimulado.FirstOrDefault(s => s.intSimuladoID == idSimulado).txtSimuladoDescription.Split('-')[1].Trim();
                        ranking.Simulado = new Simulado
                        {
                            Nome = ctx.tblSimulado.FirstOrDefault(s => s.intSimuladoID == idSimulado).txtSimuladoName
                        };
                        ranking.Nota = "-";
                        ranking.Posicao = "-";

                        var lstRankingSimulado = GetRankingFinal(
                            idSimulado,
                            unidades ?? string.Empty,
                            string.Empty,
                            especialidade ?? string.Empty,
                            string.Empty,
                            string.Empty,
                            idClient);

                        ranking.RankingSimulado = lstRankingSimulado.Select(x => new RankingSimulado()
                        {
                            Id = x.intClientID ?? 0,
                            Especialidade = x.txtEspecialidade ?? string.Empty,
                            NickName = x.txtNickName != null ? x.txtNickName.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "") : string.Empty,
                            NotaObjetiva = x.dblNotaObjetiva != null ? Math.Round(x.dblNotaObjetiva.Value, 2) : 0,
                            NotaDiscursiva = x.dblNotaDiscursiva != null ? Math.Round(x.dblNotaDiscursiva.Value, 2) : 0,
                            NotaFinal = x.dblNotaFinal != null ? Math.Round(x.dblNotaFinal.Value, 2) : 0,
                            Posicao = x.txtPosicao ?? string.Empty,
                            IdUf = x.intStateID ?? 0,
                            Uf = x.txtUnidade.IndexOf('(') > 0 ? x.txtUnidade.Substring(x.txtUnidade.IndexOf('(')).Replace("(", "").Replace(")", "") : ""
                        }).ToList();

                        if (ranking.RankingSimulado != null)
                        {
                            var al = ranking.RankingSimulado.FirstOrDefault(r => r.Id == idClient);
                            if (al != null)
                            {
                                ranking.Nota = al.NotaFinal.ToString();
                                ranking.Posicao = al.Posicao;
                            }
                        }

                        return ranking;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<RankingDTO> GetRankingFinal(int idSimulado, string txtUnidade = "", string txtLocal = "", string txtEspecialidade = "", string txtNickName = "", string txtStore = "", int matricula = 0)
        { 
            using (var ctx = new AcademicoContext())
            {
                int[] unidade = string.IsNullOrEmpty(txtUnidade) || txtUnidade.ToUpper() == "TODOS" ? new int[0] : txtUnidade.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                int[] intStoreID = string.IsNullOrEmpty(txtStore) || txtStore.ToUpper() == "TODOS" ? null : Array.ConvertAll(txtStore.Split(','), Int32.Parse);

                var rankingFase02 = (from r in ctx.tblSimuladoRanking_Fase02
                                     join s in ctx.tblSimulado
                                       on r.intSimuladoID equals s.intSimuladoID
                                     where s.intSimuladoID == idSimulado
                                     && (txtLocal.ToUpper() == "TODOS" || txtLocal == "" || (txtLocal == "Rio de Janeiro" ? (r.txtLocal.Contains("Barra") || r.txtLocal.Contains("Tijuca")) : r.txtLocal.Contains(txtLocal)) || r.intClientID == matricula)
                                     && (txtUnidade.ToUpper() == "TODOS" || txtUnidade == "" || unidade.Contains(r.intStateID ?? 0) || r.intClientID == matricula)
                                     && (txtEspecialidade.ToUpper() == "TODOS" || txtEspecialidade == "" || r.txtEspecialidade == txtEspecialidade || r.intClientID == matricula)
                                     select new
                                     {
                                         r.intSimuladoID,
                                         r.txtPosicao,
                                         r.intAcertos,
                                         r.dblNotaProvaDiscursiva,
                                         r.dblNotaObjetiva,
                                         r.dblNotaDiscursiva,
                                         r.dblNotaFinal,
                                         r.intClientID,
                                         r.txtUnidade,
                                         r.txtLocal,
                                         r.txtName,
                                         r.txtEspecialidade,
                                         r.intStateID
                                     }).ToList();
                using (var ctxMatDir = new DesenvContext())
                {
                    var listIntClientID = rankingFase02.Select(x => x.intClientID).Distinct().ToList();
                    var listUnidade = rankingFase02.Select(x => x.txtUnidade).Distinct().ToList();

                    var persons = (from p in ctxMatDir.tblPersons
                                   where listIntClientID.Contains(p.intContactID)
                                   select new { 
                                       p.intContactID, 
                                       txtNickName = p.txtNickName != null ? p.txtNickName.Trim() : null
                                   }).ToList();

                    var stores = (from s in ctxMatDir.tblStores
                                  where listUnidade.Contains(s.txtStoreName)
                                  select s).ToList();

                    int posicao = 1;
                    var ranking = (from r in rankingFase02
                                   join p in persons
                                     on r.intClientID equals p.intContactID
                                   join s in stores
                                     on r.txtUnidade.ToUpper() equals s.txtStoreName.ToUpper()
                                   where (txtNickName == null || txtNickName == "" || p.txtNickName == txtNickName)
                                     && (txtStore == null || txtStore == "" || intStoreID.Contains(s.intStoreID) || r.intClientID == matricula)
                                   orderby r.dblNotaFinal descending, r.intClientID
                                   select new
                                   {
                                       intSimuladoID = r.intSimuladoID,
                                       intPosicao = posicao++,
                                       intAcertos = r.intAcertos,
                                       dblNotaProvaDiscursiva = r.dblNotaProvaDiscursiva,
                                       dblNotaObjetiva = r.dblNotaObjetiva,
                                       dblNotaDiscursiva = r.dblNotaDiscursiva,
                                       dblNotaFinal = r.dblNotaFinal,
                                       intClientID = r.intClientID,
                                       txtUnidade = r.txtUnidade,
                                       txtLocal = r.txtLocal,
                                       txtName = r.txtName,
                                       txtEspecialidade = r.txtEspecialidade,
                                       intStateID = r.intStateID,
                                       txtNickName = p.txtNickName != null ? p.txtNickName.Trim() : String.Empty
                                   }).ToList();

                    var notasAgrupadas = ranking
                        .GroupBy(x => x.dblNotaFinal)
                        .Select(x => new { dblNotaFinal = x.Key, minPosicao = x.Min(r => r.intPosicao) })
                        .ToList();

                    var rankingCorrigido = (
                        from r in ranking
                        join agrupadas in notasAgrupadas on r.dblNotaFinal equals agrupadas.dblNotaFinal
                        select new RankingDTO()
                        {
                            intSimuladoID = r.intSimuladoID,
                            txtPosicao = agrupadas.minPosicao.ToString() + "º",
                            intAcertos = r.intAcertos,
                            dblNotaProvaDiscursiva = r.dblNotaProvaDiscursiva,
                            dblNotaObjetiva = r.dblNotaObjetiva,
                            dblNotaDiscursiva = r.dblNotaDiscursiva,
                            dblNotaFinal = r.dblNotaFinal,
                            intClientID = r.intClientID,
                            txtUnidade = r.txtUnidade,
                            txtLocal = r.txtLocal,
                            txtName = r.txtName,
                            txtEspecialidade = r.txtEspecialidade,
                            intStateID = r.intStateID,
                            txtNickName = r.txtNickName
                        }
                    ).ToList();

                    return rankingCorrigido;
                }            
            }
        }

        public AlunoConcursoEstatistica GetEstatisticaAlunoSimuladoModoProva(int idMatricula, int idExercicio, int idHistorico)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    var questoes = (from sq in ctx.tblQuestao_Simulado
                                    join q in ctx.tblQuestoes
                                    on sq.intQuestaoID equals q.intQuestaoID
                                    join qa in ctx.tblQuestaoAlternativas
                                    on sq.intQuestaoID equals qa.intQuestaoID
                                    where sq.intSimuladoID == idExercicio
                                    && (q.bitCasoClinico != "1")
                                    select new
                                    {
                                        questaoId = qa.intQuestaoID,
                                        alternativa = qa.txtLetraAlternativa,
                                        alternativaCorreta = qa.bitCorreta,
                                        anulada = q.bitAnulada
                                    }
                                    ).ToList();

                    var qtdeQuestoesAnuladas = questoes.Where(x => x.anulada).Select(x => x.questaoId).Distinct().Count();
                    var questoesNaoAnuladas = questoes.Where(x => x.anulada == false && x.alternativaCorreta == true).ToList();

                    var estatistica = new AlunoConcursoEstatistica();

                    var respostasAluno = new List<object>().Select(t => new { questaoId = default(int), alternativa = default(string) }).ToList();

                    int[] intTipoExercicio = new int[] { (int)Exercicio.tipoExercicio.SIMULADO, (int)Exercicio.tipoExercicio.MONTAPROVA };
                    respostasAluno = (from eh in ctx.tblExercicio_Historico
                                      join cro in ctx.tblCartaoResposta_objetiva
                                      on eh.intHistoricoExercicioID equals cro.intHistoricoExercicioID
                                      where eh.intClientID == idMatricula && intTipoExercicio.Contains(eh.intExercicioTipo) && eh.intHistoricoExercicioID == idHistorico
                                      select new { questaoId = cro.intQuestaoID, alternativa = cro.txtLetraAlternativa, historicoExercicioId = eh.intHistoricoExercicioID }
                                    ).ToList()
                                    .OrderByDescending(x => x.historicoExercicioId)
                                    .GroupBy(x => x.questaoId)
                                    .Select(y => new { questaoId = y.FirstOrDefault().questaoId, alternativa = y.FirstOrDefault().alternativa }).ToList();


                    var respostasAlunoNaoAnuladas = (from ra in respostasAluno
                                                     join q in questoesNaoAnuladas
                                                     on ra.questaoId equals q.questaoId
                                                     where q.anulada == false
                                                     select new { questaoId = ra.questaoId, alternativa = ra.alternativa }
                                                     ).Distinct().ToList();

                    estatistica.TotalQuestoes = questoes.GroupBy(x => x.questaoId).Count();
                    estatistica.NaoRealizadas = estatistica.TotalQuestoes - (respostasAlunoNaoAnuladas.Count() + qtdeQuestoesAnuladas);
                    estatistica.Acertos = ((from q in questoesNaoAnuladas
                                            join r in respostasAlunoNaoAnuladas
                                            on q.questaoId equals r.questaoId
                                            where q.alternativa == r.alternativa
                                            select q.questaoId
                                            ).Count() + qtdeQuestoesAnuladas);

                    estatistica.Erros = (from q in questoesNaoAnuladas
                                         join r in respostasAlunoNaoAnuladas
                                         on q.questaoId equals r.questaoId
                                         where q.alternativa != r.alternativa
                                         select q.questaoId
                                         ).Count();

                    estatistica.Nota = estatistica.Acertos;



                    return estatistica;

                }


            }
            catch
            {

                throw;
            }
        }

        public RankingSimuladoAluno GetResultadoRankingAluno(int matricula, int idSimulado, int idAplicacao, string especialidade, string unidades, string localidade = "")
        {
            try
            {
                var simuladoConsolidado = GetSimuladoConsolidado(matricula, idSimulado);
                var result = new RankingSimuladoAluno();

                if (simuladoConsolidado == null)
                {
                    result.EstatisticasAlunoRankingOnline = GetEstatisticaAlunoSimulado(matricula, idSimulado, true);

                    InsertSimuladoConsolidado(matricula, idSimulado, result.EstatisticasAlunoRankingOnline);
                }
                else
                    result.EstatisticasAlunoRankingOnline = GetEstatisticaAlunoSimulado(matricula, idSimulado, true);

                result.EstatisticasAlunoRankingEstudo = GetEstatisticaAlunoSimulado(matricula, idSimulado, false);

                var local = string.IsNullOrEmpty(localidade) ? string.Empty : (localidade.IndexOf('(') > 0 ? localidade.Substring(0, localidade.IndexOf('(')).Trim() : localidade);

                var rank = GetRankingObjetiva(matricula, idSimulado, especialidade, unidades, local);

                if (rank != null)
                {
                    result.Nota = rank.Nota;
                    result.Posicao = rank.Posicao;
                    result.DataRealizacao = rank.Simulado.DtHoraInicio;
                    result.QuantidadeParticipantes = rank.QuantidadeParticipantes;
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FiltroRanking GetFiltroRankingSimulado(int idSimulado)
        {
            try
            {
                var filtroRanking = new FiltroRanking();
                var especialidades = new Especialidades();
                var estados = new Estados();
                var filiais = new Filiais();
                var ranking = GetRankingParcial(idSimulado);
                foreach (var item in ranking)
                {
                    especialidades.Add(new Especialidade { Descricao = item.txtEspecialidade });

                    var hasSigla = item.txtUnidade.Any(x => x.Equals('('));
                    estados.Add(new Estado
                    {
                        Sigla = hasSigla ? item.txtUnidade.Substring(item.txtUnidade.IndexOf('(')).Replace("(", "").Replace(")", "") : item.txtUnidade,
                        ID = (int)item.intStateID
                    });

                    filiais.Add(new Filial
                    {

                        Nome = item.txtUnidade == "MEDREADER" ? "MEDREADER" : item.txtUnidade,
                        EstadoID = (int)item.intStateID
                    });
                }

                var estadoEAD = -1;

                filtroRanking.Especialidades.AddRange(especialidades.GroupBy(e => new { e.Descricao }).Select(g => g.First()).ToList());
                filtroRanking.Estados.AddRange(estados.GroupBy(e => new { e.Sigla, e.ID }).Select(g => g.First()).ToList());
                filtroRanking.Unidades.AddRange(filiais.Where(e => e.EstadoID != estadoEAD).GroupBy(e => new { e.Nome, e.ID, e.EstadoID }).Select(g => g.First()).ToList());

                return filtroRanking;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SimuladoDTO GetSimulado(int intSimuladoID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (
                    from s in ctx.tblSimulado
                    where s.intSimuladoID == intSimuladoID
                    select new SimuladoDTO()
                    {
                        ID = s.intSimuladoID,
                        ExercicioName = s.txtSimuladoName,
                        Descricao = s.txtSimuladoDescription,
                        Ano = s.intAno ?? 0,
                        DataInicio = s.dteDataHoraInicioWEB,
                        DataFim = s.dteDataHoraTerminoWEB,
                        TipoId = s.intTipoSimuladoID,
                        Online = s.bitOnline,
                        Duracao = s.intDuracaoSimulado,
                        DtLiberacaoRanking = s.dteInicioConsultaRanking,
                        QuantidadeQuestoes = s.intQtdQuestoes,
                        PesoProvaObjetiva = s.intPesoProvaObjetiva
                    }
                ).FirstOrDefault();
            }
        }

        public List<SimuladoResultadosDTO> ListResultado(int intSimuladoID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (
                    from res in ctx.tblSimuladoResultados
                    where res.intSimuladoID == intSimuladoID
                    select new SimuladoResultadosDTO()
                    {
                        intClientID = res.intClientID,
                        intSimuladoID = res.intSimuladoID,
                        intVersaoID = res.intVersaoID,
                        intAcertos = res.intAcertos,
                        intArquivoID = res.intArquivoID,
                    }
                ).ToList();
            }
        }

        public List<DadosOrdemVendaDTO> GetOrdemVendaTodosClientes(int intYear)
        {
            var statusOrdem = new int[] { (int)Utilidades.ESellOrderStatus.Pendente, (int)Utilidades.ESellOrderStatus.Ativa };

            using (var ctxMatDir = new DesenvContext())
            {
                return (
                    from ordemVenda in ctxMatDir.tblSellOrders
                    join ordemVendaDet in ctxMatDir.tblSellOrderDetails on ordemVenda.intOrderID equals ordemVendaDet.intOrderID
                    join courses in ctxMatDir.tblCourses on ordemVendaDet.intProductID equals courses.intCourseID
                    join prod in ctxMatDir.tblProducts on ordemVendaDet.intProductID equals prod.intProductID
                    join stores in ctxMatDir.tblStores on ordemVenda.intStoreID equals stores.intStoreID
                    join person in ctxMatDir.tblPersons on ordemVenda.intClientID equals person.intContactID
                    join city in ctxMatDir.tblCities on stores.intCityID equals city.intCityID into leftCity
                    from city in leftCity.DefaultIfEmpty()
                    join prodGroup in ctxMatDir.tblProductGroups1 on prod.intProductGroup1 equals prodGroup.intProductGroup1ID
                    join cli in ctxMatDir.tblClients on ordemVenda.intClientID equals cli.intClientID
                    join classRooms in ctxMatDir.tblClassRooms on courses.intPrincipalClassRoomID equals classRooms.intClassRoomID
                    where
                        courses.intYear == intYear
                        && (
                            statusOrdem.Contains(ordemVenda.intStatus ?? -1)
                            || (
                                from a in ctxMatDir.tblAlunosAnoAtualMaisAnterior
                                where a.intClientID == ordemVenda.intClientID
                                select a.intClientID
                            ).Any()
                        )
                        && ordemVenda.intStoreID != MEDREADER //removendo turma MEDREADER            
                    //&& !(from bloqueado in ctxMatDir.tblSimuladoRanking_AlunosBloqueados 
                    //     where ordemVenda.intClientID == bloqueado.intClientID
                    //     select bloqueado.intClientID).Any() //Novo contexto onde os temos alunos proibidos de serem rankeados (HUGO DIAS 22/5/2009)

                    select new DadosOrdemVendaDTO()
                    {
                        intOrderID = ordemVenda.intOrderID,
                        intClientID = ordemVenda.intClientID,
                        personName = person.txtName,
                        txtName = prod.txtName,
                        intProductGroup1ID = prodGroup.intProductGroup1ID,
                        txtDescription = prodGroup.txtDescription,
                        txtStoreName = stores.txtStoreName.Replace("Medcurso ", string.Empty).Replace("Medicine ", string.Empty),
                        cityIntState = city != null ? city.intState : 0,
                        intEspecialidadeID = cli.intEspecialidadeID
                    }
                ).ToList();
            }
        }

        public List<LogSimuladoAlunoTurmaDTO> GetLogSimuladoAlunoTurma(int intSimuladoID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (
                    from log in ctx.tblLogSimuladoAlunoTurma
                    where log.intSimuladoID == intSimuladoID
                    select new LogSimuladoAlunoTurmaDTO()
                    {
                        intSimuladoID = log.intSimuladoID,
                        intClientID = log.intClientID,
                        intOrderID = log.intOrderID,
                        txtUnidade = log.txtUnidade,
                        intState = log.intState,
                        txtTurma = log.txtTurma,
                        txtEspecialidade = log.txtEspecialidade,
                    }
                ).ToList();
            }
        }

        public void RemoverSimuladoRankingFase01(int intSimuladoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var qry = string.Format(@"delete from tblSimuladoRanking_Fase01 where intSimuladoid = {0}", intSimuladoID);
                var i = ctx.Database.ExecuteSqlRaw(qry);
            }
        }

        public void InserirSimuladoRankingFase01(List<SimuladoRankingFase01DTO> lista)
        {
            var dataTable = lista.Select(ranking => new tblSimuladoRanking_Fase01()
            {
                intSimuladoID = ranking.intSimuladoID,
                txtPosicao = ranking.txtPosicao,
                intAcertos = ranking.intAcertos,
                dblNotaObjetiva = ranking.dblNotaObjetiva,
                dblNotaDiscursiva = ranking.dblNotaDiscursiva,
                dblNotaFinal = ranking.dblNotaFinal,
                intClientID = ranking.intClientID,
                txtUnidade = ranking.txtUnidade,
                txtLocal = ranking.txtLocal,
                txtName = ranking.txtName,
                txtEspecialidade = ranking.txtEspecialidade,
                intStateID = ranking.intStateID,
                intArquivoID = ranking.intArquivoID,
            })
            .ToList()
            .ToDataTable();

            var connectionString = "";
            
            if (Utilidades.GetChaveamento())
                connectionString = ConfigurationProvider.Get("Settings:AcademicoRDS");
            else
                connectionString = ConfigurationProvider.Get("Settings:DesenvConnection");

            using (SqlBulkCopy sqlbc = typeof(tblSimuladoRanking_Fase01).CreateBulkCopy(connectionString, "tblSimuladoRanking_Fase01"))
            {
                sqlbc.WriteToServer(dataTable);
            }
        }

        public List<SimuladoRankingFase01DTO> GetRankingSimulado(int intSimuladoId)
        {
            using (var ctx = new AcademicoContext())
            {
                var ranking = (from r in ctx.tblSimuladoRanking_Fase01
                               where r.intSimuladoID == intSimuladoId
                               select new SimuladoRankingFase01DTO { 
                                    intSimuladoID = r.intSimuladoID
                                    , txtPosicao = r.txtPosicao
                                    , intAcertos = r.intAcertos
                                    , dblNotaObjetiva = r.dblNotaObjetiva
                                    , dblNotaDiscursiva = r.dblNotaDiscursiva
                                    , dblNotaFinal = r.dblNotaFinal
                                    , intClientID = r.intClientID
                                    , txtUnidade = r.txtUnidade
                                    , txtLocal = r.txtLocal
                                    , txtName = r.txtName
                                    , txtEspecialidade = r.txtEspecialidade
                                    , intStateID = r.intStateID
                                    , intArquivoID = r.intArquivoID
                               }).ToList();

                return ranking;
            }            
        }
    }
}