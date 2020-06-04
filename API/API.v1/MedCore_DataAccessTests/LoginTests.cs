using System;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Business.Enums;
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
    public class LoginTests
    {

        [TestMethod]
        [TestCategory("Login")]
        public void GetPreAcessoAluno_AlunoInexistente_NaoPermiteAcesso()
        {
            string registroInexistente = "12345678910";
            var clientesMockData = new Clientes { new Cliente { RetornoStatus = Cliente.StatusRetorno.Inexistente, TipoErro = "ErroGenerico", MensagemRetorno = "Usuário não cadastrado" } };

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            clientMock.UserGolden(registroInexistente, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetPreByFilters(Arg.Any<Cliente>(),  Arg.Any<Aplicacoes>()).Returns(clientesMockData);
            
            var alunoBus = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);
            var acessoAluno = alunoBus.GetPreAcessoAluno(registroInexistente, (int)Aplicacoes.MsProMobile);

            var statusRetorno = alunoBus.GetResponse();

            Assert.IsFalse(statusRetorno.Sucesso);
            Assert.AreEqual(TipoErroAcesso.CadastroInexistente, statusRetorno.ETipoErro);
            Assert.AreEqual(0, acessoAluno.ID);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetPreAcessoAluno_AlunoExtensivoAnoAtualAtivo_PermiteAcesso()
        {
            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();
            
            clientMock.UserGolden(clientesMockData.First().Register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetPreByFilters(Arg.Any<Cliente>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);


            var alunoBus = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);
            var acessoAluno = alunoBus.GetPreAcessoAluno(clientesMockData.First().Register, (int)Aplicacoes.MsProMobile);

            var statusRetorno = alunoBus.GetResponse();

            Assert.IsTrue(statusRetorno.Sucesso);
            Assert.AreNotEqual(0, acessoAluno.ID);

        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetPreAcessoAluno_AlunoBlackList_NaoPermiteAcesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoBlacklist();

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            
            var alunoBus = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);
            var acessoAluno = alunoBus.GetPreAcessoAluno(aluno.Register, (int)Aplicacoes.MsProMobile);

            var statusRetorno = alunoBus.GetResponse();

            Assert.IsFalse(statusRetorno.Sucesso);
            Assert.AreEqual(TipoErroAcesso.CadastroInexistente, statusRetorno.ETipoErro);
            Assert.AreEqual(0, acessoAluno.ID);

        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetPreAcessoAluno_AlunoSemSenha_MensagemCadastroSenha()
        {
            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };
            clientesMockData.First().Senha = string.Empty;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();
            
            clientMock.UserGolden(clientesMockData.First().Register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetPreByFilters(Arg.Any<Cliente>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);


            var alunoBus = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);
            var acessoAluno = alunoBus.GetPreAcessoAluno(clientesMockData.First().Register, (int)Aplicacoes.MsProMobile);

            var statusRetorno = alunoBus.GetResponse();
            
            Assert.IsFalse(statusRetorno.Sucesso);
            Assert.AreEqual(TipoErroAcesso.SemSenhaCadastrada, statusRetorno.ETipoErro);
            Assert.AreEqual(clientesMockData.First().ID, acessoAluno.ID);

        }


        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoSenhaIncorreta_ErroSenhaIncorreta()
        {
            var register = Constants.CpfClientTesteSemAcessoGolden;
            var senha = Constants.senhaErrada;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;

            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

           var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();

            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual(TipoErroAcesso.SenhaIncorreta, response.ETipoErro);
             
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoSenhaCorreta_PermiteAcesso()
        {

            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var register = Constants.CpfClientTesteSemAcessoGolden;
            var senha = clientesMockData.First().Senha;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;     

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();

            Assert.IsTrue(response.Sucesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAlunoUltimaVersaoIonic1_AlunoUsuarioNaoCadastradoVersaoBloqueada_ErroUsuarioNaoCadastrado()
        {
            var register = Constants.cadastroInvalido;
            var senha = Constants.senhaErrada;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = Utilidades.UltimaVersaoIonic1;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(new Clientes());

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();

            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual(TipoErroAcesso.CadastroInexistente, response.ETipoErro);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAlunoUltimaVersaoIonic1_AlunoSenhaIncorretaVersaoBloqueada_ErroSenhaIncorreta()
        {
            var register = Constants.CpfClientTesteSemAcessoGolden;
            var senha = Constants.senhaErrada;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = Utilidades.UltimaVersaoIonic1;

            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();

            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual(TipoErroAcesso.SenhaIncorreta, response.ETipoErro);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAlunoUltimaVersaoIonic1_AlunoSemSenhaVersaoBloqueada_MensagemCadastroSenha()
        {
            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var register = clientesMockData.First().Register;
            clientesMockData.First().Senha = string.Empty;
            var senha = string.Empty;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = Utilidades.UltimaVersaoIonic1;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();
            
            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual(TipoErroAcesso.SemSenhaCadastrada, response.ETipoErro);

        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAlunoUltimaVersaoIonic1_AlunoSenhaCorretaVersaoBloqueada_ErroVersaoBloqueada()
        {
            var clientesMockData = new Clientes { new PerfilAlunoEntityTestData().GetAlunoAcademicoMock() };

            var register = Constants.CpfClientTesteSemAcessoGolden;
            var senha = clientesMockData.First().Senha;
            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = Utilidades.UltimaVersaoIonic1;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(100);

            clientMock.UserGolden(register, Aplicacoes.MsProMobile).Returns(0);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientesMockData);

            versaoMock.GetUltimaVersaoBloqueada((int)Aplicacoes.MsProMobile).Returns(appVersion);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var aluno = business.GetAcessoAluno(register, senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            var response = business.GetResponse();

            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual(TipoErroAcesso.VersaoAppDesatualizada, response.ETipoErro);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_PerfilSomenteMedeletroAnoAtualAcessandoMedsoftPro_PermiteAcesso()
        {

            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteMedEletroAnoAtual();

            if (aluno == null)
                Assert.Inconclusive("Não há aluno no perfil");

            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, aluno.Senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }



        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_PerfilAnoSeguinte_PermiteAcesso()
        {

            if (DateTime.Now.Month < 12)
                Assert.Inconclusive("Fora da data relevante");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoAnoSeguinte();

            if (aluno == null)
                Assert.Inconclusive("Não há aluno no perfil");

            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, aluno.Senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_PerfilAnoAtualBloqueado_AnoSeguinte_PermiteAcesso()
        {

            var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoAnoAtualBloqueado_AnoSeguinte();

            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, aluno.Senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_PerfilAnoAtual_PermiteAcesso()
        {

            var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoAnoAtualAtivo();

            var deviceToken = Guid.NewGuid().ToString();
            var idDevice = (int)Utilidades.TipoDevice.androidMobile;
            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, aluno.Senha, (int)Aplicacoes.MsProMobile, appVersion, deviceToken, idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }



        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoR4MastologiaAtivo_PermiteAcesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.MASTO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente); ;

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno no cenário");
            }

            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "" , (int)Aplicacoes.MsProMobile, null, "" , idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoR4MastologiaAtivo_NaoExibeMensagensInadimplencia()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.MASTO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente);

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi encontrado aluno no cenário");
            }

            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MsProMobile, null, "", idDevice);

            Assert.IsFalse(alunoRetorno.LstOrdemVendaMsg.Any(x => !string.IsNullOrEmpty(x.Mensagem)));
        }


        [TestMethod]
        [TestCategory("Infos do aluno")]
        public void GetAcessoAluno_PerfilExtensivoAnoAtual_RetornaAlunoComSucesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoAnoAtualAtivo();

            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, (int)Aplicacoes.MsProMobile, appVersion);

            Assert.IsTrue(alunoRetorno.Sucesso);
        }

        [TestMethod]
        [TestCategory("Infos do aluno")]
        public void GetAcessoAluno_PerfilExtensivoAnoAtualInadimplenteBloqueado_RetornaAlunoComSucesso()
        {
            var alunos = new PerfilAlunoEntityTestData().GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado().ToList();
    
            if (alunos.Count() == 0) {
                Assert.Inconclusive("Não foi possível encontrar aluno no cenário");
            }

            var aluno = alunos.FirstOrDefault();

            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, (int)Aplicacoes.MsProMobile, appVersion);

            Assert.IsTrue(alunoRetorno.Sucesso);
        }

        [TestMethod]
        [TestCategory("Infos do aluno")]
        public void GetAcessoAluno_PerfilRMaisAnoAtual_RetornaAlunoComSucesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoR3();

            if (aluno == null)
            {
                Assert.Inconclusive("Não foi possível encontrar aluno no cenário");
            }

            string appVersion = null;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, (int)Aplicacoes.MsProMobile, appVersion);

            Assert.IsTrue(alunoRetorno.Sucesso);
        }

        [TestMethod]
        [TestCategory("Infos do aluno")]
        public void GetAcessoAluno_PerfilInexistente_RetornaSemSucesso()
        {
            var aluno = Constants.cadastroInvalido;

            string appVersion = "5.2.0";

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);

            var business = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno, (int)Aplicacoes.MsProMobile, appVersion);

            Assert.IsFalse(alunoRetorno.Sucesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterPendente_MsProMobile_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualPendente();
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MsProMobile, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterPendente_MedsoftProElectron_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualPendente();
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MEDSOFT_PRO_ELECTRON);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterAtivo_MsProMobile_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivo();
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoR4TegoAtivo_PermiteAcesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.TEGO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente); ;
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MsProMobile, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterAtivo_MedsoftProElectron_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualAtivo();
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MEDSOFT_PRO_ELECTRON);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterCancelado_MsProMobile_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualCancelado();
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoR4TegoAtivo_NaoExibeMensagensInadimplencia()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.TEGO, (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Adimplente);
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MsProMobile, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoMedMasterCancelado_MedsoftProElectron_PermiteAcesso()
        {
            if (Utilidades.AntesDataLiberacaoTestesMedMaster())
                Assert.Inconclusive("Não há cenarios Medmaster");

            var aluno = new PerfilAlunoEntityTestData().GetAlunoMedMasterAnoAtualCancelado();
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MEDSOFT_PRO_ELECTRON);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, null, "", idDevice);

            Assert.IsTrue(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoNaoMedMasterPendente_MsProMobile_NaoPermiteAcesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Pendente, (int)OrdemVenda.StatusOv.Pendente, true);
            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MsProMobile);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MsProMobile, null, "", idDevice);

            Assert.IsFalse(alunoRetorno.PermiteAcesso);
        }

        [TestMethod]
        [TestCategory("Login")]
        public void GetAcessoAluno_AlunoNaoMedMasterPendente_MedsoftProElectron_NaoPermiteAcesso()
        {
            var aluno = new PerfilAlunoEntityTestData().GetAlunoSomenteUmaOV(DateTime.Now.Year, (int)Produto.Produtos.MED, (int)OrdemVenda.StatusOv.Pendente, (int)OrdemVenda.StatusOv.Pendente, true);
            if (aluno == null) Assert.Inconclusive("Não possui aluno nesse cenário");

            var clientes = new ClienteEntity().GetByFilters(new Cliente { Register = aluno.Register }, aplicacao: Aplicacoes.MEDSOFT_PRO_ELECTRON);

            var idDevice = (int)Utilidades.TipoDevice.androidMobile;

            var configMock = Substitute.For<IConfigData>();
            var versaoMock = Substitute.For<IVersaoAppPermissaoEntityData>();
            var clientMock = Substitute.For<IClienteEntity>();

            configMock.GetDeveBloquearAppVersaoNula().Returns(false);
            configMock.GetTempoInadimplenciaTimeoutParametro().Returns(24);
            clientMock.UserGolden(Arg.Any<string>(), Arg.Any<Aplicacoes>()).Returns(1);
            clientMock.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>()).Returns(clientes);

            var business = new AlunoBusiness(new AlunoEntity(), clientMock, configMock, versaoMock);

            var alunoRetorno = business.GetAcessoAluno(aluno.Register, "", (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, null, "", idDevice);

            Assert.IsFalse(alunoRetorno.PermiteAcesso);
        }
    }

}