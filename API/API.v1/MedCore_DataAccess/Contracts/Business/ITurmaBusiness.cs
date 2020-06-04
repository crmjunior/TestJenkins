using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface ITurmaBusiness
    {
      List<TurmaDTO> GetTurmasCronograma(int idFilial, int anoLetivo);
    }
}