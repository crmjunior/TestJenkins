using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAContext.Domain.Entities;
using CAContext.Domain.ValueObjects;

namespace CAContext.Domain.Interfaces.Business
{
    public interface IContribuicaoBusiness : IDisposable
    {
        Task<IEnumerable<Contribuicao>> GetContribuicoes(Filtro e);
        Task<Contribuicao> InserirContribuicao(Contribuicao e);
        Task DeletarContribuicao(int id);
        Task<Contribuicao> GetContribuicao(int id);      
    }
}