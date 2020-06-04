using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using NSubstitute;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccessTests.EntitiesMockData;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Repository;
using MedCore_DataAccessTests.EntitiesDataTests;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_API.Academico;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class ExercicioEntityTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAnos_SimuladoNotNull()
        {
            var anos = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, 211904);
            Assert.IsNotNull(anos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAnos_ConcursoNotNull()
        {
            var anos = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.CONCURSO, 96409);
            Assert.IsNotNull(anos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAnos_ApostilaNotNull()
        {
            var anos = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.APOSTILA, 96409);
            Assert.IsNotNull(anos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetExercicios_Simulados2013NotNull()
        {
            var exercicios = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, 2013, 96409);
            Assert.IsNotNull(exercicios);
            Assert.IsInstanceOfType(exercicios, typeof(List<Exercicio>));
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetExercicios_Concursos2013NotNull()
        // {
        //     var exercicios = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.CONCURSO, 2013, 96409);
        //     Assert.IsNotNull(exercicios);
        //     Assert.IsInstanceOfType(exercicios, typeof(List<Exercicio>));
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetExercicios_Apostilas2013NotNull()
        {
            var exercicios = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.APOSTILA, 2013, 96409);
            Assert.IsNotNull(exercicios);
            Assert.IsInstanceOfType(exercicios, typeof(List<Exercicio>));
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Intermediario")]
        public void CanGetAtualizacaoExercicio_Apostila_Preventiva_2013()
        {
            var exercicio = new ExercicioEntity().GetAtualizacaoDados("CF61B649-40A8-42CF-9E29-B34E39FDFE95");
            Assert.IsInstanceOfType(exercicio, typeof(Exercicio));
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumAcerto_NotNull()
        // {
        //     var acertos = new ExercicioEntity().GetForumAcertos(901975, 0);
        //     Assert.IsInstanceOfType(acertos, typeof(List<ForumProva.Acerto>));
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumAcerto_ComEspecialidade_NotNull()
        // {
        //     var acertos = new ExercicioEntity().GetForumAcertos(901975, 32);
        //     Assert.IsInstanceOfType(acertos, typeof(List<ForumProva.Acerto>));
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumComentario_NotNull()
        // {
        //     var comentario = new ExercicioEntity().GetForumComentarios(906865, 10, 0);
        //     Assert.IsInstanceOfType(comentario, typeof(List<ForumProva.Comentario>));
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetSimualdosPermitidos_NotNull()
        {
            var alunonovo = new ExercicioEntity().GetSimuladosPermitidos(173010, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var acad = new ExercicioEntity().GetSimuladosPermitidos(96409, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var aluno = new ExercicioEntity().GetSimuladosPermitidos(150420, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var pedro = new ExercicioEntity().GetSimuladosPermitidos(119300, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var cancelado = new ExercicioEntity().GetSimuladosPermitidos(192062, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var ativoEcancelado = new ExercicioEntity().GetSimuladosPermitidos(192167, 1, false, (int)Constants.TipoSimulado.Extensivo);
            var ativo = new ExercicioEntity().GetSimuladosPermitidos(195491, 1, false, (int)Constants.TipoSimulado.Extensivo);


            Assert.IsInstanceOfType(aluno, typeof(List<Exercicio>));
        }
        //_____________________________________________________________________________________________________________________________________________________
        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetConcursoDeUmEstado_NotNull()
        // {
        //     var concurso = new ExercicioEntity().GetConcursosPorEstados("4");
        //     Assert.IsNotNull(concurso);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetConcursoDeVariosEstado_NotNull()
        // {
        //     var concursos = new ExercicioEntity().GetConcursosPorEstados("4,5,6");
        //     Assert.IsNotNull(concursos);
        // }


        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanSetConcursoFavorito()
        // {
        //     var concursoFavoritado = new ConcursoFavoritado
        //     {
        //         ConcursoId = 26416,
        //         Matricula = 72048
        //     };

        //     var teste = new ExercicioEntity().SetStatusProvaFavorita(concursoFavoritado.ConcursoId, concursoFavoritado.Matricula);
        //     Assert.AreEqual(1, teste);

        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetConcursoFavorito()
        {
            var concursoFavoritado = new ConcursoFavoritado
            {
                ConcursoId = 26416,
                Matricula = 72048
            };

            var teste = new ExercicioEntity().GetProvasFavoritas(concursoFavoritado.Matricula);
            Assert.IsNotNull(teste);

        }


        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetProvaComPrazo()
        // {

        //     var prazo = new ExercicioEntity().GetProvaPrazo(922064);
        //     Assert.IsNotNull(prazo);

        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetProvaSemPrazo()
        // {

        //     var prazo = new ExercicioEntity().GetProvaPrazo(46);
        //     Assert.IsNotNull(prazo);

        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetProvaDataNaoDefinida()
        // {

        //     var prazo = new ExercicioEntity().GetProvaPrazo(921440).Prazo;
        //     var mensagem = Constants.RECURSOS_PRAZO_INDEFINIDO;

        //     Assert.AreEqual(mensagem, prazo);
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void SetAcertosForumProva()
        {
            List<ForumProva.Acerto> lstAcertos = new List<ForumProva.Acerto>();
            lstAcertos.Add(new ForumProva.Acerto()
            {
                Acertos = 3,
                Especialidade = new Especialidade() { Id = 32 }
            });

            var forum = new ForumProva()
            {
                Prova = new Exercicio() { ID = 921430 },
                Acertos = lstAcertos,
                Matricula = 96409,
                Ip = "100.200.300"
            };

            var retorno = new ExercicioEntity().SetAcertosForumProva(forum);

            Assert.AreEqual(1, retorno);
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void MarcarDesmarcarFavorita()
        // {
        //     var prova = new ConcursoFavoritado()
        //     {
        //         ConcursoId = 921429,
        //         Matricula = 96409
        //     };

        //     var resultado = new ExercicioEntity().SetStatusProvaFavorita(prova.ConcursoId, prova.Matricula);

        //     Assert.AreEqual(1, resultado);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumAcertosPermissao_ComPermissao()
        // {
        //     if (Utilidades.IsSiteRecursosInativo())
        //     {
        //         Assert.Inconclusive();
        //     }
        //     var permissao = new ExercicioEntity().GetAcertosForumProvaPermissao(922782, 1);
        //     Assert.AreEqual(1, permissao);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumAcertosPermissao_SemPermissao()
        // {
        //     var permissao = new ExercicioEntity().GetAcertosForumProvaPermissao(402, 86156);

        //     Assert.AreEqual(0, permissao);
        // }


        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetForumProvaTotalAcertosEnviados()
        // {
        //     var total = new ExercicioEntity().GetForumProvaTotalAcertosEnviados(402);

        //     Assert.IsInstanceOfType(total, typeof(int));
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void CanGetAnosConcursosRecursos_NotNull()
        // {
        //     var anos = new ExercicioEntity().GetAnosConcursosRecursos();
        //     Assert.IsNotNull(anos);
        //     Assert.IsInstanceOfType(anos, typeof(List<int>));
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetConcursos_ConsiderandoBloqueio()
        // {
        //     var concursos = new ExercicioEntity().GetConcursosConsiderandoBloqueio();

        //     Assert.IsNotNull(concursos);

        //     Assert.IsInstanceOfType(concursos, typeof(List<Exercicio>));
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoConfiguracao_Sim602_NotNull()
        {
            var sim = new ExercicioEntity().GetSimuladoConfiguracao(602, 119300, 17, "0.0.0");

            Assert.IsNotNull(sim);
            Assert.IsInstanceOfType(sim, typeof(Simulado));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoOnlineConfiguracao_Sim603_AlunoPrimeiraVez_NotNull()
        {
            var simid = 603;
            var aluno = 119300;
            Utilidades.LimpaRegistroPraTesteUnitario("tblExercicio_Historico", string.Format("intclientid = {0} and intExercicioID = {1} and intExercicioTipo = 1", aluno, simid), Utilidades.GetChaveamento());
            Utilidades.LimpaRegistroPraTesteUnitario("tblExercicio_Historico", string.Format("intclientid = {0} and intExercicioID = {1} and intExercicioTipo = 1", aluno, simid), Utilidades.GetChaveamento());
            var sim = new ExercicioEntity().GetSimuladoOnlineConfiguracao(simid, aluno, 17);

            Assert.IsNotNull(sim);
            Assert.IsInstanceOfType(sim, typeof(Simulado));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladoOnlineConfiguracao_Sim603_AlunoVoltandoAtempo_NotNull()
        {
            var sim = new ExercicioEntity().GetSimuladoOnlineConfiguracao(603, 119300, 17);

            Assert.IsNotNull(sim);
            Assert.IsInstanceOfType(sim, typeof(Simulado));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void GetApostilaConfiguracao_NotNull()
        {
            RedisCacheManager.DeleteAllKeys();
            var ExercicioID = CartaoRepostaEntityTestData.GetExercicioIDValidoQuestoesApostila();
            var matricula = 96409;

            var apostila = new ExercicioEntity().GetApostilaConfiguracaoCache(ExercicioID, matricula, (int)Aplicacoes.MsProMobile);

            Assert.IsNotNull(apostila);
            Assert.IsInstanceOfType(apostila, typeof(Apostila));
        }
        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetApostilaConfiguracao_ApostilaNome_ToUpper_ReplaceMATERIALVIRTUAL()
        // {

        //     var matricula = new PerfilAlunoEntityTestData().GetAlunoAcademico().ID;
        //     var ano = Utilidades.GetYear();
        //     var ExercicioID = CartaoRepostaEntityTestData.GetExercicioIDValidoQuestoesApostilabyAnoMatriculaProduto((int)Produto.Produtos.MEDELETRO_IMED, matricula, ano);

        //     if(ExercicioID == -1)
        //     {
        //         var anoAnterior = DateTime.Now.Year - 1;
        //         ExercicioID = CartaoRepostaEntityTestData.GetExercicioIDValidoQuestoesApostilabyAnoMatriculaProduto((int)Produto.Produtos.MEDELETRO_IMED, matricula, anoAnterior);

        //         if (ExercicioID == -1)
        //             Assert.Inconclusive();
        //     }

        //     var apostila = new ExercicioEntity().GetApostilaConfiguracaoCache(ExercicioID, matricula, (int)Aplicacoes.MsProMobile);

        //     Assert.IsTrue(!apostila.Nome.Contains(" - MATERIAL VIRTUAL"));
        //     Assert.IsTrue(apostila.Nome.Count(c => char.IsLower(c)) == 0);
            
           
        // }


        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void GetProvaPersonalizadaConfiguracao_NotNull()
        {
            var prova = new ExercicioEntity().GetProvaPersonalizadaConfiguracao(124, 241676, 17);

            Assert.IsNotNull(prova);
            Assert.IsInstanceOfType(prova, typeof(Concurso));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterConfiguracaoSimuladoModoProva_NovaProva()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock  = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var matricula = 241665;
            var simulado = 631;

            exercicioMock.ObterSimulado(simulado).Returns(new TblSimuladoDTO() {
                txtSimuladoDescription = "Simulado Teste",
                intQtdQuestoes = 50,
                intQtdQuestoesCasoClinico = 0,
                intDuracaoSimulado = 60
            });

            exercicioMock.InserirExercicioSimulado(simulado, matricula, 17, TipoProvaEnum.ModoProva).Returns(new ExercicioHistoricoDTO() { intHistoricoExercicioID = 1 });

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var config = business.GetSimuladoModoProvaConfiguracao(simulado, matricula, 17);

            exercicioMock.Received().InserirExercicioSimulado(simulado, matricula, 17, TipoProvaEnum.ModoProva);

            Assert.AreEqual(config.Duracao, 60);
            Assert.AreEqual(config.DuracaoEmSeg, 3600);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterConfiguracaoSimuladoModoProva_ProvaEmAndamento()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var matricula = 241665;
            var simulado = 631;

            exercicioMock.ObterSimulado(simulado).Returns(new TblSimuladoDTO()
            {
                txtSimuladoDescription = "Simulado Teste",
                intQtdQuestoes = 50,
                intQtdQuestoesCasoClinico = 0,
                intDuracaoSimulado = 60
            });

            exercicioMock.InserirExercicioSimulado(simulado, matricula, 17, TipoProvaEnum.ModoProva).Returns(new ExercicioHistoricoDTO() { intHistoricoExercicioID = 1 });
            exercicioMock.ObterUltimoExercicioSimuladoModoProva(matricula, simulado).Returns(new ExercicioHistoricoDTO() { intHistoricoExercicioID = 1, dteDateInicio = DateTime.Now.AddMinutes(-10) });

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var config = business.GetSimuladoModoProvaConfiguracao(simulado, matricula, 17);

            exercicioMock.DidNotReceive().InserirExercicioSimulado(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<TipoProvaEnum>());

            Assert.AreNotEqual(config.Duracao, 60);
            Assert.AreNotEqual(config.DuracaoEmSeg, 3600);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterRankingModoProva()
        {
            var business =  new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());

            var matricula = 267711;
            var simulado = 641;
            var idHistorico = 23043863;

            var ranking = business.GetResultadoRankingModoProva(matricula, simulado, idHistorico, (int)Aplicacoes.MsProMobile);

            Assert.AreEqual(ranking.QuantidadeParticipantes, 13596);
        }

        // [TestMethod]
        // [TestCategory("Unitario")]
        // public void ObterRankingModoProvaAlunoDeveSerOPrimeiroQuandoRankingVazio()
        // {
        //     var exercicioMock = Substitute.For<IExercicioData>();
        //     var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
        //     var simuladoMock = Substitute.For<ISimuladoData>();
        //     var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
        //     var questaoMock = Substitute.For<IQuestaoData>();
        //     var aulaMock = Substitute.For<IAulaEntityData>();

        //     rankingSimuladoMock.GetEstatisticaAlunoSimuladoModoProva(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(15,84,1));
        //     rankingSimuladoMock.GetEstatisticaAlunoSimulado(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(15, 84, 1));


        //     exercicioMock.ObterRankingPorSimulado(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(ExercicioEntityTestData.ObterRankingPorSimuladoVazio());
        //     exercicioMock.ObterExercicio(Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterExercicioHistoricoRealizado());
        //     exercicioMock.ObterQuantidadeQuestoesSimuladoOnline(Arg.Any<int>()).Returns(100);

        //     var matricula = 241665;
        //     var simulado = 631;

        //     var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);

        //     var ranking = business.GetResultadoRankingModoProva(matricula, simulado, 9742836, 17);

        //     Assert.AreEqual("1.5", ranking.Nota);
        //     Assert.AreEqual("1º", ranking.Posicao);
        // }

        [TestMethod]
        [TestCategory("Unitario")]
        public void ObterRankingModoProvaAlunoDeveSerOPrimeiroQuandoGabaritar()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            rankingSimuladoMock.GetEstatisticaAlunoSimuladoModoProva(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(100, 0, 0));
            rankingSimuladoMock.GetEstatisticaAlunoSimulado(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(100, 0, 0));


            exercicioMock.ObterRankingPorSimulado(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(ExercicioEntityTestData.ObterRankingPorSimulado10AlunosBemColocados());
            exercicioMock.ObterExercicio(Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterExercicioHistoricoRealizado());
            exercicioMock.ObterQuantidadeQuestoesSimuladoOnline(Arg.Any<int>()).Returns(100);

            var matricula = 241665;
            var simulado = 631;

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);

            var ranking = business.GetResultadoRankingModoProva(matricula, simulado, 9742836, 17);

            Assert.AreEqual("10", ranking.Nota);
            Assert.AreEqual("1º", ranking.Posicao);
        }

        [TestMethod]
        [TestCategory("Unitario")]
        public void ObterRankingModoProvaAlunoDeveSerUltimoQuandoZerar()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            rankingSimuladoMock.GetEstatisticaAlunoSimuladoModoProva(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(0,100,0));
            rankingSimuladoMock.GetEstatisticaAlunoSimulado(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(ExercicioEntityTestData.ObterEstatisticaAlunoSimulado(0, 100, 0));


            exercicioMock.ObterRankingPorSimulado(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(ExercicioEntityTestData.ObterRankingPorSimulado10AlunosBemColocados());
            exercicioMock.ObterExercicio(Arg.Any<int>()).Returns(ExercicioEntityTestData.ObterExercicioHistoricoRealizado());
            exercicioMock.ObterQuantidadeQuestoesSimuladoOnline(Arg.Any<int>()).Returns(100);

            var matricula = 241665;
            var simulado = 631;

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);

            var ranking = business.GetResultadoRankingModoProva(matricula, simulado, 9742836, 17);

            Assert.AreEqual("0", ranking.Nota);
            Assert.AreEqual("11º", ranking.Posicao);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioInexistente()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.DidNotReceive().ObterSimulado(Arg.Any<int>());
            exercicioMock.DidNotReceive().AlunoJaRealizouSimuladoOnline(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterSimuladoAlunoExcecao(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterQuestoes(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimuladoOnline(Arg.Any<List<CartaoRespostaObjetivaDTO>>());
            exercicioMock.DidNotReceive().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());

            Assert.AreEqual(0, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoEstudo()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = false,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = false,
                intSimuladoID = simuladoId
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().ObterSimulado(simuladoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);

            exercicioMock.DidNotReceive().ObterSimuladoAlunoExcecao(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterQuestoes(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimuladoOnline(Arg.Any<List<CartaoRespostaObjetivaDTO>>());
            exercicioMock.DidNotReceive().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnlineDentroDoPrazo()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = false,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now,
                intHistoricoExercicioID = historicoId
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(-2),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(2)
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);
            exercicioMock.ObterQuestoesOnline(historicoId).Returns(new List<CartaoRespostaObjetivaDTO>());

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().ObterSimulado(simuladoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);
            exercicioMock.Received().ObterSimuladoAlunoExcecao(matricula, simuladoId);
            exercicioMock.Received().RegistrarSimuladoOnline(historicoId);
            exercicioMock.Received().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.Received().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnlineDentroDoPrazoAlunoExcecao()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = false,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(5),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(8)
            };

            var simuladoExcecao = new SimuladoOnlineExcecaoDTO()
            {
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(-2),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(2)
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);
            exercicioMock.ObterSimuladoAlunoExcecao(matricula, simuladoId).Returns(simuladoExcecao);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().ObterSimulado(simuladoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);
            exercicioMock.Received().ObterSimuladoAlunoExcecao(matricula, simuladoId);
            exercicioMock.Received().RegistrarSimuladoOnline(historicoId);
            exercicioMock.Received().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.Received().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnlineDentroDoPrazo_SimuladoJaRealizado()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = false,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(5),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(8)
            };

            var simuladoExcecao = new SimuladoOnlineExcecaoDTO()
            {
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(-2),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(2)
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(true);
            exercicioMock.ObterSimuladoAlunoExcecao(matricula, simuladoId).Returns(simuladoExcecao);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);

            exercicioMock.DidNotReceive().ObterSimulado(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterSimuladoAlunoExcecao(Arg.Any<int>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnlineForaDoPrazo()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>(); 

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = false,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(5),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(8)
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.SetFinalizaExercicio(exercicio);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);
            exercicioMock.Received().ObterSimulado(simuladoId);
            exercicioMock.Received().ObterSimuladoAlunoExcecao(matricula, simuladoId);

            exercicioMock.DidNotReceive().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnline_HorarioEsgotado()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;
            bool finalizarOnlinePorTempo = true;

            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = true,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now,
                dteDateFim = null
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddMinutes(-2),
                dteDataHoraTerminoWEB = DateTime.Now.AddMinutes(1)
            };

            var simuladoDTO = new SimuladoDTO()
            {
                ID = simuladoId,
                Duracao = -2
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);
            simuladoMock.GetSimulado(simuladoId).Returns(simuladoDTO);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            var retorno = service.SetFinalizaExercicio(exercicio, finalizarOnlinePorTempo);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.Received().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);
            exercicioMock.Received().ObterSimulado(simuladoId);
            exercicioMock.Received().ObterSimuladoAlunoExcecao(matricula, simuladoId);

            exercicioMock.Received().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.Received().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.Received().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FinalizarExercioSimuladoOnline_ComTempoRestante()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var simuladoId = 628;
            var matricula = 241771;
            var historicoId = 1245632;
            bool finalizarOnlinePorTempo = true;
            var exercicio = new Exercicio()
            {
                HistoricoId = historicoId
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                bitRealizadoOnline = true,
                intClientID = matricula,
                intExercicioID = simuladoId,
                intExercicioTipo = 1,
                dteDateInicio = DateTime.Now
            };

            var simulado = new TblSimuladoDTO()
            {
                bitOnline = true,
                intSimuladoID = simuladoId,
                dteDataHoraInicioWEB = DateTime.Now.AddHours(5),
                dteDataHoraTerminoWEB = DateTime.Now.AddHours(8)
            };

            var simuladoDTO = new SimuladoDTO()
            {
                ID = simuladoId,
                Duracao = 200
            };

            exercicioMock.ObterExercicio(historicoId).Returns(exercicioHistorico);
            exercicioMock.ObterSimulado(simuladoId).Returns(simulado);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, simuladoId).Returns(false);
            simuladoMock.GetSimulado(simuladoId).Returns(simuladoDTO);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            var retorno = service.SetFinalizaExercicio(exercicio, finalizarOnlinePorTempo);

            exercicioMock.Received().ObterExercicio(historicoId);
            exercicioMock.DidNotReceive().AlunoJaRealizouSimuladoOnline(matricula, simuladoId);
            exercicioMock.DidNotReceive().ObterSimulado(simuladoId);
            exercicioMock.DidNotReceive().ObterSimuladoAlunoExcecao(matricula, simuladoId);

            exercicioMock.DidNotReceive().RegistrarSimuladoOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().ObterQuestoesOnline(Arg.Any<int>());
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.DidNotReceive().ReplicarSimuladoOnlineTabelasMGE(Arg.Any<int>(), Arg.Any<int>());

            Assert.AreEqual(0, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimulado_SolicitandoSimuladosAnoAtualPorAlunoExtensivoAnoAtualParaExibicaoNaAreaRestrita_NaoDeveRetornarSimuladoComDataLiberacaoRankingQueAindaNaoChegou()
        {
            var ano = Utilidades.GetYear();

            var business = new PerfilAlunoEntityTestData();
            var matricula = business.GetAlunoExtensivoAnoAtualAtivo();

            var exercicios = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, ano, matricula.ID, (int)Aplicacoes.AreaRestrita);
            if (exercicios.Count() == 0)
            {
                Assert.Inconclusive("Ainda não há simulados liberados para o Ano");
            }
            else
            {
                Assert.AreEqual(false, exercicios.Any(x => x.DtLiberacaoRanking > DateTime.Now));
            }
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimulado_SolicitandoSimuladosAnoAtualPorAlunoExtensivoAnoAtualParaExibicaoMedsoft_DeveRetornarTodosSimuladosPermitidos()
        {
            var ano = Utilidades.GetYear();

            var business = new PerfilAlunoEntityTestData();
            var matricula = business.GetAlunoExtensivoAnoAtualAtivo();

            var exercicios = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, ano, matricula.ID, (int)Aplicacoes.MsProMobile, true);
            var hasSimuladosQueAindaNaoForamRealizadosOnline = exercicios.Any(x => x.DtLiberacaoRanking > Utilidades.UnixTimeStampToDateTime(x.DataFim));
            if (exercicios.Count() == 0 || !hasSimuladosQueAindaNaoForamRealizadosOnline)
            {
                Assert.Inconclusive();
            }
            else
            {
                Assert.AreEqual(true, hasSimuladosQueAindaNaoForamRealizadosOnline);
            }
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimulado_SolicitandoSimuladosAnoAtualModoEstudoPorAlunoExtensivoAnoAtualAtivoParaExibicaoAreaRestrita_RetornaTambemUltimoSimuladoDisponivelParaModoEstudo()
        {
            var dataAtual = Utilidades.GetServerDate();
            var anoAtual = dataAtual.Year;

            using (var ctx = new AcademicoContext())
            {
                var ultimoSimuladoDisponivelParaEstudo = ctx.tblSimulado.Where(s => s.intAno == anoAtual && s.intTipoSimuladoID == (int)Exercicio.tipoExercicio.SIMULADO
                && s.dteReleaseGabarito < dataAtual
                ).OrderByDescending(s=>s.dteReleaseGabarito).FirstOrDefault();

                var business = new PerfilAlunoEntityTestData();
                var matricula = business.GetAlunoExtensivoAnoAtualAtivo();

                var simulados = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, anoAtual, matricula.ID, (int)Aplicacoes.AreaRestrita);
                if (simulados.Count() == 0)
                {
                    Assert.Inconclusive("Ainda não há simulados liberados para o Ano");
                }
                else
                {
                    Assert.IsTrue(simulados.Count() > 0);
                    Assert.IsTrue(simulados.Select(s => s.ID).Contains(ultimoSimuladoDisponivelParaEstudo.intSimuladoID));
                }
            }
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetSimulado_SolicitandoPrimeiroSimuladoAnoAnteriorModoEstudoPorAlunoExtensivoAnoAtualAtivoParaExibicaoAreaRestrita_RetornaTambemPrimeiroSimuladoAnoAnterior()
        // {
        //     var anoAnterior = Utilidades.GetYear() - 1;
        //     using (var ctx = new AcademicoContext())
        //     {
        //         var primeiroSimuladoAnoAnterior = ctx.tblSimulado.Where(s => s.intSimuladoOrdem == 1 && s.intAno == anoAnterior && s.intTipoSimuladoID == (int)Exercicio.tipoExercicio.SIMULADO).FirstOrDefault();

        //         var business = new PerfilAlunoEntity();
        //         var matricula = business.GetAlunoExtensivoAnoAtualAtivo();

        //         var simulados = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, anoAnterior, matricula.ID, (int)Aplicacoes.AreaRestrita);

        //         Assert.IsTrue(simulados.Count() > 0);
        //         Assert.IsTrue(simulados.Exists(x => x.ID == primeiroSimuladoAnoAnterior.intSimuladoID));
        //     }
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetSimulado_SolicitandoSimuladoAnoAnteriorModoEstudoPorAlunoExtensivoAnoAtualAtivoParaMSPro_RetornaTambemPrimeiroSimuladoAnoAnterior()
        // {
        //     var anoAnterior = Utilidades.GetYear() - 1;
        //     using (var ctx = new AcademicoContext())
        //     {
        //         var primeiroSimuladoAnoAnterior = ctx.tblSimulado.Where(s => s.intSimuladoOrdem == 1 && s.intAno == anoAnterior && s.intTipoSimuladoID == (int)Exercicio.tipoExercicio.SIMULADO).FirstOrDefault();

        //         var business = new PerfilAlunoEntity();
        //         var matricula = business.GetAlunoExtensivoAnoAtualAtivo();

        //         var simulados = new ExercicioEntity().GetByFilters(Exercicio.tipoExercicio.SIMULADO, anoAnterior, matricula.ID, (int)Aplicacoes.MsProMobile);

        //         Assert.IsTrue(simulados.Count() > 0);
        //         Assert.IsTrue(simulados.Exists(x => x.ID == primeiroSimuladoAnoAnterior.intSimuladoID));
        //     }
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void ChecaRegraMedEletroParaSimuladoAnoCorrente()
        // {
        //     if (Utilidades.GetYear().Equals(Constants.anoInscricaoMedeletro +1 ))
        //     {
        //         int anoCorrente = Utilidades.GetYear();
        //         int applicationNumber = (int)Aplicacoes.MsProMobile;
        //         Aluno aluno;
        //         List<int> anosSimulados;

        //         //Pegar um aluno com OV somente medEletro - Ano Corrente // Nenhuma outra OV
        //         aluno = new AlunoEntity().GetAluno("11464042713");
        //         //SIM deve Exibir o Simulado do Ano corrente
        //         anosSimulados = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, aluno.ID, true, applicationNumber);
        //         Assert.IsTrue(anosSimulados.Exists(x => x == anoCorrente));

        //         //Pegar um aluno com OV somente medEletro - Ano Corrente - Outras OVs anos anteriores
        //         aluno = new AlunoEntity().GetAluno("84783958300");
        //         //SIM deve Exibir o Simulado do Ano corrente
        //         anosSimulados = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, aluno.ID, true, applicationNumber);
        //         Assert.IsTrue(anosSimulados.Exists(x => x == anoCorrente));

        //         //Pegar um aluno com OV somente medEletro - Ano Corrente - Outras OVs DE ANO CORRENTE
        //         aluno = new AlunoEntity().GetAluno("17518611805");
        //         //DEVE Exibir o Simulado do Ano corrente
        //         anosSimulados = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, aluno.ID, true, applicationNumber);
        //         Assert.IsTrue(anosSimulados.Exists(x => x == anoCorrente));
        //     }
        //     else {
        //         Assert.Inconclusive();
        //     }
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void ChecaSimuladoRetornaDuracaoExataEmSegundosNotNull()
        {
            var matricula = 241718; //Matrícula de Teste
            var idAplicacao = (int)Aplicacoes.MsProMobile;

            var simulado = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetSimuladoOnlineCorrente();

            if (simulado == null)
            {
                Assert.Inconclusive();
            }

            var idSimuladoOnlineCorrente = simulado.ID;
            var exercicioSimulado = new ExercicioEntity().GetSimuladoOnlineConfiguracao(Convert.ToInt32(idSimuladoOnlineCorrente), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));


            Assert.IsNotNull(exercicioSimulado.DuracaoEmSeg);
            

        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void ChecaSimuladoRetornaDuracaoExataEmSegundosCoerenteComMinutos()
        // {

        //     //var matricula = 96409;
        //     var dataTolerancia = Utilidades.DataToleranciaTestes();

        //     if (DateTime.Now <= dataTolerancia)
        //     {
        //         Assert.Inconclusive();
        //     }

        //     var matricula = 241718; //Matrícula de Teste
        //     var idAplicacao = (int)Aplicacoes.MsProMobile;
        //     var simulado = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
        //         .GetSimuladoOnlineCorrente();

        //     if (simulado == null)
        //     {
        //         Assert.Inconclusive();
        //     }

        //     var idSimuladoOnlineCorrente = simulado.ID;
        //     var exercicioSimulado = new ExercicioEntity().GetSimuladoOnlineConfiguracao(Convert.ToInt32(idSimuladoOnlineCorrente), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));

        //     //Minuto e Segundos devem estar coerentes
        //     var IsMinutoArredondadoOuTruncadoEsperado = ((int)(exercicioSimulado.DuracaoEmSeg / 60) == exercicioSimulado.Duracao)
        //         || ((int)((exercicioSimulado.DuracaoEmSeg / 60) + 1) == exercicioSimulado.Duracao)
        //         || ((int)((exercicioSimulado.DuracaoEmSeg / 60) - 1) == exercicioSimulado.Duracao);

        //     Assert.IsTrue(IsMinutoArredondadoOuTruncadoEsperado);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void ChecaRetornoExerciciosNaoFinalizadosSimuladoAgendado()
        // {
        //     Assert.Inconclusive();
        //     var _exercicioBusiness = new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            
        //     var simuladoAgendado = _exercicioBusiness.GetSimuladoOnlineCorrente();
        //     //TODO: Colocar algoritmo para so rodar este teste quando nao estivermos em periodo de simulado, pois a interacao do aluno nos exercicios afeta este teste.
        //     if (simuladoAgendado == null)
        //          Assert.Inconclusive();

        //      var exercicioSimuladosNaoFinalizados = _exercicioBusiness.GetExerciciosSimuladoNaoFinalizados(simuladoAgendado.ID);

        //      foreach (var exercicioId in exercicioSimuladosNaoFinalizados)
        //      {
        //          Assert.IsTrue(_exercicioBusiness.ObterExercicio(exercicioId).dteDateFim == null);
        //      }
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetAnosExerciciosContratados__PassandoMatriculaExtensivoAnoAtual_TipoExercicioSimulado_DataRankingPrimeiroSimuladoAnoAtualJaPassou_RetornaAnoAtual()
        // {
        //     var exercicioEntity = new ExercicioEntity();
        //     var dataAtual = Utilidades.GetServerDate();
        //     var anoAtual = dataAtual.Year;

        //     using (var ctx = new AcademicoContext())
        //     {
        //         var primeiroSimulado = ctx.tblSimulado.Where(s => s.intSimuladoOrdem == 1 && s.intAno == anoAtual && s.intTipoSimuladoID == (int)Exercicio.tipoExercicio.SIMULADO).FirstOrDefault();

        //         if (primeiroSimulado == null)
        //             Assert.Inconclusive();

        //         var gabaritoJaFoiLiberado = primeiroSimulado.dteReleaseGabarito < dataAtual;

        //         if (!gabaritoJaFoiLiberado)
        //         {
        //             Assert.Inconclusive();
        //         }

        //         var alunoExtensivoAnoAtual = new PerfilAlunoEntity().GetAlunoExtensivoAnoAtualAtivo();
        //         var anosExerciciosContratados = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, alunoExtensivoAnoAtual.ID);

        //         Assert.IsTrue(anosExerciciosContratados.Contains(anoAtual));
        //     }
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetAnosExerciciosContratados__PassandoMatriculaExtensivoAnoAtual_TipoExercicioSimulado_DataRankingPrimeiroSimuladoAnoAtualAindaNaoPassou_NaoRetornaAnoAtual()
        // {
        //     var dataAtual = Utilidades.GetServerDate();
        //     var anoAtual = dataAtual.Year;

        //     using (var ctx = new AcademicoContext())
        //     {
        //         var primeiroSimulado = ctx.tblSimulado.Where(s => s.intSimuladoOrdem == 1 && s.intAno == anoAtual && s.intTipoSimuladoID == (int)Exercicio.tipoExercicio.SIMULADO).FirstOrDefault();

        //         if (primeiroSimulado == null)
        //             Assert.Inconclusive();

        //         var gabaritoJaFoiLiberado = primeiroSimulado.dteReleaseGabarito < dataAtual;

        //         if (gabaritoJaFoiLiberado)
        //         {
        //             Assert.Inconclusive();
        //         }

        //         var alunoExtensivoAnoAtual = new PerfilAlunoEntity().GetAlunoExtensivoAnoAtualAtivo();
        //         var anosExerciciosContratados = new ExercicioEntity().GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, alunoExtensivoAnoAtual.ID);

        //         Assert.IsTrue(!anosExerciciosContratados.Contains(anoAtual));
        //     }
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladosIniciados_SemSimulados()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            

            var matricula = Constants.CONTACTID_ACADEMICO;
            var aplicacao = (int)Aplicacoes.MsProMobile;
            exercicioMock.ObterExerciciosEmAndamento(matricula, aplicacao).Returns(new List<ExercicioHistoricoDTO>());
            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.GetSimuladosIniciados(matricula, aplicacao);
            
            exercicioMock.Received().ObterExerciciosEmAndamento(matricula, aplicacao);
            Assert.IsNull(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladosIniciados_ComSimulados()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var aplicacao = (int)Aplicacoes.MsProMobile;

            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simulados = new List<ExercicioHistoricoDTO>();
            var exercicio = new ExercicioHistoricoDTO()
            {
                intExercicioID = 1,
                intApplicationID = aplicacao,
            };

            var sim = new TblSimuladoDTO()
            {
                intSimuladoID = 1
            };

            simulados.Add(exercicio);
            
            exercicioMock.ObterExerciciosEmAndamento(matricula, aplicacao).Returns(simulados);
            exercicioMock.ObterSimulado(exercicio.intExercicioID).Returns(sim);

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.GetSimuladosIniciados(matricula, aplicacao);

            exercicioMock.Received().ObterExerciciosEmAndamento(matricula, aplicacao);
            exercicioMock.Received().ObterSimulado(exercicio.intExercicioID);
            Assert.IsNotNull(retorno);           
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetSimuladosIniciados_ComSimuladosExpirados_ValidarChamadaFinalizacao()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var aplicacao = (int)Aplicacoes.MsProMobile;

            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simulados = new List<ExercicioHistoricoDTO>();
            var exercicio = new ExercicioHistoricoDTO()
            {
                intHistoricoExercicioID = 100,
                intClientID = matricula,
                intExercicioID = 1,
                intApplicationID = aplicacao,
                intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO,
                intTipoProva = (int)TipoProvaEnum.ModoOnline,
                dteDateInicio = DateTime.Now.AddHours(-4.5),
                bitRealizadoOnline = true
            };

            var sim = new TblSimuladoDTO()
            {
                intSimuladoID = 1,
                intDuracaoSimulado = 240,
                dteDataHoraInicioWEB = DateTime.Now.AddDays(-1),
                dteDataHoraTerminoWEB = DateTime.Now.AddDays(1),
                bitOnline = true
            };

            simulados.Add(exercicio);

            exercicioMock.ObterExerciciosEmAndamento(matricula, aplicacao).Returns(simulados);
            exercicioMock.ObterSimulado(exercicio.intExercicioID).Returns(sim);
            exercicioMock.ObterExercicio(exercicio.intHistoricoExercicioID).Returns(exercicio);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, exercicio.intExercicioID).Returns(false);
            exercicioMock.ObterSimulado(exercicio.intExercicioID).Returns(sim);
            simuladoMock.GetSimulado(exercicio.intExercicioID).Returns(new SimuladoDTO { ID = sim.intSimuladoID, Duracao = sim.intDuracaoSimulado });

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.GetSimuladosIniciados(matricula, aplicacao);

            exercicioMock.Received().ObterExerciciosEmAndamento(matricula, aplicacao);
            exercicioMock.Received().ObterSimulado(exercicio.intExercicioID);

            exercicioMock.Received().RegistrarSimuladoOnline(exercicio.intHistoricoExercicioID);
            exercicioMock.Received().ObterQuestoesOnline(exercicio.intHistoricoExercicioID);
            exercicioMock.DidNotReceive().InserirQuestoesSimulado(Arg.Any<List<CartaoRespostaObjetivaDTO>>(), Arg.Any<int>());
            exercicioMock.Received().ReplicarSimuladoOnlineTabelasMGE(matricula, exercicio.intExercicioID);
            exercicioMock.Received().FinalizarExercicio(Arg.Any<Exercicio>());

            Assert.IsNull(retorno);
        }

        [TestMethod]
        [TestCategory("Simulado_Andamento")]
        public void GetSimuladosIniciados_ComSimuladosExpirados_ValidarCamposRetornadosNaModal()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var aplicacao = (int)Aplicacoes.MsProMobile;

            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
            var simuladoMock = Substitute.For<ISimuladoData>();
            var cartaoRespostaMock = Substitute.For<ICartaoRespostaData>();
            var questaoMock = Substitute.For<IQuestaoData>();
            var aulaMock = Substitute.For<IAulaEntityData>();

            var simulados = new List<ExercicioHistoricoDTO>();
            var exercicio = new ExercicioHistoricoDTO()
            {
                intHistoricoExercicioID = 100,
                intClientID = matricula,
                intExercicioID = 1,
                intApplicationID = aplicacao,
                intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO,
                intTipoProva = (int)TipoProvaEnum.ModoOnline,
                dteDateInicio = DateTime.Now,
                bitRealizadoOnline = true
            };

            var sim = new TblSimuladoDTO()
            {
                intSimuladoID = 1,
                intDuracaoSimulado = 240,
                dteDataHoraInicioWEB = DateTime.Now.AddDays(1),
                dteDataHoraTerminoWEB = DateTime.Now.AddDays(+1),
                bitOnline = true,
                txtSimuladoName = "Teste Simulado",
                intTipoSimuladoID = (int) Constants.TipoSimulado.Extensivo
            };

            simulados.Add(exercicio);

            exercicioMock.ObterExerciciosEmAndamento(matricula, aplicacao).Returns(simulados);
            exercicioMock.ObterSimulado(exercicio.intExercicioID).Returns(sim);
            exercicioMock.ObterExercicio(exercicio.intHistoricoExercicioID).Returns(exercicio);
            exercicioMock.AlunoJaRealizouSimuladoOnline(matricula, exercicio.intExercicioID).Returns(false);
            exercicioMock.ObterSimulado(exercicio.intExercicioID).Returns(sim);
            simuladoMock.GetSimulado(exercicio.intExercicioID).Returns(new SimuladoDTO { ID = sim.intSimuladoID, Duracao = sim.intDuracaoSimulado });

            var service = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, simuladoMock, cartaoRespostaMock, questaoMock, aulaMock);
            var retorno = service.GetSimuladosIniciados(matricula, aplicacao);

            exercicioMock.Received().ObterExerciciosEmAndamento(matricula, aplicacao);
            exercicioMock.Received().ObterSimulado(exercicio.intExercicioID);

            Assert.IsNotNull(retorno);            
            Assert.AreEqual(retorno.IdHistorico, exercicio.intHistoricoExercicioID);
            Assert.AreEqual(retorno.IdSimulado, sim.intSimuladoID);
            Assert.AreEqual(retorno.NomeSimulado, sim.txtSimuladoName);
            Assert.AreEqual(retorno.TipoProva, "Simulado Agendado");
            Assert.AreEqual(retorno.TipoSimulado, sim.intTipoSimuladoID);
        }

        #region Simulado RMais
        [TestMethod]
        [TestCategory("SimuladoRmais")]
        public void GetSimuladosByFilters_RMais_EntregaListaExercicio()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            var simus = new ExercicioEntity().GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado);

            Assert.IsNotNull(simus);
            Assert.IsTrue(simus.Count > 0);

            foreach (var item in simus)
            {
                Assert.IsTrue(item.Ano == ano);
                
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
                Assert.IsTrue(item.TipoId > -1);

                Assert.IsTrue(item.ID > 0);
                Assert.IsTrue(item.Duracao > 0);
                Assert.IsTrue(item.DataFim > 0);
                Assert.IsTrue(item.DataInicio > 0);
                Assert.IsNotNull(item.DtLiberacaoRanking);
            }
        }


        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimuladosByFilters_RMais_TipoInexistente_EntregaException()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = -1;//Id inexistente

            try
            {
                var simus = new ExercicioEntity().GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimuladosByFilters_RMais_AnoInexistente_EntregaException()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var ano = -1;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            try
            {
                var simus = new ExercicioEntity().GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetSimuladosByFilters_RMais_MatriculaInexistente_EntregaException()
        {
            var matricula = -1;
            var ano = 2019;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            try
            {
                var simus = new ExercicioEntity().GetSimuladosByFilters(ano, matricula, idApp, true, idTipoSimulado);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetAnosSimuladosTipoR_RMais_EntregaAnos()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = (int)Constants.TipoSimulado.R3_Cirurgia;

            var anos = new ExercicioEntity().GetAnosSimulados(matricula, true, idApp, idTipoSimulado);

            Assert.IsNotNull(anos);

            foreach (var item in anos)
            {
                Assert.IsTrue(item > 0);
            }
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void GetAnosSimuladosTipoR_RMais_TipoSimuladoInexistente_EntregaVazio()
        {
            var matricula = Constants.CONTACTID_ACADEMICO;
            var idApp = (int)Aplicacoes.MsProMobile;
            var idTipoSimulado = -1;

            var anos = new ExercicioEntity().GetAnosSimulados(matricula, true, idApp, idTipoSimulado);

            Assert.IsTrue(anos.Count == 0);
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void IsProdutoSomenteMedeletro_RMais_EntregaSomenteMedeletro()
        {
            var mockSomenteMedeltero = new List<Produto>();
            mockSomenteMedeltero.Add(new Produto() { IDProduto = (int)Produto.Produtos.MEDELETRO });

            Assert.IsTrue(new ExercicioEntity().IsProdutoSomenteMedeletro(mockSomenteMedeltero));            
        }

        [TestMethod]
        [TestCategory("SimuladoRMais")]
        public void IsProdutoSomenteMedeletro_RMais_EntregaOutrosProdutos()
        {
            var mockSomenteMedeltero = new List<Produto>();
            mockSomenteMedeltero.Add(new Produto() { IDProduto = (int)Produto.Produtos.MED });
            mockSomenteMedeltero.Add(new Produto() { IDProduto = (int)Produto.Produtos.R3CIRURGIA });

            Assert.IsFalse(new ExercicioEntity().IsProdutoSomenteMedeletro(mockSomenteMedeltero));
        }

        #endregion


        // [TestMethod]
        // [TestCategory("Prova_Impressa")]
        // public void GetConcursoConfiguracao_ConcursoProvaR1_PermiteImpressao()
        // {
        //     var exercicioMock = Substitute.For<IExercicioData>();
        //     var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();
        //     var Concurso = ExercicioEntityTestData.GetConcursoConfiguracaoR1();
        //     exercicioMock.GetProvaPersonalizadaConfiguracao(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(Concurso);
        //     var Idconcurso = 924913;
        //     var matricula = 267711;
        //     var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
        //     var Retorno = business.GetProvaPersonalizadaConfiguracao(Idconcurso,matricula,(int)Aplicacoes.MsProMobile );
        //     Assert.AreEqual(1, Retorno.PermissaoProva.Impressao);
        // }

        // [TestMethod]
        // [TestCategory("Prova_Impressa")]
        // public void GetConcursoConfiguracao_ConcursoProvaR3_PermiteImpressao()
        // {
        //     var aluno = new PerfilAlunoEntityTestData().GetAlunoR3();
        //     var concurso = new ConcursoEntity().GetConcursosR3(aluno.ID).FirstOrDefault();

        //     var retorno = new ExercicioEntity().GetConcursoConfiguracao(concurso, aluno.ID, (int)Aplicacoes.MsProMobile);
        //     Assert.AreEqual(1, retorno.PermissaoProva.Impressao);
        // }


        // [TestMethod]
        // [TestCategory("Prova_Impressa")]
        // public void GetUrlProvaImpressa_ConcursoProvaR3_UrlValida()
        // {
        //     var aluno = new PerfilAlunoEntityTestData().GetAlunoR3();
        //     var concurso = new ConcursoEntity().GetConcursosR3(aluno.ID).FirstOrDefault();

        //     var urlValida = Constants.URLPROVAIMPRESSA.Replace("IDEXERCICIO", concurso.ToString()).Replace("TIPOEXERCICIO", "Concurso").Replace("NOME", "");

        //     var retorno = ExercicioEntity.GetUrlProvaImpressa(concurso, "Concurso");
        //     Assert.IsTrue((!string.IsNullOrEmpty(retorno)));
        //     Assert.IsTrue(retorno.Contains(urlValida));
        // }

        // [TestMethod]
        // [TestCategory("Acertos prova")]
        // public void EnviarAcertosProvaConcurso_EnviaDadosCorretosBanco_DadosIguaisMock()
        // {
        //     var matricula = 1;
        //     var especialidade = 15;
        //     var idProva = 7;
        //     var acertos = 77;
        //     var ip = "123.456.789";

        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetIdEspecialidade(matricula).Returns(especialidade);
        //     mock.SetAcertosForumProva(Arg.Any<ForumProva>().Returns(1);

        //     var result = business.EnviarAcertosProvaConcurso(idProva, acertos, matricula, ip);

        //     mock.Received().GetIdEspecialidade(matricula);
        //     mock.Received().SetAcertosForumProva(
        //         Arg.Any<ForumProva>.Matches(
        //             b => b.Acertos.ElementAt(0).Acertos == acertos
        //                 && b.Acertos.ElementAt(0).Especialidade.Id == especialidade
        //                 && b.Matricula == matricula && b.Prova.ID == idProva
        //                 && b.Ip == ip
        //         )
        //     );
            
        //     mock.VerifyAllExpectations();
        //     Assert.AreEqual(1, result);
        // }

        // [TestMethod]
        // [TestCategory("Acertos prova")]
        // public void GetRankingAcertosProva_RetornaProva_DadosProva()
        // {
        //     var matricula = 1;
        //     var idProva = 7;
        //     var nomeProva = "UNISO - SP";
        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null);

        //     mockQuestao.GetProvaConcurso((int)idProva).Returns(new ProvaConcursoDTO
        //     {
        //         Nome = nomeProva,
        //         Ano = DateTime.Now.Year
        //     });

        //     mock.GetForumAcertos(idProva, default(int), matricula).Returns(new List<ForumProva.Acerto>());

        //     var result = business.GetRankingAcertosProva(idProva,  matricula);

        //     mockQuestao.AssertWasCalled(a => a.GetProvaConcurso(idProva));
        //     mock.VerifyAllExpectations();
        //     Assert.IsNotNull(result);
        //     Assert.IsNotNull(result.Prova);
        //     Assert.AreEqual(DateTime.Now.Year, result.Prova.Ano);
        //     Assert.AreEqual(nomeProva, result.Prova.Nome);
        // }

        [TestMethod]
        [TestCategory("Combo Simulados Realizados")]
        public void GetComboSimuladosRealizados_ComboRealizadoDesktop_RetornarComboRelizadoOnline()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();

            var Matricula = 1;
            var SimuladoID = 1;
            var AplicacaoID = (int) Aplicacoes.MsProDesktop;

            exercicioMock.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID).Returns(new List<ExercicioHistoricoDTO>());

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            var retorno = business.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            exercicioMock.Received().GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            Assert.IsNotNull(retorno);
        }

        [TestMethod]
        [TestCategory("Combo Simulados Realizados")]
        public void GetComboSimuladosRealizados_ComboRealizadoDesktopElectron_RetornarComboRelizadoOnline()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();

            var Matricula = 1;
            var SimuladoID = 1;
            var AplicacaoID = (int)Aplicacoes.MEDSOFT_PRO_ELECTRON;

            exercicioMock.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID).Returns(new List<ExercicioHistoricoDTO>());

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            var retorno = business.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            exercicioMock.Received().GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            Assert.IsNotNull(retorno);
        }

        [TestMethod]
        [TestCategory("Combo Simulados Realizados")]
        public void GetComboSimuladosRealizados_ComboRealizadoMobile_RetornarComboRelizadoOnline()
        {
            var exercicioMock = Substitute.For<IExercicioData>();
            var rankingSimuladoMock = Substitute.For<IRankingSimuladoData>();

            var Matricula = 1;
            var SimuladoID = 1;
            var AplicacaoID = (int)Aplicacoes.MsProMobile;

            exercicioMock.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID).Returns(new List<ExercicioHistoricoDTO>());

            var business = new ExercicioBusiness(exercicioMock, rankingSimuladoMock, new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
            var retorno = business.GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            exercicioMock.Received().GetComboSimuladosRealizados(Matricula, SimuladoID, AplicacaoID);

            Assert.IsNotNull(retorno);
        }

        // [TestMethod]
        // [TestCategory("Acertos prova")]
        // public void GetRankingAcertosProva_AgrupamentoAcertoEspecialidade_Quatro()
        // {
        //     var matricula = 1;
        //     var idProva = 7;
        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null);

        //     mockQuestao.GetProvaConcurso((int)idProva).Returns(new ProvaConcursoDTO());

        //     var dataAcertos = ExercicioEntityTestData.GetAcertosEspecialidadeProva();
        //     mock.GetForumAcertos(idProva, default(int), matricula).Returns(dataAcertos);

        //     var result = business.GetRankingAcertosProva(idProva, matricula);

        //     mockQuestao.Received().GetProvaConcurso(idProva);
        //     mock.Received().GetForumAcertos(idProva, default(int), matricula);

        //     mock.VerifyAllExpectations();
        //     Assert.IsNotNull(result);
        //     Assert.IsNotNull(result.AcertoEspecialidade);
        //     Assert.IsTrue(result.AcertoEspecialidade.Any(a => a.Acertos == 50 && a.QtdAlunos == 2));
        //     Assert.AreEqual(4, result.AcertoEspecialidade.Count);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetComentariosForumProva_QuantidadeCorreta_Tres()
        // {
        //     var matricula = 2;
        //     var idProva = 7;
        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();

        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null);

        //     var comentarios = ExercicioEntityTestData.GetComentariosForumProva();
        //     mock.GetForumComentarios(idProva, short.MaxValue, 0F, matricula).Returns(comentarios);
        //     mockQuestao.GetProvaConcurso(idProva).Returns(new ProvaConcursoDTO());

        //     var result = business.GetComentariosForumProva(idProva, matricula);

        //     mock.Received().GetForumComentarios(idProva, short.MaxValue, 0F, matricula);

        //     mock.VerifyAllExpectations();
        //     Assert.IsNotNull(result);
        //     Assert.AreEqual(3, result.Count);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetComentariosForumProva_MarcaComentarioAluno_ComentarioMarcado()
        // {
        //     var matricula = 2;
        //     var idProva = 7;
        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();

        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null);

        //     var comentarios = ExercicioEntityTestData.GetComentariosForumProva();
        //     mock.GetForumComentarios(idProva, short.MaxValue, 0F, matricula).Returns(comentarios);
        //     mockQuestao.GetProvaConcurso(idProva).Returns(new ProvaConcursoDTO());

        //     var result = business.GetComentariosForumProva(idProva, matricula);

        //     mock.Received().GetForumComentarios(idProva, short.MaxValue, 0F, matricula);

        //     mock.VerifyAllExpectations();
        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.Any(c => c.Autor));
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetComentariosForumProva_ComentarioComUf_NomeComUf()
        // {
        //     var matricula = 2;
        //     var idProva = 7;
        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();

        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null);

        //     var comentarios = ExercicioEntityTestData.GetComentariosForumProva();
        //     mock.GetForumComentarios(idProva, short.MaxValue, 0F, matricula).Returns(comentarios);
        //     mockQuestao.GetProvaConcurso(idProva).Returns(new ProvaConcursoDTO());

        //     var result = business.GetComentariosForumProva(idProva, matricula);

        //     mock.Received().GetForumComentarios(idProva, short.MaxValue, 0F, matricula);

        //     mock.VerifyAllExpectations();
        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.All(a => a.Nome.Contains(" - ")));
        //     Assert.IsTrue(result.ElementAt(0).Nome.EndsWith("- SP"));
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void EnviarComentarioForumProva_EnviaDadosCorretosBanco_DadosIguaisMock()
        // {
        //     var matricula = 1;
        //     var especialidade = 15;
        //     var idProva = 7;
        //     var texto = "Comentario 1";
        //     var ip = "123.456.789";

        //     var mock = Substitute.For<IExercicioData>();
        //     var mockQuestao = Substitute.For<IQuestaoData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, mockQuestao, null, null);

        //     mockQuestao.GetProvaConcurso(idProva).Returns((ProvaConcursoDTO) null);
        //     mock.GetIdEspecialidade(matricula).Returns(especialidade);
        //     mock.SetComentarioForumProva(Arg.Any<ForumProva>()).Returns(1);

        //     var result = business.EnviarComentarioForumProva(idProva, texto, matricula, ip);

        //     mock.Received().GetIdEspecialidade(matricula);
        //     mock.Received().SetComentarioForumProva(
        //         Arg.Is<ForumProva>(
        //             b => b.Comentarios.ElementAt(0).ComentarioTexto == texto
        //                 && b.Comentarios.ElementAt(0).Especialidade.Id == especialidade
        //                 && b.Matricula == matricula && b.Prova.ID == idProva
        //                 && b.Ip == ip
        //         )
        //     );

        //     Assert.AreEqual(1, result);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaFavorito_ConcursoFavorito()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var concursoFavorito = 3;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {

        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, GrandeAreas = new List<int> { 39 } },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, GrandeAreas = new List<int> { 161 } },
        //         new ProvaRecursoConcursoDTO { Id = concursoFavorito, IdConcurso = 3, GrandeAreas = new List<int> { 39 } },
        //         new ProvaRecursoConcursoDTO { Id = 4, IdConcurso = 4 }
        //     });
        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso
        //     {
        //         new ProvaRecurso { ConcursoId = concursoFavorito }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>()).Returns((IDictionary<int, ProvaSubespecialidade>)null);

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(matricula));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.First(c => c.Id == concursoFavorito).Favorito);
        // }
        
        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaGrupoConcurso_GrupoPorStatus()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var concursosAnalise = new[] { 1, 2, 3 };
        //     var concursoProximo = 4;
        //     var concursoExpirado = 5;
        //     var grandeAreas = new List<int> { 39 };

        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso());
        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {

        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, IdStatus = 1, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, IdStatus = 2, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = 3, IdConcurso = 3, IdStatus = 13, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = concursoProximo, IdConcurso = 4, IdStatus = 6, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = concursoExpirado, IdConcurso = 5, IdStatus = 9, GrandeAreas = grandeAreas }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>()).Returns((IDictionary<int, ProvaSubespecialidade>)null);

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(matricula));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.Where(c => concursosAnalise.Contains(c.Id)).All(c => c.Grupo == ProvaRecurso.GrupoConcurso.Analise));
        //     Assert.IsTrue(result.First(c => c.Id == concursoProximo).Grupo == ProvaRecurso.GrupoConcurso.Proximos);
        //     Assert.IsTrue(result.First(c => c.Id == concursoExpirado).Grupo == ProvaRecurso.GrupoConcurso.Expirados);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaSubespecialidade_SubNaProva()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var concursos = new[] { 1, 2, 3 };
        //     var grandeAreas = new List<int> { 39 };

        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso());
        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {
        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, IdStatus = 1, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, IdStatus = 2, GrandeAreas = grandeAreas },
        //         new ProvaRecursoConcursoDTO { Id = 3, IdConcurso = 3, IdStatus = 13, GrandeAreas = grandeAreas }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>().Returns( 
        //     new Dictionary<int, ProvaSubespecialidade>
        //     {
        //         { 1, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = grandeAreas } }
        //     });

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(matricula));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.First(p => p.IdConcurso == 1).Subespecialidades.Any());
        //     Assert.AreEqual("SubTeste", result.First(p => p.IdConcurso == 1).Subespecialidades.First());
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaFiltroRMais_ProvasFiltradas()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var concursosCirurgia = new[] { 1, 3, 5 };
        //     var idAreaCirurgia = 39;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso());
        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {
        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, IdStatus = 1 },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 3, IdConcurso = 3, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 4, IdConcurso = 4, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 5, IdConcurso = 5, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 6, IdConcurso = 6, IdStatus = 1 }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>()).Returns(
        //     new Dictionary<int, ProvaSubespecialidade>
        //     {
        //         { 1, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { idAreaCirurgia } } },
        //         { 2, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 41 } } },
        //         { 3, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { idAreaCirurgia } } },
        //         { 4, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } },
        //         { 5, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { idAreaCirurgia } } },
        //         { 6, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } }
        //     });

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(matricula));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.All(p => concursosCirurgia.Contains(p.Id)));
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaFiltroRMaisMatriculaErrada_NenhumaProva()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso());
        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {
        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, IdStatus = 1 },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 3, IdConcurso = 3, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 4, IdConcurso = 4, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 5, IdConcurso = 5, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 6, IdConcurso = 6, IdStatus = 1 }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>().Returns(
        //     new Dictionary<int, ProvaSubespecialidade>
        //     {
        //         { 1, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 2, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 41 } } },
        //         { 3, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 4, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } },
        //         { 5, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 6, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } }
        //     });

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(3));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.Count == 0);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void GetProvaConcursos_TestaFiltroRMaisAlunoSemPermissao_NenhumaProva()
        // {
        //     var ano = DateTime.Now.Year;
        //     var matricula = 1;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.GetProvasFavoritas(matricula).Returns(new ProvasRecurso());
        //     mock.GetProvasConcursos(ano, matricula, true).Returns(new List<ProvaRecursoConcursoDTO>
        //     {
        //         new ProvaRecursoConcursoDTO { Id = 1, IdConcurso = 1, IdStatus = 1 },
        //         new ProvaRecursoConcursoDTO { Id = 2, IdConcurso = 2, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 3, IdConcurso = 3, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 4, IdConcurso = 4, IdStatus = 2 },
        //         new ProvaRecursoConcursoDTO { Id = 5, IdConcurso = 5, IdStatus = 13 },
        //         new ProvaRecursoConcursoDTO { Id = 6, IdConcurso = 6, IdStatus = 1 }
        //     });
        //     mock.GetSubespecialidadesProvas(Arg.Any<int>(), Arg.Any<bool>().Returns(
        //     new Dictionary<int, ProvaSubespecialidade>
        //     {
        //         { 1, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 2, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 41 } } },
        //         { 3, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 4, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } },
        //         { 5, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 39 } } },
        //         { 6, new ProvaSubespecialidade { Subespecialidades = new List<string> { "SubTeste" }, GrandeAreasProva = new List<int> { 161 } } }
        //     });

        //     var result = business.GetProvaConcursos(ano, true, matricula, ExercicioEntityTestData.GetHashPermissaoRMais(matricula, false));

        //     Assert.IsNotNull(result);
        //     Assert.IsTrue(result.Count == 0);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void TrocarStatusProvaFavorita_TestaJaFavoritoSolicitandoDeNovo_NaoAlterar()
        // {
        //     var idProva = 2;
        //     var matricula = 3;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.AlunoFavoritouProva(idProva, matricula).Returns(true);

        //     var result = business.TrocarStatusProvaFavorita(idProva, matricula, true);

        //     mock.AssertWasNotCalled(e => e.SetStatusProvaFavorita(idProva, matricula));

        //     mock.VerifyAllExpectations();
        //     Assert.AreEqual(0, result);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void TrocarStatusProvaFavorita_TestaJaFavoritoSolicitandoDesfavoritar_AlterarFavorito()
        // {
        //     var idProva = 2;
        //     var matricula = 3;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.AlunoFavoritouProva(idProva, matricula).Returns(true);
        //     mock.SetStatusProvaFavorita(idProva, matricula).Returns(1);

        //     var result = business.TrocarStatusProvaFavorita(idProva, matricula, false);

        //     mock.AssertWasCalled(e => e.SetStatusProvaFavorita(idProva, matricula));

        //     mock.VerifyAllExpectations();
        //     Assert.AreEqual(1, result);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void TrocarStatusProvaFavorita_TestaNaoFavoritoSolicitandoDeNovoDesfavoritar_NaoAlterar()
        // {
        //     var idProva = 2;
        //     var matricula = 3;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.AlunoFavoritouProva(idProva, matricula).Returns(false);

        //     var result = business.TrocarStatusProvaFavorita(idProva, matricula, false);

        //     mock.AssertWasNotCalled(e => e.SetStatusProvaFavorita(idProva, matricula));

        //     mock.VerifyAllExpectations();
        //     Assert.AreEqual(0, result);
        // }

        // [TestMethod]
        // [TestCategory("Forum prova")]
        // public void TrocarStatusProvaFavorita_TestaNaoFavoritoSolicitando_Favoritar()
        // {
        //     var idProva = 2;
        //     var matricula = 3;
        //     var mock = Substitute.For<IExercicioData>();
        //     var business = new ExercicioBusiness(mock, null, null, null, null, null);

        //     mock.AlunoFavoritouProva(idProva, matricula).Returns(false);
        //     mock.SetStatusProvaFavorita(idProva, matricula).Returns(1);

        //     var result = business.TrocarStatusProvaFavorita(idProva, matricula, true);

        //     mock.Received().SetStatusProvaFavorita(idProva, matricula);

        //     mock.VerifyAllExpectations();
        //     Assert.AreEqual(1, result);
        // }
    }
}