using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmed_SessoesAtivas
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteDate { get; set; }
        public string txtSessionID { get; set; }
        public string txtBrowser { get; set; }
        public string txtIP { get; set; }
    }
}
