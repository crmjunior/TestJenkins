using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;


namespace MedCore_DataAccess.Repository
{
    public class ChamadoCallCenterEntity : IDataAccess<ChamadoCallCenter>, IChamadoCallCenterData
    {
        public bool ExisteChamadoInadimplenciaPrimeiroAvisoAberto(int matricula, int intOrderID)
        {
            var categoriaChamadoTermoAceiteInadimplencia = Constants.INADIMPLENCIA_CHAMADO_CATEGORIA;
            var statusFechadoPeloCliente = (int)ChamadoCallCenter.StatusChamado.FechadoPeloCliente;
            var statusInternoPrimeiroAviso = Constants.INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO;

            try
            {
                using (var ctx = new DesenvContext())
                {
                    var isChamadoAberto = (from c in ctx.tblCallCenterCalls
                                           join ci in ctx.tblCallCenterCallsInadimplencia on c.intCallCenterCallsID equals ci.intCallCenterCallsID
                                           where c.intCallCategoryID == categoriaChamadoTermoAceiteInadimplencia && c.intStatusID < statusFechadoPeloCliente && statusInternoPrimeiroAviso.Contains((int)c.intStatusInternoID)
                                           && c.intClientID == matricula
                                           && (intOrderID == 0 || ci.intOrderID == intOrderID)
                                           select new
                                           {
                                               ChamadoId = c.intCallCenterCallsID
                                           }).Any();

                    return isChamadoAberto;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool ExisteChamadoAberto(int idCallCategory, int matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var isChamadoAberto = (from c in ctx.tblCallCenterCalls
                                           where c.intCallCategoryID == idCallCategory && c.intStatusID != 7
                                           && c.intClientID == matricula
                                           select new
                                           {
                                               ChamadoId = c.intCallCenterCallsID
                                           }).Any();

                    return isChamadoAberto;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool ExisteChamadoInadimplenciaTermoAceiteAberto(int matricula, string ov = null)
        {
            var categoriaChamadoTermoAceiteInadimplencia = Constants.INADIMPLENCIA_CHAMADO_CATEGORIA;
            var statusFechadoPeloCliente = (int)ChamadoCallCenter.StatusChamado.FechadoPeloCliente;
            var statusInternoChamadoFechado = Constants.INADIMPLENCIA_CHAMADO_FECHADO;
            var dataAtual = DateTime.Now;

            try
            {
                using (var ctx = new DesenvContext())
                {
                    var isChamadoAberto = (from c in ctx.tblCallCenterCalls
                                           where c.intCallCategoryID == categoriaChamadoTermoAceiteInadimplencia && c.intStatusID < statusFechadoPeloCliente && c.intStatusInternoID != statusInternoChamadoFechado
                                           && c.intClientID == matricula
                                           && c.dteOpen.Year == dataAtual.Year
                                           && c.dteOpen.Month == dataAtual.Month
                                           && (ov == null || c.txtSubject.Contains(ov))
                                           select new
                                           {
                                               ChamadoId = c.intCallCenterCallsID
                                           }).Any();

                    return isChamadoAberto;
                }
            }
            catch
            {
                throw;
            }
        }

        public int InsertGenerico(ChamadoCallCenter registro)
        {
            using (var ctx = new DesenvContext())
            {

                var tblCallCenter = new tblCallCenterCalls
                {
                    intStatusInternoID = registro.IdStatusInterno,
                    intCallCategoryID = registro.IdCategoria,
                    intCallGroupID = registro.IdGrupoChamado,
                    dteOpen = DateTime.Now,
                    intClientID = registro.IdCliente,
                    bitNotify = registro.Notificar,
                    intSeverity = registro.Gravidade > 0 ? registro.Gravidade : (int)ChamadosCallCenterEnum.Gravidade.Normal,
                    intCourseID = registro.IdCurso > 0 ? registro.IdCurso : -1,
                    txtSubject = registro.Assunto.Length > 50 ? registro.Assunto.Substring(0,50) : registro.Assunto,
                    intStatusID = registro.Status > 0 ? registro.Status : (int)ChamadosCallCenterEnum.Status.Aberto,
                    intFirstEmployeeID = registro.AbertoPorIdFuncionario > 0 ? registro.AbertoPorIdFuncionario : Constants.MatriculaInternet_MGE,
                    intLastEmployeeID = registro.AbertoPorIdFuncionario > 0 ? registro.AbertoPorIdFuncionario : Constants.MatriculaInternet_MGE,
                    intDepartmentID = registro.IdDepartamentoOrigem > 0 ? registro.IdDepartamentoOrigem : (int)ChamadosCallCenterEnum.DepartamentoOrigem.Relacionamento,
                    txtDetail = registro.Detalhe,
                    intCallSectorID = registro.IdSetor == 0 ? 4 : registro.IdSetor,
                    dteDataPrevisao1 = registro.DataPrevista1,
                    dteDataPrevisao2 = registro.DataPrevista2,
                    intSectorComplementID = registro.IdComplementoSetor > 0 ? registro.IdComplementoSetor : -1
                };
                var dadosInseridos = ctx.tblCallCenterCalls.Add(tblCallCenter);
                ctx.SaveChanges();
                InserirEvento(dadosInseridos.Entity);


                return dadosInseridos.Entity.intCallCenterCallsID;
            }
        }

        public int InserirEvento(tblCallCenterCalls chamado)
        {

            var eventochamado = new ChamadoCallCenterEventos()
            {
                ID = chamado.intCallCenterCallsID,
                Status = chamado.intStatusID,
                IdStatusInterno = chamado.intStatusInternoID ?? 0,
                Assunto = chamado.txtSubject,
                Detalhe = chamado.txtDetail,
                AbertoPorIdFuncionario = 131220,
                Gravidade = chamado.intSeverity,
                IdSetor = chamado.intDepartmentID ?? 0,
                IdComplementoSetor = chamado.intSectorComplementID ?? -1,
                InformacaoInterna = false
            };
            return InsertCallCenterEvents(eventochamado);

        }

        public int InsertCallCenterEvents(ChamadoCallCenterEventos chamado)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var eventoChamado = new tblCallCenterEvents()
                    {
                        intCallCenterCallsID = chamado.ID,
                        intCallStatusID = chamado.Status,
                        intStatusInternoID = chamado.IdStatusInterno,
                        txtSubject = chamado.Assunto,
                        txtDetails = chamado.Detalhe,
                        intEmployeeID = chamado.AbertoPorIdFuncionario,
                        intSeverityID = chamado.Gravidade,
                        intSectorID = chamado.IdSetor,
                        bitInternalInformation = chamado.InformacaoInterna,
                        dteDate = DateTime.Now,
                        intSectorComplementID = chamado.IdComplementoSetor
                    };

                    ctx.tblCallCenterEvents.Add(eventoChamado);
                    ctx.SaveChanges();
                }
                return 1;
            }
            catch
            {
                throw;
            }

        }

        public int SetOvInadimplencia(int idChamado, int idOv, int idAplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var ovInadimplente = new tblCallCenterCallsInadimplencia
                {
                    intCallCenterCallsID = idChamado,
                    intCallCenterCallsIDRef = idChamado,
                    intOrderID = idOv

                };
                ctx.tblCallCenterCallsInadimplencia.Add(ovInadimplente);
                ctx.SaveChanges();

                SetLogChamadoInadimplencia(idChamado, idAplicacao);
                ctx.Database.ExecuteSqlRaw("exec cspExecuteEnviaEmailInadimplencia @intOrderID = {0}, @intEmployeeID = {1}, @txtTipo = {2}",idOv, Constants.IdEmployeeChamado, "principal");
                return 1;
            }
        }        

