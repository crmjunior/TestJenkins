using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IEspecialidadeData
    {
         List<Especialidade> GetByFilters(int QuestaoID);

         List<Especialidade> GetByQuestaoSimulado(int questaoID, int simuladoID);

         List<Especialidade> GetAll();
    }
}