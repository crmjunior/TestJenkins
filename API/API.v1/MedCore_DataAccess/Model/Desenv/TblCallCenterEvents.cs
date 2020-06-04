using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCallCenterEvents
    {
        public int intCallEventsID { get; set; }
        public int intCallCenterCallsID { get; set; }
        public DateTime? dteDate { get; set; }
        public bool? bitInternalInformation { get; set; }
        public int? intCallStatusID { get; set; }
        public string txtSubject { get; set; }
        public string txtDetails { get; set; }
        public int? intEmployeeID { get; set; }
        public int? intSeverityID { get; set; }
        public string txtFilename { get; set; }
        public int? intSectorComplementID { get; set; }
        public int? intSectorID { get; set; }
        public int? intStatusInternoID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
