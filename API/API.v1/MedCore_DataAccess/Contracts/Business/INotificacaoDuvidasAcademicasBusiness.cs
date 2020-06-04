using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface INotificacaoDuvidasAcademicasBusiness
    {
        int SetNotificacaoLida(int DuvidaId, int ClientId, int Categoria);

        int SetNotificacaoAtiva(int DuvidaId, int ClientId, int Categoria);

        void SetNotificacao(DuvidaAcademicaContract duvida, List<int> alunosFavoritaram, List<int> alunosInteragiram, EnumTipoNotificacaoDuvidasAcademicas tipo, int? clientResposta);

        int DeleteNotificacoesAluno(int matricula);

        List<NotificacaoDuvidaAcademica> GetNotificacoesDuvidaPorAluno(int duvidaId, int clientId, int categoria = 0);
    }
}