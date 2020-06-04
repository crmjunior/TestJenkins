using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Model;
using System;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Contracts.Data
{
   public interface INotificacaoData
    {
        List<Notificacao> GetAll(int matricula, int idAplicacao);

        Notificacao Get(int idNotificacao);

        int SetNotificacaoLida(Notificacao notificacao);

        int SetNotificacao(Notificacao notificacao);

        List<Notificacao> GetNotificacoesPermitidas(int matricula, int idAplicacao);

        List<Notificacao> GetNotificacoesAplicacao(int idAplicacao, int matricula);

        List<DeviceNotificacao> GetDevicesNotificacaoFila(int notificacaoId);

        List<Notificacao> GetNotificacoesAProcessar(EStatusEnvioNotificacao status);

        void InserirDevicesNotificacao(List<DeviceNotificacao> devicesInscritos);

        void UpdateNotificacao(Notificacao notificacao);

        List<Notificacao> GetNotificacoesAdmin(int idAplicacao);

        List<PermissaoRegra> GetRegrasAdmin();

        int SetNotificacaoAgendada(Notificacao notificacao);

        int UpdateNotificacaoAgendada(Notificacao notificacao);

        int DeleteNotificacaoAgendada(int notificacaoId);
        List<AlunoTemaAvaliacao> GetAlunoTemaAvaliacao(ParametrosAvaliacaoAula parametros);

        List<DeviceNotificacao> GetDevicesNotificados(int idNotificacao, DateTime date);

        List<mview_Cronograma> GetCursosComUltimaAulaDoDia(ParametrosAvaliacaoAula parametros);

        List<Notificacao> GetNotificacoesPosEvento(EStatusEnvioNotificacao status);

        List<DeviceNotificacao> DefinirDevicesNotificacaoPosEvento(Notificacao notificacao, EStatusEnvioNotificacao status);

        List<DeviceNotificacao> BuscarFilaNotificacaoPosEvento(int idNotificacao);

        List<tblNotificacaoEvento> InserirNotificacoesPosEvento(params NotificacaoPosEventoDTO[] notificacoes);

        List<NotificacaoPosEventoDTO> GetNotificacoesAlunoPosEvento(int matricula, Aplicacoes aplicacao);
        int AtualizarNotificacoesPosEvento(List<tblNotificacaoEvento> notificacoes);

        tblNotificacaoEvento GetNotificacaoAlunoPosEvento(int idNotificacaoEvento);
    }
}