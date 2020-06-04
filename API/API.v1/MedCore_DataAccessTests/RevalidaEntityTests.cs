using MedCore_DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class RevalidaEntityTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void GetRevalida_NotNull()
        {
            var revalida = new RevalidaEntity().GetEspecialRevalida(2017, 119300, 17 );
            Assert.IsNotNull(revalida);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetRevalidaTema_NotNull()
        {
            var revalida = new RevalidaEntity().GetTemaRevalida(2017,119300, 17,  1);
            Assert.IsNotNull(revalida);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetRevalidaTema_Tema1()
        {
            var revalida = new RevalidaEntity().GetTemaRevalida(2017, 119300, 17, 1);
            Assert.IsTrue(revalida.IdTema == 1);
        }
    }
}