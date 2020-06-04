using System.Threading.Tasks;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IPerfilAlunoData
    {
        bool IsAlunoR3(int matricula);

        bool AlunoTemInteresseRMais(int matricula);

        Task<bool> IsAlunoExtensivoAsync(string register);
    }
}