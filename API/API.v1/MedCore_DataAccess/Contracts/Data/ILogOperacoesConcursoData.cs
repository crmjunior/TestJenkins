using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface ILogOperacoesConcursoData
    {
         Task<int> InserirLogAsync(TipoOperacoesConcursoEnum tipoAlteracao, int idEmployee, params object[] dados);
    }
}