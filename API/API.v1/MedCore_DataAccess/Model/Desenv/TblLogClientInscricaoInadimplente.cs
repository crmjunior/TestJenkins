using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogClientInscricaoInadimplente
    {
        public int intLogID { get; set; }
        public int intClientID { get; set; }
        public DateTime DataInclusao { get; set; }
        public int? SellOrderID { get; set; }
        public DateTime? DataSellOrder { get; set; }
        public int? intProductGroup1 { get; set; }
    }
}
