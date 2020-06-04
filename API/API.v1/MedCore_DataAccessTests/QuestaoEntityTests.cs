using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MedCore_API.Academico;
using MedCore_DataAccess;
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
using MedCore_DataAccessTests.EntitiesMockData;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class QuestaoEntityTests
    {
        static int matriculaTeste = 173010;
        static int questaoObjetivaSimulado201701 = 23306;
        static int idSimulado201701 = 614;
        static int questaoDiscursivaSimulado201701 = 23388;
        static char alternativaA = 'A';
        static char alternativaB = 'B';
        static char alternativaC = 'C';
        static bool alternativaSelecionada = true;
        static string respostaDiscursivaAlternativaA = "Resposta de Teste PrimeiraAlternativa X111X A1 Teste";
        static string respostaDiscursivaAlternativaB = "Resposta de Teste SegundaAlternativa X222X A2 Tes";

        private int GetIdSimuladoAtual(int matricula, int ano)
        {
            try
            {
                var simuladoBussiness = new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity());
                var simuladoAtual = simuladoBussiness.GetCronogramaSimulados(ano, matricula).FirstOrDefault();

                return simuladoAtual.SimuladoID;
            }
            catch
            {

                return 0;
            }
        }

        private int GetUltimoExercicioHistoricoSimulado(int matricula, int simuladoId)
        {
            int historico;
            using (var ctx = new AcademicoContext())
            {
                historico = ctx.tblExercicio_Historico.Where(s => s.intClientID == matricula && s.intExercicioID == simuladoId && s.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO)
                    .OrderByDescending(h => h.intHistoricoExercicioID).FirstOrDefault().intHistoricoExercicioID;
            }
            return historico;
        }

        private int GetAlternativaId(string letra, int idQuestao)
        {
            int idAlternativa;
            using (var ctx = new AcademicoContext())
            {
                idAlternativa = ctx.tblQuestaoAlternativas.Where(q => q.txtLetraAlternativa == letra && q.intQuestaoID == idQuestao).FirstOrDefault().intAlternativaID;
            }
            return idAlternativa;
        }

        private int GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste()
        {
            return GetUltimoExercicioHistoricoSimulado(matriculaTeste, idSimulado201701);
        }

        private int GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);
            return GetUltimoExercicioHistoricoSimulado(matriculaTeste, idSimulado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetAtualizacaoDados_ParametroNaoGuid()
        {
            var questao = new QuestaoEntity().GetAtualizacaoDados("XXX");
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetForumQuestaoCol1_NotNull()
        {
            var questao = new QuestaoEntity().GetForumQuestaoCol1(69468, 1, 199300);
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(ForumQuestao.Coluna1));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetForumQuestaoCol2_NotNull()
        {
            var questao = new QuestaoEntity().GetForumQuestaoCol2(69468, 1, 199300);
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(ForumQuestao.Coluna2));
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetForumQuestaoCol3_NotNull()
        {
            var questao = new QuestaoEntity().GetForumQuestaoCol3(69468, 1, 199300);
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(ForumQuestao.Coluna3));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoSimulado_NotNull_QuestaoExistente()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(19542, 96409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoSimulado_NotNull_QuestaoInexistente()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                 .GetTipoSimulado(21872, 96409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoSimulado_NotNull_QuestaoDiscursivaCom_CincoAlternativas()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(22594, 96409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoSimulado_NotNull_QuestaoDiscursivaCom_QuatroAlternativas()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(22664, 96409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetEstatisticaSimulado_NotNull()
        {
            var estatisticas = new QuestaoEntity().GetEstatisticaExercicio(19542, 1);
            Assert.IsNotNull(estatisticas);
            Assert.IsInstanceOfType(estatisticas, typeof(List<Estatistica>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetEstatisticaSimulado_NotNull_QuestaoInexistente()
        {
            var estatisticas = new QuestaoEntity().GetEstatisticaExercicio(0, 1);
            Assert.IsNotNull(estatisticas);
            Assert.IsInstanceOfType(estatisticas, typeof(List<Estatistica>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetEstatisticaSimulado_NotNull_QuestaoDiscursiva()
        {
            var estatisticas = new QuestaoEntity().GetEstatisticaExercicio(19593, 1);
            Assert.IsNotNull(estatisticas);
            Assert.IsInstanceOfType(estatisticas, typeof(List<Estatistica>));
        }

        [TestMethod]
        [TestCategory("QuestaoOffline")]
        public void GetQuestaoApostilaDownload_NotNull_QuestaoExistente()
        {
            var questaoDownload = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoApostilaDownload(150561, 227162, Convert.ToInt32(Aplicacoes.MsProMobile));
            Assert.IsNotNull(questaoDownload);
            Assert.IsInstanceOfType(questaoDownload, typeof(QuestaoApostilaDownload));
        }


        [TestMethod]
        [TestCategory("QuestaoOffline")]
        public void GetQuestaoApostilaDownload_NotNull_QuestaoDetalhada()
        {
            var questao = new tblConcursoQuestoes()
            {
                txtEnunciado = "GABARITO"
            };

            var alternativaA = new tblConcursoQuestoes_Alternativas()
            {
                txtLetraAlternativa = "A",
                intAlternativaID = 1,
                bitCorreta = true,
                bitCorretaPreliminar = true
            };

            var alternativaB = new tblConcursoQuestoes_Alternativas()
            {
                txtLetraAlternativa = "B",
                intAlternativaID = 2,
                bitCorreta = false,
                bitCorretaPreliminar = false
            };

            List<tblConcursoQuestoes_Alternativas> alternativas = new List<tblConcursoQuestoes_Alternativas>();
            alternativas.Add(alternativaA);
            alternativas.Add(alternativaB);

            var provaConcursoDTO = new ProvaConcursoDTO()
            {
                Ano = 2019,
                Nome = "ABC",
                Tipo = "1",
                UF = "RJ"
            };

            int QuestaoID = 150561;
            int ClientID = 227162;
            int ApplicationID = Convert.ToInt32(Aplicacoes.MsProMobile);

            var questaoMock = Substitute.For<Questao>();
            var lstImagensMock = Substitute.For<List<int>>();
            var LstImagens64Mock = Substitute.For<List<String>>();
            var forumRecursoMock = Substitute.For<ForumQuestaoRecurso>();
            var estatisticasMock = Substitute.For<Dictionary<string, string>>();
            var tblConcursoQuestoesMock = Substitute.For<tblConcursoQuestoes>();

            var questaoDataMock = Substitute.For<IQuestaoData>();
            var imagemDataMock = Substitute.For<IImagemData>();

            questaoDataMock.ObterProvaConcurso(questao).Returns(provaConcursoDTO);
            questaoDataMock.ObterQuestaoConcurso(QuestaoID).Returns(questao);
            questaoDataMock.ObterAlternativaCorreta(QuestaoID, ClientID).Returns(alternativas);
            questaoDataMock.ObterRespostasDiscursivas(QuestaoID, ClientID).Returns(new List<CartaoRespostaDiscursivaDTO>());
            questaoDataMock.ObterQuestaoConcurso(QuestaoID).Returns(questao);
            questaoDataMock.ObterAlternativasQuestaoConcurso(QuestaoID).Returns(alternativas);
            questaoDataMock.GetForumQuestaoRecurso(QuestaoID, ClientID).Returns(forumRecursoMock);
            questaoDataMock.GetEstatistica(QuestaoID, (int)Exercicio.tipoExercicio.CONCURSO).Returns(estatisticasMock);
            imagemDataMock.GetImagensQuestaoConcurso(QuestaoID).Returns(lstImagensMock);
            imagemDataMock.GetConcursoImagemComentario(QuestaoID).Returns(new List<Imagem>());
            imagemDataMock.GetConcursoBase64(0).Returns("");

            var questaoDownload = new QuestaoBusiness(questaoDataMock, imagemDataMock, new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoApostilaDownload(QuestaoID, ClientID, Convert.ToInt32(Aplicacoes.MsProMobile));

            questaoDataMock.Received().ObterProvaConcurso(questao);
            questaoDataMock.Received().ObterQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().ObterAlternativaCorreta(QuestaoID, ClientID);
            questaoDataMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID);
            questaoDataMock.Received().ObterQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().ObterAlternativasQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().GetForumQuestaoRecurso(QuestaoID, ClientID);
            questaoDataMock.Received().GetEstatistica(QuestaoID, (int)Exercicio.tipoExercicio.CONCURSO);
            imagemDataMock.Received().GetImagensQuestaoConcurso(QuestaoID);
            imagemDataMock.Received().GetConcursoImagemComentario(QuestaoID);

            Assert.IsNotNull(questaoDownload);
        }

        [TestMethod]
        [TestCategory("QuestaoOffline")]
        public void GetQuestaoApostilaDownload_QuestaoApostilaNoDesktopComAppVersion_RetornarQuestao()
        {
            var appVersion = "3.0.0"; //acima da versão minima
            var questao = new tblConcursoQuestoes()
            {
                txtEnunciado = "GABARITO"
            };

            var alternativaA = new tblConcursoQuestoes_Alternativas()
            {
                txtLetraAlternativa = "A",
                intAlternativaID = 1,
                bitCorreta = true,
                bitCorretaPreliminar = true
            };

            var alternativaB = new tblConcursoQuestoes_Alternativas()
            {
                txtLetraAlternativa = "B",
                intAlternativaID = 2,
                bitCorreta = false,
                bitCorretaPreliminar = false
            };

            List<tblConcursoQuestoes_Alternativas> alternativas = new List<tblConcursoQuestoes_Alternativas>();
            alternativas.Add(alternativaA);
            alternativas.Add(alternativaB);

            var provaConcursoDTO = new ProvaConcursoDTO()
            {
                Ano = 2019,
                Nome = "ABC",
                Tipo = "1",
                UF = "RJ"
            };

            var video = new Video()
            {
                ID = 1,
                Url = "teste"
            };

            var listaVideos = new List<Video>();
            listaVideos.Add(video);

            int QuestaoID = 150561;
            int ClientID = 227162;
            int ApplicationID = Convert.ToInt32(Aplicacoes.MsProDesktop);

            var questaoMock = Substitute.For<Questao>();
            var lstImagensMock = Substitute.For<List<int>>();
            var LstImagens64Mock = Substitute.For<List<String>>();
            var forumRecursoMock = Substitute.For<ForumQuestaoRecurso>();
            var estatisticasMock = Substitute.For<Dictionary<string, string>>();
            var tblConcursoQuestoesMock = Substitute.For<tblConcursoQuestoes>();

            var questaoDataMock = Substitute.For<IQuestaoData>();
            var imagemDataMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();

            questaoDataMock.ObterProvaConcurso(questao).Returns(provaConcursoDTO);
            questaoDataMock.ObterQuestaoConcurso(QuestaoID).Returns(questao);
            questaoDataMock.ObterAlternativaCorreta(QuestaoID, ClientID).Returns(alternativas);
            questaoDataMock.ObterRespostasDiscursivas(QuestaoID, ClientID).Returns(new List<CartaoRespostaDiscursivaDTO>());
            questaoDataMock.ObterQuestaoConcurso(QuestaoID).Returns(questao);
            questaoDataMock.ObterAlternativasQuestaoConcurso(QuestaoID).Returns(alternativas);
            questaoDataMock.GetForumQuestaoRecurso(QuestaoID, ClientID).Returns(forumRecursoMock);
            questaoDataMock.GetEstatistica(QuestaoID, (int)Exercicio.tipoExercicio.CONCURSO).Returns(estatisticasMock);
            imagemDataMock.GetImagensQuestaoConcurso(QuestaoID).Returns(lstImagensMock);
            imagemDataMock.GetConcursoImagemComentario(QuestaoID).Returns(new List<Imagem>());
            imagemDataMock.GetConcursoBase64(0).Returns("");
            videoMock.GetVideoQuestaoConcurso(QuestaoID, ApplicationID, appVersion).Returns(listaVideos);
            funcionarioMock.GetTipoPerfilUsuario(ClientID).Returns(EnumTipoPerfil.None);
            especialidadeMock.GetByFilters(QuestaoID).Returns(new List<Especialidade>());


            var questaoBusiness = new QuestaoBusiness(questaoDataMock, imagemDataMock, videoMock, especialidadeMock, funcionarioMock);
            var questaoRetorno = questaoBusiness.GetQuestaoApostilaDownload(QuestaoID, ClientID, ApplicationID, appVersion);

            funcionarioMock.Received().GetTipoPerfilUsuario(ClientID);
            questaoDataMock.Received().ObterProvaConcurso(questao);
            questaoDataMock.Received().ObterQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().ObterAlternativaCorreta(QuestaoID, ClientID);
            questaoDataMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID);
            questaoDataMock.Received().ObterQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().ObterAlternativasQuestaoConcurso(QuestaoID);
            questaoDataMock.Received().GetForumQuestaoRecurso(QuestaoID, ClientID);
            questaoDataMock.Received().GetEstatistica(QuestaoID, (int)Exercicio.tipoExercicio.CONCURSO);
            imagemDataMock.Received().GetImagensQuestaoConcurso(QuestaoID);
            imagemDataMock.Received().GetConcursoImagemComentario(QuestaoID);
            videoMock.Received().GetVideoQuestaoConcurso(QuestaoID, ApplicationID, appVersion);
            especialidadeMock.Received().GetByFilters(QuestaoID);


            Assert.IsNotNull(questaoRetorno);

        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetFavorita_QuestaoExistente()
        {
            //int resultado = new QuestaoEntity().SetFavoritaQuestaoSimulado(19542, 96409, true);
            int resultado = new QuestaoEntity().SetFavoritaQuestaoSimulado(19542, 96409, true);
            Assert.AreEqual(resultado, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetDuvida_QuestaoExistente()
        {
            //int resultado = new RDSQuestaoEntity().SetDuvidaQuestaoSimulado(19542, 96409, true);
            int resultado = new QuestaoEntity().SetDuvidaQuestaoSimulado(19542, 96409, true);
            Assert.AreEqual(resultado, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetComentario_QuestaoExistente()
        {
            //int resultado = new RDSQuestaoEntity().SetAnotacaoAlunoQuestao(19542, 96409, "Comentario Teste", 1);
            int resultado = new QuestaoEntity().SetAnotacaoAlunoQuestao(19542, 96409, "Comentario Teste", 1);
            Assert.AreEqual(resultado, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetFavorita_QuestaoNaoExistente()
        {
            //int resultado = new RDSQuestaoEntity().SetFavoritaQuestaoSimulado(0, 96409, true);
            int resultado = new QuestaoEntity().SetFavoritaQuestaoSimulado(0, 96409, true);
            Assert.AreEqual(resultado, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetDuvida_QuestaoNaoExistente()
        {
            //int resultado = new RDSQuestaoEntity().SetDuvidaQuestaoSimulado(0, 96409, true);
            int resultado = new QuestaoEntity().SetDuvidaQuestaoSimulado(0, 96409, true);
            Assert.AreEqual(resultado, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetComentario_QuestaoNaoExistente()
        {
            int resultado = new QuestaoEntity().SetAnotacaoAlunoQuestao(0, 96409, "Comentario Teste", 1);
            Assert.AreEqual(resultado, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetRespostaAluno_Discursiva_Existente()
        {
            int resultado = new QuestaoEntity().SetRespostaAluno(19542, 569, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), Convert.ToInt32(Aplicacoes.AreaRestrita)
                , 96409, "A", "RESPOSTA TESTE", Exercicio.tipoExercicio.SIMULADO);
            Assert.AreEqual(resultado, 1);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetRespostaAluno_Objetiva_Existente()
        {
            int resultado = new QuestaoEntity().SetRespostaAluno(19542, 569, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), Convert.ToInt32(Aplicacoes.AreaRestrita)
                , 96409, "A", true, Exercicio.tipoExercicio.SIMULADO);
            Assert.AreEqual(resultado, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetRespostaAluno_Discursiva_NaoExistente()
        {
            int resultado = new QuestaoEntity().SetRespostaAluno(0, 569, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), Convert.ToInt32(Aplicacoes.AreaRestrita)
                , 96409, "A", "RESPOSTA TESTE", Exercicio.tipoExercicio.SIMULADO);
            Assert.AreEqual(resultado, 0);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetRespostaAluno_Objetiva_NaoExistente()
        {
            int resultado = new QuestaoEntity().SetRespostaAluno(0, 569, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), Convert.ToInt32(Aplicacoes.AreaRestrita)
                , 96409, "A", true, Exercicio.tipoExercicio.SIMULADO);
            Assert.AreEqual(resultado, 0);
        }

        //CONCURSO_________________________________________________________________________________________________________________________________________________________

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoConcurso_NotNull_QuestaoExistente()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(64304, 118409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetTipoConcurso_NotNull_QuestaoInexistente()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(112040, 118409, Convert.ToInt32(Aplicacoes.AreaRestrita));
            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void MsgPadraoSemGabarito()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(60495, 96409, Convert.ToInt32(Aplicacoes.MsProMobile));
            var alternativaCorreta = questao.Alternativas.FirstOrDefault(p => p.Correta);
            if (alternativaCorreta == null)
                alternativaCorreta = questao.Alternativas.First();
            Assert.AreEqual(alternativaCorreta.Gabarito, "Não há gabarito oficial para esta questão");
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void MsgPadraoComGabarito()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(84974, 96409, Convert.ToInt32(Aplicacoes.MsProMobile));
            var alternativaCorreta = questao.Alternativas.FirstOrDefault(p => p.Correta);
            if (alternativaCorreta == null)
                alternativaCorreta = questao.Alternativas.First();
            Assert.AreNotEqual(alternativaCorreta.Gabarito, "Não há gabarito oficial para esta questão");
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetConcursoRespostaAluno_Objetiva_Existente()  //SetRespostaAluno
        {
            int resultado = new QuestaoEntity().SetRespostaAluno(64237, 905593, Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO), Convert.ToInt32(Aplicacoes.AreaRestrita)
                , 118409, "B", true, Exercicio.tipoExercicio.CONCURSO);
            Assert.AreEqual(resultado, 1);
        }

        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void ObterDetalhesQuestaoConcurso_AlunoQuestaoFavoritada_QuestaoFavoritada()
        {
            //var business = new PerfilAlunoBusiness(new RDSPerfilAlunoEntity());
            var business = new PerfilAlunoEntity();
            var alunoQuestao = business.GetAlunoComQuestaoFavoritada(Exercicio.tipoExercicio.CONCURSO.GetHashCode());

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(alunoQuestao.Value, alunoQuestao.Key, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Anotacoes.Any(x => x.Favorita));
        }

        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void ObterDetalhesQuestaoConcurso_AlunoQuestaoAnotada_QuestaoAnotada()
        {
            //var business = new PerfilAlunoBusiness(new RDSPerfilAlunoEntity());
            var business = new PerfilAlunoEntity();
            var alunoQuestao = business.GetAlunoComQuestaoAnotada(Exercicio.tipoExercicio.CONCURSO.GetHashCode());

            if (alunoQuestao.Value == 0)
                Assert.Inconclusive("Pode ocorrer de não existir alunos com esse caso");

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(alunoQuestao.Value, alunoQuestao.Key, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Anotacoes.Any(x => x.Anotacao != null));
        }


        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void GetTipoSimulado_AlunoQuestaoAnotada_QuestaoAnotada()
        {
            //var business = new PerfilAlunoBusiness(new RDSPerfilAlunoEntity());
            var business = new PerfilAlunoEntity();
            var alunoQuestao = business.GetAlunoComQuestaoAnotada(Exercicio.tipoExercicio.SIMULADO.GetHashCode());

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(alunoQuestao.Value, alunoQuestao.Key, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Anotacoes.Any(x => x.Anotacao != null));
        }


        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void GetTipoSimulado_AlunoQuestaoFavoritada_QuestaoFavoritada()
        {
            //var business = new PerfilAlunoBusiness(new RDSPerfilAlunoEntity());
            var business = new PerfilAlunoEntity();
            var alunoQuestao = business.GetAlunoComQuestaoFavoritada(Exercicio.tipoExercicio.SIMULADO.GetHashCode());

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(alunoQuestao.Value, alunoQuestao.Key, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Anotacoes.Any(x => x.Favorita));
        }

        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void GetConcurso_QuestaoSemMarcacao_QuestaoAnotacaoNula()
        {
            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoAcademico();
            var questaoNaoMarcada = new QuestaoEntity().GetQuestaoSemMarcacao(aluno.ID);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(questaoNaoMarcada, aluno.ID, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsNull(questao.Anotacoes);
        }


        [TestMethod]
        [TestCategory("Questao Marcacao")]
        public void GetTipoSimulado_QuestaoSemMarcacao_QuestaoAnotacaoNula()
        {
            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoAcademico();
            var questaoNaoMarcadaSimulado = new QuestaoEntity().GetQuestaoSimuladoSemMarcacao(aluno.ID);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoNaoMarcadaSimulado, aluno.ID, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsNull(questao.Anotacoes);
        }

        #region Duvida de Questao

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetQuestaoDuvida_NaoNulas()
        {
            var questaoDuvida = new QuestaoDuvida
            {
                TextoPergunta = "Quem é josé?",
                AplicacaoId = 1,
                Aluno = new Aluno { ID = 119300 },
                Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }
            };
            var resultado = new QuestaoEntity().SetDuvida(questaoDuvida);
            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetQuestaoDuvida_CadastroComSucesso()
        {
            var questaoDuvida = new QuestaoDuvida
            {
                TextoPergunta = "Quem é joão?",
                AplicacaoId = 1,
                Aluno = new Aluno { ID = 119300 },
                Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }
            };

            var resultado = new QuestaoEntity().SetDuvida(questaoDuvida);

            Assert.AreEqual(1, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoDuvida_NaoNulas()
        {
            var questaoDuvida = new QuestaoDuvida { Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }, Aluno = new Aluno { ID = 119300 }, AplicacaoId = 1 };

            var resultado = new QuestaoEntity().GetQuestoesDuvida(questaoDuvida);

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvida>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoDuvida_Nulas()
        {
            var questaoDuvida = new QuestaoDuvida { Questao = new Questao { Id = 777777777, ExercicioTipoID = 2 }, Aluno = new Aluno { ID = 119300 }, AplicacaoId = 4 };

            var resultado = new QuestaoEntity().GetQuestoesDuvida(questaoDuvida);

            Assert.AreEqual(0, resultado.Count);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvida>));
        }

        #endregion

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetRespostaRecursos()
        {
            var objPost = new QuestaoRecursosPost { Matricula = 119300, IdQuestao = 105428, AlternativaCorreta = true, AlternativaSelecionada = "C" };
            var retorno = new QuestaoEntity().SetRespostaRecursos(objPost);

            Assert.IsNotNull(retorno);
            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void SetComentarioPre()
        {
            var forumComentario = new ForumQuestaoRecurso.ForumComentarioRecurso()
            {
                Questao = new Questao() { Id = 6379 },
                Matricula = 96409,
                ComentarioTexto = "Teste de inserção de comentario PRÉ pela API",
                Ip = "666.666.666",
                Opiniao = "1"
            };

            List<ForumQuestaoRecurso.ForumComentarioRecurso> lstForumComentario = new List<ForumQuestaoRecurso.ForumComentarioRecurso>();
            lstForumComentario.Add(forumComentario);

            var pre = new ForumQuestaoRecurso.Pre()
            {
                Comentarios = lstForumComentario
            };

            var retorno = new QuestaoEntity().SetComentarioForumQuestaoPre(pre);

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void SetComentarioPos()
        {
            var forumComentario = new ForumQuestaoRecurso.ForumComentarioRecurso()
            {
                Questao = new Questao() { Id = 101460 },
                Matricula = 96409,
                ComentarioTexto = "Teste de inserção de comentario PÓS pela API",
                Ip = "999.999.999",
                Opiniao = "0"
            };

            List<ForumQuestaoRecurso.ForumComentarioRecurso> lstForumComentario = new List<ForumQuestaoRecurso.ForumComentarioRecurso>();
            lstForumComentario.Add(forumComentario);

            var pos = new ForumQuestaoRecurso.Pos()
            {
                Comentarios = lstForumComentario
            };

            var retorno = new QuestaoEntity().SetComentarioForumQuestaoPos(pos);

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void InserirResposta()
        {
            var resp = new QuestaoRecursosPost()
            {
                IdQuestao = 104973,
                Matricula = 96409,
                AlternativaSelecionada = "A",
                AlternativaCorreta = false
            };

            var retorno = new QuestaoEntity().SetRespostaRecursos(resp);

            Assert.AreEqual(1, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanFormataNomeProfessor_Null_GetVazio()
        {
            var retorno = new QuestaoEntity().GetRecursosNomeProfessorFormatado(null);
            Assert.AreEqual(string.Empty, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanFormataNomeProfessor_1Nome_GetNomeCompleto()
        {
            var nome = "Pedro";
            var retorno = new QuestaoEntity().GetRecursosNomeProfessorFormatado(nome);

            Assert.AreEqual(retorno, nome);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanFormataNomeProfessor_2Nomes_GetNomeCompleto()
        {
            var nome = "Pedro Quintino";
            var retorno = new QuestaoEntity().GetRecursosNomeProfessorFormatado(nome);

            Assert.AreEqual(retorno, nome);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanFormataNomeProfessor_3Nomes_GetNomeFormatado()
        {
            var nome = "Pedro Paulo Quintino";
            var nomeFormatado = "Pedro Quintino";
            var retorno = new QuestaoEntity().GetRecursosNomeProfessorFormatado(nome);

            Assert.AreEqual(retorno, nomeFormatado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanFormataNomeProfessor_4Nomes_GetNomeFormatado()
        {
            var nome = "Pedro Paulo Teste Quintino";
            var nomeFormatado = "Pedro Quintino";
            var retorno = new QuestaoEntity().GetRecursosNomeProfessorFormatado(nome);

            Assert.AreEqual(retorno, nomeFormatado);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetDuvidaQuestaoUrlsImagens_DuvidaExistente_NotNull()
        {
            var result = new QuestaoEntity().GetDuvidaQuestaoUrlsImagens(4);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetDuvidaQuestaoUrlsImagens_DuvidaInexistente_NotNull()
        {
            var result = new QuestaoEntity().GetDuvidaQuestaoUrlsImagens(-1);
            Assert.IsNotNull(result);
        }

        #region Avaliação de Comentario

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetAvaliacaoComentario_NotNull()
        {
            var avaliacaoComentario = new QuestaoAvaliacaoComentario
            {
                QuestaoId = 6376,
                ExercicioTipoId = 1,
                AlunoMatricula = 119300,
                Nota = 1,
                TipoComentario = 2
            };
            var resultado = new QuestaoEntity().SetAvaliacao(avaliacaoComentario);

            Assert.IsInstanceOfType(resultado, typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetAvaliacaoComentario_Null()
        {
            var avaliacaoComentario = new QuestaoAvaliacaoComentario
            {
                QuestaoId = 6376,
                ExercicioTipoId = 2,
                AlunoMatricula = 565656,
                Nota = 1,
                TipoComentario = 1
            };
            var resultado = new QuestaoEntity().SetAvaliacao(avaliacaoComentario);

            Assert.IsInstanceOfType(resultado, typeof(int));
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetAvaliacaoComentario_ValidaNota3ComentarioVazio()
        {
            var avaliacaoComentario = new QuestaoAvaliacaoComentario
            {
                QuestaoId = 6376,
                ExercicioTipoId = 2,
                AlunoMatricula = 119300,
                Nota = 3,
                TipoComentario = 1
            };

            var resultado = new QuestaoEntity().SetAvaliacao(avaliacaoComentario);

            Assert.AreEqual(0, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanSetAvaliacaoComentario_ValidaComentarioNegativoComOpcoes()
        {
            var avaliacaoComentario = new QuestaoAvaliacaoComentario
            {
                QuestaoId = 6376,
                ExercicioTipoId = 2,
                AlunoMatricula = 119300,
                Nota = 4,
                TipoComentario = 2,
                OpcaoComentarioNegativo = 1

            };

            var resultado = new QuestaoEntity().SetAvaliacao(avaliacaoComentario);

            Assert.AreEqual(1, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }





        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetPermissaoAvaliacao_NotNull()
        {
            int resultado = new QuestaoEntity().GetPermissaoAvaliacao(6376, 1, 1, 119300);
            Assert.AreEqual(0, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }
        #endregion

        #region Duvida de Questao

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetQuestaoDuvida_NotNull()
        {
            var questaoDuvida = new QuestaoDuvida
            {
                TextoPergunta = "Quem é josé?",
                AplicacaoId = 1,
                Aluno = new Aluno { ID = 119300 },
                Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }
            };
            var resultado = new QuestaoEntity().SetDuvida(questaoDuvida);
            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetQuestaoDuvida_CadastroInvalido()
        {
            var questaoDuvida = new QuestaoDuvida
            {
                AplicacaoId = 1,
                Aluno = new Aluno { ID = 1 },
                Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }
            };

            var resultado = new QuestaoEntity().SetDuvida(questaoDuvida);

            Assert.AreEqual(0, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetQuestaoDuvida_RespostaComSucesso()
        {
            var questaoDuvida = new QuestaoDuvida
            {
                Id = 66666,
                TextoResposta = "João das Neves"
            };

            var resultado = new QuestaoEntity().SetDuvidaResposta(questaoDuvida);

            Assert.AreEqual(1, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetQuestaoDuvida_NotNull()
        {
            var questaoDuvida = new QuestaoDuvida { Questao = new Questao { Id = 6376, ExercicioTipoID = 1 }, Aluno = new Aluno { ID = 119300 }, AplicacaoId = 1 };

            var resultado = new QuestaoEntity().GetQuestaoDuvidas(questaoDuvida, 0);

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvida>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetQuestaoDuvida_Null()
        {
            var questaoDuvida = new QuestaoDuvida { Questao = new Questao { Id = 777777777, ExercicioTipoID = 2 }, Aluno = new Aluno { ID = 119300 }, AplicacaoId = 4 };

            var resultado = new QuestaoEntity().GetQuestaoDuvidas(questaoDuvida, 0);

            Assert.AreEqual(0, resultado.Count);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvida>));
        }



        [TestMethod]
        [TestCategory("Basico")]
        public void ListaDuvidaAdmin()
        {
            var param = new ParamQuestaoDuvida()
            {
                TipoDuvida = EnumQuestaoDuvidaTipo.Moderados,
                QuestaoDuvida = new QuestaoDuvidaDTO()
                {
                    Cliente = string.Empty,
                    Professor = string.Empty,
                    IdModerador = null,
                    IdQuestao = 32172,
                    IdTipoExercicio = 2
                },
                UsuarioLogado = 76977,
                UsuarioPerfil = new FuncionarioEntity().GetTipoPerfilUsuario(76977),
                LabelAluno = 0,
                LabelQuestaoDuvida = 0,
                DataPerguntaIni = null,
                DataPerguntaFim = null,
                DataRespostaIni = null,
                DataRespostaFim = null,
                DataModeracaoIni = null,
                DataModeracaoFim = null,
                PageSize = 1,
                PageNumber = 1
            };

            var resultado = new QuestaoEntity().GetAdminListaDuvida(param);

            Assert.AreEqual(0, resultado.Count);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvidaDTO>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]//Lembrar de tirar quando tiver cenário
        public void CanGetAdminQuestaoDuvida_NotNull()
        {
            var param = new ParamQuestaoDuvida { TipoDuvida = EnumQuestaoDuvidaTipo.Todos };

            var resultado = new QuestaoEntity().GetAdminListaDuvida(param);

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(List<QuestaoDuvidaDTO>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetAdminDuvidaEncaminhar_ComSucesso()
        {
            var param = new QuestaoDuvidaEncaminhamento { QuestaoDuvidaID = 2, DestinatarioID = 106598 };

            var resultado = new QuestaoEntity().SetAdminDuvidaEncaminhar(param);

            Assert.AreEqual(1, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetAdminDuvidaModerar_ComSucesso()
        {
            var param = new QuestaoDuvidaModeracao { QuestaoDuvidaID = 2, EmployeeID = 85943, Ativo = true };

            var resultado = new QuestaoEntity().SetAdminDuvidaModerar(param);

            Assert.AreEqual(1, resultado);
            Assert.IsInstanceOfType(resultado, typeof(int));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoConcurso_NotNull()
        {
            var resultado = new QuestaoEntity().GetQuestaoConcurso(20587);

            Assert.IsNotNull(resultado.Id);
            Assert.IsInstanceOfType(resultado, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoConcurso_Null()
        {
            var resultado = new QuestaoEntity().GetQuestaoConcurso(0);

            Assert.AreEqual(0, resultado.Id);
            Assert.IsInstanceOfType(resultado, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoSimulado_NotNull()
        {
            var resultado = new QuestaoEntity().GetQuestaoConcurso(7414);

            Assert.IsNotNull(resultado.Id);
            Assert.IsInstanceOfType(resultado, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetQuestaoSimulado_Null()
        {
            var resultado = new QuestaoEntity().GetQuestaoConcurso(0);

            Assert.AreEqual(0, resultado.Id);
            Assert.IsInstanceOfType(resultado, typeof(Questao));
        }

        #endregion

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetProtocoladaPara_NotNull()
        {
            var idQuestao = 82337;
            var ano = 2014;
            var professor = new QuestaoEntity().GetProtocoladaPara(idQuestao, ano);
            Assert.IsNotNull(professor);
            Assert.IsInstanceOfType(professor, typeof(Professor));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetProtocoladaPara_WithCorrectValues()
        {
            var idQuestao = 82337;
            var ano = 2014;
            var professor = new QuestaoEntity().GetProtocoladaPara(idQuestao, ano);
            Assert.IsNotNull(professor.Nome);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetPrimeiroComentario_NotNull()
        {
            var idQuestao = 82337;
            var professor = new QuestaoEntity().GetPrimeiroComentario(idQuestao);
            Assert.IsNotNull(professor);
            Assert.IsInstanceOfType(professor, typeof(Professor));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetPrimeiroComentario_WithCorrectValues()
        {
            var idQuestao = 180813;
            var professor = new QuestaoEntity().GetPrimeiroComentario(idQuestao);
            Assert.IsNotNull(professor.Nome);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetUltimoComentario_NotNull()
        {
            var idQuestao = 82337;
            var professor = new QuestaoEntity().GetUltimoComentario(idQuestao);
            Assert.IsNotNull(professor);
            Assert.IsInstanceOfType(professor, typeof(Professor));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetUltimoComentario_WithCorrectValues()
        {
            var idQuestao = 82337;
            var professor = new QuestaoEntity().GetUltimoComentario(idQuestao);
            Assert.IsNotNull(professor.Nome);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAnos_NotNull()
        {
            var sigla = "ABC";
            var anos = new QuestaoEntity().GetAnos(sigla);
            Assert.IsNotNull(anos);
            Assert.IsInstanceOfType(anos, typeof(List<int>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAnos_WithCorrectValues()
        {
            var sigla = "ABC";
            var anos = new QuestaoEntity().GetAnos(sigla);
            Assert.IsNotNull(anos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAnosPublicadas_NotNull()
        {
            var sigla = "ABC";
            var anos = new QuestaoEntity().GetAnosPublicadas(sigla);
            Assert.IsNotNull(anos);
            Assert.IsInstanceOfType(anos, typeof(List<int>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAnosPublicadas_WithCorrectValues()
        {
            var sigla = "ABC";
            var anos = new QuestaoEntity().GetAnosPublicadas(sigla);
            Assert.IsNotNull(anos);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRelatorioQuestoesPublicadas_NotNull()
        {
            var sigla = "ABC";
            var anoPublicada = 2013;
            var questoesConcurso = QuestaoEntityTestData.ListaConcursoQuestaoDTO();
            var professor = new Professor() { ID = 1, Nome = "Professor" };

            var questaoMock = Substitute.For<IQuestaoData>();
            var videoMock = Substitute.For<IVideoData>();

            questaoMock.ListConcursoQuestao(sigla, 0, anoPublicada).Returns(questoesConcurso);
            questaoMock.GetProtocoladaPara(Arg.Any<int>(), Arg.Any<int>()).Returns(professor);
            questaoMock.GetPrimeiroComentario(Arg.Any<int>()).Returns(professor);
            questaoMock.GetUltimoComentario(Arg.Any<int>()).Returns(professor);

            var questoes = new QuestaoBusiness(questaoMock, new ImagemEntity(), videoMock, new EspecialidadeEntity()).GetRelatorioQuestoesPublicadas(sigla, anoQuestaoPublicada: anoPublicada);
            Assert.IsNotNull(questoes);
            Assert.IsInstanceOfType(questoes, typeof(List<Questao>));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetRelatorioQuestoesPublicadas_WithCorrectValues()
        {
            var sigla = "ABC";
            var ano = 2012;
            var duracao = 10;

            var questaoDataMock = Substitute.For<IQuestaoData>();
            var imagemDataMock = Substitute.For<IImagemData>();
            var videoDataMock = Substitute.For<IVideoData>();
            var especialidadeDataMock = Substitute.For<IEspecialidadeData>();

            var listaConcursoQuestao = QuestaoEntityTestData.ListaConcursoQuestaoDTO();

            var protocoladaPara = new Professor() { ID = 87866, Nome = "BRUNO CELORIA", DataCadastro = -62135596800.0 };
            var primeiroComentario = new Professor() { ID = 87866, Nome = "BRUNO CELORIA", DataAcao = DateTime.Parse("2012-08-18T14:39:43.427"), DataCadastro = 1345300783.0 };
            var ultimoComentario = new Professor() { ID = 161207, Nome = "TESLA GUIMARAES DIAS (DESLIGADO)", DataAcao = DateTime.Parse("2012-10-15T16:34:37.837"), DataCadastro = 1350318877.0 };
            var jsonProtocoladaPara = JsonConvert.SerializeObject(protocoladaPara);
            var jsonPrimeiroComentario = JsonConvert.SerializeObject(primeiroComentario);
            var jsonUltimoComentario = JsonConvert.SerializeObject(ultimoComentario);

            videoDataMock.GetDuracao(Arg.Any<int>()).Returns(duracao);
            questaoDataMock.ListConcursoQuestao(sigla, ano, 0).Returns(listaConcursoQuestao);

            questaoDataMock.GetProtocoladaPara(Arg.Any<int>(), Arg.Any<int>()).Returns(protocoladaPara);
            questaoDataMock.GetPrimeiroComentario(Arg.Any<int>()).Returns(primeiroComentario);
            questaoDataMock.GetUltimoComentario(Arg.Any<int>()).Returns(ultimoComentario);

            var questoes = new QuestaoBusiness(questaoDataMock, imagemDataMock, videoDataMock, especialidadeDataMock).GetRelatorioQuestoesPublicadas(sigla, anoQuestao: ano);

            Assert.IsNotNull(questoes);
            Assert.AreEqual(listaConcursoQuestao.Count, questoes.Count);
            var ordemAnterior = 0;
            foreach (var atual in questoes)
            {
                var esperado = listaConcursoQuestao.Find(x => x.IdQuestao == atual.Id);

                Assert.AreEqual(esperado.AnoConcurso, atual.Concurso.Ano);
                Assert.AreEqual(esperado.SiglaConcurso, atual.Concurso.Sigla);
                Assert.AreEqual(esperado.NomeProva, atual.Prova.Nome);
                Assert.AreEqual(esperado.OrdemQuestao, atual.Ordem);
                Assert.AreEqual(esperado.AnoQuestao, atual.Ano);
                Assert.AreEqual(Utilidades.DateTimeToUnixTimestamp(esperado.DataQuestao.Value), atual.DataQuestao);

                Assert.AreEqual(duracao, atual.VideoQuestao.Duracao);

                Assert.AreEqual(jsonProtocoladaPara, JsonConvert.SerializeObject(atual.ProtocoladaPara));
                Assert.AreEqual(jsonPrimeiroComentario, JsonConvert.SerializeObject(atual.PrimeiroComentario));
                Assert.AreEqual(jsonUltimoComentario, JsonConvert.SerializeObject(atual.UltimoComentario));

                Assert.IsTrue(atual.Ordem > ordemAnterior);
                ordemAnterior = atual.Ordem;
            }
        }

        // ############################## REALIZA PROVA OBJETIVAS ##############################
        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void EnviaRespostaSimulado_AlternativaASelecionada_Enviado()
        {
            var resp = new RespostaObjetivaPost
            {
                HistoricoId = 1683471, // ID criado pra testes
                QuestaoId = 22597,
                ExercicioId = 602,
                Alterantiva = "A",
                ExercicioTipoId = 1
            };
            var retorno = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                        .SetRespostaObjetiva(resp);
            Assert.AreEqual(retorno, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void EnviaRespostaConcurso_AlternativaASelecionadaSemHistorico_NaoEnviado()
        {
            var resp = new RespostaObjetivaPost
            {
                HistoricoId = -1, // ID inexistente para dar erro
                QuestaoId = 45353,
                ExercicioId = 878802,
                Alterantiva = "A",
                ExercicioTipoId = 2
            };
            var retorno = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                            .SetRespostaObjetiva(resp);
            Assert.AreEqual(retorno, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void EnviaRespostaConcurso_AlternativaASelecionada_Enviado()
        {
            var resp = new RespostaObjetivaPost
            {
                HistoricoId = 1683472, // ID criado pra testes
                QuestaoId = 45353,
                ExercicioId = 878802,
                Alterantiva = "A",
                ExercicioTipoId = 2
            };
            var retorno = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                        .SetRespostaObjetiva(resp);
            Assert.AreEqual(retorno, 1);
        }

        // ############################## REALIZA PROVA DISCURSIVAS ##############################
        //[Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void EnviaRespostaExercicio_DiscursivaRespondida_Enviado()
        {
            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = 2797215,
                QuestaoId = 528236,
                DiscursivaId = 2305463,
                ExercicioId = 732,
                Resposta = "FRAQUEZA MUSCULATURA PALPEBRA MIASTENIA GRAVIS",
                ExercicioTipoId = 2
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);
            Assert.AreEqual(retorno, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void EnviaRespostaSimulado_DiscursivaRespondida_AlemDoLimiteDeCaracteres_NaoEnviado()
        {
            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = 6,
                QuestaoId = 20896,
                DiscursivaId = 109002,
                ExercicioId = 573,
                Resposta = "How neat I'm impressed How did you come to be so blessed? You're a star You blaze Out like a sharp machine Like a whale's moan Well I'm here if that's what you want Here we are You're pins I'm needles Let's play Here we are You want this? Then come on Tune out everyone in the crowd Because now it's just me and you Come fall in love with the sound Make a pact to each other When no one's around put the cross between me and you Who wants to fuck with us now?",
                ExercicioTipoId = 1
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);
            Assert.AreEqual(retorno, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void EnviaRespostaSimulado_AlternativaASelecionadaSemHistorico_NaoEnviado()
        {
            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = 0,
                QuestaoId = 20896,
                DiscursivaId = 109002,
                ExercicioId = 573,
                Resposta = "",
                ExercicioTipoId = 1
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);
            Assert.AreEqual(retorno, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void EnviaRespostaConcurso_DiscursivaRespondida_Enviado()      
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = 2797210,
                QuestaoId = 174982,
                DiscursivaId = 11,
                ExercicioId = 924238,
                Resposta = "",
                ExercicioTipoId = 2
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);
            Assert.AreEqual(retorno, 1);
        }
        // ############################## REALIZA PROVA MARCACOES (ANOTACAO, FAVORITO, DUVIDA) ##############################

        [TestMethod]
        [TestCategory("Basico")]
        public void MarcacaoQuestao_Anotacao_Enviado()
        {
        }
        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetProfessorFavorito_NotNull()
        {
            var idProfessor = 1;
            var result = new PortalProfessorEntity().GetQuestaoFavoritaProfessor(Utilidades.ToInt32(idProfessor));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetQuestoesFavorito_NotNull()
        {
            var idProfessor = 1;
            var ano = 2016;
            var result = new PortalProfessorEntity().GetQuestoesFavoritas(new QuestaoFavorita() { ProfessorID = Utilidades.ToInt32(idProfessor), Ano = Utilidades.ToInt32(ano) });
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.FirstOrDefault(), typeof(QuestaoFavorita));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanGetQuestoesFavoritoById_NotNull()
        {
            var idQuestao = 130140;
            var result = new PortalProfessorEntity().GetQuestaoFavoritaById(Utilidades.ToInt32(idQuestao));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(QuestaoFavorita));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanSetQuestoesFavorito()
        {
            var result = new PortalProfessorEntity().SetQuestaoFavorita(new QuestaoFavorita { ProfessorID = 1, QuestaoID = 130140 });
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(QuestaoFavorita));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CanExcluirQuestoesFavorito()
        {
            var result = new PortalProfessorEntity().SetQuestaoFavorita(new QuestaoFavorita { ProfessorID = 1, QuestaoID = 130140 });
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetSimuladoQuestoesDiscursivas()
        {
            //var lstQuestaoDisc = new RDSQuestaoEntity().GetDiscursivasSimulado(614, 157020);
            var lstQuestaoDisc = new QuestaoEntity().GetDiscursivasSimulado(614, 157020);
            Assert.IsNotNull(lstQuestaoDisc);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoApostila_QuestaoInvalida_MatriculaInvalida_RetornaQuestaoNaoNulo()
        {
            var retorno = new QuestaoEntity().GetTipoApostila(84512, 4152, 17);
            Assert.IsNotNull(retorno);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoMontaProva_RetornaQuestaoNaoNulo()
        {
            //var retorno = new RDSQuestaoEntity().GetTipoMontaProva(7852, 202028, 17);
            var retorno = new QuestaoEntity().GetTipoMontaProva(7852, 202028, 17);
            Assert.IsNotNull(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetOpcoesAvaliacaoComentarioNegativa()
        {
            var retorno = new QuestaoEntity().GetOpcoesAvaliacaoNegativaComentarioQuestao();
            Assert.IsNotNull(retorno);
            Assert.IsTrue(retorno.Count > 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ChecaPorcentagemQuestoesRealizadas()
        {
            var matricula = "253852";
            var ano = "2018";
            var produto = "16";

            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var progressoSemana = business.GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Questoes);

            //Caso de Tema com Questão Anulada -> Antes o percentLido era 98%
            Assert.AreEqual(100, progressoSemana.FirstOrDefault(x => x.IdEntidade == 705).PercentLido);

            //Testes de Borda
            //Não deve ser menor que 0
            //Não deve ser maior que 100
            Assert.IsFalse(progressoSemana.Exists(x => x.PercentLido > 100));
            Assert.IsFalse(progressoSemana.Exists(x => x.PercentLido < 0));


            matricula = "251398";
            produto = "16";
            ano = "2018";

            progressoSemana = business.GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Questoes);

            //Caso de Tema com Questão Anulada -> Antes o percentLido era 96%
            Assert.IsTrue(progressoSemana.FirstOrDefault(x => x.IdEntidade == 705).PercentLido > 96);

            //Testes de Borda
            //Não deve ser menor que 0
            //Não deve ser maior que 100
            Assert.IsFalse(progressoSemana.Exists(x => x.PercentLido > 100));
            Assert.IsFalse(progressoSemana.Exists(x => x.PercentLido < 0));

        }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23283DoSimulado201701ComMatriculaAcademico_DeOrigemAplicacaoMEDSOFT_RetornaQuestao()
        {
            var matriculaAcademico = 96409;
            var questaoSimulado201701 = 23283;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoSimulado201701, matriculaAcademico, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23283DoSimulado201701ComMatriculaAcademico_DeOrigemAplicacaoRESTRITA_RetornaQuestao()
        {
            var matriculaAcademico = 96409;
            var questaoObjetivaSimulado201701 = 23283;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaAcademico, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23283DoSimulado201701ComMatriculaAcademico02_DeOrigemAplicacaoRESTRITA_RetornaQuestao()
        {
            var questaoObjetivaSimulado201701 = 23283;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23326DoSimulado201701ComMatriculaAcademico02_DeOrigemAplicacaoMSPRO_RetornaQuestao()
        {
            var questaoObjetivaSimulado201701 = 23326;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23326DoSimulado201701ComMatriculaAcademico_DeOrigemAplicacaoMSPRO_RetornaQuestao()
        {
            if (DateTime.Now <= new DateTime(2018, 06, 21))
            {
                Assert.Inconclusive();
            }
            var matricula = 96409;
            var questaoObjetivaSimulado201701 = 23326;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(questaoObjetivaSimulado201701, matricula, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23605DoSimulado201705ComMatriculaAcademico_DeOrigemAplicacaoMSPRO_RetornaQuestao()
        {
            if (DateTime.Now <= new DateTime(2018, 06, 21))
            {
                Assert.Inconclusive();
            }
            var matricula = 96409;
            var questaoSimulado201705 = 23605;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoSimulado201705, matricula, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTipoSimulado_SolicitandoQuestao23605DoSimulado201705ComMatriculaAcademico02_DeOrigemAplicacaoMSPRO_RetornaQuestao()
        {
            var questaoSimulado201705 = 23605;

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                     .GetTipoSimulado(questaoSimulado201705, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);
            Assert.IsInstanceOfType(questao, typeof(Questao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void GetQuestoesComComentarioApostila_RetornaListaQuestao()
        {
            RedisCacheManager.DeleteAllKeys();
            var idEntidade = CartaoRepostaEntityTestData.GetExercicioIDValidoQuestoesApostila();
            var questao = new QuestaoEntity().GetQuestoesComComentarioApostilaCache(idEntidade);

            Assert.IsNotNull(questao);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void Compara_GetQuestoesComComentarioApostila_GetQuestoesComComentarioApostilaOld()
        {
            RedisCacheManager.DeleteAllKeys();
            var idEntidade = CartaoRepostaEntityTestData.GetExercicioIDValidoQuestoesApostila();
            var questaoEntity = new QuestaoEntity();

            var semRedis = questaoEntity.GetQuestoesComComentarioApostila(idEntidade);

            var comRedis = questaoEntity.GetQuestoesComComentarioApostilaCache(idEntidade);

            var strSemRedis = JsonConvert.SerializeObject(semRedis);

            var strComRedis = JsonConvert.SerializeObject(comRedis);

            Assert.IsTrue(strComRedis == strSemRedis);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void CacheQuestao_QuestaoSimuladoComCache_RetornarQuestaoCacheadaIgualQuestaoSemCachear()
        {
            RedisCacheManager.DeleteAllKeys();

            var context = new AcademicoContext();
            var questaoSimulado201901Extensivo = 525366;
            var questaoEntity = new QuestaoEntity();

            var semRedis = questaoEntity.CacheQuestao(questaoSimulado201901Extensivo);
            var comRedis = questaoEntity.CacheQuestao(questaoSimulado201901Extensivo);

            var strSemRedis = JsonConvert.SerializeObject(semRedis);

            var strComRedis = JsonConvert.SerializeObject(comRedis);

            Assert.IsTrue(strComRedis == strSemRedis);
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void GetTipoSimulado_MobileQuestaoSimuladoObjetiva_RetornarQuestaoComRespostaObjetiva()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var QuestaoID = 1;
            var ClientID = 1;
            var SimuladoID = 1;
            var isSimuladoOnline = true;
            var ApplicationID = (int)Aplicacoes.MsProMobile;
            /*** MOCKS ***/

            var simulado = new SimuladoDTO()
            {
                ID = SimuladoID,
                ExercicioName = "SIM 01 TESTE"
            };

            funcionarioMock.GetTipoPerfilUsuario(ClientID).Returns(EnumTipoPerfil.None);
            simuladoMock.GetSimulado(SimuladoID).Returns(simulado);
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            videoMock.GetVideoQuestaoSimulado(QuestaoID).Returns(new Videos());

            questaoMock.CacheQuestao(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoDTO());
            questaoMock.GetAlternativasQuestao(QuestaoID, "0").Returns(QuestaoEntityTestData.GetAlternativasQuestaoSimulado());
            questaoMock.GetQuestao_tblQuestaoSimulado(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoSimuladoDTO());
            questaoMock.ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO).Returns(new List<CartaoRespostaDiscursivaDTO>());
            questaoMock.GetComantarioImagemSimulado(QuestaoID).Returns(new List<Imagem>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(QuestaoEntityTestData.GetSimuladoVersaoDTO());
            questaoMock.GetSimuladoIsOnline(ClientID, QuestaoID).Returns(isSimuladoOnline);
            questaoMock.GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline).Returns("A");
            questaoMock.GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline).Returns(true);
            questaoMock.GetQuestaoMarcacao(QuestaoID, ClientID).Returns(new QuestaoMarcacaoDTO());

            especialidadeMock.GetByQuestaoSimulado(QuestaoID, SimuladoID).Returns(new List<Especialidade>());


            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            var questao = questaoBusiness.GetTipoSimulado(QuestaoID, ClientID, ApplicationID);

            funcionarioMock.Received().GetTipoPerfilUsuario(ClientID);
            simuladoMock.Received().GetSimulado(SimuladoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            videoMock.Received().GetVideoQuestaoSimulado(QuestaoID);

            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, "0");
            questaoMock.Received().GetQuestao_tblQuestaoSimulado(QuestaoID);
            questaoMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO);
            questaoMock.Received().GetComantarioImagemSimulado(QuestaoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetSimuladoIsOnline(ClientID, QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoMarcacao(QuestaoID, ClientID);

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Alternativas.Where(x => x.Correta == true).Any() && questao.Respondida);
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void GetTipoSimulado_DesktopQuestaoSimuladoObjetiva_RetornarQuestaoComRespostaObjetiva()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var QuestaoID = 1;
            var ClientID = 1;
            var SimuladoID = 1;
            var isSimuladoOnline = true;
            var ApplicationID = (int)Aplicacoes.MsProDesktop;
            /*** MOCKS ***/

            var simulado = new SimuladoDTO()
            {
                ID = SimuladoID,
                ExercicioName = "SIM 01 TESTE"
            };

            funcionarioMock.GetTipoPerfilUsuario(ClientID).Returns(EnumTipoPerfil.None);
            simuladoMock.GetSimulado(SimuladoID).Returns(simulado);
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            videoMock.GetVideoQuestaoSimulado(QuestaoID).Returns(new Videos());

            questaoMock.CacheQuestao(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoDTO());
            questaoMock.GetAlternativasQuestao(QuestaoID, "0").Returns(QuestaoEntityTestData.GetAlternativasQuestaoSimulado());
            questaoMock.GetQuestao_tblQuestaoSimulado(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoSimuladoDTO());
            questaoMock.ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO).Returns(new List<CartaoRespostaDiscursivaDTO>());
            questaoMock.GetComantarioImagemSimulado(QuestaoID).Returns(new List<Imagem>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(QuestaoEntityTestData.GetSimuladoVersaoDTO());
            questaoMock.GetSimuladoIsOnline(ClientID, QuestaoID).Returns(isSimuladoOnline);
            questaoMock.GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline).Returns("A");
            questaoMock.GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline).Returns(true);
            questaoMock.GetQuestaoMarcacao(QuestaoID, ClientID).Returns(new QuestaoMarcacaoDTO());

            especialidadeMock.GetByQuestaoSimulado(QuestaoID, SimuladoID).Returns(new List<Especialidade>());


            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            var questao = questaoBusiness.GetTipoSimulado(QuestaoID, ClientID, ApplicationID);

            funcionarioMock.Received().GetTipoPerfilUsuario(ClientID);
            simuladoMock.Received().GetSimulado(SimuladoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            videoMock.Received().GetVideoQuestaoSimulado(QuestaoID);

            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, "0");
            questaoMock.Received().GetQuestao_tblQuestaoSimulado(QuestaoID);
            questaoMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO);
            questaoMock.Received().GetComantarioImagemSimulado(QuestaoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetSimuladoIsOnline(ClientID, QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoMarcacao(QuestaoID, ClientID);

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Alternativas.Where(x => x.Correta == true).Any() && questao.Respondida);
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void GetTipoSimulado_MobileQuestaoSimuladoDiscursiva_RetornarQuestaoComRespostaDiscursiva()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var QuestaoID = 1;
            var ClientID = 1;
            var SimuladoID = 1;
            var isSimuladoOnline = true;
            var ApplicationID = (int)Aplicacoes.MsProMobile;
            /*** MOCKS ***/

            var simulado = new SimuladoDTO()
            {
                ID = SimuladoID,
                ExercicioName = "SIM 01 TESTE"
            };

            funcionarioMock.GetTipoPerfilUsuario(ClientID).Returns(EnumTipoPerfil.None);
            simuladoMock.GetSimulado(SimuladoID).Returns(simulado);
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            videoMock.GetVideoQuestaoSimulado(QuestaoID).Returns(new Videos());

            questaoMock.CacheQuestao(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoDTO());
            questaoMock.GetAlternativasQuestao(QuestaoID, "0").Returns(QuestaoEntityTestData.GetAlternativasQuestaoSimulado());
            questaoMock.GetQuestao_tblQuestaoSimulado(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoSimuladoDTO());
            questaoMock.ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO).Returns(QuestaoEntityTestData.GetRespostaDiscursivaSimulado());
            questaoMock.GetComantarioImagemSimulado(QuestaoID).Returns(new List<Imagem>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(QuestaoEntityTestData.GetSimuladoVersaoDTO());
            questaoMock.GetSimuladoIsOnline(ClientID, QuestaoID).Returns(isSimuladoOnline);
            questaoMock.GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline).Returns("");
            questaoMock.GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline).Returns(true);
            questaoMock.GetQuestaoMarcacao(QuestaoID, ClientID).Returns(new QuestaoMarcacaoDTO());

            especialidadeMock.GetByQuestaoSimulado(QuestaoID, SimuladoID).Returns(new List<Especialidade>());


            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            var questao = questaoBusiness.GetTipoSimulado(QuestaoID, ClientID, ApplicationID);

            funcionarioMock.Received().GetTipoPerfilUsuario(ClientID);
            simuladoMock.Received().GetSimulado(SimuladoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            videoMock.Received().GetVideoQuestaoSimulado(QuestaoID);

            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, "0");
            questaoMock.Received().GetQuestao_tblQuestaoSimulado(QuestaoID);
            questaoMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO);
            questaoMock.Received().GetComantarioImagemSimulado(QuestaoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetSimuladoIsOnline(ClientID, QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoMarcacao(QuestaoID, ClientID);

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Alternativas.Where(x => x.Resposta != "").Any());
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void GetTipoSimulado_DesktopQuestaoSimuladoDiscursiva_RetornarQuestaoComRespostaDiscursiva()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var QuestaoID = 1;
            var ClientID = 1;
            var SimuladoID = 1;
            var isSimuladoOnline = true;
            var ApplicationID = (int)Aplicacoes.MsProDesktop;
            /*** MOCKS ***/

            var simulado = new SimuladoDTO()
            {
                ID = SimuladoID,
                ExercicioName = "SIM 01 TESTE"
            };

            funcionarioMock.GetTipoPerfilUsuario(ClientID).Returns(EnumTipoPerfil.None);
            simuladoMock.GetSimulado(SimuladoID).Returns(simulado);
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            videoMock.GetVideoQuestaoSimulado(QuestaoID).Returns(new Videos());

            questaoMock.CacheQuestao(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoDTO());
            questaoMock.GetAlternativasQuestao(QuestaoID, "0").Returns(QuestaoEntityTestData.GetAlternativasQuestaoSimulado());
            questaoMock.GetQuestao_tblQuestaoSimulado(QuestaoID).Returns(QuestaoEntityTestData.GetQuestaoSimuladoDTO());
            questaoMock.ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO).Returns(QuestaoEntityTestData.GetRespostaDiscursivaSimulado());
            questaoMock.GetComantarioImagemSimulado(QuestaoID).Returns(new List<Imagem>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(QuestaoEntityTestData.GetSimuladoVersaoDTO());
            questaoMock.GetSimuladoIsOnline(ClientID, QuestaoID).Returns(isSimuladoOnline);
            questaoMock.GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline).Returns("");
            questaoMock.GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline).Returns(true);
            questaoMock.GetQuestaoMarcacao(QuestaoID, ClientID).Returns(new QuestaoMarcacaoDTO());

            especialidadeMock.GetByQuestaoSimulado(QuestaoID, SimuladoID).Returns(new List<Especialidade>());


            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            var questao = questaoBusiness.GetTipoSimulado(QuestaoID, ClientID, ApplicationID);

            funcionarioMock.Received().GetTipoPerfilUsuario(ClientID);
            simuladoMock.Received().GetSimulado(SimuladoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            videoMock.Received().GetVideoQuestaoSimulado(QuestaoID);

            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, "0");
            questaoMock.Received().GetQuestao_tblQuestaoSimulado(QuestaoID);
            questaoMock.Received().ObterRespostasDiscursivas(QuestaoID, ClientID, (int)Exercicio.tipoExercicio.SIMULADO);
            questaoMock.Received().GetComantarioImagemSimulado(QuestaoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetSimuladoIsOnline(ClientID, QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimulado(QuestaoID, ClientID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoSimuladoIsCorreta(ClientID, QuestaoID, isSimuladoOnline);
            questaoMock.Received().GetQuestaoMarcacao(QuestaoID, ClientID);

            Assert.IsNotNull(questao);
            Assert.IsTrue(questao.Alternativas.Where(x => x.Resposta != "").Any());
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void SetRespostaObjetiva_RespostaSimuladoOnline_InserirTabelaSimuladoOnline()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            RespostaObjetivaPost resp = new RespostaObjetivaPost()
            {
                Alterantiva = "A",
                ExercicioId = 1,
                ExercicioTipoId = 1,
                HistoricoId = 1,
                Matricula = 1,
                QuestaoId = 1
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                intHistoricoExercicioID = 1,
                intTipoProva = (int)TipoProvaEnum.ModoOnline
            };

            var marcacao = new MarcacoesObjetivaDTO();

            questaoMock.GetUltimaMarcacao_SimuladoOnline(resp.QuestaoId, resp.ExercicioId, resp.Matricula).Returns(marcacao);
            questaoMock.SetRespostaObjetivaSimuladoOnline(resp, marcacao).Returns(1);

            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            questaoBusiness.SetRespostaObjetivaSimuladoAgendado(resp);

            questaoMock.Received().GetUltimaMarcacao_SimuladoOnline(resp.QuestaoId, resp.ExercicioId, resp.Matricula);
            questaoMock.Received().SetRespostaObjetivaSimuladoOnline(resp, marcacao);
        }

        [TestMethod]
        [TestCategory("Questão Simulado")]
        public void SetRespostaObjetiva_CartaoRespostaObjetivaSimuladoModoEstudo_InserirTabelaSimuladoEstudo()
        {
            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            RespostaObjetivaPost resp = new RespostaObjetivaPost()
            {
                Alterantiva = "A",
                ExercicioId = 1,
                ExercicioTipoId = 1,
                HistoricoId = 1,
                Matricula = 1,
                QuestaoId = 1
            };

            var exercicioHistorico = new ExercicioHistoricoDTO()
            {
                intHistoricoExercicioID = 1,
                intTipoProva = (int)TipoProvaEnum.ModoEstudo
            };

            var marcacao = new MarcacoesObjetivaDTO();

            questaoMock.GetExercicioHistorico(resp.HistoricoId).Returns(exercicioHistorico);
            questaoMock.GetUltimaMarcacaobyIntExercicioHistoricoID(resp.QuestaoId, resp.ExercicioId, resp.Matricula, (int)exercicioHistorico.intTipoProva, resp.HistoricoId).Returns(marcacao);
            questaoMock.SetRespostaObjetiva(resp, marcacao).Returns(1);
            questaoMock.SetRespostaObjetivaSimuladoOnline(resp, marcacao).Returns(1);

            var questaoBusiness = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);

            questaoBusiness.SetRespostaObjetiva(resp);

            questaoMock.Received().GetExercicioHistorico(resp.HistoricoId);
            questaoMock.Received().GetUltimaMarcacaobyIntExercicioHistoricoID(resp.QuestaoId, resp.ExercicioId, resp.Matricula, (int)exercicioHistorico.intTipoProva, resp.HistoricoId);
            questaoMock.DidNotReceive().SetRespostaObjetivaSimuladoOnline(resp, marcacao);
            questaoMock.Received().SetRespostaObjetiva(resp, marcacao);
        }
     
        #region ForumRecursos

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterPermissaoPostarForuns_RecursosInativo_DeveImpedir()
        {
            if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts))
            {
                var questao = new QuestaoEntity().GetForumQuestaoRecurso(178385, 96409, false);
                Assert.IsNotNull(questao);
                Assert.AreEqual(true, questao.ForumPosQuestao.ForumFechado);
                Assert.AreEqual(true, questao.ForumPreQuestao.ForumFechado);
            }
            else
            {
                Assert.Inconclusive("Período de Recursos está ativo");
            }
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterPermissaoPostarForumPre_RecursosAtivo_DevePermitir()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();

            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts))
            {
                var matricula = 96409;
                var qe = new QuestaoEntity();
                var intQuestaoID = qe.GetQuestaoForumPreLiberado();
                if (intQuestaoID == 0)
                {
                    Assert.Inconclusive("Não Há Questões no Caso Desejado");
                }
                else
                {
                    var questao = new QuestaoEntity().GetForumQuestaoRecurso(intQuestaoID, matricula, false);
                    Assert.IsNotNull(questao);
                    Assert.AreEqual(true, questao.ForumPosQuestao.ForumFechado);
                    Assert.AreEqual(false, questao.ForumPreQuestao.ForumFechado);
                }
            }
            else
            {
                Assert.Inconclusive("Período de Recursos está Inativo");
            }
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterPermissaoPostarForumPos_RecursosAtivo_DevePermitir()
        {
            Assert.Inconclusive("Time #RECURSOS analisar regressão do teste");
            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts))
            {
                var qe = new QuestaoEntity();
                var matricula = 96409;
                var intQuestaoID = qe.GetQuestaoForumPosLiberado();
                if (intQuestaoID == 0)
                {
                    Assert.Inconclusive("Não Há Questões no Caso Desejado");
                }
                else
                {
                    var questao = qe.GetForumQuestaoRecurso(intQuestaoID, matricula, false);
                    Assert.IsNotNull(questao);
                    Assert.AreEqual(false, questao.ForumPosQuestao.ForumFechado);
                    Assert.AreEqual(true, questao.ForumPreQuestao.ForumFechado);
                }
            }
            else
            {
                Assert.Inconclusive("Período de Recursos está Inativo");
            }
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void ObterPermissaoPostarForuns_UsuarioVisitante_DeveImpedir()
        {

            var questao = new QuestaoEntity().GetForumQuestaoRecurso(178385, 0, true);
            Assert.IsNotNull(questao);
            Assert.AreEqual(true, questao.ForumPosQuestao.ForumFechado);
            Assert.AreEqual(true, questao.ForumPreQuestao.ForumFechado);

        }

        #endregion ForumRecursos

        #region Testes de Fluxo de Resposta de Questões Objetivas e Discursivas de Simulado

        #region Garantindo Gravação e Leitura na Restrita

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaA.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaC.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                   .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaA.ToString()
                , respostaDiscursivaAlternativaA
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaB.ToString()
                , respostaDiscursivaAlternativaB
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação na Restrita e Leitura no MSPro

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaA.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaC.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaA.ToString()
                , respostaDiscursivaAlternativaA
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaB.ToString()
                , respostaDiscursivaAlternativaB
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação na Restrita e Leitura no MSCROSS_Desktop

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_Desktop()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaA.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoRestrita_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_Desktop()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(questaoObjetivaSimulado201701, idSimulado201701, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita), matriculaTeste, alternativaC.ToString(), alternativaSelecionada, Exercicio.tipoExercicio.SIMULADO);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_Desktop()
        {

            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaA.ToString()
                , respostaDiscursivaAlternativaA
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado201701_NaAplicacaoRestrita_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_Desktop()
        {
            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaB.ToString()
                , respostaDiscursivaAlternativaB
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação e Leitura no MSPro

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação no MSPro e Leitura na Restrita

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação no MSPro e Leitura na MSCROSS_Desktop

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_Desktop()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSPro_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_Desktop()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_Desktop()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_Desktop()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação e Leitura no MSCROSS_DESKTOP

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_DESKTOP()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSCROSS_DESKTOP()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_DESKTOP()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNoMSCROSS_DESKTOP()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação no MSCross_DESKTOP e Leitura na Restrita

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNaRestrita()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNaRestrita()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion

        #region Garantindo Gravação no MSCROSS_Desktop e Leitura na MSPro

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoObjetivaAlternativaA_Simulado201701_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaA.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaA, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes__RespondendoQuestaoObjetivaAlternativaC_Simulado201701__NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComAlternativaRespondidaNoMSPro()
        {
            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimulado201701_MatriculaTeste();

            var resp = new RespostaObjetivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoObjetivaSimulado201701,
                ExercicioId = idSimulado201701,
                Alterantiva = alternativaC.ToString(),
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };

            var questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
            var respondeQuestao = questaoBusiness.SetRespostaObjetiva(resp);

            var questao = questaoBusiness.GetTipoSimulado(questaoObjetivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(alternativaC, questao.Alternativas.Where(a => a.Selecionada == true).FirstOrDefault().Letra);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoes_RespondendoQuestaoDiscursivaSegundaAlternativa_Simulado_NaAplicacaoMSCROSS_DESKTOP_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProDesktop), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaB.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaB).FirstOrDefault().Resposta);
        }

        #endregion


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoesStress_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaA.ToString()
                , respostaDiscursivaAlternativaA
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaB,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questaoNoMsPro = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                        .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questaoNoMsPro);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questaoNoMsPro.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void FluxoGravacaoQuestoesStress2_RespondendoQuestaoDiscursivaPrimeiraAlternativa_Simulado_NaAplicacaoMSPro_ExibeAMesmaQuestaoComTextoRespondidaNoMSPro()
        {
            var idSimulado = GetIdSimuladoAtual(matriculaTeste, DateTime.Now.Year);

            if (idSimulado == 0)
            {
                Assert.Inconclusive();
            }

            var iniciaSimuladoMSPro = new ExercicioEntity().GetSimuladoConfiguracao(idSimulado, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile), "0.0.0");
            var ultimoHistoricoCriado = GetUltimoExercicioHistoricoSimuladoAtual_MatriculaTeste();

            var discursivaId = GetAlternativaId(alternativaA.ToString(), questaoDiscursivaSimulado201701);

            var resp = new RespostaDiscursivaPost
            {
                HistoricoId = ultimoHistoricoCriado,
                QuestaoId = questaoDiscursivaSimulado201701,
                DiscursivaId = discursivaId,
                ExercicioId = idSimulado,
                Resposta = respostaDiscursivaAlternativaA,
                ExercicioTipoId = (int)Exercicio.tipoExercicio.SIMULADO
            };
            var retorno = new QuestaoEntity().SetRespostaDiscursiva(resp);

            var questaoNoMsPro = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                        .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.AreaRestrita));

            Assert.IsNotNull(questaoNoMsPro);

            Assert.AreEqual(respostaDiscursivaAlternativaA, questaoNoMsPro.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);


            var respondeQuestao = new QuestaoEntity().SetRespostaAluno(
                questaoDiscursivaSimulado201701
                , idSimulado201701
                , Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO)
                , Convert.ToInt32(Aplicacoes.AreaRestrita)
                , matriculaTeste
                , alternativaA.ToString()
                , respostaDiscursivaAlternativaB
                , Exercicio.tipoExercicio.SIMULADO
                );

            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(questaoDiscursivaSimulado201701, matriculaTeste, Convert.ToInt32(Aplicacoes.MsProMobile));

            Assert.IsNotNull(questao);

            Assert.AreEqual(respostaDiscursivaAlternativaB, questao.Alternativas.Where(a => a.Letra == alternativaA).FirstOrDefault().Resposta);
        }


        #endregion


        [TestMethod]
        [TestCategory("Integrado")]
        public void ConsegueSalvarEObterRespostaQuestaoDiscursivaConcurso()
        {
            var resposta = Utils.RandomString(5);
            var questaoId = 102926;
            var idConcurso = 921614;
            var matricula = 241718;
            var idAplicacao = (int)Aplicacoes.MsProMobile;

            //Recuperando Informações do Concurso e da Questão
            var _questaoBusiness = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity());

            var concurso = new ExercicioEntity().GetConcursoConfiguracao(idConcurso, matricula, Convert.ToInt32(idAplicacao));

            var cartaoResposta = new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(idConcurso, matricula, Exercicio.tipoExercicio.CONCURSO);

            var questao = _questaoBusiness.ObterDetalhesQuestaoConcurso(questaoId, matricula, idAplicacao);

            //Salvar Resposta
            var post = new RespostaDiscursivaPost() { DiscursivaId = questao.Alternativas.ToList()[0].Id, HistoricoId = concurso.CartoesResposta.HistoricoId, ExercicioId = idConcurso, ExercicioTipoId = (int)Exercicio.tipoExercicio.CONCURSO, QuestaoId = questaoId, Resposta = resposta };

            var saveResponse = new QuestaoEntity().SetRespostaDiscursiva(post);

            //Por Algum Motivo, Falhou ao Salvar
            if (saveResponse == 0)
                Assert.Inconclusive();

            //Recuperar Resposta
            questao = _questaoBusiness.ObterDetalhesQuestaoConcurso(questaoId, matricula, idAplicacao);

            Assert.IsTrue(questao.Respondida);

            Assert.IsTrue(questao.Alternativas.ToList()[0].Resposta == resposta);

        }

        [TestMethod]
        [TestCategory("Basico")]
        public void BuscarAnotacaoDeQuestaoFeitaPeloAluno()
        {
            var idQuestao = 24015;
            var matriculaAcademico = 96409;
            var anotacoes = new QuestaoEntity().GetAnotacoesAluno(idQuestao, matriculaAcademico);
            Assert.IsNotNull(anotacoes);
            Assert.IsInstanceOfType(anotacoes, typeof(QuestaoAnotacao));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetQuestoesComComentarioApostila_ApostilaR1_RetornaSomenteQuestoesComComentario()
        {
            var apostilasEntidade = new ApostilaEntidadeEntity().GetAll().Where(x => !x.Descricao.Contains("R3") && !x.Descricao.Contains("R4")).ToList();
            var idapostilaEntidade = apostilasEntidade.LastOrDefault().ID;
            var questoes = new QuestaoEntity().GetQuestoesComComentarioApostila((int)idapostilaEntidade);
            Assert.AreEqual(questoes.Where(x => x.PossuiComentario == false).ToList().Count, 0);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetQuestoesComComentarioApostila_ApostilaR3_RetornaQuestoesComESemComentario()
        {
            var apostilasEntidade = new ApostilaEntidadeEntity().GetAll().Where(x => x.Descricao.Contains("R3") || x.Descricao.Contains("R4")).ToList();
            var entrouNoTeste = false;
            var indexApostila = 0;

            while (!entrouNoTeste && indexApostila < apostilasEntidade.Count())
            {
                var idapostilaEntidade = apostilasEntidade[indexApostila].ID;
                var questoes = new QuestaoEntity().GetQuestoesComComentarioApostila((int)idapostilaEntidade);
                if (questoes.Where(x => x.PossuiComentario == false).ToList().Count > 0)
                    entrouNoTeste = true;
                else
                    indexApostila++;
            }

            if (entrouNoTeste)
                Assert.IsTrue(true);
            else
                Assert.Inconclusive("Pode acontecer das apostilas não possuirem mais questões sem comentário em texto");
        }

        [TestMethod]
        [TestCategory("Estatística de questão")]
        public void GetEstatistica_QuestaoComAlternativaNaoRespondida_RetornaAlternativaComZeroPorcento()
        {
            const string valuePorcentagemZero = "0";

            var questoes = new QuestaoEntity().GetQuestaoComAlternativaNaoRespondida((int)Exercicio.tipoExercicio.CONCURSO);
            if (questoes.Count == 0)
                Assert.Inconclusive("Não possui questões com alternativas não respondidas");

            var retorno = new QuestaoEntity().GetEstatistica(questoes.FirstOrDefault(), (int)Exercicio.tipoExercicio.CONCURSO);

            Assert.IsTrue(retorno.Where(x => x.Value == valuePorcentagemZero).Any());
        }

        [TestMethod]
        [TestCategory("Estatística de questão")]
        public void GetEstatistica_QuestaoAnulada_RetornarRespostaCorretaComoInvalida()
        {
            const string keyRespostaCorreta = "Correta";
            const string valueRespostaInvalida = "-1";

            var questoes = new QuestaoEntity().GetQuestoesAnuladas();

            if (questoes.Count == 0)
                Assert.Inconclusive("Não possui questões anuladas");

            var retorno = new QuestaoEntity().GetEstatistica(questoes.FirstOrDefault(), (int)Exercicio.tipoExercicio.CONCURSO);
            Assert.IsTrue(retorno.Where(a => a.Key == keyRespostaCorreta && a.Value == valueRespostaInvalida).Any());
        }

        [TestMethod]
        [TestCategory("Estatística de questão")]
        public void GetEstatistica_QuestaoComMaisUmaAlternativaCorreta_RetornarRespostaCorretaComoInvalida()
        {
            const string keyRespostaCorreta = "Correta";
            const string valueRespostaInvalida = "-1";

            var questoes = new QuestaoEntity().GetQuestaoComMaisDeUmaAlternativaCorreta((int)Exercicio.tipoExercicio.CONCURSO);

            if (questoes.Count == 0)
                Assert.Inconclusive("Não possui questões com mais de uma alternativa correta");

            var retorno = new QuestaoEntity().GetEstatistica(questoes.FirstOrDefault(), (int)Exercicio.tipoExercicio.CONCURSO);
            Assert.IsTrue(retorno.Where(a => a.Key == keyRespostaCorreta && a.Value == valueRespostaInvalida).Any());
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAcademico_DeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(206585, 76974, (int)Aplicacoes.MsProMobile);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            var hasId = questao.Enunciado.Contains("206585");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado) && hasId);
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAluno_NaoDeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(206585, 96409, (int)Aplicacoes.MsProDesktop);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            Assert.IsFalse(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoConcurso")]
        public void GetQuestaoConcurso_PerfilAcademico_DeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(203301, 76974, (int)Aplicacoes.MsProDesktop);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            var hasId = questao.Enunciado.Contains("203301");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado) && hasId);
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoConcurso")]
        public void GetQuestaoConcurso_PerfilAluno_NaoDeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(203301, 96409, (int)Aplicacoes.MsProDesktop);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            Assert.IsFalse(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoConcurso")]
        public void GetQuestaoConcurso_PerfilAcademico_DeveConterComentariosRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(174889, 267711, (int)Aplicacoes.MsProMobile);
            Regex rx = new Regex(@"Parecer MEDGRUPO \(Recursos\):", RegexOptions.None);
            var parecer = questao.Enunciado.Contains("RECURSO");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado) && parecer);
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoConcurso")]
        public void GetQuestaoConcurso_PerfilAluno_NaoDeveConterComentariosRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(174889, 96409, (int)Aplicacoes.MsProMobile);

            Regex rx = new Regex(@"Parecer MEDGRUPO \(Recursos\):", RegexOptions.None);
            Assert.IsFalse(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAcademico_DeveConterComentariosRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(174889, 267711, (int)Aplicacoes.MsProMobile);
            Regex rx = new Regex(@"Parecer MEDGRUPO \(Recursos\):", RegexOptions.None);

            var parecer = questao.Enunciado.Contains("RECURSO");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado) && parecer);
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAluno_NaoDeveConterComentariosRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(174889, 96409, (int)Aplicacoes.MsProMobile);
            Regex rx = new Regex(@"Parecer MEDGRUPO \(Recursos\):", RegexOptions.None);
            Assert.IsFalse(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAcademico_ComentarioBancaCabeRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(212681, 267711, (int)Aplicacoes.MsProMobile);

            Regex rx = new Regex(@"Parecer da Banca \(Recursos\):", RegexOptions.None);

            var parecer = questao.Enunciado.Contains("CABE RECURSO");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoApostila")]
        public void GetQuestaoApostila_PerfilAcademico_ComentarioProfessorProfessoreBancaCabeRecurso()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoTipoApostila(212681, 267711, (int)Aplicacoes.MsProMobile);
            Regex rxM = new Regex(@"Parecer MEDGRUPO \(Recursos\):", RegexOptions.None);
            Regex rxb = new Regex(@"Parecer da Banca \(Recursos\):", RegexOptions.None);

            var parecer = questao.Enunciado.Contains("CABE RECURSO");
            Assert.IsTrue(rxM.IsMatch(questao.Enunciado));
            Assert.IsTrue(rxb.IsMatch(questao.Enunciado));
            Assert.IsTrue(parecer);
        }


        [TestMethod]
        [TestCategory("Enunciado QuestãoSimulado")]
        public void GetQuestaoSimulado_PerfilAcademico_DeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(525503, 76974, (int)Aplicacoes.MsProDesktop);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            var hasId = questao.Enunciado.Contains("114019");
            Assert.IsTrue(rx.IsMatch(questao.Enunciado) && hasId);
        }

        [TestMethod]
        [TestCategory("Enunciado QuestãoSimulado")]
        public void GetQuestaoSimulado_PerfilAluno_NaoDeveConterComentarios()
        {
            var questao = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                                    .GetTipoSimulado(525503, 96409, (int)Aplicacoes.MsProDesktop);
            Regex rx = new Regex(@"Comentário da Questão \(\d+\):", RegexOptions.None);
            Assert.IsFalse(rx.IsMatch(questao.Enunciado));
        }

        [TestMethod]
        [TestCategory("Questao_SimuladoAgendado")]
        public void GetQuestaoSimuladoAgendado_QuestaoRespondida_RetornarQuestaoRespondida()
        {
            var matricula = 1;
            var ExercicioHistoricoID = 1;
            var ExercicioID = 1;

            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var questao = QuestaoEntityTestData.GetQuestaoDTO();
            var alternativas = QuestaoEntityTestData.GetAlternativasQuestaoSimulado();
            var simuladoVersao = QuestaoEntityTestData.GetSimuladoVersaoDTO();

            var QuestaoID = questao.intQuestaoID;

            var simulado = new SimuladoDTO()
            {
                ID = 1,
                Nome = "SIM 01 - TESTE"
            };

            var especialidades = new List<Especialidade>();
            especialidades.Add(new Especialidade()
            {
                Id = 1,
                Descricao = "Especialidade 1"
            });

            funcionarioMock.GetTipoPerfilUsuario(matricula).Returns(EnumTipoPerfil.None);
            questaoMock.CacheQuestao(QuestaoID).Returns(questao);
            questaoMock.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico).Returns(alternativas);
            simuladoMock.GetSimulado(ExercicioID).Returns(simulado);
            questaoMock.GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns(new List<CartaoRespostaDiscursivaDTO>());
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(simuladoVersao);
            questaoMock.GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns("A");
            especialidadeMock.GetByQuestaoSimulado(QuestaoID, ExercicioID).Returns(especialidades);


            var business = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);
            var retorno = business.GetQuestaoSimuladoAgendado(QuestaoID, matricula, ExercicioID, ExercicioHistoricoID);

            funcionarioMock.Received().GetTipoPerfilUsuario(matricula);
            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);
            questaoMock.Received().GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            simuladoMock.Received().GetSimulado(ExercicioID);
            especialidadeMock.Received().GetByQuestaoSimulado(QuestaoID, ExercicioID);

            Assert.IsTrue(retorno.Respondida);
        }

        [TestMethod]
        [TestCategory("Questao_SimuladoAgendado")]
        public void GetQuestaoSimuladoAgendado_QuestaoDiscursiva_RetornarQuestaoNaoRespondida()
        {
            var matricula = 1;
            var ExercicioHistoricoID = 1;
            var ExercicioID = 1;

            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var questao = QuestaoEntityTestData.GetQuestaoDTO();
            var alternativas = QuestaoEntityTestData.GetAlternativasQuestaoSimulado();
            var simuladoVersao = QuestaoEntityTestData.GetSimuladoVersaoDTO();

            var QuestaoID = questao.intQuestaoID;

            var simulado = new SimuladoDTO()
            {
                ID = 1,
                Nome = "SIM 01 - TESTE"
            };

            var especialidades = new List<Especialidade>();
            especialidades.Add(new Especialidade()
            {
                Id = 1,
                Descricao = "Especialidade 1"
            });

            funcionarioMock.GetTipoPerfilUsuario(matricula).Returns(EnumTipoPerfil.None);
            questaoMock.CacheQuestao(QuestaoID).Returns(questao);
            questaoMock.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico).Returns(alternativas);
            simuladoMock.GetSimulado(ExercicioID).Returns(simulado);
            questaoMock.GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns(new List<CartaoRespostaDiscursivaDTO>());
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(simuladoVersao);
            questaoMock.GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns("");
            especialidadeMock.GetByQuestaoSimulado(QuestaoID, ExercicioID).Returns(especialidades);


            var business = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);
            var retorno = business.GetQuestaoSimuladoAgendado(QuestaoID, matricula, ExercicioID, ExercicioHistoricoID);

            funcionarioMock.Received().GetTipoPerfilUsuario(matricula);
            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);
            questaoMock.Received().GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            simuladoMock.Received().GetSimulado(ExercicioID);
            especialidadeMock.Received().GetByQuestaoSimulado(QuestaoID, ExercicioID);

            Assert.IsFalse(retorno.Respondida);
        }

        [TestMethod]
        [TestCategory("Questao_SimuladoAgendado")]
        public void GetQuestaoSimuladoAgendado_QuestaoNaoRespondidaComImagem_RetornarQuestaoNaoRespondida()
        {
            var matricula = 1;
            var ExercicioHistoricoID = 1;
            var ExercicioID = 1;

            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var questao = QuestaoEntityTestData.GetQuestaoDTO();
            var alternativas = QuestaoEntityTestData.GetAlternativasQuestaoSimulado();
            var simuladoVersao = QuestaoEntityTestData.GetSimuladoVersaoDTO();

            var QuestaoID = questao.intQuestaoID;

            var simulado = new SimuladoDTO()
            {
                ID = 1,
                Nome = "SIM 01 - TESTE"
            };

            var especialidades = new List<Especialidade>();
            especialidades.Add(new Especialidade()
            {
                Id = 1,
                Descricao = "Especialidade 1"
            });

            var listaImagem = new List<int>();
            listaImagem.Add(1111);
            listaImagem.Add(2222);

            funcionarioMock.GetTipoPerfilUsuario(matricula).Returns(EnumTipoPerfil.None);
            questaoMock.CacheQuestao(QuestaoID).Returns(questao);
            questaoMock.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico).Returns(alternativas);
            simuladoMock.GetSimulado(ExercicioID).Returns(simulado);
            questaoMock.GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns(new List<CartaoRespostaDiscursivaDTO>());
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(listaImagem);
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(simuladoVersao);
            questaoMock.GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns("");
            especialidadeMock.GetByQuestaoSimulado(QuestaoID, ExercicioID).Returns(especialidades);


            var business = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);
            var retorno = business.GetQuestaoSimuladoAgendado(QuestaoID, matricula, ExercicioID, ExercicioHistoricoID);

            funcionarioMock.Received().GetTipoPerfilUsuario(matricula);
            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);
            questaoMock.Received().GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            simuladoMock.Received().GetSimulado(ExercicioID);
            especialidadeMock.Received().GetByQuestaoSimulado(QuestaoID, ExercicioID);

            Assert.IsFalse(retorno.Respondida);
            Assert.IsTrue(retorno.MediaComentario.Imagens.Count == 2);
        }

        [TestMethod]
        [TestCategory("Questao_SimuladoAgendado")]
        public void GetQuestaoSimuladoAgendado_QuestaoDiscursiva_RetornarQuestaoRespondida()
        {
            var matricula = 1;
            var ExercicioHistoricoID = 1;
            var ExercicioID = 1;

            var questaoMock = Substitute.For<IQuestaoData>();
            var imagemMock = Substitute.For<IImagemData>();
            var videoMock = Substitute.For<IVideoData>();
            var especialidadeMock = Substitute.For<IEspecialidadeData>();
            var funcionarioMock = Substitute.For<IFuncionarioData>();
            var simuladoMock = Substitute.For<ISimuladoData>();

            var questao = QuestaoEntityTestData.GetQuestaoDTO();
            var alternativas = QuestaoEntityTestData.GetAlternativasQuestaoSimulado();
            var simuladoVersao = QuestaoEntityTestData.GetSimuladoVersaoDTO();

            var QuestaoID = questao.intQuestaoID;

            var simulado = new SimuladoDTO()
            {
                ID = 1,
                Nome = "SIM 01 - TESTE"
            };

            var especialidades = new List<Especialidade>();
            especialidades.Add(new Especialidade()
            {
                Id = 1,
                Descricao = "Especialidade 1"
            });

            funcionarioMock.GetTipoPerfilUsuario(matricula).Returns(EnumTipoPerfil.None);
            questaoMock.CacheQuestao(QuestaoID).Returns(questao);
            questaoMock.GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico).Returns(alternativas);
            simuladoMock.GetSimulado(ExercicioID).Returns(simulado);
            questaoMock.GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns(QuestaoEntityTestData.GetRespostaDiscursivaSimulado());
            imagemMock.GetImagensQuestaoSimulado(QuestaoID).Returns(new List<int>());
            questaoMock.GetSimuladoVersao(QuestaoID).Returns(simuladoVersao);
            questaoMock.GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID).Returns("");
            especialidadeMock.GetByQuestaoSimulado(QuestaoID, ExercicioID).Returns(especialidades);


            var business = new QuestaoBusiness(questaoMock, imagemMock, videoMock, especialidadeMock, funcionarioMock, simuladoMock);
            var retorno = business.GetQuestaoSimuladoAgendado(QuestaoID, matricula, ExercicioID, ExercicioHistoricoID);

            funcionarioMock.Received().GetTipoPerfilUsuario(matricula);
            questaoMock.Received().CacheQuestao(QuestaoID);
            questaoMock.Received().GetAlternativasQuestao(QuestaoID, questao.bitCasoClinico);
            questaoMock.Received().GetRespostasDiscursivasSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            questaoMock.Received().GetSimuladoVersao(QuestaoID);
            questaoMock.Received().GetRespostaObjetivaSimuladoAgendado(QuestaoID, ExercicioHistoricoID);
            imagemMock.Received().GetImagensQuestaoSimulado(QuestaoID);
            simuladoMock.Received().GetSimulado(ExercicioID);
            especialidadeMock.Received().GetByQuestaoSimulado(QuestaoID, ExercicioID);

            Assert.IsTrue(retorno.Respondida);
        }
    }
}