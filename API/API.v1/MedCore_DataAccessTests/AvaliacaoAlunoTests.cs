using System;
using System.Configuration;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests.EntitiesDataTests
{
    [TestClass]
    public class AvaliacaoAlunoTests
    {
         [TestMethod]
        [TestCategory("Basico")]
        public void CanGetIntervaloValido()
        {
            var entity = new AvaliacaoAlunoEntity();

            var retorno = entity.ValidarIntervalo(GetDataValida());

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetIntervaloInvalido()
        {
            var entity = new AvaliacaoAlunoEntity();

            var retorno = entity.ValidarIntervalo(DateTime.Now);

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAvaliarDataValida()
        {
            var entity = new AvaliacaoAlunoEntity();

            var retorno = entity.CanAvaliar(new tblAvaliacaoAluno()
            {
                bitAvaliarMaisTarde = true,
                dteDataAtualizacao = GetDataValida()
            });

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAvaliarDataInvalida()
        {
            var entity = new AvaliacaoAlunoEntity();

            var retorno = entity.CanAvaliar(new tblAvaliacaoAluno()
            {
                bitAvaliarMaisTarde = true,
                dteDataAtualizacao = DateTime.Now
            });

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAvaliarJaAvaliado()
        {
            var entity = new AvaliacaoAlunoEntity();

            var retorno = entity.CanAvaliar(new tblAvaliacaoAluno()
            {
                bitAvaliarMaisTarde = false,
                dteDataAtualizacao = DateTime.Now
            });

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanAvaliar()
        {
            var entity = new AvaliacaoAlunoEntity();

            var matricula = 96409;
            var retorno = entity.GetAvaliar(matricula);

            Assert.IsTrue(retorno == 1 || retorno == 0);
        }

        private DateTime GetDataValida()
        {
            var intervalo = Convert.ToInt32(ConfigurationProvider.Get("Settings:horasIntervaloAvaliacaoAluno"));

            var dataValida = DateTime.Now.AddDays(-(intervalo + 2));

            return dataValida;
        }
    }
}
