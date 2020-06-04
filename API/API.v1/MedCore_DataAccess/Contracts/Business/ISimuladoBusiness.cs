using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface ISimuladoBusiness
    {
         SimuladoDTO GetSimuladoPorId(int id);
         List<SimuladoDTO> GetSimuladosPorAno(int ano);
         List<TemaSimuladoDTO> GetTemasSimuladoPorAno(int ano);
         List<TipoSimuladoDTO> GetTiposSimulado();
    }
}