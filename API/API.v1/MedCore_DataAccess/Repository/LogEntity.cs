using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class LogEntity : IDataAccess<Log>
    {
        public List<Log> GetByFilters(Log registro)
        {
            throw new NotImplementedException();
        }

        public List<Log> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Insert(Log registro)
        {
            var ctx = new DesenvContext();
            var objLog = new tblIntensivaoLog
                         {
                             intClientID = registro.ClientID,
                             intFilialID = registro.FilialID,
                             intMarcadorID = registro.PaginaLog,
                             intProductID = registro.ProdutID,
                             txtPerfil = registro.Perfil.ToString(""),
                             intSessionID = registro.SessaoID,
                             dteTimeStamp = DateTime.Now
                         };
            ctx.tblIntensivaoLog.Add(objLog);
            ctx.SaveChanges();

            return 0;
        }

        public int Update(Log registro)
        {
            throw new NotImplementedException();
        }

        public int Delete(Log registro)
        {
            throw new NotImplementedException();
        }

        public int InsertSimuladoImpresso(LogSimuladoImpresso log)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var objLog = new tblLogAcoesSimuladoImpresso
                                 {
                                     intAplicationID = log.AplicacaoId,
                                     intSimuladoID = log.SimuladoId,
                                     intAcaoID = log.AcaoId,
                                     intClientID = log.Matricula,
                                     dteData = DateTime.Now
                                 };
                    ctx.tblLogAcoesSimuladoImpresso.Add(objLog);
                    ctx.SaveChanges();
                }

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int InsertAcessoLogin(LogLogin log)
        {
            using(MiniProfiler.Current.Step("Criando registro de acesso do usuario"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        var objLog = new tblLogAcessoLogin
                        {
                            intAplicacaoID = (int)log.AplicacaoId,
                            intClientID = log.Matricula,
                            dteDate = DateTime.Now,
                            intAcessoID = (int)log.AcessoId
                        };
                        ctx.tblLogAcessoLogin.Add(objLog);
                        ctx.SaveChanges();
                    }

                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public List<LogLogin> GetAcessoLoginAluno(int matricula, int idAplicacao)
        {
            var lstLogAcesso = new List<LogLogin>();
            using (var ctx = new DesenvContext())
            {
                lstLogAcesso = (from l in ctx.tblLogAcessoLogin
                                where (l.intClientID == matricula && l.intAplicacaoID == idAplicacao)
                                orderby l.dteDate descending
                                select new LogLogin()
                                {
                                    Matricula = l.intClientID,
                                    AplicacaoId = l.intAplicacaoID,
                                    AcessoId = l.intAcessoID

                                }).ToList();
            }

            return lstLogAcesso;
        }

        public int SetLogAcesso(LogLogin log)
        {
            return new LogEntity().InsertAcessoLogin(log);
        }

        public int InsertLogAcesso(Log log) 
        {
            try
            {
                using (var ctx = new DesenvContext())
                    ctx.Database.ExecuteSqlRaw("exec emed_insert_log_session @intModuleItemID, @intClientID, @txtIP, @txtItemDetail", 26, log.ClientID, log.IP, string.Empty);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }


        }
    }    
}