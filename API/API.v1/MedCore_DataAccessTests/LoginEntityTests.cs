using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using MedCore_DataAccessTests.EntitiesMockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class LoginEntityTests
    {
        [TestMethod]
        public void InsertLogAcessoLoginAluno()
        {
            int aplicacao = (int)Aplicacoes.AreaRestrita;
            int idAcesso = (int)Cliente.StatusRetorno.Sucesso;

            var logAcessoLoginAluno = new LogLogin()
            {
                Matricula = 96409,
                AplicacaoId = aplicacao,
                AcessoId = idAcesso,
            };

            var Log = new LogEntity().InsertAcessoLogin(logAcessoLoginAluno);

            Assert.IsTrue(Log > 0);
        }

        [TestMethod]
        public void InsertLogAcessoLoginAluno__ComStatusCanceladoSomenteRestritaConsegueLogar()
        {
            int aplicacao = (int)Aplicacoes.AreaRestrita;
            int idAcesso = (int)Cliente.StatusRetorno.Cancelado;

            var logAcessoLoginAluno = new LogLogin()
            {
                Matricula = 96409,
                AplicacaoId = aplicacao,
                AcessoId = idAcesso,
            };
            var Log = new LogEntity().InsertAcessoLogin(logAcessoLoginAluno);
            Assert.IsTrue(Log > 0);
        }

        [TestMethod]
        public void Login_Liberado_ValidacaoSucesso()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = -1;
            var senha = "senha";
            var estado = "SP";

            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);

            mockAluno.GetAlunoEstado(matricula).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.AreEqual(nome, result.Perfil.Nome);
            Assert.AreEqual(estado, result.Perfil.Estado);
            Assert.AreEqual(matricula, result.Perfil.Matricula);
        }

        [TestMethod]
        public void Login_SenhaIncorreta_ValidacaoErroSenha()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = 123;
            var senha = "senha";
            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);
                        
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });

            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, "senhaErrada", Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.ErroAviso, result.Validacao);
            Assert.AreEqual(ValidaLoginDTO.SenhaInvalida.Mensagem, result.Mensagem);
        }

        [TestMethod]
        public void Login_InadimplenteTermos_ValidacaoSucessoInadimplente()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();
            var estado = "SP";

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = -1;
            var senha = "senha";
            var mensagemInadimplencia = "Mensagem de inadimplencia para: "+ nome;
            var idOrdemVenda = 3030;

            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);


            mockAluno.GetAlunoEstado(matricula).Returns(estado);
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1, Mensagem = mensagemInadimplencia, IdOrdemDeVenda = idOrdemVenda
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.InadimplenteTermos, result.Validacao);
            Assert.AreEqual(mensagemInadimplencia, result.Mensagem);
            Assert.AreEqual(estado, result.Perfil.Estado);
            Assert.AreEqual(idOrdemVenda, result.IdOrdemVenda);
        }

        [TestMethod]
        public void Login_BloqueadoInadimplencia_ValidacaoErroInadimplente()
        {

            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = 123;
            var senha = "senha";
            var mensagemInadimplencia = "Mensagem de inadimplencia para: " + nome;


            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);


            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 0, Mensagem = mensagemInadimplencia
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.SemAcesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });

            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.ErroMensagem, result.Validacao);
            Assert.AreEqual(mensagemInadimplencia, result.Mensagem);
        }

        [TestMethod]
        public void GetPermissaoInativo_AlunoCancelado2018_NaoPermiteLogar()
        {
            //todo pegar cpf somente com ov <= 2018 e seja cancelado
            var clientes = new AlunoEntity().GetPermissao("08942660711", (int)Aplicacoes.MEDSOFT);
            Assert.IsFalse(Convert.ToBoolean(clientes.PermiteAcesso));
        }

        [TestMethod]
        public void GetPermissaoInativo_AlunoSuspensoPendente_NaoPermiteLogar()
        {
            //todo pegar cpf supenso ou pendente e seja cancelado
            var clientes = new AlunoEntity().GetPermissao("53345438801", (int)Aplicacoes.MsProMobile);
            Assert.IsFalse(Convert.ToBoolean(clientes.PermiteAcesso));
        }

        [TestMethod]
        public void Login_AcessoGolden_SucessoAcessoGolden()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();


            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = 123231;
            var senha = "senhaGolden";
            var mensagemInadimplencia = "Mensagem de inadimplencia para: " + nome;

            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "0.0.0").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);

            
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(1);
            mockCliente.ObterSenhaGolden().Returns(senha);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1("senhaCliente"), ID = matricula
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "0.0.0");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.AreEqual(matricula, result.Perfil.Matricula);
        }

        [TestMethod]
        public void Login_LiberaRMaisAlunoR1InteresseRMais_RMaisLiberado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = -1;
            var senha = "senha";
            var estado = "SP";

            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);

            mockAluno.GetAlunoEstado(matricula).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            mockPerfil.AlunoTemInteresseRMais(matricula).Returns(true);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.IsTrue(result.Perfil.RMais);
        }

        [TestMethod]
        public void Login_NaoLiberaRMaisAlunoR1SemInteresseRMais_RMaisNaoLiberado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Godzilla Zilla";
            var matricula = -1;
            var senha = "senha";
            var estado = "SP";

            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll((int)Aplicacoes.Recursos, "").Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);

            mockAluno.GetAlunoEstado(matricula).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   Nome = nome, Senha = Utilidades.EncryptionSHA1(senha), ID = matricula
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            mockPerfil.AlunoTemInteresseRMais(matricula).Returns(false);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.Login(register, senha, Aplicacoes.Recursos, "");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.IsFalse(result.Perfil.RMais);
        }

        [TestMethod]
        public void GanhaPermissaoRMaisPorInteresse_PossuiR1PossuiInteresseRmaisNaoPossuiRMais_GanhaPermissao()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var permissoes = new Dictionary<Utilidades.EMenuAccessObject, bool>
            {
                { Utilidades.EMenuAccessObject.RecursosRMaisCirurgia, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisClinica, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisPediatria, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisGO, false }
            };

            mockPerfil.AlunoTemInteresseRMais(1).Returns(true);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.GanhaPermissaoRMaisPorInteresse(1, true, permissoes);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GanhaPermissaoRMaisPorInteresse_PossuiR1PossuiInteresseRmaisPossuiRMais_NaoGanhaPermissao()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var permissoes = new Dictionary<Utilidades.EMenuAccessObject, bool>
            {
                { Utilidades.EMenuAccessObject.RecursosRMaisCirurgia, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisClinica, true },
                { Utilidades.EMenuAccessObject.RecursosRMaisPediatria, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisGO, false }
            };

            mockPerfil.AlunoTemInteresseRMais(1).Returns(true);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.GanhaPermissaoRMaisPorInteresse(1, true, permissoes);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GanhaPermissaoRMaisPorInteresse_NaoPossuiR1PossuiInteresseRmaisNaoPossuiRMais_NaoGanhaPermissao()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var permissoes = new Dictionary<Utilidades.EMenuAccessObject, bool>
            {
                { Utilidades.EMenuAccessObject.RecursosRMaisCirurgia, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisClinica, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisPediatria, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisGO, false }
            };

            mockPerfil.AlunoTemInteresseRMais(1).Returns(true);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.GanhaPermissaoRMaisPorInteresse(1, false, permissoes);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GanhaPermissaoRMaisPorInteresse_PossuiR1NaoPossuiInteresseRmaisNaoPossuiRMais_NaoGanhaPermissao()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var permissoes = new Dictionary<Utilidades.EMenuAccessObject, bool>
            {
                { Utilidades.EMenuAccessObject.RecursosRMaisCirurgia, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisClinica, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisPediatria, false },
                { Utilidades.EMenuAccessObject.RecursosRMaisGO, false }
            };

            mockPerfil.AlunoTemInteresseRMais(1).Returns(false);

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.GanhaPermissaoRMaisPorInteresse(1, true, permissoes);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ObterDadosVersao_TestaBloqueioVersaoMinima_VersaoNaoValida()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            mockVersao.GetUltimaVersaoBloqueada((int)Aplicacoes.Recursos).Returns("1.0.0");
            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var resultVersaoIgual = business.ObterDadosVersao("1.0.0", "1.2.1", Aplicacoes.Recursos);
            var resultVersaoMenor = business.ObterDadosVersao("0.9.115", "1.2.1", Aplicacoes.Recursos);

            Assert.AreEqual(false, resultVersaoIgual.VersaoValida);
            Assert.AreEqual(false, resultVersaoIgual.VersaoAtualizada);
            Assert.IsFalse(string.IsNullOrEmpty(resultVersaoIgual.NumeroUltimaVersao));
        }

        [TestMethod]
        public void ObterDadosVersao_TestaExisteNovaVersao_VersaoNaoAtualizada()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            mockVersao.GetUltimaVersaoBloqueada((int)Aplicacoes.Recursos).Returns("1.0.0");
            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var resultVersaoIgual = business.ObterDadosVersao("1.0.1", "1.2.1", Aplicacoes.Recursos);
            var resultVersaoMenor = business.ObterDadosVersao("1.1.115", "1.2.1", Aplicacoes.Recursos);

            Assert.AreEqual(true, resultVersaoIgual.VersaoValida);
            Assert.AreEqual(false, resultVersaoIgual.VersaoAtualizada);
            Assert.IsFalse(string.IsNullOrEmpty(resultVersaoIgual.NumeroUltimaVersao));
        }

        [TestMethod]
        public void ObterDadosVersao_TestaVersaoAtualizada_VersaoValidaAtualizada()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            mockVersao.GetUltimaVersaoBloqueada((int)Aplicacoes.Recursos).Returns("1.0.0");
            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var resultVersaoIgual = business.ObterDadosVersao("1.2.1", "1.2.1", Aplicacoes.Recursos);

            Assert.AreEqual(true, resultVersaoIgual.VersaoValida);
            Assert.AreEqual(true, resultVersaoIgual.VersaoAtualizada);
            Assert.IsFalse(string.IsNullOrEmpty(resultVersaoIgual.NumeroUltimaVersao));
        }

        [TestMethod]
        public void ValidaAcesso_TestaSucessoSemSenha_AcessoLiberado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Fulano Tal";
            var matricula = -1;
            var estado = "SP";
            var mockMenu = Substitute.For<IMenuData>();
            var mockPessoa = Substitute.For<IDataAccess<Pessoa>>();
            var mockBlacklist = Substitute.For<IBlackListData>();
            var menuEntityTestData = new MenuEntityTestData();
            var pessoasBlacklistMocked = menuEntityTestData.ObterListaDeAlunosBlackList();
            var pessoasMocked = menuEntityTestData.ObterListaDeAlunos();

            mockPessoa.GetByFilters(Arg.Any<Pessoa>()).Returns(pessoasMocked);
            mockBlacklist.GetAll().Returns(pessoasBlacklistMocked);


            var listMenu = menuEntityTestData.ObtemListaMenusMockados();
            var listPermissaoRegra = menuEntityTestData.ObtemPermissaoRegraMockados();

            mockMenu.GetAll(Arg.Any<int>(), Arg.Any<String>()).Returns(listMenu);
            mockMenu.GetAlunoPermissoesMenu(Arg.Any<List<Menu>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime?>(), Arg.Any<int>()).Returns(listPermissaoRegra);

            mockAluno.GetAlunoEstado(matricula).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });

            mockCliente.GetDadosBasicos(matricula).Returns(new Cliente { Register = register });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   ID = matricula, Nome = nome
               }
            });
            mockCliente.GetDadosBasicos(register).Returns(new Cliente
            {
                ID = matricula,
                Register = register
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, mockMenu, mockPessoa, mockBlacklist);

            var result = business.ValidaAcesso(matricula, Aplicacoes.Recursos, "1.0.1");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.AreEqual(nome, result.Perfil.Nome);
            Assert.AreEqual(estado, result.Perfil.Estado);
            Assert.AreEqual(matricula, result.Perfil.Matricula);
        }

        [TestMethod]
        public void ValidaAcesso_TestaValidaVisitante_AcessoLiberado()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "12345678909";
            var nome = "Fulano Tal";
            var estado = "SP";

            mockAluno.GetAlunoEstado(0).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Cliente);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });

            mockCliente.GetDadosBasicos(0).Returns(new Cliente { Register = register });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   ID = 0, Nome = nome
               }
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.ValidaAcesso(0, Aplicacoes.Recursos, "1.0.1");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.IsNotNull(result.Versao);
            Assert.AreNotEqual(string.Empty, result.Versao.NumeroUltimaVersao ?? string.Empty);
        }

        [TestMethod]
        public void ValidaAcesso_TestaAcessoProfessor_AcessoLiberado()
        {
            var mockCliente = Substitute.For<IClienteEntity>();
            var mockFuncionario = Substitute.For<IFuncionarioData>();
            var mockAluno = Substitute.For<IAlunoEntity>();
            var mockPerfil = Substitute.For<IPerfilAlunoData>();
            var mockVersao = Substitute.For<IVersaoAppPermissaoEntityData>();

            var register = "08080653780";
            var nome = "MESSINA";
            var estado = "SP";
            var matricula = 85943;

            var lstFuncionario = new List<Funcionario>();
            var funcionario = new Funcionario()
            {
                ID = 85943,
                Nome = "MESSINA",
                Register = "08080653780",
                Senha = "123131",
                IdCargo = 131
            };

            lstFuncionario.Add(funcionario);



            mockAluno.GetAlunoEstado(85943).Returns(estado);
            mockPerfil.IsAlunoExtensivoAsync(register).Returns(Task.Factory.StartNew(() => true));
            mockCliente.GetPersonType(register).Returns(Pessoa.EnumTipoPessoa.Professor);
            mockAluno.GetPermissao(register, (int)Aplicacoes.Recursos).Returns(new PermissaoInadimplencia
            {
                PermiteAcesso = 1
            });

            mockCliente.GetDadosBasicos(85943).Returns(new Cliente { Register = register });
            mockCliente.UserGolden(register, Aplicacoes.Recursos).Returns(0);
            mockFuncionario.GetFuncionariosRecursos(Arg.Any<String>()).Returns(lstFuncionario);
            mockCliente.GetByFilters(Arg.Any<Cliente>(), Arg.Any<int>(), Arg.Any<Aplicacoes>())
            .Returns(new Clientes
            {
               new Cliente
               {
                   Register = register,  RetornoStatus = Cliente.StatusRetorno.Sucesso,
                   ID = 85943, Nome = nome
               }
            });

            var business = new LoginBusiness(mockCliente, mockAluno, mockFuncionario, mockPerfil, mockVersao, null, null, null);

            var result = business.ValidaAcesso(matricula, Aplicacoes.Recursos, "1.0.1");

            Assert.AreEqual(ValidacaoLogin.Sucesso, result.Validacao);
            Assert.IsNotNull(result.Versao);
            Assert.AreNotEqual(string.Empty, result.Versao.NumeroUltimaVersao ?? string.Empty);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_ComStatusCancelada()
        {
            string register = "04153809365";

            Cliente cliente = new Cliente();
            cliente.Register = register;
            cliente.RetornoStatus = Cliente.StatusRetorno.Cancelado;
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDELETRO);

            Assert.IsTrue(clientes.Count > 0);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_ComStatusSemAcessoMedEletro()
        {
            string register = "04153809365";

            Cliente cliente = new Cliente();
            cliente.Register = register;
            cliente.RetornoStatus = Cliente.StatusRetorno.SemAcesso;
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDELETRO);

            Assert.IsTrue(clientes.Count > 0);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_ComStatusSemAcessoMEDSOFT()
        {
            string register = "04153809365";
            
            Cliente cliente = new Cliente();
            cliente.Register = register;
            cliente.RetornoStatus = Cliente.StatusRetorno.SemAcesso;
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDSOFT);

            Assert.IsTrue(clientes.Count > 0);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_ComStatusInadimplenteMEDSOFT()
        {
            string register = "93268152856";

            Cliente cliente = new Cliente();
            cliente.Register = register;
            cliente.RetornoStatus = Cliente.StatusRetorno.Inadimplente;
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDSOFT);

            Assert.IsTrue(clientes.Count > 0);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_SemAcessoMEDELETRO()
        {
            string register = "04153809365";

            Cliente cliente = new Cliente();
            cliente.Register = register;            
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDELETRO);

            Assert.IsTrue(clientes.Count > 0);
        }

        [Ignore]
        [TestMethod]
        public void InsertLogAcessoLoginAluno_ComAcessoMEDELETRO()
        {
            string register = "50012816019";

            Cliente cliente = new Cliente();
            cliente.Register = register;            
            List<Cliente> clientes = new ClienteEntity().GetByFilters(cliente, 0, Aplicacoes.MEDELETRO);

            Assert.IsTrue(clientes.Count > 0);
        }
    }
}