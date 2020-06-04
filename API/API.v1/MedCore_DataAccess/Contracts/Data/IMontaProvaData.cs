using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IMontaProvaData
    {
        List<KeyValuePair<int, int?>> ObterQuestoesMontaProva(ProvaAluno prova);
        List<ProvaAluno> ObterProvasAluno(int idFiltro);
        List<RespostaConcursoDTO> ObterRespostasConcurso(int matricula, int[] questoesConcurso);
        List<RespostaSimuladoDTO> ObterRespostasSimulado(int matricula, int[] questoesSimulado);
        int GetQuantidadeQuestoesNaoAssociadas(int idFiltro);
        tblFiltroAluno_MontaProva GetFiltro(int idFiltro);
        int GetQuantidadeQuestoesAssociadas(int idFiltro);
        int GetQuantidadeQuestoesFiltro(int idFiltro);
        void SetFiltroQuantityCounter(int idFiltro, int questoesCount);
        void DeleteQuestoesNaoAssociadas(int idFiltro);
        List<KeyValuePair<int, int?>> GetQuestoesProva(ProvaAluno prova);
		List<ProvaAluno> ObterContadorDeQuestoes(int matricula);
		int DeleteNovo(ProvaAluno provaAluno);
        int AlterarQuestoesProvaNovo(int idFiltro, int idProva, int quantidade);
        List<FiltroAluno> GetFiltrosAluno(int matricula, int page, int limit);
		void InserirContadorDeQuestoes(ProvaAluno prova, int matricula);
		List<int> ObterIdProvasPorQuestao(int questao);
		void ModificarQuestoesContador(int idFiltro, int qtdQuestoes, int acertos = 0, int erros = 0, int realizacao = 0);
		void AtualizarContadorDeQuestoes(ProvaAluno prova);
    }
}