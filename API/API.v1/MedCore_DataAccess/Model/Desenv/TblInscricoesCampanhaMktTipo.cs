using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoesCampanhaMktTipo
    {
        public tblInscricoesCampanhaMktTipo()
        {
            tblInscricoesCampanhaMkt = new HashSet<tblInscricoesCampanhaMkt>();
        }

        public int intTipoCampanhaMktID { get; set; }
        public string txtTipo { get; set; }

        public virtual ICollection<tblInscricoesCampanhaMkt> tblInscricoesCampanhaMkt { get; set; }
    }
}
