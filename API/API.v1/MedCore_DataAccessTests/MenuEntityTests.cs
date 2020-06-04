using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static MedCore_DataAccess.Repository.CronogramaEntity;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class MenuEntityTests
    {
        private List<EMenu> menuPadraoMsPro = new List<EMenu>()
        {
            EMenu.Main,
            EMenu.AreaTreinamento,
            EMenu.Medcode,
            EMenu.SlidesDeAula,
            EMenu.DuvidasAcademicas,
            EMenu.Contribuicoes,
            EMenu.Administrativo,
            EMenu.Academico
        
        };

        private List<ESubMenus> subMenusPadraoMsPro = new List<ESubMenus>()
        {
            ESubMenus.Aulas,
            ESubMenus.Materiais,
            ESubMenus.Questoes,
            ESubMenus.ConcursoNaIntegra,
            ESubMenus.Simulados,
            ESubMenus.MontaProva,
            ESubMenus.MainSub,
            ESubMenus.Materiais,
            ESubMenus.DuvidasQuestaoMedCode,
            ESubMenus.InserirContribuicao,
            ESubMenus.FeedContribuir,
            ESubMenus.Financeiro,
            ESubMenus.Cronograma,
            ESubMenus.Revalida,
            ESubMenus.MultimidiaCPMED,
            ESubMenus.NotasFiscais
            
        };
        private int aplicacaoRestrita = (int)Aplicacoes.AreaRestrita;
        [TestMethod]
        public void CanGetMenusPermitidos()
        {
            var idAplicacao = 17;
            var matricula = 212642; //171140;//204305;//215942;
            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula);
            Assert.IsNotNull(menu);
            Assert.AreNotEqual(0, menu.Count);
        }

        [Ignore]
        [TestMethod]
        public void CanGetMenusPermitidos_SomenteAula()
        {
            var idAplicacao = 1;
            var matricula = 204305;
            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula);

            var lstMenu = new List<Menu>();

            lstMenu = menu;

            var idMenus = new int[] { 3, 15, 16, 18, 19, 20, 21, 22, 24, 25, 26 };
            var i = 0;
            foreach (var item in lstMenu)
            {
                if (idMenus.Contains(item.Id)) i++;
                if (item.SubMenu != null)
                {
                    foreach (var item2 in item.SubMenu)
                    {
                        if (idMenus.Contains(item2.Id)) i++;
                    }
                }

            }
            var teste = false;
            if (i == idMenus.Count()) teste = true;

            Assert.IsTrue(teste);


            Assert.IsNotNull(menu);
            Assert.AreNotEqual(0, menu.Count);
        }

        [Ignore]
        [TestMethod]
        public void CanGetMenusPermitidos_AcessoEspecialMontaProva()
        {
            var idAplicacao = 17;
            var matricula = 227144;
            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula);

            var lstMenu = new List<Menu>();

            lstMenu = menu;

            var teste = lstMenu.FirstOrDefault(x => x.Id == 85).SubMenu.Any(x => x.Id == 99);

            Assert.IsTrue(teste);

        }

        [Ignore]
        [TestMethod]
        public void CanGetMenusPermitidos_AcessoEspecialMontaProva_NaoIncluso()
        {
            var idAplicacao = 17;
            var matricula = 119300;
            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula);

            var lstMenu = new List<Menu>();

            lstMenu = menu;

            var teste = lstMenu.FirstOrDefault(x => x.Id == 85).SubMenu.Any(x => x.Id == 99);

            Assert.IsTrue(teste);

        }

        [Ignore] //matricula que atenda o cenário ainda não encontrada
        [TestMethod]
        [TestCategory("Menus_Permitidos")]
        public void CanGetMenuPermitidos_Mspro_AlunoMedeletro()
        {
            var matricula = 0;

            var subMenusPermitidos = subMenusPadraoMsPro;

            subMenusPermitidos.Remove(ESubMenus.Aulas);
            subMenusPermitidos.Remove(ESubMenus.Questoes);

            AssertsMenusPermitidos(matricula, menuPadraoMsPro, subMenusPermitidos);
        }


        private void AssertsMenusPermitidos(int matricula, List<EMenu> menusPermitidos, List<ESubMenus> subMenusPermitidos, int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            var ultimaVersao = new VersaoAppPermissaoEntity().GetUltimaVersao((Aplicacoes)idAplicacao);
            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula, versao: ultimaVersao);

            var possuiMenuNaoPermitido = menu.Any(x => !menusPermitidos.Contains((EMenu)x.Id));

            Assert.IsFalse(possuiMenuNaoPermitido);

            foreach (var m in menu)
            {
                var possuiSubMenuNaoPermitido = m.SubMenu.Any(x => !subMenusPermitidos.Contains((ESubMenus)x.Id));

                Assert.IsFalse(possuiSubMenuNaoPermitido);
            }
        }

        [TestMethod]
        [TestCategory("Menus_Permitidos")]
        public void GetPermitidos_AlunoMedMasterCanceladoAntesDataLimite_VisualizaConteudoVitalicio()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var idCurso = (int)Produto.Cursos.MED;
            var anoMatricula = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualCanceladoSemAnoAnterior();
            var versaoApp = "5.3.0";

            if (matricula == 0)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula, 0, idCurso, versaoApp);

            Assert.AreNotEqual(0, menu.Count);
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Main));
            Assert.IsFalse(menu.Any(x => x.Id == (int)EMenu.AreaTreinamento));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Academico));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Administrativo));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Medcode));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.SlidesDeAula));

            var resMain = menu.Where(a => a.Id == (int)EMenu.Main).FirstOrDefault().SubMenu;
            Assert.IsFalse(resMain.Any(x => x.Id == (int)ESubMenus.Aulas));
            Assert.IsTrue(resMain.Any(x => x.Id == (int)ESubMenus.Materiais));
            Assert.IsFalse(resMain.Any(x => x.Id == (int)ESubMenus.Questoes));
            
            var resCronograma = menu.Where(a => a.Id == (int)EMenu.Academico).FirstOrDefault().SubMenu;
            Assert.IsTrue(resCronograma.Any(x => x.Id == (int)ESubMenus.Cronograma));

            var resFinanceiro = menu.Where(a => a.Id == (int)EMenu.Administrativo).FirstOrDefault().SubMenu;
            Assert.IsTrue(resFinanceiro.Any(x => x.Id == (int)ESubMenus.Financeiro));
        }

        [TestMethod]
        [TestCategory("Menus_Permitidos")]
        public void GetPermitidos_AlunoMedMasterInadimplente_VisualizaConteudoExtensivo()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var idCurso = (int)Produto.Cursos.MED;
            var anoMatricula = Utilidades.GetYear();
            var alunos = new PerfilAlunoEntityTestData().GetAlunosMedMasterInadimplenteVisualizouTermo();

            if (alunos.Count() == 0)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var matricula = alunos.FirstOrDefault().ID;
            var versaoApp = "5.3.0";

            var menu = new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(idAplicacao, matricula, 0, idCurso, versaoApp);

            Assert.AreNotEqual(0, menu.Count);
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Main));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.AreaTreinamento));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Academico));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Administrativo));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.Medcode));
            Assert.IsTrue(menu.Any(x => x.Id == (int)EMenu.SlidesDeAula));

            var resMain = menu.Where(a => a.Id == (int)EMenu.Main).FirstOrDefault().SubMenu;
            Assert.IsTrue(resMain.Any(x => x.Id == (int)ESubMenus.Aulas));
            Assert.IsTrue(resMain.Any(x => x.Id == (int)ESubMenus.Materiais));
            Assert.IsTrue(resMain.Any(x => x.Id == (int)ESubMenus.Questoes));

            var resAreaTreinamento = menu.Where(a => a.Id == (int)EMenu.AreaTreinamento).FirstOrDefault().SubMenu;
            Assert.IsTrue(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.MontaProva));
            Assert.IsTrue(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.ConcursoNaIntegra));
            Assert.IsTrue(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.Simulados));
            Assert.IsFalse(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.SimuladoR3Cirurgia));
            Assert.IsFalse(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.SimuladoR3Clinica));
            Assert.IsFalse(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.SimuladoR3Pediatria));
            Assert.IsFalse(resAreaTreinamento.Any(x => x.Id == (int)ESubMenus.SimuladoR4GO));

            var resCronograma = menu.Where(a => a.Id == (int)EMenu.Academico).FirstOrDefault().SubMenu;
            Assert.IsTrue(resCronograma.Any(x => x.Id == (int)ESubMenus.Cronograma));

            var resFinanceiro = menu.Where(a => a.Id == (int)EMenu.Administrativo).FirstOrDefault().SubMenu;
            Assert.IsTrue(resFinanceiro.Any(x => x.Id == (int)ESubMenus.Financeiro));
        }
    }
}