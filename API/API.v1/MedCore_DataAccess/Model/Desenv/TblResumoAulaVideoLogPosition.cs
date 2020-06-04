using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblResumoAulaVideoLogPosition
    {
        public int intLogPositionId { get; set; }
        public int intClientId { get; set; }
        public int intSecond { get; set; }
        public int intResumoAulaVideoId { get; set; }
        public DateTime dteLastUpdate { get; set; }
    }
}
