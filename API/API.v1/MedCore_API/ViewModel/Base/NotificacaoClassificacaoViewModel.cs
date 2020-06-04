using System.Collections.Generic;

namespace MedCore_API.ViewModel.Base
{
     public class NotificacaoClassificacaoViewModel
    {
        public IEnumerable<NotificacaoViewModel> Notificacoes { get; set; }

        public int TipoNotificacao { get; set; }

        public string Alias { get; set; }

        public int Quantidade { get; set; }
    }
}