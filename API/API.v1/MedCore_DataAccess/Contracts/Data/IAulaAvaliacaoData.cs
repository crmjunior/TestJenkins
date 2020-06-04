using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IAulaAvaliacaoData
    {
        List<AulaAvaliacao> GetAulaAvaliacaoPorAluno(int alunoID);

        AulaAvaliacao GetAulaAvaliacao(int alunoID, int apostilaID);         
    }
}