        public void SetLogChamadoInadimplencia(int idChamado, int idAplicativo)
        {
            using (var ctx = new DesenvContext())
            {
                var LogInadimplencia = new tblCallCenterCallsInadimplenciaLog
                {
                    intCallCenterCallsID = idChamado,
                    intAplicativoID = idAplicativo,
                    dteDate = DateTime.Now
                };
                ctx.tblCallCenterCallsInadimplenciaLog.Add(LogInadimplencia);
                ctx.SaveChanges();
            }
        }        



            public void AdicionarClassificacaoTurmaDesejada(int idCliente, int idClassificacao, int idProduto)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(ChamadoCallCenter registro)
        {
            throw new System.NotImplementedException();
        }

        public List<ChamadoCallCenter> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public int GetAtributoClassificacao(int idClassificacao, string idProduto)
        {
            throw new System.NotImplementedException();
        }

        public List<ChamadoCallCenter> GetByFilters(ChamadoCallCenter registro)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(ChamadoCallCenter registro)
        {
            throw new System.NotImplementedException();
        }

        public int SetClassificacaoTurmaDesejada(int matricula, int idClassificacao, int idAtributoClassificacao)
        {
            throw new System.NotImplementedException();
        }

        public int Update(ChamadoCallCenter registro)
        {
            throw new System.NotImplementedException();
        }

        public bool VerificarSeClienteTemChamadoAntecipacaoMaterial(int idCliente)
        {
            using (var ctx = new DesenvContext())
            {
                var ano = Utilidades.GetYear().ToString();
                DateTime anoAtual = new DateTime(int.Parse(ano), 1, 1);

                var idsChamadosInternos = new List<int>() { 9876, 9877, 9880 };

                return ctx.tblCallCenterCalls.Any(c => c.intClientID == idCliente
                                                    && c.intCallCategoryID == 2353
                                                    && c.intCallGroupID == 181
                                                    && idsChamadosInternos.Contains(c.intStatusInternoID ?? 0)
                                                    && c.intStatusID != 7
                                                    && c.dteOpen > anoAtual);
            }
        }

        public static List<KeyValuePair<int, int?>> GetChamadosDeTrocaTemporaria(int matricula, List<int> anosConsideradosComoAtual)
        {

            using (var ctx = new DesenvContext())
            {
                List<int> chamadoTrocaTemporaria = new List<int>() { 1845, 1844 };
                List<KeyValuePair<int, int?>> chamados = new List<KeyValuePair<int, int?>>();

                var chamado = (from ccc in ctx.tblCallCenterCalls
                               join courses in ctx.tblCourses on ccc.intCourseID equals courses.intCourseID
                               where ccc.intClientID == matricula && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7 && anosConsideradosComoAtual.Contains(courses.intYear ?? 0)
                               select new { intCourseID = ccc.intCourseID, intYear = courses.intYear });
                foreach (var item in chamado)
                    chamados.Add(new KeyValuePair<int, int?>(item.intCourseID, item.intYear));
                return chamados;
            }

        }
    }
}