using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccessLog
    {
        public int intPeopleID { get; set; }
        public DateTime dteDateTime { get; set; }
        public bool bitEntrada { get; set; }
        public int intEmployeeID { get; set; }
        public int? intClassroomID { get; set; }
        public bool? bitAccess { get; set; }
        public string txtComment { get; set; }
        public int? intAccessType { get; set; }
        public string txtOpCode { get; set; }
    }
}
