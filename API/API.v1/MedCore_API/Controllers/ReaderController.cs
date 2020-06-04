using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Business;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.DTO;
using Microsoft.Extensions.Primitives;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Enums;
using Microsoft.AspNetCore.Cors;

namespace MedCoreAPI.Controllers
{
    [ApiController]
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [EnableCors]
    public class ReaderController : BaseService
    {
        public ReaderController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpGet]
        [Route("Materiais/Ano/{ano}/IdProduto/{idProduto}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<Semana> GetMateriaisSemanas(string ano, string idProduto, string matricula, string idAplicacao)
        {
            return new AulaEntity().GetSemanas(Convert.ToInt32(ano), Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Semana.TipoAba.Materiais);
        }

        [HttpGet]
        [Route("Materiais/Ano/{ano}/Matricula/{matricula}/Produto/{produto}/Progresso")]
        public List<ProgressoSemana> GetProgressoMateriais(string ano, string matricula, string produto)
        {
            return new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity()).GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Materiais);
        }

        [HttpGet]
        [Route("Materiais/Ano/{ano}/Matricula/{matricula}/Produto/{produto}/Permissao")]
        public List<Semana> GetPermissaoMaterialSemanas(string ano, string matricula, string produto)
        {
            return new AulaBusiness(new AulaEntity()).GetPermissaoSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto));
        }

        [EnableCors]
        [HttpGet]
        [Route("Materiais/Apostila/IdEntidade/{IdEntidade}/Matricula/{matricula}/Versao/{IdVersao}")]
        public MaterialApostilaAluno GetMateriaisApostila(string IdEntidade, string matricula, string IdVersao)
        {
            return new MaterialApostilaEntity().GetApostilaAtiva(Convert.ToInt32(IdEntidade), Convert.ToInt32(matricula), Convert.ToInt32(IdVersao));
        }

        [HttpGet]
        [Route("Materiais/ApostilaOriginal/IdEntidade/{IdEntidade}")]
        public ResultViewModel<MaterialApostilaConteudoViewModel> GetApostilaOriginal(string IdEntidade)
        {
            var result = Execute(() =>
            {
                return new MaterialApostilaBusiness(new MaterialApostilaEntity()).GetApostilaOriginal(Convert.ToInt32(IdEntidade));
            });
            return GetResultViewModel<MaterialApostilaConteudoViewModel, MaterialApostilaDTO>(result);
            
        }

        [HttpPost]
        [Route("Materiais/Apostila/Inserir/")]
        public int PostModificacaoApostila(MaterialApostilaAluno apostila)
        {
            return new MaterialApostilaEntity().PostModificacaoApostila(apostila.ClientId, apostila.ApostilaId, apostila.Conteudo);
        }

        [HttpGet]
        [Route("Materiais/Apostila/Versoes/IdApostila/{apostilaId}/Matricula/{matricula}/")]
        public List<int> GetVersoesApostilas(string apostilaId, string matricula)
        {
            return new MaterialApostilaEntity().GetIdsVersoesApostila(Convert.ToInt32(apostilaId), Convert.ToInt32(matricula));
        }

        [HttpPost]
        [Route("Materiais/Apostila/Versoes/Limpar/")]
        public MaterialApostilaAluno LimpaVersoes(MaterialApostilaAluno apostila)
        {
            return new MaterialApostilaEntity().LimpaVersoes(apostila.EntidadeId, apostila.ClientId);
        }

        [HttpGet]
        [Route("Materiais/Apostila/Progresso/IdApostila/{idApostila}/Matricula/{matricula}/")]
        public decimal GetProgressoApostila(string apostilaId, string matricula)
        {
            return new MaterialApostilaEntity().GetProgresso(Convert.ToInt32(apostilaId), Convert.ToInt32(matricula));
        }

        [HttpPost]
        [Route("Materiais/Apostila/Progresso/")]
        public int PostProgressoApostila(MaterialApostilaProgresso progresso)
        {
            return new MaterialApostilaEntity().PostProgresso(progresso);
        }

        [HttpGet]
        [Route("Materiais/Apostila/Interacoes/IdApostila/{idApostila}/Matricula/{matricula}")]
        public List<MaterialApostilaInteracao> GetInteracoesAlunoApostila(string idApostila, string matricula)
        {
            return new MaterialApostilaEntity().GetInteracoesAluno(Convert.ToInt32(idApostila), Convert.ToInt32(matricula));
        }

        [HttpPost]
        [Route("Materiais/Apostila/Interacoes/")]
        public int PostInteracoesApostila(MaterialApostilaInteracoes interacoes)
        {
            return new MaterialApostilaEntity().PostInteracoes(interacoes);
        }

        [HttpGet]
        [Route("Materiais/Apostila/Entidadeid/{idApostila}")]
        public long? GetEntidadeId(string idApostila)
        {
            return new MaterialApostilaEntity().GetEntidadeId(Convert.ToInt32(idApostila));
        }

        [HttpGet]
        [Route("Materiais/Apostila/Addons/IdApostila/{idApostila}")]
        public List<AddOnApostila> GetAddonApostila(string idApostila)
        {
            return new MaterialApostilaEntity().GetAddonApostila(Convert.ToInt32(idApostila));
        }  

        [HttpGet]
        [Route("Cronograma/Produto/{produtoId}/Ano/{ano}/Menu/{id}/matricula={matricula}")]
        public CronogramaSemana GetCronograma(string produtoId, string ano, string id, string matricula)
        {
            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            return business.GetCronogramaAluno(Convert.ToInt32(produtoId), Convert.ToInt32(ano), Convert.ToInt32(id), Convert.ToInt32(matricula));
        }      

        [HttpPost]
        [Route("Materiais/Apostila/AvaliacaoVideo/Inserir")]
        public int SetAvaliacaoVideo(AvaliacaoVideoApostila avaliacao)
        {
            return new VideoEntity().SetAvaliacaoVideo(avaliacao);
        } 

        [HttpPost]
        [Route("Materiais/Apostilas/LogPrint/Inserir")]
        public int SetLogPrintApostila(LogPrintApostila registro)
        {
            return new MaterialApostilaBusiness(new MaterialApostilaEntity()).RegistraPrintApostila(registro);
        }

        [HttpGet]
        [Route("Materiais/Apostila/VideosApostilaCodigo/{codigo}/")]
        public GetVideosApostilaCodigoJsonDTO GetVideosApostilaCodigoJson(string codigo, string matricula, string idaplicacao, string appversion)
        {
            var result = new GetVideosApostilaCodigoJsonDTO();
            result.GetVideosApostilaCodigoJsonResult = new VideoBusiness(new VideoEntity()).GetVideoApostila(codigo, matricula, (Aplicacoes)Convert.ToInt32(idaplicacao), appversion);
            return result;
        }

        [HttpPost]
        [MapToApiVersion("2")]
        [Route("Materiais/Apostilas/Videos/Listar")]
        public ResultViewModel<IList<VideoViewModel>> VideosApostila(VideoApostilaFiltroDTO filtro)
        {
            var result = Execute(() =>
            {
                var videos = new VideoBusiness(new VideoEntity()).ObterVideosApostila(filtro);               
                return videos;
            }, true);
            return GetResultViewModel<IList<VideoViewModel>, IList<VideoDTO>>(result);                
        }
    }
}