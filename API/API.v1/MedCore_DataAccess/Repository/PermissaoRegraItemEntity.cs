using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
     public class PermissaoRegraItemEntity : IPermissaoRegraItemData
    {
        public List<Aplicacao> GetAplicacao()
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var aplicacoes = (from app in ctx.tblAccess_Application 
                                      select new Aplicacao
                                      {
                                          Id = app.intApplicationID,
                                          Nome = app.txtApplication
                                      }).ToList();

                      
                    

                    return aplicacoes;
                }
            }
            catch
            {

                throw;
            }

        }

        public List<ItemAplicativo> GetCategorias(int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var itensAplicativo = (from ao in ctx.tblAccess_Object
                                           join aot in ctx.tblAccess_ObjectType on ao.intObjectTypeId equals aot.intObjectTypeId
                                           where ao.intApplicationId == idAplicacao
                                           select new ItemAplicativo
                                           {
                                               Id = ao.intObjectTypeId,
                                               Descricao = aot.txtType
                                           }).Distinct().ToList();

                    return itensAplicativo;
                }
            }
            catch
            {

                throw;
            }
        }

        public List<Menu> GetItemCategoria(int idAplicacao, int idTipoItem)
        {
            var lstMenu = new List<Menu>();
            using (var ctx = new DesenvContext())
            {
                var isAntesDataLimite = Utilidades.IsBeforeDataLimite(idAplicacao, Utilidades.GetYear());
                var consulta = (from o in ctx.tblAccess_Object
                                where o.intObjectTypeId == idTipoItem && o.intApplicationId == idAplicacao && o.bitAtivo == true
                                orderby o.intOrdem ascending
                                select new Menu()
                                {
                                    Id = o.intObjectId,
                                    IdPai = o.intPaiId,
                                    IdAplicacao = idAplicacao,
                                    Nome = o.txtNome
                                }).Distinct().ToList();

                return consulta;
            }
        }

        public List<Menu> GetDetalheItem(int idAplicacao, int idTipoItem)
        {
            var lstMenu = new List<Menu>();
            using (var ctx = new DesenvContext())
            {
                var isAntesDataLimite = Utilidades.IsBeforeDataLimite(idAplicacao, Utilidades.GetYear());
                var consulta = (from o in ctx.tblAccess_Object
                                join menu in ctx.tblAccess_Menu on o.intObjectId equals menu.intObjectId
                                where o.intObjectTypeId == idTipoItem && o.intApplicationId == idAplicacao && o.bitAtivo == true
                                orderby o.intOrdem ascending
                                select new Menu()
                                {
                                    Id = o.intObjectId,
                                    IdPai = o.intPaiId,
                                    IdAplicacao = idAplicacao,
                                    Nome = o.txtNome,
                                    Target = menu.txtTarget,
                                    Url = menu.txtUrl
                                }).Distinct().ToList();

                return consulta;
            }
        }

        public List<PermissaoRegra> GetPermissoesObjeto(int idObjeto, int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var itensAplicativo = (from po in ctx.tblAccess_PermissionObject
                                           join pr in ctx.tblAccess_Permission_Rule on po.intPermissaoRegraId equals pr.intPermissaoRegraId
                                           join av in ctx.tblAvisos on pr.intMensagemId equals av.intAvisoID into AV
                                           from av in AV.DefaultIfEmpty()
                                           where po.intObjectId == idObjeto
                                           && (po.intApplicationID == -1 || po.intApplicationID == idAplicacao)
                                           select new PermissaoRegra
                                           {
                                               Id = po.intPermissionObject,
                                               Regra = new Regra { Id = pr.intRegraId },
                                               Descricao = pr.txtDescricao,
                                               Ordem = po.intOrdem,
                                               AcessoId = pr.intAccessoId,
                                               MensagemId = av.intAvisoID,
                                               DescricaoMensagem = av.txtDescricao
                                           }).ToList();


                    return itensAplicativo;
                }
            }
            catch
            {

                throw;
            }

        }

        public bool SetAssociacaoRegra(int idObjeto, int idPermissaoRegra, int idOrdem)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var objPermissionObject = new tblAccess_PermissionObject()
                    {
                        intObjectId = idObjeto,
                        intPermissaoRegraId = idPermissaoRegra,
                        intOrdem = idOrdem
                    };

                    ctx.tblAccess_PermissionObject.Add(objPermissionObject);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool SetDessacociacaoObjeto(int permissaoObjeto)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var dessacociarPermissao = ctx.tblAccess_PermissionObject.Where(x => x.intPermissionObject == permissaoObjeto).FirstOrDefault();
                    if (dessacociarPermissao != null)
                    {
                        ctx.tblAccess_PermissionObject.Remove(dessacociarPermissao);
                        ctx.SaveChanges();
                        return true;
                    }
                    else return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Regra> GetRegras(int idRegra = 0)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var regras = (from r in ctx.tblAccess_Rule
                                  where (idRegra == 0 | r.intRegraId == idRegra)
                                  select new Regra
                                  {
                                      Id = r.intRegraId,
                                      Descricao = r.txtDescricao
                                  }).ToList();

                    return regras;

                }
            }
            catch
            {

                throw;
            }
        }

        public int SetRegra(PermissaoRegra permissaoRegra)
        {
            using (var ctx = new DesenvContext())
            {
                var condicoes = new List<RegraCondicao>();

                foreach (var item in permissaoRegra.Regra.RegraDetalhes)
                {
                    new RegraCondicao { Id = item.Id };
                    condicoes.Add(item);
                }


                var dadosCondicoes = (from c in condicoes
                                      join ad in ctx.tblAccess_Detail on c.Id equals ad.intDetalheId
                                      select new RegraCondicao
                                      {
                                          Id = ad.intDetalheId,
                                          Descricao = ad.txtDescricao
                                      }).ToList();

                var descricaoRegra = "";
                var count = dadosCondicoes.Count();
                var i = 1;
                if (count >= 1)
                {
                    foreach (var desc in dadosCondicoes)
                    {
                        if (i == count)
                        {
                            descricaoRegra += desc.Descricao;
                        }
                        else
                        {
                            descricaoRegra += desc.Descricao + " + ";
                        }
                        i++;
                    }
                }

                var idRegraCriada = InsertRegra(descricaoRegra, permissaoRegra.EmployeeId);

                var ret1 = InsertCondicoesRegra(idRegraCriada, dadosCondicoes);

                if (ret1 == 0) return 0;

                var idPermissaoRegraCriada = InsertPermissaoRegra(descricaoRegra, idRegraCriada, permissaoRegra);

                return idPermissaoRegraCriada;
            }
        }

        public int InsertRegra(string descricaoRegra, int idEmpregado)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var idRegra = 0;
                    var r = new tblAccess_Rule()
                    {
                        txtDescricao = descricaoRegra,
                        dteCriacao = DateTime.Now,
                        dteUltimaAlteracao = DateTime.Now,
                        intEmployeeID = idEmpregado,
                        bitAtivo = true
                    };
                    ctx.tblAccess_Rule.Add(r);
                    ctx.SaveChanges();
                    idRegra = r.intRegraId;

                    return idRegra;
                }

            }
            catch
            {
                throw;
            }

        }

        public int InsertCondicoesRegra(int idRegra, List<RegraCondicao> condicoes)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {

                    foreach (var c in condicoes)
                    {
                        var r = new tblAccess_RuleDetail()
                        {
                            intRegraId = idRegra,
                            intDetalheId = c.Id,
                            bitAtivo = true
                        };

                        ctx.tblAccess_RuleDetail.Add(r);
                        ctx.SaveChanges();
                    }
                    return 1;
                }

            }
            catch
            {
                return 0;
                throw;
            }

        }

        public int InsertPermissaoRegra(string descricao, int idRegra, PermissaoRegra permissaoRegra)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var idPermissaoRegra = 0;
                    var pr = new tblAccess_Permission_Rule()
                    {
                        txtDescricao = descricao,
                        intRegraId = idRegra,
                        intAccessoId = permissaoRegra.AcessoId,
                        intOrdem = -1,
                        dteUltimaAlteracao = DateTime.Now,
                        intEmployeeID = permissaoRegra.EmployeeId,
                        intMensagemId = permissaoRegra.MensagemId,
                        bitDataLimite = permissaoRegra.IsDataLimite,
                        intInterruptorId = -1,
                        bitAtivo = true
                    };

                    ctx.tblAccess_Permission_Rule.Add(pr);
                    ctx.SaveChanges();
                    idPermissaoRegra = pr.intPermissaoRegraId;

                    return idPermissaoRegra;
                }

            }
            catch
            {
                return 0;
                throw;
            }

        }

        public int AlteraNivelAcesso(PermissaoRegra permissaoObjeto)
        {
            using (var ctx = new DesenvContext())
            {
                var regraPermitida = (from po in ctx.tblAccess_PermissionObject
                                      join pr in ctx.tblAccess_Permission_Rule on po.intPermissaoRegraId equals pr.intPermissaoRegraId
                                      where po.intPermissionObject == permissaoObjeto.Id
                                      select new
                                      {
                                          idPermissaoRegra = po.intPermissaoRegraId
                                      }
                                          ).FirstOrDefault();

                var dados = ctx.tblAccess_Permission_Rule.Where(p => p.intPermissaoRegraId == regraPermitida.idPermissaoRegra).ToList();

                dados.ForEach(x => { x.intAccessoId = permissaoObjeto.AcessoId; });
                ctx.SaveChanges();
            }

            return 1;
        }

        public List<RegraCondicao> GetCondicoes()
        {
            using (var ctx = new DesenvContext())
            {
                var condicoes = (from d in ctx.tblAccess_Detail
                                 select new RegraCondicao
                                 {
                                     Id = d.intDetalheId,
                                     Descricao = d.txtDescricao,
                                     Produto = d.intProductGroupId,
                                     TipoAno = (RegraCondicao.tipoAno)d.intTipoAnoId,
                                     StatusOV = d.intStatusOV,
                                     StatusPagamento = d.intStatusPagamento,
                                     CallCategory = d.intCallCategory,
                                     StatusInterno = d.intStatusInterno,
                                     ClientId = d.intClientId,
                                     DataUltimaAlteracao = d.dteUltimaAlteracao,
                                     EmployeeId = d.intEmployeeID,
                                     Ativo = d.bitAtivo

                                 }).ToList();

                return condicoes;

            }
        }

        public int SetCondicao(RegraCondicao regraCondicao)
        {
            using (var ctx = new DesenvContext())
            {
                var condicaoCriada = 0;
                var c = new tblAccess_Detail()
                {
                    txtDescricao = regraCondicao.Descricao,
                    intProductGroupId = regraCondicao.Produto,
                    intTipoAnoId = (int)regraCondicao.TipoAno,
                    intStatusOV = regraCondicao.StatusOV,
                    intStatusPagamento = regraCondicao.StatusPagamento,
                    intCallCategory = regraCondicao.CallCategory,
                    intStatusInterno = regraCondicao.StatusInterno,
                    intClientId = regraCondicao.ClientId,
                    dteUltimaAlteracao = DateTime.Now,
                    intEmployeeID = regraCondicao.EmployeeId,
                    bitAtivo = regraCondicao.Ativo
                };

                ctx.tblAccess_Detail.Add(c);
                ctx.SaveChanges();
                condicaoCriada = c.intDetalheId;

                return condicaoCriada;
            }
        }

        public int RemoveCondicao(List<RegraCondicao> condicoes)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var dadosCondicoes = (from c in condicoes
                                          select new RegraCondicao
                                          {
                                              Id = c.Id,
                                              IdRegra = c.IdRegra
                                          }).ToList();


                    var i = 0;
                    foreach (var c in dadosCondicoes)
                    {
                        var removeCondicao = ctx.tblAccess_RuleDetail.Where(x => x.intDetalheId == c.Id && x.intRegraId == c.IdRegra).FirstOrDefault();

                        if (removeCondicao != null)
                        {
                            ctx.tblAccess_RuleDetail.Remove(removeCondicao);
                            ctx.SaveChanges();
                            i = 1;
                        }
                    }
                    if (i == 1) return 1;
                    return 0;

                }
            }
            catch
            {
                throw;
            }
        }

        public int DesativaRegra(int idRegra)
        {
            using (var ctx = new DesenvContext())
            {
                var dados = ctx.tblAccess_Rule.Where(r => r.intRegraId == idRegra).ToList();
                dados.ForEach(x => { x.bitAtivo = false; });
                ctx.SaveChanges();
            }

            return 1;
        }

        public bool IsAplicacaoDisponivel(int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var isDisponivel = false;

                    var consulta = (from o in ctx.tblAccess_Object
                                    join ov in ctx.tblAcess_Object_Validity on o.intObjectId equals ov.intObjectId
                                    where o.intApplicationId == idAplicacao && o.intObjectTypeId == Constants.TipoAppAtivo && o.bitAtivo == true && ov.bitAtivo == true
                                    select new
                                    {
                                        DataInicioDisponibilidade = ov.dteInicio,
                                        DataFimDisponibilidade = ov.dteFim
                                    }).ToList();

                    if (consulta.Any())
                    {
                        var DatasDisponibilidade = consulta.FirstOrDefault();
                        isDisponivel = DatasDisponibilidade.DataInicioDisponibilidade < DateTime.Now && DatasDisponibilidade.DataFimDisponibilidade > DateTime.Now ? true : false;
                    }

                    return isDisponivel;
                }
            }
            catch
            {
                throw;
            }

        }

        public bool IsFeatureAtiva(int idAplicacao, int idObjeto)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var isFeatureAtiva = ctx.tblAccess_Object.Where(o => o.intApplicationId == idAplicacao
                        && o.intObjectTypeId == Constants.TipoFeature
                        && o.intObjectId == idObjeto
                        && o.bitAtivo == true).Any();

                    return isFeatureAtiva;
                }
            }
            catch
            {
                throw;
            }
        }        

        public AccessObject GetAcessoObjetoAtivoComDatasDisponibilidadeAtivas(int idObjeto)
        {
            using (var ctx = new DesenvContext())
            {
                var consulta = (from o in ctx.tblAccess_Object
                                join ov in ctx.tblAcess_Object_Validity on o.intObjectId equals ov.intObjectId
                                where o.bitAtivo == true && ov.bitAtivo == true
                                && o.intObjectId == idObjeto
                                select new AccessObject()
                                {
                                    DataInicioDisponibilidade = ov.dteInicio,
                                    DataFimDisponibilidade = ov.dteFim
                                }).ToList().FirstOrDefault();

                return consulta;
            }
        }

        AccessObject IPermissaoRegraItemData.GetAcessoObjetoAtivoComDatasDisponibilidadeAtivas(int idObjeto)
        {
            throw new System.NotImplementedException();
        }


        /*      
        SetAlteracaoOrdem(List<PermissaoRegra> listaPermissoes) // Altera a Ordem de Permissoes        
        AtivaRegraAssociada(idPermissaoObjeto) // Ativa a permissão de visualização de regra
        DesativaRegraAssociada(idPermissaoObjeto) // Desativa a permissão de visualização de regra 
         */




    }
}