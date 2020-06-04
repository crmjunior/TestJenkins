using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO.Base;
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
    public class RevalidaController : BaseService
    {
        public RevalidaController(IMapper mapper) : base(mapper) 
        { }

        [HttpGet("Revalida/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<EspecialRevalida> GetEspecialRevalida(string ano, string matricula, string idAplicacao)
        {
            return new RevalidaEntity().GetEspecialRevalida(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }

        [HttpGet("Revalida/Acesso/Matricula/{matricula}")]
        public ResponseDTO<List<int>> ObterTemasRevalidaPermitidos(string matriculaId)
        {
            return new RevalidaBusiness(new MednetEntity()).ObterTemasRevalidaPermitidos(Convert.ToInt32(matriculaId));
        }

        [HttpGet("Revalida/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Tema/{idTema}/")]
        public TemaApostila GetTemaRevalida(string ano, string matricula, string idAplicacao, string idTema)
        {
            return new RevalidaEntity().GetTemaRevalida(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), Convert.ToInt32(idTema));
        }

        //[HttpGet("json/Revalida/Avaliacao/IdTema/{IdTema}/?professorId={professorId}")]
        [HttpGet("Revalida/Avaliacao/IdTema/{IdTema}/")]
        public AvaliacoesProfessor GetAvaliacoesTemasVideosRevalida(string IdTema, string professorId)
        {
            return new MednetEntity().GetAvaliacoesTemasVideosRevalida(Convert.ToInt32(IdTema), Convert.ToInt32(professorId));
        }

        [HttpPost("Revalida/Avaliacao/Insert/")]
        public int SetAvaliacaoVideoAulaRevalida(AulaAvaliacaoPost avaliacao)
        {
            avaliacao.TipoVideo = (int)ETipoVideo.Revalida;
            return new MednetEntity().SetAvaliacaoTemaVideo(avaliacao);
        }

        [HttpGet("Revalida/Avaliacao/Permissao/Matricula/{matricula}/TemaId/{temaId}/ProfessorId/{professorId}")]
        public int GetPermissaoAvaliacaoTemaAulaRevalida(string matricula, string temaId, string professorId)
        {
            return new MednetEntity().GetPermissaoAvaliacaoVideoAulaRevalida(Convert.ToInt32(matricula), Convert.ToInt32(temaId), Convert.ToInt32(professorId), (int)ETipoVideo.Revalida);
        }

        [HttpPost("Revalida/Video/Progresso/Inserir/")]
        public int SetProgressoVideoRevalida(ProgressoVideo Progresso)
        {
            return new MednetEntity().SetProgressoVideoRevalida(Progresso);
        }

        [HttpGet("Revalida/Avaliacao/Aluno/Matricula/{matricula}/IdRevalidaAulaIndice/{IdRevalidaAulaIndice}/IdAplicacao/{IdAplicacao}/")]
        public AvaliacaoProfessor GetAvaliacaoRealizadaRevalida(string matricula, string idRevalidaAula, string idAplicacao)
        {
            return new MednetEntity().GetAvaliacaoRealizadaRevalida(Convert.ToInt32(matricula), Convert.ToInt32(idRevalidaAula), Convert.ToInt32(idAplicacao));
        }

    }
}