using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
    public class AccessEntity : IAccessData
    {

        //michael
         public List<AccessObject> GetAll(int applicationId, int objectTypeId)
        {
            using (var ctx = new DesenvContext())
            {
                var consulta = (from a in ctx.tblAccess_Object
                                join toa in ctx.tblAccess_Object_Application on a.intObjectId equals toa.intObjectId
                                where a.intObjectTypeId == objectTypeId && toa.intApplicationId == applicationId && a.bitAtivo
                                orderby a.intOrdem ascending
                                select new AccessObject()
                                {
                                    Id = a.intObjectId,
                                    Nome = a.txtNome
                                }).AsEnumerable().Distinct().ToList();

                return consulta;
            }
        }

        public List<PermissaoRegra> GetAlunoPermissoes(List<AccessObject> lstObj, int idClient, int applicationId)
        {
            using (var ctx = new DesenvContext())
            {
                var condicoesPreenchidasPeloAluno = new MenuEntity().GetCondicoesPreenchidasPeloAluno(idClient, applicationId);

                var regrasMenus = GetRegras(lstObj, applicationId);

                var botao = from r in regrasMenus
                            group r by r.ObjetoId into g
                            select new { IdMenu = g.Key, PermissoesMenu = g.ToList().OrderBy(x => x.Ordem).ToList() };

                var lstPermissoesMenu = new List<PermissaoRegra>();

                var condicoesRegras = GetRegraCondicoes(applicationId);

                foreach (var itemMenu in botao)
                {
                    var permissao = GetPermissoes(condicoesPreenchidasPeloAluno, itemMenu.PermissoesMenu, condicoesRegras);
                    lstPermissoesMenu.Add(permissao);
                }

                return lstPermissoesMenu;
            }
        }

        public List<Aluno> GetAlunosPorRegra(List<RegraCondicao> listRegraCondicao, Aplicacoes aplicacao)
        {
            throw new NotImplementedException();
        }
        public List<RegraCondicao> GetCondicoesPreenchidasPeloAluno(int matricula, int idAplicacao)
        {
            var ovs = GetCondicoesAlunoOVs(matricula, idAplicacao);
            var chamados = GetCondicoesAlunoChamados(matricula);
            var condicoes = new List<RegraCondicao>();
            condicoes.AddRange(chamados);
            condicoes.AddRange(ovs);
            return condicoes;

        }

        public List<RegraCondicao> GetCondicoesAlunoChamados(int matricula)
        {
            var isCacheEnable = !RedisCacheManager.CannotCache(RedisCacheConstants.Access.KeyGetCondicoesAlunoChamados);
            var key = isCacheEnable ? RedisCacheManager.GenerateKey(RedisCacheConstants.Access.KeyGetCondicoesAlunoChamados, matricula) : null;
            List<RegraCondicao> listaRegraCondicao = isCacheEnable ? RedisCacheManager.GetItemObject<List<RegraCondicao>>(key) : null;

            if (listaRegraCondicao != null)
                return listaRegraCondicao;

            using (var ctx = new DesenvContext())
            {
                var perfilAluno = (from ccc in ctx.tblCallCenterCalls
                                   join d in ctx.tblAccess_Detail on ccc.intStatusInternoID equals d.intStatusInterno
                                   where ccc.intClientID == matricula
                                   && ccc.intStatusID != (int)ChamadoCallCenter.StatusChamado.Excluido
                                   select new
                                   {
                                       //Data = ccc.dteOpen,
                                       StatusInterno = ccc.intStatusInternoID ?? -1,
                                       CallCategory = ccc.intCallCategoryID
                                   }).Distinct().ToList();

                var lCondicoes = new List<RegraCondicao>();
                foreach (var r in perfilAluno)
                {

                    var condicao = new RegraCondicao
                    {
                        TipoAno = RegraCondicao.tipoAno.Todos,
                        Produto = -1,
                        StatusOV = -1,
                        StatusPagamento = -1,
                        StatusInterno = r.StatusInterno,
                        CallCategory = r.CallCategory
                    };

                    var condicaoAjustada = AjustaValidadeChamadoInadimplencia(condicao, matricula);

                    lCondicoes.Add(condicaoAjustada);

                }
                
                listaRegraCondicao = lCondicoes;

                if (isCacheEnable && listaRegraCondicao != null)
                {
                    var timeoutHour = 1;
                    RedisCacheManager.SetItemObject(key, listaRegraCondicao, TimeSpan.FromHours(timeoutHour));
                }

                return listaRegraCondicao;
            }

        }

        public List<RegraCondicao> GetCondicoesAlunoOVs(int matricula, int idAplicacao)
        {
            var isCacheEnable = !RedisCacheManager.CannotCache(RedisCacheConstants.Access.KeyGetCondicoesAlunoOVs);
            var key = isCacheEnable ? RedisCacheManager.GenerateKey(RedisCacheConstants.Access.KeyGetCondicoesAlunoOVs, matricula, idAplicacao) : null;
            List<RegraCondicao> listaRegraCondicao = isCacheEnable ? RedisCacheManager.GetItemObject<List<RegraCondicao>>(key) : null;


            if (listaRegraCondicao != null)
                return listaRegraCondicao;

            using (var ctx = new DesenvContext())
            {
                var produtosExtensivos = new[] { 1, 5 };
                var ajusteSomenteAula = new RegraCondicao();
                var statusPermitidos = new[] { 0, 2, 5 };
                var perfilAluno = (from so in ctx.tblSellOrders
                                   join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                   join prod in ctx.tblProducts on sod.intProductID equals prod.intProductID
                                   join c in ctx.tblCourses on prod.intProductID equals c.intCourseID
                                   //join pgd in ctx.tblPaymentDocuments on so.intOrderID equals pgd.intSellOrderID
                                   //join ccc in ctx.tblCallCenterCalls on so.intClientID equals ccc.intClientID
                                   where so.intClientID == matricula && statusPermitidos.Contains(so.intStatus ?? 1)
                                   orderby so.dteDate descending
                                   select new
                                   {
                                       IdOv = so.intOrderID,
                                       Produto = prod.intProductGroup1 ?? -1,
                                       StatusOV = so.intStatus.Value,
                                       StatusPagamento = so.intStatus2,
                                       Ano = c.intYear.Value,
                                       Turma = c.intCourseID,
                                       Filial = so.intStoreID
                                   }).Distinct();
                var condicoesAluno = new List<RegraCondicao>();
                var isAntesDataLimite = Utilidades.IsBeforeDataLimite(idAplicacao, Utilidades.GetYear());
                foreach (var r in perfilAluno)
                {
                    var tipoAno = GetTipoAno(r.Ano);
                    var statusOv = OrdemVendaEntity.GetRealStatusOv((OrdemVenda.StatusOv)r.StatusOV, (RegraCondicao.tipoAno)tipoAno, matricula, isAntesDataLimite);

                    if (produtosExtensivos.Contains(r.Produto))
                        ajusteSomenteAula = AjustaChamadoSomenteAula(r.IdOv, matricula);
                    else
                        ajusteSomenteAula = new RegraCondicao() { CallCategory = -1, StatusInterno = -1 };

                    var condicaoSomenteAula = new RegraCondicao
                    {
                        ClientId = matricula,
                        TipoAno = tipoAno,
                        Produto = r.Produto,
                        StatusOV = (int)statusOv,
                        StatusPagamento = r.StatusPagamento ?? -1,
                        StatusInterno = ajusteSomenteAula.StatusInterno,
                        CallCategory = ajusteSomenteAula.CallCategory,
                        CourseId = r.Turma,
                        StoreId = r.Filial
                    };
                    var ajusteStatusInadimplencia = AjustaChamadoInadimplencia(condicaoSomenteAula, r.IdOv, matricula);
                    condicoesAluno.Add(condicaoSomenteAula);

                }

                if (VerificaDireitoRevalida(matricula, condicoesAluno, isAntesDataLimite, idAplicacao))
                {
                    var condicaoRevalida = new RegraCondicao  //Add ov de Revalida
                    {
                        ClientId = matricula,
                        TipoAno = RegraCondicao.tipoAno.Atual,
                        Produto = 63,
                        StatusOV = -1,
                        StatusPagamento = -1,
                        StatusInterno = ajusteSomenteAula.StatusInterno,
                        CallCategory = ajusteSomenteAula.CallCategory,
                        CourseId = -1,
                        StoreId = -1
                    };

                    condicoesAluno.Add(condicaoRevalida);
                }

                listaRegraCondicao = condicoesAluno;

                if (isCacheEnable && listaRegraCondicao != null)
                {
                    var timeoutHour = 1;
                    RedisCacheManager.SetItemObject(key, listaRegraCondicao, TimeSpan.FromHours(timeoutHour));
                }

                return listaRegraCondicao;
            }
        }

        public bool VerificaDireitoRevalida(int matricula, List<RegraCondicao> condicoesAluno, bool isAntesDataLimite, int idAplicacao)
        {

            List<int> groupsIds = new List<int>() { 1, 5, 8, 9, 14, 57, 58, 61 };

            var IsAnoAtual = condicoesAluno.Where(x => x.TipoAno == RegraCondicao.tipoAno.Atual && groupsIds.Contains(x.Produto)).Any();
            var IsAnoAnterior = condicoesAluno.Where(x => x.TipoAno == RegraCondicao.tipoAno.Anterior && groupsIds.Contains(x.Produto)).Any();

            var dataLiberacao = ObterDataLiberacaoRevalida(idAplicacao);

            using (var ctx = new DesenvContext())
            {
                if (dataLiberacao != default(DateTime))
                    return ctx.tblClients.Where(x => x.intClientID == matricula &&
                                                      (x.txtArea.Contains(Constants.TEXT_AREA_REVALIDA)
                                                      || x.intEspecialidadeID == Constants.ID_ESPECIALIDADE_REVALIDA)
                                                      && (DateTime.Now >= dataLiberacao && IsAnoAtual
                                                      || IsAnoAnterior && isAntesDataLimite)).Any();
            }

            return false;
        }

        public DateTime ObterDataLiberacaoRevalida(int idAplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                return (from ov in ctx.tblAcess_Object_Validity
                        join o in ctx.tblAccess_Object on ov.intObjectId equals o.intObjectId
                        where o.intApplicationId == idAplicacao && o.txtNome.Contains("Libera Revalida")
                        select ov.dteInicio).FirstOrDefault();
            }
        }

        private object AjustaChamadoInadimplencia(RegraCondicao condicao, int idOv, int matricula)
        {
            if (condicao.StatusInterno != -1) return condicao;
            var retorno = condicao;
            using (var ctx = new DesenvContext())
            {

                var chamado = (from cc in ctx.tblCallCenterCalls
                               join cci in ctx.tblCallCenterCallsInadimplencia on cc.intCallCenterCallsID equals cci.intCallCenterCallsID
                               where (Constants.INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO.Contains(cc.intStatusInternoID ?? 0)
                               || Constants.INADIMPLENCIA_CHAMADO_AVISOBLOQUEIO == cc.intStatusInternoID)
                               && cc.intStatusID != 7
                               && cc.intClientID == matricula
                               && cci.intOrderID == idOv
                               && cc.dteOpen.Year == DateTime.Now.Year
                               && cc.dteOpen.Month == DateTime.Now.Month
                               orderby cc.dteOpen descending
                               select cc).FirstOrDefault();
                if (chamado != null)
                {
                    retorno.StatusInterno = chamado.intStatusInternoID ?? 0;
                    return AjustaValidadeChamadoInadimplencia(retorno, matricula, idOv);
                }
            }

            return retorno;
        }

       public RegraCondicao AjustaValidadeChamadoInadimplencia(RegraCondicao condicao, int matricula, int ov = 0)
        {
            var condicaoretorno = condicao;
            var isChamadoPrimeiroAviso = Constants.INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO.Contains(condicao.StatusInterno);
            var isChamadoBloqueio = Constants.INADIMPLENCIA_CHAMADO_AVISOBLOQUEIO.Equals(condicao.StatusInterno);

            if (!isChamadoPrimeiroAviso && !isChamadoBloqueio) return condicaoretorno;
            if (isChamadoPrimeiroAviso) return AjustaChamadoPrimeiroAviso(condicao, matricula, ov);
            if (isChamadoBloqueio) return AjustaChamadoBloqueio(condicao, matricula);

            return condicaoretorno;
        }

        private RegraCondicao AjustaChamadoBloqueio(RegraCondicao condicao, int matricula)
        {
            var condicaoretorno = condicao;
            using (var ctx = new DesenvContext())
            {
                var HasPrimeiroAviso = ctx.tblCallCenterCalls.Where(x => Constants.INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO.Contains(x.intStatusInternoID ?? 0)
                    && x.intStatusID != 7
                    && x.intClientID == matricula
                    && x.dteDataPrevisao2.Value < DateTime.Now
                    ).Any();


                if (!HasPrimeiroAviso)
                {
                    condicaoretorno.StatusInterno = -1;
                }
            }
            return condicaoretorno;
        }

        public RegraCondicao AjustaChamadoPrimeiroAviso(RegraCondicao condicao, int matricula, int ov = 0)
        {
            var condicaoretorno = condicao;
            using (var ctx = new DesenvContext())
            {
                //var chamado = ctx.tblCallCenterCallsInadimplencia.Where(o => o.intOrderID == ov).OrderByDescending(o => o.intCallCenterInadimplenciaID).FirstOrDefault();

                //var chamado = (from ccIn in ctx.tblCallCenterCallsInadimplencia
                //               join cc in ctx.tblCallCenterCalls on ccIn.intCallCenterCallsID equals cc.intCallCenterCallsID
                //               where (ov != 0 && ccIn.intOrderID == ov && cc.intCallCategoryID == 2277)
                //               select new 
                //               {
                //                   id = ccIn.intCallCenterCallsID
                //               }).FirstOrDefault();


                //var objCcc = ctx.tblCallCenterCalls.Where(x => x.intStatusInternoID == condicao.StatusInterno && x.intStatusID != 7 && x.intCallCenterCallsID == chamado.id).FirstOrDefault();

                var objCcc = (from cc in ctx.tblCallCenterCalls
                              join ci in ctx.tblCallCenterCallsInadimplencia on cc.intCallCenterCallsID equals ci.intCallCenterCallsID
                              where cc.intStatusInternoID == condicao.StatusInterno && cc.intStatusID != 7 && ((ov == 0 && cc.intClientID == matricula) || ci.intOrderID == ov) && cc.intCallCategoryID == 2277
                              select new
                              {
                                  dteDataPrevisao2 = cc.dteDataPrevisao2
                              }).FirstOrDefault();

                var HasChamadoInadimplenciaVencido = false;

                if ((objCcc != null && objCcc.dteDataPrevisao2 != null) && objCcc.dteDataPrevisao2 < DateTime.Now.Date)
                    HasChamadoInadimplenciaVencido = true;

                //var HasChamadoInadimplenciaVencido = ctx.tblCallCenterCalls.Where(x => x.intStatusInternoID == condicao.StatusInterno && x.intStatusID != 7 && x.dteDataPrevisao2 < DateTime.Now.Date).Any();
                if (HasChamadoInadimplenciaVencido)
                {
                    condicaoretorno.StatusInterno = Constants.INADIMPLENCIA_CHAMADO_AVISOBLOQUEIO;
                }
            }
            return condicaoretorno;

        }

        public RegraCondicao AjustaChamadoSomenteAula(int idOv, int matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var chamado = ctx.tblCallCenterCalls.Where(c => c.intClientID == matricula && c.intStatusInternoID == 8037).FirstOrDefault();

                    var retorno = new RegraCondicao();

                    if (chamado == null || chamado.dteOpen.Year != DateTime.Now.Year)
                        return retorno = new RegraCondicao()
                        {
                            CallCategory = -1,
                            StatusInterno = -1
                        };
                    else
                    {
                        var oV = ctx.tblSellOrders.Where
                            (so => so.intOrderID == idOv).FirstOrDefault();

                        if (Convert.ToDateTime(oV.dteDate).Year != chamado.dteOpen.Year)
                            return retorno = new RegraCondicao()
                            {
                                CallCategory = -1,
                                StatusInterno = -1
                            };

                        if (oV.dteDate < chamado.dteOpen)
                            return retorno = new RegraCondicao()
                            {
                                CallCategory = 171,
                                StatusInterno = 8037
                            };
                        else
                            return retorno = new RegraCondicao()
                            {
                                CallCategory = -1,
                                StatusInterno = -1
                            };
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        public RegraCondicao.tipoAno GetTipoAno(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var consulta = ctx.tblAccess_Year_Type.Where(x => x.intAno == ano).Select(y => y.intTipoAnoId);
                var retorno = (RegraCondicao.tipoAno)consulta.FirstOrDefault();
                return retorno == RegraCondicao.tipoAno.Inexistente ? RegraCondicao.tipoAno.Outros : retorno;

            }

        }

        public PermissaoRegra GetPermissoes(List<RegraCondicao> condicoesPreenchidasPeloAluno, List<PermissaoRegra> permissoes, List<RegraCondicao> condicoesRegras)
        {
            foreach (var permissao in permissoes)
            {
                if (AlunoPossuiPermissao(condicoesPreenchidasPeloAluno, permissao, condicoesRegras))
                    return permissao;

            }
            return new PermissaoRegra();
        }

       public bool AlunoPossuiPermissao(List<RegraCondicao> condicoesPreenchidasPeloAluno, PermissaoRegra permissao, List<RegraCondicao> condicoesRegras)
        {
            var condicoesDaRegra = condicoesRegras.Where(c => c.IdRegra == permissao.Regra.Id);
            var regraPossuiCondicoes = condicoesDaRegra.Count() > 0;
            if (!regraPossuiCondicoes) return false;
            foreach (var condicaoDaRegra in condicoesDaRegra)
            {
                if (!AlunoPreencheCondicao(condicoesPreenchidasPeloAluno, condicaoDaRegra))
                    return false;
            }
            return true;
        }

        public bool AlunoPreencheCondicao(List<RegraCondicao> condicoesPreenchidasPeloAluno, RegraCondicao condicaoDaRegra)
        {
            foreach (var condicaoDoAluno in condicoesPreenchidasPeloAluno)
            {
                if (IsCondicaoNaRegra(condicaoDoAluno, condicaoDaRegra))
                    return true;
            }
            return false;
        }

        public List<RegraCondicao> GetRegraCondicoes(int? idAplicacao, int regraId = 0)
        {
            var isCacheEnable = !RedisCacheManager.CannotCache(RedisCacheConstants.Access.KeyGetRegraCondicoes);
            var key = isCacheEnable ? RedisCacheManager.GenerateKey(RedisCacheConstants.Access.KeyGetRegraCondicoes, idAplicacao, regraId) : null;
            List<RegraCondicao> listaRegraCondicao = isCacheEnable ? RedisCacheManager.GetItemObject<List<RegraCondicao>>(key) : null;


            if (listaRegraCondicao != null)
                return listaRegraCondicao;
                
            List<RegraCondicao> lRegras = new List<RegraCondicao>();

            using (var ctx = new DesenvContext())
            {
                lRegras = (from det in ctx.mview_CondicoesRegra_Group
                           join ea in ctx.tblAccess_Empresa_Application on det.intEmpresaId equals ea.intEmpresaId
                           where ea.intApplicationId == idAplicacao
                                 && (regraId == 0 ? true : regraId == det.intRegraId)
                           select new RegraCondicao
                           {
                               Id = det.intDetalheId,
                               IdRegra = det.intRegraId,
                               ClientId = det.intClientId,
                               TipoAno = (RegraCondicao.tipoAno)det.intTipoAnoId,
                               Produto = det.intProductGroupId,
                               StatusOV = det.intStatusOV,
                               StatusPagamento = det.intStatusPagamento,
                               CallCategory = det.intCallCategory,
                               StatusInterno = det.intStatusInterno,
                               DataUltimaAlteracao = det.dteUltimaAlteracao ?? DateTime.MinValue,
                               Ativo = det.bitAtivo,
                               EmpresaId = det.intEmpresaId,
                               StoreId = det.intStoreID,
                               CourseId = det.intCourseID,
                               GroupId = det.intGroupId,
                               GroupClientId = det.intGroupClientId,
                               AnoMinimo = det.intAnoMinimo
                           }).ToList();
            }

            var regrasComGrupo = lRegras.Where(x => !x.GroupId.Equals(-1)).ToList();
            if (regrasComGrupo.Any())
            {
                var regraComIdsAgrupados = regrasComGrupo
                                                         .GroupBy(rcg => new
                                                         {
                                                             rcg.Id,
                                                             rcg.IdRegra,
                                                             rcg.ClientId,
                                                             rcg.TipoAno,
                                                             rcg.Produto,
                                                             rcg.StatusOV,
                                                             rcg.StatusPagamento,
                                                             rcg.CallCategory,
                                                             rcg.StatusInterno,
                                                             rcg.DataUltimaAlteracao,
                                                             rcg.Ativo,
                                                             rcg.EmpresaId,
                                                             rcg.StoreId,
                                                             rcg.CourseId,
                                                             rcg.GroupId
                                                         }).Select(r => new RegraCondicao
                                                         {
                                                             Id = r.Key.Id,
                                                             IdRegra = r.Key.IdRegra,
                                                             ClientId = r.Key.ClientId,
                                                             TipoAno = r.Key.TipoAno,
                                                             Produto = r.Key.Produto,
                                                             StatusOV = r.Key.StatusOV,
                                                             StatusPagamento = r.Key.StatusPagamento,
                                                             CallCategory = r.Key.CallCategory,
                                                             StatusInterno = r.Key.StatusInterno,
                                                             DataUltimaAlteracao = r.Key.DataUltimaAlteracao,
                                                             Ativo = r.Key.Ativo,
                                                             EmpresaId = r.Key.EmpresaId,
                                                             StoreId = r.Key.StoreId,
                                                             CourseId = r.Key.CourseId,
                                                             GroupId = r.Key.GroupId,
                                                             ListGroupClientId = r.Select(x => x.GroupClientId).ToList()
                                                         }).ToList();

                var lRegrasGrupo = regraComIdsAgrupados;
                lRegrasGrupo.AddRange(lRegras.Where(x => x.GroupId.Equals(-1)));
                lRegras = lRegrasGrupo;

            }

            listaRegraCondicao = lRegras;


            if (isCacheEnable && listaRegraCondicao != null)
            {
                var timeoutHour = 24;
                RedisCacheManager.SetItemObject(key, listaRegraCondicao, TimeSpan.FromHours(timeoutHour));
            }

            return listaRegraCondicao;
        }

        public List<PermissaoRegra> GetRegras(List<AccessObject> accessObjects, int idAplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var lstRegraDetalhes = (from m in accessObjects
                                        join permObj in ctx.tblAccess_PermissionObject on m.Id equals permObj.intObjectId
                                        join perm in ctx.tblAccess_Permission_Rule on permObj.intPermissaoRegraId equals perm.intPermissaoRegraId
                                        join rule in ctx.tblAccess_Rule on perm.intRegraId equals rule.intRegraId
                                        where perm.bitAtivo == true
                                            && rule.bitAtivo == true
                                            && (perm.dteValidoDe == null || perm.dteValidoDe <= Utilidades.GetServerDate())
                                            && (perm.dteValidoAte == null || perm.dteValidoAte >= Utilidades.GetServerDate())
                                            && (permObj.intApplicationID == -1 || permObj.intApplicationID == idAplicacao)
                                        select new PermissaoRegra()
                                        {
                                            Id = perm.intPermissaoRegraId,
                                            Regra = new Regra { Id = perm.intRegraId },
                                            ObjetoId = permObj.intObjectId,
                                            Ordem = permObj.intOrdem,
                                            AcessoId = perm.intAccessoId,
                                            MensagemId = perm.intMensagemId ?? -1,
                                            IsDataLimite = perm.bitDataLimite
                                        }).Distinct().ToList();

                return lstRegraDetalhes;
            }
        }

        public List<PermissaoRegra> GetRegrasNotificacoes(List<AccessObject> listObjects)
        {
            using (var ctx = new DesenvContext())
            {
                var lstRegraDetalhes = (from m in listObjects
                                        join permObj in ctx.tblAccess_PermissionNotification on m.Id equals permObj.intNotificacaoId
                                        join perm in ctx.tblAccess_Permission_Rule on permObj.intPermissaoRegra equals perm.intPermissaoRegraId
                                        join rule in ctx.tblAccess_Rule on perm.intRegraId equals rule.intRegraId
                                        where perm.bitAtivo == true 
                                            && rule.bitAtivo == true
                                            && (perm.dteValidoDe == null || perm.dteValidoDe <= Utilidades.GetServerDate())
                                            && (perm.dteValidoAte == null || perm.dteValidoAte >= Utilidades.GetServerDate())
                                        select new PermissaoRegra()
                                        {
                                            Id = perm.intPermissaoRegraId,
                                            Regra = new Regra { Id = perm.intRegraId },
                                            ObjetoId = permObj.intNotificacaoId,
                                            Ordem = permObj.intOrdem,
                                            AcessoId = perm.intAccessoId,
                                            MensagemId = perm.intMensagemId ?? -1,
                                            IsDataLimite = perm.bitDataLimite
                                        }).Distinct().ToList();

                return lstRegraDetalhes;
            }
        }

        public bool IsCondicaoNaRegra(RegraCondicao condicao, RegraCondicao condicaoDaRegra)
        {
            var isStatusInterno = (condicaoDaRegra.StatusInterno.Equals(-1)) ? true : condicao.StatusInterno == condicaoDaRegra.StatusInterno;
            if (!isStatusInterno) return false;

            var isTipoAno = (condicaoDaRegra.TipoAno.Equals((RegraCondicao.tipoAno)(-1))) ? true : condicao.TipoAno == condicaoDaRegra.TipoAno;
            if (!isTipoAno) return false;

            var isProduto = (condicaoDaRegra.Produto.Equals(-1)) ? true : condicao.Produto == condicaoDaRegra.Produto;
            if (!isProduto) return false;

            var isStatusOV = (condicaoDaRegra.StatusOV.Equals(-1)) ? true : condicao.StatusOV == condicaoDaRegra.StatusOV;
            if (!isStatusOV) return false;

            var isStatusPagamento = (condicaoDaRegra.StatusPagamento.Equals(-1)) ? true : condicao.StatusPagamento == condicaoDaRegra.StatusPagamento;
            if (!isStatusPagamento) return false;

            var isCliente = (condicaoDaRegra.ClientId.Equals(-1)) ? true : condicao.ClientId == condicaoDaRegra.ClientId;
            if (!isCliente) return false;

            var isCallCategory = (condicaoDaRegra.CallCategory.Equals(-1)) ? true : condicao.CallCategory == condicaoDaRegra.CallCategory;
            if (!isCallCategory) return false;

            var isStore = (condicaoDaRegra.StoreId.Equals(-1)) ? true : condicao.StoreId == condicaoDaRegra.StoreId;
            if (!isStore) return false;

            var isCourse = (condicaoDaRegra.CourseId.Equals(-1)) ? true : condicao.CourseId == condicaoDaRegra.CourseId;
            if (!isCourse) return false;

            var isClienteGrupo = (condicaoDaRegra.GroupId.Equals(-1)) ? true : condicaoDaRegra.ListGroupClientId.Contains(condicao.ClientId);
            if (!isClienteGrupo) return false;

            var isOVMinimaPermitida = condicaoDaRegra.AnoMinimo.Equals(-1) ? true : condicao.AnoOV >= condicaoDaRegra.AnoMinimo;
            if (!isOVMinimaPermitida) return false;

            return true;

        }
    }
}