using MedCore_DataAccess.Entidades;
using System.Collections.Generic;

namespace MedCore_DataAccess.Contracts.Repository
{
    public interface IContribuicaoArquivoData
    {
        int InserirContribuicaoArquivo(ContribuicaoArquivo contribuicao);
        int DeletarContribuicaoArquivo(IList<int> lstArquivoIDs);
        IList<ContribuicaoArquivo> ListarArquivosContribuicao(int idContribuicao);
        int UpdateContribuicaoArquivo(ContribuicaoArquivo arquivo);
    }
}
