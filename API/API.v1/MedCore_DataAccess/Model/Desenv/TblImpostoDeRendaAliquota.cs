using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblImpostoDeRendaAliquota
    {
        public tblImpostoDeRendaAliquota()
        {
            tblImpostoDeRenda = new HashSet<tblImpostoDeRenda>();
        }

        public int intTipoAliquota { get; set; }
        public string txtAliquota { get; set; }

        public virtual ICollection<tblImpostoDeRenda> tblImpostoDeRenda { get; set; }
    }
}
