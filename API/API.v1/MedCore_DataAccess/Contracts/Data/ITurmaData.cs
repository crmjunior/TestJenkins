using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface ITurmaData
    {
         List<Turma> GetTurmasContratadas(int intClientID, int[] anos, int produto = 0, int adimplentes = 0);
        List<TurmaDTO> GetTurmasCronograma(int idFilial, int anoLetivoAtual);       
    }
}