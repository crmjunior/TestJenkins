using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedmeVideoLogPosition
    {
        public int intLogPositionId { get; set; }
        public int intClientId { get; set; }
        public int intSecond { get; set; }
        public int intPercent { get; set; }
        public int intMedmeVideoId { get; set; }
        public DateTime dteLastUpdate { get; set; }
    }
}
