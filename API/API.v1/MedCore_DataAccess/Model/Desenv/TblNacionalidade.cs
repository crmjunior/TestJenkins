using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNacionalidade
    {
        public int intNacionalidadeID { get; set; }
        public string txtNacionalidade { get; set; }
        public int? intCodigoSefaz { get; set; }
    }
}
