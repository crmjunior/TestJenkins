using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCompanySectors
    {
        public tblCompanySectors()
        {
            tblEmployees = new HashSet<tblEmployees>();
        }

        public int intCompanySectorID { get; set; }
        public string txtDescription { get; set; }
        public int? intResponsibleID { get; set; }
        public int? intSubstituteID { get; set; }

        public virtual tblPersons intResponsible { get; set; }
        public virtual tblPersons intSubstitute { get; set; }
        public virtual ICollection<tblEmployees> tblEmployees { get; set; }
    }
}
