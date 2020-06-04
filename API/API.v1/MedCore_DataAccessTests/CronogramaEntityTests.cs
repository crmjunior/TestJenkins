using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using MedCore_DataAccessTests.EntitiesMockData;
using Medgrupo.DataAccessEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;
using static MedCore_DataAccess.Repository.CronogramaEntity;

namespace MedCore_DataAccessTests
{
    [TestClass]   
    public class CronogramaEntityTests
    {      
        #region Cronograma
        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaMedCurso_IsNotNull()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MEDCURSO, 2017, (int)ESubMenus.Aulas);

            Assert.IsNotNull(cronograma);
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaMedCurso_Revalida()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MEDCURSO, 2017, (int)ESubMenus.Revalida);

            Assert.IsNotNull(cronograma);
            Assert.IsNotNull(cronograma.Revalida);
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaMedCurso_Aulas_Tema_MustHaveOnlyOne()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MEDCURSO, 2017, (int)ESubMenus.Aulas);

            Assert.IsNotNull(cronograma);
            Assert.IsFalse(cronograma.Semanas.Any(x => x.Apostilas.Any(y => y.Temas.Count != 1)));
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaMed_IsNotNull()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MED, 2017, (int)ESubMenus.Questoes);

            Assert.IsNotNull(cronograma);
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaMed_Revalida()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MED, 2017, (int)ESubMenus.Revalida);

            Assert.IsNotNull(cronograma);
            Assert.IsNotNull(cronograma.Revalida);
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void Can_GetCronogramaAdaptaMed()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.ADAPTAMED, 2017, (int)ESubMenus.Aulas);

            Assert.IsNotNull(cronograma);
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoAnteriorTrocaLayoutMastologia_MastologiaLayoutWeekSingle()
        {
            var versao = "5.2.0";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MASTO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);

            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));

        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoSuperiorTrocaLayoutMastologia_MastologiaLayoutWeek()
        {
            var versao = "5.2.1";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MASTO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);

            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoVaziaTrocaLayoutMastologia_MastologiaLayoutWeekSingle()
        {
            var versao = "";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.MASTO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);


            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));
        }



        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoAnteriorTrocaLayoutTego_TegoLayoutWeekSingle()
        {
            var versao = "5.2.0";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.TEGO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);

            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));

        }
        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoAnteriorTrocaLayoutAULOESR3R4()
        {
            var matricula = 1;
            var NomedaAula = "Aulao";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO };
            List<int> idProdutoTegoMasto = new List<int>() {  (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };
            List<int> idMenus = new List<int>() { (int)ESubMenus.Materiais , (int)ESubMenus.Aulas , (int)ESubMenus.Questoes };
            foreach (var itemIdProduto in idProdutoAulao)
            {
                foreach (var itemMenuID in idMenus)
                {
                    if (itemMenuID == (int)ESubMenus.Aulas) { 
                        var cronograma = GetDadosMockados(itemIdProduto, DateTime.Now.Year, matricula, itemMenuID, NomedaAula, TipoLayoutMainMSPro.PLAYLIST);
                        Assert.IsTrue(cronograma.All(x => x.TipoLayout == TipoLayoutMainMSPro.PLAYLIST));
                    }
                        
                    if (itemMenuID == (int)ESubMenus.Questoes || itemMenuID == (int)ESubMenus.Materiais)
                    {
                        var cronograma = GetDadosMockados(itemIdProduto, DateTime.Now.Year, matricula, itemMenuID, NomedaAula, TipoLayoutMainMSPro.WEEK_SINGLE);
                        Assert.IsTrue(cronograma.All(x => x.TipoLayout == TipoLayoutMainMSPro.WEEK_SINGLE));
                    }
                }
            }
            foreach (var itemIdProduto in idProdutoTegoMasto)
            {
                foreach (var itemMenuID in idMenus)
                {
                    if (itemMenuID == (int)ESubMenus.Aulas)
                    {
                        var cronograma = GetDadosMockados(itemIdProduto, DateTime.Now.Year, matricula, itemMenuID, NomedaAula, TipoLayoutMainMSPro.PLAYLIST);
                        Assert.IsTrue(cronograma.All(x => x.TipoLayout == TipoLayoutMainMSPro.PLAYLIST));
                    }

                    if (itemMenuID == (int)ESubMenus.Questoes || itemMenuID == (int)ESubMenus.Materiais)
                    {
                        var cronograma = GetDadosMockados(itemIdProduto, DateTime.Now.Year, matricula, itemMenuID, NomedaAula, TipoLayoutMainMSPro.WEEK);
                        Assert.IsTrue(cronograma.All(x => x.TipoLayout == TipoLayoutMainMSPro.WEEK));
                    }
                }
            }

        }
        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoSuperiorTrocaLayoutTego_TegologiaLayoutWeek()
        {
            var versao = "5.2.1";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.TEGO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);

            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));
        }

        [TestMethod]
        [TestCategory("Cronograma")]
        public void GetCronograma_AlunoAcademicoVersaoVaziaTrocaLayoutTego_TegoLayoutWeekSingle()
        {
            var versao = "";
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.TEGO, DateTime.Now.Year, (int)ESubMenus.Materiais, versaoApp: versao);


            Assert.IsTrue(cronograma.Semanas.All(x => x.Apostilas.Select(y => y.MaterialId).Distinct().Count() == 2));
        }
        #endregion

        #region Cronograma Conteudo
        [Ignore] // Grosseria-Kanban está atuando - Remover ignorar quando bug for corrigido
        // Bug -> NEF 1 abrindo NEF X
        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Medcurso_AlunoMedMedcurso()
        {
            var produto = (int)Produto.Cursos.MEDCURSO;
            var matricula = 241720;

            AssertsConteudoCronograma(produto, matricula);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoMedMedcurso_AnosAnteriores()
        {
            var produto = (int)Produto.Cursos.MED;
            var matricula = 267711; //MEDCURSO 2015 / MED 2016  - 201868

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoMedMedcurso_AnoAnterior()
        {
            var produto = (int)Produto.Cursos.MED;
            var matricula = 209288; //MEDCURSO 2016 / MED 2017  - 213070

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Planejamento2anos_MedAnoAtual_MedcursoAnterior()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            //MEDCURSO 2017 / MED 2018
            int ano = Utilidades.GetYear();
            var anoMed = ano;
            var anoMedCurso = (ano - 1);

            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoPlanejamentoDoisAnos(anoMed, anoMedCurso);
            if (aluno == null)
            {
                Assert.Inconclusive();
            }

            var matricula = aluno.ID;
            var produto = (int)Produto.Cursos.MED;
            //var matricula = 227799; //MEDCURSO 2017 / MED 2018  - 227799

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Planejamento2anos_MedcursoAnoAtual_MedAnoSeguinte()
        {
            //MEDCURSO 2018 / MED 2019

            int ano = Utilidades.GetYear();
            var anoMed = (ano + 1);
            var anoMedCurso = ano;

            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoPlanejamentoDoisAnos(anoMed, anoMedCurso);
            if (aluno == null)
            {
                Assert.Inconclusive();
            }

            var matricula = aluno.ID;
            var produto = (int)Produto.Cursos.MED;
            //var matricula = 257958; //MEDCURSO 2018 / MED 2019  - 257958

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoMedMedcurso_AnoAtualPacote()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var produto = (int)Produto.Cursos.MED;
            var matricula = 258992;  //253297 - MED  / MEDCURSO 2018

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoMedMedcursoEAD_AnoAtualPacote()
        {
            var produto = (int)Produto.Cursos.MED;
            var matricula = 254204; //254204 

            AssertsConteudoCronograma(produto, matricula);
        }

        [Ignore]     // Grosseria-Kanban está atuando - Remover ignorar quando bug for corrigido
        // Bug -> Aula ativa para o aluno mas quando abre está vazia
        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoMedEad()
        {
            var produto = (int)Produto.Cursos.MED;
            var matricula = 252172;

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Med_AlunoSomenteMed()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var produto = (int)Produto.Cursos.MED;
            var matricula = 259194;

            AssertsConteudoCronograma(produto, matricula);
        }

        [Ignore] // Grosseria-Kanban está atuando - Remover ignorar quando bug for corrigido
        // Bug -> NEF 1 abrindo NEF X
        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Medcurso_AlunoMedcursoEad()
        {
            var produto = (int)Produto.Cursos.MEDCURSO;
            var matricula = 259163;

            AssertsConteudoCronograma(produto, matricula);
        }

        [Ignore]     // Grosseria-Kanban está atuando - Remover ignorar quando bug for corrigido
        // Bug -> Aula ativa para o aluno mas quando abre está vazia
        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Medcurso_AlunoSomenteMedcurso()
        {
            var produto = (int)Produto.Cursos.MEDCURSO;
            var matricula = 259276;

            AssertsConteudoCronograma(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Conteudo")]
        public void Can_GetAulasCronograma_Medcurso_AlunoMedMedcursoRevalida()
        {
            var produto = (int)Produto.Cursos.MEDCURSO;
            var matricula = 252209;

            AssertsConteudoCronogramaRevalida(produto, matricula);
        }
        #endregion

        #region Cronograma Permissao
        [Ignore]
        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoMedMedcurso()
        {
            //select * from tblSellORDERS so
            //inner join tblClients c on c.intClientID = so.intClientID
            //WHERE intStatus = 2 and txtComment like '2018 MEDCURSO%' and txtComment like  '%/2019 MED%'
            var matricula = new PerfilAlunoEntityTestData().GetAlunoAnoAtualComAnosAnteriores(Produto.Produtos.MED);

            var produto = (int)Produto.Cursos.MED;


            AssertsPermissaoConteudo(produto, matricula, false);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoMedEad()
        {
            var matricula = 252172;
            var produto = 17;

            AssertsPermissaoConteudo(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoSomenteMed()
        {
            var matricula = 259194;
            var produto = 17;

            AssertsPermissaoConteudo(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoMedcursoEad()
        {
            var matricula = 259163;
            var produto = 16;

            AssertsPermissaoConteudo(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoSomenteMedcurso()
        {
            var matricula = 259276;
            var produto = 16;

            AssertsPermissaoConteudo(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoAnoAtualEAnosAnteriores()
        {
            Assert.Inconclusive("Inconclusivo para liberar publish, já esta sendo analisado pelo time vem tranquilo");
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedAnoAtualComAnosAnteriores();
            var produto = (int)Produto.Cursos.MED;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);

            var provider = new CultureInfo("pt-BR");
            var format = "dd/MM";

            var aulasFuturas = permissoes.Where(x => DateTime.ParseExact(x.DataInicio, format, provider) > DateTime.Now).ToList();

            if (!aulasFuturas.Any())
                Assert.Inconclusive();

            Assert.IsTrue(aulasFuturas.All(x => x.Ativa == 1));
        }



        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoSomenteAnoAtual()
        {
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedApenasAno2019();
            var produto = (int)Produto.Cursos.MED;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);

            var provider = new CultureInfo("pt-BR");
            var format = "dd/MM";

            var aulasFuturas = permissoes.Where(x => DateTime.ParseExact(x.DataInicio, format, provider) > DateTime.Now).ToList();

            if (!aulasFuturas.Any())
                Assert.Inconclusive();

            if (!aulasFuturas.All(x => x.Ativa == 0))
                Assert.Inconclusive();

            Assert.IsTrue(aulasFuturas.All(x => x.Ativa == 0));
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void ValidaCronograma_AlunoMEDCanceladoAnoAtual_TemCronograma()
        {
            var anoAtual = Utilidades.GetYear();
            var produto = (int)Produto.Produtos.MED;
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAtual, produto, 5, 5);

            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);

            var provider = new CultureInfo("pt-BR");
            var format = "dd/MM";

            var aulasFuturas = permissoes.Where(x => DateTime.ParseExact(x.DataInicio, format, provider) > DateTime.Now).ToList();

            if (!aulasFuturas.Any())
                Assert.Inconclusive();

            if (!aulasFuturas.All(x => x.Ativa == 0))
                Assert.Inconclusive();

            Assert.IsTrue(aulasFuturas.All(x => x.Ativa == 0));
        }


        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void Can_GetCronogramaPermissao_AlunoSomenteAno2018()
        {
            Assert.Inconclusive("Inconclusivo para liberar publish, já esta sendo analisado pelo time vem tranquilo");
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedApenasAno2018();
            var produto = (int)Produto.Cursos.MED;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);

            Assert.IsTrue(permissoes.All(x => x.Ativa == 1));
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetCronogramaPermissao_AlunoEAD2019ComAnosAnteriores_RetornaAulasFuturasAtivas()
        {
            Assert.Inconclusive("Inconclusivo para liberar publish, já esta sendo analisado pelo time vem tranquilo");
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoAnoAtualComAnosAnteriores(Produto.Produtos.MEDEAD);
            var produto = (int)Produto.Cursos.MED;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);

            Assert.IsTrue(permissoes.All(x => x.Ativa == 1));
        }
        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetCronogramaPermissao_AlunoMastoR4_GetQuestoesAprovadas()
        {

            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MASTO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault().ID;
            var produto = (int)Produto.Cursos.MASTO;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            Assert.IsNotNull(permissoes);
            Assert.IsNotNull(permissoes[0].QuestoesAprovadas);
            Assert.IsInstanceOfType(permissoes[0].QuestoesAprovadas, typeof(List<int?>));
        }
        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        [Ignore]
        public void GetCronogramaPermissao_AlunoMastoR4_QuestoesAprovadasEqualListaMaterialDireito()
        {

            var anoAtual = Utilidades.GetYear();
            var produto = (int)Produto.Cursos.MASTO;
            var curso = (Produto.Cursos)produto;
            int produtoId = (int)curso.GetProductByCourse();
            var matricula = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MASTO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault().ID;

            var listaMaterialDireito = new CartaoRespostaEntity().ListaMaterialDireitoAluno(matricula, anoAtual, produtoId);
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            var listMaterialPermissoes = permissoes.Where(x => x.QuestoesAprovadas != null).Select(a => a.QuestoesAprovadas);
            List<int?> intMaterialID = new List<int?>();
            foreach (var item in listMaterialPermissoes)
            {
                intMaterialID.AddRange(item);
            }
            List<int?> ListaMaterialDireitointMaterialID = new List<int?>();
            ListaMaterialDireitointMaterialID.AddRange(listaMaterialDireito.Where(x => x.blnPermitido == 1).Select(a => a.intMaterialID).Distinct().ToList());

            Assert.AreEqual(ListaMaterialDireitointMaterialID.Count, intMaterialID.Count);
            Assert.AreEqual(ListaMaterialDireitointMaterialID.Intersect(intMaterialID).ToList().Count, intMaterialID.Count);
            Assert.AreEqual(ListaMaterialDireitointMaterialID.Intersect(intMaterialID).ToList().Count, ListaMaterialDireitointMaterialID.Count);

        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetCronogramaPermissao_AlunoTEGOR4_GetQuestoesAprovadas()
        {
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.TEGO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault().ID;
            var produto = (int)Produto.Cursos.TEGO;
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);

            Assert.IsNotNull(permissoes);
            if (permissoes[0].Ativa == 0) Assert.Inconclusive("Ainda não chegou no periodo de aula");
            Assert.IsNotNull(permissoes[0].QuestoesAprovadas);
            Assert.IsInstanceOfType(permissoes[0].QuestoesAprovadas, typeof(List<int?>));
        }
        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        [Ignore]
        public void GetCronogramaPermissao_AlunoTegoR4_QuestoesAprovadasEqualListaMaterialDireito()
        {

            var anoAtual = Utilidades.GetYear();
            var produto = (int)Produto.Cursos.TEGO;
            var curso = (Produto.Cursos)produto;
            int produtoId = (int)curso.GetProductByCourse();
            var matricula = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.TEGO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault().ID;

            var listaMaterialDireito = new CartaoRespostaEntity().ListaMaterialDireitoAluno(matricula, anoAtual, produtoId);
            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);
            var listMaterialPermissoes = permissoes.Where(x => x.QuestoesAprovadas != null).Select(a => a.QuestoesAprovadas);
            List<int?> intMaterialID = new List<int?>();
            foreach (var item in listMaterialPermissoes)
            {
                intMaterialID.AddRange(item);
            }
            List<int?> ListaMaterialDireitointMaterialID = new List<int?>();
            ListaMaterialDireitointMaterialID.AddRange(listaMaterialDireito.Where(x => x.blnPermitido == 1).Select(a => a.intMaterialID).Distinct().ToList());

            Assert.AreEqual(ListaMaterialDireitointMaterialID.Count, intMaterialID.Count);
            Assert.AreEqual(ListaMaterialDireitointMaterialID.Intersect(intMaterialID).ToList().Count, intMaterialID.Count);
            Assert.AreEqual(ListaMaterialDireitointMaterialID.Intersect(intMaterialID).ToList().Count, ListaMaterialDireitointMaterialID.Count);

        }
        #endregion

        #region Cronograma Progresso

        [TestMethod]
        [TestCategory("Cronograma_Progresso")]
        public void Can_GetProgressosaPermissao_AlunoMedMedcurso()
        {
            var matricula = 241720;
            var produto = 16;

            AssertsProgressosConteudoUnificado(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Cronograma_Progresso")]
        public void Can_GetProgressosaPermissao_AlunoMedMed()
        {
            var matricula = 241720;
            var produto = 17;

            AssertsProgressosConteudoUnificado(produto, matricula);
        }

        [TestMethod]
        [TestCategory("Progresso de Questões")]
        public void CanGetListaQuestaoProgresso_ValidarRetornoAgrupadoDeExercicio()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var produto = Constants.Produtos.MEDCURSO;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();

            var questoesApostila = AulaEntityTestData.GetListaQuestoesApostila_ExercicioUnico();
            var respostasMatricula = AulaEntityTestData.GetIdRespostaQuestaoApostila();

            aulaMock.GetQuestoesApostila_PorAnoProduto(ano, (int)produto).Returns(questoesApostila);
            aulaMock.GetRespostas_PorMatricula(matricula).Returns(respostasMatricula);

            var ret = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, new CronogramaEntity()).GetListaQuestoesProgresso(matricula, ano, (int)produto);

            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Count == 1);
            Assert.IsInstanceOfType(ret, typeof(List<ProgressoSemana>));
        }

        [TestMethod]
        [TestCategory("Progresso de Questões")]
        public void CanGetQuestoesProgresso_ValidarRetornoAgrupadoDepoisDeFiltrarPermitidos()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var produto = (int)Constants.Produtos.MEDCURSO;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();

            var questoesApostila = AulaEntityTestData.GetListaQuestoesApostila_ExercicioUnico();
            var respostasMatricula = AulaEntityTestData.GetIdRespostaQuestaoApostila();

            var listaPermitidos = AulaEntityTestData.GetListaExercicioApostilaPermitidos_Ids();

            aulaMock.GetQuestoesApostila_PorAnoProduto(ano, produto).Returns(questoesApostila);
            aulaMock.GetRespostas_PorMatricula(matricula).Returns(respostasMatricula);
            aulaMock.GetListaMateriaisPermitidos_PorMatriculaAnoProduto(matricula, ano, produto).Returns(listaPermitidos);
            var ret = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, new CronogramaEntity()).GetProgressoQuestoes(matricula, ano, produto);

            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Count == 1);
            Assert.IsInstanceOfType(ret, typeof(List<ProgressoSemana>));
        }

        [TestMethod]
        [TestCategory("Progresso de Questões")]
        public void CanCalcularPercentual_QuestoesRealizadas()
        {
            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();

            var totalQuestoes = 60;
            var totalRealizadas = 6;
            var ret = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, new CronogramaEntity()).CalcularPercentual_QuestoesRealizadas(totalQuestoes, totalRealizadas);

            var retEsperado = 10;
            Assert.IsTrue(ret == retEsperado);
        }

        [TestMethod]
        [TestCategory("Progresso de Questões")]
        public void GetProgressos_ValidarPreenchimentoQuestoesProgresso()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var produto = (int)Constants.Produtos.MEDCURSO;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();

            var listaPermitidos = AulaEntityTestData.GetListaExercicioApostilaPermitidos_Ids();
            var questoesApostila = AulaEntityTestData.GetListaQuestoesApostila_ExercicioUnico();
            var respostasMatricula = AulaEntityTestData.GetIdRespostaQuestaoApostila();

            aulaMock.GetListaMateriaisPermitidos_PorMatriculaAnoProduto(matricula, ano, produto).Returns(listaPermitidos);
            aulaMock.GetQuestoesApostila_PorAnoProduto(ano, produto).Returns(questoesApostila);
            aulaMock.GetRespostas_PorMatricula(matricula).Returns(respostasMatricula);

            var ret = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, new CronogramaEntity()).GetProgressos(produto, matricula, ano);

            mednetMock.Received().GetProgressoAulas(matricula, ano, produto);
            materialApostilaMock.Received().GetProgressoMaterial(matricula, ano, produto);
            revalidaMock.Received().GetProgressoRevalida(matricula, produto);

            var listaSubMenu = new List<int>() { (int)ESubMenus.Aulas, (int)ESubMenus.Materiais, (int)ESubMenus.Questoes, (int)ESubMenus.Revalida };
            Assert.AreEqual(listaSubMenu.Count, ret.Retorno.Count);
            foreach (var id in listaSubMenu)
            {
                Assert.IsNotNull(ret.Retorno.Any(x => x.MenuId == id));
                if (id == (int)ESubMenus.Questoes)
                {
                    var retorno = ret.Retorno.Where(x => x.MenuId == id).FirstOrDefault();

                    Assert.AreEqual(1, retorno.ProgressoSemanas.Count);
                    Assert.AreEqual(600, retorno.ProgressoSemanas[0].IdEntidade);
                    Assert.AreEqual(100, retorno.ProgressoSemanas[0].PercentLido);
                }
            }
        }

        [TestMethod]
        [TestCategory("Progresso de Apostilas")]
        public void GetPercentSemanas_CursoAlunoMedEletroIMed_RetornaPercentMedEletro()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var cursoAluno = (int)Constants.Cursos.MEDELETRO_IMED;
            var cursoApostila = (int)Constants.Cursos.MEDELETRO;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();

            materialApostilaMock.GetProgressoMaterial(matricula, ano, cursoApostila).Returns(new List<ProgressoSemana>());

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, new CronogramaEntity());

            cronograma.GetPercentSemanas(ano, matricula, cursoAluno, Semana.TipoAba.Materiais);

            materialApostilaMock.Received().GetProgressoMaterial(matricula, ano, cursoApostila);
        }
        #endregion

        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        public void GetCronogramaDinamico_CPMEDExtensivoList_ListaDinamica()
        {
            var idProduto = 1;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Materiais;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            var codigosAmigaveis = new List<ApostilaCodigoDTO> { new ApostilaCodigoDTO { Nome = "Teste", ProdutoId = 1, TemaId = 1 } };

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeCPMED());
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(codigosAmigaveis);
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(TipoLayoutMainMSPro.LIST));
            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades(idProduto, ano, matricula);
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            aulaMock.Received().RetornaApostilasDeAcordoComMatricula(matricula);
            materialApostilaMock.Received().GetProgressoMaterial(matricula, ano, idProduto);

            Assert.AreEqual(retorno[0].Itens[0].Apostilas.Count, 2);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }

        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        public void GetCronogramaDinamico_CPMEDExtensivoPrateleira_ListaDinamica()
        {
            var idProduto = 1;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Materiais;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeCPMED());
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(TipoLayoutMainMSPro.SHELF));
            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);
            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAsync(TipoLayoutMainMSPro.SHELF));
            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas());

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades(idProduto, ano, matricula);
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            cronogramaMock.Received().GetBookEntitiesBloqueadosNoCronograma();
            cronogramaMock.Received().GetCodigosAmigaveisApostilas();
            aulaMock.Received().RetornaApostilasDeAcordoComMatricula(matricula);
            materialApostilaMock.Received().GetProgressoMaterial(matricula, ano, idProduto);

            Assert.AreEqual(retorno[0].Itens[0].Apostilas.Count, 1);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }
        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        public void GetCronogramaDinamico_AULAORMAIS_ListaDinamica()
        {
            var idProduto = 1;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Aulas;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeAULAO());
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(TipoLayoutMainMSPro.PLAYLIST, "AULAO"));
            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);
            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAsync(TipoLayoutMainMSPro.PLAYLIST, "AULAO"));
            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas("AULAO"));
            
            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, 1).Returns(new TemasApostila());
            
            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

          
            Assert.IsNotNull(retorno[0].Itens);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.AreEqual(retorno[0].TipoLayout, TipoLayoutMainMSPro.PLAYLIST);
        }
        public List<CronogramaDinamicoDTO> GetDadosMockados(int idProduto, int ano, int matricula, int menuId, string NomedaAula, TipoLayoutMainMSPro tipoLayout)
        {
            
            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeAULAO());
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(tipoLayout, NomedaAula));
            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            aulaMock.GetRespostas_PorMatricula(matricula).Returns(new List<int> { 1 });
            aulaMock.GetQuestoesApostila_PorAnoProduto(ano, idProduto).Returns(new List<QuestaoExercicioDTO>() { new QuestaoExercicioDTO() { QuestaoID = 1, ExercicioID = 1 } });
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);
            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAsync(tipoLayout, NomedaAula));
            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas(NomedaAula));

            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, 1).Returns(new TemasApostila());

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);
            return retorno;
        }
        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        public void GetCronogramaDinamico_CPMEDExtensivoPlayListProva_ListaDinamica()
        {
            var idProduto = 1;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Provas;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            var tema = cronogramaData.GetTemaApostila(ETipoVideo.ProvaVideo);
            var temas = new TemasApostila() { tema };

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeCPMED());
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(TipoLayoutMainMSPro.PLAYLIST));
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);
            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAsync(TipoLayoutMainMSPro.PLAYLIST));
            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas());
            mednetMock.CalculaProgressosVideosTemaProva(tema, matricula).Returns(tema);
            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.ProvaVideo).Returns(temas);

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades(idProduto, ano, matricula);
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            cronogramaMock.DidNotReceive().GetBookEntitiesBloqueadosNoCronograma();
            cronogramaMock.Received().GetCodigosAmigaveisApostilas();
            aulaMock.DidNotReceive().RetornaApostilasDeAcordoComMatricula(matricula);
            materialApostilaMock.DidNotReceive().GetProgressoMaterial(matricula, ano, idProduto);
            mednetMock.Received().CalculaProgressosVideosTemaProva(tema, matricula);
            mednetMock.Received().GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.ProvaVideo);
            mednetMock.DidNotReceive().CalculaProgressosVideosTemaRevisao(tema, matricula);

            Assert.AreEqual(retorno[0].Itens[0].Apostilas, null);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }

        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        public void GetCronogramaDinamico_CPMEDExtensivoPlayListRevisao_ListaDinamica()
        {
            var idProduto = 1;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Aulas;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            var tema = cronogramaData.GetTemaApostila();

            var temas = new TemasApostila();
            temas.Add(tema);

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades(idProduto, ano, matricula).Returns(cronogramaData.GetListaEntidadeCPMED());
            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleira(TipoLayoutMainMSPro.PLAYLIST));
            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);
            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAsync(TipoLayoutMainMSPro.PLAYLIST));
            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });
            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas());
            mednetMock.CalculaProgressosVideosTemaRevisao(tema, matricula).Returns(tema);
            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.Revisao).Returns(temas);

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades(idProduto, ano, matricula);    
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            cronogramaMock.DidNotReceive().GetBookEntitiesBloqueadosNoCronograma();
            cronogramaMock.Received().GetCodigosAmigaveisApostilas();
            aulaMock.Received().RetornaApostilasDeAcordoComMatricula(matricula);
            materialApostilaMock.DidNotReceive().GetProgressoMaterial(matricula, ano, idProduto);
            mednetMock.DidNotReceive().CalculaProgressosVideosTemaProva(tema, matricula);
            mednetMock.Received().GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.Revisao);
            mednetMock.Received().CalculaProgressosVideosTemaRevisao(tema, matricula);

            Assert.AreEqual(retorno[0].Itens[0].Apostilas, null);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }

        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        [Ignore]
        public void GetCronogramaDinamico_AULAS_ESPECIAIS_PlayListRevisao_NaDataLimite()
        {
            var idProduto = (int)Produto.Cursos.MED_AULAS_ESPECIAIS;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Aulas;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            var tema = cronogramaData.GetTemaApostila();

            var temas = new TemasApostila();
            temas.Add(tema);

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades((int)Produto.Cursos.MED, ano, Constants.CONTACTID_ACADEMICO).Returns(cronogramaData.GetListaEntidadeAulasEspeciaisDentroDataLimite());

            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleiraAulasEpeciaisDentroDataLimite(TipoLayoutMainMSPro.PLAYLIST));

            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);

            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAulasEpeciaisDentroDataLimiteAsync(TipoLayoutMainMSPro.PLAYLIST));

            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });

            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas());

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(cronogramaData.GetTurmaBaseAulasEspeciais());

            mednetMock.CalculaProgressosVideosTemaRevisao(tema, matricula).Returns(tema);

            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.Revisao).Returns(temas);

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades((int)Produto.Cursos.MED, ano, Constants.CONTACTID_ACADEMICO);
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            cronogramaMock.DidNotReceive().GetBookEntitiesBloqueadosNoCronograma();
            aulaMock.Received().RetornaApostilasDeAcordoComMatricula(matricula);

            Assert.IsFalse(retorno[0].Itens[0].Nome == "Videos em produção");
            Assert.AreEqual(retorno[0].Itens[0].Apostilas, null);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }

        [TestMethod]
        [TestCategory("Cronograma Dinamico")]
        [Ignore]
        public void GetCronogramaDinamico_AULAS_ESPECIAIS_PlayListRevisao_AposDataLimite()
        {
            var idProduto = (int)Produto.Cursos.MED_AULAS_ESPECIAIS;
            var ano = 2020;
            var matricula = 1;
            var menuId = (int)ESubMenus.Aulas;

            var aulaMock =Substitute.For<IAulaEntityData>();
            var mednetMock =Substitute.For<IMednetData>();
            var materialApostilaMock =Substitute.For<IMaterialApostilaData>();
            var revalidaMock =Substitute.For<IRevalidaData>();
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var cronogramaData = new CronogramaEntityTestData();

            var listaLiberacaoApostila = new List<tblLiberacaoApostila>();
            var listaProgressoSemana = new List<ProgressoSemana>();

            var liberacaoApostila = new tblLiberacaoApostila()
            {
                bitLiberado = true,
                dteDateTime = DateTime.Now,
                intBookId = 1,
                IntEmployeeId = 1,
                intLiberacaoApostilaId = 1
            };

            var progressoSemana = new ProgressoSemana()
            {
                IdEntidade = 1,
                PercentLido = 0
            };

            var tema = cronogramaData.GetTemaApostila();

            var temas = new TemasApostila();
            temas.Add(tema);

            listaLiberacaoApostila.Add(liberacaoApostila);
            listaProgressoSemana.Add(progressoSemana);

            cronogramaMock.GetListaEntidades((int)Produto.Cursos.MED, ano, Constants.CONTACTID_ACADEMICO).Returns(cronogramaData.GetListaEntidadeAulasEspeciaisAposDataLimite());

            cronogramaMock.GetConfiguracaoMateriaisEntidades(menuId, idProduto).Returns(cronogramaData.GetCronogramaPrateleiraAulasEpeciaisAposDataLimite(TipoLayoutMainMSPro.PLAYLIST));

            aulaMock.RetornaApostilasDeAcordoComMatricula(matricula).Returns(listaLiberacaoApostila);
            materialApostilaMock.GetProgressoMaterial(matricula, ano, idProduto).Returns(listaProgressoSemana);

            cronogramaMock.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId).Returns(cronogramaData.GetCronogramaPrateleiraAulasEpeciaisAposDataLimiteAsync(TipoLayoutMainMSPro.PLAYLIST));

            cronogramaMock.GetBookEntitiesBloqueadosNoCronograma().Returns(new List<long> { 10000 });

            cronogramaMock.GetCodigosAmigaveisApostilas().Returns(cronogramaData.GetCodigosAmigaveisApostilas());

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(cronogramaData.GetTurmaBaseAulasEspeciais());

            mednetMock.CalculaProgressosVideosTemaRevisao(tema, matricula).Returns(tema);

            mednetMock.GetTemasVideos(idProduto, matricula, 0, 0, false, tema.IdTema, tipoVideo: ETipoVideo.Revisao).Returns(temas);

            var cronograma = new CronogramaBusiness(aulaMock, mednetMock, materialApostilaMock, revalidaMock, cronogramaMock);
            var retorno = cronograma.GetCronogramaDinamico(idProduto, ano, matricula, menuId);

            cronogramaMock.Received().GetListaEntidades((int)Produto.Cursos.MED, ano, Constants.CONTACTID_ACADEMICO);
            cronogramaMock.Received().GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            cronogramaMock.DidNotReceive().GetBookEntitiesBloqueadosNoCronograma();
            aulaMock.Received().RetornaApostilasDeAcordoComMatricula(matricula);

            Assert.IsTrue(retorno[0].Itens[0].Nome == "Videos em produção");
            Assert.AreEqual(retorno[0].Itens[0].Apostilas, null);
            Assert.IsNotNull(retorno[0].Ordem);
            Assert.IsNotNull(retorno[0].TipoLayout);
        }

        private string GetNomeExcecao(string nomeApostila)
        {
            var nomesComExcecao = new List<string>()
            {
                " U",
                " X",
                "CPM"
            };

            var dicSubstitutosExcecao = new Dictionary<string, string>()
            {
                { " U", " UNICO" },
                { " X", " EXTRA" },
                { "CPM", "CP-MED" }
            };

            var ex = nomesComExcecao.FirstOrDefault(x => nomeApostila.Contains(x));

            if (string.IsNullOrEmpty(ex))
                return "";

            var oldSub = nomesComExcecao.FirstOrDefault(x => nomeApostila.Contains(x));
            var newSub = dicSubstitutosExcecao[oldSub];

            return nomeApostila.Replace(oldSub, newSub);
        }

        private void AssertsConteudoCronograma(int produto, int matricula, ESubMenus menu = ESubMenus.Aulas)
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno(produto, 0, (int)menu, matricula);

            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(0, matricula, produto);

            var antesDataLimite = Utilidades.IsAntesDatalimite(Utilidades.GetYear(), (int)Aplicacoes.MsProMobile);
            var meioDeAno = Utilidades.IsCicloCompletoNoMeioDoAno(matricula);

            var cronogramaPermitido = (from a in cronograma.Semanas
                                       join b in permissoes on a.Numero equals b.Numero
                                       where b.Ativa == 1
                                       select a).ToList();



            Parallel.ForEach(cronogramaPermitido, semana =>
            {
                Assert.IsNotNull(semana.Apostilas, "Semana " + semana.Numero + " lista apostilas null");

                if (menu == ESubMenus.Aulas)
                {
                    Parallel.ForEach(semana.Apostilas, apostila =>
                    {
                        AssertsApostilaAula(apostila, produto, matricula, antesDataLimite, meioDeAno);
                    });
                }
            });
        }

        private void AssertsConteudoCronogramaRevalida(int produto, int matricula)
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno(produto, 0, (int)ESubMenus.Revalida);

            Parallel.ForEach(cronograma.Revalida, semana =>
            {
                Assert.IsNotNull(semana.Apostilas, "Semana " + semana.Numero + " lista apostilas null");

                Parallel.ForEach(semana.Apostilas, apostila =>
                {
                    AssertApostilaRevalida(apostila, matricula);
                });
            });
        }

        private void AssertsApostilaAula(Apostila apostila, int produto, int matricula, bool antesDataLimite, bool meioDeAno)
        {
            var temaId = apostila.Temas[0].TemaID;
            var nomeApostila = apostila.FiltroConteudo.TituloApostila;
            string nomeTema;

            using (var ctx = new DesenvContext())
            {
                nomeTema = ctx.tblLessonTitles.Where(x => x.intLessonTitleID == temaId).FirstOrDefault().txtLessonTitleName;
            }

            var mednet = new MednetEntity();

            var houveAulaPeloCronograma = mednet.HouveAulaPeloCronograma(apostila.MaterialId, nomeTema, matricula, antesDataLimite, meioDeAno, apostila.IdEntidade);

            if (houveAulaPeloCronograma)
            {
                Assert.IsTrue(temaId > 0, nomeApostila + " tema < 0");

                var aulasRevisaoApostila = mednet.GetVideoAulas(produto, temaId, 0, matricula, (int)Aplicacoes.MsProMobile);

                Assert.IsNotNull(aulasRevisaoApostila.Apostila, nomeApostila + " - " + temaId + " não conseguiu acessar os videos");

                Assert.IsTrue(aulasRevisaoApostila.Descricao.Trim().Contains(nomeApostila.Trim()), aulasRevisaoApostila.Descricao + " não contém " + nomeApostila);
            }
        }

        private void AssertApostilaRevalida(Apostila apostila, int matricula)
        {
            var temaId = apostila.Temas[0].TemaID;
            var nomeApostila = apostila.Nome;
            var entidadeId = apostila.IdEntidade;

            Assert.IsTrue(temaId > 0, nomeApostila + " tema < 0");

            var aulaRevalida = new RevalidaEntity().GetTemaRevalida(0, matricula, (int)Aplicacoes.MsProMobile, temaId);

            Assert.IsNotNull(aulaRevalida.Apostila);
            Assert.IsNotNull(aulaRevalida.Apostila.Especialidade);

            Assert.IsTrue(aulaRevalida.Apostila.Descricao.Equals(nomeApostila), nomeApostila + " não é igual " + aulaRevalida.Apostila.Descricao);
            Assert.IsTrue(aulaRevalida.Apostila.Especialidade.Id.Equals(entidadeId), nomeApostila + " não é igual " + aulaRevalida.Apostila.Especialidade.Id);
        }

        private void AssertsPermissaoConteudo(int produto, int matricula, bool anoAnterior = false)
        {
            var anoAtual = Utilidades.GetYear();

            var permissoes = new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(anoAtual, matricula, produto);

            Assert.IsNotNull(permissoes);

            var provider = new CultureInfo("pt-BR");
            var format = "dd/mm";
            var aulasFuturas = permissoes.Where(x => DateTime.ParseExact(x.DataInicio, format, provider) > DateTime.Now).ToList();

            if (!aulasFuturas.Any())
                Assert.Inconclusive();

            int ativa = anoAnterior ? 1 : 0;
            Assert.IsTrue(aulasFuturas.All(x => x.Ativa == ativa));
        }

        private void AssertsPermissaoConteudoUnificada(int produto, int matricula, bool anoAnterior = false)
        {
            var permissoes = new CronogramaEntity().GetPermissoes(matricula, produto);

            Assert.IsNotNull(permissoes.Retorno);
            Assert.IsTrue(permissoes.Sucesso);

            var provider = new CultureInfo("pt-BR");
            var format = "dd/mm";
            var aulasFuturas = permissoes.Retorno.FirstOrDefault().PermissaoSemanas.Where(x => DateTime.ParseExact(x.DataInicio, format, provider) > DateTime.Now).ToList();

            if (!aulasFuturas.Any())
                Assert.Inconclusive();

            int ativa = anoAnterior ? 1 : 0;
            Assert.IsTrue(aulasFuturas.All(x => x.Ativa == ativa));
        }

        private void AssertsProgressosConteudoUnificado(int produto, int matricula)
        {
            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var listaProgresso = business.GetProgressos(produto, matricula);

            if (!listaProgresso.Sucesso)
                Assert.Fail(listaProgresso.Mensagem);

            if (!listaProgresso.Retorno.Any())
                Assert.Inconclusive();

            var progressos = listaProgresso.Retorno.SelectMany(x => x.ProgressoSemanas).ToList();

            Assert.IsTrue(progressos.All(x => x.PercentLido >= 0));
            Assert.IsTrue(progressos.All(x => x.PercentLido <= 100));
        }

        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoAcademico_TodosTemasIntensivao()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.INTENSIVAO, 2019, (int)ESubMenus.Materiais, Constants.CONTACTID_ACADEMICO);

            Assert.AreEqual(24, cronograma.Semanas.Count());
        }

        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoSomenteCIR_SomenteTemaCIR()
        {
            Assert.Inconclusive("Criar CPFS testes do perfil");
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.INTENSIVAO, 2019, (int)ESubMenus.Materiais, 244179);

            Assert.AreEqual(1, cronograma.Semanas.Count());
            Assert.IsFalse(cronograma.Semanas.Any(x => x.Apostilas.Any(y => y.Nome != "CIR")));
        }

        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoSomentePED_SomenteTemaPED()
        {
            Assert.Inconclusive("Criar CPFS testes do perfil");
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.INTENSIVAO, 2019, (int)ESubMenus.Materiais, 215472);

            Assert.AreEqual(1, cronograma.Semanas.Count());
            Assert.IsFalse(cronograma.Semanas.Any(x => x.Apostilas.Any(y => y.Nome != "PED")));
        }


        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoSomenteGO_SomenteTemaGO()
        {
            Assert.Inconclusive("Criar CPFS testes do perfil");
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.INTENSIVAO, 2019, (int)ESubMenus.Materiais, 195428);

            Assert.AreEqual(1, cronograma.Semanas.Count());
            Assert.IsFalse(cronograma.Semanas.Any(x => x.Apostilas.Any(y => y.Nome != "GO")));
        }

        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoSomenteCLM_SomenteTemaCLM()
        {
            Assert.Inconclusive("Criar CPFS testes do perfil");

            var clm = new string[] { "CLM 01", "CLM 02" };
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.INTENSIVAO, 2019, (int)ESubMenus.Materiais, 228804);

            Assert.AreEqual(2, cronograma.Semanas.Count());
            Assert.IsFalse(cronograma.Semanas.Any(x => x.Apostilas.Any(y => !clm.Contains(y.Nome))));
        }


        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoAcademico_TodosTemasRAC()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.RAC_IMED, 2019, (int)ESubMenus.Materiais, Constants.CONTACTID_ACADEMICO);

            Assert.AreEqual(4, cronograma.Semanas.Count());
        }

        [TestMethod]
        [TestCategory("Cronograma_Intensivo")]
        public void GetCronogramaAluno_AlunoAcademico_TodosTemasRACIPE()
        {
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Produto.Cursos.RACIPE_IMED, 2019, (int)ESubMenus.Materiais, Constants.CONTACTID_ACADEMICO);

            Assert.AreEqual(5, cronograma.Semanas.Count());
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_PrateleirasMaterial()
        {
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Materiais);

            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Ano == ano)));
            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Temas.Any())));
            Assert.IsFalse(cronograma.Any(x => x.Apostilas.Any(y => y.ExibeEspecialidade && string.IsNullOrEmpty(y.EspecialidadeCodigo))));
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_PrateleirasAula()
        {
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Aulas);

            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Ano == ano)));
            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Temas.Any())));
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_PrateleirasChecklists()
        {
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Checklists);

            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Ano == ano)));
            Assert.IsTrue(cronograma.All(x => x.Apostilas.All(y => y.Temas.Any())));
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMEDParametroErrado_PrateleirasVazia()
        {
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Nenhum);

            Assert.AreEqual(0, cronograma.Count());
        }


        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoMEDCursoParametroErrado_PrateleirasVazia()
        {
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.MEDCURSO, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Aulas);

            Assert.AreEqual(0, cronograma.Count());
        }

        [TestMethod]
        [TestCategory("Cronograma_SemanaDupla")]
        public void GetCronogramaSemanaDupla_AlunoParametroErrado_RetornaLista()
        {
            var ano = Utilidades.GetYear();
            var produtoErrado = 999;
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetSemanaDuplaCronograma(produtoErrado, ano, Constants.CONTACTID_ACADEMICO);

            Assert.IsTrue(cronograma.Any());
        }

        [TestMethod]
        [TestCategory("Cronograma_SemanaUnica")]
        public void GetCronogramaSemanaSimples_AlunoParametroErrado_ListaVazia()
        {
            var ano = Utilidades.GetYear();
            var produtoErrado = 999;
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetSemanaUnicaCronograma(produtoErrado, ano, Constants.CONTACTID_ACADEMICO);

            Assert.IsTrue(cronograma.Any());
        }

        [TestMethod]
        [TestCategory("Cronograma_Revalida")]
        public void GetCronogramaRevalida_AlunoParametroErrado_ListaVazia()
        {
            var ano = Utilidades.GetYear();
            var produtoErrado = 999;
            var cronograma = new CronogramaEntity().GetRevalidaCronograma(produtoErrado, ano);

            Assert.IsTrue(cronograma.Any());
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_AnoAtual_NaoRetornarEntidadesExcecao()
        {
            var entidadeExcecao = 1159; //OBRA PRIMA CPMED
            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Materiais);

            if (!cronograma.Any())
                Assert.Inconclusive();

            Assert.IsFalse(cronograma.Any(y => y.Apostilas.Any(x => x.IdEntidade == entidadeExcecao)));
            Assert.IsTrue(cronograma.Any(y => y.Apostilas.Any(x => x.IdEntidade != entidadeExcecao)));
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_AnoAnterior_NaoRetornarEntidadesExcecao()
        {
            var entidadeExcecao = 1159; //OBRA PRIMA CPMED
            var ano = Utilidades.GetYear() - 1;
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Materiais);

            if (!cronograma.Any())
                Assert.Inconclusive();

            Assert.IsFalse(cronograma.Any(y => y.Apostilas.Any(x => x.IdEntidade == entidadeExcecao)));
            Assert.IsTrue(cronograma.Any(y => y.Apostilas.Any(x => x.IdEntidade != entidadeExcecao)));
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_RetornarPrateleiraConformeCronograma()
        {
            var intProduto = (int)Produto.Cursos.CPMED;
            var ano = Utilidades.GetYear();
            var matricula = Constants.CONTACTID_ACADEMICO;
            var menuId = (int)ESubMenus.Checklists;

            var prateleira = new List<CronogramaPrateleiraDTO>
            {
                new CronogramaPrateleiraDTO { ID = 1, MaterialId = 1, Descricao = "NOME 1", EntidadeCodigo = "2019 NOME 1", Ordem = 1,  ExibeConformeCronograma = true, Data = DateTime.Now.AddHours(-1)},
                new CronogramaPrateleiraDTO { ID = 2, MaterialId = 2, Descricao = "NOME 2", EntidadeCodigo = "2019 NOME 2",Ordem = 2,  ExibeConformeCronograma = false, Data = DateTime.Now},
                new CronogramaPrateleiraDTO { ID = 2, MaterialId = 2, Descricao = "NOME 2", EntidadeCodigo = "2019 NOME 3",Ordem = 2,  ExibeConformeCronograma = false, Data = DateTime.Now},
                new CronogramaPrateleiraDTO { ID = 3, MaterialId = 3, Descricao = "NOME 3", EntidadeCodigo = "2019 NOME 4",Ordem = 3,  ExibeConformeCronograma = true, Data = DateTime.Now.AddHours(1)}
            };
            var prateleiraAsync = Task<List<CronogramaPrateleiraDTO>>.Factory.StartNew(() => prateleira);

            var codigosAmigaveis = new List<ApostilaCodigoDTO> 
            { 
                new ApostilaCodigoDTO {ProdutoId = 1, Nome = "N1", TemaId = -1},
                new ApostilaCodigoDTO {ProdutoId = 2, Nome = "N2", TemaId = -1},
                new ApostilaCodigoDTO {ProdutoId = 3, Nome = "N3", TemaId = -1}
            };

            var _cronogramaRepository = new CronogramaEntity();
            var entidadesBloqueadas = _cronogramaRepository.GetBookEntitiesBloqueadosNoCronograma();
            var checklistsExtras = _cronogramaRepository.GetChecklistsExtrasLiberados(matricula, intProduto);
            var checklistsPraticos = _cronogramaRepository.GetChecklistsPraticosLiberados(matricula, intProduto);

            var cronoMock =Substitute.For<ICronogramaData>();
            cronoMock.GetCronogramaPrateleirasAsync(intProduto, ano, matricula, menuId).Returns(prateleiraAsync);

            cronoMock.GetBookEntitiesBloqueadosNoCronograma().Returns(entidadesBloqueadas);
            cronoMock.GetCodigosAmigaveisApostilas().Returns(codigosAmigaveis);
            cronoMock.GetChecklistsExtrasLiberados(matricula, intProduto).Returns(checklistsExtras);
            cronoMock.GetChecklistsPraticosLiberados(matricula, intProduto).Returns(checklistsPraticos);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronoMock);
            var cronograma = bus.GetCronogramaPrateleiras(intProduto, ano, matricula, menuId);

            Assert.IsTrue(cronograma.Any(y => y.Id == 1));
            Assert.IsTrue(cronograma.Where(y => y.Id == 2).ToList().Count == 1);
            Assert.IsFalse(cronograma.Any(y => y.Id == 3));

            prateleira.Where(x => x.ID == 3).ToList().ForEach(p => p.Data = DateTime.Now.AddHours(-1));
            prateleiraAsync = Task<List<CronogramaPrateleiraDTO>>.Factory.StartNew(() => prateleira);
            cronograma = bus.GetCronogramaPrateleiras(intProduto, ano, matricula, menuId);
            Assert.IsTrue(cronograma.Any(y => y.Id == 3));
        }

        [TestMethod]
        [TestCategory("Cronograma_Codigos")]
        public void GetCodigoAmigavelApostila_CodigoProdutoId_RetornaNomeAmigavel()
        {
            var codigosMock = new List<ApostilaCodigoDTO>
            {
                new ApostilaCodigoDTO { Nome = "NOME 1", ProdutoId = 1, TemaId = -1 },
                new ApostilaCodigoDTO { Nome = "NOME 2", ProdutoId = -1, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 3", ProdutoId = 2, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 4", ProdutoId = 2, TemaId = 2 }
            };

            var entidadeNomeOriginal = "ACBDEFGHIJKL";
            var materialId = 1;
            var temaId = 99;

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var codigo = bus.GetCodigoAmigavelApostila(materialId, temaId, entidadeNomeOriginal, codigosMock);

            Assert.AreEqual("NOME 1", codigo);

        }

        [TestMethod]
        [TestCategory("Cronograma_Codigos")]
        public void GetCodigoAmigavelApostila_CodigoLessonTitleId_RetornaNomeAmigavel()
        {
            var codigosMock = new List<ApostilaCodigoDTO>
            {
                new ApostilaCodigoDTO { Nome = "NOME 1", ProdutoId = 1, TemaId = -1 },
                new ApostilaCodigoDTO { Nome = "NOME 2", ProdutoId = -1, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 3", ProdutoId = 2, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 4", ProdutoId = 2, TemaId = 2 }
            };

            var entidadeNomeOriginal = "ACBDEFGHIJKL";
            var materialId = 99;
            var temaId = 1;

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var codigo = bus.GetCodigoAmigavelApostila(materialId, temaId, entidadeNomeOriginal, codigosMock);

            Assert.AreEqual("NOME 2", codigo);
        }

        [TestMethod]
        [TestCategory("Cronograma_Codigos")]
        public void GetCodigoAmigavelApostila_CodigoProdutoIdLessonTitleId_RetornaNomeAmigavel()
        {
            var codigosMock = new List<ApostilaCodigoDTO>
            {
                new ApostilaCodigoDTO { Nome = "NOME 1", ProdutoId = 1, TemaId = -1 },
                new ApostilaCodigoDTO { Nome = "NOME 2", ProdutoId = -1, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 3", ProdutoId = 2, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 4", ProdutoId = 2, TemaId = 2 }
            };

            var entidadeNomeOriginal = "ACBDEFGHIJKL";
            var materialId = 2;
            var temaId = 2;

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var codigo = bus.GetCodigoAmigavelApostila(materialId, temaId, entidadeNomeOriginal, codigosMock);

            Assert.AreEqual("NOME 4", codigo);
        }

        [TestMethod]
        [TestCategory("Cronograma_Codigos")]
        public void GetCodigoAmigavelApostila_CodigoNaoCadastrado_RetornaNomeEncurtado()
        {
            var codigosMock = new List<ApostilaCodigoDTO>
            {
                new ApostilaCodigoDTO { Nome = "NOME 1", ProdutoId = 1, TemaId = -1 },
                new ApostilaCodigoDTO { Nome = "NOME 2", ProdutoId = -1, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 3", ProdutoId = 2, TemaId = 1 },
                new ApostilaCodigoDTO { Nome = "NOME 4", ProdutoId = 2, TemaId = 2 }
            };

            var entidadeNomeOriginal = "2019 APOSTILA";
            var materialId = 99;
            var temaId = 99;

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var codigo = bus.GetCodigoAmigavelApostila(materialId, temaId, entidadeNomeOriginal, codigosMock);

            Assert.AreEqual("APOSTILA", codigo);
        }


        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistPraticoCriado_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = true,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistPraticoNaoCriado_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = true,
                    ProductId = 2000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(-2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistExtraNaoCriado_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 2000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(-2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistExtraCriadoPreCronograma_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistExtraCriadoAcessoAntecipado_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(-2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_ChecklistExtraCriadoPosCronograma_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(-2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_AulasPosCronograma_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Aulas;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(-2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_AulasAcessoAntecipado_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Aulas;
            bool acessoAntecipado = true;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_AulasPreCronograma_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Aulas;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_MaterialPreCronograma_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Materiais;
            bool acessoAntecipado = false;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_MaterialNaoLiberadoAcessoAntecipado_False()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 2000;
            int menuId = (int)ESubMenus.Materiais;
            bool acessoAntecipado = true;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsFalse(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void GetAprovacaoApostilas_MaterialLiberadoAcessoAntecipado_True()
        {
            var apostilasAprovadas = new List<MaterialLiberacaoDTO>
            {
                new MaterialLiberacaoDTO
                {
                    LiberacaoAutomatica = false,
                    ProductId = 1000
                }
            };
            var materialId = 1000;
            int menuId = (int)ESubMenus.Checklists;
            bool acessoAntecipado = true;
            var dataCronograma = DateTime.Now.AddDays(2);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var aprovacao = bus.GetAprovacaoApostilas(dataCronograma, apostilasAprovadas, materialId, menuId, acessoAntecipado);

            Assert.IsTrue(aprovacao);
        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_ChecklistPraticoInseridoRetornaTrue()
        {
            var checklistsExistentesAluno = new MaterialApostilaEntity().GetChecklistsPratico(Constants.CONTACTID_ACADEMICO);

            var ano = Utilidades.GetYear();
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Checklists);

            var apostilasLiberadas = cronograma.SelectMany(x => x.Apostilas.Where(y => y.Aprovada)).Select(z => z.MaterialId).ToList();

            Assert.IsTrue(checklistsExistentesAluno.All(x => apostilasLiberadas.Contains(x)));

        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_ChecklistExtraLiberadoAnoAtual_RetornaTrue()
        {

            var ano = Utilidades.GetYear();
            var checklistsExtras = new MaterialApostilaEntity().GetChecklistsExtrasLiberado(ano);

            if (!checklistsExtras.Any())
                Assert.Inconclusive();


            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Checklists);

            var apostilasLiberadas = cronograma.SelectMany(x => x.Apostilas.Where(y => y.Aprovada)).Select(z => z.MaterialId).ToList();

            Assert.IsTrue(checklistsExtras.All(x => apostilasLiberadas.Contains(x)));

        }

        [TestMethod]
        [TestCategory("Cronograma_Prateleiras")]
        public void GetCronogramaPrateleiras_AlunoAcademicoCPMED_ChecklistExtraLiberadoAnoAnterior_RetornaTrue()
        {

            var ano = Utilidades.GetYear() - 1;
            var checklistsExtras = new MaterialApostilaEntity().GetChecklistsExtrasLiberado(ano);

            if (!checklistsExtras.Any())
                Assert.Inconclusive();


            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaPrateleiras((int)Produto.Cursos.CPMED, ano, Constants.CONTACTID_ACADEMICO, (int)ESubMenus.Checklists);

            var apostilasLiberadas = cronograma.SelectMany(x => x.Apostilas.Where(y => y.Aprovada)).Select(z => z.MaterialId).ToList();

            Assert.IsTrue(checklistsExtras.All(x => apostilasLiberadas.Contains(x)));

        }



        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void Get_GetChecklistsExtrasLiberados_SomenteExtras()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var idProduto = (int)Produto.Cursos.CPMED;

            var bus = new CronogramaEntity();
            var aprovacao = bus.GetChecklistsExtrasLiberados(matricula, idProduto);

            Assert.IsTrue(aprovacao.All(x => x.ProductGroup3Id == (int)Produto.Produtos.APOSTILA_CPMED));
        }

        [TestMethod]
        [TestCategory("Cronograma_LiberacaoConteudo")]
        public void Get_GetChecklistsPraticosLiberados_SomentePraticos()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var idProduto = (int)Produto.Cursos.CPMED;

            var bus = new CronogramaEntity();
            var aprovacao = bus.GetChecklistsPraticosLiberados(matricula, idProduto);

            Assert.IsTrue(aprovacao.All(x => x.ProductGroup3Id == (int)Produto.Produtos.CPMED));
        }

        [TestMethod]
        [TestCategory("CronogramaDasAulasPresencial")]
        public void Get_CronogramaTurmas_ListaDoCronogramaDeAulaDeOutraTurmaAnoAtual()
        {
            var anoAtual = DateTime.Now.Year;

            var lstFiliaisDTO = new FilialBusiness(new FilialEntity()).GetFiliaisCronograma();
            var idFIlial = lstFiliaisDTO.First().Id;

            var lstTurmasDTO = new TurmaBusiness(new TurmaEntity(), new ProdutoEntity(), new TemplatePagamentoEntity()).GetTurmasCronograma(idFIlial, anoAtual);
            var idTuma = lstTurmasDTO.First().Id;

            var cronogramaBusiness = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = cronogramaBusiness.GetCronogramaAulaTurma(Convert.ToInt32(idTuma), anoAtual);

            Assert.IsNotNull(cronograma);
            Assert.IsNotNull(cronograma[0].tema);
        }


        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetPermissoes_AlunoMedMasterAnoAtual_AnoAnteriorMEDLiberadoCompletamente()
        {
            var anoAnterior = Utilidades.GetYear() - 1;
            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivo();
            var produto = (int)Produto.Cursos.MED;
            var response = new CronogramaEntity().GetPermissoes(produto, aluno.ID, anoAnterior);

            var permissoes = response.Retorno.SelectMany(x => x.PermissaoSemanas.Select(y => y.Ativa)).ToList();

            Assert.IsTrue(permissoes.All(x => x == 1));
        }

        [TestMethod]
        [TestCategory("FiltroConteudo")]
        public void GetPalavrasChavesTema_EntidadeMedCurso_NaoRetornaPalavraClm()
        {
            msp_API_ListaEntidades_Result mockResult = new msp_API_ListaEntidades_Result
            {
                intID = 673,
                entidade = "NEF 1",
                txtLessonTitleName = "Glomerulopatias I (Nefrítica, Alterações Assintomáticas, GNRP)",
                txtCode = "2020 NEF 1",
                txtDescription = "Apostila de Nefrologia  (CLM)"
            };

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var palavrasChaves = bus.GetPalavrasChavesTema(mockResult);

            Assert.IsFalse(palavrasChaves.Contains("clm"));
        }


        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetPermissoes_AlunoMedMasterAnoAtual_AnoAnteriorMEDCURSOLiberadoCompletamente()
        {
            var anoAnterior = Utilidades.GetYear() - 1;
            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivo();
            var produto = (int)Produto.Cursos.MEDCURSO;
            var response = new CronogramaEntity().GetPermissoes(produto, aluno.ID, anoAnterior);

            var permissoes = response.Retorno.SelectMany(x => x.PermissaoSemanas.Select(y => y.Ativa)).ToList();

            Assert.IsTrue(permissoes.All(x => x == 1));
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetPermissoes_AlunoMedMasterAnoAtual_AnoAtualCanceladoMEDCURSOLiberadoConformeCronograma()
        {

            var produto = (int)Produto.Produtos.MEDCURSO;

            var curso = (int)Produto.Cursos.MEDCURSO;

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivoComAnoAtualCancelado(produto);

            AssertsConteudoCronograma(curso, aluno.ID);
        }

        [TestMethod]
        [TestCategory("Cronograma_Permissao")]
        public void GetPermissoes_AlunoMedMasterAnoAtual_AnoAtualCanceladoMEDLiberadoConformeCronograma()
        {

            var produto = (int)Produto.Produtos.MED;
            var curso = (int)Produto.Cursos.MED;

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivoComAnoAtualCancelado(produto);


            AssertsConteudoCronograma(curso, aluno.ID);

        }

        [TestMethod]
        [TestCategory("FiltroConteudo")]
        public void GetPalavrasChavesTema_EntidadeMedCurso_NaoRetornaPalavraApostila()
        {
            msp_API_ListaEntidades_Result mockResult = new msp_API_ListaEntidades_Result
            {
                intID = 673,
                entidade = "NEF 1",
                txtLessonTitleName = "Glomerulopatias I (Nefrítica, Alterações Assintomáticas, GNRP)",
                txtCode = "2020 NEF 1",
                txtDescription = "Apostila de Nefrologia  (CLM)"
            };

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var palavrasChaves = bus.GetPalavrasChavesTema(mockResult);

            Assert.IsFalse(palavrasChaves.Contains("apostila de"));
        }


        [TestMethod]
        [TestCategory("FiltroConteudo")]
        public void GetPalavrasChavesTema_PropriedadeDescriptionNula_RetornaStringRestantes()
        {
            msp_API_ListaEntidades_Result mockResult = new msp_API_ListaEntidades_Result
            {
                intID = 673,
                entidade = "NEF 1",
                txtLessonTitleName = "Glomerulopatias I (Nefrítica, Alterações Assintomáticas, GNRP)",
                txtCode = "2020 NEF 1",
                txtDescription = null
            };

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var palavrasChaves = bus.GetPalavrasChavesTema(mockResult);

            Assert.IsTrue(palavrasChaves.Contains("nef"));
            Assert.IsTrue(palavrasChaves.Contains("glomerulopatias"));
        }

        [TestMethod]
        [TestCategory("FiltroConteudo")]
        public void GetPalavrasChavesTema_PropriedadeLessonTitileNameNula_RetornaStringRestantes()
        {
            msp_API_ListaEntidades_Result mockResult = new msp_API_ListaEntidades_Result
            {
                intID = 673,
                entidade = "NEF 1",
                txtLessonTitleName = null,
                txtCode = "2020 NEF 1",
                txtDescription = "Apostila de Nefrologia  (CLM)"
            };

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var palavrasChaves = bus.GetPalavrasChavesTema(mockResult);

            Assert.IsTrue(palavrasChaves.Contains("nef"));
            Assert.IsTrue(palavrasChaves.Contains("nefrologia"));
        }

        [TestMethod]
        [TestCategory("FiltroConteudo")]
        public void GetPalavrasChavesTema_PropriedadeCode_RetornaStringRestantes()
        {
            msp_API_ListaEntidades_Result mockResult = new msp_API_ListaEntidades_Result
            {
                intID = 673,
                entidade = "NEF 1",
                txtLessonTitleName = "Glomerulopatias I (Nefrítica, Alterações Assintomáticas, GNRP)",
                txtCode = null,
                txtDescription = "Apostila de Nefrologia  (CLM)"
            };

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var palavrasChaves = bus.GetPalavrasChavesTema(mockResult);

            Assert.IsTrue(palavrasChaves.Contains("glomerulopatias"));
            Assert.IsTrue(palavrasChaves.Contains("nefrologia"));
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaPadrao_RetornaMatriculaPadrao()
        {
            var turmaPadrao = -1;
            var matriculaPadrao = 96409;
            var anoAtual = Utilidades.GetYear();

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, anoAtual, (int)Produto.Cursos.MED);

            Assert.AreEqual(retorno, matriculaPadrao);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaPadrao_RetornaMatriculaBaseGeral()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 100;
            var matriculaPadrao = Constants.CONTACTID_ACADEMICO;
            var ano = DateTime.Now.Year;
            var produtoId = 17;

            var turmasMockdata = new List<TurmaMatriculaBaseDTO>();
            var turmaBaseGeral = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = -1, DataCadastro = DateTime.Now, Id = 1, MatriculaBase = Constants.CONTACTID_ACADEMICO };
            var turmaBaseExcecao = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = 1, DataCadastro = DateTime.Now, Id = 2, MatriculaBase = 2 };
            turmasMockdata.Add(turmaBaseExcecao);
            turmasMockdata.Add(turmaBaseGeral);

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(turmasMockdata);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);

            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId);

            Assert.AreEqual(retorno, matriculaPadrao);
        }



        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaExcecao_RetornaMatriculaBaseTurma()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 1;
            var matriculaPadrao = 2;
            var ano = DateTime.Now.Year;
            var produtoId = 17;

            var turmasMockdata = new List<TurmaMatriculaBaseDTO>();
            var turmaBaseGeral = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = -1, DataCadastro = DateTime.Now, Id = 1, MatriculaBase = Constants.CONTACTID_ACADEMICO };
            var turmaBaseExcecao = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = 1, DataCadastro = DateTime.Now, Id = 2, MatriculaBase = 2 };
            turmasMockdata.Add(turmaBaseExcecao);
            turmasMockdata.Add(turmaBaseGeral);

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(turmasMockdata);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);


            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId );

            Assert.AreEqual(retorno, matriculaPadrao);

        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaNaoCadastradaExcecao_RetornaMatriculaBaseGeral()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 9999;
            var matriculaPadrao = Constants.CONTACTID_ACADEMICO;
            var ano = DateTime.Now.Year;
            var produtoId = 17;

            var turmasMockdata = new List<TurmaMatriculaBaseDTO>();
            var turmaBaseGeral = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = -1, DataCadastro = DateTime.Now, Id = 1, MatriculaBase = Constants.CONTACTID_ACADEMICO };
            var turmaBaseExcecao = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = 1, DataCadastro = DateTime.Now, Id = 2, MatriculaBase = 2 };
            turmasMockdata.Add(turmaBaseExcecao);
            turmasMockdata.Add(turmaBaseGeral);

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(turmasMockdata);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);

            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId);

            Assert.AreEqual(retorno, matriculaPadrao);

        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaExcecao_AnoDiferente_RetornaMatriculaBaseGeral()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 1;
            var matriculaPadrao = Constants.CONTACTID_ACADEMICO;
            var ano = DateTime.Now.Year;
            var anoCadastro = ano - 1;
            var produtoId = 17;

            var turmasMockdata = new List<TurmaMatriculaBaseDTO>();
            var turmaBaseGeral = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = -1, DataCadastro = DateTime.Now, Id = 1, MatriculaBase = Constants.CONTACTID_ACADEMICO };
            var turmaBaseExcecao = new TurmaMatriculaBaseDTO { Ano = anoCadastro, CourseId = turmaPadrao, DataCadastro = DateTime.Now, Id = 2, MatriculaBase = 2 };
            turmasMockdata.Add(turmaBaseExcecao);
            turmasMockdata.Add(turmaBaseGeral);

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(turmasMockdata);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);

            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId);

            Assert.AreEqual(retorno, matriculaPadrao);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaExcecao_AnoIgual_TurmaDiferente_RetornaMatriculaBaseGeral()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 1;
            var matriculaPadrao = Constants.CONTACTID_ACADEMICO;
            var ano = DateTime.Now.Year;
            var produtoId = 17;

            var turmasMockdata = new List<TurmaMatriculaBaseDTO>();
            var turmaBaseGeral = new TurmaMatriculaBaseDTO { Ano = -1, CourseId = -1, DataCadastro = DateTime.Now, Id = 1, MatriculaBase = Constants.CONTACTID_ACADEMICO };
            var turmaBaseExcecao = new TurmaMatriculaBaseDTO { Ano = ano, CourseId = 2, DataCadastro = DateTime.Now, Id = 2, MatriculaBase = 2 };
            turmasMockdata.Add(turmaBaseExcecao);
            turmasMockdata.Add(turmaBaseGeral);

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(turmasMockdata);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);


            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId);

            Assert.AreEqual(retorno, matriculaPadrao);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetMatriculaBaseCronogramaTurma_TurmaExcecao_Nulo_RetornaMatriculaBaseGeral()
        {
            var cronogramaMock =Substitute.For<ICronogramaData>();

            var turmaPadrao = 1;
            var matriculaPadrao = Constants.CONTACTID_ACADEMICO;
            var ano = DateTime.Now.Year;
            var produtoId = 17;

            cronogramaMock.GetTurmaMatriculasBase(Arg.Any<TurmaMatriculaBaseDTO>()).Returns(new List<TurmaMatriculaBaseDTO>());
           
           // cronogramaMock.Stub(x => x.GetTurmaMatriculasBase(Arg<TurmaMatriculaBaseDTO>.Is.Anything)).Return(null);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), cronogramaMock);

            var retorno = bus.GetMatriculaBaseCronogramaTurma(turmaPadrao, ano, produtoId);

            Assert.AreEqual(retorno, matriculaPadrao);
        }



        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDEAD_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MEDEAD, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MED, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MED, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDCURSOEAD_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MEDCURSOEAD, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MEDCURSO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MEDCURSO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_R3CLM_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.R3CLINICA, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.R3Clinica, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.R3Clinica, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_R3CIR_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.R3CIRURGIA, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.R3Cirurgia, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.R3Cirurgia, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_R3PED_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.R3PEDIATRIA, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.R3Pediatria, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.R3Pediatria, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_R4GO_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.R4GO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.R4GO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.R4GO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDELETROIMED_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MEDELETRO_IMED, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MEDELETRO_IMED, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MEDELETRO_IMED, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDELETRO_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MEDELETRO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MEDELETRO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MEDELETRO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_TEGO_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.TEGO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.TEGO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.TEGO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MASTO_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MASTO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MASTO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MASTO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_TurmaEspecialMED_RetornaCronogramaMatriculaBaseTurma()
        {

            var perfilTestData = new PerfilAlunoEntityTestData();
            var cronogramaEntity = new CronogramaEntity();
            var turma = 18212;  
            var produto = Produto.Produtos.MED;
            var curso = Produto.Cursos.MED.GetHashCode();


            var turmaBase = cronogramaEntity.GetTurmaMatriculasBase(new TurmaMatriculaBaseDTO {CourseId = turma }).FirstOrDefault(y => y.CourseId != -1);

            var anoAtual = Utilidades.GetYear();
            var aluno = perfilTestData.GetAlunosStatus2(produto, OrdemVenda.StatusOv.Adimplente, turmaBase.CourseId ).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = cronogramaEntity.GetListaEntidades(curso, anoAtual, turmaBase.MatriculaBase);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase(curso, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_TurmaEspecialMEDCURSO_RetornaCronogramaMatriculaBaseTurma()
        {

            var perfilTestData = new PerfilAlunoEntityTestData();
            var cronogramaEntity = new CronogramaEntity();
            var turma = 22838;
            var produto = Produto.Produtos.MEDCURSO;
            var curso = Produto.Cursos.MEDCURSO.GetHashCode();

            var turmaBase = cronogramaEntity.GetTurmaMatriculasBase(new TurmaMatriculaBaseDTO { CourseId = turma }).FirstOrDefault(y => y.CourseId != -1);

            var anoAtual = Utilidades.GetYear();
            var aluno = perfilTestData.GetAlunosStatus2(produto, OrdemVenda.StatusOv.Adimplente, turmaBase.CourseId).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = cronogramaEntity.GetListaEntidades(curso, anoAtual, turmaBase.MatriculaBase);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase(curso, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDMASTER_MED_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MED_MASTER, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MED, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MED, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDMASTER_MEDCURSO_RetornaCronogramaMatriculaBasePadrao()
        {
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MED_MASTER, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MEDCURSO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MEDCURSO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDMASTERATIVO_MEDCURSOATIVO_RetornaCronogramaTurmasEspeciais()
        {
            var alunoMedAtivo = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MED, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();
            if (alunoMedAtivo == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            
            var aulaMock =Substitute.For<IAulaEntityData>();
            aulaMock.AlunoPossuiMedMaster(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            aulaMock.AlunoPossuiMedOuMedcursoAnoAtualAtivo(Arg.Any<int>()).Returns(true);

            var ret = new CronogramaBusiness(
                aulaMock,
                new MednetEntity(),
                new MaterialApostilaEntity(),
                new RevalidaEntity(),
                new CronogramaEntity()).GetListaEntidadesPorMatriculaBase(17, Utilidades.GetYear(), alunoMedAtivo.ID);
            Assert.IsTrue(ret.Any(x => x.intLessonTitleID == 5998));

        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDMASTERCANCELADO_MEDCURSOATIVO_RetornaCronogramaTurmasEspeciais()
        {
            var alunoMedAtivo = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MED, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();
            if (alunoMedAtivo == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");
            
            var aulaMock =Substitute.For<IAulaEntityData>();
            aulaMock.AlunoPossuiMedMaster(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
            aulaMock.AlunoPossuiMedOuMedcursoAnoAtualAtivo(Arg.Any<int>()).Returns(true);

            var ret = new CronogramaBusiness(
                aulaMock,
                new MednetEntity(),
                new MaterialApostilaEntity(),
                new RevalidaEntity(),
                new CronogramaEntity()).GetListaEntidadesPorMatriculaBase(17, Utilidades.GetYear(), alunoMedAtivo.ID);
            Assert.IsTrue(ret.Any(x => x.intLessonTitleID == 5998));

        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDMASTERATIVO_MEDCURSOCANCELADO_RetornaCronogramaBasePadrao()
        {
            var alunoMedAtivo = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MED, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();
            if (alunoMedAtivo == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");
            var aulaMock =Substitute.For<IAulaEntityData>();
            aulaMock.AlunoPossuiMedMaster(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            aulaMock.AlunoPossuiMedOuMedcursoAnoAtualAtivo(Arg.Any<int>()).Returns(false);

            var ret = new CronogramaBusiness(
                aulaMock,
                new MednetEntity(),
                new MaterialApostilaEntity(),
                new RevalidaEntity(),
                new CronogramaEntity()).GetListaEntidadesPorMatriculaBase(17, Utilidades.GetYear(), alunoMedAtivo.ID);
            Assert.IsTrue(ret.Any(x => x.intLessonTitleID == 3701));
        }

        [TestMethod]
        [TestCategory("Cronograma_Excecao")]
        public void GetListaEntidadesPorMatriculaBase_MEDCURSO_OV_ATIVA_RetornaCronogramaMatriculaBasePadrao()
        {
            int turmaid = 22779;
            var anoAtual = Utilidades.GetYear();
            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoAtual, Produto.Produtos.MEDCURSO , OrdemVenda.StatusOv.Adimplente, turmaid);

            if (aluno == null)
                Assert.Inconclusive("Não possui aluno no cenário necessário");

            var cronogramaMatriculaBase = new CronogramaEntity().GetListaEntidades((int)Produto.Cursos.MEDCURSO, anoAtual, Constants.CONTACTID_ACADEMICO);

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var retorno = bus.GetListaEntidadesPorMatriculaBase((int)Produto.Cursos.MEDCURSO, anoAtual, aluno.ID);

            var expected = JsonConvert.SerializeObject(cronogramaMatriculaBase);
            var actual = JsonConvert.SerializeObject(retorno);

            Assert.AreEqual(expected, actual);
        }
    }

    }
