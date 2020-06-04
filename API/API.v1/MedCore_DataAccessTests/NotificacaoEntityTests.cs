using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class NotificacaoEntityTest
    {
        [TestMethod]
        public void CanGetNotificacaoAluno_SemNotificacaoPrivada()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var matriculaComNotificacaoPrivada = 96409;
            var matriculaSemNotificacaoPrivada = 90918;
            var appMsPro = 17;
            
            var notificacoesPrivadas = new NotificacaoEntity().GetAll(matriculaComNotificacaoPrivada, appMsPro);
            var notificacoesPublicas = new NotificacaoEntity().GetAll(matriculaSemNotificacaoPrivada, appMsPro);

            var quantidateNotificacaoPrivada = notificacoesPrivadas.Count;
            Assert.AreEqual((notificacoesPrivadas.Count - notificacoesPublicas.Count) , quantidateNotificacaoPrivada);
        }

        [TestMethod]
        public void CanGetNotificacaoAluno_ComNotificacaoPrivada()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var matriculaComNotificacaoPrivada = 96409;
            var matriculaSemNotificacaoPrivada = 90918;
            var appMsPro = 17;
            var notificacoes1 = new NotificacaoEntity().GetAll(matriculaComNotificacaoPrivada, appMsPro);
            var notificacoes2 = new NotificacaoEntity().GetAll(matriculaSemNotificacaoPrivada, appMsPro);
            Assert.IsTrue(notificacoes1.Count > notificacoes2.Count);
        }

        [TestMethod]
        public void CanGetNotificacaoAluno_AplicacaoSemNotificacoes()
        {
            var matriculaSemNotificacaoPrivada = 96409;
            var appMsPro = 1;
            var notificacoes = new NotificacaoEntity().GetAll(matriculaSemNotificacaoPrivada, appMsPro);
            Assert.AreEqual(0, notificacoes.Count);
        }

        [TestMethod]
        public void NaoExibirNotificacoesAgendadasParaOFuturo()
        {
            var matricula = 241724;
            var appMsPro = 17;

            var notificacoes = new NotificacaoEntity().GetAll(matricula, appMsPro);
            var serverDate = Utilidades.GetServerDate();

            Assert.IsTrue(notificacoes.All(x => x.DataOriginal <= serverDate));
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificacaoAvaliacao_MontaArrayInfoAdicional_ArrayConforme()
        {

            var temasAvaliacao = NotificacaoEntityTestData.GetTemasAvaliacaoUnicoAluno();

            var arrayInfo = new NotificacaoBusiness(new NotificacaoEntity(), 
                                                 new AccessEntity(), 
                                                 new AlunoEntity(), 
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())).MontaArrayInfoAdicional(temasAvaliacao);


            Regex regex = new Regex(@"({.*?})");
            Match match = regex.Match(arrayInfo);

            Assert.IsTrue(match.Success);

        }


        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificacaoAvaliacao_SetNotificacoesAvaliacao_SeletivasMensagemTratada()
        {

            var parametros = new ParametrosAvaliacaoAula();

            var notificacao = NotificacaoEntityTestData.GetNotificacaoAvaliacao();
            var devices = NotificacaoEntityTestData.GetDeviceNotificacoes();
            var hoje = DateTime.Now.ToString("dd/MM");

            var seletivas = new NotificacaoBusiness(new NotificacaoEntity(),
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).SetNotificacoesAvaliacao( devices, notificacao, parametros);


            Assert.IsTrue(seletivas.All(x => !x.Mensagem.Contains("#DATA")));
            Assert.IsTrue(seletivas.All(x => x.Mensagem.Contains(hoje)));
         
        }


        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificacaoAvaliacao_SetNotificacoesAvaliacao_SeletivasMensagemTratadaDiaParametizado()
        {

            var notificacao = NotificacaoEntityTestData.GetNotificacaoAvaliacao();
            var devices = NotificacaoEntityTestData.GetDeviceNotificacoes();

            var parametros = new ParametrosAvaliacaoAula();


            parametros.Data = new DateTime(2019,01,02);


            var data = new DateTime(2019,01,02).ToString("yyyy-MM-dd");
            var dataTratada = new DateTime(2019, 01, 02).ToString("dd/MM/yyyy");

            var seletivas = new NotificacaoBusiness(new NotificacaoEntity(),
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())).SetNotificacoesAvaliacao(devices, notificacao, parametros);


            Assert.IsTrue(seletivas.All(x => !x.Mensagem.Contains("#DATA")));
            Assert.IsTrue(seletivas.All(x => x.Mensagem.Contains(dataTratada)));
         

        }


        // [TestMethod]
        // [TestCategory("Notificacoes")]
        // public void NotificacaoAvaliacao_GetDevicesNotificacaoAvaliacaoAula_SomenteAlunosPresentesNoDia()
        // {



        //     var notificacao = NotificacaoEntityTestData.GetNotificacaoAvaliacao();
        //     var parametros = new ParametrosAvaliacaoAula();
            

        //     var agora = DateTime.Now;
        //     var hoje = DateTime.Today;
        //     parametros.Data = hoje;

        //     var devices = new NotificacaoBusiness(new NotificacaoEntity(),
        //                                          new AccessEntity(),
        //                                          new AlunoEntity(),
        //                                          new NotificacaoDuvidasAcademicasEntity(),
        //                                          new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
        //                                          ).GetDevicesNotificacaoAvaliacaoAula(notificacao, parametros);


        //     if(!devices.Any())
        //     {
        //         Assert.Inconclusive("Não há notificações a processar");
        //     }

        //     Assert.IsTrue(devices.All(x => x.Data < agora && x.Data > hoje));

        // }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificacaoPrimeiraAula_GetDevicesNotificacaoPrimeiraAula_DataSemParametro()
        {
                       
            var notificacao = NotificacaoEntityTestData.GetNotificacaoPrimeiraAula();
            var parametros = new ParametrosPrimeiraAula();

            var hoje = DateTime.Today;
    
            var devices = new NotificacaoBusiness(new NotificacaoEntity(),
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).GetDevicesNotificacaoPrimeiraAula(notificacao, parametros);


            if (!devices.Any())
            {
                Assert.Inconclusive("Não há notificações a processar");
            }

            Assert.IsTrue(devices.All(x => x.Data.Date == hoje.AddDays(Utilidades.NotificacaoPrimeiraAulaDiasAntecedenciaPadrao)));

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificacaoPrimeiraAula_GetDevicesNotificacaoPrimeiraAula_DataParametrizada()
        {

            var notificacao = NotificacaoEntityTestData.GetNotificacaoPrimeiraAula();

            var parametros = new ParametrosPrimeiraAula
            {
                Data = new DateTime(2019, 01, 11)
            };


            var devices = new NotificacaoBusiness(new NotificacaoEntity(),
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).GetDevicesNotificacaoPrimeiraAula(notificacao, parametros);


            if (!devices.Any())
            {
                Assert.Inconclusive("Não há notificações a processar");
            }

            Assert.IsTrue(devices.All(x => x.Data.Date == parametros.Data.Date));

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void GetNotificacoesAplicacao_NaoPodeRetornarNotificacaoAnoAnteriorNaoLida()
        {
            var aplicacaoID = Aplicacoes.MsProMobile;
            var matricula = new PerfilAlunoEntityTestData().GetAlunoAnoAtualComAnosAnteriores();
            var anoAtual = Utilidades.GetYear();

            var notificacoes = new NotificacaoEntity().GetNotificacoesAplicacao((int)aplicacaoID, matricula);

            if (!notificacoes.Any())
            {
                Assert.Inconclusive("Não há notificações a processar");
            }
            
            Assert.IsFalse(notificacoes.All(x => x.DataOriginal.Year < anoAtual && x.Lida == false));
        }


        [TestMethod]
        [TestCategory("Notificacoes")]
        public void GetNotificacoesAplicacao_NaoPodeRetornarNotificacaoTipoSomenteExterna()
        {
            var aplicacaoID = Aplicacoes.MsProMobile;
            var matricula = new PerfilAlunoEntityTestData().GetAlunoR3().ID;
            var anoAtual = Utilidades.GetYear();

            var notificacoes = new NotificacaoEntity().GetNotificacoesAplicacao((int)aplicacaoID, matricula);

            if (!notificacoes.Any())
            {
                Assert.Inconclusive("Não há notificações a processar");
            }

            Assert.IsFalse(notificacoes.Any(x => x.TipoEnvio == ETipoEnvioNotificacao.PushExterna));
        }


        [TestMethod]
        [TestCategory("Notificacoes")]
        public void ConfiguraNotificacao_NotificacaoSimulado_NotificacoesSeparadasPeloLimiteDevices()
        {
            var parametros = new NotificacaoPushRequest();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var devicesDataTest = NotificacaoEntityTestData.GetListDevicesNotificacao(5000);


            var notificacaoDataMock = Substitute.For<INotificacaoData>();
            notificacaoDataMock.GetDevicesNotificacaoFila(notificacao.IdNotificacao).Returns(devicesDataTest);


            var seletivas = new NotificacaoBusiness(notificacaoDataMock,
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).ConfiguraNotificacao(notificacao, parametros);


            Assert.IsTrue(seletivas.Count == 3);

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void ConfiguraNotificacao_NotificacaoSimulado_NotificacoesComMesmaMensagem()
        {
            var parametros = new NotificacaoPushRequest();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var devicesDataTest = NotificacaoEntityTestData.GetListDevicesNotificacao(5000);

            var notificacaoDataMock = Substitute.For<INotificacaoData>();
            notificacaoDataMock.GetDevicesNotificacaoFila(notificacao.IdNotificacao).Returns(devicesDataTest);


            var seletivas = new NotificacaoBusiness(notificacaoDataMock,
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).ConfiguraNotificacao(notificacao, parametros);


            Assert.IsTrue(seletivas.All(x => x.Mensagem == notificacao.Texto));
            Assert.IsTrue(seletivas.All(x => x.Titulo == notificacao.Titulo));

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void GetInfoAdicionalSimuladoSerializado_NotificacaoSimulado_InfoAdicionalSerializado()
        {
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var devicesDataTest = NotificacaoEntityTestData.GetListDevicesNotificacao(5000);

            var notificacaoDataMock = Substitute.For<INotificacaoData>();
            notificacaoDataMock.GetDevicesNotificacaoFila(notificacao.IdNotificacao).Returns(devicesDataTest);

            var infoAdicional = new NotificacaoBusiness(notificacaoDataMock,
                                                 new AccessEntity(),
                                                 new AlunoEntity(),
                                                 new NotificacaoDuvidasAcademicasEntity(),
                                                 new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())
                                                 ).GetInfoAdicionalSimuladoSerializado(notificacao);


            Assert.IsFalse(String.IsNullOrEmpty(infoAdicional));

            var obj = JsonConvert.DeserializeObject<NotificacaoInfoAdicional>(infoAdicional);

            Assert.AreNotEqual(obj.InfoAdicionalSimulado.TipoSimuladoId, default(int));
            Assert.AreNotEqual(obj.InfoAdicionalSimulado.SimuladoId, default(int));


        }

        // [TestMethod]
        // [TestCategory("Notificacoes")]
        // public void CriarNotificacoesMudancaStatusProva_CriaQtdCorreta_Tres()
        // {
        //     var idProva = 2;
        //     var idNotificacao = (int)Constants.Notificacoes.Recursos.StatusProvaFavoritos;
        //     var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
        //     var mockNotif = Substitute.For<INotificacaoData>();
        //     var mockExercicio = Substitute.For<IExercicioData>();

        //     mockNotif.Get(idNotificacao).Returns(new Notificacao
        //     {
        //         IdNotificacao = idNotificacao, Texto = string.Empty,
        //         Titulo = string.Empty
        //     });
        //     mockNotif.AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>()).Returns(3);
        //     mockNotif.InserirNotificacoesPosEvento(null));
        //     mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
        //     {
        //         Prova = new ProvaConcursoDTO { Nome = string.Empty },
        //         MatriculasFavoritaram = new List<int> { 1, 2, 3 }
        //     });
            
        //     var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, null);
        //     var result = business.CriarNotificacoesMudancaStatusProva(idProva, ProvaRecurso.StatusProva.RecursosEmAnalise);

        //     mockNotif.Received().Get(idNotificacao));
        //     mockNotif.VerifyAllExpectations();
        //     Assert.AreEqual(3, result);
        // }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesMudancaStatusProva_NaoCriarNotificacoesDemaisStatus_Zero()
        {
            var idProva = 2;
            var idNotificacao = (int)Constants.Notificacoes.Recursos.StatusProvaFavoritos;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();

            mockNotif.Get(idNotificacao).Returns(new Notificacao { IdNotificacao = idNotificacao, Texto = string.Empty });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, null);

            var result = business.CriarNotificacoesMudancaStatusProva(idProva, ProvaRecurso.StatusProva.RecursosExpirados);
            Assert.AreEqual(0, result);

            result = business.CriarNotificacoesMudancaStatusProva(idProva, ProvaRecurso.StatusProva.Bloqueado);
            Assert.AreEqual(0, result);

            result = business.CriarNotificacoesMudancaStatusProva(idProva, ProvaRecurso.StatusProva.AguardandoInteração);
            Assert.AreEqual(0, result);

            result = business.CriarNotificacoesMudancaStatusProva(idProva, ProvaRecurso.StatusProva.SobDemanda);
            Assert.AreEqual(0, result);

            mockNotif.DidNotReceive().InserirNotificacoesPosEvento(Arg.Any<NotificacaoPosEventoDTO[]>());
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
            
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void GetNotificacoesAluno_OrdenacaoNotificacoes_Decrescente()
        {
            var matricula = 7;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();

            mockNotif.GetNotificacoesAlunoPosEvento(matricula, Aplicacoes.Recursos).Returns(new List<NotificacaoPosEventoDTO>
            {
                new NotificacaoPosEventoDTO
                {
                    IdNotificacao = 1,
                    Data = DateTime.Now
                },
                new NotificacaoPosEventoDTO
                {
                    IdNotificacao = 2,
                    Data = DateTime.Now.AddHours(1)
                }
            });
            
            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, null);
            var result = business.GetNotificacoesAluno(matricula);

            Assert.AreEqual(2, result.ElementAt(0).IdNotificacao);
            Assert.AreEqual(1, result.ElementAt(1).IdNotificacao);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesMudancaStatusQuestao_NaoCriarNotificacoesDemaisStatus_Zero()
        {
            var idQuestao = 2;
            var idNotificacao = (int)Constants.Notificacoes.Recursos.StatusQuestaoFavoritos;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            mockNotif.Get(idNotificacao).Returns(new Notificacao { IdNotificacao = idNotificacao, Texto = string.Empty });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.CriarNotificacoesMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.NaoExiste);
            Assert.AreEqual(0, result);

            result = business.CriarNotificacoesMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.NaoSolicitado);
            Assert.AreEqual(0, result);

            result = business.CriarNotificacoesMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.Invalido);
            Assert.AreEqual(0, result);

            mockNotif.DidNotReceive().InserirNotificacoesPosEvento(Arg.Any<NotificacaoPosEventoDTO[]>());
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesMudancaStatusQuestao_CriarNotificacaoAnaliseProvaRmais_LinhasAfetadas()
        {
            var idQuestaoRMais = 3;
            var idNotificacao = (int)Constants.Notificacoes.Recursos.StatusQuestaoFavoritos;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();


            mockNotif.Get(idNotificacao).Returns(new Notificacao
            {
                IdNotificacao = idNotificacao, Texto = string.Empty,
                Titulo = string.Empty
            });
            mockQuestao.IsQuestaoProvaRMais(idQuestaoRMais).Returns(true);
            mockQuestao.GetQuestaoConcursoById(idQuestaoRMais).Returns(new tblConcursoQuestoes());

            mockNotif.AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>()).Returns(3);
            mockNotif.InserirNotificacoesPosEvento(null);
            mockExercicio.GetAlunosFavoritaramProva(0).Returns(new ProvaAlunosFavoritoDTO
            {
                Prova = new ProvaConcursoDTO { Nome = string.Empty },
                MatriculasFavoritaram = new List<int> { 1, 2, 3 }
            });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var resultRMais = business.CriarNotificacoesMudancaStatusQuestao(idQuestaoRMais, QuestaoRecurso.StatusQuestao.EmAnalise);
            Assert.IsTrue(resultRMais > 0);

            mockNotif.Received().InserirNotificacoesPosEvento(Arg.Any<NotificacaoPosEventoDTO[]>());
            mockNotif.Received().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesMudancaStatusQuestao_NaoCriarNotificacoesAnaliseProvaR1_ZeroLinhas()
        {
            var idQuestao = 2;
            var idNotificacao = (int)Constants.Notificacoes.Recursos.StatusQuestaoFavoritos;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            mockNotif.Get(idNotificacao).Returns(new Notificacao { IdNotificacao = idNotificacao, Texto = string.Empty });
            mockQuestao.IsQuestaoProvaRMais(idQuestao).Returns(false);


            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.CriarNotificacoesMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.EmAnalise);
            Assert.AreEqual(0, result);

            mockNotif.DidNotReceive().InserirNotificacoesPosEvento(Arg.Any<NotificacaoPosEventoDTO[]>());
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void ProcessarAgendamentoNotificacoesPosEvento_DuplicaNotificacao_UmDevice()
        {
            var notificacao = new Notificacao();
            var notificaDeviceUm = new DeviceNotificacao { DeviceToken = "123" };
            var notificaDeviceDois = new DeviceNotificacao { DeviceToken = "123" };
            var mockNotif = Substitute.For<INotificacaoData>();
            mockNotif.GetNotificacoesPosEvento(EStatusEnvioNotificacao.NaoEnviado).Returns(new List<Notificacao>
            {
                notificacao
            });
            mockNotif.DefinirDevicesNotificacaoPosEvento(notificacao, EStatusEnvioNotificacao.NaoEnviado).Returns(new List<DeviceNotificacao>
            {
                notificaDeviceUm, notificaDeviceDois, notificaDeviceUm
            });

            var business = new NotificacaoBusiness(mockNotif, null, null, null, null);
            business.ProcessarAgendamentoNotificacoesPosEvento();

            mockNotif.DidNotReceive().InserirDevicesNotificacao(new List<DeviceNotificacao>
            {
                notificaDeviceUm
            });

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void ProcessarAgendamentoNotificacoesPosEvento_EnvioPraFila_MetodoChamado()
        {
            var notificacao = new Notificacao();
            var notificaDeviceUm = new DeviceNotificacao { DeviceToken = "123" };
            var notificaDeviceDois = new DeviceNotificacao { DeviceToken = "321" };
            var mockNotif = Substitute.For<INotificacaoData>();
            mockNotif.GetNotificacoesPosEvento(EStatusEnvioNotificacao.NaoEnviado).Returns(new List<Notificacao>
            {
                notificacao
            });
            mockNotif.DefinirDevicesNotificacaoPosEvento(notificacao, EStatusEnvioNotificacao.NaoEnviado).Returns(new List<DeviceNotificacao>
            {
                notificaDeviceUm, notificaDeviceDois, notificaDeviceUm
            });

            var business = new NotificacaoBusiness(mockNotif, null, null, null, null);
            business.ProcessarAgendamentoNotificacoesPosEvento();

            mockNotif.Received().DefinirDevicesNotificacaoPosEvento(notificacao, EStatusEnvioNotificacao.NaoEnviado);
            mockNotif.DidNotReceive().InserirDevicesNotificacao(new List<DeviceNotificacao>
            {
                notificaDeviceUm, notificaDeviceDois
            });
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesAprovacaoAnaliseQuestao_NaoCriarNotificacoesReprovada_Zero()
        {
            var idQuestao = 2;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.CriarNotificacoesAprovacaoAnaliseQuestao(idQuestao, false);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesAprovacaoAnaliseQuestao_NaoCriarNotificacoesAlteradoBanca_Zero()
        {
            var idQuestao = 2;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            mockQuestao.GetQuestaoConcursoById(idQuestao).Returns(new tblConcursoQuestoes
            {
                ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.AlteradaPelaBanca
            });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.CriarNotificacoesAprovacaoAnaliseQuestao(idQuestao, true);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarMudancaStatusQuestao_TestaNaoEnviaCabeSeNecessitaAprovacao_Zero()
        {
            var idQuestao = 2;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            mockQuestao.IsQuestaoProvaRMais(idQuestao).Returns(false);

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            var result = business.NotificarMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.CabeRecurso);
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarMudancaStatusQuestao_TestaNaoEnviaNaoCabeSeNecessitaAprovacao_Zero()
        {
            var idQuestao = 2;
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            mockQuestao.IsQuestaoProvaRMais(idQuestao).Returns(false);

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            var result = business.NotificarMudancaStatusQuestao(idQuestao, QuestaoRecurso.StatusQuestao.NaoCabeRecurso);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarMudancaRankingAcertos_TestaNaoEnviaRankingBloqueado_Zero()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            
            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            var result = business.NotificarMudancaRankingAcertos(idProva, false);

            mockExercicio.DidNotReceive().GetAlunosFavoritaramProva(idProva);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarMudancaRankingAcertos_TestaEnviaRankingLiberado_ChamaMetodoInclusao()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
            {
                MatriculasFavoritaram = new List<int> { 1, 2, 3 },
                Prova = new ProvaConcursoDTO()
            });

            mockNotif.Get((int)Constants.Notificacoes.Recursos.RankingAcertosLiberado).Returns(notificacao);
            mockNotif.AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>()).Returns(3);
            var result = business.NotificarMudancaRankingAcertos(idProva, true);

            mockNotif.Received().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
            mockNotif.Received().Get((int)Constants.Notificacoes.Recursos.RankingAcertosLiberado);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarQuestaoAlteradoBanca_TestaNaoEnviaAnalise_NaoChamaMetodoInclusao()
        {
            var idQuestao = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            mockQuestao.ObterStatusRecursoBanca(idQuestao).Returns(11);

            business.NotificarQuestaoAlteradoBanca(idQuestao, (int)QuestaoRecurso.StatusBancaAvaliadora.EmAnalise);
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarQuestaoAlteradoBanca_TestaNaoEnviaNaoCabe_NaoChamaMetodoInclusao()
        {
            var idQuestao = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            mockQuestao.ObterStatusRecursoBanca(idQuestao).Returns(11);

            business.NotificarQuestaoAlteradoBanca(idQuestao, (int)QuestaoRecurso.StatusBancaAvaliadora.Nao);
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificarQuestaoAlteradoBanca_TestaNaoEnviaStatusNaoMudou_NaoChamaMetodoInclusao()
        {
            var idQuestao = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            mockQuestao.ObterStatusRecursoBanca(idQuestao).Returns((int)QuestaoRecurso.StatusBancaAvaliadora.Sim);

            business.NotificarQuestaoAlteradoBanca(idQuestao, (int)QuestaoRecurso.StatusBancaAvaliadora.Sim);
            mockNotif.DidNotReceive().AtualizarNotificacoesPosEvento(Arg.Any<List<tblNotificacaoEvento>>());
        }


        [TestMethod]
        [TestCategory("Notificacoes")]
        public void StatusQuestaoNotificaAluno_TestaNaoNotificaNaoCabeProvaR1_False()
        {
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.StatusQuestaoNotificaAluno(QuestaoRecurso.StatusQuestao.NaoCabeRecurso, false);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void StatusQuestaoNotificaAluno_TestaNotificaNaoCabeProvaRMais_True()
        {
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.StatusQuestaoNotificaAluno(QuestaoRecurso.StatusQuestao.NaoCabeRecurso, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void StatusQuestaoNotificaAluno_TestaNotificaAnaliseProvaRMais_True()
        {
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.StatusQuestaoNotificaAluno(QuestaoRecurso.StatusQuestao.EmAnalise, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void StatusQuestaoNotificaAluno_TestaNaoNotificaAnaliseProvaR1_False()
        {
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var notificacao = NotificacaoEntityTestData.GetNotificacaoSimulado();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.StatusQuestaoNotificaAluno(QuestaoRecurso.StatusQuestao.EmAnalise, false);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificaMudancaComunicadoLiberado_TestaNaoNotificaNaoLiberado_Zero()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);

            var result = business.NotificaMudancaComunicadoLiberado(idProva, false);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificaMudancaComunicadoLiberado_TestaNaoCriaNotificacaoAlunoNaoFavoritou_Zero()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO());

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            var result = business.NotificaMudancaComunicadoLiberado(idProva, true);

            mockExercicio.Received().GetAlunosFavoritaramProva(idProva);
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void CriarNotificacoesLiberacaoComunicado_TestaNaoCriaNotificacaoAlunoNaoFavoritou_Zero()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
            {
                MatriculasFavoritaram = new List<int>()
            });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao);
            var result = business.NotificaMudancaComunicadoLiberado(idProva, true);

            mockExercicio.Received().GetAlunosFavoritaramProva(idProva);
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificaConclusaoEstadoQuestoesProva_TestaNaoCriaNotificacaoNaoPermitidaExternamente_Zero()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var mockConcurso = Substitute.For<IConcursoData>();
            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
            {
                MatriculasFavoritaram = new List<int>()
            });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao, mockConcurso);
            var result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.AvisaProfessorComentarioPre);
            Assert.AreEqual(0, result);

            result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.ComunicadoLiberado);
            Assert.AreEqual(0, result);

            result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.RankingAcertosLiberado);
            Assert.AreEqual(0, result);

            result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.StatusProvaFavoritos);
            Assert.AreEqual(0, result);

            result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.StatusQuestaoFavoritos);
            Assert.AreEqual(0, result);

            mockConcurso.DidNotReceive().InserirConfiguracaoProvaAluno(Arg.Any<tblProvaAlunoConfiguracoes>());
        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void NotificaConclusaoEstadoQuestoesProva_InsereConfiguracaoNotificacao_ChamaMetodoInsere()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var mockConcurso = Substitute.For<IConcursoData>();
            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
            {
                MatriculasFavoritaram = new List<int>()
            });
            mockConcurso.InserirConfiguracaoProvaAluno(Arg.Any<tblProvaAlunoConfiguracoes>()).Returns(1);

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao, mockConcurso);
            var result = business.NotificaConclusaoEstadoQuestoesProva(idProva, Constants.Notificacoes.Recursos.ConclusaoAnaliseAcademica);

            mockConcurso.Received().InserirConfiguracaoProvaAluno(Arg.Any<tblProvaAlunoConfiguracoes>());

        }

        [TestMethod]
        [TestCategory("Notificacoes")]
        public void StatusConclusaoNotificaExternamente_PermiteNotificacoesExternas_True()
        {
            var idProva = 2;
            var mockNotif = Substitute.For<INotificacaoData>();
            var mockExercicio = Substitute.For<IExercicioData>();
            var mockQuestao = Substitute.For<IQuestaoData>();
            var mockConcurso = Substitute.For<IConcursoData>();
            mockExercicio.GetAlunosFavoritaramProva(idProva).Returns(new ProvaAlunosFavoritoDTO
            {
                MatriculasFavoritaram = new List<int>()
            });

            var business = new NotificacaoRecursosBusiness(mockNotif, mockExercicio, mockQuestao, mockConcurso);
            var result = business.StatusConclusaoNotificaExternamente(Constants.Notificacoes.Recursos.ConclusaoAnaliseAcademica);
            Assert.IsTrue(result);

            result = business.StatusConclusaoNotificaExternamente(Constants.Notificacoes.Recursos.ConclusaoAnaliseBancaQuestoes);
            Assert.IsTrue(result);
        }
    }
}