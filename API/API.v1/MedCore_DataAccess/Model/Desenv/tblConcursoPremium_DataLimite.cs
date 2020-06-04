using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoPremium_DataLimite
    {
        public int id { get; set; }
        public int intIDPremium { get; set; }
        public int intYear { get; set; }
        public DateTime dteDataLimite { get; set; }
    }
}
