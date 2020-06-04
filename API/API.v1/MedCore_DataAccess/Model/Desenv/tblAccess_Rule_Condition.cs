using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Rule_Condition
    {
        public int intRuleConditionID { get; set; }
        public int intRuleID { get; set; }
        public int intConditionID { get; set; }

        public virtual tblAccess_Condition intCondition { get; set; }
    }
}
