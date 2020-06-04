using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblApostilaAddOn
    {
        public int intApostilaAddOnID { get; set; }
        public string txtPosicao { get; set; }
        public string txtConteudo { get; set; }
        public int intApostilaID { get; set; }

        public virtual tblBooks intApostila { get; set; }
    }
}
