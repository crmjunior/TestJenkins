using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using System.Collections.Generic;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IContribuicaoBusiness
    {
        IList<ContribuicaoDTO> GetContribuicoes(ContribuicaoFiltroDTO e);
        int InserirContribuicao(Contribuicao e);
        int DeletarContribuicao(int id);
        ContribuicaoDTO GetContribuicao(int id);
        int AprovarContribuicao(Contribuicao e);
        int ArquivarContribuicao(Contribuicao e);
        int EncaminharContribuicao(Contribuicao e);
        int InsertInteracao(ContribuicaoInteracao e);
        ContribuicaoBucketDTO GetContribuicaoBucket();
    }
}