using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLog_PrintApostila
    {
        public int intID { get; set; }
        public string cpf { get; set; }
        public DateTime data { get; set; }
        public int? apostila { get; set; }
        public int? pagina { get; set; }
    }
}
