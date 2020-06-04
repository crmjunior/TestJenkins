using System.Collections.Generic;
using System.Threading.Tasks;
using PermissaoContext.Domain.ValueObjects;

namespace PermissaoContext.Domain.Repositories
{
    public interface IAcessoConteudoRepository
    {
        Task<List<Material>> GetApostilasPermitidas(int matricula);

        Task<IList<MaterialDireito>> GetMateriaisDireito(int matricula);


    }
}