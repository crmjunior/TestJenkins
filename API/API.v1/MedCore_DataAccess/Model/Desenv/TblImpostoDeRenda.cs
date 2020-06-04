using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblImpostoDeRenda
    {
        public int intID { get; set; }
        public int intAnoReferencia { get; set; }
        public int intTipoAliquota { get; set; }
        public decimal dbBaseCalculo { get; set; }
        public decimal dbDeducao { get; set; }

        public virtual tblImpostoDeRendaAliquota intTipoAliquotaNavigation { get; set; }
    }
}
