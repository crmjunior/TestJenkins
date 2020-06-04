using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
     public interface INotificacaoBusiness
    {
        List<Notificacao> GetAll(int matricula, int idAplicacao);

        Notificacao GetNotificacao(int idNotificacao);

        int SetNotificacaoLida(Notificacao notificacao);

        OneSignalNotificationResponse SendSeletiva(NotificacaoSeletiva notificacao, string chaveKeyAppId, string chaveKeyGcm);

        List<Notificacao> GetNotificacoesPorPerfil(int idClient, int idAplicacao, int conteudoCompleto = 0, int idProduto = 0, string versao = "");
    }
}