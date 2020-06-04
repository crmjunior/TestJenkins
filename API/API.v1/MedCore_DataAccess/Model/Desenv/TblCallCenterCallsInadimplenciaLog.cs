using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCallCenterCallsInadimplenciaLog
    {
        public int intCallCenterCallsID { get; set; }
        public int? intAplicativoID { get; set; }
        public DateTime? dteDate { get; set; }

        public virtual tblAccess_Application intAplicativo { get; set; }
    }
}
