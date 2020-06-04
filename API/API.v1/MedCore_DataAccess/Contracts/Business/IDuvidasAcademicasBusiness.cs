using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.DTO.DuvidaAcademica;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IDuvidasAcademicasBusiness
    {
        IList<DuvidaAcademicaContract> GetDuvidas(DuvidaAcademicaFiltro filtro);
        IList<DuvidasAcademicasProfessorDTO> GetDuvidasProfessor(int idProfessor);
        IList<DuvidaAcademicaContract> GetRespostasPorDuvida(DuvidaAcademicaFiltro filtro);
        DuvidaAcademicaReplicaResponse GetReplicasResposta(DuvidaAcademicaFiltro filtro);
        string GetTrechoApostilaSelecionado(int duvidaId);
        List<CronogramaSimplificadoDTO> GetCronogramaSimplificado(int idProduto, int matricula, bool isQuestao);
        tblDuvidasAcademicas_Resposta GetResposta(int idResposta);

        int InsertInteracao(DuvidaAcademicaInteracao interacao);
        int InsertDuvidaLida(DuvidaAcademicaInteracao interacao);
        int InsertDuvida(DuvidaAcademicaInteracao interacao);
        DuvidaAcademicaContract InsertObservacaoMedGrupo(DuvidaAcademicaInteracao interacao);
        DuvidaAcademicaContract InsertResposta(DuvidaAcademicaInteracao interacao);
        DuvidaAcademicaContract InsertReplica(DuvidaAcademicaInteracao interacao);
        int InsertDuvidasEncaminhadas(DuvidaAcademicaInteracao duvidaInteracao);
        bool SetRespostaHomologada(DuvidaAcademicaInteracao interacao);
        int SetDuvidaArquivada(DuvidaAcademicaInteracao duvidaInteracao);
        bool SetDenuncia(DenunciaDuvidasAcademicasDTO obj);

        int DeleteDuvida(DuvidaAcademicaInteracao interacao);
        int DeleteRespostaReplica(DuvidaAcademicaInteracao interacao);
        int DeleteDuvidaApostilaPorMarcacao(DuvidaAcademicaInteracao interacao);

        bool SetRespostaReplicaPrivada(DuvidasRespostaPrivadaDTO obj);
        bool SetDuvidaAcademicaPrivada(DuvidasRespostaPrivadaDTO obj);
    }
}