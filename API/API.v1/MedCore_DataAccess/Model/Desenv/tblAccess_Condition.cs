using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Condition
    {
        public tblAccess_Condition()
        {
            tblAccess_Rule_Condition = new HashSet<tblAccess_Rule_Condition>();
        }

        public int intConditionID { get; set; }
        public int intProductGroupID { get; set; }
        public int intYearTypeID { get; set; }
        public int intStatus { get; set; }

        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblAccess_YearType intYearType { get; set; }
        public virtual ICollection<tblAccess_Rule_Condition> tblAccess_Rule_Condition { get; set; }
    }
}
