using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using static MedCore_DataAccess.Repository.CronogramaEntity;

namespace MedCore_DataAccess.Repository
{
    public class MenuEntity : IMenuData
    {
        private readonly AccessEntity _accessEntity;

        public MenuEntity()
        {
            _accessEntity = new AccessEntity();
        }

        public List<Menu> GetAll(int idAplicacao, string versao = "" )
        {
            var lstMenu = new List<Menu>();
            using (var ctx = new DesenvContext())
            {
                var consulta = (from o in ctx.tblAccess_Object
                                join menu in ctx.tblAccess_Menu on o.intObjectId equals menu.intObjectId
                                join toa in ctx.tblAccess_Object_Application on o.intObjectId equals toa.intObjectId
                                where o.intObjectTypeId == 1 && toa.intApplicationId == idAplicacao && o.bitAtivo == true && o.bitAcessoEspecial == false
                                orderby o.intOrdem ascending
                                select new Menu()
                                {
                                    Id = o.intObjectId,
                                    IdPai = o.intPaiId,
                                    IdAplicacao = idAplicacao,
                                    Nome = menu.txtNome,
                                    Url = menu.txtUrl,
                                    Target = menu.txtTarget,
                                    Ordem = o.intOrdem,
                                    Autenticacao = (menu.bitAutenticacao) ? 1 : 0,
                                    Novo = (menu.bitNovo) ? 1 : 0,
                                    VersaoMinima = toa.txtMinVersion,
                                    PermiteOffline = (toa.bitPermiteOffline) ? 1 : 0,
                                    VersaoMinimaOffline = toa.txtMinVersionOffline,
                                    ExternalPagesUrl = menu.txtExternalPageUrl                                    
                                }).Distinct().ToList();

                if (string.IsNullOrEmpty(versao)) versao = "0.0.0";
                Version version = Version.Parse(versao);

                return consulta.Where(x => Version.Parse(x.VersaoMinima) <= version).ToList();
            }
        }


        public List<PermissaoRegra> GetAlunoPermissoesMenu(List<Menu> lstMenu, int idClient, int idAplicacao, DateTime? data = null, int idProduto = 0)
        {

            using (var ctx = new DesenvContext())
            {
                var aplicacoesServico = Utilidades.AplicacoesServico();
                var condicoesPreenchidasPeloAluno = GetCondicoesPreenchidasPeloAluno(idClient, idAplicacao);
                var condicoesPreenchidasPeloAlunoProduto = condicoesPreenchidasPeloAluno.Where(x => idProduto == 0 || idProduto == (int)Produto.Produtos.NAO_DEFINIDO || x.Produto == idProduto).ToList();

                var acessoEspecial = GetAcessoEspecialAluno(idClient);

                if (acessoEspecial.Any())
                    lstMenu.AddRange(acessoEspecial);

                var regrasMenus = GetRegrasMenu(lstMenu, idAplicacao);
                var isBeforeDataLimite = Utilidades.IsBeforeDataLimite(idAplicacao, Utilidades.GetYear());
                if (!isBeforeDataLimite)
                {
                    regrasMenus.RemoveAll(s => s.IsDataLimite == true);
                }

                var menu = from r in regrasMenus
                           group r by r.ObjetoId into g
                           select new { IdMenu = g.Key, PermissoesMenu = g.ToList().OrderBy(x => x.Ordem).ToList() };

                var lstPermissoesMenu = new List<PermissaoRegra>();

                var condicoesRegras = _accessEntity.GetRegraCondicoes(idAplicacao);

                var produtosPermissoesSeparadas = Utilidades.ProdutosR3();
                produtosPermissoesSeparadas.Add(Produto.Produtos.MEDELETRO.GetHashCode());
                produtosPermissoesSeparadas.Add(Produto.Produtos.MEDELETRO_IMED.GetHashCode());
                produtosPermissoesSeparadas.Add(Produto.Produtos.CPMED.GetHashCode());

                var condicoesPreenchidasPeloAlunoDemaisProduto = condicoesPreenchidasPeloAluno.Where(x => idProduto == 0 || idProduto == (int)Produto.Produtos.NAO_DEFINIDO || !produtosPermissoesSeparadas.Contains(x.Produto)).ToList();

                foreach (var itemMenu in menu)
                {

                    if (produtosPermissoesSeparadas.Contains(idProduto) && IsMenuMain(itemMenu.IdMenu))
                    {
                        var permissao = GetPermissoesAoItemDoAplicativo(condicoesPreenchidasPeloAlunoProduto, itemMenu.PermissoesMenu, condicoesRegras);
                        lstPermissoesMenu.Add(permissao);
                    }
                    else if(aplicacoesServico.Contains(idAplicacao))
                    {
                        var permissao = GetPermissoesAoItemDoAplicativo(condicoesPreenchidasPeloAluno, itemMenu.PermissoesMenu, condicoesRegras);
                        lstPermissoesMenu.Add(permissao);
                    }
                    else
                    {
                        var permissao = GetPermissoesAoItemDoAplicativo(condicoesPreenchidasPeloAlunoDemaisProduto, itemMenu.PermissoesMenu, condicoesRegras);
                        lstPermissoesMenu.Add(permissao);
                    }
                }

                var HasMenuRevisaoDeEstudos = lstMenu.Any(x => x.Id == Constants.MenuRevisaoDeEstudos);
                if (HasMenuRevisaoDeEstudos && PodeVerRevisaoEstudos(idClient, data))
                {
                    var pr = new PermissaoRegra
                    {
                        AcessoId = 3,
                        Ativo = true,
                        ObjetoId = 79,
                        MensagemId = -1,
                    };
                    lstPermissoesMenu.Add(pr);
                }

                var HasMenuRoteiroDeTreinamento = lstMenu.Any(x => x.Id == Constants.MenuRoteiroDeTreinamento);
                if (HasMenuRoteiroDeTreinamento && PodeVerRoteiroDeTreinamento(idClient, data))
                {
                    var pr = new PermissaoRegra
                    {
                        AcessoId = 3,
                        Ativo = true,
                        ObjetoId = 14,
                        MensagemId = -1,
                    };
                    lstPermissoesMenu.Add(pr);
                }

                return lstPermissoesMenu;
            }
        }

