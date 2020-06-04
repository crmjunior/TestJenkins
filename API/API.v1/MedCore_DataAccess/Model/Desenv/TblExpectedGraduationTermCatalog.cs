using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblExpectedGraduationTermCatalog
    {
        public tblExpectedGraduationTermCatalog()
        {
            tblClients = new HashSet<tblClients>();
        }

        public int intGraduationPeriodID { get; set; }
        public string txtDescription { get; set; }
        public int intYear { get; set; }
        public int intSemester { get; set; }

        public virtual ICollection<tblClients> tblClients { get; set; }
    }
}
