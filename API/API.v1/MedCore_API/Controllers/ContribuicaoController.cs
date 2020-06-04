using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MedCoreAPI.ViewModel;
using MedCoreAPI.ViewModel.Base;
using MedCore_DataAccess.ViewModels;
using System;
using Microsoft.AspNetCore.Cors;

namespace MedCoreAPI.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    [EnableCors]
    public class ContribuicaoController : BaseService
    {
        public ContribuicaoController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpPost]
        [MapToApiVersion("2")]
        [Route("[controller]/GetContribuicoes")]
        public ResultViewModel<IList<ContribuicaoViewModel>> GetContribuicoesViewModel(ContribuicaoFiltroDTO filtro)
        {
            SetStateHeadersFromRequest();
            if (Request == null) throw new NullReferenceException("Request null");
            var result = Execute(() =>
            {
                var business = new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity());
                var contribuicoes = business.GetContribuicoes(filtro);
                return contribuicoes;
            }, true);
            return GetResultViewModel<IList<ContribuicaoViewModel>, IList<ContribuicaoDTO>>(result);
        }

        [HttpPost]
        [Route("[controller]/GetContribuicoes")]
        public IList<ContribuicaoDTO> GetContribuicoes(ContribuicaoFiltroDTO filtro)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).GetContribuicoes(filtro); 
        }

        [HttpGet]
        [Route("[controller]/GetContribuicoesTeste")]
        public string GetContribuicoes()
        {
            return "Teste";
        }

        
        [HttpPost]
        [Route("[controller]/Inserir")]
        public int InserirContribuicao(Contribuicao contribuicao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).InserirContribuicao(contribuicao);
        }

        [HttpPost]
        [Route("[controller]/Aprovar")]
        public int AprovarContribuicao(Contribuicao contribuicao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).AprovarContribuicao(contribuicao);
        }

        [HttpPost]
        [Route("[controller]/Arquivar")]
        public int ArquivarContribuicao(Contribuicao contribuicao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).ArquivarContribuicao(contribuicao);
        }

        [HttpPost]
        [Route("[controller]/Encaminhar")]
        public int EncaminharContribuicao(Contribuicao contribuicao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).EncaminharContribuicao(contribuicao);
        }

        [HttpPost]
        [Route("[controller]/Deletar")]
        public int DeletarContribuicao(Contribuicao contribuicao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).DeletarContribuicao(contribuicao.ContribuicaoId);
        }

        [HttpPost]
        [Route("[controller]/Interacao")]
        public int InsertContribuicaoInteracao(ContribuicaoInteracao interacao)
        {
            return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).InsertInteracao(interacao);
        }

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("[controller]/GetBucket")]
        public ResultViewModel<ContribuicaoBucketViewModel> GetContribuicaoBucket()
        {
            var result = Execute(() =>
            {
                return new ContribuicaoBusiness(new ContribuicaoEntity(), new ContribuicaoArquivoEntity()).GetContribuicaoBucket();
            }, true);
            return GetResultViewModel<ContribuicaoBucketViewModel, ContribuicaoBucketDTO>(result);
        }
    }
}