using System.Collections.Generic;

namespace MedCore_DataAccess.Entidades
{
    public class NotificacaoPushResponse
    {
        public int NotificacaoId { get; set; }

        public bool Sucesso { get; set; }

        public string Erro { get; set; }

        public List<OneSignalNotificationResponse> OneSignalResponse { get; set; }
    }
}