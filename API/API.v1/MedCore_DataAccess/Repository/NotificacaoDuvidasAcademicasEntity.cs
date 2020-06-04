using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Repository
{
    public class NotificacaoDuvidasAcademicasEntity : INotificacaoDuvidasAcademicasData
    {
        public List<NotificacaoDuvidaAcademica> GetNotificacoesDuvidaPorAluno(int duvidaId, int clientId, int categoria = (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.Indefinido)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var notificacoes = ctx.tblNotificacaoDuvidas
                        .Where(x => x.intDuvidaId == duvidaId && x.intContactId == clientId)
                        .Select(x => new NotificacaoDuvidaAcademica()
                        {
                            NotificacaoDuvidaId = x.intNotificacaoDuvidaId,
                            NotificacaoId = x.intNotificacaoId.Value,
                            ClientId = x.intContactId.Value,
                            DataCadastro = x.dteCadastro.Value,
                            Descricao = x.txtDescricao,
                            DuvidaId = x.intDuvidaId.Value,
                            TipoCategoria = (EnumTipoMensagemNotificacaoDuvidasAcademicas)x.intTipoCategoria,
                            Status = (EnumStatusNotificacao)x.intStatus
                        }).OrderByDescending(x => x.DataCadastro).ToList();

                    if(categoria > (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.Indefinido)
                    {
                        notificacoes = notificacoes.Where(x => (int)x.TipoCategoria == categoria).ToList();
                    }

                    return notificacoes;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetNotificacaoDuvidaAcademica(NotificacaoDuvidaAcademica notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.tblNotificacaoDuvidas.Add(new tblNotificacaoDuvidas
                    {
                        intNotificacaoId = notificacao.NotificacaoId,
                        dteCadastro = DateTime.Now,
                        intStatus = (int)notificacao.Status,
                        intDuvidaId = notificacao.DuvidaId,
                        intContactId = notificacao.ClientId,
                        txtDescricao = notificacao.Descricao,
                        intTipoCategoria = (int)notificacao.TipoCategoria
                    });

                    return ctx.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetNotificacaoDuvidaAcademicaLida(NotificacaoDuvidaAcademica notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var entity = ctx.tblNotificacaoDuvidas.FirstOrDefault(x => x.intNotificacaoDuvidaId == notificacao.NotificacaoDuvidaId);
                    entity.intStatus = (int)EnumStatusNotificacao.Lida;

                    return ctx.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public int SetNotificacaoDuvidaAcademicaAtiva(NotificacaoDuvidaAcademica notificacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var entity = ctx.tblNotificacaoDuvidas.FirstOrDefault(x => x.intNotificacaoDuvidaId == notificacao.NotificacaoDuvidaId);
                    entity.intStatus = (int)EnumStatusNotificacao.Enviado;

                    return ctx.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public List<DeviceNotificacao> GetAlunosNotificacaoDuvida()
        {

            using (var ctx = new DesenvContext())
            {
                var notificacoes = (from n in ctx.tblNotificacaoDuvidas
                                    join d in ctx.tblDeviceToken on n.intContactId equals d.intClientID
                                    where n.intStatus == 0 && d.bitAtivo == true
                                    && (d.intApplicationId == null
                                        || (d.intApplicationId.HasValue && d.intApplicationId.Value == (int)Aplicacoes.MsProMobile)
                                    )
                                    group new { n, d } by new { n.intContactId, n.intDuvidaId }
                                    into not
                                    select not.FirstOrDefault()
                                    ).ToList().Select(y => new DeviceNotificacao
                                    {
                                        ClientId = y.d.intClientID,
                                        NotificacaoId = y.n.intNotificacaoId.Value,
                                        DeviceToken = y.d.txtOneSignalToken,
                                        InfoAdicional = "[" + y.n.intDuvidaId.Value + "]"
                                    }).ToList();


                return notificacoes;
            }
        }

        public List<Notificacao> GetNotificacoesDuvidasAcademicasAluno(int clientId)
        {

            using (var ctx = new DesenvContext())
            {
                var notificacoes = (from a in ctx.tblNotificacaoDuvidas
                                    join b in ctx.tblDuvidasAcademicas_Duvidas on a.intDuvidaId equals b.intDuvidaID
                                    where b.bitAtiva && a.intContactId == clientId
                                    select a).ToList();

                var tipoNotificacao = ctx.tblNotificacaoTipo.Where(x => x.intNotificacaoTipoId == (int)ETipoNotificacao.DuvidasAcademicas)
                                                            .Select(x => new TipoNotificacao
                                                            {
                                                                Id = x.intNotificacaoTipoId,
                                                                Alias = x.txtAlias,
                                                                Ordem = x.intOrdem
                                                            }).FirstOrDefault();
                
                var grp = notificacoes.GroupBy(x => new { x.intContactId, x.intDuvidaId, x.intTipoCategoria, x.intStatus }).Select(y => new { key = y.Key, not = y.ToList(), count = y.Count() }).ToList();

                var notificacoesExibicao = grp.Select(x => x.not.Select(y => new Notificacao
                {
                    IdNotificacao = y.intNotificacaoId.Value,
                    Texto = y.txtDescricao,
                    Quantidade = x.count,
                    Data = y.dteCadastro.Value.ToString("dd/MM"),
                    DataOriginal = y.dteCadastro.HasValue ? y.dteCadastro.Value : new DateTime(),
                    Lida = y.intStatus > 0,
                    TipoNotificacao = tipoNotificacao,
                    DuvidaId = y.intDuvidaId.Value,
                    TipoRespostaId = y.intTipoCategoria.Value,
                    Matricula = y.intContactId.Value

                }).OrderByDescending(z => z.DataOriginal).First()
                ).ToList();

                return notificacoesExibicao;
            }
        }

        public int DeleteNotificacoesAluno(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var notificacoes = ctx.tblNotificacaoDuvidas.Where(x => x.intContactId == matricula);
                foreach (var item in notificacoes)
                {
                    ctx.tblNotificacaoDuvidas.Remove(item);
                }

                return ctx.SaveChanges();
            }
        }

        public int SetNotificacaoDuvidasAcademicaAlunoEnviada(int clientId, DateTime data)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var entity = ctx.tblNotificacaoDuvidas.Where(x => x.intContactId == clientId && x.dteCadastro >= data).ToList();
                    entity.ForEach(x => x.intStatus = (int)EnumStatusNotificacao.Enviado);

                    return ctx.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }
    }
}