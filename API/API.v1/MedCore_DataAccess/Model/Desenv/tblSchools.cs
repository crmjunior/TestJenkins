using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSchools
    {
        public tblSchools()
        {
            tblClients = new HashSet<tblClients>();
            tblClients_BlackListPre = new HashSet<tblClients_BlackListPre>();
        }

        public int intSchoolID { get; set; }
        public string txtName { get; set; }
        public string txtSigla { get; set; }
        public int intStateId { get; set; }

        public virtual ICollection<tblClients> tblClients { get; set; }
        public virtual ICollection<tblClients_BlackListPre> tblClients_BlackListPre { get; set; }
    }
}
