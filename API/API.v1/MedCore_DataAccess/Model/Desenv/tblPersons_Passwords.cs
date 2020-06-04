using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPersons_Passwords
    {
        public int intID { get; set; }
        public int intContactID { get; set; }
        public string txtPassword { get; set; }
        public DateTime dteDatePassword { get; set; }
        public int? intChave { get; set; }
        public DateTime? dteDataLimite { get; set; }
        public int? intAplicacaoId { get; set; }
    }
}
