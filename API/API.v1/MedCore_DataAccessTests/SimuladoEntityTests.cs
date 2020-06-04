using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesMockData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class SimuladoEntityTests
    {
        #region Fields
        private ISimuladoData _mockSimulado;
        private IExercicioData _mockExercicio;
        private IBannerData _mockBanner;
        private ISimuladoBusiness _simuladoBusiness;
		#endregion

		#region Inicialização
		[TestInitialize]
        public void Initialize()
        {
            _mockSimulado = Substitute.For<ISimuladoData>();
            _mockExercicio = Substitute.For<IExercicioData>();
            _mockBanner = Substitute.For<IBannerData>();

            _simuladoBusiness = new SimuladoBusiness(_mockSimulado, _mockExercicio, _mockBanner);
        }
		#endregion

		#region Unitários
		[TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoEspecialidadesAgrupadas_ProcessandoRanking_DescricaoRealizado()
        {
            var ano = 2017;
            var matricula = 525;
            var idAplicacao = 17;

            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(ano, matricula, idAplicacao, true)
                                .Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoByFilters2017MED());


            var servico = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity());
            List<Exercicio> retorno = servico.GetSimuladoEspecialidadesAgrupadas(ano, matricula, idAplicacao, (int)Constants.TipoSimulado.Extensivo);

            Assert.IsTrue(retorno.FirstOrDefault().Descricao.Contains("REALIZADO"));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoEspecialidadesAgrupadas_SimuladoRealizado_Descricao()
        {
            var ano = 2017;
            var matricula = 525;
            var idAplicacao = 17;

            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(ano, matricula, idAplicacao, true)
                                .Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoByFilters2017MED());


            var servico = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity());
            List<Exercicio> retorno = servico.GetSimuladoEspecialidadesAgrupadas(ano, matricula, idAplicacao, (int)Constants.TipoSimulado.Extensivo);

            Assert.IsFalse(retorno.LastOrDefault().Descricao.Contains("REALIZADO"));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetSimuladoEspecialidadesAgrupadas_Realizados()
        {


            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(525)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(2017, 525, 17, true)
                                .Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoByFilters2017MED());


            var servico = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity());
            List<Exercicio> retorno = servico.GetSimuladoEspecialidadesAgrupadas(2017, 525, 17, (int)Constants.TipoSimulado.Extensivo);

            Assert.AreEqual(1, retorno.ElementAt(0).Realizado);
            Assert.AreEqual(1, retorno.ElementAt(1).Realizado);
            Assert.AreEqual(1, retorno.ElementAt(2).Realizado);
            Assert.AreEqual(1, retorno.ElementAt(3).Realizado);

        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetSimuladoEspecialidadesAgrupadas_QuandoIguais()
        {

            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(525)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(2017, 525, 17, true)
                                .Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoByFilters2017MED());


            var servico = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity());
            List<Exercicio> retorno = servico.GetSimuladoEspecialidadesAgrupadas(2017, 525, 17, (int)Constants.TipoSimulado.Extensivo);

            //Testa a Quantidade de Itens na lista
            Assert.AreEqual(4, retorno.Count);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetSimuladoEspecialidadesAgrupadas_QuandoExisteOnline()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(525)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(2017, 525, 17, true)
                                .Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoByFilters2017MED());


            var servico = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity());
            List<Exercicio> retorno = servico.GetSimuladoEspecialidadesAgrupadas(2017, 525, 17, (int)Constants.TipoSimulado.Extensivo);


            Assert.AreEqual(true, retorno.ElementAt(3).Ativo);
            Assert.AreEqual(0, retorno.ElementAt(3).Online);
        }

        [TestMethod]
        [TestCategory("Básico")]
        public void GetInformacaoSimuladoParaHotsite_ComSimuladoQueDataInicioMenorQueAtual_RetornaInformacoesSiteComAtivoTrue()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var simuladoQueDataInicioMenorQueAtual = new Exercicio
            {
                DataInicio = 1534118400
            };

            simuladoMock.GetInformacoesBasicasSimulado(Arg.Any<Banner>()).Returns(simuladoQueDataInicioMenorQueAtual);

            var ret = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity()).GetInformacaoSimuladoParaHotsite();
            Assert.IsTrue(ret.Ativo);
        }

        [TestMethod]
        [TestCategory("Básico")]
        public void GetInformacaoSimuladoParaHotsite_ComSimuladoQueDataInicioMaiorQueAtual_RetornaInformacoesSiteComAtivoFalse()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var simuladoQueDataInicioMaiorQueAtual = new Exercicio
            {
                DataInicio = 2545318688
            };

            simuladoMock.GetInformacoesBasicasSimulado(Arg.Any<Banner>()).Returns(simuladoQueDataInicioMaiorQueAtual);

            var ret = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity()).GetInformacaoSimuladoParaHotsite();

            Assert.IsFalse(ret.Ativo);
        }

        [TestMethod]
        [TestCategory("Básico")]
        public void GetInformacaoSimuladoParaHotsite_NaoImportandoSimulado_RealizaFluxoDeObtencaoDeInformacaoDeSimulado()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var bannerMock = Substitute.For<IBannerData>();

            var ret = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), bannerMock).TryGetInformacaoSimuladoParaHotsite();

            simuladoMock.Received().GetInformacoesBasicasSimulado(Arg.Any<Banner>());
            bannerMock.Received().GetBanners(Arg.Any<Aplicacoes>());
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetLabelsSimuladoRealize()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();
            var servico = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity());
            var dtAtual = Utilidades.GetServerDate();
            var simuladoRealize = new Exercicio
            {
                DataInicio = Utilidades.ToUnixTimespan(dtAtual.AddDays(-1)),
                DataFim = Utilidades.ToUnixTimespan(dtAtual.AddDays(1)),
                Online = 1,
                Realizado = 0,
                Ativo = true
            };
            
            Assert.AreEqual("realize", servico.GetLabelSimulado(simuladoRealize.Ativo,
                                               Convert.ToBoolean(simuladoRealize.Realizado),
                                               Convert.ToBoolean(simuladoRealize.Online),
                                               simuladoRealize.DataInicio,
                                               simuladoRealize.DataFim));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetLabelsSimuladoEstude()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();
            var servico = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity());
            var dtAtual = Utilidades.GetServerDate();
            var simuladoEstude = new Exercicio
            {
                DataInicio = Utilidades.ToUnixTimespan(dtAtual.AddDays(-2)),
                DataFim = Utilidades.ToUnixTimespan(dtAtual.AddDays(-1)),
                Online = 0,
                Realizado = 1,
                Ativo = true
            };

            Assert.AreEqual("estude", servico.GetLabelSimulado(simuladoEstude.Ativo,
                                               Convert.ToBoolean(simuladoEstude.Realizado),
                                               Convert.ToBoolean(simuladoEstude.Online),
                                               simuladoEstude.DataInicio,
                                               simuladoEstude.DataFim));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetLabelsSimuladoProcessando()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();
            var servico = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity());
            var dtAtual = Utilidades.GetServerDate();
            var simuladoProcessando = new Exercicio
            {
                DataInicio = Utilidades.ToUnixTimespan(dtAtual.AddDays(-2)),
                DataFim = Utilidades.ToUnixTimespan(dtAtual.AddDays(-1)),
                Online = 0,
                Realizado = 1,
                Ativo = false
            };

            Assert.AreEqual("processando", servico.GetLabelSimulado(simuladoProcessando.Ativo,
                                               Convert.ToBoolean(simuladoProcessando.Realizado),
                                               Convert.ToBoolean(simuladoProcessando.Online),
                                               simuladoProcessando.DataInicio,
                                               simuladoProcessando.DataFim));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetLabelsSimuladoBloqueado()
        {
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();
            var servico = new SimuladoBusiness(simuladoMock, new ExercicioEntity(), new BannerEntity());
            var dtAtual = Utilidades.GetServerDate();
            var simuladoBloqueado = new Exercicio
            {
                DataInicio = Utilidades.ToUnixTimespan(dtAtual.AddDays(1)),
                DataFim = Utilidades.ToUnixTimespan(dtAtual.AddDays(2)),
                Online = 1,
                Realizado = 0,
                Ativo = false
            };

            Assert.AreEqual("bloqueado", servico.GetLabelSimulado(simuladoBloqueado.Ativo,
                                               Convert.ToBoolean(simuladoBloqueado.Realizado),
                                               Convert.ToBoolean(simuladoBloqueado.Online),
                                               simuladoBloqueado.DataInicio,
                                               simuladoBloqueado.DataFim));
        }

        [TestMethod]
        [TestCategory("TipoSimulado")]
        public void GetIdsExerciciosRealizadosAluno_Extensivo()
        {
            var retorno = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO);
            var retorno_comparametro = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO, (int)Constants.TipoSimulado.Extensivo);

            Assert.AreEqual(retorno[true].Count, retorno_comparametro[true].Count);
            Assert.AreEqual(retorno[false].Count, retorno_comparametro[false].Count);
        }        

        #endregion

        #region Integrados
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetSimulados_NotNull()
        {
            var lstSimulado = new SimuladoEntity().GetAll();

            Assert.IsNotNull(lstSimulado.FirstOrDefault());

            Assert.IsInstanceOfType(lstSimulado.FirstOrDefault(), typeof(Simulado));

            Assert.IsNotNull(lstSimulado.FirstOrDefault().Especialidades);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetIdsRealizados_AlunoAtivo_NotNull()
        {
            var ids = new SimuladoEntity().GetIdsRealizados(119300);
            Assert.IsNotNull(ids);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetIdsRealizados_AlunoInativo_NotNull()
        {
            var ids = new SimuladoEntity().GetIdsRealizados(007);
            Assert.IsNotNull(ids);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoEspecialidadesAgrupadas_AlunoAtivo_NotNull()
        {
            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(2016, 227151, 17);
            Assert.IsNotNull(simus);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoEspecialidadesAgrupadas_AlunoInativo_NotNull()
        {
            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(2016, 007, 17);
            Assert.IsNotNull(simus);
        }


        //[TestMethod]
        //[TestCategory("Basico")]
        //public void GetSimuladoAgendado_AlunoDesenv_RetornaProva()
        //{
        //    var simus = new RDSSimuladoEntity().GetSimuladoAgendado(119300);
        //    Assert.IsNotNull(simus);
        //}  


        [TestMethod]
        [TestCategory("Basico")]
        public void IsSimuladoOnline_ApenasObjetivo()
        {
            var isSimuladoObjetivo = new SimuladoEntity().isObjetivo(615);
            Assert.AreEqual(isSimuladoObjetivo, 1);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_PrimeiroSimuladoNaoDeveVirComPropriedadesNulas()
        {
            var idSimulado = 628;

            var lstSimulado = new SimuladoEntity().GetAll();
            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();


            Assert.IsNotNull(simulado.DtHoraInicio);
            Assert.IsNotNull(simulado.DtHoraFim);
            Assert.IsNotNull(simulado.Nome);
            Assert.IsNotNull(simulado.Descricao);
            Assert.IsNotNull(simulado.Ano);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_SegundoSimuladoNaoDeveVirComPropriedadesNulas()
        {
            var idSimulado = 629;

            var lstSimulado = new SimuladoEntity().GetAll();
            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();


            Assert.IsNotNull(simulado.DtHoraInicio);
            Assert.IsNotNull(simulado.DtHoraFim);
            Assert.IsNotNull(simulado.Nome);
            Assert.IsNotNull(simulado.Descricao);
            Assert.IsNotNull(simulado.Ano);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_TerceiroSimuladoNaoDeveVirComPropriedadesNulas()
        {
            var idSimulado = 630;

            var lstSimulado = new SimuladoEntity().GetAll();
            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();


            Assert.IsNotNull(simulado.DtHoraInicio);
            Assert.IsNotNull(simulado.DtHoraFim);
            Assert.IsNotNull(simulado.Nome);
            Assert.IsNotNull(simulado.Descricao);
            Assert.IsNotNull(simulado.Ano);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_PrimeiroSimuladoDeveVirComPropriedadesVálidos()
        {
            var idSimulado = 628;

            var lstSimulado = new SimuladoEntity().GetAll();

            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();

            Assert.IsTrue(simulado.Nome != "");
            Assert.IsTrue(simulado.DtHoraInicio != null);
            Assert.IsTrue(simulado.DtHoraFim != null);
            Assert.IsTrue(simulado.Descricao != "");
            Assert.IsTrue(simulado.Ano > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_SegundoSimuladoDeveVirComPropriedadesVálidos()
        {
            var idSimulado = 629;

            var lstSimulado = new SimuladoEntity().GetAll();

            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();

            Assert.IsTrue(simulado.Nome != "");
            Assert.IsTrue(simulado.DtHoraInicio != null);
            Assert.IsTrue(simulado.DtHoraFim != null);
            Assert.IsTrue(simulado.Descricao != "");
            Assert.IsTrue(simulado.Ano > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAll_TrazendoTodosOsSimulados_TerceiroSimuladoDeveVirComPropriedadesVálidos()
        {
            var idSimulado = 630;

            var lstSimulado = new SimuladoEntity().GetAll();

            var simulado = lstSimulado.Where(x => x.ID == idSimulado).FirstOrDefault();

            Assert.IsTrue(simulado.Nome != "");
            Assert.IsTrue(simulado.DtHoraInicio != null);
            Assert.IsTrue(simulado.DtHoraFim != null);
            Assert.IsTrue(simulado.Descricao != "");
            Assert.IsTrue(simulado.Ano > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoEspecialidadesAgrupadas_UsuarioSemSimulados_DeveRetornarZeroSimulados()
        {
            var matricula = 167129;
            var ano = 2017;
            var idApp = 17;
            int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo;

            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();
            var bannerMock = Substitute.For<IBannerData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());

            exercicioMock.GetSimuladosByFilters(ano, matricula, idApp, true)
                                .Returns(new List<Exercicio>());

            var simus = new SimuladoBusiness(simuladoMock, exercicioMock, bannerMock).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);

            Assert.AreEqual(0, simus.Count);
        }

        [TestMethod]
        [TestCategory("Simulado Agendado")]
        public void SimuladosDevemConterQuantidadeCorretasDeQuestoes()

        {
            var entidade = new SimuladoEntity();
            var simulado = entidade.ObterSimuladoCorrente7DiasAntes();
            
            if (simulado != null)
            {
                var quantidadeQuestoesCadastradas = entidade.ObterQuantidadeDeQuestoesCadastradasNoSimulado(simulado.intSimuladoID);
                var quantidadeQuestoesDeFatoCadastradas = entidade.ObterQuantidadeDeQuestoesDeFatoCadastradasNoSimulado(simulado.intSimuladoID);

                Assert.AreEqual(quantidadeQuestoesCadastradas, quantidadeQuestoesDeFatoCadastradas);
            }
        }

        [TestMethod]
        [TestCategory("Simulado Agendado")]
        public void QuestoesSimuladoAgendadoDevemTrazerEnunciadosCorretos()
        {
            var simuladoEntity = new SimuladoEntity();
            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());

            var exercicioBusiness = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            
            int simuladoID = 0;
            var simulado = simuladoEntity.ObterSimuladoCorrente7DiasAntes();

            if (simulado != null)
            {
                simuladoID = simulado.intSimuladoID;

                var quantidadeQuestoesCadastradas = simuladoEntity.ObterQuantidadeDeQuestoesCadastradasNoSimulado(simuladoID);
                var quantidadeQuestoesDeFatoCadastradas = simuladoEntity.ObterQuantidadeDeQuestoesDeFatoCadastradasNoSimulado(simuladoID);

                if (quantidadeQuestoesCadastradas == quantidadeQuestoesDeFatoCadastradas)
                {
                    var questoes = exercicioBusiness.GetModalSimuladoOnline(simuladoID, 241665, 17).Questoes;

                    foreach (var questao in questoes)
                    {
                        var questaoObtidaPorId = questaoBusiness.GetTipoSimulado(questao.Id, 241665, 17);
                        Assert.AreEqual(questao.Enunciado, questaoObtidaPorId.Enunciado);
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("Simulado Agendado")]
        public void QuestoesSimuladoAgendadoDevemTrazerAlternativasCorretas()
        {
            var simuladoEntity = new SimuladoEntity();
            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());//new RDSQuestaoEntity();

            var exercicioBusiness = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());

            int simuladoID = 0;
            var simulado = simuladoEntity.ObterSimuladoCorrente7DiasAntes();

            if (simulado != null)
            {
                simuladoID = simulado.intSimuladoID;

                var quantidadeQuestoesCadastradas = simuladoEntity.ObterQuantidadeDeQuestoesCadastradasNoSimulado(simuladoID);
                var quantidadeQuestoesDeFatoCadastradas = simuladoEntity.ObterQuantidadeDeQuestoesDeFatoCadastradasNoSimulado(simuladoID);

                if (quantidadeQuestoesCadastradas == quantidadeQuestoesDeFatoCadastradas)
                {
                    var questoes = exercicioBusiness.GetModalSimuladoOnline(simuladoID, 241665, 17).Questoes;

                    foreach (var questao in questoes)
                    {
                        var alternativasQuestao = questaoBusiness.GetTipoSimulado(questao.Id, 241665, 17).Alternativas.ToList();
                        var alternativas = simuladoEntity.ObterAlternativasQuestaoSimulado(questao.Id);

                        for(var i = 0; i < alternativasQuestao.Count; i++)
                        {
                            Assert.AreEqual(alternativasQuestao[0].Letra.ToString(), alternativas[0].txtLetraAlternativa);
                        }
                    }
                }
            }
        }


        [TestMethod]
        [TestCategory("Simulado Agendado")]
        public void ChecaConsistenciaGravacaoRespostasSimulado()
        {
            var simulado = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetSimuladoOnlineCorrente();

            if (simulado == null)
                Assert.Inconclusive();

            var idSimulado = simulado.ID;

            var matricula = 241718;
            var idAplicacao = (int)Aplicacoes.MsProMobile;

            //Iniciar Simulado
            var versaoMinimaImpressaoMobile = ConfigurationProvider.Get("Settings:VersaoMinimaMsProDesktopSimuladoImpressao");
            var simuladoConfiguracao = new ExercicioEntity().GetSimuladoOnlineConfiguracao(idSimulado, matricula, idAplicacao, versaoMinimaImpressaoMobile);
            
            //Checa que o cartão Resposta está limpo
            if (simuladoConfiguracao.CartoesResposta.Questoes.Any(x => x.Respondida == true))
                LimparRealizacaoSimulado(idSimulado, matricula);

            //Cartao Resposta 
            var cartaoResposta = new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(idSimulado, matricula, Exercicio.tipoExercicio.SIMULADO);
            

            //Questao
            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            //Checa que o cartão Resposta está limpo
            if (cartaoResposta.Questoes.Any(x => x.Respondida == true))
                LimparRealizacaoSimulado(idSimulado, matricula);

            var respostas = new List<string>();
            respostas.Add("A");
            respostas.Add("B");
            respostas.Add("C");

            //Enviar Respostas
            for (int i = 0; i < 3; i++)
            {
                var respostaObjetivaPost1Exemplo = new RespostaObjetivaPost()
                {
                    Alterantiva = respostas.ElementAt(i),
                    ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO,
                    HistoricoId = simuladoConfiguracao.CartoesResposta.HistoricoId,
                    QuestaoId = cartaoResposta.Questoes.ElementAt(i).Id,
                    Matricula = matricula
                };

                var setRespostaResponse = questaoBusiness.SetRespostaObjetivaSimuladoAgendado(respostaObjetivaPost1Exemplo);
            }

            //Checar Respotas Enviadas
            for (int i = 0; i < 3; i++)
            {
                //var getQuestao = new QuestaoEntity().GetTipoSimulado(cartaoResposta.Questoes.ElementAt(i).Id, matricula, idAplicacao);
                var getQuestao = questaoBusiness.GetQuestaoSimuladoAgendado(cartaoResposta.Questoes.ElementAt(i).Id, matricula, idAplicacao, simuladoConfiguracao.CartoesResposta.HistoricoId);
                Assert.IsTrue(getQuestao.Respondida);
                Assert.AreEqual(respostas.ElementAt(i), getQuestao.RespostaAluno);
            }

            var cartaoRespostaSimulado = new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoRespostaSimuladoAgendado(matricula, idSimulado, simuladoConfiguracao.CartoesResposta.HistoricoId);

            for (int i = 0; i < 3; i++)
               Assert.AreEqual(cartaoRespostaSimulado.Questoes.ElementAt(i).RespostaAluno, respostas.ElementAt(i));
            LimparRealizacaoSimulado(idSimulado, matricula);
        }

        private void LimparRealizacaoSimulado(int simulado, int clientid)
        {
            //using (var ctx = new materiaisDireitoEntities(true))
            using (var ctx = new AcademicoContext())
            {
                var historico = ctx.tblExercicio_Historico.Where(x => x.intClientID == clientid && x.intExercicioID == simulado && x.intApplicationID == 17 && x.intExercicioTipo == 1).FirstOrDefault();

                var cartaoRespostaObjetiva = ctx.tblCartaoResposta_objetiva.Where(x => x.intHistoricoExercicioID == historico.intHistoricoExercicioID).ToList();
                var cartaoRespostaObjetivaSimuladoOnline = ctx.tblCartaoResposta_objetiva_Simulado_Online.Where(x => x.intHistoricoExercicioID == historico.intHistoricoExercicioID).ToList();


                if (cartaoRespostaObjetiva.Count > 0)
                {
                    foreach (var item in cartaoRespostaObjetiva)
                    {
                        ctx.Entry<tblCartaoResposta_objetiva>(item).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }

                if (cartaoRespostaObjetivaSimuladoOnline.Count > 0)
                {
                    foreach (var item in cartaoRespostaObjetivaSimuladoOnline)
                    {
                        ctx.Entry<tblCartaoResposta_objetiva_Simulado_Online>(item).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                
                ctx.Entry<tblExercicio_Historico>(historico).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }


        #endregion

        #region Simulados R+
        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_R3Cir2019_EspecialidadesAgrupadas_EntregaSimulado()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int) Constants.TipoSimulado.R3_Cirurgia;

            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Cir(ano, matricula, idApp);

            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count > 0);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);
                
                Assert.IsFalse(string.IsNullOrEmpty(item.Label));
                Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
                Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

                Assert.IsNotNull(item.Ativo);
                Assert.IsNotNull(item.IsPremium);

                Assert.IsTrue(item.IdTipoRealizacao > -1);
                Assert.IsTrue(item.Acertos > -1);
                Assert.IsTrue(item.Atual > -1);
                Assert.IsTrue(item.EntidadeApostilaID > -1);
                Assert.IsTrue(item.EstadoID > -1);
                Assert.IsTrue(item.HistoricoId > -1);
                Assert.IsTrue(item.IdConcurso > -1);
                Assert.IsTrue(item.Online > -1);
                Assert.IsTrue(item.Ordem > -1);
                Assert.IsTrue(item.QtdQuestoes > -1);
                Assert.IsTrue(item.Ranqueado > -1);
                Assert.IsTrue(item.Realizado > -1);
                Assert.IsTrue(item.RegiaoID > -1);
                Assert.IsTrue(item.StatusId > -1);
                Assert.IsTrue(item.TempoExcedido > -1);
                Assert.IsTrue(item.TempoTolerancia > -1);
                Assert.IsTrue(item.TipoApostilaId > -1);
                Assert.IsTrue(item.TipoId == idTipoSimulado);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);

                Assert.IsTrue(item.Especialidades.Count > 0);

                foreach (var especialidade in item.Especialidades)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

                    Assert.IsTrue(especialidade.Id > 0);
                    Assert.IsTrue(especialidade.IdAreaAcademica > -1);
                    Assert.IsTrue(especialidade.IntEmployeeID > -1);

                    Assert.IsNotNull(especialidade.Editavel);
                    Assert.IsNotNull(especialidade.DataClassificacao);
                }

            }
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_R3Ped2019_EspecialidadesAgrupadas_EntregaSimulado()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Pediatria;

            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Ped(ano, matricula, idApp);

            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count > 0);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);

                Assert.IsFalse(string.IsNullOrEmpty(item.Label));
                Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
                Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

                Assert.IsNotNull(item.Ativo);
                Assert.IsNotNull(item.IsPremium);

                Assert.IsTrue(item.IdTipoRealizacao > -1);
                Assert.IsTrue(item.Acertos > -1);
                Assert.IsTrue(item.Atual > -1);
                Assert.IsTrue(item.EntidadeApostilaID > -1);
                Assert.IsTrue(item.EstadoID > -1);
                Assert.IsTrue(item.HistoricoId > -1);
                Assert.IsTrue(item.IdConcurso > -1);
                Assert.IsTrue(item.Online > -1);
                Assert.IsTrue(item.Ordem > -1);
                Assert.IsTrue(item.QtdQuestoes > -1);
                Assert.IsTrue(item.Ranqueado > -1);
                Assert.IsTrue(item.Realizado > -1);
                Assert.IsTrue(item.RegiaoID > -1);
                Assert.IsTrue(item.StatusId > -1);
                Assert.IsTrue(item.TempoExcedido > -1);
                Assert.IsTrue(item.TempoTolerancia > -1);
                Assert.IsTrue(item.TipoApostilaId > -1);
                Assert.IsTrue(item.TipoId == idTipoSimulado);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);

                Assert.IsTrue(item.Especialidades.Count > 0);

                foreach (var especialidade in item.Especialidades)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

                    Assert.IsTrue(especialidade.Id > 0);
                    Assert.IsTrue(especialidade.IdAreaAcademica > -1);
                    Assert.IsTrue(especialidade.IntEmployeeID > -1);

                    Assert.IsNotNull(especialidade.Editavel);
                    Assert.IsNotNull(especialidade.DataClassificacao);
                }

            }
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_R3Cli2019_EspecialidadesAgrupadas_EntregaSimulado()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Clinica;

            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Cli(ano, matricula, idApp);

            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count > 0);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);

                Assert.IsFalse(string.IsNullOrEmpty(item.Label));
                Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
                Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

                Assert.IsNotNull(item.Ativo);
                Assert.IsNotNull(item.IsPremium);

                Assert.IsTrue(item.IdTipoRealizacao > -1);
                Assert.IsTrue(item.Acertos > -1);
                Assert.IsTrue(item.Atual > -1);
                Assert.IsTrue(item.EntidadeApostilaID > -1);
                Assert.IsTrue(item.EstadoID > -1);
                Assert.IsTrue(item.HistoricoId > -1);
                Assert.IsTrue(item.IdConcurso > -1);
                Assert.IsTrue(item.Online > -1);
                Assert.IsTrue(item.Ordem > -1);
                Assert.IsTrue(item.QtdQuestoes > -1);
                Assert.IsTrue(item.Ranqueado > -1);
                Assert.IsTrue(item.Realizado > -1);
                Assert.IsTrue(item.RegiaoID > -1);
                Assert.IsTrue(item.StatusId > -1);
                Assert.IsTrue(item.TempoExcedido > -1);
                Assert.IsTrue(item.TempoTolerancia > -1);
                Assert.IsTrue(item.TipoApostilaId > -1);
                Assert.IsTrue(item.TipoId == idTipoSimulado);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);

                Assert.IsTrue(item.Especialidades.Count > 0);

                foreach (var especialidade in item.Especialidades)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

                    Assert.IsTrue(especialidade.Id > 0);
                    Assert.IsTrue(especialidade.IdAreaAcademica > -1);
                    Assert.IsTrue(especialidade.IntEmployeeID > -1);

                    Assert.IsNotNull(especialidade.Editavel);
                    Assert.IsNotNull(especialidade.DataClassificacao);
                }

            }
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_R4Go2019_EspecialidadesAgrupadas_EntregaSimulado()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R4_GO;

            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR4GO(ano, matricula, idApp);

            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count > 0);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);

                Assert.IsFalse(string.IsNullOrEmpty(item.Label));
                Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
                Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

                Assert.IsNotNull(item.Ativo);
                Assert.IsNotNull(item.IsPremium);

                Assert.IsTrue(item.IdTipoRealizacao > -1);
                Assert.IsTrue(item.Acertos > -1);
                Assert.IsTrue(item.Atual > -1);
                Assert.IsTrue(item.EntidadeApostilaID > -1);
                Assert.IsTrue(item.EstadoID > -1);
                Assert.IsTrue(item.HistoricoId > -1);
                Assert.IsTrue(item.IdConcurso > -1);
                Assert.IsTrue(item.Online > -1);
                Assert.IsTrue(item.Ordem > -1);
                Assert.IsTrue(item.QtdQuestoes > -1);
                Assert.IsTrue(item.Ranqueado > -1);
                Assert.IsTrue(item.Realizado > -1);
                Assert.IsTrue(item.RegiaoID > -1);
                Assert.IsTrue(item.StatusId > -1);
                Assert.IsTrue(item.TempoExcedido > -1);
                Assert.IsTrue(item.TempoTolerancia > -1);
                Assert.IsTrue(item.TipoApostilaId > -1);
                Assert.IsTrue(item.TipoId == idTipoSimulado);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);

                Assert.IsTrue(item.Especialidades.Count > 0);

                foreach (var especialidade in item.Especialidades)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

                    Assert.IsTrue(especialidade.Id > 0);
                    Assert.IsTrue(especialidade.IdAreaAcademica > -1);
                    Assert.IsTrue(especialidade.IntEmployeeID > -1);

                    Assert.IsNotNull(especialidade.Editavel);
                    Assert.IsNotNull(especialidade.DataClassificacao);
                }

            }
        }
        
        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_RMais_TipoInexistente_EntregaException()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = -1;//Id inexistente

            try
            {
                var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_RMais_AnoInexistente_EntregaException()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = -1;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            try
            {
                var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimulado_RMais_MatriculaInexistente_EntregaException()
        {
            var matricula = -1;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            try
            {
                var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetAnos_RMais_EntregaAnos()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var tipoExercicio = Exercicio.tipoExercicio.SIMULADO;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            var anos = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosExerciciosPermitidos(tipoExercicio, matricula, idTipoSimulado, true, idApp);

            Assert.IsNotNull(anos);

            foreach (var item in anos)
            {
                Assert.IsTrue(item > 0);
            }
        }


        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetAnos_RMais_TipoSimuladoInexistente_EntregaVazio()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var tipoExercicio = Exercicio.tipoExercicio.SIMULADO;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = -1;

            var anos = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosExerciciosPermitidos(tipoExercicio, matricula, idTipoSimulado, true, idApp);

            Assert.IsTrue(anos.Count == 0);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimuladosDistintosAgrupados_RMais_ExercicioRepetidos_EntregaExercicioUnico()
        {
            var exercicios = new List<Exercicio>();
            exercicios.Add(new Exercicio() { ID = 1 });
            exercicios.Add(new Exercicio() { ID = 1 });

            var simulados = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetSimuladosDistintosAgrupados(exercicios);

            Assert.IsTrue(simulados.Count() == 1);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimuladosDistintosAgrupados_RMais_ExercicioDistintos_EntregaExercicios()
        {
            var exercicios = new List<Exercicio>();
            exercicios.Add(new Exercicio() { ID = 1 });
            exercicios.Add(new Exercicio() { ID = 2 });

            var simulados = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetSimuladosDistintosAgrupados(exercicios);

            Assert.IsTrue(simulados.Count() == exercicios.Count());
        }


        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaAlgumExercicioDoSimuladoFoiRealizado_RMais_EntregaRealizado()
        {
            var idSimuladoMock = 1;

            var dic = new Dictionary<bool, List<int>>();
            dic.Add(true, new List<int>() { idSimuladoMock });

            var exercicioDTO = new ExercicioDTO() { ID = idSimuladoMock };

            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaAlgumExercicioDoSimuladoFoiRealizado(dic, exercicioDTO);

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaAlgumExercicioDoSimuladoFoiRealizado_RMais_EntregaNaoRealizado()
        {
            var idSimuladoMock = 1;

            var dic = new Dictionary<bool, List<int>>();
            dic.Add(true, new List<int>() { idSimuladoMock });

            var exercicioDTO = new ExercicioDTO() { ID = -1 }; //id não existente

            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaAlgumExercicioDoSimuladoFoiRealizado(dic, exercicioDTO);

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecarEspecialidades_GetEspecialidadesSimulado()
        {
            var especialidades = new Especialidades();
            especialidades.Add(new Especialidade() { Id = 1, Descricao = "Cirurgia 01" });
            especialidades.Add(new Especialidade() { Id = 1, Descricao = "Cirurgia 02" });
            var simulado = new ExercicioDTO()
            {
                ID = 1,                                
            };

            var listaSimulados = new List<Exercicio>();
            listaSimulados.Add(new Exercicio() { ID = 1, Especialidades = especialidades });

            var retorno = new SimuladoEntity().GetEspecialidadesSimulado(listaSimulados, simulado);
        }

        public void ChecaSimuladoOnlineDeveEstarHabilitado_RMais_Offline_EntregaDesabilitado()
        {
            var dic = new Dictionary<bool, List<int>>();
            var exercicioDTO = new ExercicioDTO();
            var simulado = new Exercicio();

            var matricula = Constants.CONTACTID_ACADEMICO;
            exercicioDTO.Online = Convert.ToInt32(false);

            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaSimuladoOnlineDeveEstarHabilitado(exercicioDTO, simulado, dic, matricula);

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaSimuladoOnlineDeveEstarHabilitado_RMais_DtLiberacao_EntregaHabilitado()
        {
            var dic = new Dictionary<bool, List<int>>();
            var exercicioDTO = new ExercicioDTO();
            var simulado = new Exercicio();

            var matricula = Constants.CONTACTID_ACADEMICO;
            exercicioDTO.DtLiberacaoRanking = DateTime.Now.AddDays(-1); //Data para o dia anterior
            exercicioDTO.Online = Convert.ToInt32(true);

            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaSimuladoOnlineDeveEstarHabilitado(exercicioDTO, simulado, dic, matricula);

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaSimuladoOnlineDeveEstarHabilitado_RMais_DtLiberacao_EntregaDesabilitado()
        {
            var dic = new Dictionary<bool, List<int>>();
            var exercicioDTO = new ExercicioDTO();
            var simulado = new Exercicio();

            var matricula = Constants.CONTACTID_ACADEMICO;
            exercicioDTO.DtLiberacaoRanking = DateTime.Now.AddDays(1); //Data para o proximo dia
            exercicioDTO.Online = Convert.ToInt32(true);

            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaSimuladoOnlineDeveEstarHabilitado(exercicioDTO, simulado, dic, matricula);

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaSimuladoOnlineDeveEstarHabilitado_RMais_Datas_EntregaHabilitado()
        {
            var dic = new Dictionary<bool, List<int>>();
            var exercicioDTO = new ExercicioDTO();
            var simulado = new Exercicio();

            var matricula = Constants.CONTACTID_ACADEMICO;
            exercicioDTO.DtLiberacaoRanking = DateTime.Now.AddDays(1); //Data para o proximo dia
            exercicioDTO.Online = Convert.ToInt32(true);
            simulado.DataFim = Utilidades.DateTimeToUnixTimestamp(DateTime.Now.AddDays(1));
            simulado.DataInicio = Utilidades.DateTimeToUnixTimestamp(DateTime.Now.AddDays(-1));


            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaSimuladoOnlineDeveEstarHabilitado(exercicioDTO, simulado, dic, matricula);

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void ChecaSimuladoOnlineDeveEstarHabilitado_RMais_Datas_EntregaDesabilitado()
        {
            var dic = new Dictionary<bool, List<int>>();
            var exercicioDTO = new ExercicioDTO();
            var simulado = new Exercicio();

            var matricula = Constants.CONTACTID_ACADEMICO;
            exercicioDTO.DtLiberacaoRanking = DateTime.Now.AddDays(1); //Data para o proximo dia
            exercicioDTO.Online = Convert.ToInt32(true);
            simulado.DataFim = Utilidades.DateTimeToUnixTimestamp(DateTime.Now.AddDays(-1));
            simulado.DataInicio = Utilidades.DateTimeToUnixTimestamp(DateTime.Now.AddDays(1));


            var retorno = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).ChecaSimuladoOnlineDeveEstarHabilitado(exercicioDTO, simulado, dic, matricula);

            Assert.IsFalse(retorno);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetEspecialidadesSimulado_RMais_EntregaEspecialidades()
        {
            var simulados = new List<Exercicio>();
            var simulado = new ExercicioDTO();

            var mockIdSimulado = 1;

            simulado.ID = mockIdSimulado;

            //Mock especialidades
            simulados.Add(new Exercicio() { Especialidade = new Especialidade(), ID = mockIdSimulado });
            simulados.Add(new Exercicio() { Especialidade = new Especialidade(), ID = mockIdSimulado });

            var retEspecialidades = new SimuladoEntity().GetEspecialidadesSimulado(simulados, simulado);

            Assert.IsTrue(retEspecialidades.Count == 2);//Total mockado
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetEspecialidadesSimulado_RMais_EntregaVazio()
        {
            var simulados = new List<Exercicio>();
            var simulado = new ExercicioDTO();

            var mockIdSimulado = 1;

            simulado.ID = -1;//id inexistente

            //Mock especialidades
            simulados.Add(new Exercicio() { Especialidade = new Especialidade(), ID = mockIdSimulado });
            simulados.Add(new Exercicio() { Especialidade = new Especialidade(), ID = mockIdSimulado });

            var retEspecialidades = new SimuladoEntity().GetEspecialidadesSimulado(simulados, simulado);

            Assert.IsTrue(retEspecialidades.Count == 0);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetIdsExerciciosRealizadosAluno_RMaisClinica()
        {
            var retorno = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO, (int)Constants.TipoSimulado.R3_Clinica);

            if (retorno.Count > 0)
            {
                foreach (var item in retorno)
                {
                    Assert.IsTrue(item.Value.Count > 0);
                }
            }
            else
            {
                Assert.Inconclusive();
            }
            
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetIdsExerciciosRealizadosAluno_RMaisCirugia()
        {
            var retorno = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO, (int)Constants.TipoSimulado.R3_Cirurgia);

            if (retorno.Count > 0)
            {
                foreach (var item in retorno)
                {
                    Assert.IsTrue(item.Value.Count > 0);
                }
            }
            else
            {
                Assert.Inconclusive();
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetIdsExerciciosRealizadosAluno_RMaisPediatria()
        {
            var retorno = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO, (int)Constants.TipoSimulado.R3_Pediatria);

            if (retorno.Count > 0)
            {
                foreach (var item in retorno)
                {
                    Assert.IsTrue(item.Value.Count > 0);
                }
            }
            else
            {
                Assert.Inconclusive();
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetIdsExerciciosRealizadosAluno_RMaisR4GO()
        {
            var retorno = new ExercicioEntity().GetIdsExerciciosRealizadosAluno(Constants.CONTACTID_ACADEMICO, (int)Constants.TipoSimulado.R4_GO);

            if (retorno.Count > 0)
            {
                foreach (var item in retorno)
                {
                    Assert.IsTrue(item.Value.Count > 0);
                }
            }
            else
            {
                Assert.Inconclusive();
            }

        }

        private int UltimoSimuladoComQuestoes()
        {
            var simuladoID = 0;

            using (var ctx = new AcademicoContext())
            {
                simuladoID = (from qs in ctx.tblQuestao_Simulado
                              orderby qs.intSimuladoID descending
                              select qs.intSimuladoID).FirstOrDefault();

            }

            return simuladoID;
        }
        #endregion

		#region Simulado CPMED
		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetSimulado_CPMED2019_EspecialidadesAgrupadas_EntregaSimulado()
		{
			var matricula = Constants.CONTACTID_ACADEMICO;
			var ano = 2019;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = (int)Constants.TipoSimulado.CPMED;


            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());
            
            exercicioMock.GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado).Returns(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoBy2019CPMED());
            // simuladoMock.GetEspecialidadesSimulado(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoBy2019CPMED(), ExercicioEntityTestData.GetExercicioPeloAlunoByFilters2019CPMED())).Returns(ExercicioEntityTestData.GetEspacialidadeByFilters2019CPMED());
                                


            var simus = new SimuladoBusiness(new SimuladoEntity(), exercicioMock, new BannerEntity()).GetSimuladosCPMED(ano, matricula, idApp);

			Assert.IsNotNull(simus);
			Assert.IsTrue(simus.Count > 0);

			foreach (var item in simus)
			{
				Assert.IsTrue(item.Ano == ano);

				Assert.IsFalse(string.IsNullOrEmpty(item.Label));
				Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
				Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

				Assert.IsNotNull(item.Ativo);
				Assert.IsNotNull(item.IsPremium);

				Assert.IsTrue(item.IdTipoRealizacao > -1);
				Assert.IsTrue(item.Acertos > -1);
				Assert.IsTrue(item.Atual > -1);
				Assert.IsTrue(item.EntidadeApostilaID > -1);
				Assert.IsTrue(item.EstadoID > -1);
				Assert.IsTrue(item.HistoricoId > -1);
				Assert.IsTrue(item.IdConcurso > -1);
				Assert.IsTrue(item.Online > -1);
				Assert.IsTrue(item.Ordem > -1);
				Assert.IsTrue(item.QtdQuestoes > -1);
				Assert.IsTrue(item.Ranqueado > -1);
				Assert.IsTrue(item.Realizado > -1);
				Assert.IsTrue(item.RegiaoID > -1);
				Assert.IsTrue(item.StatusId > -1);
				Assert.IsTrue(item.TempoExcedido > -1);
				Assert.IsTrue(item.TempoTolerancia > -1);
				Assert.IsTrue(item.TipoApostilaId > -1);
				Assert.IsTrue(item.TipoId == idTipoSimulado);

				Assert.IsNotNull(item.ID);
				Assert.IsTrue(item.Duracao > 0);
				Assert.IsTrue(item.DataFim > 0);
				Assert.IsTrue(item.DataInicio > 0);
				Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
				Assert.IsNotNull(item.DtLiberacaoRanking);

				Assert.IsTrue(item.Especialidades.Count > 0);

				foreach (var especialidade in item.Especialidades)
				{
					Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

					Assert.IsNotNull(especialidade.Id);
					Assert.IsTrue(especialidade.IdAreaAcademica > -1);
					Assert.IsTrue(especialidade.IntEmployeeID > -1);

					Assert.IsNotNull(especialidade.Editavel);
					Assert.IsNotNull(especialidade.DataClassificacao);
				}

			}
		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetSimulado_CPMED_TipoInexistente_EntregaException()
		{
			var matricula = Constants.CONTACTID_ACADEMICO;
			var ano = 2019;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = -1;//Id inexistente
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());
            simuladoMock.GetEspecialidadesSimulado(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoBy2019CPMED(), ExercicioEntityTestData.GetExercicioPeloAlunoByFilters2019CPMED()).Returns(ExercicioEntityTestData.GetEspacialidadeByFilters2019CPMED());
            exercicioMock.GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado)
                                .Returns(new List<Exercicio>() { });

            try
			{
				var simus = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsNotNull(ex);
			}

		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetSimulado_CPMED_AnoInexistente_EntregaException()
		{
			var matricula = Constants.CONTACTID_ACADEMICO;
			var ano = -1;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = (int)Constants.TipoSimulado.CPMED;
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());
            simuladoMock.GetEspecialidadesSimulado(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoBy2019CPMED(), ExercicioEntityTestData.GetExercicioPeloAlunoByFilters2019CPMED()).Returns(ExercicioEntityTestData.GetEspacialidadeByFilters2019CPMED());
            exercicioMock.GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado)
                                .Returns(new List<Exercicio>() { });
            try
			{
				var simus = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsNotNull(ex);
			}

		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetSimulado_CPMED_MatriculaInexistente_EntregaException()
		{
			var matricula = -1;
			var ano = 2019;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = (int)Constants.TipoSimulado.CPMED;
            var simuladoMock = Substitute.For<ISimuladoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado)
                                .Returns(ExercicioEntityTestData.GetIdsExerciciosRealizadosAluno());
            simuladoMock.GetEspecialidadesSimulado(ExercicioEntityTestData.GetSimuladosRealizadosPeloAlunoBy2019CPMED(), ExercicioEntityTestData.GetExercicioPeloAlunoByFilters2019CPMED()).Returns(ExercicioEntityTestData.GetEspacialidadeByFilters2019CPMED());
            exercicioMock.GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado)
                                .Returns(new List<Exercicio>() { });
            try
			{
				var simus = new SimuladoBusiness(simuladoMock, exercicioMock, new BannerEntity()).GetSimuladoEspecialidadesAgrupadas(ano, matricula, idApp, idTipoSimulado);
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsNotNull(ex);
			}

		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetAnos_CPMED_EntregaAnos()
		{
			var matricula = Constants.CONTACTID_ACADEMICO;
			var tipoExercicio = Exercicio.tipoExercicio.SIMULADO;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = (int)Constants.TipoSimulado.CPMED;
            var produtoMock = Substitute.For<IProdutoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetAnosSimulados(matricula,true, idApp, idTipoSimulado)
                                .Returns(new List<int>() { 2019,2018 });
            produtoMock.GetProdutosContratadosPorAnoMatricula(matricula)
                                .Returns(ExercicioEntityTestData.GetListaProdutos());
            exercicioMock.IsProdutoSomenteMedeletro(ExercicioEntityTestData.GetListaProdutos()).Returns(false);
            var anos = new SimuladoBusiness(new SimuladoEntity(), exercicioMock, new BannerEntity(), produtoMock).GetAnosExerciciosPermitidos(tipoExercicio, matricula, idTipoSimulado, true, idApp);
       
            Assert.IsNotNull(anos);

			foreach (var item in anos)
			{
				Assert.IsTrue(item > 0);
			}
		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetAnos_CPMED_MatriculaInexistente_EntregaVazio()
		{
			var matricula = -1;
			var tipoExercicio = Exercicio.tipoExercicio.SIMULADO;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = (int)Constants.TipoSimulado.CPMED;
            var produtoMock = Substitute.For<IProdutoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetAnosSimulados(matricula, true, idApp, idTipoSimulado)
                                .Returns(new List<int>() {});
            produtoMock.GetProdutosContratadosPorAnoMatricula(matricula)
                                .Returns(new List<Produto>() { });
            exercicioMock.IsProdutoSomenteMedeletro(ExercicioEntityTestData.GetListaProdutos()).Returns(false);
            var anos = new SimuladoBusiness(new SimuladoEntity(), exercicioMock, new BannerEntity(), produtoMock).GetAnosExerciciosPermitidos(tipoExercicio, matricula, idTipoSimulado, true, idApp);
            

			Assert.IsTrue(anos.Count == 0);
		}

		[TestMethod]
		[TestCategory("Simulado CPMED")]
		public void GetAnos_CPMED_TipoSimuladoInexistente_EntregaVazio()
		{
			var matricula = Constants.CONTACTID_ACADEMICO;
			var tipoExercicio = Exercicio.tipoExercicio.SIMULADO;
			var idApp = (int)Aplicacoes.MsProMobile;
			var idTipoSimulado = -1;

            var produtoMock = Substitute.For<IProdutoData>();
            var exercicioMock = Substitute.For<IExercicioData>();

            exercicioMock.GetAnosSimulados(matricula, true, idApp, idTipoSimulado)
                                .Returns(new List<int>() { });
            produtoMock.GetProdutosContratadosPorAnoMatricula(matricula)
                                .Returns(new List<Produto>() { });
            exercicioMock.IsProdutoSomenteMedeletro(ExercicioEntityTestData.GetListaProdutos()).Returns(false);
            var anos = new SimuladoBusiness(new SimuladoEntity(), exercicioMock, new BannerEntity(), produtoMock).GetAnosExerciciosPermitidos(tipoExercicio, matricula, idTipoSimulado, true, idApp);

            Assert.IsTrue(anos.Count == 0);
		}

		#endregion

		#region Simulado Extensivo

		[TestMethod]
        [TestCategory("Simulado Extensivo")]
        public void GetAnosSimuladoExtensivo_MsProMobile_OrdemCrescente()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var idApp = (int)Aplicacoes.MsProMobile;

            var anos = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetAnosSimuladoExtensivo(matricula, idApp);
            Assert.IsNotNull(anos);
            Assert.IsTrue(anos.Count > 0);

            var anoAnterior = 0;
            foreach (var item in anos)
            {
                Assert.IsTrue(item >= anoAnterior, "Os anos devem ser ordenados de forma crescente");
                anoAnterior = item;
            }
        }

        [TestMethod]
        [TestCategory("Simulado Extensivo")]
        public void GetSimulado_Extensivo2018_EspecialidadesAgrupadas_EntregaSimulado()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2018;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.Extensivo;

            var simus = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosExtensivo(ano, matricula, idApp);
            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count == 9);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);

                Assert.IsFalse(string.IsNullOrEmpty(item.Label));
                Assert.IsFalse(string.IsNullOrEmpty(item.ExercicioName));
                Assert.IsFalse(string.IsNullOrEmpty(item.Descricao));

                Assert.IsNotNull(item.Ativo);
                Assert.IsNotNull(item.IsPremium);

                Assert.IsTrue(item.IdTipoRealizacao > -1);
                Assert.IsTrue(item.Acertos > -1);
                Assert.IsTrue(item.Atual > -1);
                Assert.IsTrue(item.EntidadeApostilaID > -1);
                Assert.IsTrue(item.EstadoID > -1);
                Assert.IsTrue(item.HistoricoId > -1);
                Assert.IsTrue(item.IdConcurso > -1);
                Assert.IsTrue(item.Online > -1);
                Assert.IsTrue(item.Ordem > -1);
                Assert.IsTrue(item.QtdQuestoes > -1);
                Assert.IsTrue(item.Ranqueado > -1);
                Assert.IsTrue(item.Realizado > -1);
                Assert.IsTrue(item.RegiaoID > -1);
                Assert.IsTrue(item.StatusId > -1);
                Assert.IsTrue(item.TempoExcedido > -1);
                Assert.IsTrue(item.TempoTolerancia > -1);
                Assert.IsTrue(item.TipoApostilaId > -1);
                Assert.IsTrue(item.TipoId == idTipoSimulado);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsTrue(item.DtUnixLiberacaoRanking > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);

                Assert.IsTrue(item.Especialidades.Count > 0);

                foreach (var especialidade in item.Especialidades)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(especialidade.Descricao));

                    Assert.IsTrue(especialidade.Id > 0);
                    Assert.IsTrue(especialidade.IdAreaAcademica > -1);
                    Assert.IsTrue(especialidade.IntEmployeeID > -1);

                    Assert.IsNotNull(especialidade.Editavel);
                    Assert.IsNotNull(especialidade.DataClassificacao);
                }
            }
        }

		#endregion

		#region MGEWeb Testes
		[TestMethod]
		[TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_GetSimuladoPorId()
		{
            var idSimulado = 641;

            _mockSimulado.GetSimuladoPorId(idSimulado)
                .Returns(SimuladoEntityTestData.GetSimuladosAnoAtual().FirstOrDefault(c => c.ID == idSimulado)
            );

            var simulado = _simuladoBusiness.GetSimuladoPorId(idSimulado);

            Assert.IsTrue(simulado != null);
        }

		[TestMethod]
		[TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_GetSimuladosPorAno()
		{
            var anoAtual = DateTime.Now.Year;

            _mockSimulado.GetSimuladosPorAno(anoAtual)
                .Returns(SimuladoEntityTestData.GetSimuladosAnoAtual()
            );

			var simulados = _simuladoBusiness.GetSimuladosPorAno(anoAtual);

			Assert.IsTrue(simulados != null && simulados.Count() > 0);
		}
        
        [TestMethod]
        [Ignore]
        [TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_AlterarSimulado()
		{
			var ret = 0;
			var simuladoBusiness = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity());
			var registros = simuladoBusiness.GetSimuladosPorAno(DateTime.Now.Year);
			SimuladoDTO simulado = null;

			if (registros != null && registros.Count > 0)
			{
				simulado = registros[0];

				ret = simuladoBusiness.Alterar(simulado);
			}

			Assert.IsTrue(ret == 1);
		}

		[TestMethod]
		[TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_GetTemasSimuladoPorAno()
		{
            var anoAtual = DateTime.Now.Year;

            _mockSimulado.GetTemasSimuladoPorAno(anoAtual)
                .Returns(SimuladoEntityTestData.GetTemasSimuladoAnoAtual()
            );

            var temas = _simuladoBusiness.GetTemasSimuladoPorAno(anoAtual);

			Assert.IsTrue(temas != null && temas.Count() > 0);
		}

		[TestMethod]
		[TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_GetTiposSimulado()
		{
            _mockSimulado.GetTiposSimulado()
                .Returns(SimuladoEntityTestData.GetTiposSimulado()
            );

            var tipos = _simuladoBusiness.GetTiposSimulado();

			Assert.IsTrue(tipos != null && tipos.Count() > 0);
		}

		[TestMethod]
		[TestCategory("MGEWeb - Simulado Nova Base")]
		public void MGEWeb_RDS_GetEspecialidadesSimulado()
		{
            var simuladoBusiness = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity());
			var especialidades = simuladoBusiness
				.GetEspecialidadesSimulado();

			Assert.IsTrue(especialidades != null && especialidades.Count > 0);
		}
		#endregion
	}
}