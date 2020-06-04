using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClients_BlackListPre
    {
        public int intClientBlackListID { get; set; }
        public int? intClientID { get; set; }
        public string txtRegister { get; set; }
        public string txtNome { get; set; }
        public string txtEmail { get; set; }
        public int? intSchoolID { get; set; }
        public string txtFaculdade { get; set; }

        public virtual tblSchools intSchool { get; set; }
    }
}
