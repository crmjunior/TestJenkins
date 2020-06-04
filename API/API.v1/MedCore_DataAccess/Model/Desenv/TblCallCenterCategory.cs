using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCallCenterCategory
    {
        public int intCallCategoryID { get; set; }
        public string txtDescription { get; set; }
        public int intCallGroupID { get; set; }
        public bool? bitActive { get; set; }
        public int? intSolutionDays { get; set; }
    }
}
