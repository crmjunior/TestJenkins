using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class NotificacaoInfoAdicional
    {
        public InfoAdicionalSimulado InfoAdicionalSimulado { get; set; }
        public IList<InfoAdicionalAvaliacao> InfoAdicionalAvaliacao { get; set; }

        public InfoAdicionalDuvidasAcademicas InfoAdicionalDuvidasAcademicas { get; set; }

        public string InfoAdicionalLinkExterno { get; set; }
    }
}