using System.Collections.Generic;
using System.Threading.Tasks;
using PermissaoContext.Domain.ValueObjects;

namespace PermissaoContext.Domain.Repositories
{
    public interface IAcessoAplicacaoRepository
    {
        Task<List<Material>> GetMateriaisPermitidos(int matricula);

        List<MaterialDireito> GetMateriaisDireito(int matricula);
        bool IsBlackList(int matricula);

    }
}