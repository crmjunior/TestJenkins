using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class AulaAvaliacaoEntityTests
    {

        #region basico

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoNotNull()
        {
            int alunoID = 96409;
            int apostilaID = 7805;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(alunoID, apostilaID);

            Assert.IsNotNull(aulaAvaliacao);
            Assert.IsInstanceOfType(aulaAvaliacao, typeof(AulaAvaliacao));
            Assert.AreNotEqual(aulaAvaliacao.Temas.Count, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetSlidesBaixadosNotNull()
        {
            int alunoID = new PerfilAlunoEntityTestData().GetAlunoAcademico().ID;
            //int apostilaID = 9474;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetSlidesPermitidos(alunoID).First();

            Assert.IsNotNull(aulaAvaliacao);
            Assert.IsInstanceOfType(aulaAvaliacao, typeof(AulaAvaliacao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacao_TemaComProfessorSubstituto()
        {
            int alunoID = 227546;
            int apostilaID = 13648;
            int temaID = 3229;
            //int produtoID = 16;
            //int grandeAreaID = 12; 
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(alunoID, apostilaID);
            // var apostila = aulaAvaliacao.Where(x => x.ID == apostilaID).FirstOrDefault();
            var tema = aulaAvaliacao.Temas.Where(x => x.TemaID == temaID).FirstOrDefault();

            Assert.IsFalse(tema.PodeAvaliar);
            Assert.AreEqual(121433, tema.ProfessorID);
        }

        #endregion basico

        #region CPMED

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAulaAvaliacao_ApostilaTP_AulasTP()
        {
            if (DateTime.Now.Month < 9)
                Assert.Inconclusive();
            int alunoID = 96409;
            int apostilaID = 11662;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(alunoID, apostilaID);

            Assert.IsTrue(aulaAvaliacao.Temas.FirstOrDefault().Nome.Contains("Med TP"));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAulaAvaliacao_AlunoSemDireitoApostilaTP_Vazio()
        {
            int matriculaAlunoSemDireito = 13223;
            int apostilaID = 11662;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaAlunoSemDireito, apostilaID);

            Assert.IsTrue(aulaAvaliacao.IdApostila == 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAulaAvaliacao_AlunoSemTodasAulasTPAssistidas_AulasTP()
        {
            if (DateTime.Now.Month < 9)
                Assert.Inconclusive();

            int matriculaAlunoSemTodasAulasTPAssistidas = 223055;
            int apostilaID = 11662;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaAlunoSemTodasAulasTPAssistidas, apostilaID);
            var quantidadeTemasAssistidos = aulaAvaliacao.Temas.Count;
            var qta = quantidadeTemasAssistidos;
            Assert.IsTrue(qta >= 2 && qta < 8);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAulaAvaliacao_AlunoComTodasAulasTPAssistidas_AulasTP()
        {
            if (DateTime.Now.Month < 9)
                Assert.Inconclusive();
            int matriculaAlunoComTodasAulasTPAssistidas = 96409;
            int apostilaID = 11662;
            var aulaAvaliacao = new AulaAvaliacao();
            aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaAlunoComTodasAulasTPAssistidas, apostilaID);
            var quantidadeTemasAssistidos = aulaAvaliacao.Temas.Count;
            var qta = quantidadeTemasAssistidos;
            Assert.IsTrue(qta == 8);
        }

        #endregion CPMED

        #region Presencial

        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Permitir_AlunoPresencial_Presente_AvaliarAula()
        {
            //Necessario criar consulta para recuperar avaliacao de aula que aconteceu a menos de 11 meses de diferença da data atual
            if (DateTime.Now <= Utilidades.DataToleranciaTestes())
            {
                Assert.Inconclusive();
            }
            var matriculaPresencial = 247187;
            var apostila = 15660;

            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaPresencial, apostila);

            Assert.AreNotEqual(avaliacao.Temas.Count, 0); // Retorna temas da Apostila
            Assert.IsTrue(avaliacao.Temas.First().PodeAvaliar); //Permitido Avaliar
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Impedir_AlunoPresencial_AvaliarAula_AntesDaData()
        {
            var matriculaPresencial = 241308;
            var apostila = 13651; //2017 CIR 4
            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaPresencial, apostila);
            Assert.AreEqual(avaliacao.Temas.Count, 0); //Não Retorna temas da Apostila

        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Impedir_AlunoPresencial_Ausente_AvaliarAula()
        {
            var matriculaPresencialAusente = 241308;
            var apostila = 13639;
            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaPresencialAusente, apostila);
            Assert.AreEqual(avaliacao.Temas.Count, 0); // Não Retorna temas da Apostila
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Basico")]
        public void Deve_Permitir_AlunoPresencial_EmOutraFilial_AvaliarAula()
        {
            var dataAtual = DateTime.Now;
            var dataCarenciaTeste = new DateTime(2018, 04, 05);
            if (dataAtual > dataCarenciaTeste)
            {
                #region QuerySQL para consultar matricula e apostila para passar neste teste
                //select DISTINCT top 1 so.intClientID, lm.intMaterialID,convert(varchar(11),mc.dteDateTime,121) from tblSellOrders so 
                //inner join tblSellOrderDetails sod on so.intOrderID = sod.intOrderID
                //inner join tblCourses c on sod.intProductID = c.intCourseID
                //inner join mview_Cronograma mc on c.intCourseID = mc.intCourseID
                //inner join tblLesson_Material lm on mc.intLessonID = lm.intLessonID
                //inner join tblCallCenterCalls cc on cc.intClientID = so.intClientID
                //inner join tblLessonTeachersByGroupAndTitle lt on mc.intLessonTitleID = lt.intLessonTitleID 
                //where c.intYear = 2017 and so.intStatus = 2 and
                //c.intPrincipalClassRoomID != (select top 1 l.intClassroomID from tblAccessLog l where intPeopleID = so.intClientID and l.intClassroomID != c.intPrincipalClassRoomID
                //and not exists (select * from tblLessonsEvaluation ev where ev.intClientID = so.intClientID and ev.intLessonID = mc.intLessonID)
                //order by l.dteDateTime desc)
                //and mc.dteDateTime < getdate()
                //and (cc.intCallCategoryID in (1845, 1844 ) and cc.intStatusID != 7
                //and convert(varchar(11),mc.dteDateTime,121)>=convert(varchar(11),cc.dteDataPrevisao1,121)
                //and convert(varchar(11),mc.dteDateTime,121)<=convert(varchar(11),cc.dteDataPrevisao2,121)
                //)
                //and year(cc.dteOpen)=2017

                #endregion


                var matriculaPresencialOutraFilial = 45494;
                var apostila = 13761;
                var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaPresencialOutraFilial, apostila);
                Assert.AreNotEqual(avaliacao.Temas.Count, 0); // Retorna temas da Apostila
                Assert.IsTrue(avaliacao.Temas.First().PodeAvaliar); //Permitido Avaliar
            }
            else
            {
                Assert.Inconclusive();
            }

        }

        #endregion Presencial

        #region EAD

        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Permitir_AlunoEAD_AvaliarAula()
        {
            if (DateTime.Now.Month == 1 &&  DateTime.Now.Day < 21)
            {
                Assert.Inconclusive("Este Teste não deve rodar antes de aproximadamente 20 de janeiro, para garantir q material já foi retirado pelo aluno - Regra Tardivo");
            }

            var alunos = new PerfilAlunoEntityTestData().GetAlunosStatus2(Produto.Produtos.MEDEAD, OrdemVenda.StatusOv.Adimplente);

            if (alunos.Count() == 0)
            {
                Assert.Inconclusive("Ainda não possui aluno nesse cenário");
            }

            var matriculaEAD = alunos.FirstOrDefault().ID;

            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var apostila = bus.GetCronogramaAluno((int)Produto.Cursos.MED, 0, 0, matriculaEAD).Semanas.FirstOrDefault().Apostilas.FirstOrDefault().MaterialId;

            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaEAD, apostila);
            Assert.AreNotEqual(avaliacao.Temas.Count, 0); // Retorna temas da Apostila
            Assert.IsTrue(avaliacao.Temas.First().PodeAvaliar); //Permitido Avaliar
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Impedir_AlunoEAD_AvaliarAula_AntesDaData()
        {
            if (DateTime.Now.Month == 12)
            {
                Assert.Inconclusive();
            }
            var matriculaEAD = 252633;
            var apostila = 13803; //2017 PSI UNICO
            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaEAD, apostila);
            Assert.AreEqual(avaliacao.Temas.Count, 0); // Não Retorna temas da Apostila
        }

        [Ignore]//IGNORAR ATÉ O AJUSTE DA PROPRIEDADE PODEAVALIAR NO MEDCODE SER FEITA
        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Impedir_Aluno_AvaliarSegundoTema_AntesDaData()
        {
            var matricula = 237056;
            var apostila = 13670;
            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matricula, apostila);
            Assert.AreNotEqual(avaliacao.Temas.Count, 0); // Retorna temas da Apostila
            Assert.IsFalse(avaliacao.Temas[1].PodeAvaliar); //Impedindo Avaliar apostila do Segundo Tema quando a do primeiro está permitida
        }


        #endregion EAD

        #region troca
        [TestMethod]
        [TestCategory("Basico")]
        public void Deve_Permitir_AlunoTroca_AvaliarAula()
        {
            var matriculaTroca = 225367;
            var apostila = 13787;
            var avaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacao(matriculaTroca, apostila);
            Assert.AreNotEqual(avaliacao.Temas.Count, 0); // Retorna temas da Apostila
                                                          //Assert.IsTrue(avaliacao.Temas.First().PodeAvaliar); //Impedindo Avaliar apostila do Segundo Tema quando a do primeiro está permitida


        }
        #endregion troca

        #region aulaPorAluno

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoPorAlunoNotNull()
        {
            int alunoID = 116502;
            int produtoID = 16;
            int grandeAreaID = 11;
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID, produtoID, grandeAreaID);

            Assert.IsNotNull(aulaAvaliacao);
            Assert.IsInstanceOfType(aulaAvaliacao, typeof(List<AulaAvaliacao>));
            Assert.AreNotEqual(aulaAvaliacao.Select(x => x.Temas.Count), 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoPorAlunoSemPresenca()
        {
            int alunoID = 241740; //necessário id sem aulas assistidas
            int produtoID = 16;
            int grandeAreaID = 11;
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID, produtoID, grandeAreaID);

            Assert.IsTrue(aulaAvaliacao.Count() == 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoPorAlunoTemaAvaliado()
        {
            int alunoID = 116502;
            int apostilaID = 13640;
            int temaID = 3279;
            int produtoID = 16;
            int grandeAreaID = 11;
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID, produtoID, grandeAreaID);
            var apostila = aulaAvaliacao.Where(x => x.ID == apostilaID).FirstOrDefault();
            var tema = apostila.Temas.Where(x => x.TemaID == temaID).FirstOrDefault();

            Assert.IsTrue(tema.IsAvaliado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoPorAlunoTemaNaoAvaliado()
        {
            int alunoID = 116502;
            int apostilaID = 13641;
            int temaID = 3283;
            int produtoID = 16;
            int grandeAreaID = 11;
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID, produtoID, grandeAreaID);
            var apostila = aulaAvaliacao.Where(x => x.ID == apostilaID).FirstOrDefault();
            var tema = apostila.Temas.Where(x => x.TemaID == temaID).FirstOrDefault();

            Assert.IsFalse(tema.IsAvaliado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAulaAvaliacaoPorAlunoTemaComProfessorSubstituto()
        {
            int alunoID = 227546;
            int apostilaID = 13648;
            int temaID = 3229;
            int produtoID = 16;
            int grandeAreaID = 12;
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID, produtoID, grandeAreaID);
            var apostila = aulaAvaliacao.Where(x => x.ID == apostilaID).FirstOrDefault();
            var tema = apostila.Temas.Where(x => x.TemaID == temaID).FirstOrDefault();

            Assert.IsFalse(tema.PodeAvaliar);
            Assert.AreEqual(121433, tema.ProfessorID);
        }

        [TestMethod]
        [TestCategory("Aulas Avaliadas")]
        public void GetAulaAvaliacaoPorAluno_RetornaAulasAvaliacao()
        {
            int alunoID = new PerfilAlunoEntityTestData().GetAlunoAnoAtualComAnosAnteriores();
            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID);

            Assert.AreNotEqual(aulaAvaliacao.Count(), 0);
        }

        [TestMethod]
        [TestCategory("Aulas Avaliadas")]
        public void GetAulaAvaliacaoPorAluno_AlunoComRegraExcecao_RetornaAulasAvaliacao()
        {
            int alunoID = new PerfilAlunoEntityTestData().GetAlunoComRegraExcecaoSlideAulas();

            if (alunoID == 0) Assert.Inconclusive("Não foi possível encontrar aluno nesse perfil");

            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID);

            Assert.AreNotEqual(aulaAvaliacao.Count(), 0);
            Assert.IsFalse(aulaAvaliacao.Any(x => x.Ano < (int)Utilidades.AnoLancamentoMedsoftPro));
        }

        [TestMethod]
        [TestCategory("Aulas Avaliadas")]
        public void GetAulaAvaliacaoPorAluno_AlunoSemRegraExcecao_RetornaAulasAvaliacao()
        {
            int alunoID = Constants.CONTACTID_ACADEMICO;

            var aulaAvaliacao = new AulaAvaliacaoEntity().GetAulaAvaliacaoPorAluno(alunoID);

            Assert.AreNotEqual(aulaAvaliacao.Count(), 0);
            Assert.IsTrue(aulaAvaliacao.Any(x => x.Ano < (int)Utilidades.AnoLancamentoMedsoftPro));
        }

        [TestMethod]
        [TestCategory("Avaliação de aula")]
        public void GetProfessorAula_retornaProfessor()
        {
            var professor = 76975; 
            var aula = 535650;

            var retorno = new AulaAvaliacaoEntity().GetProfessorAula(aula);

            Assert.IsTrue(professor == retorno);
        }

        [TestMethod]
        [TestCategory("Avaliação de aula")]
        public void GetProfessorAula_retornaProfessorSubstituto()
        {
            var professor = 76975;
            var professorSubstituto = 121433;
            var aula = 601565;

            var retorno = new AulaAvaliacaoEntity().GetProfessorAula(aula);

            Assert.IsFalse(professor == retorno);
            Assert.IsTrue(professorSubstituto == retorno);
        }


        #endregion aulaPorAluno

        public void GetAvaliacaoAulaSlides_AlunoAcademico_MEDContemVideos()
        {
            var bus = new AulaAvaliacaoBusiness(new AulaAvaliacaoEntity(), new VideoEntity());
            var aulaSlides = bus.GetAvaliacaoAulaSlides(Constants.CONTACTID_ACADEMICO);
            var aulasSlidesMed = aulaSlides.Where(x => x.IdProduto == (int)Produto.Produtos.MED).ToList();

            Assert.IsTrue(aulasSlidesMed.Any(x => x.Temas.Any(y => y.Slides.Any(z => z.Tipo == (int)AulaAvaliacaoBusiness.ETipoMidiaConteudoAvaliacao.Video))));
        }

        public void GetAvaliacaoAulaSlides_AlunoAcademico_MEDCURSONaoContemVideos()
        {
            var bus = new AulaAvaliacaoBusiness(new AulaAvaliacaoEntity(), new VideoEntity());
            var aulaSlides = bus.GetAvaliacaoAulaSlides(Constants.CONTACTID_ACADEMICO);
            var aulasSlidesMed = aulaSlides.Where(x => x.IdProduto == (int)Produto.Produtos.MEDCURSO).ToList();

            Assert.IsFalse(aulasSlidesMed.Any(x => x.Temas.Any(y => y.Slides.Any(z => z.Tipo == (int)AulaAvaliacaoBusiness.ETipoMidiaConteudoAvaliacao.Video))));
        }

        public void GetAvaliacaoAulaSlides_AlunoAcademico_VideosContemPropriedades()
        {
            var bus = new AulaAvaliacaoBusiness(new AulaAvaliacaoEntity(), new VideoEntity());
            var aulaSlides = bus.GetAvaliacaoAulaSlides(Constants.CONTACTID_ACADEMICO);
            var slides = aulaSlides.SelectMany(x => x.Temas).SelectMany(y => y.Slides ).Where(z => z.Tipo == (int)AulaAvaliacaoBusiness.ETipoMidiaConteudoAvaliacao.Video).ToList();

            Assert.IsTrue(slides.All(x => x.Conteudo.All(y => !string.IsNullOrEmpty(y.Link))));
            Assert.IsTrue(slides.All(x => x.Conteudo.All(y => !string.IsNullOrEmpty(y.Qualidade))));
            Assert.IsTrue(slides.All(x => x.Conteudo.All(y => y.Qualidade != "hls" &&  y.Altura > 0 )));
            Assert.IsTrue(slides.All(x => x.Conteudo.All(y => y.Qualidade != "hls" && y.Largura > 0 )));

        }
    }
}