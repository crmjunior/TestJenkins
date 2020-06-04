using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoesCampanhaMkt
    {
        public int intInscricoesCampanhaMktID { get; set; }
        public int? intMatricula { get; set; }
        public int? intOrdemVendaID { get; set; }
        public int intTipoCampanhaMkt { get; set; }
        public DateTime dteData { get; set; }

        public virtual tblInscricoesCampanhaMktTipo intTipoCampanhaMktNavigation { get; set; }
    }
}
