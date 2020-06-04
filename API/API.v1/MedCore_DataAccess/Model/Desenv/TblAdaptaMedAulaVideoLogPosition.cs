using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAdaptaMedAulaVideoLogPosition
    {
        public int intLogPositionId { get; set; }
        public int intClientId { get; set; }
        public int intSecond { get; set; }
        public int intAdaptaMedAulaVideoId { get; set; }
        public DateTime dteLastUpdate { get; set; }
    }
}
