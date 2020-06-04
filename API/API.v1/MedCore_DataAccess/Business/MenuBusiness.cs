using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using Medgrupo.DataAccessEntity;
using static MedCore_DataAccess.Repository.CronogramaEntity;

namespace MedCore_DataAccess.Business
{
    public class MenuBusiness : IMenuBusiness
    {
        private readonly IMenuData _menuRepository;
        private readonly IDataAccess<Pessoa> _pessoaRepository;
        private readonly IBlackListData _blackListRepository;

        public MenuBusiness(IMenuData menuRepository, IDataAccess<Pessoa> pessoaRepository, IBlackListData blackListRepository)
        {
            _menuRepository = menuRepository;
            _pessoaRepository = pessoaRepository;
            _blackListRepository = blackListRepository;
        }

        public List<Menu> GetAll(int idAplicacao, string versao = "")
        {
            return _menuRepository.GetAll(idAplicacao, versao);
        }

        public List<PermissaoRegra> GetAlunoPermissoesMenu(List<Menu> lstMenu, int idClient, int idAplicacao, DateTime? data = null, int idProduto = 0)
        {
            return _menuRepository.GetAlunoPermissoesMenu(lstMenu, idClient, idAplicacao, data, idProduto);
        }

        public List<AccessObjectPermissaoOffline> GetPermitidosOffline(int idAplicacao, int idClient, int idProduto = 0, string versao = "")
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPermitidosOffline))
                return RedisCacheManager.GetItemObject<List<AccessObjectPermissaoOffline>>(RedisCacheConstants.DadosFakes.KeyGetPermitidosOffline);

            Version version = Version.Parse(versao);

            var lstMenu = new List<Menu>();

            lstMenu = GetAll(idAplicacao, versao);

            var lstPermissoes = GetAlunoPermissoesMenu(lstMenu, idClient, idAplicacao, null);

            var lstPermissoesVisualizacao = lstPermissoes
                .Where(x => x.AcessoId != (int)(Utilidades.PermissaoStatus.SemAcesso)).ToList();

            var menusOffline = new List<AccessObjectPermissaoOffline>();

            foreach (var y in lstMenu)
            {
                var menuPermissao = lstPermissoesVisualizacao.Find(p => p.ObjetoId == y.Id);

                if (menuPermissao == null) continue;
                bool bPermiteOffline =
                    ((y.PermiteOffline == 1) && (Version.Parse(y.VersaoMinimaOffline) <= version));

                menusOffline.Add(new AccessObjectPermissaoOffline
                {
                    Id = y.Id,
                    VersaoMinimaOffline = y.VersaoMinimaOffline,
                    PermiteOffline = bPermiteOffline ? 1 : 0
                });
            }
            return menusOffline;
        }

        public List<Menu> GetPermitidos(int idAplicacao, int idClient, int conteudoCompleto = 0, int idProduto = 0, string versao = "")
        {
            var permitePesquisaItens = new List<int> {  (int)EPesquisaConteudoMenu.PesquisaAula, 
                                                        (int)EPesquisaConteudoMenu.PesquisaMaterial, 
                                                        (int)EPesquisaConteudoMenu.PesquisaQuestoes };
            
            versao = string.IsNullOrEmpty(versao) ? "0.0.0" : versao;

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPermitidos))
            {
                var listaMenu = RedisCacheManager.GetItemObject<List<Menu>>(RedisCacheConstants.DadosFakes.KeyGetPermitidos + "-" + idClient);

                if (listaMenu != null)
                {
                    listaMenu = RetornarMenuCronograma(versao, idAplicacao, listaMenu);
                    return listaMenu;
                }
               
            }


            var lstMenu = _menuRepository.GetAll(idAplicacao, versao);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(lstMenu);

            var lstPermissoes = _menuRepository.GetAlunoPermissoesMenu(lstMenu, idClient, idAplicacao, null, (int)Extend.GetProductByCourse((Produto.Cursos)idProduto));

            var lstPermissoesVisualizacao = lstPermissoes.Where(x => x.AcessoId != (int)(Utilidades.PermissaoStatus.SemAcesso)).ToList();

            var menuPermitido = new List<Menu>();

            foreach (var objeto in lstMenu) 
            {

                var menuPermissao = lstPermissoesVisualizacao.Find(p => p.ObjetoId == objeto.Id);

                if (menuPermissao != null) 
                {
                    objeto.IdMensagem = menuPermissao.MensagemId.Value;
                    if (menuPermissao != null && menuPermissao.AcessoId == (int)Utilidades.PermissaoStatus.Ler)
                        objeto.Url = string.IsNullOrEmpty(objeto.Url.Trim()) ? string.Empty : "#bloqueado";

                    menuPermitido.Add(objeto);
                }
            }

            var retorno = GetMenuFormatado(menuPermitido);

            var Pessoa = _pessoaRepository.GetByFilters(new Pessoa() { ID = idClient }).FirstOrDefault();

            var PessoaBlackList = _blackListRepository.GetAll().Where(x => x.ID == Pessoa.ID).FirstOrDefault();

            if (PessoaBlackList != null && PessoaBlackList.Bloqueios.Any(x => x.AplicacaoId == (int)Aplicacoes.LeitordeApostilas))
                RemoverSubMenu(retorno, (int)ESubMenus.Materiais); 
                
            if (idProduto != 0)
            {
                List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO , (int)Produto.Cursos.TEGO , (int)Produto.Cursos.MASTO};

                var idsPermitidos = _menuRepository.ObterMenusPermitidoParaProduto(idProduto);

                
                
                if (idProdutoAulao.Contains(idProduto) && (
                  ((idAplicacao == (int)Aplicacoes.MsProMobile && Utilidades.VersaoMenorOuIgual(versao,
                         ConfigurationProvider.Get("Settings:VersaoAppTrocaLayoutAuloesSR3R4")))
                         || !Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivaAuloesSR3R4"))
                  )
                  || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON))

                {
                    idsPermitidos.Remove((int)ESubMenus.Aulas);
                }
                

                    var idsProdutosEspeciais = new List<int> { Produto.Cursos.MED_AULAS_ESPECIAIS.GetHashCode(), Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS.GetHashCode() };
                if (idsProdutosEspeciais.Contains(idProduto))
                    idsPermitidos.Add(ESubMenus.Aulas.GetHashCode());

                foreach (var x in retorno)
                {
                    x.SubMenu = (from a in x.SubMenu
                                 join b in idsPermitidos on a.Id equals b
                                 select a).ToList();

                    x.SubMenu.ForEach(y => y.PermitePesquisa = y.SubMenu.Any(z => permitePesquisaItens.Contains(z.Id) 
                                                                                  && z.IdPai == y.Id 
                                                                                  && idsPermitidos.Contains(z.Id)));

                }
            }
            retorno = RetornarMenuCronograma(versao, idAplicacao, retorno);

            return retorno;
            
        }
        private List<Menu> RetornarMenuCronograma(string versao, int idAplicacao, List<Menu> resultado)
        {
            if (versao == "4.4.0" && idAplicacao == (int)Aplicacoes.MsProMobile)
            {
                var res = resultado.Where(a => a.Id == (int)EMenu.Academico).Select(c => c.SubMenu).ToList();
                List<Menu> res1 = new List<Menu>();
                if (res.Count > 0)
                    res1 = res.FirstOrDefault().Where(a => a.Id == (int)ESubMenus.Cronograma).ToList();
                if (res1.Count > 0)
                    res1.FirstOrDefault().SubMenu = new List<Menu>();

            }
            return resultado;
        }
        private void RemoverSubMenu(List<Menu> retorno, int idSubMenu)
        {
            var xMenusQueContemMedReader = retorno.FindAll(x => x.SubMenu.Any(y => y.Id == idSubMenu));

            foreach (var menuContemMaterial in xMenusQueContemMedReader)
            {
                menuContemMaterial.SubMenu.RemoveAll(x => x.Id == idSubMenu);
            }
        }

        public List<Menu> GetMenuFormatado(List<Menu> menu)
        {
            var lstMenu = new List<Menu>();
            var pais = menu.Where(x => x.IdPai == -1);
            foreach (var item in pais)
            {
                if ((string.IsNullOrEmpty(item.Url) || item.Url == null) || item.IdAplicacao == (int)Aplicacoes.MsProMobile || item.IdAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON) //é pai
                {
                    var objPaiComTodosFilhos = PreencherFilhos(item, menu);
                    lstMenu.Add(objPaiComTodosFilhos);
                }
                else
                    lstMenu.Add(item);
            }
            return lstMenu;
        }

        public Menu PreencherFilhos(Menu objMenuPai, List<Menu> lstMenuBase)
        {
            var lstTodosOsFilhosDoObjMenu = lstMenuBase.Where(x => x.IdPai == objMenuPai.Id).OrderBy(m => m.Ordem).ToList();
            objMenuPai.SubMenu = new List<Menu>();

            foreach (var item in lstTodosOsFilhosDoObjMenu)
            {
                var xPaiComFIlho = PreencherFilhos(item, lstMenuBase);
                objMenuPai.SubMenu.Add(xPaiComFIlho);
            }
            return objMenuPai;
        }
    }
}