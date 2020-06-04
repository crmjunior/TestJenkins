using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IRevalidaData
    {
         
         List<ProgressoSemana> GetProgressoRevalida(int idCliente, int produto);
    }
}