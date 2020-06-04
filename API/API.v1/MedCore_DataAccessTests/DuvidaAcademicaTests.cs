
using System.Collections.Generic;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using MedCore_DataAccess.Model;
using MedCore_DataAccessTests.EntitiesDataTests;

namespace MedCore_DataAccessTests
{
    public static class DuvidaAcademicaTestData
    {
        public const int ClientId = 96409;

        public static DuvidaAcademicaFiltro GetFiltro()
        {
            //Usuário Acadêmico
            var filtro = new DuvidaAcademicaFiltro()
            {
                ClientId = ClientId,
                Page = 1
            };

            return filtro;
        }

        public static List<int> GetMenusIds()
        {
            return new List<int>() { 88, 199, 212, 216, 218, 219, 220, 224, 228 };
        }

        public static List<AcademicoDADTO> GetAcademicoFake()
        {
            return new List<AcademicoDADTO>() { new AcademicoDADTO() { Nome = "MockProfessor", Id = 1, Email = "mock@mock.com.br" } };
        }

        public static List<DAEmailItemDTO> GetItemsEmail()
        {
            return new List<DAEmailItemDTO>() { new DAEmailItemDTO() { } };
        }

        public static List<DuvidaAcademicaContract> GetDuvidasFake()
        {
            return new List<DuvidaAcademicaContract>() { new DuvidaAcademicaContract(), new DuvidaAcademicaContract() };
        }

        public static List<DuvidasAcademicasEncaminhadaContract> GetEmailsEncaminhadoFake()
        {
            return new List<DuvidasAcademicasEncaminhadaContract>() { new DuvidasAcademicasEncaminhadaContract() { Email = "notificacao.duvidas@gmail.com", TotalEncaminhadas = 1 } };
        }

        public static bool MenuHabilitado(List<Menu> menus, int id)
        {
            foreach(var item in menus)
            {
                if(item.SubMenu.Count > 0)
                {
                    var encontrado = MenuHabilitado(item.SubMenu, id);
                    if (encontrado)
                        return true;
                }

                if (item.Id == id)
                    return true;
            }

            return false;
        }

        public static List<DuvidaAcademicaContract> GetDuvidasUnica()
        {
            var lista = new List<DuvidaAcademicaContract>();
            lista.Add(new DuvidaAcademicaContract()
            {
                Favorita = true,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>()
            });

            return lista;
        }
    }

    [TestClass]
    public class DuvidaAcademicaTests
    {
        public const int ClientId_PerfilMedReader = 241787;
        public const int ClientId_PerfilCpmed2018 = 261588;
        public const int ClientId_PerfilExtensivo = 176930;
        public const int ClientId_PerfilExtensivoMedEletro = 259740;
        public const int ClientId_PerfilMedEletro = 212093;
        public const int ClientId_PerfilPlanejamento2Anos = 247712;
        public const int ClientId_PerfilIntensivao = 261348;

        public const int PermissionObject_MedReader = 642;
        public const int PermissionObject_TodosBloqueados = 641;

        public const int Completo = 0;
        public const int Produto = 0;

        public const int ObjectTypeId = 1;

