using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IVersaoAppPermissaoEntityData
    {
        string GetUltimaVersaoBloqueada(int AplicacaoId = 0);

        List<VersaoAppPermissaoDTO> GetVersoesProduto(int produtoId);
    }
}