using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Repository
{
        public class NotificacaoEntity : INotificacaoData
    {
        public List<Notificacao> GetAll(int matricula, int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var lidas = ctx.tblNotificacaoAluno.Where(x => x.intClientId == matricula).Select(x => x.intNotificacaoId).ToList();
                    var serverDate = Utilidades.GetServerDate();
                    var notificacoes = (from a in ctx.tblSellOrders
                                        join b in ctx.tblSellOrderDetails on a.intOrderID equals b.intOrderID
                                        join c in ctx.tblCourses on b.intProductID equals c.intCourseID
                                        join d in ctx.tblNotificacao on c.intYear equals d.dteCadastro.Year
                                        where a.intClientID == matricula
                                              && d.intApplicationId == idAplicacao && (d.intClientID == -1 || d.intClientID == matricula)
                                              && d.dteLiberacao <= serverDate
                                        select new Notificacao
                                        {
                                            IdNotificacao = d.intNotificacaoId,
                                            Texto = d.txtTexto,
                                            DataOriginal = d.dteLiberacao,
                                            TipoNotificacao = new TipoNotificacao() { Id = d.intNotificacaoTipoId },
                                        }).Distinct().OrderByDescending(x => x.DataOriginal).ToList();

                    foreach (var n in notificacoes)
                    {
                        n.Data = n.DataOriginal.ToString("dd/MM");
                        n.Lida = lidas.Contains(n.IdNotificacao);
                    }

                    return notificacoes;
                }
            }
            catch
            {
                throw;
            }
        }

        public Notificacao Get(int idNotificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var nt = (from n in ctx.tblNotificacao
                              where n.intNotificacaoId == idNotificacao
                              select new { IdNotificacao = n.intNotificacaoId, Texto = n.txtTexto, TipoNotificacao = n.intNotificacaoTipoId, Data = n.dteCadastro, Titulo = n.txtTitulo }).FirstOrDefault();

                    return new Notificacao
                    {
                        IdNotificacao = nt.IdNotificacao,
                        Texto = nt.Texto,
                        Titulo = nt.Titulo,
                        Data = nt.Data.ToString("dd/MM"),
                        TipoNotificacao = new TipoNotificacao() { Id = nt.TipoNotificacao }
                    };
                }
            }
            catch
            {
                throw;
            }
        }


        public List<DeviceNotificacao> GetDevicesNotificados(int idNotificacao, DateTime date)
        {
            using (var ctx = new DesenvContext())
            {

                var devices = (from dev in ctx.tblNotificacaoDeviceToken
                               where dev.intNotificacaoId == idNotificacao && dev.dteEnvio >= date
                               select new DeviceNotificacao
                               {
                                   NotificacaoId = dev.intNotificacaoId,
                                   Status = (EStatusEnvioNotificacao)dev.intStatusEnvio,
                                   DeviceToken = dev.txtOneSignalToken,
                                   InfoAdicional = dev.txtInfoAdicional,
                                   Data = dev.dteEnvio ?? default(DateTime)
                               }
                              ).ToList();

                return devices;


            }
        }

        public int SetNotificacao(Notificacao notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.tblNotificacao.Add(new tblNotificacao
                    {
                        intNotificacaoTipoId = notificacao.TipoNotificacao.Id,
                        intEmployeeId = notificacao.Matricula,
                        txtTexto = notificacao.Texto,
                        intApplicationId = (int)Aplicacoes.MsProMobile,
                        dteCadastro = DateTime.Now,
                        intClientID = notificacao.Matricula,
                        dteLiberacao = DateTime.Now,
                        intTipoEnvio = (int)notificacao.TipoEnvio,
                        intStatusEnvio = (int)notificacao.StatusEnvio,
                        txtTitulo = notificacao.Titulo,
                        txtInfoAdicional = notificacao.InfoAdicional
                    });
                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetNotificacaoLida(Notificacao notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.tblNotificacaoAluno.Add(new tblNotificacaoAluno
                    {
                        intClientId = notificacao.Matricula,
                        dteLida = DateTime.Now,
                        intNotificacaoId = notificacao.IdNotificacao
                    });
                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateNotificacao(Notificacao notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {

                    var notificacaoEntity = ctx.tblNotificacao.FirstOrDefault(x => x.intNotificacaoId == notificacao.IdNotificacao);

                    notificacaoEntity.intStatusEnvio = (int)notificacao.StatusEnvio;

                    ctx.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Notificacao> GetNotificacoesPermitidas(int matricula, int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var lidas = ctx.tblNotificacaoAluno.Where(x => x.intClientId == matricula).Select(x => x.intNotificacaoId).ToList();

                    var serverDate = Utilidades.GetServerDate();

                    var notificacoes = (from a in ctx.tblSellOrders
                                        join b in ctx.tblSellOrderDetails on a.intOrderID equals b.intOrderID
                                        join c in ctx.tblCourses on b.intProductID equals c.intCourseID
                                        join d in ctx.tblNotificacao on c.intYear equals d.dteCadastro.Year
                                        where a.intClientID == matricula
                                              && d.intApplicationId == idAplicacao && (d.intClientID == -1 || d.intClientID == matricula)
                                              && d.dteLiberacao <= serverDate

                                        select new Notificacao
                                        {
                                            IdNotificacao = d.intNotificacaoId,
                                            Texto = d.txtTexto,
                                            DataOriginal = d.dteLiberacao,
                                            TipoNotificacao = new TipoNotificacao() { Id = d.intNotificacaoTipoId },
                                        }).Distinct().OrderByDescending(x => x.DataOriginal).ToList();

                    foreach (var n in notificacoes)
                    {
                        n.Data = n.DataOriginal.ToString("dd/MM");
                        n.Lida = lidas.Contains(n.IdNotificacao);
                    }

                    return notificacoes;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Notificacao> GetNotificacoesAplicacao(int idAplicacao, int matricula)
        {

            using (var ctx = new DesenvContext())
            {
                var datalimiteNotificacao = new DateTime(Utilidades.GetYear() - 1, 12, 31, 23, 59, 59);

                var serverDate = Utilidades.GetServerDate();

                var notificacoes = (from not in ctx.tblNotificacao
                                    join tipo in ctx.tblNotificacaoTipo on not.intNotificacaoTipoId equals tipo.intNotificacaoTipoId
                                    join lidas in ctx.tblNotificacaoAluno on not.intNotificacaoId equals lidas.intNotificacaoId
                                    where not.intApplicationId == idAplicacao
                                    && not.dteLiberacao <= serverDate
                                    && lidas.intClientId == matricula
                                    && (not.intClientID == -1 || not.intClientID == matricula)
                                    && (not.intTipoEnvio == (int)ETipoEnvioNotificacao.Interna || not.intTipoEnvio == (int)ETipoEnvioNotificacao.Todos)
                                    && (not.dteLiberacao > datalimiteNotificacao || lidas.intNotificacaoId == not.intNotificacaoId)
                                    select new Notificacao
                                    {
                                        IdNotificacao = not.intNotificacaoId,
                                        Texto = not.txtTexto,
                                        DataOriginal = not.dteLiberacao,
                                        Matricula = not.intClientID,
                                        Destaque = not.bitDestaque,
                                        InfoAdicional = not.txtInfoAdicional,
                                        //Data = SqlFunctions.DateName("dy", not.dteLiberacao) + "/" + SqlFunctions.DateName("mm", not.dteLiberacao),
                                        Lida = lidas.intNotificacaoId == not.intNotificacaoId,
                                        TipoNotificacao = new TipoNotificacao()
                                        {
                                            Id = tipo.intNotificacaoTipoId,
                                            Descricao = tipo.txtDescricao,
                                            Ordem = tipo.intOrdem,
                                            Alias = tipo.txtAlias
                                        }
                                    }).Distinct().OrderByDescending(x => x.DataOriginal).ToList();

                return notificacoes;
            }
        }

        public List<Notificacao> GetNotificacoesAProcessar(EStatusEnvioNotificacao status)
        {
            var data = Utilidades.GetServerDate();

            using (var ctx = new DesenvContext())
            {
                var notificacoesPendentes = ctx.tblNotificacao
                       .Where(x => x.intStatusEnvio == (int)status
                           && (x.intTipoEnvio == (int)ETipoEnvioNotificacao.PushExterna || x.intTipoEnvio == (int)ETipoEnvioNotificacao.Todos)
                           && x.dteLiberacao <= data
                           && x.intNotificacaoTipoId != (int)ETipoNotificacao.DisparadaPosEvento)
                       .OrderBy(x => x.dteLiberacao)
                       .Select(x => new Notificacao
                       {
                           IdNotificacao = x.intNotificacaoId,
                           Titulo = x.txtTitulo,
                           Texto = x.txtTexto,
                           InfoAdicional = x.txtInfoAdicional,
                           DataOriginal = x.dteLiberacao,
                           TipoNotificacao = new TipoNotificacao() { Id = x.intNotificacaoTipoId },
                           StatusEnvio = (EStatusEnvioNotificacao)x.intStatusEnvio,
                           TipoEnvio = (ETipoEnvioNotificacao)x.intTipoEnvio,
                           Matricula = x.intClientID,
                           AplicacaoId = x.intApplicationId
                       }).ToList();

                return notificacoesPendentes;
            }
        }

        public List<Notificacao> GetNotificacoesPosEvento(EStatusEnvioNotificacao status)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblNotificacao
                       .Where(x => x.intStatusEnvio == (int)status
                           && (x.intTipoEnvio == (int)ETipoEnvioNotificacao.PushExterna || x.intTipoEnvio == (int)ETipoEnvioNotificacao.Todos)
                           && x.intNotificacaoTipoId == (int)ETipoNotificacao.DisparadaPosEvento)
                       .OrderBy(x => x.dteLiberacao)
                       .Select(x => new Notificacao
                       {
                           IdNotificacao = x.intNotificacaoId,
                           Titulo = x.txtTitulo,
                           Texto = x.txtTexto,
                           InfoAdicional = x.txtInfoAdicional,
                           DataOriginal = x.dteLiberacao,
                           TipoNotificacao = new TipoNotificacao() { Id = x.intNotificacaoTipoId },
                           StatusEnvio = (EStatusEnvioNotificacao)x.intStatusEnvio,
                           TipoEnvio = (ETipoEnvioNotificacao)x.intTipoEnvio,
                           Matricula = x.intClientID,
                           AplicacaoId = x.intApplicationId
                       }).ToList();
            }
        }

        public List<DeviceNotificacao> BuscarFilaNotificacaoPosEvento(int idNotificacao)
        {
            var now = Utilidades.GetServerDate();
            using (var ctx = new DesenvContext())
            {
                var result = (from n in ctx.tblNotificacaoEvento
                              join d in ctx.tblNotificacaoDeviceToken on n.intNotificacaoEvento equals d.intIdentificacaoId
                              where n.bitAtivo && d.intStatusEnvio == (int)EStatusEnvioNotificacao.NaoEnviado
                                && n.intNotificacaoId.HasValue && n.intNotificacaoId.Value == idNotificacao
                              select new { notificacoes = n, fila = d }).ToList();

                foreach (var r in result)
                {
                    r.notificacoes.intStatus = (int)EStatusEnvioNotificacao.Enviado;
                    r.fila.intStatusEnvio = (int)EStatusEnvioNotificacao.Enviado;
                    r.fila.dteEnvio = now;
                }
                ctx.SaveChanges();

                return (from r in result
                        select new DeviceNotificacao
                        {
                            ClientId = r.notificacoes.intContactId,
                            NotificacaoId = r.notificacoes.intNotificacaoEvento,
                            DeviceToken = r.fila.txtOneSignalToken,
                            Titulo = r.notificacoes.txtTitulo,
                            Mensagem = r.notificacoes.txtDescricao,
                            InfoAdicional = r.notificacoes.Metadados
                        }).ToList();
            }
        }

        public List<DeviceNotificacao> DefinirDevicesNotificacaoPosEvento(Notificacao notificacao, EStatusEnvioNotificacao status)
        {
            var limiteAtraso = DateTime.Now.AddHours(
                Constants.LIMITE_HORAS_NOTIFICACAO * (-1)
                );

            using (var ctx = new DesenvContext())
            {
                return (from n in ctx.tblNotificacaoEvento
                        join d in ctx.tblDeviceToken on n.intContactId equals d.intClientID
                        where n.bitAtivo && d.bitAtivo == true && n.intNotificacaoId.HasValue
                            && n.dteCadastro.HasValue && n.dteCadastro.Value >= limiteAtraso
                            && (d.intApplicationId.HasValue && d.intApplicationId.Value == notificacao.AplicacaoId)
                            && n.intStatus.HasValue && n.intStatus.Value == (int)status
                        select new DeviceNotificacao
                        {
                            ClientId = n.intContactId,
                            NotificacaoId = notificacao.IdNotificacao,
                            DeviceToken = d.txtOneSignalToken,
                            Titulo = n.txtTitulo,
                            Mensagem = n.txtDescricao,
                            InfoAdicional = n.Metadados,
                            IdentificadorId = n.intNotificacaoEvento
                        }).ToList();
            }
        }

        public void InserirDevicesNotificacao(List<DeviceNotificacao> devicesInscritos)
        {
            try
            {
                var tbl = new List<tblNotificacaoDeviceToken>();
                foreach (var item in devicesInscritos)
                {
                    tbl.Add(new tblNotificacaoDeviceToken
                    {
                        intNotificacaoId = item.NotificacaoId,
                        intStatusEnvio = (int)EStatusEnvioNotificacao.NaoEnviado,
                        txtOneSignalToken = item.DeviceToken,
                        txtInfoAdicional = item.InfoAdicional,
                        txtTitulo = item.Titulo,
                        txtMensagem = item.Mensagem,
                        intIdentificacaoId = item.IdentificadorId
                    });
                }

                var dt = ToDataTable(tbl);
                BulkInsert(dt);
            }
            catch
            {
                throw;
            }
        }

        private DataTable ToDataTable(List<tblNotificacaoDeviceToken> devicesInscritos)
        {
            var dt = new DataTable();

            dt.Columns.Add(("intNotificacaoId"));
            dt.Columns.Add(("intStatusEnvio"));
            dt.Columns.Add(("txtOneSignalToken"));
            dt.Columns.Add(("txtInfoAdicional"));
            dt.Columns.Add(("txtTitulo"));
            dt.Columns.Add(("txtMensagem"));
            dt.Columns.Add(("intIdentificacaoId"));


            foreach (var device in devicesInscritos)
            {
                DataRow row = dt.NewRow();

                row["intNotificacaoId"] = device.intNotificacaoId;
                row["intStatusEnvio"] = device.intStatusEnvio;
                row["txtOneSignalToken"] = device.txtOneSignalToken;
                row["txtInfoAdicional"] = device.txtInfoAdicional;
                row["txtTitulo"] = device.txtTitulo;
                row["txtMensagem"] = device.txtMensagem;
                row["intIdentificacaoId"] = device.intIdentificacaoId;

                dt.Rows.Add(row);
            }

            return dt;
        }

        private int BulkInsert(DataTable dataTable)
        {
            var connectionString = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");
            var tableName = "tblNotificacaoDeviceToken";

            using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString))
            {
                sqlbc.BulkCopyTimeout = 120;

                sqlbc.DestinationTableName = tableName;
                sqlbc.ColumnMappings.Add("intNotificacaoId", "intNotificacaoId");
                sqlbc.ColumnMappings.Add("intStatusEnvio", "intStatusEnvio");
                sqlbc.ColumnMappings.Add("txtOneSignalToken", "txtOneSignalToken");
                sqlbc.ColumnMappings.Add("txtInfoAdicional", "txtInfoAdicional");
                sqlbc.ColumnMappings.Add("txtTitulo", "txtTitulo");
                sqlbc.ColumnMappings.Add("txtMensagem", "txtMensagem");
                sqlbc.ColumnMappings.Add("intIdentificacaoId", "intIdentificacaoId");
                sqlbc.WriteToServer(dataTable);
            }

            return 1;
        }

        public List<DeviceNotificacao> GetDevicesNotificacaoFila(int notificacaoId)
        {
            var data = Utilidades.GetServerDate();

            using (var ctx = new DesenvContext())
            {
                var consultaDevicesFila = ctx.tblNotificacaoDeviceToken.Where(x => x.intNotificacaoId == notificacaoId
                                                                            && x.intStatusEnvio == (int)EStatusEnvioNotificacao.NaoEnviado)
                                                                            .ToList();

                foreach (var device in consultaDevicesFila)
                {
                    device.intStatusEnvio = (int)EStatusEnvioNotificacao.Enviado;
                    device.dteEnvio = data;
                }

                var devicesFila = consultaDevicesFila.GroupBy(g => g.txtOneSignalToken)
                                                     .ToList()
                                                     .Select(x => new DeviceNotificacao
                                                     {
                                                         DeviceToken = x.Key,
                                                         InfoAdicional = x.First().txtInfoAdicional,
                                                         Titulo = x.First().txtTitulo,
                                                         Mensagem = x.First().txtMensagem
                                                     }).ToList();

                ctx.SaveChanges();

                return devicesFila;
            }

        }

        // public List<AlunoTemaAvaliacao> GetAlunoTemaAvaliacao(ParametrosAvaliacaoAula parametros)
        // {
        //     using (var ctx = new DesenvContext())
        //     {
        //         var ano = Utilidades.GetYear();
        //         List<int?> produtos = new List<int?>();

        //         produtos.AddRange(new List<int?>() { (int)Produto.Produtos.MEDCURSO ,
        //                                              (int)Produto.Produtos.MED,
        //                                              (int)Produto.Produtos.INTENSIVAO,
        //                                              (int)Produto.Produtos.MEDELETRO,
        //                                              (int)Produto.Produtos.CPMED,
        //                                              (int)Produto.Produtos.RAC,
        //                                              (int)Produto.Produtos.RACIPE
        //         }
        //         );

        //         DateTime inicio = parametros.Data != default(DateTime) ? parametros.Data : DateTime.Today;
        //         DateTime fim = inicio.AddDays(1);

        //         var presenca = (
        //                         from mcr in ctx.mview_Cronograma
        //                         join c in ctx.tblCourses on mcr.intCourseID equals c.intCourseID
        //                         join prd in ctx.tblProducts on c.intCourseID equals prd.intProductID
        //                         join lm in ctx.tblLesson_Material on mcr.intLessonID equals lm.intLessonID
        //                         join prd2 in ctx.tblProducts on lm.intMaterialID equals prd2.intProductID
        //                         join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
        //                         join sod in ctx.tblSellOrderDetails on prd.intProductID equals sod.intProductID
        //                         join so in ctx.tblSellOrders on sod.intOrderID equals so.intOrderID
        //                         join dt in ctx.tblDeviceToken on so.intClientID equals dt.intClientID
        //                         join al in ctx.tblAccessLogs on new { x = mcr.intClassRoomID, y = so.intClientID } equals new { x = al.intClassroomID.Value, y = al.intPeopleID }
        //                         where c.intYear == ano
        //                         && dt.bitAtivo == true
        //                         && (dt.intApplicationId == null
        //                             || (dt.intApplicationId.HasValue && dt.intApplicationId.Value == (int)Aplicacoes.MsProMobile)
        //                         )
        //                         && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
        //                         && produtos.Contains(prd.intProductGroup1)
        //                         && mcr.dteDateTime > inicio
        //                         && mcr.dteDateTime < fim
        //                         && mcr.intDuration > 0
        //                         && al.dteDateTime > inicio
        //                         && al.dteDateTime < fim
        //                         select new AlunoTemaAvaliacao
        //                         {
        //                             LessonTitleID = mcr.intLessonID,
        //                             ClientID = so.intClientID,
        //                             Entrada = al.dteDateTime,
        //                             DeviceToken = dt.txtOneSignalToken,
        //                             MaterialId = b.intBookID,
        //                             CourseId = c.intCourseID
        //                         }
        //                         ).Distinct().ToList();

        //         return presenca;

        //     }
        // }

        // public List<mview_Cronograma> GetCursosComUltimaAulaDoDia(ParametrosAvaliacaoAula parametros)
        // {
        //     DateTime inicio = parametros.Data != default(DateTime) ? parametros.Data : DateTime.Today;
        //     DateTime fim = inicio.AddDays(1);
        //     using (var ctx = new DesenvContext())
        //     {
        //         var crono = (from mcr in ctx.mview_Cronograma
        //                      where mcr.intDuration > 0
        //                      && mcr.dteDateTime >= inicio
        //                      && mcr.dteDateTime <= fim
        //                      group mcr by new { mcr.intCourseID, mcr.intDuration }
        //                           into g
        //                      select new
        //                      {
        //                          intCourseID = g.Key.intCourseID,
        //                          intDuration = g.Key.intDuration,
        //                          dteDateTime = g.Max(x => x.dteDateTime)
        //                      }
        //                       ).ToList();

        //         var cronograma = crono.Select(x => new mview_Cronograma { intCourseID = x.intCourseID, intDuration = x.intDuration, dteDateTime = x.dteDateTime }).ToList();
        //         return cronograma;
        //     }

        // }

        public List<tblNotificacaoEvento> InserirNotificacoesPosEvento(params NotificacaoPosEventoDTO[] notificacoes)
        {
            var list = new List<tblNotificacaoEvento>();
            using (var ctx = new DesenvContext())
            {
                foreach (var notificacao in notificacoes)
                {
                    var item = new tblNotificacaoEvento
                    {
                        intNotificacaoId = notificacao.IdNotificacao,
                        intContactId = notificacao.Matricula,
                        txtTitulo = notificacao.Titulo,
                        txtDescricao = notificacao.Mensagem,
                        Metadados = notificacao.Metadados,
                        intStatus = (int)EStatusEnvioNotificacao.NaoEnviado,
                        intStatusLeitura = (int)ELeituraNotificacaoEvento.NaoLido,
                        dteCadastro = DateTime.Now,
                        bitAtivo = notificacao.Ativa
                    };
                    list.Add(item);
                    ctx.tblNotificacaoEvento.Add(item);
                }
                ctx.SaveChanges();
            }
            return list;
        }

        public List<NotificacaoPosEventoDTO> GetNotificacoesAlunoPosEvento(int matricula, Aplicacoes aplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                return (from ne in ctx.tblNotificacaoEvento
                        join n in ctx.tblNotificacao on ne.intNotificacaoId equals n.intNotificacaoId
                        where ne.intContactId == matricula && ne.bitAtivo
                            && n.intApplicationId == (int)aplicacao
                        select new NotificacaoPosEventoDTO
                        {
                            IdNotificacao = ne.intNotificacaoId ?? 0,
                            Matricula = ne.intContactId,
                            Titulo = ne.txtTitulo,
                            Mensagem = ne.txtDescricao,
                            Metadados = ne.Metadados,
                            Data = ne.dteCadastro ?? DateTime.MinValue,
                            Lida = ne.intStatusLeitura.HasValue
                                && ne.intStatusLeitura.Value == (int)ELeituraNotificacaoEvento.Lida
                        }).ToList();
            }
        }

        public int AtualizarNotificacoesPosEvento(List<tblNotificacaoEvento> notificacoes)
        {
            using (var ctx = new DesenvContext())
            {
                foreach (var notificacao in notificacoes)
                {
                    ctx.tblNotificacaoEvento.Add(notificacao);
                    ctx.Entry(notificacao).State = EntityState.Modified;
                }
                return ctx.SaveChanges();
            }
        }

        public tblNotificacaoEvento GetNotificacaoAlunoPosEvento(int idNotificacaoEvento)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblNotificacaoEvento.FirstOrDefault(f => f.intNotificacaoEvento == idNotificacaoEvento);
            }
        }

        #region ADMIN

        public List<Notificacao> GetNotificacoesAdmin(int idAplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var serverDate = Utilidades.GetServerDate();
                var notificacoesAutomaticas = new int[]
                {
                  (int)ETipoNotificacao.DuvidasAcademicas,
                  (int)ETipoNotificacao.AvaliacaoAula
                };

                var notificacoes = (from not in ctx.tblNotificacao
                                    join notRegra in ctx.tblAccess_PermissionNotification on not.intNotificacaoId equals notRegra.intNotificacaoId
                                    join regraPermissao in ctx.tblAccess_Permission_Rule on notRegra.intPermissaoRegra equals regraPermissao.intPermissaoRegraId
                                    join regra in ctx.tblAccess_Rule on regraPermissao.intRegraId equals regra.intRegraId
                                    where not.intApplicationId == idAplicacao
                                    && (not.intClientID == -1)
                                    && (!notificacoesAutomaticas.Contains(not.intNotificacaoTipoId))
                                    select new Notificacao
                                    {
                                        IdNotificacao = not.intNotificacaoId,
                                        Texto = not.txtTexto,
                                        Titulo = not.txtTitulo,
                                        DataOriginal = not.dteLiberacao,
                                        TipoNotificacao = new TipoNotificacao { Id = not.intNotificacaoTipoId },
                                        TipoEnvio = (ETipoEnvioNotificacao)not.intTipoEnvio,
                                        RegrasVisualizacao = new List<Regra> { new Regra { Descricao = regra.txtDescricao, Id = regraPermissao.intPermissaoRegraId } },
                                        StatusEnvio = (EStatusEnvioNotificacao)not.intStatusEnvio,
                                        InfoAdicional = not.txtInfoAdicional
                                    }).ToList();

                foreach (var n in notificacoes)
                {
                    n.Data = n.DataOriginal.ToString("yyyy/MM/dd HH:mm");
                }


                return notificacoes.OrderByDescending(x => x.IdNotificacao).ToList();
            }
        }

        public List<PermissaoRegra> GetRegrasAdmin()
        {
            using (var ctx = new DesenvContext())
            {
                var lstRegras = (from perm in ctx.tblAccess_Permission_Rule
                                 join rule in ctx.tblAccess_Rule on perm.intRegraId equals rule.intRegraId
                                 where perm.bitAtivo == true && rule.bitAtivo == true
                                 && perm.intAccessoId == (int)ETipoPermissaoRegra.AcessoPermitido
                                 select new PermissaoRegra()
                                 {
                                     Id = perm.intPermissaoRegraId,
                                     Descricao = rule.txtDescricao
                                 }).Distinct().ToList();

                return lstRegras;
            }
        }

        public int SetNotificacaoAgendada(Notificacao notificacao)
        {
            try
            {
                notificacao.DataOriginal = Convert.ToDateTime(notificacao.Data);

                using (var ctx = new DesenvContext())
                {
                    var novaNotificacao = new tblNotificacao
                    {
                        intNotificacaoTipoId = notificacao.TipoNotificacao.Id,
                        intEmployeeId = Utilidades.UsuarioSistema,
                        txtTexto = notificacao.Texto,
                        txtTitulo = notificacao.Titulo,
                        txtInfoAdicional = notificacao.InfoAdicional,
                        intApplicationId = (int)Aplicacoes.MsProMobile,
                        dteCadastro = DateTime.Now,
                        intClientID = -1,
                        dteLiberacao = notificacao.DataOriginal,
                        intStatusEnvio = (int)EStatusEnvioNotificacao.NaoEnviado,
                        intTipoEnvio = (int)notificacao.TipoEnvio
                    };

                    foreach (var regra in notificacao.RegrasVisualizacao)
                    {
                        novaNotificacao.tblAccess_PermissionNotification.Add(new tblAccess_PermissionNotification
                        {
                            intEmployeeId = Utilidades.UsuarioSistema,
                            intOrdem = 1,
                            intPermissaoRegra = regra.Id,
                            dteDataAlteracao = DateTime.Now
                        });
                    }

                    ctx.tblNotificacao.Add(novaNotificacao);

                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateNotificacaoAgendada(Notificacao notificacao)
        {
            try
            {
                notificacao.DataOriginal = Convert.ToDateTime(notificacao.Data);
                var regrasNotificacao = notificacao.RegrasVisualizacao.Select(x => x.Id).ToArray();

                using (var ctx = new DesenvContext())
                {
                    var notificacaoEntidade = ctx.tblNotificacao.FirstOrDefault(x => x.intNotificacaoId == notificacao.IdNotificacao);


                    if (notificacaoEntidade != null)
                    {
                        notificacaoEntidade.txtTitulo = notificacao.Titulo;
                        notificacaoEntidade.txtTexto = notificacao.Texto;
                        notificacaoEntidade.dteLiberacao = notificacao.DataOriginal;
                        notificacaoEntidade.intTipoEnvio = (int)notificacao.TipoEnvio;
                        notificacaoEntidade.txtInfoAdicional = notificacao.InfoAdicional;
                        notificacaoEntidade.intNotificacaoTipoId = notificacao.TipoNotificacao.Id;
                        notificacaoEntidade.intStatusEnvio = (int)EStatusEnvioNotificacao.NaoEnviado;
                    }


                    var notificacaoRegra = ctx.tblAccess_PermissionNotification.Where(x => x.intNotificacaoId == notificacao.IdNotificacao);

                    foreach (var item in notificacaoRegra)
                    {
                        ctx.Entry(item).State = EntityState.Deleted;
                    }

                    foreach (var regra in regrasNotificacao)
                    {
                        ctx.tblAccess_PermissionNotification.Add(new tblAccess_PermissionNotification
                        {
                            intEmployeeId = Utilidades.UsuarioSistema,
                            intOrdem = 1,
                            intPermissaoRegra = regra,
                            dteDataAlteracao = DateTime.Now,
                            intNotificacaoId = notificacaoEntidade.intNotificacaoId
                        });
                    }

                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteNotificacaoAgendada(int notificacaoId)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var notificacaoEntidade = ctx.tblNotificacao.FirstOrDefault(x => x.intNotificacaoId == notificacaoId);

                    var lstNotificacaoRegra = ctx.tblAccess_PermissionNotification.Where(x => x.intNotificacaoId == notificacaoId);

                    foreach (var notificacaoRegra in lstNotificacaoRegra)
                    {
                        ctx.Entry(notificacaoRegra).State = EntityState.Deleted;
                    }

                    ctx.Entry(notificacaoEntidade).State = EntityState.Deleted;

                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<AlunoTemaAvaliacao> GetAlunoTemaAvaliacao(ParametrosAvaliacaoAula parametros)
        {
            throw new NotImplementedException();
        }

        public List<mview_Cronograma> GetCursosComUltimaAulaDoDia(ParametrosAvaliacaoAula parametros)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}