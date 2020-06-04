using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmailNotificacaoDuvidasAcademicas
    {
        public int intEmailDuvidasID { get; set; }
        public int? intContactID { get; set; }
        public string txtEmail { get; set; }
        public string txtName { get; set; }

        public virtual tblPersons intContact { get; set; }
    }
}
