using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSellOrderDetails
    {
        public int intOrderRecordID { get; set; }
        public int intOrderID { get; set; }
        public int intProductID { get; set; }
        public double dblAmount { get; set; }
        public double? dblPrice { get; set; }
        public int? intMaterialID { get; set; }

        public virtual tblSellOrders intOrder { get; set; }
        public virtual tblProducts intProduct { get; set; }
    }
}
