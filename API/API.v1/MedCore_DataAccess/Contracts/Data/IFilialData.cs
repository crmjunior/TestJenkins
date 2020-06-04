using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IFilialData
    {
        Filial GetFilial(Int32 FilialID);
         List<FilialCronogramaDTO> GetFilialCronograma();
    }
}