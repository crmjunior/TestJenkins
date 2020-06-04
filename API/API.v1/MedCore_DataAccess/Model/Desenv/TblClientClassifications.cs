using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClientClassifications
    {
        public int intPersonID { get; set; }
        public int intClassificationID { get; set; }
        public int intAttributeID { get; set; }
        public DateTime? dteDate { get; set; }
        public int intEmployeeID { get; set; }

        public virtual tblPersons intPerson { get; set; }
    }
}
