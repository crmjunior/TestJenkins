using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_Ativo_Historico
    {
        public int intID { get; set; }
        public DateTime? dteData { get; set; }
        public int intRPAID { get; set; }
        public string txtMudancas { get; set; }
        public int intTipo { get; set; }
        public int? intEmployeeID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblRPA intRPA { get; set; }
    }
}
