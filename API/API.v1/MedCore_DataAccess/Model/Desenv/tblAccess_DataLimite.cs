using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_DataLimite
    {
        public int intDataLimiteID { get; set; }
        public int intAplicationID { get; set; }
        public DateTime dteDataLimite { get; set; }
        public int intAlunoYear { get; set; }
    }
}
