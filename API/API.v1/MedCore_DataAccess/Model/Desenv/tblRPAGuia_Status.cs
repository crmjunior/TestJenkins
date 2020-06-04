using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPAGuia_Status
    {
        public tblRPAGuia_Status()
        {
            tblRPAGuia = new HashSet<tblRPAGuia>();
            tblRPAGuia_Status_Historico = new HashSet<tblRPAGuia_Status_Historico>();
        }

        public int intStatusID { get; set; }
        public string txtStatus { get; set; }

        public virtual ICollection<tblRPAGuia> tblRPAGuia { get; set; }
        public virtual ICollection<tblRPAGuia_Status_Historico> tblRPAGuia_Status_Historico { get; set; }
    }
}
