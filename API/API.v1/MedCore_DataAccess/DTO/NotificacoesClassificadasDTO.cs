using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class NotificacoesClassificadasDTO
    {
        public List<NotificacaoClassificacaoDTO> NotificacoesClassificacao { get; set; }

        public NotificacaoDTO NotificacaoDestaque { get; set; }
    }
}