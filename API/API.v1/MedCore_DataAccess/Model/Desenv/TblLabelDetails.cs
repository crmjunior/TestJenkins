using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLabelDetails
    {
        public int intLabelDetailID { get; set; }
        public int intLabelID { get; set; }
        public int intObjetoID { get; set; }
        public bool? bitPadrao { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblLabels intLabel { get; set; }
    }
}
