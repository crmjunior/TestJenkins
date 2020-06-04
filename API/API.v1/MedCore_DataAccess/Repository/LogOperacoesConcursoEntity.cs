using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Repository
{
    public class LogOperacoesConcursoEntity : ILogOperacoesConcursoData
    {
        public IList<HistoricoQuestaoConcursoDTO> ListarHistoricoQuestaoConcurso(int questaoID)
        {
            using (var ctx = new DesenvContext())
            {
                var result = (from l in ctx.tblLogOperacoesConcurso
                              join q in ctx.tblConcursoQuestoes on questaoID equals q.intQuestaoID
                              join e in ctx.tblEmployees on l.intEmployeeID equals e.intEmployeeID
                              join p in ctx.tblPersons  on e.intEmployeeID equals p.intContactID
                              where (l.intQuestaoID == q.intQuestaoID || l.intProvaID == q.intProvaID)
                              select new HistoricoQuestaoConcursoDTO
                              {
                                  DataAlteracao = l.dteDataAlteracao,
                                  Alteracao = l.txtDescricao,
                                  Login = e.txtLogin,
                                  NomeColaborador = p.txtName,
                                  AndamentoCadastro = l.intAndamentoCadastro.Value

                              }
                        ).ToList();

                return result;

            }
        }

        public Task<int> SetLogAlternativaAsync(int questaoId, int employeeID, string letra, TipoOperacoesConcursoEnum tipoAlteracao, AndamentoCadastroQuestao andamentoCadastroQuestao)
        {
            return new Task<int>(() =>
            {
                using (var ctx = new DesenvContext())
                {
                    var log = new tblLogOperacoesConcurso();
                    log.dteDataAlteracao = DateTime.Now;
                    log.intEmployeeID = employeeID;
                    log.intQuestaoID = questaoId;
                    log.txtDescricao = string.Format(tipoAlteracao.GetDescription(), letra);
                    log.intAndamentoCadastro = (int)andamentoCadastroQuestao;

                    ctx.tblLogOperacoesConcurso.Add(log);

                    return ctx.SaveChanges();
                }
            });
        }

        public Task<int> SetLogProvaAsync(int provaId, int employeeID, TipoOperacoesConcursoEnum tipoAlteracao, AndamentoCadastroQuestao andamentoCadastroQuestao)
        {
            return new Task<int>(() =>
            {
                using (var ctx = new DesenvContext())
                {
                    var log = new tblLogOperacoesConcurso();
                    log.dteDataAlteracao = DateTime.Now;
                    log.intEmployeeID = employeeID;
                    log.intProvaID = provaId;
                    log.txtDescricao = tipoAlteracao.GetDescription();
                    log.intAndamentoCadastro = (int)andamentoCadastroQuestao;

                    ctx.tblLogOperacoesConcurso.Add(log);

                    return ctx.SaveChanges();
                }
            });
        }

        public Task<int> SetLogAlteracaoPerfilAsync(int employeeID, TipoOperacoesConcursoEnum tipoAlteracao, string perfil, int colaboradorID)
        {
            return new Task<int>(() =>
            {
                using (var ctx = new DesenvContext())
                {
                    var colaborador = ctx.tblEmployees.SingleOrDefault(x => x.intEmployeeID == colaboradorID);

                    var log = new tblLogOperacoesConcurso();
                    log.dteDataAlteracao = DateTime.Now;
                    log.intEmployeeID = employeeID;
                    log.intProvaID = null;
                    log.txtDescricao = string.Format(tipoAlteracao.GetDescription(), colaborador.txtLogin ,  perfil);
                    log.intAndamentoCadastro = null;

                    ctx.tblLogOperacoesConcurso.Add(log);

                    return ctx.SaveChanges();
                }
            });
        }

        public Task<int> SetLogQuestaoAsync(int questaoId, int employeeID, TipoOperacoesConcursoEnum tipoAlteracao, AndamentoCadastroQuestao andamentoCadastroQuestao)
        {
            return new Task<int>(() =>
            {
                using (var ctx = new DesenvContext())
                {
                    var log = new tblLogOperacoesConcurso();
                    log.dteDataAlteracao = DateTime.Now;
                    log.intEmployeeID = employeeID;
                    log.intQuestaoID = questaoId;
                    log.txtDescricao = tipoAlteracao.GetDescription();
                    log.intAndamentoCadastro = (int)andamentoCadastroQuestao;

                    ctx.tblLogOperacoesConcurso.Add(log);

                    return ctx.SaveChanges();
                }
            });
        }

        public List<HistoricoAlteracaoPerfilDTO> ListarLogAlteracoesPerfis()
        {
            using (var ctx = new DesenvContext())
            {

                var result = (from l in ctx.tblLogOperacoesConcurso.Where(x => x.intProvaID == null && x.intQuestaoID == null)
                              join e in ctx.tblEmployees on l.intEmployeeID equals e.intEmployeeID
                              join p in ctx.tblPersons on e.intEmployeeID equals p.intContactID
                              select new HistoricoAlteracaoPerfilDTO
                              {
                                  DataAlteracao = l.dteDataAlteracao,
                                  Alteracao = l.txtDescricao,
                                  Login = e.txtLogin,
                                  NomeColaborador = p.txtName
                              }
                        ).ToList();

                return result;

            }
        }

        public Task<int> InserirLogAsync(TipoOperacoesConcursoEnum tipoAlteracao, int idEmployee, params object[] dados)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var ctx = new DesenvContext())
                {
                    var log = new tblLogOperacoesConcurso();
                    log.dteDataAlteracao = DateTime.Now;
                    log.intQuestaoID = default(int);
                    log.intEmployeeID = idEmployee;
                    log.txtDescricao = string.Format(tipoAlteracao.GetDescription(), dados);
                    ctx.tblLogOperacoesConcurso.Add(log);
                    return ctx.SaveChanges();
                }
            });
        }
    }
}