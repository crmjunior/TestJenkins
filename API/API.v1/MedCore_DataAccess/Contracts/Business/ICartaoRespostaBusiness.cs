using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface ICartaoRespostaBusiness
    {
        CartaoRespostaSimuladoAgendadoDTO GetCartaoRespostaSimuladoAgendado(int ClientID, int ExercicioID, int ExercicioHistoricoID);
    }
}