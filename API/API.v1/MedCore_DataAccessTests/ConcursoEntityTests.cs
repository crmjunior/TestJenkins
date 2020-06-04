using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class ConcursoEntityTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetConcursosSiglas_NotNull()
        {
            var concursos = new ConcursoEntity().GetConcursosSiglas();
            Assert.IsNotNull(concursos);
            Assert.IsInstanceOfType(concursos, typeof(List<Concurso>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetConcursosSiglas_WithCorrectValues()
        {
            var concursos = new ConcursoEntity().GetConcursosSiglas();
            Assert.IsNotNull(concursos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ConcursoAllMedsoft_AlunoAdimplente_NotNull()
        {
            var concursos = new ConcursoEntity().GetAll(119300, 16);
            Assert.IsNotNull(concursos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ConcursoAllMedsoft_AlunoAdimplente_TemConcursos()
        {
            var concursos = new ConcursoEntity().GetAll(119300, 16);
            Assert.IsTrue(concursos.Count > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ConcursoAllMedsoft_AlunoAdimplente_VeConcursosAnoAtual()
        {
            var concursos = new ConcursoEntity().GetAll(119300, 16);
            var temAnoAtual = concursos.FindAll(c => c.Ano == DateTime.Now.Year);
            Assert.IsTrue(concursos.Count > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetProvas_TextoComCaracterEspaco_ListaProvas()
        {
            var provas = new ConcursoEntity().GetProvas("HAC - PR", 119300);
            Assert.IsNotNull(provas);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetProvas_TextoSemCaracterEspaco_ListaProvas()
        {
            var provas = new ConcursoEntity().GetProvas("ufrj", 119300);
            Assert.IsNotNull(provas);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetProvas_TextoVazio_ListaObjetoVazio()
        {
            var provas = new ConcursoEntity().GetProvas("", 119300);
            Assert.IsNotNull(provas);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetGetConcursoStatsAluno_AlunoInexistente()
        {
            var result = new ConcursoEntity().GetConcursoStatsAluno(923250, 9999999);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetGetConcursoStatsAluno_ExercicioInexistente()
        {
            var result = new ConcursoEntity().GetConcursoStatsAluno(0, 227162);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetGetConcursoStatsAluno_IniciadoSemRespostas()
        {
            var result = new ConcursoEntity().GetConcursoStatsAluno(923250, 227162);
            Assert.IsTrue(result.TotalQuestoes > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetGetConcursoStatsAluno_IniciadoComRespostas()
        {
            var result = new ConcursoEntity().GetConcursoStatsAluno(922423, 227151);
            Assert.IsTrue(result.TotalQuestoes > 0);
        }
        [TestMethod]
        [TestCategory("Basico")]
        public void GetConcursoStatsAluno_CalcularAcertosErrosNaoRealizadasTotalQuestoes()
        {
            int matricula = new PerfilAlunoEntityTestData().GetAlunoAcademico().ID;
            int exerecicioID = new ConcursoEntity().GetExercicioIDPorMatricula(matricula);

            var result = new ConcursoEntity().GetConcursoStatsAluno(exerecicioID, matricula);
               Assert.IsTrue(result.TotalQuestoes == (result.Acertos + result.Erros + result.NaoRealizadas)
                           && result.Acertos == (result.TotalQuestoes - result.Erros - result.NaoRealizadas)
                           && result.Erros == (result.TotalQuestoes - result.Acertos - result.NaoRealizadas)
                           && result.NaoRealizadas == (result.TotalQuestoes - result.Acertos - result.Erros)
           );
           
            
        }
        

        [TestMethod]
        [TestCategory("Basico")]
        public void GetIsProvaDiscursiva_ProvaDiscursiva()
        {
            var result = new ConcursoEntity().PermiteNotadeCorte(876316);
            Assert.AreEqual(result, 0);
        }
        [TestMethod]
        [TestCategory("Basico")]
        public void GetConcurso_NaoRetornarNomeComEspacoInicioFim()
        {
            var matricula = new PerfilAlunoEntityTestData().GetAlunoAcademico().ID;
            var concursos = new ConcursoEntity().GetAll(matricula, (int)Aplicacoes.MsProMobile);
            var litaconcurso = concursos.Where(a => a.ExercicioName.StartsWith(" ") || a.ExercicioName.EndsWith(" ")).ToList();
            Assert.IsTrue(litaconcurso.Count() == 0);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetIsProvaDiscursiva_ProvaObjetiva()
        {
            var result = new ConcursoEntity().PermiteNotadeCorte(923629);
            Assert.AreEqual(result, 1);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void CanInsert_Atualizacao_ProvaSobDemanda()
        {
            var pr = new Prova
            {
                ID = 923674,
                SobDemanda = true
            };

            var ret = new ConcursoEntity().SetProvaSobDemanda(pr);

            Assert.IsTrue(ret);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Get_ProvaSobDemanda()
        {
            var idProva = 894592;

            var ret = new ConcursoEntity().GetProvaSobDemanda(idProva);

            Assert.IsFalse(ret);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAlunoR1_GetConcursoR3()
        {
            var idAluno = 241682;
            var alunoR3 = new PerfilAlunoEntity().IsAlunoR3(idAluno);
            var concursosAMP = new ConcursoEntity().GetProvas("AMP", idAluno);
            var concursoR3 = concursosAMP.FirstOrDefault(p => p.TipoConcursoProva.Contains("R3"));
            Assert.IsFalse(alunoR3);
            Assert.IsNull(concursoR3);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAlunoR3_GetConcursoR3()
        {
            var idAluno = new PerfilAlunoEntityTestData().GetAlunoR3().ID; //149114;
            var alunoR3 = new PerfilAlunoEntity().IsAlunoR3(idAluno);
            var concursosAMP = new ConcursoEntity().GetProvas("AMP", idAluno);
            var concursoR3 = concursosAMP.FirstOrDefault(p => p.TipoConcursoProva.Contains("R3"));
            Assert.IsTrue(alunoR3);
            Assert.IsNotNull(concursoR3);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void AlunoR3ComCarencia_GetConcursoR3()
        {
            var alunoR3ComCarencia = new PerfilAlunoEntityTestData().GetAlunoR3ComCarencia();

            if (alunoR3ComCarencia == null)
                Assert.Inconclusive();

            var curso = (int)Produto.Cursos.R3Pediatria;
            var concursos = new ConcursoEntity().GetAll(alunoR3ComCarencia.ID, curso);
            var concursosR3 = concursos.Where(x => x.Tipo == "R3").ToList(); 

            Assert.IsNotNull(concursosR3);
        }


        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoR3Inadimplente_ExibeConcursoR3()
        {
            var alunoR3Inadimplente = new PerfilAlunoEntityTestData().GetAlunoR3Inadimplente();

            if (alunoR3Inadimplente == null)
                Assert.Inconclusive();

            var curso = (int)Produto.Cursos.R3Pediatria;
            var concursos = new ConcursoEntity().GetAll(alunoR3Inadimplente.ID, curso);
            var concursosR3 = concursos.Any(x => x.Tipo == "R3");
            Assert.IsTrue(concursosR3);          
        }

        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoExtensivoTemInteresseR3_ExibeConcursoR3()
        {
            var alunosInteresseR3 = new PerfilAlunoEntityTestData().GetAlunoExtensivoComInteresseR3();

            if (alunosInteresseR3 == null)
                Assert.Inconclusive();

            var codAplicacao = (int)Aplicacoes.MsProMobile;
            var concursos = new ConcursoEntity().GetAll(alunosInteresseR3.FirstOrDefault().ID, codAplicacao);
            var concursosR3 = concursos.Any(x => x.Tipo == "R3");
            Assert.IsTrue(concursosR3);
        }


        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoExtensivoSemInteresseR3_NaoExibeConcursoR3()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }


            var alunosSemInteresseR3 = new PerfilAlunoEntityTestData().GetAlunoExtensivoSemInteresseR3();

            if (alunosSemInteresseR3 == null)
                Assert.Inconclusive();

            var codAplicacao = (int)Aplicacoes.MsProMobile;
            var concursos = new ConcursoEntity().GetAll(alunosSemInteresseR3.FirstOrDefault().ID, codAplicacao);
            var concursosR3 = concursos.Any(x => x.Tipo == "R3");
            Assert.IsFalse(concursosR3);
        }

        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoR3_ExibeConcursoR3()
        {
            var alunoR3 = new PerfilAlunoEntityTestData().GetAlunoR3();

            if (alunoR3 == null)
                Assert.Inconclusive();

            var codAplicacao = (int)Aplicacoes.MsProMobile;
            var concursos = new ConcursoEntity().GetAll(alunoR3.ID, codAplicacao);
            var concursosR3 = concursos.Any(x => x.Tipo == "R3");
            Assert.IsTrue(concursosR3);
        }

        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoRMais_ExibeConcursoR4()
        {
            var alunoRMais = new PerfilAlunoEntityTestData().GetAlunoR3();

            if (alunoRMais == null)
                Assert.Inconclusive();

            var curso = (int)Produto.Cursos.R3Pediatria;
            var concursos = new ConcursoEntity().GetAll(alunoRMais.ID, curso);
            Assert.IsTrue(concursos.Any(x => x.TipoConcursoProva.ToUpper().Contains("MASTOLOGIA")));
            Assert.IsTrue(concursos.Any(x => x.TipoConcursoProva.ToUpper().Contains("GINECOLOGIA E OBSTETRÍCIA")));
            Assert.IsTrue(concursos.Any(x => x.TipoConcursoProva.ToUpper().Contains("MEDICINA FETAL")));

        }

        [TestMethod]
        [TestCategory("Concurso")]
        public void ConcursoGetAll_AlunoAcademico_NaoRetornaConcursoComTipoNull()
        {
            var concursos = new ConcursoEntity().GetAll(Constants.CONTACTID_ACADEMICO, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON); ;
            Assert.IsFalse(concursos.Any(x => x.Tipo == null));
        }
    }
}