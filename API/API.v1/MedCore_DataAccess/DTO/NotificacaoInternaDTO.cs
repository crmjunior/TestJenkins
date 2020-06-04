using System;

namespace MedCore_DataAccess.DTO
{
    public class NotificacaoInternaDTO
    {
        public int IdNotificacao { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public DateTime Data { get; set; }
        public string Icone { get; set; }
        public string Parametros { get; set; }
        public bool Lida { get; set; }
    }
}