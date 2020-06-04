using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLocaisRetiradaMaterial
    {
        public int ID { get; set; }
        public int? intAttributeID { get; set; }
        public int? intStoreId { get; set; }
        public int? intWharehouseID { get; set; }
        public int? intGroupID { get; set; }
        public DateTime? dteDate { get; set; }
        public int? intYear { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
