using System;

namespace MedCore_DataAccess.DTO
{
    public class NotificacaoPosEventoDTO
    {
        public int IdNotificacao { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Metadados { get; set; }
        public DateTime Data { get; set; }
        public int Matricula { get; set; }
        public bool Ativa { get; set; }
        public bool Lida { get; set; }
    }
}