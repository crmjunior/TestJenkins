using System.Collections.Generic;

namespace MedCore_API.ViewModel.Base
{
    public class NotificacoesClassificacaoViewModel
    {
        public IList<NotificacaoClassificacaoViewModel> NotificacoesClassificacao { get; set; }

        public NotificacaoViewModel NotificacaoDestaque { get; set; }
    }
}