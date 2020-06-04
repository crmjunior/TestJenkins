using System;
using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IExercicioData
    {
        Dictionary<bool, List<int>> GetIdsExerciciosRealizadosAluno(int matricula, int idTipoSimulado = (int) Constants.TipoSimulado.Extensivo);
        List<Exercicio> GetSimuladosByFilters(int ano, int matricula, int idAplicacao, bool p, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo);
        void RegistrarSimuladoOnline(int historicoId);
        List<CartaoRespostaObjetivaDTO> ObterQuestoes(int exercicioHistoricoId);
        SimuladoOnlineExcecaoDTO ObterSimuladoAlunoExcecao(int clientId, int exercicioId);
        TblSimuladoDTO ObterSimulado(int exercicioId);
        ExercicioHistoricoDTO ObterExercicio(int historicoId);
        void FinalizarExercicio(Exercicio exercicio);
        void InserirQuestoesSimuladoOnline(List<CartaoRespostaObjetivaDTO> questoes);
        bool AlunoJaRealizouSimuladoOnline(int clientId, int simuladoId);
        Exercicio GetSimuladoOnlineCorrente();
        List<int> GetExerciciosSimuladoNaoFinalizados(int simuladoId);
        List<int> GetExerciciosSimuladoNaoFinalizadosAlunosExcecao(int simuladoId);
        Exercicio GetUltimoSimuladoAgendadoComGabaritoLiberado();
        void ReplicarSimuladoOnlineTabelasMGE(int clientId, int simuladoId);
        ExercicioHistoricoDTO InserirExercicioSimulado(int idSimulado, int matricula, int idAplicacao, TipoProvaEnum tipoProva);
        ExercicioHistoricoDTO ObterUltimoExercicioSimuladoModoProva(int matricula, int simuladoId);
        List<ExercicioHistoricoDTO> GetComboSimuladosRealizados(int matricula, int simuladoId, int idAplicacao);
        int ObterAcertosDoAlunoNoSimulado(int idSimulado, int matricula, int idAplicacao);
        List<PosicaoRankingDTO> ObterRankingPorSimulado(int idSimulado, string especialidade, string unidades, string localidade);
        int ObterQuantidadeParticipantesSimuladoOnline(int idSimulado);
        int ObterQuantidadeQuestoesSimuladoOnline(int idSimulado);
        List<ExercicioHistoricoDTO> ObterExerciciosEmAndamento(int matricula, int idAplicacao);
        List<Exercicio> GetSimulados(int matricula, int idAplicacao = 1, bool getOnline = false, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo, int anoExercicio = 0);
        Simulado GetSimuladoOnlineConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion);
        Simulado GetSimuladoConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion);
                
        bool IsProdutoSomenteMedeletro(List<Produto> produtosContratados);
        List<Int32> GetAnosSimulados(int matricula, bool getOnline = false, int idAplicacao = 1, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo);
        List<Int32> GetAnosExerciciosPermitidos(Exercicio.tipoExercicio tipoExercicio, int matricula, bool getOnline = false, int idAplicacao = 1);

        String GetPdfSimuladoImpressa(int idexercicio, int ano, string environmentRootPath);

        List<Exercicio> GetByFilters(Exercicio.tipoExercicio tipoExercicio, Int32 anoExercicio, int matricula, int idAplicacao = 1, bool getOnline = false);
        Concurso GetProvaPersonalizadaConfiguracao(int idProva, int matricula, int idAplicacao);

        List<int> ListarHistoricoExercicioIdAbertos(DateTime data, int tolerancia);
        List<int> ListarHistoricoExercicioIdAbertos(int tolerancia, List<int> SimuladoVigenteIds);
        List<SimuladoDTO> ListarIdsSimuladoVigente();
        int SetAcertosForumProva(ForumProva forum);
        int GetIdEspecialidade(int matricula);
        List<ForumProva.Acerto> GetForumAcertos(int idProva, int idEspecialidade, int matricula = 0);
        List<ForumProva.Comentario> GetForumComentarios(int idProva, int qtdComentarios, double ultimaDataComentario, int matricula = 0);
        int SetComentarioForumProva(ForumProva forum);
        List<CartaoRespostaObjetivaDTO> ObterQuestoesOnline(int exercicioHistoricoId);
        void InserirQuestoesSimulado(List<CartaoRespostaObjetivaDTO> questoes, int intClientId);
        ProvasRecurso GetConcursosRecursos(int ano, int matricula);
        int[] FiltrarIdsProvasDiscursivas(params int[] idsProva);
        int SetStatusProvaFavorita(int provaId, int matricula);
        ProvasRecurso GetProvasFavoritas(int matricula);
        List<ProvaRecursoConcursoDTO> GetProvasConcursos(int ano, int matricula, bool modoRMais);

        ProvaAlunosFavoritoDTO GetAlunosFavoritaramProva(int idProva);
        List<SimuladoCronogramaDTO> GetCronogramaSimulados(int ano, int matricula);

        IDictionary<int, ProvaSubespecialidade> GetSubespecialidadesProvas(int ano, bool modoRMais);
        bool AlunoFavoritouProva(int idProva, int matricula);
        int GetIDSimuladoCPMED(int ano);
    }
}