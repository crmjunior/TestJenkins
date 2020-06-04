using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class ComboEntityTests
    {
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetCombo_Mspro()
        {
            var idAplicacao = 17;
            var matricula = 96409;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(matricula, idAplicacao, null);
            
            Assert.IsNotNull(combo);
            Assert.AreNotEqual(0, combo.Count);
        }

               
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoMedMedcursoAdaptaMed()

        {
            //var matricula = 96409;
            var dataTolerancia = Utilidades.DataToleranciaTestes();


            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            int ano = Utilidades.GetYear();
            var anoMed = ano;
            var anoMedCurso = (ano - 1);
            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoMedMedCursoAdaptaMed(anoMed, anoMedCurso);
            if (aluno == null)
            {
                Assert.Inconclusive();
            }

            var matricula = aluno.ID;



            var permitidos = new List<int>() { (int)Produto.Cursos.MEDCURSO, (int)Produto.Cursos.MED, (int)Produto.Cursos.ADAPTAMED };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoSomenteMed()
        {
            var business = new PerfilAlunoEntityTestData();
            var matricula = business.GetMatriculaAluno_SomenteUmaOV(2019, (int)Produto.Produtos.MED, 2, 6);
            var permitidos = new List<int>() { (int)Produto.Cursos.MED };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoSomenteMedcurso()
        {
            var matricula = 44776;
            var permitidos = new List<int>() { (int)Produto.Cursos.MEDCURSO };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoMedMedcurso()
        {
            var matricula = 241740;
            var permitidos = new List<int>() { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDCURSO };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [Ignore] //matricula que atenda o cenário ainda não encontrada
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoMedAdaptaMed()
        {
            var matricula = 0;
            var permitidos = new List<int>() { (int)Produto.Cursos.MED, (int)Produto.Cursos.ADAPTAMED };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [Ignore] //matricula que atenda o cenário ainda não encontrada
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void CanGetProdutosValidosCombo_Mspro_AlunoMedcursoAdaptaMed()
        {
            var matricula = 0;
            var permitidos = new List<int>() { (int)Produto.Cursos.MEDCURSO, (int)Produto.Cursos.ADAPTAMED };

            AssertsProdutosPermitidos(matricula, permitidos);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_RetornaSomenteMaiorAnoProdutoAntes2018_OU_RetornaAnosProdutoApartir2018()
        {
            var idAplicacao = 17;
            var business = new PerfilAlunoEntityTestData();
            var matricula = business.GetAlunoAnoAtualComAnosAnteriores();
            var ano = 2018;

            var comboMsPro = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);

            Assert.IsNotNull(comboMsPro);
            Assert.AreNotEqual(0, comboMsPro.Count);

            foreach (Combo combo in comboMsPro)
            {
                if (combo.Anos.Any(a => a >= ano))
                {
                    Assert.IsTrue(combo.Anos.Any(a => a >= ano));
                    Assert.IsFalse(combo.Anos.Any(a => a < ano));
                }
                else
                {
                    Assert.IsFalse(combo.Anos.Any(a => a >= ano));
                    Assert.IsTrue(combo.Anos.Any(a => a < ano));
                }
            } 
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoR3Cancelado_RetornaProdutoR3()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile; 
            var matricula = new PerfilAlunoEntity().GetAlunoR3Cancelado().ID;

            var comboMsPro = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);

            var cursosR3 = Utilidades.CursosR3();

            Assert.IsTrue(comboMsPro.Where(x => cursosR3.Contains(x.ComboId)).Any());  
        }

        private void AssertsProdutosPermitidos(int matricula, List<int> permitidos, int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(matricula, idAplicacao, null);
            
            Assert.IsTrue(combo.All(x => permitidos.Contains(x.ComboId)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoAnoAtualMEDCURSO_RetornaAnoAtual()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedcursoAnoAtualAtivo();
            var anoAtual = Utilidades.GetYear();
            AssertAnosdeProduto(aluno.ID, (int)Produto.Cursos.MEDCURSO, anoAtual);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoAnoAtualMED_RetornaAnoAtual()
        {
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAtual, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente);

            AssertAnosdeProduto(matricula, (int)Produto.Cursos.MED, anoAtual);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoCanceladoAnoAtualMEDCURSO_RetornaAnoAtual()
        {
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAtual, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada);

            AssertAnosdeProduto(matricula, (int)Produto.Cursos.MEDCURSO, anoAtual);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoCanceladoAnoAtualMED_RetornaAnoAtual()
        {
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAtual, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada);

            AssertAnosdeProduto(matricula, (int)Produto.Cursos.MED, anoAtual);
        }


        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoAnoAnteriorlMEDCURSO_RetornaAnoAnterior()
        {
            var ano = Utilidades.GetYear() - 1;
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(ano, (int)Produto.Produtos.MEDCURSO, (int)Utilidades.ESellOrderStatus.Ativa, (int)Utilidades.ESellOrderStatus.Adimplente) ;         
            AssertAnosdeProduto(matricula, (int)Produto.Cursos.MEDCURSO, ano);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void ComboProdutos_AlunoAnoAnteriorMED_RetornaAnoAnterior()
        {
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedAnoAnterior();
            var ano = Utilidades.GetYear() - 1;
            AssertAnosdeProduto(matricula, (int)Produto.Cursos.MED, ano);
        }


        private void AssertAnosdeProduto(int matricula, int produtoId, int ano , int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(matricula, idAplicacao, null);
            var comboProduto = combo.FirstOrDefault(x => x.ComboId == produtoId );
            Assert.IsTrue(comboProduto.Anos.Contains(ano));
        }

        private void AssertsProdutosPermitidosAntesDepoisDataLimite(int matricula, int anoMatricula, List<int> cursos, int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            var permissoes = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetPermissoes(matricula, idAplicacao);
            var comboPermitido =   new ComboEntity().GetCombosPermitidos(permissoes, idAplicacao, null);
            var lstAnoPorProduto =  new ComboEntity().GetAnosPorProduto(matricula);



            //Antes da data limite
            var comboMock = Substitute.For<IComboData>();
            comboMock.IsBeforeDataLimite(idAplicacao, anoMatricula).Returns(true);
            comboMock.GetCombosPermitidos(Arg.Any<List<AccessObject>>(), Arg.Any<int>(), Arg.Any<string>()).Returns(comboPermitido);
            comboMock.GetAnosPorProduto(matricula).Returns(lstAnoPorProduto);

            var comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);
            Assert.IsTrue(comboMsPro.Where(x => cursos.Contains(x.ComboId)).Any());
            Assert.IsTrue(comboMsPro.Where(x => cursos.Contains(x.ComboId)).Where(y => y.Anos.Contains(anoMatricula)).Any());

            //Depois da data limite
            comboMock = Substitute.For<IComboData>();
            comboMock.IsBeforeDataLimite(idAplicacao, anoMatricula).Returns(false);
            comboMock.GetCombosPermitidos(Arg.Any<List<AccessObject>>(), Arg.Any<int>(), Arg.Any<string>()).Returns(comboPermitido);
            comboMock.GetAnosPorProduto(matricula).Returns(lstAnoPorProduto);

            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);
            Assert.IsTrue(comboMsPro.Where(x => cursos.Contains(x.ComboId)).Any());
            Assert.IsTrue(comboMsPro.Where(x => cursos.Contains(x.ComboId)).Where(y => y.Anos.Contains(anoMatricula)).Any());
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_RMaisAnoAtual_RetornaRMaisAnoAtual()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoR3(anoAtual).ID;
            var cursosR3 = Utilidades.CursosR3();

            AssertsProdutosPermitidosAntesDepoisDataLimite(matricula, anoAtual, cursosR3, idAplicacao);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_ExtensivoAnoAtual_RetornaExtensivoAnoAtual()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAtual, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada);

            var cursos = new List<int>() { Produto.Cursos.MED.GetHashCode(), Produto.Cursos.MEDCURSO.GetHashCode() };

            AssertsProdutosPermitidosAntesDepoisDataLimite(matricula, anoAtual, cursos, idAplicacao);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_ExtensivoAnoAnterior_RetornaExtensivoAnoAnterior()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoAnterior = Utilidades.GetYear() -1;
            var matricula = new PerfilAlunoEntityTestData().GetMatriculaAluno_SomenteUmaOV(anoAnterior, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente);
            var cursos = new List<int>() { Produto.Cursos.MED.GetHashCode(), Produto.Cursos.MEDCURSO.GetHashCode() };

            AssertsProdutosPermitidosAntesDepoisDataLimite(matricula, anoAnterior, cursos, idAplicacao);
        }

 [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_MedEletroAnoAtual_RetornaMedEletroAnoAtual()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoAtual = Utilidades.GetYear();
            var matricula = new PerfilAlunoEntityTestData().GetAlunoExtensivoEMedEletroAnoAtualAtivo();
            var cursos = new List<int>() { Produto.Cursos.MEDELETRO.GetHashCode() };
            if (matricula != null)
                AssertsProdutosPermitidosAntesDepoisDataLimite(matricula.ID, anoAtual, cursos, idAplicacao);
            else
                Assert.Inconclusive("Não existe nenhum Aluno Extensivo e MedEletro ano atual ativo");
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_MedEletroAnoAnterior_RetornaMedEletroAnoAnterior()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive("Esse teste só será válido a partir de 01/01/2020");
            }

            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoAnterior = Utilidades.GetYear() - 1;
            var matricula = new PerfilAlunoEntityTestData().GetAlunosMedEletroAnoAnteriorAtivo().FirstOrDefault().ID;
            var cursos = new List<int>() { Produto.Cursos.MEDELETRO.GetHashCode() };

            AssertsProdutosPermitidosAntesDepoisDataLimite(matricula, anoAnterior, cursos, idAplicacao);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoMedEletroIMedAtivo_RetornaProdutoMedEletroIMed()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var aluno = new PerfilAlunoEntityTestData().GetAlunosMedEletroIMedAnoAtualAtivo().FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive();

            var matricula = aluno.ID;

            var comboMsPro = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);

            var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();

            Assert.IsTrue(comboMsPro.Where(x => x.ComboId == produtoMedEletroIMed).Any());
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoMedEletroIMedCancelado_RetornaProdutoMedEletroIMed()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var matricula = new PerfilAlunoEntityTestData().GetAlunoMedEletroIMedCancelado().ID;

            var comboMsPro = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, null);

            var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();

            Assert.IsTrue(comboMsPro.Where(x => x.ComboId == produtoMedEletroIMed).Any());
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MedEAD_Med()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDEAD, new int[] { 2017 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);
            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDEAD));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2019_MEDCURSO2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019()
        {

            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
 
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2019_MED2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019()
        {


            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2018_MEDCURSO2017_VISUALIZA_MEDCURSO2020_MEDMEDCURSO2019_MED2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2018_MED2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2019_MED2018_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019_MED2018()
        {

            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2019_MEDCURSO2018_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2018_MED2018_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019_MEDMEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2020_MEDCURSO2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2020_MED2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2017_MED2017_VISUALIZA_MEDMEDCURSO2020_MEDMEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2017 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MedeMedEAD_MedAgrupado()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018, 2019 } },
                { (int)Utilidades.ProductGroups.MEDEAD, new int[] { 2017 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);          
            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDEAD));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MedCursoEAD_Med()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSOEAD, new int[] { 2017 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);
            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSOEAD));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MedcursoeMedCursoEAD_MedAgrupado()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018, 2019 } },
                { (int)Utilidades.ProductGroups.MEDCURSOEAD, new int[] { 2017 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);
            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSOEAD));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
        }


        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MED2018_MEDCURSO2019_MEDMASTER2020_MED2018_2019_2020_MEDCURSO_2019_2020()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));

        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MED2017_2018_MEDCURSO2017_2018_2020_MEDMASTER2020_MED2018_2020_MEDCURSO_2018_2020()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MED, new int[] { 2017, 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2020, 2017} },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));

        }

        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2019_MEDCURSO2018_VISUALIZA_MEDMEDCURSO2020_MED2019_MEDCURSO2019_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDCURSO2019_MED2018_VISUALIZA_MEDMEDCURSO2020_MEDCURSO2019_MED2019_MED2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2019_MEDCURSO2019_VISUALIZA_MEDMEDCURSO2020_MED2019_MEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));

        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2018_MEDCURSO2018_VISUALIZA_MEDMEDCURSO2020_MED2018_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));

        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2018_MEDCURSO2019_VISUALIZA_MEDMEDCURSO2020_MED2019_MEDCURSO2019_MED2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
           
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDCURSO2018_MEDMASTER2020_MEDCURSO2018E2020()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
            Assert.IsFalse(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2019_MEDCURSOANTERIORES_VISUALIZA_MEDMEDCURSO2020_MED2019_MEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2015 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDANTERIORES_MEDCURSO2019_VISUALIZA_MEDMEDCURSO2020_MED2019_MEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2019 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2015 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2018_MEDCURSOANTERIORES_VISUALIZA_MEDMEDCURSO2020_MED2018_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2015 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MEDANTERIORES_MEDCURSO2018_VISUALIZA_MEDMEDCURSO2020_MED2018_MEDCURSO2018()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2018 } },
                { (int)Utilidades.ProductGroups.MED, new int[] { 2015 } },
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO));
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MED));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));
        }
        [Ignore]
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDMASTER2020_MED2018_MEDCURSO2019_MED2015_VISUALIZA_MEDMEDCURSO2020_MED2018_MEDCURSO2018_MEDMEDCURSO2019()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MedMaster, new int[] { 2020 } },

                { (int)Utilidades.ProductGroups.MED, new int[] { 2018 } },

                { (int)Utilidades.ProductGroups.MEDCURSO, new int[] { 2015, 2019 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);

            Assert.IsFalse(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MED).Any(y => y.Value.Contains(2018)));

            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2020)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2019)));
            Assert.IsTrue(produtosAgrupados.Where(x => x.Key == (int)Utilidades.ProductGroups.MEDCURSO).Any(y => y.Value.Contains(2018)));

        }



        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void AgrupaProdutos_MEDELETRO_MEDELETRO()
        {
            var dicionarioProdutos = new Dictionary<int, int[]>
            {
                { (int)Utilidades.ProductGroups.MEDELETRO, new int[] { 2018, 2019 } }
            };

            var produtosAgrupados = new ComboBusiness(new ComboEntity(), new AccessEntity()).AgrupaProdutos(dicionarioProdutos);
            Assert.IsTrue(produtosAgrupados.Any(x => x.Key == (int)Utilidades.ProductGroups.MEDELETRO));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaoAnteriorCPMED_NaoRetornaCPMED()
        {
            var versao = "4.0.0";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);
            
            Assert.IsFalse(combo.Any(c => c.ComboId == (int)Produto.Cursos.CPMED));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoAcademicoVersaoPosteriorCPMED_RetornaCPMED()
        {
            var versao = "4.0.20";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsTrue(combo.Any(c => c.ComboId == (int)Produto.Cursos.CPMED));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoAcademicoCPMED_RetornaCPMEDAnoLancamento()
        {
            var versao = "4.0.20";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo
                .Where(x => x.ComboId == (int)Produto.Cursos.CPMED)
                .Any(y => y.Anos
                .Any(z => z < Utilidades.AnoLancamentoMaterialCPMed)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_RetornaApenasProdutoLiberado()
        {
            var versao = "4.1.3";
            var matricula = 96409;
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoMatricula = 2019;

            var permissoes = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetPermissoes(matricula, idAplicacao);
            var comboPermitido = new ComboEntity().GetCombosPermitidos(permissoes, idAplicacao, null);
            var lstAnoPorProduto = new ComboEntity().GetAnosPorProduto(matricula);
            var comboLiberacao = new List<ProdutoComboLiberadoDTO>();

            var comboMock = Substitute.For<IComboData>();
            comboMock.IsBeforeDataLimite(idAplicacao, anoMatricula).Returns(true);
            comboMock.GetCombosPermitidos(Arg.Any<List<AccessObject>>(), Arg.Any<int>(), Arg.Any<string>()).Returns(comboPermitido);
            comboMock.GetAnosPorProduto(matricula).Returns(lstAnoPorProduto);
            comboMock.GetProdutoComboLiberado(matricula).Returns(comboLiberacao);

            var comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsTrue(comboMsPro.Any());

            comboLiberacao.Add(new ProdutoComboLiberadoDTO { intCurso = (int)Produto.Cursos.MED, intYear = 2019 });
            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsFalse(comboMsPro.Where(c => !comboLiberacao.Select(cl => cl.intCurso).Contains(c.ComboId)).Any());

            comboLiberacao.Add(new ProdutoComboLiberadoDTO { intCurso = (int)Produto.Cursos.MEDCURSO, intYear = 2019 });
            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsFalse(comboMsPro.Where(c => !comboLiberacao.Select(cl => cl.intCurso).Contains(c.ComboId)).Any());
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_RetornaApenasProdutoFakeELiberado()
        {
            var versao = "4.1.3";
            var matricula = 96409;
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoMatricula = 2019;

            var produtoLiberadoFake = new ProdutoComboLiberadoDTO()
            {
                Nome = "Fake",
                intCurso = 999,
                intYear = 2019,
                Ordem = 1,
                ProdutoFake = true
            };

            var produtoLiberadoMED = new ProdutoComboLiberadoDTO()
            {
                Nome = "MED",
                intCurso = 17,
                intYear = 2019,
                Ordem = 2,
                ProdutoFake = false
            };


            var permissoes = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetPermissoes(matricula, idAplicacao);
            var comboPermitido = new ComboEntity().GetCombosPermitidos(permissoes, idAplicacao, null);
            var lstAnoPorProduto = new ComboEntity().GetAnosPorProduto(matricula);
            var comboLiberacao = new List<ProdutoComboLiberadoDTO>();
            comboLiberacao.Add(produtoLiberadoFake);
            comboLiberacao.Add(produtoLiberadoMED);

            var comboMock = Substitute.For<IComboData>();
            comboMock.IsBeforeDataLimite(idAplicacao, anoMatricula).Returns(true);
            comboMock.GetCombosPermitidos(Arg.Any<List<AccessObject>>(), Arg.Any<int>(), Arg.Any<string>()).Returns(comboPermitido);
            comboMock.GetAnosPorProduto(matricula).Returns(lstAnoPorProduto);
            comboMock.GetProdutoComboLiberado(matricula).Returns(comboLiberacao);

            var comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsTrue(comboMsPro.Any());

            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.AreEqual(comboLiberacao.Count(), comboMsPro.Count());

            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsFalse(comboMsPro.Where(c => !comboLiberacao.Select(cl => cl.intCurso).Contains(c.ComboId)).Any());
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_RetornaProdutoFakeAdicionado()
        {
            var versao = "4.1.3";
            var matricula = 96409;
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var anoMatricula = 2019;

            
            var permissoes = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetPermissoes(matricula, idAplicacao);
            var comboPermitido = new ComboEntity().GetCombosPermitidos(permissoes, idAplicacao, null);
            var lstAnoPorProduto = new ComboEntity().GetAnosPorProduto(matricula);

            var produtoLiberado = new ProdutoComboLiberadoDTO()
            {
                Nome = "Fake",
                intCurso = 999,
                intYear = 2019,
                Ordem = 1,
                ProdutoFake = true
            };

            var comboLiberacao = new List<ProdutoComboLiberadoDTO>( );
            comboLiberacao.Add(produtoLiberado);

            var comboMock = Substitute.For<IComboData>();
            comboMock.IsBeforeDataLimite(idAplicacao, anoMatricula).Returns(true);
            comboMock.GetCombosPermitidos(Arg.Any<List<AccessObject>>(), Arg.Any<int>(), Arg.Any<string>()).Returns(comboPermitido);
            comboMock.GetAnosPorProduto(matricula).Returns(lstAnoPorProduto);
            comboMock.GetProdutoComboLiberado(matricula).Returns(comboLiberacao);

            var comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsTrue(comboMsPro.Any());
            
            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsTrue(comboMsPro.Count() > 1);

            comboMsPro = new ComboBusiness(comboMock, new AccessEntity()).GetComboAposLancamentoMsPro(matricula, idAplicacao, versao);
            Assert.IsTrue(comboMsPro.Any(x => x.ComboId == produtoLiberado.intCurso));

        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoExtensivoMEDAnoAnterior_NaoRetornaProdutoAnoAtual()
        {
            var versao = "5.3.0";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.AreEqual(0, combo.Where(c => c.Anos.Any(a => a == anoMatricula + 1)).ToList().Count);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaoAnteriorTrocaLayoutMastologia_MastologiaLayoutWeekSingle()
        {
            var versao = "5.2.0";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK));

        }
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaoAnteriorTVERSAO_APP_TROCA_LAYOUT_AULOESR3R4tWeekSingle()
        {
            var versao = "5.5.1";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Cirurgia && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Clinica && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Pediatria && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.R4GO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK_SINGLE));

            versao = "5.5.2";
            combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Cirurgia && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Clinica && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Pediatria && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.R4GO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.MIXED));

            versao = "";
            combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Cirurgia && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Clinica && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.R3Pediatria && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_SINGLE));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.R4GO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK_SINGLE));

        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoExtensivoMEDEADAnoAnterior_NaoRetornaProdutoAnoAtual()
        {
            var versao = "5.3.0";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.AreEqual(0, combo.Where(c => c.Anos.Any(a => a == anoMatricula + 1)).ToList().Count);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaSuperiorTrocaLayoutMastologia_MastologiaLayoutWeek()
        {
            var versao = "5.2.1";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaoVaziaTrocaLayoutMastologia_MastologiaLayoutWeekSingle()
        {
            var versao = "";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.MASTO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoExtensivoMEDCURSOAnoAnterior_NaoRetornaProdutoAnoAtual()
        {
            var versao = "5.3.0";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.AreEqual(0, combo.Where(c => c.Anos.Any(a => a == anoMatricula + 1)).ToList().Count);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaoAnteriorTrocaLayoutTego_TegoLayoutWeekSingle()
        {
            var versao = "5.2.0";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);
        
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoExtensivoMEDCURSOEADAnoAnterior_NaoRetornaProdutoAnoAtual()
        {
            var versao = "5.3.0";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSOEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.AreEqual(0, combo.Where(c => c.Anos.Any(a => a == anoMatricula + 1)).ToList().Count);
        }

        //-------------------------------------

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnoAtualMobile_RetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnoAtualDesktop_RetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnoAtualMobile_RetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnoAtualDesktop_RetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }


        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnteriorMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear()- 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnoAnteriorDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnteriorMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnoAnteriorDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear() - 1;

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }


        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnoAtualMobile_RetornaProdutoAulaEspecialLayoutMixed()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsTrue(combo.FirstOrDefault(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS).tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnoAtualMobile_RetornaProdutoAulaEspecialLayoutMixed()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsTrue(combo.FirstOrDefault(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS).tipoLayoutMain == (int)TipoLayoutMainMSPro.MIXED);
        }



        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedAtivoAnoAtualDesktop_RetornaProdutoAulaEspecialLayoutSemanaDupla()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsTrue(combo.FirstOrDefault(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS).tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_DOUBLE);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        [Ignore]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoAtivoAnoAtualDesktop_RetornaProdutoAulaEspecialLayoutSemanaDupla()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsTrue(combo.FirstOrDefault(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS).tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK_DOUBLE);
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCanceladoAnoAtualMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada, excluiMeioDeAno:true).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCanceladoAnoAtualDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada, excluiMeioDeAno:true ).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoCanceladoAnoAtualMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada, true, excluiMeioDeAno:true).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoCanceladoAnoAtualDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSO, (int)OrdemVenda.StatusOv.Cancelada, (int)OrdemVenda.StatusOv.Cancelada, true, excluiMeioDeAno:true).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoEADAtivoAnoAtualMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSOEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoEADAtivoAnoAtualDesktop_RetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDCURSOEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedEADAtivoAnoAtualMobile_RetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedEADAtivoAnoAtualDesktop_RetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(anoMatricula, (int)Produto.Produtos.MEDEAD, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente).ID;

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(matricula, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        //---
        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedBloqueadoAnoAtualMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunosInadimplenteBloqueado((int)Produto.Produtos.MED).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoBloqueadoAnoAtualMobile_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunosInadimplenteBloqueado((int)Produto.Produtos.MEDCURSO).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedBloqueadoAnoAtualDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunosInadimplenteBloqueado((int)Produto.Produtos.MED).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoMedCursoBloqueadoAnoAtualDesktop_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "2.0.22";
            var anoMatricula = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunosInadimplenteBloqueado((int)Produto.Produtos.MEDCURSO).FirstOrDefault();

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 23754; // 2020 MED MEDICINE ITUVERAVA SEGUNDA

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, courseId: turmaid);

            if (aluno.ID == 0)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma23756_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 23756; // 2020 MEDCURSO MEDICINE ITUVERAVA

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, status2:OrdemVenda.StatusOv.Adimplente, courseId: turmaid);

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma18141_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 18141; // 2020 MEDCURSO MEDICINE GURUPI SÁBADO

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, status2: OrdemVenda.StatusOv.Adimplente, courseId: turmaid);

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma22748_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 22748; //2020 MED MEDICINE GURUPI QUINTA

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, status2: OrdemVenda.StatusOv.Adimplente, courseId: turmaid);

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma18296_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 18296; // 2020 MED MEDICINE JATAÍ QUARTA

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, status2: OrdemVenda.StatusOv.Adimplente, courseId: turmaid);

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MED_AULAS_ESPECIAIS));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetComboAposLancamentoMsPro_AlunoTurma22755_NaoRetornaProdutoAulaEspecial()
        {
            var versao = "5.4.0";
            var anoMatricula = Utilidades.GetYear();
            var turmaid = 22755; //2020 MEDCURSO MEDICINE JATAÍ TERÇA

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoMatricula, status2: OrdemVenda.StatusOv.Adimplente, courseId: turmaid);

            if (aluno == null)
                Assert.Inconclusive();

            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsPro(aluno.ID, (int)Aplicacoes.MsProMobile, versao);
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS));
        }



        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetCombo_AlunoAcademicoVersaSuperiorTrocaLayoutTego_TegoLayoutWeek()
        {
            var versao = "5.2.1";
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetCombo(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MsProMobile, versao);

            Assert.IsTrue(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain == (int)TipoLayoutMainMSPro.WEEK));
            Assert.IsFalse(combo.Any(x => x.ComboId == (int)Produto.Cursos.TEGO && x.tipoLayoutMain != (int)TipoLayoutMainMSPro.WEEK));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetAnosPorProduto_AlunoCPMedExtensivoAnoAtual_VisualizaCPMedExtensivoAnoAtual()
        {
            var anoAtual = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.CPMED_EXTENSIVO, OrdemVenda.StatusOv.Adimplente).FirstOrDefault();

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var retorno = new ComboEntity().GetAnosPorProduto(aluno.ID);
            Assert.IsTrue(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED_EXTENSIVO));

            var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED_EXTENSIVO).Select(x => x.Value).ToList();
            Assert.IsTrue(anosProduto.Any(a => a.Contains(anoAtual)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetAnosPorProduto_AlunoCPMedAnoAnterior_VisualizaCPMedAnoAnterior()
        {
            var anoAnterior = Utilidades.GetYear() - 1;

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoAnterior, Produto.Produtos.CPMED, OrdemVenda.StatusOv.Adimplente);

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var retorno = new ComboEntity().GetAnosPorProduto(aluno.ID);
            Assert.IsTrue(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED));

            var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED).Select(x => x.Value).ToList();
            Assert.IsTrue(anosProduto.Any(a => a.Contains(anoAnterior)));
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetAnosPorProduto_AlunoCPMedPremiumAnoAtual_VisualizaCPMedPremiumAnoAtualAposInicioTurma()
        {
            var anoAtual = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoAtual, Produto.Produtos.CPMED, OrdemVenda.StatusOv.Adimplente);

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var retorno = new ComboEntity().GetAnosPorProduto(aluno.ID);

            if (aluno.Turma.Inicio < DateTime.Now)
            {
                Assert.IsTrue(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED));

                var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED).Select(x => x.Value).ToList();

                Assert.IsTrue(anosProduto.Any(a => a.Contains(anoAtual)));
            }
            else if (retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED))
            {
                var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED).Select(x => x.Value).ToList();
                Assert.IsFalse(anosProduto.Any(a => a.Contains(anoAtual)));
            }
            else
            {
                Assert.IsFalse(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED));
            }
        }

        [TestMethod]
        [TestCategory("Combo_Produtos")]
        public void GetAnosPorProduto_AlunoCPMedExpressoAnoAtual_VisualizaCPMedExpressomAnoAtualAposInicioTurma()
        {
            var anoAtual = Utilidades.GetYear();

            var aluno = new PerfilAlunoEntityTestData().GetAlunoStatus2ComTurma(anoAtual, Produto.Produtos.CPMED_EXPRESSO, OrdemVenda.StatusOv.Adimplente);

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno nesse cenário");
            }

            var retorno = new ComboEntity().GetAnosPorProduto(aluno.ID);

            if (aluno.Turma.Inicio < DateTime.Now)
            {
                Assert.IsTrue(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED_EXPRESSO));

                var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED_EXPRESSO).Select(x => x.Value).ToList();

                Assert.IsTrue(anosProduto.Any(a => a.Contains(anoAtual)));
            }
            else if (retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED_EXPRESSO))
            {
                var anosProduto = retorno.Where(p => p.Key == (int)Produto.Produtos.CPMED_EXPRESSO).Select(x => x.Value).ToList();
                Assert.IsFalse(anosProduto.Any(a => a.Contains(anoAtual)));
            }
            else
            {
                Assert.IsFalse(retorno.Any(p => p.Key == (int)Produto.Produtos.CPMED_EXPRESSO));
            }
        }

        [TestMethod]
        [TestCategory("Combo_Produtos_Declaracao_Matricula")]
        public void ComboProdutos_GetDeclaracaoMatriculaComboProdutos_RetornaCombor()
        {
            var combo = new ComboBusiness(new ComboEntity(), new AccessEntity()).GetDeclaracaoMatriculaComboProdutos(Convert.ToInt32(96409)); //227292
            var res = combo.FirstOrDefault();
            var idsEntesivo = res.ListaProdutos.Where(a => a.Descricao == "extensivo").ToList().Select(b => b.ID).First();
            var extensivo = new List<int>() { (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MED, };
            var medcursoExtensivo = new List<int>() { (int)Produto.Produtos.MEDCURSO };
            var medExtensivo = new List<int>() { (int)Produto.Produtos.MED };
            Assert.IsTrue(idsEntesivo.Count == extensivo.Count || idsEntesivo.First() == medcursoExtensivo.First() || idsEntesivo.First() == medExtensivo.First());
        }
    }
}