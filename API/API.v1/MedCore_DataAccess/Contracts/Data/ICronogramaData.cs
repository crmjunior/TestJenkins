using System.Collections.Generic;
using System.Threading.Tasks;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface ICronogramaData
    {
         public int UltimoAnoCursadoAluno(int matricula, int idProduto);

         List<EspecialRevalida> RevalidaCronogramaPermissao(List<EspecialRevalida> lsRevalida, int matriculaId);

         List<EspecialRevalida> GetRevalidaCronograma(int idProduto, int ano);

         Task<List<CronogramaPrateleiraDTO>> GetCronogramaPrateleirasAsync(int idProduto, int ano, int matricula, int menuId);

         List<long> GetBookEntitiesBloqueadosNoCronograma();

         List<ApostilaCodigoDTO> GetCodigosAmigaveisApostilas();

         List<CronogramaPrateleiraDTO> GetCronogramaPrateleirasCPMEDTurmaConvidada(int ano, int menuId, int turmaId);

         List<MaterialChecklistDTO> GetChecklistsExtrasLiberados(int matricula, int idProduct);

         List<MaterialChecklistDTO> GetChecklistsPraticosLiberados(int matricula, int idProduct);

         List<AulaTema> GetTemasAnosAnteriores(List<string> nomes);

         List<msp_API_ListaEntidades_Result> GetListaEntidades(int idProduto, int ano, int matricula);

         List<CronogramaPrateleiraDTO> GetConfiguracaoMateriaisEntidades(int menuId, int produtoId);

         List<TurmaMatriculaBaseDTO> GetTurmaMatriculasBase(TurmaMatriculaBaseDTO filtro);
         
         List<int?> AnosProdutoAluno(int matricula, int idProduto, int anoMaterial = 0);
    }
}