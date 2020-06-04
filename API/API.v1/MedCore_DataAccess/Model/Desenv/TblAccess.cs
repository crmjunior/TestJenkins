using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess
    {
        public int intAccessID { get; set; }
        public int intClientID { get; set; }
        public int intYearTypeID { get; set; }
        public int intProductGroupID { get; set; }
        public int? intTotPayments { get; set; }
        public int? intApplicationID { get; set; }
        public int? intScreenID { get; set; }
        public int? intActionID { get; set; }
        public bool bitAccesGranted { get; set; }

        public virtual tblAccess_Action intAction { get; set; }
        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblAccess_Screen intScreen { get; set; }
        public virtual tblAccess_YearType intYearType { get; set; }
    }
}