        public List<PermissaoRegra> PermissaoMenuConcursoNaIntegra(List<PermissaoRegra> lstPermissao)
        {
            var menuConcursonaIntegra = lstPermissao.Find(p => p.ObjetoId == 35);

            if (menuConcursonaIntegra == null)
                return lstPermissao;

            var extensivo = lstPermissao.Find(p => p.ObjetoId == 40 && p.MensagemId == 17);
            var medeletro = lstPermissao.Find(p => p.ObjetoId == 51 && p.MensagemId == 21);

            if (extensivo != null && medeletro != null)
            {
                menuConcursonaIntegra.MensagemId = extensivo.MensagemId;
                menuConcursonaIntegra.AcessoId = extensivo.AcessoId;
            }
            else
            {
                menuConcursonaIntegra.MensagemId = -1;
                menuConcursonaIntegra.AcessoId = (int)Utilidades.PermissaoStatus.AcessoPermitido;
            }

            return lstPermissao;
        }

        public List<PermissaoRegra> GetRegrasMenu(List<Menu> menu, int idAplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var lstRegraDetalhes = (from m in menu
                                        join permObj in ctx.tblAccess_PermissionObject on m.Id equals permObj.intObjectId
                                        join perm in ctx.tblAccess_Permission_Rule on permObj.intPermissaoRegraId equals perm.intPermissaoRegraId
                                        join rule in ctx.tblAccess_Rule on perm.intRegraId equals rule.intRegraId
                                        //join detail in ctx.tblAccess_Rule_Detail on rule.intRegraId equals detail.intRegraId
                                        where perm.bitAtivo == true && rule.bitAtivo == true
                                        && (permObj.intApplicationID == -1 || permObj.intApplicationID == idAplicacao)
                                        select new PermissaoRegra()
                                        {
                                            Id = perm.intPermissaoRegraId,
                                            Regra = new Regra { Id = perm.intRegraId },
                                            ObjetoId = permObj.intObjectId, //perm.intObjectId,
                                            Ordem = permObj.intOrdem,
                                            AcessoId = perm.intAccessoId,
                                            MensagemId = perm.intMensagemId ?? -1,
                                            IsDataLimite = perm.bitDataLimite
                                        }).Distinct().ToList();
                //TODO: só trazer métodos com o interruptor ativo
                return lstRegraDetalhes;

            }
        }

