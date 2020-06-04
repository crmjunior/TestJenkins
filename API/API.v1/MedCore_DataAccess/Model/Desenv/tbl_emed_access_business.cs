using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tbl_emed_access_business
    {
        public int intAccessBusinessId { get; set; }
        public int intModuleItemId { get; set; }
        public string txtItemDetail { get; set; }
        public int intClientID { get; set; }
        public string txtIP { get; set; }
        public DateTime dteDateTime { get; set; }
    }
}
