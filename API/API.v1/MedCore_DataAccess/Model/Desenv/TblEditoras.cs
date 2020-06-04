using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEditoras
    {
        public int ID { get; set; }
        public string txtNome { get; set; }
        public string txtEmail { get; set; }
        public string txtTelefone { get; set; }
        public int? intStoreID { get; set; }
        public string txtSite { get; set; }
    }
}
