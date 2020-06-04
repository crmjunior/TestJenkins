using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAContext.Application.ViewModels;
using CAContext.Domain.Entities;
using CAContext.Domain.ValueObjects;

namespace CAContext.Application.Interfaces
{
    public interface IContribuicaoAppService : IDisposable
    {
        Task<IEnumerable<ContribuicaoViewModel>> GetContribuicoes(Filtro e);
        Task<ContribuicaoViewModel> InserirContribuicao(ContribuicaoViewModel e);
        Task<int> DeletarContribuicao(int id);
        Task<ContribuicaoViewModel> GetContribuicao(int id);   
    }
}