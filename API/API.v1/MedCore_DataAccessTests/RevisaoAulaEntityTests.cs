using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class RevisaoAulaEntityTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_SetAvaliacao_TemaExistente()
        {
            var avaliacao = new AulaAvaliacaoPost { AlunoID = 119300, ApostilaID = 6325, Nota = 2, ProfessorID = 90918 };
            var retorno = new MednetEntity().SetAvaliacaoTemaVideoRevisao(avaliacao);
            Assert.IsNotNull(retorno);
            Assert.IsInstanceOfType(retorno, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_SetAvaliacao_TemaInexistente()
        {
            var avaliacao = new AulaAvaliacaoPost
            {
                AlunoID = 119300,
                Nota = 2,
                ProfessorID = 90918,
                ApplicationID = 1,
                IdRevisaoIndice = 1,
                Observacao = "Teste unitário de de observação",
            };
            var retorno = new MednetEntity().SetAvaliacaoTemaVideoRevisao(avaliacao);
            Assert.IsNotNull(retorno);
            Assert.IsInstanceOfType(retorno, typeof(int));
        }
    }
}