using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_DuvidasEncaminhadas
    {
        public int intDuvidaEncaminhadaID { get; set; }
        public int? intDuvidaID { get; set; }
        public int? intGestorID { get; set; }
        public int? intEmployeeID { get; set; }
        public DateTime? dteDataEncaminhamento { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblEmployees intGestor { get; set; }
        
    }
}