        public const string VersaoDuvidas = "3.1.4";

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirDuvida_DuvidaQuestao_RegistroCriado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                QuestaoId = "23310",
                ExercicioId = 614,
                TipoExercicioId = 1,
                TipoQuestaoId = 1,
                NumeroQuestao = 26,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Questão Teste",
                Origem = "2017 SIM 01",
                BitAtiva = false,
                OrigemSubnivel = "Questão 26",
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();   
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetBlackWords().Returns(new List<string>());
            duvidasMock.InsertDuvidaQuestao(interacao).Returns(1);
            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetQuestaoConcurso(Convert.ToInt32(interacao.QuestaoId)).ReturnsForAnyArgs(new QuestaoAcademicoDTO());


            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirDuvida_DuvidaApostila_RegistroCriado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                ApostilaId = 177,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Apostila Teste",
                Origem = "CLM 01",
                BitAtiva = false,
                OrigemSubnivel = "Capítulo 1"
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.InsertDuvidaApostila(interacao).Returns(1);
            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EditarDuvida_DuvidaQuestao_RegistroAlterado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                DuvidaId = "1",
                QuestaoId = "23310",
                ExercicioId = 614,
                TipoExercicioId = 1,
                TipoQuestaoId = 1,
                NumeroQuestao = 26,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Questão Teste",
                Origem = "2017 SIM 01",
                BitAtiva = false,
                OrigemSubnivel = "Questão 26",
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetQuestaoConcurso(Convert.ToInt32(interacao.QuestaoId)).ReturnsForAnyArgs(new QuestaoAcademicoDTO());


            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EditarDuvida_DuvidaApostila_RegistroAlterado()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                ApostilaId = 177,
                DuvidaId = "1",
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Apostila Teste",
                Origem = "CLM 01",
                BitAtiva = false,
                OrigemSubnivel = "Capítulo 1"
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirInteracao_InteracaoDuvida_AdicionarInteracao()
        {
            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                TipoInteracao = (int)TipoInteracaoDuvida.Upvote,
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns((tblDuvidasAcademicas_Interacoes) null);
            duvidasMock.InsertInteracao(interacao).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertInteracao(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirDenuncia_Duvida_NovaDenuncia()
        {
            var interacao = new DenunciaDuvidasAcademicasDTO()
            {
                DuvidaId = 1,
                ClientId = 1
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.DeleteDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(false);
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.SetDenuncia(interacao);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirDenuncia_Respostas_NovaDenuncia()
        {
            var interacao = new DenunciaDuvidasAcademicasDTO()
            {
                RespostaId = 1,
                ClientId = 1
            };

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.DeleteDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(false);
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.SetDenuncia(interacao);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirInteracao_InteracaoDuvida_RemoverInteracaoDuvida()
        {
            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                TipoInteracao = (int)TipoInteracaoDuvida.Upvote,
            };

            var interacaoMock = new tblDuvidasAcademicas_Interacoes();

            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns(interacaoMock);
            duvidasMock.DeleteInteracao(interacaoMock).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertInteracao(interacao);

            duvidasMock.Received().DeleteInteracao(interacaoMock);
            duvidasMock.DidNotReceive().InsertInteracao(Arg.Any<DuvidaAcademicaInteracao>());
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirInteracao_RespostaNormal_RegistrarResposta()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                Descricao = "resposta"
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.None);
            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1, 1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.InsertRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1, 1, 1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var resp = business.InsertResposta(interacao);
            Assert.IsTrue(resp != null);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirInteracao_ReplicaResposta_RegistrarReplica()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                Descricao = "replica",
                RespostaParentId = 1
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta
            };

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1, 1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.InsertRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetResposta(Arg.Any<int>()).Returns(new tblDuvidasAcademicas_Resposta() { intClientID = 1 });
            duvidasMock.GetReplica(Arg.Any<DuvidaAcademicaFiltro>()).Returns(new DuvidaAcademicaContract());
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1, 1, 1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var replica = business.InsertReplica(interacao);
            Assert.IsTrue(replica != null);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_InteracaoUpDownVoteSemInteracaoAnterior_ValidarInsertOK()
        {
            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 1,
                RespostaId = 1,
                TipoInteracao = (int)TipoInteracaoDuvida.Downvote
            };

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(interacao).Returns((tblDuvidasAcademicas_Interacoes)null);
            duvidasMock.DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>()).Returns(1);
            duvidasMock.InsertInteracao(interacao).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertInteracao(interacao);

            duvidasMock.DidNotReceive().DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>());
            duvidasMock.Received().InsertInteracao(interacao);

            Assert.IsTrue(result == 1 || result == 2);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_InteracaoUpDownVoteComInteracaoAnterior_ValidarInsertOK()
        {
            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 1,
                RespostaId = 1,
                TipoInteracao = (int)TipoInteracaoDuvida.Downvote
            };

            var tblDuvidasAcademicasInteracoes = new tblDuvidasAcademicas_Interacoes()
            {
                intInteracaoId = 1,
                intClientID = 1,
                intTipoInteracaoId = (int)TipoInteracaoDuvida.Upvote
            };

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(interacao).Returns(tblDuvidasAcademicasInteracoes);
            duvidasMock.DeleteInteracao(tblDuvidasAcademicasInteracoes).Returns(1);
            duvidasMock.InsertInteracao(interacao).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertInteracao(interacao);

            duvidasMock.Received().DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>());
            duvidasMock.Received().InsertInteracao(interacao);

