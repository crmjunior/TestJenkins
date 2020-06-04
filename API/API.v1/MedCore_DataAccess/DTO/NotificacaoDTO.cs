using System;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO
{
    public class NotificacaoDTO
    {     
        public int IdNotificacao { get; set; }
        public string Texto { get; set; }
        public string Titulo { get; set; }
        public bool Lida { get; set; }
        public string Data { get; set; }
        public int Quantidade { get; set; }
        public NotificacaoInfoAdicional InfoAdicional { get; set; }
        public DateTime DataOriginal { get; set; }
        public long DataUnix { get; set; }
        public bool Destaque { get; set; }
        public string Dia { get; set; }
        public int TipoNotificacaoId { get; set; }
        public TipoNotificacao TipoNotificacao { get; set; }
        public int Matricula { get; set; }

    }
}