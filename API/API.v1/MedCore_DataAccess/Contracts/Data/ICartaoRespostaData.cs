using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface ICartaoRespostaData
    {
        int? GetYearCache();
        int? GetYear();
        CartoesResposta GetCartaoRespostaSimulado(Int32 ExercicioID, Int32 ClientID, int historicoExercicioId);
        CartoesResposta GetCartaoRespostaConcurso(int ExercicioID, int ClientID);
        CartoesResposta GetCartaoRespostaMontaProva(int ExercicioID, int ClientID);      
        List<csp_ListaMaterialDireitoAluno_Result> ListaMaterialDireitoAluno(int clientID, int ano, int? intProductGroup1);  
        List<Questao> GetQuestoesSomenteImpressasComOuSemVideoCache(int ExercicioID, int? anoAtual);
        List<Questao> GetQuestoesComVideosCache(int ExercicioID, List<int> idsPPQuestoes, List<int> idsQuestoesImpressas);
        List<MarcacoesObjetivaDTO> ListarUltimasMarcacoesObjetiva(int clientID, List<int> listaIdsQuestoes);
        List<ConcursoQuestoes_Alternativas> ListarMarcacoesObjetivasComGabarito(List<int> listaIdsQuestoes);
        bool ExisteRespostasDiscursivas(int clientID, int exercicioID, int questaoID);
        List<OrdemSimuladoDTO> ObterOrdemSimulado(int exercicioID);
        List<Questao> ObterQuestoesSimuladoComOrdem(int ExercicioID);
        List<Questao> ObterQuestoesSimuladoSemOrdem(int exercicioID);
        List<RespostasObjetivasCartaoRespostaSimuladoDTO> GetRespostasObjetivasSimuladoAgendado(int exercicioHistoricoID, int[] questoesIds);
        List<RespostasDiscursivasCartaoRespostaSimuladoDTO> GetRespostasDiscursivasSimuladoAgendado(int exercicioHistoricoID, int[] questoesIds);
        CartoesResposta GetCartaoRespostaApostila(int ExercicioID, int ClientID, int idApostila);
    }
}