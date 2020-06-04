using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_CacheConfig
    {
        public int intID { get; set; }
        public string txtNome { get; set; }
        public int intHoras { get; set; }
        public int? intMinutos { get; set; }
    }
}
