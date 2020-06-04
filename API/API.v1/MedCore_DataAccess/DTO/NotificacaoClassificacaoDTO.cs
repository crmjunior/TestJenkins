using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class NotificacaoClassificacaoDTO
    {

        public int TipoNotificacao { get; set; }

        public int Ordem { get; set; }

        public int Quantidade { get; set; }

        public string Alias { get; set; }

        public List<NotificacaoDTO> Notificacoes { get; set; }

    }
}