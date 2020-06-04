using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRegions
    {
        public tblRegions()
        {
            tblStates = new HashSet<tblStates>();
        }

        public int intRegionID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblStates> tblStates { get; set; }
    }
}
