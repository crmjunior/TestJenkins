using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStore_CombosPaymentTemplate
    {
        public int intStoreID { get; set; }
        public int intComboID { get; set; }
        public int intPaymentTemplateID { get; set; }
        public bool? bitActive { get; set; }
        public bool? bitInternet { get; set; }
        public bool? bitProducao { get; set; }

        public virtual tblStores intStore { get; set; }
    }
}
