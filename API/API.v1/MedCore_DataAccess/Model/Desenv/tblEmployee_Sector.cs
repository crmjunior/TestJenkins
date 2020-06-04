using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmployee_Sector
    {
        public int intEmployeeID { get; set; }
        public int intSectorID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
