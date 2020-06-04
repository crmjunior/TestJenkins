using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Contracts.Repository
{
    public interface IDuvidasAcademicasData
    {
        IList<DuvidaAcademicaContract> GetDuvidas(DuvidaAcademicaFiltro filtroPost);
        IList<DuvidasAcademicasProfessorDTO> GetDuvidasProfessor(DuvidaAcademicaFiltro filtroPost);
        IList<DuvidaAcademicaContract> GetRespostasPorDuvida(DuvidaAcademicaFiltro filtro);
        DuvidaAcademicaReplicaResponse GetReplicasResposta(DuvidaAcademicaFiltro filtroPost);
        DuvidaAcademicaContract GetReplica(DuvidaAcademicaFiltro filtro);
        tblDuvidasAcademicas_Interacoes GetInteracao(DuvidaAcademicaInteracao interacao);
        IList<DuvidaAcademicaContract> GetDuvidasConcurso();
        string GetTrechoApostilaSelecionado(int duvidaId);
        tblDuvidasAcademicas_DuvidasArquivadas GetDuvidaArquivada(DuvidaAcademicaInteracao interacao);
        DuvidaAcademicaDTO GetDuvida(int idDuvida);
        tblDuvidasAcademicas_Resposta GetResposta(int idResposta);
        List<int> ListarUsuariosFavoritaramDuvida(int idDuvida, int donoDuvida, int clientId);
        List<int> ListarUsuariosResponderamDuvida(int idDuvida, int clientId);
        List<CronogramaSimplificadoDTO> GetExerciciosDuvidasQuestao();
        List<CronogramaSimplificadoDTO> GetProdutoIdDuvidasApostila();
        List<PessoaGrupoDTO> GetProfessoresDuvidasEncaminhadas(int matricula);
        List<AcademicoDADTO> GetProfessores();
        List<AcademicoDADTO> GetCoordenadores();
        IList<DuvidaAcademicaDTO> GetResolvidosProfessor(int idProfessor);
        IList<string> GetBlackWords();

        bool DeleteDenuncia(DenunciaDuvidasAcademicasDTO obj);
        bool InsertDenuncia(DenunciaDuvidasAcademicasDTO obj);

        int EnviarEmailDuvidaAcademica(string mailTo, string mailBody, string mailSubject, string mailProfile);

        int InsertInteracao(DuvidaAcademicaInteracao interacao);
        int InsertDuvidaQuestao(DuvidaAcademicaInteracao interacao);
        int InsertDuvidaApostila(DuvidaAcademicaInteracao interacao);
        int InsertDuvida(DuvidaAcademicaInteracao interacao);
        int InsertRespostaReplica(DuvidaAcademicaInteracao interacao);
        int InsertDuvidaLida(tblDuvidasAcademicas_Lidas entity);
        int InsertDuvidasEncaminhadas(DuvidaAcademicaInteracao duvidaInteracao);
        bool SetDuvidaAcademicaPrivada(DuvidasRespostaPrivadaDTO obj);
        bool SetRespostaReplicaPrivada(DuvidasRespostaPrivadaDTO obj);

        int SetRespostaHomologada(DuvidaAcademicaInteracao interacao);
        int SetDuvidaArquivada(DuvidaAcademicaInteracao duvidaInteracao);
        bool HasRespostaHomologada(int duvidaID);

        int UpdateRespostaReplica(DuvidaAcademicaInteracao interacao);
        int UpdateDuvida(DuvidaAcademicaInteracao interacao);
        int UpdateObservacaoMedGrupo(DuvidaAcademicaInteracao interacao);

        int DeleteDuvida(DuvidaAcademicaInteracao interacao);
        int DeleteDuvidaApostilaPorMarcacao(DuvidaAcademicaInteracao duvidaAcademicaInteracao);
        int DeleteRespostaReplica(DuvidaAcademicaInteracao interacao);
        int DeleteInteracao(tblDuvidasAcademicas_Interacoes interacao);
        int DeleteDuvidaArquivada(tblDuvidasAcademicas_DuvidasArquivadas duvida);
        QuestaoAcademicoDTO GetQuestaoConcurso(int questaoId);
        IList<DuvidaAcademicaContract> GetDuvidasAlunoApostila(DuvidaAcademicaFiltro filtroPost);
        IList<DuvidaAcademicaContract> GetDuvidasAlunoQuestoes(DuvidaAcademicaFiltro filtroPost);
        IList<DuvidaAcademicaContract> GetDuvidasAluno(DuvidaAcademicaFiltro filtroPost);
    }
}