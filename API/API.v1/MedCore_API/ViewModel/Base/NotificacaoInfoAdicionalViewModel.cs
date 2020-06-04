using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_API.ViewModel.Base
{
    public class NotificacaoInfoAdicionalViewModel
    {
        public InfoAdicionalSimulado InfoAdicionalSimulado { get; set; }
        public IList<InfoAdicionalAvaliacao> InfoAdicionalAvaliacao { get; set; }
        public InfoAdicionalDuvidasAcademicas InfoAdicionalDuvidasAcademicas { get; set; }
        public string InfoAdicionalLinkExterno { get; set; }
    }
}