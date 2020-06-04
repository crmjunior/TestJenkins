using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IRankingSimuladoData
    {
        public AlunoConcursoEstatistica GetSimuladoConsolidado(int matricula, int idSimulado);
        AlunoConcursoEstatistica GetEstatisticaAlunoSimulado(int matricula, int idSimulado, bool v);
        void InsertSimuladoConsolidado(int matricula, int idSimulado, AlunoConcursoEstatistica estatisticasAlunoRankingOnline);
        RankingSimuladoAluno GetRankingObjetiva(int matricula, int idSimulado, string especialidade, string unidades, string local = "");
        RankingSimuladoAluno GetRankingObjetivaCache(int matricula, int idSimulado, string especialidade, string unidades, string local = "");
        List<RankingDTO> GetRankingParcial(int idSimulado, string txtUnidade = "", string txtLocal = "", string txtEspecialidade = "", string txtStore = "", int matricula = 0);
        AlunoConcursoEstatistica GetEstatisticaAlunoSimuladoModoProva(int matricula, int idSimulado, int idHistorico);
        SimuladoDTO GetSimulado(int intSimuladoID);
        List<SimuladoResultadosDTO> ListResultado(int intSimuladoID);
        List<DadosOrdemVendaDTO> GetOrdemVendaTodosClientes(int intYear);
        List<LogSimuladoAlunoTurmaDTO> GetLogSimuladoAlunoTurma(int intSimuladoID);
        void RemoverSimuladoRankingFase01(int intSimuladoID);
        void InserirSimuladoRankingFase01(List<SimuladoRankingFase01DTO> lista);
        List<SimuladoRankingFase01DTO> GetRankingSimulado(int intSimuladoId);
        

    }
}