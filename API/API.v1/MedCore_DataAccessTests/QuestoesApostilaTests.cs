using System.Security.Cryptography;
using System;
using System.Linq;
using AutoMapper;
using MedCore_API.Controllers;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.ViewModels;
using MedCoreAPI.ViewModel;
using MedCoreAPI.ViewModel.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class QuestoesApostilaTestes
    {
        const string GetApostilaImagem_idImagem = "29";

        const string GetApostilaPermissaoAvaliacao_idQuestao = "210737";
        const string GetApostilaPermissaoAvaliacao_idTipoComentario = "2";
        const string GetApostilaPermissaoAvaliacao_matricula = "292113";
        
        
        const string GetApostilaEstatisticaQuestao_idQuestao = "6377";
        const string GetApostilaEstatisticaQuestao_idAplicacao = "3";
        
        
        const string GetApostilaVideoQuestao_idQuestao = "6385";
        const string GetApostilaVideoQuestao_idAplicacao = "25";
        const string GetApostilaVideoQuestao_appVersion = "25";

        const string GetApostilaQuestao_idQuestao = "6385";
        const string GetApostilaQuestao_idAplicacao = "25";

        const string GetApostilaConfiguracao_idEntidade = "11671";
        const string GetApostilaConfiguracao_Matricula = "211982";
        const string GetApostilaConfiguracao_idAplicacao = "17";

        Random random = new Random();

        MapperConfiguration config;
        IMapper mapper;

        public QuestoesApostilaTestes()
        {
            config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Contribuicao, ContribuicaoDTO>();
                cfg.CreateMap<ContribuicaoDTO, ContribuicaoViewModel>();
                cfg.CreateMap<DuvidasAcademicasProfessorViewModel, DuvidasAcademicasProfessorDTO>();
                cfg.CreateMap<DuvidasAcademicasProfessorDTO, DuvidasAcademicasProfessorViewModel>();
                cfg.CreateMap<VideoDTO, VideoViewModel>();
                cfg.CreateMap<VideoQualidadeDTO, VideoQualidadeViewModel>();
                cfg.CreateMap<ContribuicaoBucketDTO, ContribuicaoBucketViewModel>();
                cfg.CreateMap<MaterialApostilaDTO, MaterialApostilaConteudoViewModel>();
            });
            
            mapper = config.CreateMapper();
        }

        [TestMethod]
        public void TestGetApostilaImagem()
        {
            var controller = new QuestoesApostilaController(mapper);
            var result = controller.GetApostilaImagem(GetApostilaImagem_idImagem);
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void TestGetApostilaPermissaoAvaliacao()
        {
            var controller = new QuestoesApostilaController(mapper);
            
            var result = controller.GetApostilaPermissaoAvaliacao(GetApostilaPermissaoAvaliacao_idQuestao, GetApostilaPermissaoAvaliacao_idTipoComentario, GetApostilaPermissaoAvaliacao_matricula);
            Assert.IsTrue(result == 0);

            result = controller.GetApostilaPermissaoAvaliacao("-1", "-1", "-1");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void TestGetApostilaEstatisticaQuestao()
        {
            var controller = new QuestoesApostilaController(mapper);
            var result = controller.GetApostilaEstatisticaQuestao(GetApostilaEstatisticaQuestao_idQuestao, "0", GetApostilaEstatisticaQuestao_idAplicacao);

            Assert.IsFalse(result == null);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TestGetApostilaVideoQuestao()
        {
            var controller = new QuestoesApostilaController(mapper);
            var result = controller.GetApostilaVideoQuestao(GetApostilaVideoQuestao_idQuestao, "0", GetApostilaVideoQuestao_idAplicacao, GetApostilaVideoQuestao_appVersion);
            Assert.IsFalse(result == null);
        }

        [TestMethod]
        public void TestGetApostilaQuestao()
        {
            var controller = new QuestoesApostilaController(mapper);
            var result = controller.GetApostilaQuestao(GetApostilaQuestao_idQuestao, "0", GetApostilaQuestao_idAplicacao);
            Assert.IsFalse(result == null);
        }

        [TestMethod]
        public void TestGetApostilaConfiguracao()
        {
            var controller = new QuestoesApostilaController(mapper);
            var result = controller.GetApostilaConfiguracao(GetApostilaConfiguracao_idEntidade, GetApostilaConfiguracao_Matricula, GetApostilaConfiguracao_idAplicacao);
            Assert.IsFalse(result == null);
        }
    }
}