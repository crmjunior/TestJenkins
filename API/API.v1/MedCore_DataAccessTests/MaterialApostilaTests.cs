
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
using static MedCore_DataAccess.Repository.CronogramaEntity;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccessTests
{
    public class MaterialApostilaTests
    {
        private int videoId = 42923;
        private int bookId = 17834;
        private string matricula = "96409";

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetApostilaAddOn()
        {
            List<AddOnApostila> lstApostilaAddOn = new MaterialApostilaEntity().GetAddonApostila(16298);
            Assert.IsNotNull(lstApostilaAddOn);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetApostilaAtiva()
        {
            var apostila = new MaterialApostilaEntity().GetApostilaAtiva(17715, 267308, 1);

            Assert.IsNotNull(apostila);
            Assert.IsTrue(apostila.Ativa);
        }

        [TestMethod]
        [TestCategory("Basico")]
        [Ignore]
        public void Can_PostModificacaoApostila()
        {
            var apostila = new MaterialApostilaEntity().PostModificacaoApostila(96408, 71, "<html><body></body></html>");

            Assert.IsTrue(apostila == 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void InsertLogApostila_Academico_1()
        {
            var logprint = new LogPrintApostila
            {
                CPF = "95411747872",
                Date = DateTime.Now,
                Pagina = (decimal)15.50,
                Apostila = 15605
            };

            var retorno = new MaterialApostilaEntity().RegistraPrintApostila(logprint);

            Assert.AreEqual(1, retorno);
        }      

        [TestMethod]
        [TestCategory("Basico")]
        public void InsertLogApostila_SemCpf_0()
        {
            var logprint = new LogPrintApostila
            {
                Date = DateTime.Now,
                Pagina = (decimal)15.80,
                Apostila = 15605
            };

            var retorno = new MaterialApostilaEntity().RegistraPrintApostila(logprint);

            Assert.AreEqual(0, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void InsertLogApostila_AcademicoSemData_0()
        {
            var logprint = new LogPrintApostila
            {
                CPF = "95411747872",
                Pagina = 15,
                Apostila = 15605
            };

            var retorno = new MaterialApostilaEntity().RegistraPrintApostila(logprint);

            Assert.AreEqual(0, retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void SetAvaliacaoVideoMiolo_UpVote_MudouAvaliacao()
        {
            var videoEntity = new VideoEntity();

            var avaliacao = new AvaliacaoVideoApostila()
            {
                Matricula = Convert.ToInt32(matricula),
                TipoVote = (int)ETipoVideoVote.Upvote,
                VideoID = videoId,
                MaterialID = bookId
            };

            var ret = videoEntity.SetAvaliacaoVideo(avaliacao);
            Assert.AreEqual(ret, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void SetAvaliacaoVideoMiolo_DownVote_MudouAvaliacao()
        {
            var videoEntity = new VideoEntity();

            var avaliacao = new AvaliacaoVideoApostila()
            {
                Matricula = Convert.ToInt32(matricula),
                TipoVote = (int)ETipoVideoVote.Downvote,
                VideoID = videoId,
                MaterialID = bookId
            };

            var ret = videoEntity.SetAvaliacaoVideo(avaliacao);
            Assert.AreEqual(ret, 1);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetApostila_AnoAtual_CssFatiado()
        {
            var versaoApostila = 0;
            var anoAtual = DateTime.Now.Year;
            var alunoEntity = new PerfilAlunoEntityTestData();
            var materialEntity = new MaterialApostilaEntity();
            
            var aluno = alunoEntity.GetAlunosMedAnoAtualAtivo().FirstOrDefault();
            if(aluno.ID == 0)
            {
                Assert.Inconclusive();
            }
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            var cronograma = bus.GetCronogramaAluno((int)Aplicacoes.MsProMobile, anoAtual, (int)ESubMenus.Materiais, aluno.ID);
            var semana = cronograma.Semanas.FirstOrDefault();

            Assert.IsTrue(semana.Apostilas.Count > 0);
            var entidade = semana.Apostilas.FirstOrDefault();
            var nomecss = materialEntity.GetNameCss(entidade.MaterialId, anoAtual);
            var result = materialEntity.GetApostilaAtiva(entidade.MaterialId, aluno.ID, versaoApostila);
            Assert.IsTrue(result.LinkCss.Contains(nomecss));
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetApostila_Ano2018_CssCompletoAntigo()
        {
            var versaoApostila = 0;
            var anoAtual = DateTime.Now.Year;
            var alunoEntity = new PerfilAlunoEntityTestData();
            var materialEntity = new MaterialApostilaEntity();
            var cronogramaEntity = new CronogramaEntity();

            var aluno = alunoEntity.GetAlunosMedAtivo(2018).FirstOrDefault();
            if (aluno.ID == 0)
            {
                Assert.Inconclusive();
            }

            var linkCssApostilasAntigas = materialEntity.GetAssetApostila(2018);
            var bus = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());

            var cronograma = bus.GetCronogramaAluno((int)Produto.Produtos.MEDCURSO, 2018, (int)ESubMenus.Materiais, aluno.ID);
            var semana = cronograma.Semanas.FirstOrDefault();

            Assert.IsTrue(semana.Apostilas.Count > 0);
            var entidade = semana.Apostilas.FirstOrDefault();
            var result = materialEntity.GetApostilaAtiva(entidade.MaterialId, aluno.ID, versaoApostila);
            Assert.IsTrue(result.LinkCss.Contains(linkCssApostilasAntigas));
        }        
    }
}