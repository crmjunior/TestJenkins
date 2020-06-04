using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProvaVideoLogPosition
    {
        public int intLogPositionId { get; set; }
        public int intClientId { get; set; }
        public int intSecond { get; set; }
        public int intProvaVideoId { get; set; }
        public DateTime dteLastUpdate { get; set; }
        public int intLastSecondViewed { get; set; }

        public virtual tblProvaVideo intProvaVideo { get; set; }
    }
}
