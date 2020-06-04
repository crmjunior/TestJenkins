using CAContext.Domain.Entities;
using Shared.Interfaces;

namespace CAContext.Domain.Interfaces.Repositories
{
    public interface IContribuicaoRepository : IRepository<Contribuicao>
    {
        bool TemDescricaoRepetida(string descricao);
    }
}