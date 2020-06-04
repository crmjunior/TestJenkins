using System.Collections.Generic;
using CAContext.Domain.Entities;
using CAContext.Domain.ValueObjects;
using CAContext.Application.Interfaces;
using CAContext.Application.ViewModels;
using System;
using AutoMapper;
using System.Threading.Tasks;
using CAContext.Domain.Interfaces.Business;
using Shared;
using Shared.Interfaces;

namespace CAContext.Application.Services
{
    public class ContribuicaoAppService : BaseService, IContribuicaoAppService
    {
        private readonly IContribuicaoBusiness _contribuicaoBusiness;
        private readonly IMapper _mapper;

        public ContribuicaoAppService(IContribuicaoBusiness contribuicaoBusiness,
                                   IUnitOfWork uow,
                                   IMapper mapper)
            :base(uow)
        {
            _mapper = mapper;
            _contribuicaoBusiness = contribuicaoBusiness;
        }

        public async Task<int> DeletarContribuicao(int id)
        {
            await _contribuicaoBusiness.DeletarContribuicao(id);
            return await Commit();
        }

        public void Dispose()
        {
            _contribuicaoBusiness.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<ContribuicaoViewModel> GetContribuicao(int id)
        {
            var contribuicao = await _contribuicaoBusiness.GetContribuicao(id);
            return _mapper.Map<ContribuicaoViewModel>(contribuicao);
        }

        public async Task<IEnumerable<ContribuicaoViewModel>> GetContribuicoes(Filtro e)
        {
            var contribuicoes = await _contribuicaoBusiness.GetContribuicoes(e);
            return _mapper.Map<List<ContribuicaoViewModel>>(contribuicoes);
        }

        public async Task<ContribuicaoViewModel> InserirContribuicao(ContribuicaoViewModel e)
        {
            var contribuicao = _mapper.Map<Contribuicao>(e);

            contribuicao = await _contribuicaoBusiness.InserirContribuicao(contribuicao);

            if(contribuicao.ValidationResult.IsValid)
                await Commit();

            return _mapper.Map<ContribuicaoViewModel>(contribuicao);
        }
    }
}