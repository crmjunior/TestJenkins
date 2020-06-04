using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesMockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class RankingSimuladoEntityTest
    {
        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetRankingSimuladoFaseObjetiva_SemFiltros_ProvaExistente()
        //{
        //    var ranking = new RankingSimuladoEntity().GetRankingSimuladoFaseObjetiva(119300, 591, string.Empty, string.Empty);
        //    Assert.IsNotNull(ranking);
        //}

        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetRankingSimuladoFaseObjetiva_FiltroEspecialidade_ProvaExistente()
        //{
        //    var ranking = new RankingSimuladoEntity().GetRankingSimuladoFaseObjetiva(119300, 591, "CLÍNICA MÉDICA", string.Empty);
        //    Assert.IsNotNull(ranking);
        //}

        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetRankingSimuladoFaseObjetiva_FiltroUf_ProvaExistente()
        //{
        //    var ranking = new RankingSimuladoEntity().GetRankingSimuladoFaseObjetiva(119300, 591, string.Empty, "13, 9");
        //    Assert.IsNotNull(ranking);
        //}

        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetRankingSimuladoFaseObjetiva_FiltroUfeEspecialidade_ProvaExistente()
        //{
        //    var ranking = new RankingSimuladoEntity().GetRankingSimuladoFaseObjetiva(119300, 591, "CLÍNICA MÉDICA", "13, 9");
        //    Assert.IsNotNull(ranking);
        //}

        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetRankingSimuladoFaseObjetiva_FiltroUfeEspecialidade_Prova2016()
        //{
        //    var ranking = new RankingSimuladoEntity().GetRankingSimuladoFaseObjetiva(96409, 602, "", "");
        //    Assert.IsNotNull(ranking);
        //}

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingFinalSimulado_Liberado_Prova2016()
        {
            var ranking = new RankingSimuladoEntity().IsFaseFinalLiberado(603);
            Assert.IsTrue(ranking);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetEstatisticaAlunoSimuladoOnline()
        {
            var estatistica = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(45476, 615, true);
            Assert.IsTrue(estatistica.TotalQuestoes == 50);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetEstatisticaAlunoSimuladoEstudo()
        {
            var estatistica = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(45476, 614, false);
            Assert.IsTrue(estatistica.TotalQuestoes == 100);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetEstatisticaAlunoSimuladoOnline_DiferenteEstudo()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var estatisticaOnline = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(238429, 629, true);
            var estatisticaEstudo = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(238429, 629, false);
            Assert.IsTrue(estatisticaOnline.TotalQuestoes == 50);
            Assert.IsTrue(estatisticaOnline.Acertos == 43);
            Assert.IsTrue(estatisticaOnline.Nota == 43);
            Assert.IsTrue(estatisticaOnline.Erros == 7);
            Assert.IsTrue(estatisticaOnline.NaoRealizadas == 0);

            //Assert.IsTrue(estatisticaEstudo.TotalQuestoes == 50);
            //Assert.IsTrue(estatisticaEstudo.Acertos == 44);
            //Assert.IsTrue(estatisticaEstudo.Nota == 44);
            //Assert.IsTrue(estatisticaEstudo.Erros == 6);
            //Assert.IsTrue(estatisticaEstudo.NaoRealizadas == 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetEstatisticaAlunoSimuladoOnline_AcertouAnulada()
        {
            var estatistica = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(248097, 629, true);
            Assert.IsTrue(estatistica.TotalQuestoes == 50);
            Assert.IsTrue(estatistica.Acertos == 25);
            Assert.IsTrue(estatistica.Nota == 25);
            Assert.IsTrue(estatistica.Erros == 25);
            Assert.IsTrue(estatistica.NaoRealizadas == 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetEstatisticaAlunoSimuladoOnline_ErrouAnulada()
        {
            var estatistica = new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(233311, 629, true);
            Assert.IsTrue(estatistica.TotalQuestoes == 50);
            Assert.IsTrue(estatistica.Acertos == 34);
            Assert.IsTrue(estatistica.Nota == 34);
            Assert.IsTrue(estatistica.Erros == 16);
            Assert.IsTrue(estatistica.NaoRealizadas == 0);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingObjetiva_SemFiltros_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, string.Empty, string.Empty);
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingObjetiva_FiltroEspecialidade_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, "CLÍNICA MÉDICA", string.Empty);
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingObjetiva_FiltroUf_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, string.Empty, "13, 9");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingObjetiva_FiltroUfeEspecialidade_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, "CLÍNICA MÉDICA", "13, 9");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRankingObjetiva_FiltroUfeEspecialidade_Prova2016()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(96409, 602, "", "");
            Assert.IsNotNull(ranking);
        }


        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_IsNotNull()
        {
            var ranking = new RankingSimuladoEntity().GetResultadoRankingAluno(180859, 614, 17, "", "", "Todos");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_SemFiltros_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, string.Empty, string.Empty, "Todos");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_FiltroEspecialidade_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, "CLÍNICA MÉDICA", string.Empty, "Todos");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_FiltroUf_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, string.Empty, "13, 9");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_FiltroUfeEspecialidade_ProvaExistente()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(119300, 591, "CLÍNICA MÉDICA", "13, 9");
            Assert.IsNotNull(ranking);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetResultadoRankingAluno_FiltroUfeEspecialidade_Prova2016()
        {
            var ranking = new RankingSimuladoEntity().GetRankingObjetiva(96409, 602, "", "");
            Assert.IsNotNull(ranking);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetFiltroRanking()
        {
            var ranking = new RankingSimuladoEntity().GetFiltroRankingSimulado(614);
            Assert.IsNotNull(ranking);
            Assert.IsTrue(ranking.Estados.Count == 28);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void DataDeLiberacaoDeRankingDeSimulado_Preenchida()
        {
            if (DateTime.Now.Month > 3)
            {
                var business = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity());

                var idAplicacaoRestrita = 1;
                var lstSimulados = business.GetSimuladoEspecialidadesAgrupadas(DateTime.Now.Year, 96409, idAplicacaoRestrita);

                foreach (var simulado in lstSimulados)
                {
                    Assert.IsNotNull(simulado.DtLiberacaoRanking);
                }
            }
            else
            {
                Assert.Inconclusive();
            }

        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetResultadoRankingAluno_CacheHabilitado_RetornaResultadoCacheado()
        {
            //apenas mocks, não é necessário valores reais.
            var matricula = 0;
            var idSimulado = 0;
            var idAplicacao = 0;
            var especialidae = string.Empty;
            var unidades = string.Empty;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            rankingMock.GetSimuladoConsolidado(matricula, idSimulado).Returns(new AlunoConcursoEstatistica());
            rankingMock.GetEstatisticaAlunoSimulado(matricula, idSimulado, true).Returns(new AlunoConcursoEstatistica());
            rankingMock.GetRankingObjetiva(matricula, idSimulado, especialidae, unidades).ReturnsNull();
            rankingMock.GetRankingObjetivaCache(matricula, idSimulado, especialidae, unidades).ReturnsNull();

            new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock).GetResultadoRankingAluno(matricula, idSimulado, idAplicacao, especialidae, unidades);

            rankingMock.Received().GetRankingObjetivaCache(matricula, idSimulado, especialidae, unidades);
        }
        [TestMethod]
        [TestCategory("Basico")]
        public void GetResultadoRankingAluno_EstatisticasAlunoRankingEstudo_RetornarTotalQuestoesMaiorZero()
        {
            //apenas mocks, não é necessário valores reais.
            var matricula = 0;
            var idSimulado = 0;
            var idAplicacao = 0;
            var especialidae = string.Empty;
            var unidades = string.Empty;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            rankingMock.GetSimuladoConsolidado(matricula, idSimulado).Returns(new AlunoConcursoEstatistica());
            rankingMock.GetEstatisticaAlunoSimulado(matricula, idSimulado, true).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(15, 84, 1));
            rankingMock.GetEstatisticaAlunoSimulado(matricula, idSimulado, false).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(15, 84, 1));
            rankingMock.GetRankingObjetiva(matricula, idSimulado, especialidae, unidades).ReturnsNull();
            rankingMock.GetRankingObjetivaCache(matricula, idSimulado, especialidae, unidades).ReturnsNull();



            var result = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock).GetResultadoRankingAluno(matricula, idSimulado, idAplicacao, especialidae, unidades);

            Assert.IsTrue(result.EstatisticasAlunoRankingEstudo.TotalQuestoes > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetFiltroRankingSimulado_EAD_RetornaUnidadeVazio()
        {
            //apenas mocks, não é necessário valores reais.
            var idSimulado = 0;

            var mockListaSimulado = new List<RankingDTO>();

            mockListaSimulado.Add(new RankingDTO()
            {
                intStateID = -1, //estado EAD
                txtUnidade = "EAD"
            });

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var storeMock = Substitute.For<IFilialData>();

            rankingMock.GetRankingParcial(idSimulado).Returns(mockListaSimulado);

            var filtro = new RankingSimuladoBusiness(rankingMock, new EspecialidadeEntity(), storeMock).GetFiltroRankingSimulado(idSimulado);

            Assert.IsTrue(filtro.Unidades.Count == 0);
            Assert.IsTrue(filtro.Estados.Count > 0);
            Assert.IsTrue(filtro.Especialidades.Count > 0);
        }

        [TestMethod]
        [TestCategory("Geração de Ranking")]
        public void GeraRankingSimuladoNacional_R3Cli_ValidarValoresRanking()
        {
            var idSimulado = 1;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();
            
            var simulado = new SimuladoDTO { 
                ID = 1,
                QuantidadeQuestoes = 10,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.R3_Clinica
            };

            rankingMock.GetSimulado(idSimulado).Returns(simulado);
            rankingMock.GetOrdemVendaTodosClientes(2019).Returns(RankingSimuladoEntityTestData.ListOrdemVendaTodosClientes(Constants.TipoSimulado.R3_Clinica));
            rankingMock.GetLogSimuladoAlunoTurma(idSimulado).Returns(RankingSimuladoEntityTestData.ListLogSimuladoAlunoTurma(Constants.TipoSimulado.R3_Clinica));
            especialidadeMock.GetAll().Returns(RankingSimuladoEntityTestData.ListEspecialidade());
            storesMock.GetFilial((int)Constants.Stores.R3_MEDWRITERS).Returns(new Filial(){ID = (int)Constants.Stores.R3_MEDWRITERS, Nome = "R3 MEDWRITERS"});

            var resultado = RankingSimuladoEntityTestData.ListResultado();
            rankingMock.ListResultado(idSimulado).Returns(resultado);

            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.GeraRankingSimuladoNacional(idSimulado);

            rankingMock.Received().RemoverSimuladoRankingFase01(idSimulado);
            rankingMock.Received().InserirSimuladoRankingFase01(Arg.Any<List<SimuladoRankingFase01DTO>>());

            Assert.AreEqual(resultado.Count(), ranking.Count);
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal < 0));
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal > simulado.PesoProvaObjetiva));

            int contador = 1;
            int posicao = 1;
            double? notaAnterior = -1;
            foreach (var item in ranking)
	        {
                var res = resultado.Where(x => x.intClientID == item.intClientID).FirstOrDefault();
                double nota = Math.Round(Convert.ToDouble(res.intAcertos) / simulado.QuantidadeQuestoes.Value * simulado.PesoProvaObjetiva.Value, 2);
                
                if(item.dblNotaFinal != notaAnterior){
                    posicao = contador;
                }

                Assert.AreEqual(nota, item.dblNotaFinal);
                Assert.AreEqual(posicao.ToString() + "º", item.txtPosicao);
                Assert.AreEqual("R3 MEDWRITERS", item.txtUnidade);
                Assert.AreEqual("2019 R3 CLÍNICA MÉDICA", item.txtLocal);
                Assert.AreEqual("CLÍNICA MÉDICA (ANO OPCIONAL)", item.txtEspecialidade);
                Assert.AreEqual(-1, item.intStateID);

                notaAnterior = item.dblNotaFinal;
                contador++;
	        }
        }

        [TestMethod]
        [TestCategory("Geração de Ranking")]
        public void GeraRankingSimuladoNacional_R3Cir_ValidarValoresRanking()
        {
            var idSimulado = 1;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 10,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.R3_Cirurgia
            };

            rankingMock.GetSimulado(idSimulado).Returns(simulado);
            rankingMock.GetOrdemVendaTodosClientes(2019).Returns(RankingSimuladoEntityTestData.ListOrdemVendaTodosClientes(Constants.TipoSimulado.R3_Cirurgia));
            rankingMock.GetLogSimuladoAlunoTurma(idSimulado).Returns(RankingSimuladoEntityTestData.ListLogSimuladoAlunoTurma(Constants.TipoSimulado.R3_Cirurgia));
            especialidadeMock.GetAll().Returns(RankingSimuladoEntityTestData.ListEspecialidade());
            storesMock.GetFilial((int)Constants.Stores.R3_MEDERI).Returns(new Filial() { ID = (int)Constants.Stores.R3_MEDERI, Nome = "R3 MEDERI" });

            var resultado = RankingSimuladoEntityTestData.ListResultado();
            rankingMock.ListResultado(idSimulado).Returns(resultado);

            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.GeraRankingSimuladoNacional(idSimulado);

            rankingMock.Received().RemoverSimuladoRankingFase01(idSimulado);
            rankingMock.Received().InserirSimuladoRankingFase01(Arg.Any<List<SimuladoRankingFase01DTO>>());

            Assert.AreEqual(resultado.Count(), ranking.Count);
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal < 0));
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal > simulado.PesoProvaObjetiva));

            int contador = 1;
            int posicao = 1;
            double? notaAnterior = -1;
            foreach (var item in ranking)
            {
                var res = resultado.Where(x => x.intClientID == item.intClientID).FirstOrDefault();
                double nota = Math.Round(Convert.ToDouble(res.intAcertos) / simulado.QuantidadeQuestoes.Value * simulado.PesoProvaObjetiva.Value, 2);

                if (item.dblNotaFinal != notaAnterior)
                {
                    posicao = contador;
                }

                Assert.AreEqual(nota, item.dblNotaFinal);
                Assert.AreEqual(posicao.ToString() + "º", item.txtPosicao);
                Assert.AreEqual("R3 MEDERI", item.txtUnidade);
                Assert.AreEqual("2019 R3 CIRURGIA", item.txtLocal);
                Assert.AreEqual("CIRURGIA GERAL (PROGRAMA AVANÇADO)", item.txtEspecialidade);
                Assert.AreEqual(-1, item.intStateID);

                notaAnterior = item.dblNotaFinal;
                contador++;
            }
        }

        [TestMethod]
        [TestCategory("Geração de Ranking")]
        public void GeraRankingSimuladoNacional_R3Ped_ValidarValoresRanking()
        {
            var idSimulado = 1;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 10,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.R3_Pediatria
            };

            rankingMock.GetSimulado(idSimulado).Returns(simulado);
            rankingMock.GetOrdemVendaTodosClientes(2019).Returns(RankingSimuladoEntityTestData.ListOrdemVendaTodosClientes(Constants.TipoSimulado.R3_Pediatria));
            rankingMock.GetLogSimuladoAlunoTurma(idSimulado).Returns(RankingSimuladoEntityTestData.ListLogSimuladoAlunoTurma(Constants.TipoSimulado.R3_Pediatria));
            especialidadeMock.GetAll().Returns(RankingSimuladoEntityTestData.ListEspecialidade());
            storesMock.GetFilial((int)Constants.Stores.R3_MEDERI).Returns(new Filial() { ID = (int)Constants.Stores.R3_MEDERI, Nome = "R3 MEDERI" });

            var resultado = RankingSimuladoEntityTestData.ListResultado();
            rankingMock.ListResultado(idSimulado).Returns(resultado);

            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.GeraRankingSimuladoNacional(idSimulado);

            rankingMock.Received().RemoverSimuladoRankingFase01(idSimulado);
            rankingMock.Received().InserirSimuladoRankingFase01(Arg.Any<List<SimuladoRankingFase01DTO>>());

            Assert.AreEqual(resultado.Count(), ranking.Count);
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal < 0));
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal > simulado.PesoProvaObjetiva));

            int contador = 1;
            int posicao = 1;
            double? notaAnterior = -1;
            foreach (var item in ranking)
            {
                var res = resultado.Where(x => x.intClientID == item.intClientID).FirstOrDefault();
                double nota = Math.Round(Convert.ToDouble(res.intAcertos) / simulado.QuantidadeQuestoes.Value * simulado.PesoProvaObjetiva.Value, 2);

                if (item.dblNotaFinal != notaAnterior)
                {
                    posicao = contador;
                }

                Assert.AreEqual(nota, item.dblNotaFinal);
                Assert.AreEqual(posicao.ToString() + "º", item.txtPosicao);
                Assert.AreEqual("R3 MEDERI", item.txtUnidade);
                Assert.AreEqual("2019 R3 PEDIATRIA", item.txtLocal);
                Assert.AreEqual("PEDIATRIA (ANO OPCIONAL)", item.txtEspecialidade);
                Assert.AreEqual(-1, item.intStateID);

                notaAnterior = item.dblNotaFinal;
                contador++;
            }
        }

        [TestMethod]
        [TestCategory("Validação de geração automática do Ranking")]
        public void ValidaRankingAutomatico_RoboFinalizacaoSimulado_RankingNaoGerado()
        {
            var idSimulado = 1;
            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 10,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.Extensivo
            };

            rankingMock.GetRankingSimulado(idSimulado).Returns(new List<SimuladoRankingFase01DTO>());
            rankingMock.GetSimulado(idSimulado).Returns(simulado);

            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.ValidaRankingSimulado(idSimulado);

            Assert.IsTrue(ranking.Contains("Sem registro de Ranking para o simulado ID"));
        }

        [TestMethod]
        [TestCategory("Validação de geração automática do Ranking")]
        public void ValidaRankingAutomatico_RoboFinalizacaoSimulado_GabaritaramMais10Porcento()
        {
            var idSimulado = 1;
            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 100,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.Extensivo
            };

            rankingMock.GetRankingSimulado(idSimulado).Returns(RankingSimuladoEntityTestData.ListRankingSimulado_20PorcentoGabaritou());
            rankingMock.GetSimulado(idSimulado).Returns(simulado);


            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.ValidaRankingSimulado(idSimulado);

            Assert.IsTrue(ranking.Contains("Quantidade de alunos que gabaritaram é superior a 10%"));
        }

        [TestMethod]
        [TestCategory("Validação de geração automática do Ranking")]
        public void ValidaRankingAutomatico_RoboFinalizacaoSimulado_GabaritaramMenos10Porcento()
        {
            var idSimulado = 1;
            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 100,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.Extensivo
            };

            rankingMock.GetRankingSimulado(idSimulado).Returns(RankingSimuladoEntityTestData.ListRankingSimulado_10PorcentoGabaritou());
            rankingMock.GetSimulado(idSimulado).Returns(simulado);


            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.ValidaRankingSimulado(idSimulado);

            Assert.AreEqual(ranking, "");
        }

        [TestMethod]
        [TestCategory("Geração de Ranking")]
        public void GeraRankingSimuladoNacional_R4GO_ValidarValoresRanking()
        {
            var idSimulado = 1;

            var rankingMock = Substitute.For<IRankingSimuladoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var storesMock = Substitute.For<IFilialData>();

            var simulado = new SimuladoDTO
            {
                ID = 1,
                QuantidadeQuestoes = 10,
                PesoProvaObjetiva = 100,
                Ano = 2019,
                TipoId = (int)Constants.TipoSimulado.R4_GO
            };

            rankingMock.GetSimulado(idSimulado).Returns(simulado);
            rankingMock.GetOrdemVendaTodosClientes(2019).Returns(RankingSimuladoEntityTestData.ListOrdemVendaTodosClientes(Constants.TipoSimulado.R4_GO));
            rankingMock.GetLogSimuladoAlunoTurma(idSimulado).Returns(RankingSimuladoEntityTestData.ListLogSimuladoAlunoTurma(Constants.TipoSimulado.R4_GO));
            especialidadeMock.GetAll().Returns(RankingSimuladoEntityTestData.ListEspecialidade());
            storesMock.GetFilial((int)Constants.Stores.R3_MEDERI).Returns(new Filial() { ID = (int)Constants.Stores.R3_MEDERI, Nome = "R3 MEDERI" });

            var resultado = RankingSimuladoEntityTestData.ListResultado();
            rankingMock.ListResultado(idSimulado).Returns(resultado);

            var business = new RankingSimuladoBusiness(rankingMock, especialidadeMock, storesMock);
            var ranking = business.GeraRankingSimuladoNacional(idSimulado);

            rankingMock.Received().RemoverSimuladoRankingFase01(idSimulado);
            rankingMock.Received().InserirSimuladoRankingFase01(Arg.Any<List<SimuladoRankingFase01DTO>>());

            Assert.AreEqual(resultado.Count(), ranking.Count);
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal < 0));
            Assert.IsFalse(ranking.Any(x => x.dblNotaFinal > simulado.PesoProvaObjetiva));

            int contador = 1;
            int posicao = 1;
            double? notaAnterior = -1;
            foreach (var item in ranking)
            {
                var res = resultado.Where(x => x.intClientID == item.intClientID).FirstOrDefault();
                double nota = Math.Round(Convert.ToDouble(res.intAcertos) / simulado.QuantidadeQuestoes.Value * simulado.PesoProvaObjetiva.Value, 2);

                if (item.dblNotaFinal != notaAnterior)
                {
                    posicao = contador;
                }

                Assert.AreEqual(nota, item.dblNotaFinal);
                Assert.AreEqual(posicao.ToString() + "º", item.txtPosicao);
                Assert.AreEqual("R3 MEDERI", item.txtUnidade);
                Assert.AreEqual("2019 R4 GO", item.txtLocal);
                Assert.AreEqual("GINECOLOGIA E OBSTETRÍCIA (ANO OPCIONAL)", item.txtEspecialidade);
                Assert.AreEqual(-1, item.intStateID);

                notaAnterior = item.dblNotaFinal;
                contador++;
            }
        }        
    }
}