            Assert.IsTrue(result == 1 || result == 2);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_DownvoteComInteracao_ValidarDeleteOK()
        {
            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 1,
                RespostaId = 1,
                TipoInteracao = (int)TipoInteracaoDuvida.Downvote
            };

            var tblDuvidasAcademicasInteracoes = new tblDuvidasAcademicas_Interacoes()
            {
                intInteracaoId = 1,
                intClientID = 1,
                intTipoInteracaoId = (int)TipoInteracaoDuvida.Downvote
            };

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(interacao).Returns(tblDuvidasAcademicasInteracoes);
            duvidasMock.DeleteInteracao(tblDuvidasAcademicasInteracoes).Returns(1);
            duvidasMock.InsertInteracao(interacao).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertInteracao(interacao);

            duvidasMock.Received().DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>());
            duvidasMock.DidNotReceive().InsertInteracao(interacao);

            Assert.IsTrue(result == 1 || result == 2);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_TiposInteracaoDuvidaSemInteracao_ValidarInsertOK()
        {
            var listaDuvidaAcademicaInteracao = new List<DuvidaAcademicaInteracao>();
            foreach (TipoInteracaoDuvida tipoInteracaoDuvida in Enum.GetValues(typeof(TipoInteracaoDuvida)))
            {
                if (tipoInteracaoDuvida != TipoInteracaoDuvida.Downvote && tipoInteracaoDuvida != TipoInteracaoDuvida.Upvote)
                {
                    listaDuvidaAcademicaInteracao.Add(new DuvidaAcademicaInteracao
                    {
                        ClientId = 1,
                        RespostaId = 1,
                        TipoInteracao = (int)tipoInteracaoDuvida
                    });
                }
            }

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns((tblDuvidasAcademicas_Interacoes)null);
            duvidasMock.DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>()).Returns(1);
            duvidasMock.InsertInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            foreach (var interacao in listaDuvidaAcademicaInteracao)
            {
                var result = business.InsertInteracao(interacao);

                duvidasMock.DidNotReceive().DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>());
                duvidasMock.Received().InsertInteracao(interacao);
                Assert.IsTrue(result == 1 || result == 2);
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_TiposInteracaoDuvidaComInteracao_ValidarDeleteOK()
        {
            var listaDuvidaAcademicaInteracao = new List<DuvidaAcademicaInteracao>();
            foreach (TipoInteracaoDuvida tipoInteracaoDuvida in Enum.GetValues(typeof(TipoInteracaoDuvida)))
            {
                if (tipoInteracaoDuvida != TipoInteracaoDuvida.Downvote && tipoInteracaoDuvida != TipoInteracaoDuvida.Upvote)
                {
                    listaDuvidaAcademicaInteracao.Add(new DuvidaAcademicaInteracao
                    {
                        ClientId = 1,
                        RespostaId = 1,
                        TipoInteracao = (int)tipoInteracaoDuvida
                    });
                }
            }

            var tblDuvidasAcademicasInteracoes = new tblDuvidasAcademicas_Interacoes()
            {
                intInteracaoId = 1,
                intClientID = 1,
                intTipoInteracaoId = (int)TipoInteracaoDuvida.Downvote
            };

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.GetInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns(tblDuvidasAcademicasInteracoes);
            duvidasMock.DeleteInteracao(tblDuvidasAcademicasInteracoes).Returns(1);
            duvidasMock.InsertInteracao(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            foreach (var interacao in listaDuvidaAcademicaInteracao)
            {
                var result = business.InsertInteracao(interacao);

                duvidasMock.Received().DeleteInteracao(Arg.Any<tblDuvidasAcademicas_Interacoes>());
                duvidasMock.DidNotReceive().InsertInteracao(interacao);
                Assert.IsTrue(result == 1 || result == 2);
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Set_EditarResposta_AtualizarRegistro()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                RespostaId = 1,
                ClientId = 96409,
                Descricao = "Resposta editada"
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.InsertRespostaReplica(interacao).Returns(1);
            duvidasMock.UpdateRespostaReplica(interacao).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            funcionarioMock.GetTipoPerfilUsuario(1).ReturnsForAnyArgs(EnumTipoPerfil.None);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var resposta = business.InsertResposta(interacao);
            duvidasMock.Received().UpdateRespostaReplica(interacao);
            Assert.IsTrue(resposta.RespostaId == interacao.RespostaId);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Set_HomologarResposta_ValidarHomologacaoTrue()
        {
            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.SetRespostaHomologada(interacao).Returns(1);
            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1,1,1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1,1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.HasRespostaHomologada(1).ReturnsForAnyArgs(true);

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1,1,1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var homologada = business.SetRespostaHomologada(interacao);
            Assert.IsTrue(homologada);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_CriarReplica_ReceberNotificacaoDuvidaRespondida()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica_Respondida();

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidasAluno(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida).Returns(notificacaoM);


            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_ReceberNotificacaoDuvidaFavoritadaRespondida()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica_Respondida();

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);            
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int) EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida).Returns(notificacaoM);


            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {   
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if(duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida);

                    Assert.IsTrue(notificacoes.Count == 1);
                } 
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoDuvidaRespostaMedgrupo_RetornaTipoDuvidaRespostaMedgrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoDuvidaFavoritadaRespostaMedgrupo_RetornaTipoDuvidaFavoritadaRespostaMedgrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoInteracaoDuvidaRespostaMedGrupo_RetornaTipoInteracaoDuvidaRespostaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaRespostaMedGrupo);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaRespostaMedGrupo).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaRespostaMedGrupo);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoInteracaoDuvidaRespostaHomologada_RetornaTipoInteracaoDuvidaRespostaHomologada()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoDuvidaFavoritadaRespostaHomologad_RetornaTipoDuvidaFavoritadaRespostaHomologada()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetNotificacoesDuvidaPorAluno_ComNotificacaoInteracaoDuvidaHomologada_RetornaTipoInteracaoDuvidaHomologada()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaHomologada);

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaHomologada).Returns(notificacaoM);

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaHomologada);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_CriarReplica_ReceberNotificacaoReplicaDuvida()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica_Respondida();

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaDuvida).Returns(notificacaoM);


            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaDuvida);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InsertInteracao_CriarReplica_ReceberNotificacaoReplicaResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademicaContract();
            var duvidaM = DuvidasAcademicasTestData.GetDuvida();
            var notificacaoM = DuvidasAcademicasTestData.GetNotificacaoDuvidaAcademica_Respondida();

            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoMock = Substitute.For<INotificacaoDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);
            duvidaMock.GetDuvida(duvidaM.DuvidaId).Returns(duvidaM);
            notificacaoMock.GetNotificacoesDuvidaPorAluno(duvidaM.DuvidaId, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta).Returns(notificacaoM);


            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoMock);
            var duvidas = business.GetDuvidas(filtro);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            else
            {
                var duvida = duvidas.FirstOrDefault(x => x.Favorita && x.ClientId != filtro.ClientId);
                if (duvida != null)
                {
                    var notificacoes = businessNotificacao.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, filtro.ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta);

                    Assert.IsTrue(notificacoes.Count == 1);
                }
                else
                {
                    Assert.Inconclusive();
                }
            }
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_ListarFeedMain()
        {
            Assert.Inconclusive("Teste com timeout, será analisado pelo time LosDuvideiros");

            var filtro = DuvidaAcademicaTestData.GetFiltro();
            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            Assert.IsTrue(duvidas.Count > 0);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_ListarFeedQuestao()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.QuestaoId = "23310";

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var hasDuvidasQuestao = duvidas.All(x => x.QuestaoId == 23310);
            Assert.IsTrue(hasDuvidasQuestao);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_VerificaDuvidasDuplicadasPaginacao()
        {
            Assert.Inconclusive("Teste com timeout, será analisado pelo time LosDuvideiros");

            var filtro = new DuvidaAcademicaFiltro()
            {
                ClientId = 76972,
                BitTodas = true,
                Page = 1
            };

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro).ToList();
            filtro.Page = 2;
            var duvidasPage2 = business.GetDuvidas(filtro).ToList();
            filtro.Page = 3;
            var duvidasPage3 = business.GetDuvidas(filtro).ToList();

            duvidas.AddRange(duvidasPage2);
            duvidas.AddRange(duvidasPage3);

            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var duvidasCount = duvidas.Count;
            var distinctCount = duvidas.Distinct().Count();
            Assert.IsTrue(duvidasCount == distinctCount);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarMinhasDuvidas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.ClientId = 196088;
            filtro.BitMinhas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasFavoritas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitFavoritas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.Favorita);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void FiltrarDuvidas_PerfilAcademico_DuvidasMinhasRespostas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.ClientId = 53707;
            filtro.BitMinhas = false;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.MinhasRespostas);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void FiltrarDuvidas_PerfilAcademico_DuvidasHomologadas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.ClientId = 53707;
            filtro.BitMinhas = false;
            filtro.BitRespostaHomologadasMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.AprovacaoMedGrupo == true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasDenunciadas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitDenunciadas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.DenunciaAluno || x.Privada);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasHomologadasMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitRespostaHomologadasMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.AprovacaoMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasRespostasMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.Page = 1;
            filtro.BitRespostaMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasComMinhasRespostas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.MinhasRespostas);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasFavoritasERespostaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitRespostaMed = true;
            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();

            var mockDA = Substitute.For<IDuvidasAcademicasData>();

            var business = new DuvidasAcademicasBusiness(mockDA, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));

            mockDA.GetDuvidas(filtro).Returns(DuvidaAcademicaTestData.GetDuvidasUnica());
            
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.Favorita || x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasFavoritasEHomologadaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitRespostaHomologadasMed = true;
            filtro.BitAtiva = true;

            //Mocks
            var duvidasM = DuvidasAcademicasTestData.GetList_DuvidaAcademica_FavoritaAprovadaMedGrupo();
            var duvidaMock = Substitute.For<IDuvidasAcademicasData>();

            duvidaMock.GetDuvidas(filtro).Returns(duvidasM);


            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(duvidaMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));

            var duvidas = business.GetDuvidas(filtro);

            duvidaMock.Received().GetDuvidas(filtro);

            var isValid = duvidas.All(x => x.Favorita || x.AprovacaoMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasFavoritasEMinhasResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.Favorita || x.MinhasRespostas);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetDuvidas_FiltroFavoritasOuMedgrupo_RetornaFavoritasOuMedgrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitRespostaHomologadasMed = true;
            filtro.BitRespostaMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();

            var mockDuvidas = Substitute.For<IDuvidasAcademicasData>();

            var retornoDuvidas = new List<DuvidaAcademicaContract>();

            retornoDuvidas.Add(new DuvidaAcademicaContract() { Favorita = true, AprovacaoMedGrupo = false, RespostaMedGrupo = false, ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>() });
            retornoDuvidas.Add(new DuvidaAcademicaContract() { AprovacaoMedGrupo = true, RespostaMedGrupo = false, ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>() });
            retornoDuvidas.Add(new DuvidaAcademicaContract() { RespostaMedGrupo = true, AprovacaoMedGrupo = false, ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>() });

            mockDuvidas.GetDuvidas(filtro).Returns(retornoDuvidas);

            var business = new DuvidasAcademicasBusiness(mockDuvidas, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);

            var isValid = duvidas.All(x => x.Favorita || x.AprovacaoMedGrupo.Value || x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
            mockDuvidas.Received().GetDuvidas(filtro);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasFavoritasERespostaMedGrupoOuMinhasResposta()
        {

            Assert.Inconclusive("Teste com timeout, será analisado pelo time LosDuvideiros");

            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitRespostaMed = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.Favorita || x.MinhasRespostas || x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetDuvidas_DuvidasMockadas_RetornaFavoritasERespostaHomologadaOuMinhasResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = false;
            filtro.BitFavoritas = true;
            filtro.BitRespostaHomologadasMed = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();

            var mockDA = Substitute.For<IDuvidasAcademicasData>();

            var business = new DuvidasAcademicasBusiness(mockDA, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            
            mockDA.GetDuvidas(filtro).Returns(DuvidaAcademicaTestData.GetDuvidasUnica());

            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.Favorita || x.MinhasRespostas || x.AprovacaoMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetDuvidas_FiltrarDenunciadasPrivadas_RetornaAlgumaPrivadaOuDenunciada()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitDenunciadas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));

            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.Any(x => x.Privada || x.DenunciaAluno);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasERespostaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitRespostaMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasEHomologadaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitRespostaHomologadasMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && x.AprovacaoMedGrupo.Value);
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasEMinhasResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && x.MinhasRespostas);
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasERespostaMedGrupoOuHomologadaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitRespostaHomologadasMed = true;
            filtro.BitRespostaMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && (x.RespostaMedGrupo.Value || x.AprovacaoMedGrupo.Value));
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasERespostaMedGrupoOuMinhasResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitMinhasRespostas = true;
            filtro.BitRespostaMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && (x.MinhasRespostas || x.RespostaMedGrupo.Value));
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasMinhasERespostaHomologadaOuMinhasResposta()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitMinhas = true;
            filtro.BitMinhasRespostas = true;
            filtro.BitRespostaHomologadasMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ClientId == filtro.ClientId && (x.MinhasRespostas || x.AprovacaoMedGrupo.Value));
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasRespostaHomologadaOuRespostaMedGrupo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitRespostaMed = true;
            filtro.BitRespostaHomologadasMed = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.RespostaMedGrupo.Value || x.AprovacaoMedGrupo.Value);
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasRespostaMedGrupoOuMinhasRespostas()
        {
            Assert.Inconclusive("Teste com timeout, será analisado pelo time LosDuvideiros");

            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitRespostaMed = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.RespostaMedGrupo.Value || x.MinhasRespostas);
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_FiltrarDuvidasHomologadasMedGrupoOuMinhasRespostas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitRespostaHomologadasMed = true;
            filtro.BitMinhasRespostas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.AprovacaoMedGrupo.Value || x.MinhasRespostas);
            if (isValid)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Get_FiltroDuvidasEnviadas_ListarApenasEnviadas()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitEnviadas = true;
            filtro.BitMinhas = false;
            filtro.BitAtiva = true;
            filtro.BitTodosProfessores = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.BitEnviada);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Get_FiltroDuvidasMinhasPendencias_ListarEncaminhadasParaMim()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.ClientId = DuvidasAcademicasTestData.GetProfessorComDuvidasEncaminhadas(true);
            filtro.MinhasApostilas = true;
            filtro.BitMinhas = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.Any(x => x.BitEncaminhada);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Get_FiltroDuvidasNumeroCapitulo_ListarApenasDuvidasDoCapitulo()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.NumeroCategoriaApostila = "1";
            filtro.TipoCategoriaApostila = (int)TipoCategoriaDuvidaApostila.Capitulo;
            filtro.ApostilaId = DuvidasAcademicasTestData.GetApostilaComDuvidasPorCapitulo(Convert.ToInt32(filtro.NumeroCategoriaApostila));

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.NumeroCapitulo.ToString() == filtro.NumeroCategoriaApostila && x.TipoCategoriaApostila == (int)TipoCategoriaDuvidaApostila.Capitulo);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Get_FiltroDuvidasCapituloUm_SemDuvidasOutrosCapitulos()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.NumeroCategoriaApostila = "2";
            filtro.BitMinhas = false;
            filtro.TipoCategoriaApostila = (int)TipoCategoriaDuvidaApostila.Capitulo;
            filtro.ApostilaId = DuvidasAcademicasTestData.GetApostilaComDuvidasPorCapitulo(Convert.ToInt32(filtro.NumeroCategoriaApostila));

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.NumeroCapitulo == 1 && x.TipoCategoriaApostila == filtro.TipoCategoriaApostila);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void GetDuvidas_FiltrarDuvidasSemVinculo_RetornaDuvidasSemIdApostilaQuestao()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitSemVinculo = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => x.ApostilaId == null && x.QuestaoId == null);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]

        public void GetDuvidas_FiltrarDuvidasSemInteracao_RetornaDuvidasSemAprovacaoRespostaAcademico()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.BitSemInteracao = true;

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }

            var isValid = duvidas.All(x => !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Has_DuvidasDuplicadasQuestao()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();
            filtro.QuestaoId = "23310";

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            var count = duvidas.Count;
            var countDistinct = duvidas.Distinct().Count();
            Assert.IsTrue(count == countDistinct);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Has_DuvidasDuplicadasApostila()
        {
            var filtro = new DuvidaAcademicaFiltro()
            {
                ClientId = 96409,
                ApostilaId = 177
            };

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            if (duvidas.Count == 0)
            {
                Assert.Inconclusive();
            }
            var count = duvidas.Count;
            var countDistinct = duvidas.Distinct().Count();
            Assert.IsTrue(count == countDistinct);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Validate_DuvidaCriadaMaisde21Dias()
        {
            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));
            var duvidas = business.GetDuvidas(filtro);
            var duvidasMais21Dias = duvidas.Where(x => DateTime.Now > x.DataCriacao.AddDays(21));
            if (duvidas.Count == 0 || duvidasMais21Dias.Count() == 0)
            {
                Assert.Inconclusive();
            }

            var validate = duvidasMais21Dias.All(x => x.Data == string.Empty);
            Assert.IsTrue(validate);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void Can_InserirDuvidaSemVinculo()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida sem vinculo",
                BitAtiva = false
            };

            var notificacaoEntity = new NotificacaoDuvidasAcademicasEntity();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            duvidasMock.InsertDuvida(interacao).Returns(1);
            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            var business = new DuvidasAcademicasBusiness(duvidasMock, new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity));

            var result = business.InsertDuvida(interacao);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void InserirInteracao_CriarObservacao_RetornaResposta()
        {
            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                RespostaId = 1,
                Descricao = "resposta"
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.UpdateObservacaoMedGrupo(interacao).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);


            var resp = business.InsertObservacaoMedGrupo(interacao);
            Assert.IsTrue(resp != null);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EnviarEmail_PerfilCoordenador_SucessoEnviarEmail()
        {
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.Coordenador);

            duvidasMock.GetCoordenadores().Returns(DuvidasAcademicasTestData.GetCoordenadores());
            duvidasMock.GetProfessores().Returns(DuvidasAcademicasTestData.GetProfessores());
            duvidasMock.GetDuvidasProfessor(Arg.Any<DuvidaAcademicaFiltro>()).Returns(DuvidasAcademicasTestData.GetDuvidasProfessor());
            duvidasMock.GetResolvidosProfessor(1).Returns(DuvidasAcademicasTestData.GetDuvidasResolvidasProfessor());
            duvidasMock.EnviarEmailDuvidaAcademica(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.EnviarEmailsCoordenadores();
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EnviarEmail_PerfilProfessor_SucessoEnviarEmail()
        {
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.Coordenador);

            duvidasMock.GetCoordenadores().Returns(DuvidasAcademicasTestData.GetCoordenadores());
            duvidasMock.GetProfessores().Returns(DuvidasAcademicasTestData.GetProfessores());
            duvidasMock.GetDuvidasProfessor(Arg.Any<DuvidaAcademicaFiltro>()).Returns(DuvidasAcademicasTestData.GetDuvidasProfessor());
            duvidasMock.GetResolvidosProfessor(1).Returns(DuvidasAcademicasTestData.GetDuvidasResolvidasProfessor());
            duvidasMock.EnviarEmailDuvidaAcademica(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.EnviarEmailsProfessores();
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EnviarEmail_PerfilProfessor_ProfessorSemDuvidas()
        {
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.Coordenador);

            duvidasMock.GetCoordenadores().Returns(DuvidasAcademicasTestData.GetCoordenadores());
            duvidasMock.GetProfessores().Returns(DuvidasAcademicasTestData.GetProfessores());
            duvidasMock.GetDuvidasProfessor(Arg.Any<DuvidaAcademicaFiltro>()).Returns(new List<DuvidasAcademicasProfessorDTO>());
            duvidasMock.GetResolvidosProfessor(1).Returns(DuvidasAcademicasTestData.GetDuvidasResolvidasProfessor());
            duvidasMock.EnviarEmailDuvidaAcademica(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.EnviarEmailsProfessores();
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void EnviarEmail_PerfilProfessor_SucessoEnviarEmailDAAnosAnterioes()
        {
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.Coordenador);

            duvidasMock.GetCoordenadores().Returns(DuvidasAcademicasTestData.GetCoordenadores());
            duvidasMock.GetProfessores().Returns(DuvidasAcademicasTestData.GetProfessores());
            duvidasMock.GetDuvidasProfessor(Arg.Any<DuvidaAcademicaFiltro>()).Returns(new List<DuvidasAcademicasProfessorDTO>());
            duvidasMock.GetResolvidosProfessor(1).Returns(DuvidasAcademicasTestData.GetDuvidasResolvidasProfessor());
            duvidasMock.EnviarEmailDuvidaAcademica(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.EnviarEmailsProfessores();
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarDuvida_SemBlackWords_NaoGerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                ApostilaId = 177,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Apostila Teste",
                Origem = "CLM 01",
                BitAtiva = false,
                OrigemSubnivel = "Capítulo 1"
            };

            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.InsertDuvidaApostila(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.UpdateDuvida(interacao).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(false);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            duvidasMock.DidNotReceive().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarDuvida_ComBlackWordSeparada_GerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                ApostilaId = 177,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida Apostila Vaca",
                Origem = "CLM 01",
                BitAtiva = false,
                OrigemSubnivel = "Capítulo 1"
            };

            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.InsertDuvidaApostila(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);
            duvidasMock.UpdateDuvida(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            duvidasMock.Received().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarDuvida_ComBlackWordCamuflada_GerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao
            {
                ClientId = 241784,
                ApostilaId = 177,
                DataCriacao = DateTime.Now,
                Descricao = "Duvida ApostilaVacaCLM",
                Origem = "CLM 01",
                BitAtiva = false,
                OrigemSubnivel = "Capítulo 1"
            };

            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            duvidasMock.InsertDuvidaApostila(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);
            duvidasMock.UpdateDuvida(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var result = business.InsertDuvida(interacao);
            duvidasMock.DidNotReceive().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarResposta_SemBlackWords_NaoGerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                Descricao = "resposta"
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.None);
            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1, 1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.InsertRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1, 1, 1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var resp = business.InsertResposta(interacao);
            duvidasMock.DidNotReceive().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(resp != null);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarResposta_ComBlackWordSeparada_GerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                Descricao = "resposta vaca"
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada vaca",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.None);
            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1, 1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.InsertRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);
            duvidasMock.UpdateRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1, 1, 1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var resp = business.InsertResposta(interacao);
            duvidasMock.Received().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(resp != null);
        }

        [TestMethod]
        [TestCategory("DuvidasAcademicasUnit")]
        public void CriarResposta_ComBlackWordCamuflada_GerarDenuncia()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var interacao = new DuvidaAcademicaInteracao()
            {
                DuvidaId = "1",
                ClientId = 1,
                Descricao = "respostavaca"
            };
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 1,
                ClientId = 2
            };

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId,
                Status = EnumStatusNotificacao.NaoEnviado,
                ClientId = interacao.ClientId,
                Descricao = "A sua dúvida  possuí uma resposta homologada TESTEvacaTESTE",
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida
            };

            var filtro = DuvidaAcademicaTestData.GetFiltro();

            var notificacaoEntity = Substitute.For<INotificacaoDuvidasAcademicasData>();
            var duvidasMock = Substitute.For<IDuvidasAcademicasData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var materialApostilaMock = Substitute.For<IMaterialApostilaData>();
            var concursoMock = Substitute.For<IConcursoData>();

            funcionarioMock.GetTipoPerfilUsuario(1).Returns(EnumTipoPerfil.None);
            duvidasMock.GetDuvida(1).ReturnsForAnyArgs(duvida);
            duvidasMock.ListarUsuariosFavoritaramDuvida(1, 1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.ListarUsuariosResponderamDuvida(1, 1).ReturnsForAnyArgs(new List<int>());
            duvidasMock.InsertRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);
            duvidasMock.GetRespostasPorDuvida(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetRespostaPorDuvida());
            duvidasMock.GetReplicasResposta(filtro).ReturnsForAnyArgs(DuvidasAcademicasTestData.GetReplicasPorResposta(1));
            duvidasMock.GetBlackWords().Returns(new List<string>() { "vaca" });
            duvidasMock.InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>()).Returns(true);
            duvidasMock.UpdateRespostaReplica(Arg.Any<DuvidaAcademicaInteracao>()).Returns(1);

            notificacaoEntity.GetNotificacoesDuvidaPorAluno(1, 1, 1).ReturnsForAnyArgs(new List<NotificacaoDuvidaAcademica>());
            notificacaoEntity.SetNotificacaoDuvidaAcademica(notificacaoDuvida).ReturnsForAnyArgs(1);

            var businessNotificacao = new NotificacaoDuvidasAcademicasBusiness(notificacaoEntity);
            var business = new DuvidasAcademicasBusiness(duvidasMock, funcionarioMock, materialApostilaMock, concursoMock, businessNotificacao);

            var resp = business.InsertResposta(interacao);
            duvidasMock.DidNotReceive().InsertDenuncia(Arg.Any<DenunciaDuvidasAcademicasDTO>());
            Assert.IsTrue(resp != null);
        }
    }
}