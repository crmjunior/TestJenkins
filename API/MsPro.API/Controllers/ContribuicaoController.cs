using System.Threading.Tasks;
using CAContext.Application.Interfaces;
using CAContext.Application.ViewModels;
using CAContext.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace CAContext.API.Controllers
{
    [ApiController]
    public class ContribuicaoController : ApiController
    {
        private readonly IContribuicaoAppService _contribuicaoService;

        public ContribuicaoController(IContribuicaoAppService contribuicaoService)
        {
            _contribuicaoService = contribuicaoService;
        }

        [HttpPost("[controller]/Inserir")]
        public async Task<IActionResult> InserirContribuicao(ContribuicaoViewModel contribuicao)
        {
            var model = await _contribuicaoService.InserirContribuicao(contribuicao);
            return CreateResponse(model, model.ValidationResult);
        }

        [HttpDelete("[controller]/Deletar/{id}")]
        public async Task DeletarContribuicao(int id)
        {
            await _contribuicaoService.DeletarContribuicao(id);
        }

        [HttpGet("[controller]/Obter/{id}")]
        public async Task<IActionResult> ObterContribuicao(int id)
        {
            var model = await _contribuicaoService.GetContribuicao(id);
            return CreateResponse(model);
        }

        [HttpGet("[controller]/Listar/")]
        public async Task<IActionResult> ListarContribuicoes(Filtro filtro)
        {
            var model = await _contribuicaoService.GetContribuicoes(filtro);
            return CreateResponse(model);
        }
    }
}