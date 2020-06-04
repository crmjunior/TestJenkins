using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using System.Collections.Generic;
using MedCore_DataAccess.Model;
using System.Threading.Tasks;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IQuestaoData
    {
        int GetExercicioHistorico(int intSimuladoId, int matricula);

        ExercicioHistoricoDTO GetExercicioHistorico(int ExercicioHistoricoID);
        MarcacoesObjetivaDTO GetUltimaMarcacao_SimuladoOnline(int QuestaoID, int ExercicioTipoId, int Matricula);

        int SetRespostaObjetivaSimuladoOnline(RespostaObjetivaPost resp, MarcacoesObjetivaDTO ultimaMarcacao);

        MarcacoesObjetivaDTO GetUltimaMarcacaobyIntExercicioHistoricoID(int QuestaoID, int ExercicioTipoId, int Matricula, int? TipoProva, int IntExercicioHistoricoID);

        int SetRespostaObjetiva(RespostaObjetivaPost resp, MarcacoesObjetivaDTO ultimaMarcacao);

        Dictionary<string, string> GetEstatistica(int idQuestao, int idTipoExercicio);

        tblConcursoQuestoes ObterQuestaoConcurso(int QuestaoID);

        string ObterRespostaQuestaoConcurso(int QuestaoID);

        List<tblConcursoQuestoes_Alternativas> ObterAlternativasQuestaoConcurso(int QuestaoID);

        tblConcursoQuestoes_Alternativas ObterPrimeiraAlternativa(int QuestaoID);

        List<CartaoRespostaDiscursivaDTO> ObterRespostasDiscursivas(int QuestaoID, int ClientID, int TipoExercicio = (int)Exercicio.tipoExercicio.CONCURSO);

        QuestaoAnotacao GetAnotacaoQuestaoConcurso(int QuestaoId, int ClientId);

        CartaoRespostaObjetivaDTO ObterRespostaAlternativa(int QuestaoID, int ClientID);

        List<tblConcursoQuestoes_Alternativas> ObterAlternativaCorreta(int QuestaoID, int ClientID);

        ProvaConcursoDTO ObterProvaConcurso(tblConcursoQuestoes queryQuestaoConcurso);

        ForumQuestaoRecurso GetForumQuestaoRecurso(int idQuestao, int matricula, bool visitante = false);
        List<PPQuestao> GetQuestoesComComentarioApostilaCache(int EntidadeID);

        QuestaoDTO CacheQuestao(int QuestaoID);

        List<Alternativa> GetAlternativasQuestao(int QuestaoID, string isCasoClinico);

        TabelaQuestaoSimuladoDTO GetQuestao_tblQuestaoSimulado(int QuestaoID);

        List<Imagem> GetComantarioImagemSimulado(int QuestaoID);

        SimuladoVersaoDTO GetSimuladoVersao(int QuestaoID);

        bool GetSimuladoIsOnline(int ClientID, int QuestaoID);
        
        string GetRespostaObjetivaSimulado(int QuestaoID, int ClientID, bool isSimuladoOnline);

        bool GetQuestaoSimuladoIsCorreta(int ClientID, int QuestaoID, bool isSimuladoOnline);

        QuestaoMarcacaoDTO GetQuestaoMarcacao(int QuestaoID, int ClientID);

        int EnviarVotoComentarioForum(int idQuestao, int matricula, string votoAluno, string texto, QuestaoRecurso.TipoForumRecurso tipoForum);

        Task EnvioEmailComentarioForumPosAsync(int idQuestao, int matricula, string comentario, bool voto);

        int UpdateQuestaoConcurso(tblConcursoQuestoes questao);

        tblConcursoQuestoes GetQuestaoConcursoById(int idQuestao);

        bool IsQuestaoProvaRMais(int idQuestao);

        Task EnvioEmailComentarioForumPreAsync(int idQuestao, int matricula, string comentario, bool voto);

        tblConcursoQuestoes_Aluno_Favoritas GetConcursoQuestoesAlunoFavorita(int idQuestao, int matricula);

        IEnumerable<AlternativaQuestaoConcursoDTO> ObterAlternativasComEstatisticaFavorita(int idQuestao);
        int InserirQuestaoConcursoAlunoFavoritas(tblConcursoQuestoes_Aluno_Favoritas alternativa);

        int UpdateQuestoesConcursoAlunoFavoritas(tblConcursoQuestoes_Aluno_Favoritas alternativa);

        RecursoQuestaoConcursoDTO GetQuestaoConcursoRecurso(int idQuestao, int matricula);

        Questao GetGabaritoDiscursiva(int idQuestao, string enunciado = "");

        QuestaoConcursoAlternativaFavoritaDTO GetAlternativaFavoritaQuestaoConcurso(int idQuestao, int matricula);

        IEnumerable<tblConcursoQuestoes_recursosComentario_Imagens> ObterImagensComentarioRecurso(int idQuestao);

        IEnumerable<ForumComentarioDTO> ObterComentariosForumConcursoPre(int idQuestao, int matricula);

        IEnumerable<ForumComentarioDTO> ObterComentariosForumConcursoPos(int idQuestao, int matricula);

        IEnumerable<ForumComentarioDTO> ObterComentarioForumPosProfessor(int idQuestao);

        string GetEmailEnvioAnaliseQuestaoAluno(int matricula);

        List<RecursoQuestaoConcursoDTO> GetQuestoesProvaConcurso(int idProva);

        ProvaConcursoDTO GetProvaConcurso(int idProva);

        bool AlunoTemRankAcertos(int idProva, int matricula);

        bool AlunoSelecionouAlternativaQuestaoProva(int idProva, int matricula);

        bool AlunoViuAvisoComentarioRecurso(int matricula);

        bool AlunoComunicadoHabilitado(int idProva, int matricula);
        
        ProvaRecursoLive GetLiveProva(int idProva);

        int DesabilitaAcertosQuestaoConcurso(int matricula, int idProva, int idEmployee);

        bool AlunoJaVotouForumQuestao(int idQuestao, int matricula, QuestaoRecurso.TipoForumRecurso tipoForum);

        List<QuestaoConcursoVotosDTO> GetQtdCabeQuestoesConcurso(params int[] idQuestaoList);

        List<ConcursoQuestaoDTO> ListConcursoQuestao(string siglaConcurso, int anoQuestao, int anoQuestaoPublicada);

        Professor GetProtocoladaPara(int idQuestao, int ano);

        Professor GetPrimeiroComentario(int idQuestao);

        Professor GetUltimoComentario(int idQuestao);

        Questao GetTipoApostila(int QuestaoID, int ClientID, int ApplicationID);

        List<CartaoRespostaDiscursivaDTO> GetRespostasDiscursivasSimuladoAgendado(int QuestaoID, int ExercicioHistoricoID);

        string GetRespostaObjetivaSimuladoAgendado(int QuestaoID, int ExercicioHistoricoID);

        List<QuestaoFiltroDTO> GetMarcacoesQuestoesAluno(int matricula);
        
        List<QuestaoFiltroDTO> GetQuestoesIds(int[] ids);

        ProvaAlunosFavoritoDTO GetAlunosVotaramForumPre(int idQuestao);

        int ObterStatusRecursoBanca(int idQuestao);
    }
}