using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesMockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class QuestaoConcursoEntityTests
    {
        [TestMethod]
        public void ObterQuestaoCompleta_PorId_RetornaQuestaoPreenchida()
        {
            var idQuestao = 183655;
            var idFuncionario = 96557;

            var questaoCompleta = new QuestaoConcursoEntity().ObterQuestaoCompleta(idQuestao, idFuncionario);

            Assert.IsNotNull(questaoCompleta);
            Assert.IsInstanceOfType(questaoCompleta, typeof(PPQuestao));
        }

        [TestMethod]
        public void ObterQuestaoConcursoImagemEnunciado_PorId()
        {
            var idImagem = 24356;
            var imagem = new ImagemEntity().GetConcurso(idImagem);

            Assert.IsNotNull(imagem);
            Assert.IsInstanceOfType(imagem, typeof(System.IO.Stream));
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterLetraStatusQuestao_QuestaoAlteradoBanca_LetraAlteradoBanca()
        {
            var business = new QuestaoBusiness(null, null, null, null);

            var recursoQuestao = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO
                {
                    BancaCabeRecurso = true
                }
            };

            var letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.AlteradaPelaBanca.GetDescription(), letra);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterLetraStatusQuestao_QuestaoAlteradoBanca_LetraCabeRecurso()
        {
            var business = new QuestaoBusiness(null, null, null, null);

            var recursoQuestao = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO
                {
                    BancaCabeRecurso = false,
                    ExisteAnaliseProfessor = true,
                    ForumPreAnalise = new ForumPreRecursoDTO
                    {
                        AnaliseProfessorCabeRecurso = true
                    }
                }
            };

            var letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.CabeRecurso.GetDescription(), letra);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterLetraStatusQuestao_QuestaoAlteradoBanca_LetraNaoCabeRecurso()
        {
            var business = new QuestaoBusiness(null, null, null, null);

            var recursoQuestao = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO
                {
                    BancaCabeRecurso = null,
                    ExisteAnaliseProfessor = true,
                    ForumPreAnalise = new ForumPreRecursoDTO
                    {
                        AnaliseProfessorCabeRecurso = false
                    }
                }
            };

            var letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoCabeRecurso.GetDescription(), letra);

            recursoQuestao.ForumRecurso.BancaCabeRecurso = false;
            letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoCabeRecurso.GetDescription(), letra);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterLetraStatusQuestao_QuestaoAlteradoBanca_LetraEmAnalise()
        {
            var business = new QuestaoBusiness(null, null, null, null);

            var recursoQuestao = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO
                {
                    BancaCabeRecurso = null,
                    ExisteAnaliseProfessor = false,
                    IdAnaliseProfessorStatus = (int)QuestaoRecurso.StatusQuestao.EmAnalise,
                    ForumPreAnalise = new ForumPreRecursoDTO()
                }
            };

            var letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.EmAnalise.GetDescription(), letra);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterLetraStatusQuestao_QuestaoAlteradoBanca_LetraNaoSolicitado()
        {
            var business = new QuestaoBusiness(null, null, null, null);

            var recursoQuestao = new RecursoQuestaoConcursoDTO
            {
                ForumRecurso = new ForumRecursoDTO
                {
                    BancaCabeRecurso = null,
                    ExisteAnaliseProfessor = false,
                    ForumPreAnalise = new ForumPreRecursoDTO()
                }
            };

            var letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoSolicitado.GetDescription(), letra);

            recursoQuestao.ForumRecurso.ForumPreAnalise.QtdNaocabe = 2;

            letra = business.ObterLetraStatusQuestao(recursoQuestao);

            Assert.IsNotNull(letra);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoSolicitado.GetDescription(), letra);
        }

        [TestMethod]
        [TestCategory("Alternativa Questao Forum Recurso")]
        public void GetQuestaoConcursoRecurso_AcertaGabarito_LetraCGabaritoOficial()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questaoId = 1;
            var matricula = 0;
            var imagemId = 7;

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetQuestaoConcursoRecurso(questaoId, matricula).Returns(questao);
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.ObterComentariosForumConcursoPre(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterComentariosForumConcursoPos(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterImagensComentarioRecurso(questaoId).Returns(new List<tblConcursoQuestoes_recursosComentario_Imagens>());
            mock.AlunoSelecionouAlternativaQuestaoProva(1,1).ReturnsForAnyArgs(false);
            mockImage.GetImagensQuestaoConcurso(questaoId).Returns(new List<int> { imagemId });

            var q = business.GetQuestaoConcursoRecurso(questaoId, matricula);

            Assert.IsNotNull(q);
            Assert.AreEqual(QuestaoRecurso.StatusGabarito.GabaritoOficial.GetDescription(), q.Questao.GabaritoTipo);
            Assert.AreEqual("C", q.Questao.GabaritoLetra);
            Assert.AreEqual(imagemId, q.Questao.EnunciadoImagensId.First());
        }

        [TestMethod]
        [TestCategory("Alternativa Questao Forum Recurso")]
        public void GetQuestaoConcursoRecurso_MultiplosVotos_QtdCabeNaoCabeUltimoVotoCorreto()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questaoId = 1;
            var matricula = 0;
            var imagemId = 7;
            var qtdUltimoVotosfavor = 2;
            var qtdUltimoVotosContra = 1;

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            var comentarios = QuestaoRecursoEntityTestData.GetComentariosVotosQuestaoUltimoVotoAfirma();

            mock.GetQuestaoConcursoRecurso(questaoId, matricula).Returns(questao);
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.ObterComentariosForumConcursoPre(questaoId, matricula).Returns(comentarios);
            mock.ObterComentariosForumConcursoPos(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterImagensComentarioRecurso(questaoId).Returns(new List<tblConcursoQuestoes_recursosComentario_Imagens>());
            mock.AlunoSelecionouAlternativaQuestaoProva(1,1).ReturnsForAnyArgs(false);
            mockImage.GetImagensQuestaoConcurso(questaoId).Returns(new List<int> { imagemId });

            var q = business.GetQuestaoConcursoRecurso(questaoId, matricula);

            Assert.IsNotNull(q);
            Assert.AreEqual(qtdUltimoVotosfavor, q.ForumRecurso.ForumPreAnalise.QtdCabe);
            Assert.AreEqual(qtdUltimoVotosContra, q.ForumRecurso.ForumPreAnalise.QtdNaocabe);
            Assert.AreEqual(QuestaoRecurso.StatusOpiniao.Favor.GetDescription(), q.ForumRecurso.ForumPreAnalise.VotoAluno);
        }

        [TestMethod]
        [TestCategory("Alternativa Questao Forum Recurso")]
        public void SelecionarAlternativaQuestaoConcurso_RespostaNaoExistenteSalvaPrimeiroChar_LetraColunaPrimeiroEColunaNovoVazio()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var novaLetra = "D";

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetConcursoQuestoesAlunoFavorita(questaoId, matricula).ReturnsNull();
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.InserirQuestaoConcursoAlunoFavoritas(null).ReturnsForAnyArgs(1);

            var qtd = business.SelecionarAlternativaQuestaoConcurso(questaoId, matricula, novaLetra);

            mock.Received().InserirQuestaoConcursoAlunoFavoritas(
                Arg.Is<tblConcursoQuestoes_Aluno_Favoritas>(
                    f => f.charResposta == novaLetra && f.charRespostaNova == null));


            Assert.AreEqual(1, qtd);
        }

        [TestMethod]
        [TestCategory("Alternativa Questao Forum Recurso")]
        public void SelecionarAlternativaQuestaoConcurso_RespostaExistenteSalvarSegundoChar_LetraColunaPrimeiroEColunaNovaResposta()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var novaLetra = "D";
            var velhaLetra = "A";

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();

            mock.GetConcursoQuestoesAlunoFavorita(questaoId, matricula).Returns(new tblConcursoQuestoes_Aluno_Favoritas
            {
                charResposta = velhaLetra,
                charRespostaNova = novaLetra
            });

            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.UpdateQuestoesConcursoAlunoFavoritas(new tblConcursoQuestoes_Aluno_Favoritas()).ReturnsForAnyArgs(1);

            var qtd = business.SelecionarAlternativaQuestaoConcurso(questaoId, matricula, novaLetra);

            mock.Received().UpdateQuestoesConcursoAlunoFavoritas(
                Arg.Is<tblConcursoQuestoes_Aluno_Favoritas>(
                    f => f.charResposta == velhaLetra && f.charRespostaNova == novaLetra));

            Assert.AreEqual(1, qtd);
        }

        [TestMethod]
        [TestCategory("Alternativa Questao Forum Recurso")]
        public void SelecionarAlternativaQuestaoConcurso_GravaRespostaCertaOuErrada_TrueSeAcertouFalseSeErrou()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var respostaCerta = "C";
            var respostaErrada = "A";

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetConcursoQuestoesAlunoFavorita(questaoId, matricula).ReturnsNull();
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.InserirQuestaoConcursoAlunoFavoritas(null).ReturnsForAnyArgs(1);

            var qtd = business.SelecionarAlternativaQuestaoConcurso(questaoId, matricula, respostaCerta);

            mock.Received().InserirQuestaoConcursoAlunoFavoritas(
                Arg.Is<tblConcursoQuestoes_Aluno_Favoritas>(
                    f => f.bitResultadoResposta.HasValue && f.bitResultadoResposta.Value));

            business.SelecionarAlternativaQuestaoConcurso(questaoId, matricula, respostaErrada);

            mock.Received().InserirQuestaoConcursoAlunoFavoritas(
                Arg.Is<tblConcursoQuestoes_Aluno_Favoritas>(
                     f => !f.bitResultadoResposta.HasValue || !f.bitResultadoResposta.Value));

            Assert.AreEqual(1, qtd);
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPos_TestaEnviarSeAlunoVotou_Um()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pos).Returns(true);
            mock.EnviarVotoComentarioForum(
                    0, 0, null, null, new QuestaoRecurso.TipoForumRecurso()).ReturnsForAnyArgs(1);

            var result = business.EnviarComentarioVotoForumPos(
                questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(), "Comentario novo"
                );

            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>());
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPos_TestarEnviarSeAlunoNaoVotou_Um()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var comentario = "Comentario novo";

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pos).Returns(false);
            mock.EnviarVotoComentarioForum(
                    0, 0, null, null, new QuestaoRecurso.TipoForumRecurso()
                    ).ReturnsForAnyArgs(1);


            var result = business.EnviarComentarioVotoForumPos(
                questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(), comentario
                );

            
            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    Arg.Is<int>(questaoId), Arg.Is<int>(matricula), Arg.Is<string>(QuestaoRecurso.StatusOpiniao.Favor.GetDescription()),
                   Arg.Is<string>(comentario), Arg.Is<QuestaoRecurso.TipoForumRecurso>(QuestaoRecurso.TipoForumRecurso.Pos));
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPre_TestaEnviarSeAlunoVotou_Um()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pre).Returns(true);
            mock.GetQuestaoConcursoById(questaoId).Returns(new tblConcursoQuestoes
            {
                intQuestaoID = questaoId,
                ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.EmAnalise
            });
            mock.EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()).Returns(1);

            var result = business.EnviarComentarioVotoForumPre(
                questaoId, matricula, QuestaoRecurso.StatusOpiniao.Contra.GetDescription(), "Comentario novo forum pre"
                );

            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()
                    );
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPre_TestarEnviarSeAlunoNaoVotou_Um()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var comentario = "Comentario novo pre";

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pre).Returns(false);
            mock.GetQuestaoConcursoById(questaoId).Returns(new tblConcursoQuestoes
            {
                intQuestaoID = questaoId,
                ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.EmAnalise
            });
            mock.EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()
                    ).Returns(1);
            
            var result = business.EnviarComentarioVotoForumPre(
                questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(), comentario
                );
            
            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(),
                   comentario, QuestaoRecurso.TipoForumRecurso.Pre);

            mock.DidNotReceive().UpdateQuestaoConcurso(Arg.Any<tblConcursoQuestoes>());
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPre_TestarIniciarForum_MetodoIniciarChamado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var comentario = "Comentario novo pre";

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pre).Returns(false);
            mock.GetQuestaoConcursoById(questaoId).Returns(new tblConcursoQuestoes
            {
                intQuestaoID = questaoId,
                ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.NaoSolicitado
            });
            mock.EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()).Returns(1);

            mock.UpdateQuestaoConcurso(Arg.Any<tblConcursoQuestoes>()).Returns(1);

            var result = business.EnviarComentarioVotoForumPre(
                questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(), comentario
                );

            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(),
                   comentario, QuestaoRecurso.TipoForumRecurso.Pre);

            mock.Received().UpdateQuestaoConcurso(
                Arg.Is<tblConcursoQuestoes>(b => b.intQuestaoID == questaoId
                   && b.ID_CONCURSO_RECURSO_STATUS == (int)QuestaoRecurso.StatusQuestao.EmAnalise));
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPre_TestarEnviarEmailSeVotoEnviado_MetodoChamado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var comentario = "Comentario novo pre";
            var voto = QuestaoRecurso.StatusOpiniao.Favor;

            mock.AlunoJaVotouForumQuestao(questaoId, matricula, QuestaoRecurso.TipoForumRecurso.Pre).Returns(false);
            mock.GetQuestaoConcursoById(questaoId).Returns(new tblConcursoQuestoes
            {
                intQuestaoID = questaoId,
                ID_CONCURSO_RECURSO_STATUS = (int)QuestaoRecurso.StatusQuestao.EmAnalise
            });
            mock.EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()).Returns(1);

            var result = business.EnviarComentarioVotoForumPre(
                questaoId, matricula, voto.GetDescription(), comentario
                );

            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    questaoId, matricula, QuestaoRecurso.StatusOpiniao.Favor.GetDescription(),
                   comentario, QuestaoRecurso.TipoForumRecurso.Pre);

            mock.Received().EnvioEmailComentarioForumPreAsync(
                    questaoId, matricula, comentario, voto == QuestaoRecurso.StatusOpiniao.Favor);
            
        }

        [TestMethod]
        [TestCategory("Comentario Voto Forum Recurso")]
        public void EnviarComentarioVotoForumPos_TestarEnviarEmailSeVotoEnviado_MetodoChamado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var matricula = 0;
            var comentario = "Comentario novo pos";
            var voto = QuestaoRecurso.StatusOpiniao.Contra;
            
            mock.EnviarVotoComentarioForum(
                    Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<QuestaoRecurso.TipoForumRecurso>()).Returns(1);

            var result = business.EnviarComentarioVotoForumPos(
                questaoId, matricula, voto.GetDescription(), comentario
                );

            Assert.AreEqual(1, result);
            mock.Received().EnviarVotoComentarioForum(
                    questaoId, matricula, voto.GetDescription(),
                   comentario, QuestaoRecurso.TipoForumRecurso.Pos);

            mock.Received().EnvioEmailComentarioForumPosAsync(
                    questaoId, matricula, comentario, voto == QuestaoRecurso.StatusOpiniao.Favor);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void EnviarAnaliseMedgrupoAluno_TestaRetornoEmailAluno_Email()
        {
            var matricula = 1;
            var idQuestao = 2;
            var email = "desenv@medgrupo.com.br";
            var mock = Substitute.For<IQuestaoData>();

            mock.GetQuestaoConcursoRecurso(idQuestao, matricula).Returns(new RecursoQuestaoConcursoDTO
            {
                Questao = new QuestaoConcursoRecursoDTO(),
                Prova = new ProvaConcursoDTO(),
                ForumRecurso = new ForumRecursoDTO
                {
                    ForumPreAnalise = new ForumPreRecursoDTO { TextoAnaliseProfessor = "Teste do envio de email" }
                }
            });
            mock.GetEmailEnvioAnaliseQuestaoAluno(matricula).Returns(email);
            
            var business = new QuestaoBusiness(mock, null, null, null);
            var result = business.EnviarAnaliseMedgrupoAluno(idQuestao, matricula);

            Assert.AreEqual(email, result);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void EnviarAnaliseMedgrupoAluno_TestaBuscaImagensAnalise_ChamaMetodo()
        {
            var matricula = 1;
            var idQuestao = 2;
            var email = "desenv@medgrupo.com.br";
            var mock = Substitute.For<IQuestaoData>();

            mock.GetQuestaoConcursoRecurso(idQuestao, matricula).Returns(new RecursoQuestaoConcursoDTO
            {
                Questao = new QuestaoConcursoRecursoDTO(),
                Prova = new ProvaConcursoDTO(),
                ForumRecurso = new ForumRecursoDTO
                {
                    ForumPreAnalise = new ForumPreRecursoDTO { TextoAnaliseProfessor = "Teste do envio de email" }
                }
            });
            mock.GetEmailEnvioAnaliseQuestaoAluno(matricula).Returns(email);

            var business = new QuestaoBusiness(mock, null, null, null);
            var result = business.EnviarAnaliseMedgrupoAluno(idQuestao, matricula);

            mock.Received().ObterImagensComentarioRecurso(idQuestao);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void ObterAnexosAnaliseProfessorRecurso_VerificaConstrucaoUrlImagem_UrlStaticMaisLabel()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var questaoId = 1;
            var imagemIdUm = "e82cfee0-c4c2-467a-95fb-796f20d1f90f.jpg";

            var recursoImagem = new List<tblConcursoQuestoes_recursosComentario_Imagens>
            {
                new tblConcursoQuestoes_recursosComentario_Imagens
                {
                    intID = 1,
                    intClassificacao = string.Empty,
                    intQuestao = questaoId,
                    txtLabel = imagemIdUm,

                },
                new tblConcursoQuestoes_recursosComentario_Imagens
                {
                    intID = 2,
                    intClassificacao = string.Empty,
                    intQuestao = questaoId,
                    txtLabel = "e2b07d26-cf2f-4849-aac2-819c3f70772c.JPG",

                },
                new tblConcursoQuestoes_recursosComentario_Imagens
                {
                    intID = 3,
                    intClassificacao = string.Empty,
                    intQuestao = questaoId,
                    txtLabel = string.Empty,
                }
            };

            mock.ObterImagensComentarioRecurso(questaoId).Returns(recursoImagem);


            var result = business.ObterAnexosAnaliseProfessorRecurso(questaoId);

            Assert.IsNotNull(result);
            Assert.AreEqual(recursoImagem.Count(i => !string.IsNullOrEmpty(i.txtLabel)), result.Count());

            var anexoUm = result.FirstOrDefault(f => f.IdImagem == 1);
            Assert.AreEqual(Constants.LINK_STATIC_ANEXO_ANALISERECURSOS + imagemIdUm, anexoUm != null ? anexoUm.UrlImagem : string.Empty);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_QuantidadeQuestoes_NumeroQuestoesProva()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);
            
            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(false, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);

            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);
            
            Assert.AreEqual(questoes.Count, result.Questoes.Count());

            mock.Received().GetQuestoesProvaConcurso(idProva);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_AvisoProvaConcurso_AvisoProva()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);
            
            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var avisoProva = questoes.ElementAt(0).Prova.PainelAviso;

            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(true, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);
            
            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);

            Assert.AreEqual(avisoProva, result.Prova.PainelAviso);

            mock.Received().GetQuestoesProvaConcurso(idProva);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_StatusQuestoes_StatusDecisao()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();

            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(true, idQuestoes);
            votos.First(v => v.IdQuestao == 3).QtdCabeRecurso = 0;

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);
            
            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);

            var questaoCabe = result.Questoes.First(q => q.Id == 1);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.CabeRecurso.GetDescription(), questaoCabe.StatusQuestao);

            var questaoNaoCabe = result.Questoes.First(q => q.Id == 2);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoCabeRecurso.GetDescription(), questaoNaoCabe.StatusQuestao);

            var questaoNaoSolicitado = result.Questoes.First(q => q.Id == 3);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.NaoSolicitado.GetDescription(), questaoNaoSolicitado.StatusQuestao);

            var questaoEmAnalise = result.Questoes.First(q => q.Id == 4);
            Assert.AreEqual(QuestaoRecurso.StatusQuestao.EmAnalise.GetDescription(), questaoEmAnalise.StatusQuestao);

            mock.Received().GetQuestoesProvaConcurso(idProva);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_ComunicadoInativo_TextoVazio()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(false, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);

            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);
            mock.Received().GetQuestoesProvaConcurso(idProva);

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Prova.Comunicado);
        }
        
        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_ComunicadoDataLimite_TextoVazio()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(false, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);

            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);
            mock.Received().GetQuestoesProvaConcurso(idProva);

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Prova.Comunicado);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_QuantidadeQuestoesRanking_Oitenta()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(false, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);

            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);
            mock.Received().GetQuestoesProvaConcurso(idProva);

            Assert.IsNotNull(result);
            Assert.AreEqual(80, result.Prova.QtdQuestoes);
        }

        [TestMethod]
        [TestCategory("Status Questao Forum Recurso")]
        public void GetQuestoesProvaConcursoComStatus_TituloNoPainel_TextoDeTitulo()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);

            var idProva = 2;
            var questoes = QuestaoRecursoEntityTestData.GetQuestoesConcurso();
            var idQuestoes = questoes.Select(q => q.Questao.Id).ToArray();
            var votos = QuestaoRecursoEntityTestData.GetQuestaoConcursoVotos(false, idQuestoes);

            mock.GetQuestoesProvaConcurso(idProva).Returns(questoes);
            mock.GetQtdCabeQuestoesConcurso(idQuestoes).Returns(votos);
            mock.AlunoTemRankAcertos(idProva, 1).Returns(false);
            mock.AlunoSelecionouAlternativaQuestaoProva(idProva, 1).Returns(false);

            var result = business.GetQuestoesProvaConcursoComStatus(idProva, 1);
            mock.Received().GetQuestoesProvaConcurso(idProva);

            Assert.IsNotNull(result);
            Assert.AreEqual("Titulo", result.Prova.PainelAvisoTitulo);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPreRecursosForumFechado_TestaProvaExpiradaBloqueia_ForumPreBloqueado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questaoId = 1;
            var matricula = 0;
            var imagemId = 7;

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetQuestaoConcursoRecurso(questaoId, matricula).Returns(questao);
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.ObterComentariosForumConcursoPre(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterComentariosForumConcursoPos(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterImagensComentarioRecurso(questaoId).Returns(new List<tblConcursoQuestoes_recursosComentario_Imagens>());
            mock.AlunoSelecionouAlternativaQuestaoProva(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
            mockImage.GetImagensQuestaoConcurso(questaoId).Returns(new List<int> { imagemId });

            var q = business.GetQuestaoConcursoRecurso(questaoId, matricula);
            q.Prova.StatusProva = (int)ProvaRecurso.StatusProva.RecursosExpirados;
            var a = business.DefineForumPreRecursosForumFechado(q);

            Assert.AreEqual(true, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPreRecursosForumFechado_TestaProvaBloqueadaRecursosBloqueia_ForumPreBloqueado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questaoId = 1;
            var matricula = 0;
            var imagemId = 7;

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetQuestaoConcursoRecurso(questaoId, matricula).Returns(questao);
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.ObterComentariosForumConcursoPre(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterComentariosForumConcursoPos(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterImagensComentarioRecurso(questaoId).Returns(new List<tblConcursoQuestoes_recursosComentario_Imagens>());
            mock.AlunoSelecionouAlternativaQuestaoProva(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
            mockImage.GetImagensQuestaoConcurso(questaoId).Returns(new List<int> { imagemId });

            var q = business.GetQuestaoConcursoRecurso(questaoId, matricula);
            q.Prova.StatusProva = (int)ProvaRecurso.StatusProva.BloqueadoParaNovosRecursos;
            var a = business.DefineForumPreRecursosForumFechado(q);

            Assert.AreEqual(true, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPreRecursosForumFechado_TestaForumAberto_ForumPreAberto()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questaoId = 1;
            var matricula = 0;
            var imagemId = 7;

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var alternativas = QuestaoRecursoEntityTestData.ObterAlternativasGabaritoOficialLetraC();
            mock.GetQuestaoConcursoRecurso(questaoId, matricula).Returns(questao);
            mock.ObterAlternativasComEstatisticaFavorita(questaoId).Returns(alternativas);
            mock.ObterComentariosForumConcursoPre(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterComentariosForumConcursoPos(questaoId, matricula).Returns(new List<ForumComentarioDTO>());
            mock.ObterImagensComentarioRecurso(questaoId).Returns(new List<tblConcursoQuestoes_recursosComentario_Imagens>());
            mock.AlunoSelecionouAlternativaQuestaoProva(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
            mockImage.GetImagensQuestaoConcurso(questaoId).Returns(new List<int> { imagemId });

            var q = business.GetQuestaoConcursoRecurso(questaoId, matricula);
            q.ForumRecurso.ExisteAnaliseProfessor = false;
            var a = business.DefineForumPreRecursosForumFechado(q);

            Assert.AreEqual(false, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPosRecursosForumFechado_TestaForumAberto_ForumPosAberto()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);
            
            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            var a = business.DefineForumPosRecursosForumFechado(questao);
            Assert.AreEqual(false, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPosRecursosForumFechado_TestaForumFechadoNaoExisteAnaliseProf_ForumPosFechado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            questao.ForumRecurso.ExisteAnaliseProfessor = false;
            var a = business.DefineForumPosRecursosForumFechado(questao);
            Assert.AreEqual(true, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPosRecursosForumFechado_TestaForumFechadoExisteVereditoBanca_ForumPosFechado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            questao.ForumRecurso.BancaCabeRecurso = true;
            var a = business.DefineForumPosRecursosForumFechado(questao);
            Assert.AreEqual(true, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DefineForumPosRecursosForumFechado_TestaForumFechadoComentarioSelaForum_ForumPosFechado()
        {
            var mock = Substitute.For<IQuestaoData>();
            var mockImage = Substitute.For<IImagemData>();
            var business = new QuestaoBusiness(mock, mockImage, null, null);

            var questao = QuestaoRecursoEntityTestData.ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso();
            questao.ForumRecurso.ForumPosAnalise.Comentarios = new List<ForumComentarioDTO>
            {
                new ForumComentarioDTO { Texto = "Este é um comentário final do coordenador", EncerraForum = true, Professor = true }
            };
            var a = business.DefineForumPosRecursosForumFechado(questao);
            Assert.AreEqual(true, a);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DesabilitaAcertosQuestaoConcurso_RetornaSucessoSeDesabilitou_True()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);
            mock.DesabilitaAcertosQuestaoConcurso(1, 2, 3).Returns(1);
            var result = business.DesabilitaAcertosQuestaoConcurso(1, 2, 3);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Forum Recurso")]
        public void DesabilitaAcertosQuestaoConcurso_RetornaFalhaSeNaoDesabilitou_False()
        {
            var mock = Substitute.For<IQuestaoData>();
            var business = new QuestaoBusiness(mock, null, null, null);
            mock.DesabilitaAcertosQuestaoConcurso(1, 2, 3).Returns(0);
            var result = business.DesabilitaAcertosQuestaoConcurso(1, 2, 3);
            Assert.IsFalse(result);
        }
    }
}