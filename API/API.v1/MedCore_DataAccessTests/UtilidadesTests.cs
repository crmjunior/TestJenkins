using MedCore_DataAccess.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class UtilidadesTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetFormatacaoWithCorrectValues()
        {
            var ret1 = Utilidades.GetDuracaoFormatada("2753.68");
            Assert.AreEqual("00:45:53", ret1);

            var ret2 = Utilidades.GetDuracaoFormatada("7147.3");
            Assert.AreEqual("01:59:07", ret2);

            var ret3 = Utilidades.GetDuracaoFormatada("47.80");
            Assert.AreEqual("00:00:47", ret3);

            var ret4 = Utilidades.GetDuracaoFormatada("65.80");
            Assert.AreEqual("00:01:05", ret4);

            var ret5 = Utilidades.GetDuracaoFormatada("");
            Assert.AreEqual("00:00:00", ret5);

            var ret6 = Utilidades.GetDuracaoFormatada(null);
            Assert.AreEqual("00:00:00", ret6);

            var retSum = Utilidades.GetDuracaoFormatada("11803.63");
        }
    }
}