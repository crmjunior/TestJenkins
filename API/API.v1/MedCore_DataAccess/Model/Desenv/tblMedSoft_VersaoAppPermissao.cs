using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedSoft_VersaoAppPermissao
    {
        public int intId { get; set; }
        public string txtVersaoApp { get; set; }
        public int? intProdutoId { get; set; }
        public bool? bitBloqueio { get; set; }
    }
}
