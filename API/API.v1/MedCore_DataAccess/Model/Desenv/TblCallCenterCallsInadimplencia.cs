using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCallCenterCallsInadimplencia
    {
        public int intCallCenterInadimplenciaID { get; set; }
        public int intCallCenterCallsID { get; set; }
        public int intCallCenterCallsIDRef { get; set; }
        public int intOrderID { get; set; }

        public virtual tblSellOrders intOrder { get; set; }
    }
}
