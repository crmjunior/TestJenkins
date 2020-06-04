using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class AlunoEntityTests
    {
        [TestMethod]
        [TestCategory("Permissoes")]
        public void GetPermissaoAcesso_AlunoMedsoftDesktop_RetornaTrue()
        {
            var app = Aplicacoes.MEDSOFT_PRO_ELECTRON;
            var device = Utilidades.TipoDevice.windows;
            var alunoDesktop = new PerfilAlunoEntityTestData().GetAcessoAplicacao(app, device);
            if (alunoDesktop == null) Assert.Inconclusive();

            var permissao = new AlunoEntity().GetPermissaoAcesso((int)app, alunoDesktop.intClientId, alunoDesktop.txtDeviceToken, (Utilidades.TipoDevice)alunoDesktop.intApplicationId);

            Assert.AreEqual(1, permissao.PermiteAcesso);
            Assert.AreEqual(0, permissao.PermiteTroca);
        }

        // [TestMethod]
        // [TestCategory("Dispositivo")]
        // public void AutorizaNovoDispositivo_TestaNovoDevice_StatusNovo()
        // {
        //     var alunoMock = Substitute.For<IAlunoEntity>();
        //     var matricula = 10;
        //     var token = "tokenTeste";

        //     alunoMock.GetPermissaoAcesso(
        //         (int)Aplicacoes.Recursos, matricula, token, Utilidades.TipoDevice.iosMobile
        //         ).Returns(new PermissaoDevice { PermiteAcesso = 1, PermiteTroca = 1 });

        //     alunoMock.SetAutorizacaoTrocaDispositivo(Arg.Any<SegurancaDevice>()).Returns(1);

        //     var business = new DispositivoBusiness(alunoMock);
        //     var result = business.AutorizaNovoDispositivo(
        //         matricula, token, Utilidades.TipoDevice.iosMobile, Aplicacoes.Recursos
        //         );

        //     alunoMock.Received().GetPermissaoAcesso(
        //         (int)Aplicacoes.Recursos, matricula, token, Utilidades.TipoDevice.iosMobile
        //         );

        //     Assert.AreEqual(1, result);
        // }

        // [TestMethod]
        // [TestCategory("Dispositivo")]
        // public void AutorizaNovoDispositivo_TestaBloqueado_StatusBloqueado()
        // {
        //     var alunoMock = Substitute.For<IAlunoEntity>();
        //     var matricula = 10;
        //     var token = "tokenTeste";

        //     alunoMock.GetPermissaoAcesso(
        //         (int)Aplicacoes.Recursos, matricula, token, Utilidades.TipoDevice.iosMobile
        //         ).Returns(new PermissaoDevice { PermiteAcesso = 0, PermiteTroca = 0 });

        //     alunoMock.SetAutorizacaoTrocaDispositivo(Arg.Any<SegurancaDevice>()).Returns(1);

        //     var business = new DispositivoBusiness(alunoMock);
        //     var result = business.AutorizaNovoDispositivo(
        //         matricula, token, Utilidades.TipoDevice.iosMobile, Aplicacoes.Recursos
        //         );

        //     alunoMock.Received().GetPermissaoAcesso(
        //         (int)Aplicacoes.Recursos, matricula, token, Utilidades.TipoDevice.iosMobile
        //         );

        //     Assert.AreEqual(0, result);
        // }

        [TestMethod]
        [TestCategory("Basico")]
        //[Ignore]
        public void CanSetReportScreenshot()
        {
            var matricula = 227166;

            var post = new AlunoEntity().SetMedsoftScreenshotReport(new SegurancaDevice
            {
                Matricula = matricula,
                ScreenshotCounter = 1
            });
            Assert.IsTrue(post);
        }
        
    }
}