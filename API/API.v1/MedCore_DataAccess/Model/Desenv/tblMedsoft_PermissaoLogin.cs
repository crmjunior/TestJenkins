using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_PermissaoLogin
    {
        public int intID { get; set; }
        public int? intClientID { get; set; }
        public DateTime? dteCadastro { get; set; }
    }
}
