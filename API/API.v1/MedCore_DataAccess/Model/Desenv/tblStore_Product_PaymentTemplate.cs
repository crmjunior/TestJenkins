using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStore_Product_PaymentTemplate
    {
        public int intStoreID { get; set; }
        public int intProductID { get; set; }
        public int intPaymentOptionID { get; set; }
        public bool? bitActive { get; set; }
        public bool? bitInternet { get; set; }
        public bool? bitProducao { get; set; }
    }
}
