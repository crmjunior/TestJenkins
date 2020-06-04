using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogGaleriaImagem
    {
        public int intID { get; set; }
        public int? intGaleriaImagemID { get; set; }
        public int? intEmployeeID { get; set; }
        public int? intActionId { get; set; }
        public DateTime? dteDate { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
