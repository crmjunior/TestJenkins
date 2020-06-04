using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblIntensivaoLog
    {
        public int intIntensivaoLogID { get; set; }
        public int? intClientID { get; set; }
        public int intMarcadorID { get; set; }
        public string txtPerfil { get; set; }
        public int? intProductID { get; set; }
        public int? intFilialID { get; set; }
        public DateTime dteTimeStamp { get; set; }
        public long intSessionID { get; set; }
    }
}
