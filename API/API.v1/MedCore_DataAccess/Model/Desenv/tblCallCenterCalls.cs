using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCallCenterCalls
    {
        public int intCallCenterCallsID { get; set; }
        public DateTime dteOpen { get; set; }
        public int intStatusID { get; set; }
        public int intClientID { get; set; }
        public int intCallGroupID { get; set; }
        public int intCallCategoryID { get; set; }
        public string txtSubject { get; set; }
        public string txtDetail { get; set; }
        public string txtFile { get; set; }
        public int intSeverity { get; set; }
        public bool bitNotify { get; set; }
        public int? intCallSectorID { get; set; }
        public DateTime? dteSolutionDays { get; set; }
        public int? intSectorComplementID { get; set; }
        public int? intStatusInternoID { get; set; }
        public int? intFirstEmployeeID { get; set; }
        public int? intLastEmployeeID { get; set; }
        public DateTime? dteDataPrevisao1 { get; set; }
        public DateTime? dteDataPrevisao2 { get; set; }
        public int intCourseID { get; set; }
        public int? intDepartmentID { get; set; }
    }
}