        public PermissaoRegra GetPermissoesAoItemDoAplicativo(List<RegraCondicao> condicoesPreenchidasPeloAluno, List<PermissaoRegra> permissoes, List<RegraCondicao> condicoesRegras)
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
                if (_accessEntity.IsCondicaoNaRegra(condicaoDoAluno, condicaoDaRegra))
                    return true;
            }
            return false;
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

        public List<RegraCondicao> GetCondicoesAlunoOVs(int matricula, int idAplicacao)
        {
            var aplicacoesServico = Utilidades.AplicacoesServico();
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
                                       GrupoProduto = prod.intProductGroup1 ?? -1,
                                       GrupoProduto2 = prod.intProductGroup2 ?? -1,
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
                    if (!(r.GrupoProduto == (int)Produto.Produtos.CPMED && r.GrupoProduto2 != (int)Produto.Produtos.APOSTILA_CPMED))
                    {
                        var tipoAno = GetTipoAno(r.Ano);
                        var statusOv = OrdemVendaEntity.GetRealStatusOv((OrdemVenda.StatusOv)r.StatusOV, (RegraCondicao.tipoAno)tipoAno, matricula, isAntesDataLimite);

                        if (produtosExtensivos.Contains(r.GrupoProduto))
                            ajusteSomenteAula = AjustaChamadoSomenteAula(r.IdOv, matricula);
                        else
                            ajusteSomenteAula = new RegraCondicao() { CallCategory = -1, StatusInterno = -1 };

                        var condicaoSomenteAula = new RegraCondicao
                        {
                            ClientId = matricula,
                            TipoAno = tipoAno,
                            Produto = r.GrupoProduto,
                            StatusOV = (int)statusOv,
                            StatusPagamento = r.StatusPagamento ?? -1,
                            StatusInterno = ajusteSomenteAula.StatusInterno,
                            CallCategory = ajusteSomenteAula.CallCategory,
                            CourseId = r.Turma,
                            StoreId = r.Filial,
                            AnoOV = r.Ano
                        };
                        var ajusteStatusInadimplencia = AjustaChamadoInadimplencia(condicaoSomenteAula, r.IdOv, matricula);

                        if (aplicacoesServico.Contains(idAplicacao) && condicaoSomenteAula.StatusInterno < default(int))
                        {
                            condicaoSomenteAula.StatusInterno = ObterUltimoStatusChamadoInadimplencia(r.IdOv, matricula);
                        }
                        condicoesAluno.Add(condicaoSomenteAula);
                    }
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

                return condicoesAluno;
            }
        }

        private int ObterUltimoStatusChamadoInadimplencia(int idOv, int matricula)
        {
            var status = (int)ChamadoCallCenter.StatusChamado.SemStatus;
            using (var ctx = new DesenvContext())
            {
                var chamado = (from cc in ctx.tblCallCenterCalls
                               join cci in ctx.tblCallCenterCallsInadimplencia on cc.intCallCenterCallsID equals cci.intCallCenterCallsID
                               where (Constants.INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO.Contains(cc.intStatusInternoID ?? 0)
                               || Constants.INADIMPLENCIA_CHAMADO_AVISOBLOQUEIO == cc.intStatusInternoID)
                               && cc.intStatusID != (int)ChamadoCallCenter.StatusChamado.Excluido
                               && cc.intClientID == matricula
                               && cci.intOrderID == idOv
                               orderby cc.dteOpen descending
                               select cc).FirstOrDefault();

                if(chamado != null && chamado.intStatusInternoID.HasValue)
                {
                    status = chamado.intStatusInternoID.Value;
                }
                return status;
            }
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

        public List<RegraCondicao> GetCondicoesAlunoChamados(int matricula)
        {
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
                return lCondicoes;
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
                    && (x.dteDataPrevisao2.Value) < DateTime.Now
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

        public bool IsAcesso(int idMenu, int matricula, int idAplicacao, DateTime? data = null)
        {
            var isAcesso = false;
            using (var ctx = new DesenvContext())
            {
                var lstMenu = new List<Menu>();
                var menu = new Menu() { Id = idMenu };
                lstMenu.Add(menu);
                var menuPermissao = GetAlunoPermissoesMenu(lstMenu, matricula, idAplicacao, data).Find(p => p.ObjetoId == menu.Id);

                if (menuPermissao != null)
                    if (menuPermissao.AcessoId == (int)Utilidades.PermissaoStatus.AcessoPermitido)
                        isAcesso = true;
            }
            return isAcesso;
        }

        public bool PodeVerRevisaoEstudos(int matricula, DateTime? dataValidacao = null)
        {
            var lstRevisaoEstudo = new List<int>();
            DateTime AulaAssistida = new DateTime();
            lstRevisaoEstudo.Add(Constants.AulaCartaMeioDeAnoMed);
            lstRevisaoEstudo.Add(Constants.AulaCartaMeioDeAnoMedcurso);

            using (var ctx = new DesenvContext())
            {
                foreach (var item in lstRevisaoEstudo)
                {
                    AulaAssistida = (DateTime)(ctx.Set<msp_HoraAulaTema_Result>().FromSqlRaw("msp_HoraAulaTema @intLessonTitleID = {0}, @intClientID = {1}", item, matricula).ToList().FirstOrDefault().dteDateTime ?? DateTime.MaxValue);
                    if (AulaAssistida != DateTime.MaxValue)
                        if (dataValidacao == null)
                            return AulaAssistida < DateTime.Now;
                        else
                            return AulaAssistida < dataValidacao;
                }
            }
            return false;
        }

        public bool PodeVerRoteiroDeTreinamento(int matricula, DateTime? dataValidacao = null)
        {
            var lstRoteiroTreinamento = new List<int>();
            DateTime AulaAssistida = new DateTime();
            lstRoteiroTreinamento.Add(Constants.AulaRoteirodeTreinamentoMed);
            lstRoteiroTreinamento.Add(Constants.AulaRoteirodeTreinamentoMedCurso);

            using (var ctx = new DesenvContext())
            {
                foreach (var item in lstRoteiroTreinamento)
                {
                    

                    AulaAssistida = (DateTime)(ctx.Set<msp_HoraAulaTema_Result>().FromSqlRaw("msp_HoraAulaTema @intLessonTitleID = {0}, @intClientID = {1}", item, matricula).ToList().FirstOrDefault().dteDateTime ?? DateTime.MaxValue);
                    if (AulaAssistida != DateTime.MaxValue)
                        if (dataValidacao == null)
                            return AulaAssistida < DateTime.Now;
                        else
                            return AulaAssistida < dataValidacao;
                }
            }
            return false;
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

        public List<Menu> GetAcessoEspecialAluno(int matricula)
        {
            var lstMenu = new List<Menu>();
            using (var ctx = new DesenvContext())
            {

                var acessoEspecial = (from o in ctx.tblAccess_Object
                                      join m in ctx.tblAccess_ObjectType on o.intObjectTypeId equals m.intObjectTypeId
                                      join menu in ctx.tblAccess_Menu on o.intObjectId equals menu.intObjectId
                                      join acesso in ctx.tblMedSoft_AcessoEspecial on o.intObjectId equals acesso.intObjectId
                                      where o.intObjectTypeId == 1 && o.intApplicationId == 17 && o.bitAtivo == true && o.bitAcessoEspecial == true && acesso.intClientID == matricula
                                      orderby o.intOrdem ascending
                                      select new Menu()
                                      {
                                          Id = o.intObjectId,
                                          IdPai = o.intPaiId,
                                          IdAplicacao = o.intApplicationId,
                                          Nome = menu.txtNome,
                                          Url = menu.txtUrl,
                                          Target = menu.txtTarget,
                                          Ordem = o.intOrdem,
                                          Autenticacao = (menu.bitAutenticacao) ? 1 : 0,
                                          Novo = (menu.bitNovo) ? 1 : 0
                                      }).Distinct().ToList();

                return acessoEspecial;
            }

        }

        public List<ProdutoEmpresa> GetProdutosEmpresa(RegraCondicao condicaoDaRegra)
        {
            using (var ctx = new DesenvContext())
            {
                var produtos = (from pe in ctx.tblAccess_ProdutoEmpresas
                                join e in ctx.tblAccess_Empresas on pe.intEmpresaId equals e.intEmpresaId
                                where condicaoDaRegra.EmpresaId == pe.intEmpresaId
                                select new ProdutoEmpresa()
                                {
                                    EmpresaId = e.intEmpresaId,
                                    ProdutoId = pe.intProductGroupId,

                                }).ToList();


                return produtos;
            }
        }

        public List<int> ObterMenusPermitidoParaProduto(int idProduto)
        {
            using (var ctx = new DesenvContext())
            {
                var idsPermitidos = ctx.tblAccess_MenuProduto
                    .Where(x => x.intProdutoID == idProduto)
                    .Select(y => y.intMenuID)
                    .ToList();
                return idsPermitidos;
            }
        }

        public bool IsMenuMain(int menu)
        {
            switch (menu)
            {
                case (int)EMenu.Main:
                case (int)ESubMenus.Aulas:
                case (int)ESubMenus.Materiais:
                case (int)ESubMenus.Questoes:
                case (int)ESubMenus.Revalida:
                case (int)ESubMenus.MainSub:
                case (int)ESubMenus.MaterialFake:
                case (int)ESubMenus.Checklists:
                    return true;
                default:
                    return false;
            }
        }


    }
}