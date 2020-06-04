using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MedCore_API.ViewModel.Base;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.ViewModels;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class RevisaoController : BaseService
    {
        public RevisaoController(IMapper mapper) : base(mapper)
        { }

        [HttpGet("Revisao/Avaliacao/Aluno/Matricula/{matricula}/IdRevisaoAulaIndice/{idRevisaoAulaIndice}/IdAplicacao/{idAplicacao}/")]
        public AvaliacaoProfessor GetAvaliacaoRealizada(string matricula, string idRevisaoAulaIndice, string idAplicacao)
        {
            return new MednetEntity().GetAvaliacaoRealizada(Convert.ToInt32(matricula), Convert.ToInt32(idRevisaoAulaIndice), Convert.ToInt32(idAplicacao));
        }

        [HttpGet("Revisao/Aula/IdProduto/{idProduto}/IdTema/{idTema}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public TemaApostila GetVideos(string idProduto, string matricula, string idTema, string idAplicacao)
        {
            return new MednetEntity().GetVideos(Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Convert.ToInt32(idTema), Convert.ToInt32(idAplicacao));
        }

        [HttpGet("Revisao/Avaliacao/IdTema/{IdTema}")]
        public AvaliacoesProfessor GetAvaliacoesTemasVideosRevisao(string IdTema, string professorId = "0")
        {
            return new MednetEntity().GetAvaliacoesTemasVideosRevisao(Convert.ToInt32(IdTema), Convert.ToInt32(professorId));
        }

        [HttpPost("Revisao/Avaliacao/Insert/")]
        public int SetAvaliacoesTemaAulaRevisao(AulaAvaliacaoPost avaliacao)
        {
            return new MednetEntity().SetAvaliacaoTemaVideoRevisao(avaliacao);
        }

        [HttpGet("Revisao/Avaliacao/Permissao/Matricula/{matricula}/TemaId/{temaId}/")]
        public int GetPermissaoAvaliacaoTemaAulaRevisao(string matricula, string temaId, string professorId = "0")
        {
            return new MednetEntity().GetPermissaoAvaliacao(Convert.ToInt32(matricula), Convert.ToInt32(temaId), Convert.ToInt32(professorId));
        }

        [HttpPost("Revisao/Log/Inserir")]
        public int SetLogRevisaoVideo(VideoMednet video)
        {
            return new MednetEntity().SetLogAssitido(video);
        }

        [HttpGet("Revisao/Video/Url/Id/{idVideo}/")]
        public string GetVideoRevisao(string idVideo)
        {
            int idAplicacao = (int)Aplicacoes.MsProMobile;

            if (this.Request.Query.TryGetValue("idAplicacao", out StringValues idAplicacaoString))
            {
                idAplicacao = Convert.ToInt32(idAplicacaoString);
            }

            if (!this.Request.Headers.TryGetValue("appversion", out StringValues appVersion))
            {
                if(!this.Request.Query.TryGetValue("appversion", out appVersion))
                {
                    appVersion = idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                        ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsProDesktop") :
                        ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsPro");
                }
            }

            return new MednetEntity().GetVideo(Convert.ToInt32(idVideo), idAplicacao, appVersion);
        }

        [MapToApiVersion("2")]        
        [HttpGet("Revisao/Video/Url/{idVideo}/")]
        public ResultViewModel<VideoUrlViewModel> GetVideoAulaRevisao(string idVideo)
        {
            var result = Execute(() =>
            {
                var business = new MednetBusiness(new MednetEntity());
                var video = business.GetVideoUrlComUltimaPosicao(Convert.ToInt32(idVideo), IdAplicacao, VersaoAplicacao, Matricula);
                return video;
            }, true);
            return GetResultViewModel<VideoUrlViewModel, VideoUrlDTO>(result);
        }

        [HttpGet("Revisao/Aviso/IdAplicacao/{applicationID}/")]
        public Aviso GetAviso(string applicationID)
        {
            return new Aviso { Mensagem = MensagemEntity.GetAviso(1, Convert.ToInt32(applicationID)) };
        }

        [HttpGet("Revisao/Aviso/AvisoVisualizado/Matricula/{matricula}/")]
        public int IsAvisoVisualizado(string matricula)
        {
            return Convert.ToInt32(MensagemEntity.IsAvisoVisto(Convert.ToInt32(matricula), 1));
        }

        [HttpPost("Revisao/Aviso/Inserir/")]
        public int SetLogAviso(Aviso aviso)
        {
            return Convert.ToInt32(MensagemEntity.SetLogAviso(aviso.Matricula, 1, aviso.ConfirmaVisualizacao));
        }

        [HttpPost("Revisao/Video/Progresso/Inserir/")]
        public int SetProgressoVideo(ProgressoVideo Progresso)
        {
            MednetBusiness mednetBusiness = new MednetBusiness(new MednetEntity());
            return mednetBusiness.SetProgressoVideoAsync(Progresso);
        }
    }
}