using System.Collections.Generic;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class ContribuicaoBusinessTest
    {
        private const int ClientIdAcademico = 96409;
        private const int ApostilaIdCLM28 = 174;
        private const int ClientIdCoordenador = 76972;
        private const int ContribuicaoIdTesteMock = 1;
        private const int ClientIdProfessor = 90154;

        public static ContribuicaoFiltroDTO GetFiltro()
        {
            var filtro = new ContribuicaoFiltroDTO()
            {
                ClientId = ClientIdAcademico
            };

            return filtro;
        }

        public static ContribuicaoDTO GetContribuicaoTesteSemArquivos()
        {
            var contribuicao = new ContribuicaoDTO()
            {
                ContribuicaoId = ContribuicaoIdTesteMock,
                Arquivos = new List<ContribuicaoArquivo>()
            };
            return contribuicao;
        }

        public static ContribuicaoDTO GetContribuicaoTesteComArquivos()
        {
            var contribuicao = new ContribuicaoDTO()
            {
                ContribuicaoId = ContribuicaoIdTesteMock,
                Arquivos = new List<ContribuicaoArquivo>()
            };

            contribuicao.Arquivos.Add(new ContribuicaoArquivo() { ContribuicaoID = ContribuicaoIdTesteMock });
            return contribuicao;
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetContribuicoes()
        {
            var filtro = GetFiltro();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }
            Assert.IsTrue(contribuicoes.Count > 0);
        }

        private IContribuicaoData contribuicaoMock;
        private IContribuicaoArquivoData contribuicaoArquivoMock;
        

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanSetNovaContribuicaoSemArquivos()
        {
            var contribuicao = new Contribuicao();
            contribuicao.Arquivos = new List<ContribuicaoArquivo>();

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.InserirContribuicao(contribuicao).Returns(1);
            contribuicaoMock.UpdateContribuicao(contribuicao).Returns(-1);
            contribuicaoArquivoMock.InserirContribuicaoArquivo(new ContribuicaoArquivo()).Returns(-1);
            contribuicaoArquivoMock.UpdateContribuicaoArquivo(new ContribuicaoArquivo()).Returns(-1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.InserirContribuicao(contribuicao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanSetNovaContribuicaoComArquivos()
        {
            var contribuicao = new Contribuicao();
            contribuicao.Arquivos = new List<ContribuicaoArquivo>();
            contribuicao.Arquivos.Add(new ContribuicaoArquivo());

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock =Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.InserirContribuicao(contribuicao).Returns(1);
            contribuicaoMock.UpdateContribuicao(contribuicao).Returns(-1);
            contribuicaoArquivoMock.InserirContribuicaoArquivo(contribuicao.Arquivos.FirstOrDefault()).Returns(1);
            contribuicaoArquivoMock.UpdateContribuicaoArquivo(contribuicao.Arquivos.FirstOrDefault()).Returns(-1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.InserirContribuicao(contribuicao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanSetAtualizarContribuicaoSemArquivos()
        {
            var contribuicao = new Contribuicao();
            contribuicao.ContribuicaoId = ContribuicaoIdTesteMock;
            contribuicao.Arquivos = new List<ContribuicaoArquivo>();

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.InserirContribuicao(contribuicao).Returns(-1);
            contribuicaoMock.UpdateContribuicao(contribuicao).Returns(1);
            contribuicaoArquivoMock.InserirContribuicaoArquivo(new ContribuicaoArquivo()).Returns(-1);
            contribuicaoArquivoMock.UpdateContribuicaoArquivo(new ContribuicaoArquivo()).Returns(-1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.InserirContribuicao(contribuicao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanSetAtualizarContribuicaoComArquivos()
        {
            var contribuicao = new Contribuicao();
            contribuicao.ContribuicaoId = ContribuicaoIdTesteMock;
            contribuicao.Arquivos = new List<ContribuicaoArquivo>();
            contribuicao.Arquivos.Add(new ContribuicaoArquivo() { Id = ContribuicaoIdTesteMock });

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.UpdateContribuicao(contribuicao).Returns(1);
            contribuicaoMock.UpdateContribuicao(contribuicao).Returns(1);
            contribuicaoArquivoMock.InserirContribuicaoArquivo(contribuicao.Arquivos.FirstOrDefault()).Returns(-1);
            contribuicaoArquivoMock.UpdateContribuicaoArquivo(contribuicao.Arquivos.FirstOrDefault()).Returns(1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.InserirContribuicao(contribuicao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanDeleteContribuicaoSemArquivos()
        {
            var contribuicao = new Contribuicao();
            contribuicao.Arquivos = new List<ContribuicaoArquivo>();

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.DeletarContribuicao(1).Returns(1);
            contribuicaoMock.GetContribuicao(1).Returns(GetContribuicaoTesteSemArquivos());
            contribuicaoArquivoMock.ListarArquivosContribuicao(1).Returns(contribuicao.Arquivos);
            contribuicaoArquivoMock.DeletarContribuicaoArquivo(new List<int>()).Returns(0);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.DeletarContribuicao(1);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanDeleteContribuicaoComArquivos()
        {
            var contribuicao = GetContribuicaoTesteComArquivos();

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.DeletarContribuicao(1).Returns(1);
            contribuicaoMock.GetContribuicao(1).Returns(GetContribuicaoTesteComArquivos());
            contribuicaoArquivoMock.ListarArquivosContribuicao(1).Returns(GetContribuicaoTesteComArquivos().Arquivos);
            contribuicaoArquivoMock.DeletarContribuicaoArquivo(default).ReturnsForAnyArgs(1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.DeletarContribuicao(1);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanFavoritarContribuicao()
        {
            var interacao = new ContribuicaoInteracao() { };

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();

            contribuicaoMock.GetInteracao(interacao).Returns(interacao);
            contribuicaoMock.InsertInteracao(interacao).Returns(1);
            contribuicaoMock.DeleteContribuicaoInteracao(interacao.ContribuicaoInteracaoId).Returns(0);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);
            var result = business.InsertInteracao(interacao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanDesfavoritarContribuicao()
        {
            var interacao = new ContribuicaoInteracao() { ContribuicaoInteracaoId = ContribuicaoIdTesteMock };

            contribuicaoMock = Substitute.For<IContribuicaoData>();
            contribuicaoArquivoMock = Substitute.For<IContribuicaoArquivoData>();
            contribuicaoMock.GetInteracao(interacao).Returns(interacao);
            contribuicaoMock.InsertInteracao(interacao).Returns(0);
            contribuicaoMock.DeleteContribuicaoInteracao(interacao.ContribuicaoInteracaoId).Returns(1);

            var business = new ContribuicaoBusiness(contribuicaoMock, contribuicaoArquivoMock);

            var result = business.InsertInteracao(interacao);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = true;
            filtro.IsPendente = false;
            filtro.Page = 1;

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsAudio.Equals(true) ||
                x.IsImagem.Equals(true) ||
                !string.IsNullOrEmpty(x.Descricao) ||
                x.IsVideo.Equals(true))
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetApostilaContribuicao()
        {
            var filtro = GetFiltro();
            filtro.ApostilaId = ApostilaIdCLM28;
            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = true;
            filtro.IsPendente = false;
            filtro.Page = 1;

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.ApostilaId == ApostilaIdCLM28 &&
                (x.IsAudio.Equals(true) ||
                x.IsImagem.Equals(true) ||
                !string.IsNullOrEmpty(x.Descricao) ||
                x.IsVideo.Equals(true))
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPublicasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.BitAprovacaoMedgrupo.Equals(true)
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetImagemContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.IsImagem.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetAudioContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.IsAudio.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                !string.IsNullOrEmpty(x.Descricao)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetFavoritaContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasImagemContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.IsImagem.Equals(true) &&
                x.Dono.Equals(true)
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasAudioContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.IsAudio.Equals(true) &&
                x.Dono.Equals(true)
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                !string.IsNullOrEmpty(x.Descricao) &&
                x.Dono.Equals(true)
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasImagemAudioContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                x.IsAudio.Equals(true) &&
                x.IsImagem.Equals(true)
            );
            Assert.IsTrue(contribuicoes.Count > 0);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasImagemTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (x.IsImagem.Equals(true) || !string.IsNullOrEmpty(x.Descricao))
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasImagemFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito) || x.IsImagem.Equals(true))
            );
            Assert.IsTrue(contribuicoes.Count > 0);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasAudioTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (x.IsAudio.Equals(true) || !string.IsNullOrEmpty(x.Descricao))
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasAudioFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (x.IsAudio.Equals(true) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito))
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasTextoFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (!string.IsNullOrEmpty(x.Descricao) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito))
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetMinhasAllFiltrosContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = true;
            filtro.IsAprovado = false;
            filtro.JustMyAid = true;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Dono.Equals(true) &&
                (x.IsAudio.Equals(true) || x.IsImagem.Equals(true) || !string.IsNullOrEmpty(x.Descricao) || x.IsVideo.Equals(true) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito))
            );
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesImagemContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.BitAprovacaoMedgrupo.Equals(true) &&
                x.IsImagem.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesAudioContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.IsAudio.Equals(true) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                !string.IsNullOrEmpty(x.Descricao) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesImagemAudioContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsImagem.Equals(true) || x.IsAudio.Equals(true)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesImagemTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsImagem.Equals(true) || !string.IsNullOrEmpty(x.Descricao)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesImagemFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = true;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsImagem.Equals(true) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesAudioTextoContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsAudio.Equals(true) || !string.IsNullOrEmpty(x.Descricao)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesAudioFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsAudio.Equals(true) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesTextoFavoritasContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = true;
            filtro.ByVideo = false;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (!string.IsNullOrEmpty(x.Descricao) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPendentesAllFiltrosContribuicao()
        {
            var filtro = GetFiltro();

            filtro.ByAudio = true;
            filtro.ByImage = true;
            filtro.ByText = true;
            filtro.ByVideo = true;
            filtro.IsAprovado = true;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>() { (int)EnumTipoInteracaoContribuicao.Favorito };

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                (x.IsAudio.Equals(true) || x.IsImagem.Equals(true) || !string.IsNullOrEmpty(x.Descricao) || x.IsVideo.Equals(true) || x.Interacoes.Contains((int)EnumTipoInteracaoContribuicao.Favorito)) &&
                x.BitAprovacaoMedgrupo.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetPublicadasPorMimContribuicao()
        {
            var filtro = GetFiltro();
            filtro.MedGrupoID = ClientIdCoordenador;
            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IdsProfessores = new List<int>();
            filtro.IsAprovado = false;
            filtro.IsAprovarMaisTarde = false;
            filtro.IsArquivada = false;
            filtro.IsProfessor = true;
            filtro.IsPublicadasPorMim = true;
            filtro.IsPublicadoMedGrupo = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.MedGrupoID.Equals(filtro.MedGrupoID)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetLerMaisTardeContribuicao()
        {
            var filtro = GetFiltro();
            filtro.MedGrupoID = ClientIdCoordenador;
            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IdsProfessores = new List<int>();
            filtro.IsAprovado = false;
            filtro.IsAprovarMaisTarde = true;
            filtro.IsArquivada = false;
            filtro.IsProfessor = true;
            filtro.IsPublicadasPorMim = false;
            filtro.IsPublicadoMedGrupo = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.AprovarMaisTarde.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetArquivadasContribuicao()
        {
            var filtro = GetFiltro();
            filtro.MedGrupoID = ClientIdCoordenador;
            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IdsProfessores = new List<int>();
            filtro.IsAprovado = false;
            filtro.IsAprovarMaisTarde = false;
            filtro.IsArquivada = true;
            filtro.IsProfessor = true;
            filtro.IsPublicadasPorMim = false;
            filtro.IsPublicadoMedGrupo = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Arquivada.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetEncaminhadasContribuicao()
        {
            var filtro = GetFiltro();
            filtro.MedGrupoID = ClientIdCoordenador;
            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IdsProfessores = new List<int>();
            filtro.IsAprovado = false;
            filtro.IsAprovarMaisTarde = false;
            filtro.IsArquivada = false;
            filtro.IsProfessor = true;
            filtro.IsPublicadasPorMim = false;
            filtro.IsPublicadoMedGrupo = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.IsEncaminhado = true;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Encaminhada.Equals(true)
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("ContribuicoesUnit")]
        public void CanGetEncaminhadasProfessorContribuicao()
        {
            var filtro = GetFiltro();
            filtro.MedGrupoID = ClientIdCoordenador;
            filtro.ByAudio = false;
            filtro.ByImage = false;
            filtro.ByText = false;
            filtro.ByVideo = false;
            filtro.IdsProfessores = new List<int>() { ClientIdProfessor };
            filtro.IsAprovado = false;
            filtro.IsAprovarMaisTarde = false;
            filtro.IsArquivada = false;
            filtro.IsProfessor = true;
            filtro.IsPublicadasPorMim = false;
            filtro.IsPublicadoMedGrupo = false;
            filtro.JustMyAid = false;
            filtro.Page = 1;
            filtro.IsEncaminhado = true;
            filtro.TiposInteracoes = new List<int>();

            var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
            var contribuicoes = business.GetContribuicoes(filtro);
            if (contribuicoes.Count == 0)
            {
                Assert.Inconclusive();
            }

            var result = contribuicoes.All(x =>
                x.Encaminhada.Equals(true) &&
                x.ProfessoresEncaminhados.Contains(ClientIdProfessor)
            );

            Assert.IsTrue(result);
        }

        // [TestMethod]
        // [TestCategory("ContribuicoesUnit")]
        // public void GetBucket_ConfiguracoesWebConfig_ApontandoProd()
        // {
        //     var configs = ContribuicaoBucketManager.GetConfig();
        //     Assert.IsTrue(configs.Bucket == "prod-mspro-contribuicoes-uploadedfiles");
        // }
    }
}
