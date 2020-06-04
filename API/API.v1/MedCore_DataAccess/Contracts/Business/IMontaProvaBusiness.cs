using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IMontaProvaBusiness
    {
        ProvasAluno GetProvasFiltro(int matricula, int idFiltro);
        FiltrosAluno GetProvasAluno(int matricula, int idAplicacao);
		ProvasAluno GetProvasFiltroContador(List<ProvaAluno> provasContadorQuestoes, int matricula, int idFiltro);
		ProvaAlunoDTO ObterProvaSimulado(int matricula, List<KeyValuePair<int, int?>> questoes);
        ProvaAlunoDTO ObterProvaConcurso(int matricula, List<KeyValuePair<int, int?>> questoes);         
    }
}