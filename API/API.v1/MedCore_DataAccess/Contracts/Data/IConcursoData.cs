using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IConcursoData
    {

        List<Exercicio> GetProvas(string siglaConcurso, int matricula);


        List<ConcursoDTO> GetConcursosPorProvas(int matricula, int idaplicacao, List<int> provas);

        		List<QuestaoConcursoAlunoDTO> GetQuestoesbyIdExercicio(int idExercicio);
		List<RespostaConcursoAlunoDTO> GetRespostabyIdExercicioIDMatricula(int idExercicio, int idMatriculam, int[] tipoExercicio);

        int InserirConfiguracaoProvaAluno(tblProvaAlunoConfiguracoes configuracao);
        
    }
}