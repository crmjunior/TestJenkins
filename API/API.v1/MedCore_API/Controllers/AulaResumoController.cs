using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_API.ViewModel.Base;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class AulaResumoController : BaseService
    {
        public AulaResumoController(IMapper mapper) : base(mapper)
        { }

        [HttpGet("Aulas/IdProduto/{idProduto}/IdTema/{idTema}/IdApostila/{idApostila}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public TemaApostila GetVideoAulas(string idProduto, string idTema, string idApostila, string matricula, string idAplicacao)
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);

            if(String.IsNullOrEmpty(appVersion))
                Request.Query.TryGetValue("appVersion", out appVersion);

            if(String.IsNullOrEmpty(appVersion))
            {
                appVersion = Convert.ToInt32(idAplicacao) == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                    ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsProDesktop") :
                    ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsPro");
            }

            return new MednetEntity().GetVideoAulas(Convert.ToInt32(idProduto), Convert.ToInt32(idTema), Convert.ToInt32(idApostila), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), appVersion);
        }

        [HttpGet("Resumo/Avaliacao/IdTema/{IdTema}/")]
        public AvaliacoesProfessor GetAvaliacoesTemasVideosResumo(string IdTema, string professorId)
        {
            return new MednetEntity().GetAvaliacoesTemasVideosResumo(Convert.ToInt32(IdTema), Convert.ToInt32(professorId));
        }

        [MapToApiVersion("2")]
        [HttpGet("Aula/Avaliacao/")]
        public ResultViewModel<List<AulaAvaliacaoViewModel>> GetAulaAvaliacaoSlides(string alunoid, string apostilaId)
        {
            var result = Execute(() =>
            {
                var business = new AulaAvaliacaoBusiness(new AulaAvaliacaoEntity(), new VideoEntity());
                var retorno = business.GetAvaliacaoAulaSlides (Convert.ToInt32(alunoid), Convert.ToInt32(apostilaId));
                return retorno;
            }, true);
            return GetResultViewModel<List<AulaAvaliacaoViewModel>, List<AulaAvaliacaoDTO>>(result);
        }

        [HttpPost("Resumo/Avaliacao/Insert/")]
        public int SetAvaliacoesTemaAulaResumo(AulaAvaliacaoPost avaliacao)
        {
            avaliacao.TipoVideo = (int)ETipoVideo.Resumo;
            avaliacao.IdAulaIndice = avaliacao.IdResumoIndice;
            return new MednetEntity().SetAvaliacaoTemaVideo(avaliacao);
        }

        [HttpGet("Resumo/Avaliacao/Permissao/Matricula/{matricula}/TemaId/{temaId}/")]
        public int GetPermissaoAvaliacaoTemaAulaResumo(string matricula, string temaId, string professorId)
        {
            return new MednetEntity().GetPermissaoAvaliacaoVideoAula(Convert.ToInt32(matricula), Convert.ToInt32(temaId), Convert.ToInt32(professorId), (int)ETipoVideo.Resumo);
        }

        [HttpPost("Resumo/Video/Progresso/Inserir/")]
        public int SetProgressoVideoResumo(ProgressoVideo Progresso)
        {
            return new MednetEntity().SetProgressoVideoResumo(Progresso);
        }

        [HttpGet("Resumo/Avaliacao/Aluno/Matricula/{matricula}/IdResumoAulaIndice/{IdResumoAulaIndice}/IdAplicacao/{IdAplicacao}/")]
        public AvaliacaoProfessor GetAvaliacaoRealizadaResumo(string matricula, string idResumoAula, string idAplicacao)
        {
            return new MednetEntity().GetAvaliacaoRealizadaResumo(Convert.ToInt32(matricula), Convert.ToInt32(idResumoAula), Convert.ToInt32(idAplicacao));
        }

        [HttpPost("Aulas/LogVideo/")]
        public int SetLogVideo(VideoMednet logVideo)
        {
            return new MednetEntity().SetLogVideoAssitido(logVideo);
        }
    }
}