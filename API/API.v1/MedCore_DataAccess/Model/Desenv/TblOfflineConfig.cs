using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblOfflineConfig
    {
        public int intID { get; set; }
        public string txtDescricao { get; set; }
        public int intMinutos { get; set; }
    }
}
