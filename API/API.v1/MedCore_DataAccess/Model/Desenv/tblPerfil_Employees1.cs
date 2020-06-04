using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil_Employees1
    {
        public int intPerfilEmployeeID { get; set; }
        public int intPerfilID { get; set; }
        public int intEmployeeID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblPerfil1 intPerfil { get; set; }
    }
}
