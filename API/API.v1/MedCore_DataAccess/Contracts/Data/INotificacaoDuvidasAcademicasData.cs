using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface INotificacaoDuvidasAcademicasData
    {
        int SetNotificacaoDuvidaAcademica(NotificacaoDuvidaAcademica notificacao);

        int SetNotificacaoDuvidaAcademicaLida(NotificacaoDuvidaAcademica notificacao);

        int SetNotificacaoDuvidaAcademicaAtiva(NotificacaoDuvidaAcademica notificacao);

        int SetNotificacaoDuvidasAcademicaAlunoEnviada(int clientId, DateTime data);

        List<DeviceNotificacao> GetAlunosNotificacaoDuvida();

        List<NotificacaoDuvidaAcademica> GetNotificacoesDuvidaPorAluno(int duvidaId, int clientId, int Categoria);

        List<Notificacao> GetNotificacoesDuvidasAcademicasAluno(int clientId);

        int DeleteNotificacoesAluno(int matricula);
    }
}