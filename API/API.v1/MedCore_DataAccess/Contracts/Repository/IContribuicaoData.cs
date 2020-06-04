using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using System.Collections.Generic;

namespace MedCore_DataAccess.Contracts.Repository
{
    public interface IContribuicaoData
    {
        IList<ContribuicaoDTO> GetContribuicoes(ContribuicaoFiltroDTO filtro);
        int InserirContribuicao(Contribuicao e);
        int UpdateContribuicao(Contribuicao e);
        int DeletarContribuicao(int id);
        ContribuicaoDTO GetContribuicao(int id);
        int AprovarContribuicao(Contribuicao e);
        int ArquivarContribuicao(Contribuicao e);
        int EncaminharContribuicao(Contribuicao e);
        int InsertInteracao(ContribuicaoInteracao e);
        ContribuicaoInteracao GetInteracao(ContribuicaoInteracao e);
        int DeleteContribuicaoInteracao(int id);
        int DeleteContribuicaoArquivada(int id);
        bool HasContribuicaoArquivada(Contribuicao e);
    }
}