using System.Linq;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class AulaEntityTests
    {
        [TestMethod]
        [TestCategory("Videos_Aula")]
        public void Can_GetVideosTemaMedcurso()
        {
            var alunoMedcursoAnoAtualAtivo = new PerfilAlunoEntityTestData().GetAlunoMedApenasAno2019();

            var aulas = new MednetEntity().GetVideoAulas(16, 2142, 0, alunoMedcursoAnoAtualAtivo, 17);

            Assert.IsNotNull(aulas.Apostila);
            Assert.IsFalse(string.IsNullOrEmpty(aulas.Descricao));

            var primeiraAulaRevisao = aulas.VideoAulas.FirstOrDefault(x => x.TipoAula == ETipoVideo.Revisao);

            Assert.IsNotNull(primeiraAulaRevisao);
            Assert.IsTrue(primeiraAulaRevisao.Professores.Any());
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSemanas_ParametrosDefault_NaoRetornaNulo()
        {
            var semanas = new AulaEntity().GetSemanas(0, 73, 96409, Semana.TipoAba.Aulas);

            Assert.IsNotNull(semanas);
        }

        [TestMethod]
        [TestCategory("Videos_Aula")]
        public void Cant_GetVideosTemaMedcurso_AlunoSomenteMed()
        {
            var aulas = new MednetEntity().GetVideoAulas(16, 2145, 0, 46604, 17);

            Assert.IsNull(aulas.Apostila);
            Assert.IsTrue(string.IsNullOrEmpty(aulas.Descricao));
            Assert.IsFalse(aulas.VideoAulas.Any());
        }

        [TestMethod]
        [TestCategory("Progresso_Aulas")]
        public void Can_GetProgressoGeralIgualEspecifico()
        {
            var testData = new PerfilAlunoEntityTestData();            
            var matricula = 244788; // matricula ano atual 
            var anoAtual = 2019; //Ano atual vigente
            var temaId = 2226; //utilizar o temaId com maior progresso retornado pelo método GetProgressoAulas abaixo (debuggar)
            var apostilaId = 0;

            var progressosAluno = new MednetEntity().GetProgressoAulas(matricula, anoAtual, (int)Produto.Cursos.MED); 
            var aulas = new MednetEntity().GetVideoAulas((int)Produto.Cursos.MED, temaId, apostilaId, matricula, (int)Aplicacoes.MsProMobile);

            var percentCir1Main = progressosAluno.Where(x => x.IdEntidade == temaId).Select(x => x.PercentLido).FirstOrDefault();
            var videoAulasRevisao = aulas.VideoAulas.Where(x => x.TipoAula == ETipoVideo.Revisao).FirstOrDefault();
            var professorComMaiorProgresso = videoAulasRevisao.Professores.OrderByDescending(x => x.PercentVisualizado).FirstOrDefault();

            var percent = professorComMaiorProgresso.PercentVisualizado;

            Assert.AreEqual(percent, percentCir1Main);
        }
    }
}