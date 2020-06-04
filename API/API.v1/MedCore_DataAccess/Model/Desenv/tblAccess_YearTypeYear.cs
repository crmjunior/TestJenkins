using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_YearTypeYear
    {
        public int intAno_TipoAnoID { get; set; }
        public int intTipoAnoID { get; set; }
        public int intAno { get; set; }

        public virtual tblAccess_Year_Type intTipoAno { get; set; }
    }
}
