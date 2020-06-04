using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSysRoles
    {
        public tblSysRoles()
        {
            tblEmployees = new HashSet<tblEmployees>();
        }

        public int intResponsabilityID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblEmployees> tblEmployees { get; set; }
    }
}
