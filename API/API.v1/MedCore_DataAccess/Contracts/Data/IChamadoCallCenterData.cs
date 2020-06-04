using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IChamadoCallCenterData
    {
        int GetAtributoClassificacao(int idClassificacao, string idProduto);
        int SetClassificacaoTurmaDesejada(int matricula, int idClassificacao, int idAtributoClassificacao);
        int Insert(ChamadoCallCenter registro);

        void AdicionarClassificacaoTurmaDesejada(int idCliente, int idClassificacao, int idProduto);
    }
}