using System;

namespace MedCore_DataAccess.Entidades
{
    public class DeviceNotificacao
    {
        public int DeviceNotificacaoId { get; set; }

        public string DeviceToken { get; set; }

        public int NotificacaoId { get; set; }

        public EStatusEnvioNotificacao Status { get; set; }

        public string InfoAdicional { get; set; }

        public int ClientId { get; set; }

        public int DuvidaId { get; set; }

        public int IdentificadorId { get; set; }

        public DateTime Data { get; set; }

        public string Mensagem { get; set; }

        public string Titulo { get; set; }

    }
}