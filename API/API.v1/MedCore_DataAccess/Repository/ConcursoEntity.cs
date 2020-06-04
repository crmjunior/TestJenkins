using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Util;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using System.Data.SqlClient;
using MedCore_API.Academico;
using StackExchange.Profiling;
using MedCore_DataAccess.Contracts.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Repository
{
    public class ConcursoEntity : IConcursoData
    {
        public List<ConcursoDTO> GetConcursosPorProvas(int matricula, int idaplicacao, List<int> provas)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo concursos por provas"))
                {
                    new Util.Log().SetLog(new LogMsPro
                    {
                        Matricula = matricula,
                        IdApp = (Aplicacoes)idaplicacao,
                        Tela = Util.Log.MsProLog_Tela.CIBusca,
                        Acao = Util.Log.MsProLog_Acao.Abriu
                    });

                    PerfilAlunoEntity Aluno = new PerfilAlunoEntity();
                    bool IsR3 = Aluno.IsAlunoR3(matricula);
                    bool alunoInteresseR3 = IsR3 == false ? Aluno.AlunoTemInteresseRMais(matricula) : false;


                    using (var ctx = new DesenvContext())
                    {
                        var concursos = (from c in ctx.tblConcurso
                                        join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
                                        join cpt in ctx.tblConcurso_Provas_Tipos on cp.intProvaTipoID equals cpt.intProvaTipoID
                                        where
                                        c.VL_ANO_CONCURSO >= 2008 &&
                                        (cp.bitVendaLiberada.HasValue && cp.bitVendaLiberada.Value) &&
                                        (alunoInteresseR3 || IsR3 || !cpt.txtDescription.ToUpper().Contains("R3")) &&
                                        (provas.Contains(cp.intProvaID))
                                        select new ConcursoDTO
                                        {
                                            Descricao = c.NM_CONCURSO,
                                            Sigla = c.SG_CONCURSO,
                                            SiglaEstado = c.CD_UF
                                        })
                            .Distinct()
                            .OrderBy(x => x.Descricao)
                            .ToList();

                        return concursos;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Exercicio> GetProvas(string siglaConcurso, int matricula)
		{
			try
			{
                using(MiniProfiler.Current.Step("Obtendo provas"))
                {
                    var siglaRegularizada = Criptografia.ToRegularString(siglaConcurso);

                    PerfilAlunoEntity Aluno = new PerfilAlunoEntity();
                    bool IsR3 = Aluno.IsAlunoR3(matricula);
                    bool alunoInteresseR3 = IsR3 == false ? Aluno.AlunoTemInteresseRMais(matricula) : false;

                    using (var ctx = new DesenvContext())
                    {
                        var concursos = (from c in ctx.tblConcurso
                                join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
                                join cpt in ctx.tblConcurso_Provas_Tipos on cp.intProvaTipoID equals cpt.intProvaTipoID
                                where c.SG_CONCURSO == siglaRegularizada &&
                                    c.VL_ANO_CONCURSO >= 2008 &&
                                    (cp.bitVendaLiberada.HasValue && cp.bitVendaLiberada.Value)
                                        && (alunoInteresseR3 || IsR3 || !cpt.txtDescription.ToUpper().Contains("R3"))
                                        select new Exercicio
                                    {
                                        ExercicioName = c.SG_CONCURSO,
                                        Ano = (int)c.VL_ANO_CONCURSO,
                                        ID = cp.intProvaID,
                                        IdConcurso = c.ID_CONCURSO,
                                        TipoConcursoProva = cpt.txtDescription
                                    })
                            .ToList();
                        return concursos;
                    }
                }
			}
			catch
			{
				throw;
			}
		}

        public List<Exercicio> GetAll(int matricula, int idaplicacao)
		{
			try
			{
				// ======================== LOG
                using(MiniProfiler.Current.Step("Obtendo estastisticas de uma questão"))
                {
                    new Util.Log().SetLog(new LogMsPro
                                        {
                                            Matricula = matricula,
                                            IdApp = (Aplicacoes)idaplicacao,
                                            Tela = Util.Log.MsProLog_Tela.CIBusca,
                                            Acao = Util.Log.MsProLog_Acao.Abriu
                                        });
                    // ======================== 
                    // Alunos que não são R3 não podem ver concursos R3
                    PerfilAlunoEntity Aluno = new PerfilAlunoEntity();
                    bool IsR3 = Aluno.IsAlunoR3(matricula);
                    bool alunoInteresseR3 = IsR3 == false ? Aluno.AlunoTemInteresseRMais(matricula) : false;
                    

                    using (var ctx = new DesenvContext())
                    {
                        var concursos = (from c in ctx.tblConcurso
                                        join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
                                        join cpt in ctx.tblConcurso_Provas_Tipos on cp.intProvaTipoID equals cpt.intProvaTipoID
                                        where
                                        c.VL_ANO_CONCURSO >= 2008 &&
                                        (cp.bitVendaLiberada.HasValue && cp.bitVendaLiberada.Value) &&
                                        (alunoInteresseR3 || IsR3 || !cpt.txtDescription.ToUpper().Contains("R3")) // OU o aluno é R3, ou ele não vê concursos cuja descrição contém R3.
                                        select new Exercicio
                                        {
                                            ExercicioName = c.SG_CONCURSO.Trim(),
                                            Descricao = c.NM_CONCURSO,
                                            Ano = (int)c.VL_ANO_CONCURSO,
                                            SiglaEstado = c.CD_UF,
                                            TipoConcursoProva = cpt.txtDescription,
                                            IdConcurso = c.ID_CONCURSO
                                        }).ToList();


                        var concursosProvas = concursos.Select(c => new Exercicio
                                                                    {
                                                                        ExercicioName = c.ExercicioName,
                                                                        Descricao = c.Descricao,
                                                                        Ano = c.Ano,
                                                                        SiglaEstado = c.SiglaEstado,
                                                                        TipoConcursoProva = c.TipoConcursoProva.IndexOf("ACESSO DIRETO") > -1 ? "ACESSO DIRETO" : c.TipoConcursoProva,
                                                                        Tipo = GetTipoConcurso(c.TipoConcursoProva),
                                                                        IdConcurso = c.IdConcurso
                                                                    }).Distinct()
                                                                    .OrderBy(x => x.ExercicioName)
                                                                    .ToList();
                        return concursosProvas;
                    }
                }
			}
			catch (Exception)
			{
				throw;
			}
		}

        public string GetTipoConcurso(string descricaoConcurso)
        {
            string tipo = descricaoConcurso.IndexOf("ACESSO DIRETO") > -1 ? "R1"
                        : descricaoConcurso.IndexOf("R3") > -1 ? "R3"
                        : descricaoConcurso.IndexOf("R4") > -1 ? "R4"
                        : descricaoConcurso.IndexOf("REVALIDA") > -1 ? "REVALIDA"
                        : string.Empty;

            return tipo;
        }

        public int PermiteNotadeCorte(int provaId)
        {
            //Regra Atual: Verifica se é discursiva.
            using(MiniProfiler.Current.Step("Permitindo nota de corte"))
            {
                using (var ctx = new DesenvContext())
                {


                    var isDiscursiva = (from cp in ctx.tblConcurso_Provas
                                        join cpt in ctx.tblConcurso_Provas_Tipos
                                        on cp.intProvaTipoID equals cpt.intProvaTipoID
                                        where cp.intProvaID == provaId
                                        && cpt.bitDiscursiva == true
                                        select cpt.bitDiscursiva).Any();


                    return isDiscursiva ? 0: 1;

                }
            }
        }

        public List<Concurso> GetConcursosSiglas()
		{
			using (var ctx = new DesenvContext())
			{

				var lstConcurso = (from c in ctx.tblConcurso
					join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
					join cq in ctx.tblConcursoQuestoes on cp.intProvaID equals cq.intProvaID
					select new Concurso
						   {
							   Sigla = c.SG_CONCURSO.ToUpper().Trim()
						   }
				).Distinct().OrderBy(c => c.Sigla).ToList();

				return lstConcurso;
			}
		}

        public AlunoConcursoEstatistica GetConcursoStatsAluno(int idExercicio, int idMatricula, bool anulada = false)
        {
            int[] tipoExercicio = new int[] { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };

            try
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var questoes = (from cq in ctxMatDir.tblConcursoQuestoes
                                        join qa in ctxMatDir.tblConcursoQuestoes_Alternativas
                                        on cq.intQuestaoID equals qa.intQuestaoID
                                        where cq.intProvaID == idExercicio
                                        select new
                                        {
                                            questaoId = qa.intQuestaoID,
                                            alternativa = qa.txtLetraAlternativa,
                                            alternativaCorreta = (qa.bitCorreta == true || qa.bitCorretaPreliminar == true),
                                            anulada = (cq.bitAnulada || (cq.bitAnuladaPosRecurso.HasValue && cq.bitAnuladaPosRecurso.Value == true))
                                        }
                                    ).ToList();


                        var questoesNaoAnuladas = questoes.Where(x => x.anulada == false && x.alternativaCorreta == true).ToList();



                        var respostasAluno = (from cro in ctx.tblCartaoResposta_objetiva
                                              where cro.intClientID == idMatricula && tipoExercicio.Contains(cro.intExercicioTipoId)
                                              select new { questaoId = cro.intQuestaoID, alternativa = cro.txtLetraAlternativa, ultimoregistro = cro.dteCadastro }
                                              ).Distinct().ToList();



                        var respostasAlunoNaoAnuladas = (from ra in respostasAluno
                                                         join q in questoesNaoAnuladas
                                                         on ra.questaoId equals q.questaoId
                                                         select new { questaoId = ra.questaoId, alternativa = ra.alternativa, ra.ultimoregistro }
                                                         ).Distinct().ToList();

                        var respostasAlunosemduplicada = (from c in respostasAlunoNaoAnuladas
                                                          group c by c.questaoId into grp
                                                          where grp.Count() > 1
                                                          select grp.Key
                                                     ).ToList();

                        if (respostasAlunosemduplicada.Count > 0)
                        {
                            var itemComDuplicata = respostasAlunoNaoAnuladas.Where(a => respostasAlunosemduplicada.Contains(a.questaoId)).OrderBy(b => b.ultimoregistro).ToList();
                            if (itemComDuplicata.Count > 0)
                            {
                                foreach (var item in itemComDuplicata)
                                {
                                    var indice = respostasAlunoNaoAnuladas.Where(a => a.questaoId == item.questaoId && a.ultimoregistro > item.ultimoregistro).ToList();
                                    if (indice.Count > 0)
                                        respostasAlunoNaoAnuladas.Remove(item);
                                }

                            }
                        }

                        var estatistica = new AlunoConcursoEstatistica();

                        estatistica.TotalQuestoes = questoes.GroupBy(x => x.questaoId).Count();
                        estatistica.NaoRealizadas = estatistica.TotalQuestoes - respostasAlunoNaoAnuladas.Select(a => a.questaoId).Distinct().Count();

                        estatistica.Acertos = (from q in questoesNaoAnuladas
                                               join r in respostasAlunoNaoAnuladas
                                               on q.questaoId equals r.questaoId
                                               where q.alternativa == r.alternativa
                                               select q.questaoId
                                                ).Distinct().Count();

                        estatistica.Erros = respostasAlunoNaoAnuladas.Count() - estatistica.Acertos;

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

        public int GetExercicioIDPorMatricula(int matricula)
        {
            var tipoexercicio = new List<int>() { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };
            using (var ctx = new AcademicoContext())
            {
                var result = (from eh in ctx.tblExercicio_Historico
                              join cro in ctx.tblCartaoResposta_objetiva
                              on eh.intHistoricoExercicioID equals cro.intHistoricoExercicioID
                              where eh.intClientID == matricula && tipoexercicio.Contains(eh.intExercicioTipo)
                              select eh
                            ).OrderByDescending(b => b.intHistoricoExercicioID).Select(a => a.intExercicioID).FirstOrDefault();
                return result != 0 ? result : ctx.tblExercicio_Historico.Where(a => a.intClientID == matricula && tipoexercicio.Contains(a.intExercicioTipo)).OrderByDescending(b => b.intHistoricoExercicioID).Select(a => a.intExercicioID).FirstOrDefault();
            }
        }    

        public bool SetProvaSobDemanda(Prova prova)
        {
            using (var ctx = new DesenvContext())
            {
                var pr = ctx.tblConcurso_Provas.FirstOrDefault(p => p.intProvaID == prova.ID);
                if (pr != null)
                {
                    pr.bitSobDemanda = prova.SobDemanda;
	            }

                return ctx.SaveChanges() != 0;
            }
        }

        public bool GetProvaSobDemanda(int idProva)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcurso_Provas.Where(p => p.intProvaID == idProva).FirstOrDefault().bitSobDemanda;
            }
        }

		public virtual List<QuestaoConcursoAlunoDTO> GetQuestoesbyIdExercicio(int idExercicio)
		{

			using (var ctxMatDir = new DesenvContext())
			{
				var result = (from cq in ctxMatDir.tblConcursoQuestoes
							  join qa in ctxMatDir.tblConcursoQuestoes_Alternativas
							  on cq.intQuestaoID equals qa.intQuestaoID
							  where cq.intProvaID == idExercicio
							  select new QuestaoConcursoAlunoDTO()
							  {
								  QuestaoId = qa.intQuestaoID,
								  Alternativa = qa.txtLetraAlternativa,
								  AlternativaCorreta = (qa.bitCorreta == true || qa.bitCorretaPreliminar == true),
								  Anulada = (cq.bitAnulada || (cq.bitAnuladaPosRecurso.HasValue && cq.bitAnuladaPosRecurso.Value == true))
							  }
							 ).ToList();

				return result;
			}

		}
        public virtual List<RespostaConcursoAlunoDTO> GetRespostabyIdExercicioIDMatricula(int idExercicio, int idMatricula, int[] tipoExercicio)
        {

            using (var ctxMatDir = new AcademicoContext())
            {
                var respostas = (from cro in ctxMatDir.tblCartaoResposta_objetiva
                                 where cro.intClientID == idMatricula
                                 select new RespostaConcursoAlunoDTO() { QuestaoId = cro.intQuestaoID, Alternativa = cro.txtLetraAlternativa, Ultimoregistro = cro.dteCadastro }
                                   ).Distinct().ToList();

                return respostas;
            }
        }

        public int[] GetConcursosR3(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var idsPermitidos = ctx.Set<msp_Medsoft_SelectPermissaoExercicios_Result>().FromSqlRaw("msp_Medsoft_SelectPermissaoExercicios @bitVisitanteExpirado = {0}, @bitAlunoVisitante = {1}, @intClientID = {2}", false, false, matricula).ToList()
                    .Where(c => c.intExercicioTipo == (int)Exercicio.tipoExercicio.CONCURSO)
                    .Select(c => (int)c.intExercicioID)
                    .ToList();

                var concursos = (from c in ctx.tblConcurso
                                 join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
                                 join cpt in ctx.tblConcurso_Provas_Tipos on cp.intProvaTipoID equals cpt.intProvaTipoID
                                 where idsPermitidos.Contains(cp.intProvaID)
                                     && (cpt.txtDescription.ToUpper().Contains("R3") || cpt.txtDescription.ToUpper().Contains("R4"))
                                 select cp.intProvaID).Distinct().ToArray();

                return concursos;

            }

        }

        public int InserirConfiguracaoProvaAluno(tblProvaAlunoConfiguracoes configuracao)
        {
            using (var ctx = new DesenvContext())
            {
                ctx.tblProvaAlunoConfiguracoes.Add(configuracao);
                return ctx.SaveChanges();
            }
        }
    }
}