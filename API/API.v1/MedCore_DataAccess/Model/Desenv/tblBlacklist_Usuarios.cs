using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBlacklist_Usuarios
    {
        public int intBlackListPessoasID { get; set; }
        public int intClientID { get; set; }
        public string txtName { get; set; }
    }
}
