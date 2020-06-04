using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCtrlPanel_Relacao
    {
        public int intRelacaoID { get; set; }
        public int? intLinkID { get; set; }
        public int? intResponsabilityID { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
