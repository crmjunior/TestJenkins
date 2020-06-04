using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_PersonalException
    {
        public int intPersonalExceptionID { get; set; }
        public string txtDescription { get; set; }
        public int intClientId { get; set; }
        public int intYearTypeID { get; set; }
        public int intScreenActionID { get; set; }
        public int intProductGroupID { get; set; }
        public int intApplicationId { get; set; }
        public DateTime? dteStart { get; set; }
        public DateTime? dteEnd { get; set; }
        public bool bitAccessGranted { get; set; }

        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblAccess_Screen_Action intScreenAction { get; set; }
        public virtual tblAccess_YearType intYearType { get; set; }
    }
}
