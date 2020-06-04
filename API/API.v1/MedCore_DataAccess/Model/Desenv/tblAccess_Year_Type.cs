using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Year_Type
    {
        public tblAccess_Year_Type()
        {
            tblAccess_YearTypeYear = new HashSet<tblAccess_YearTypeYear>();
        }

        public int intTipoAnoId { get; set; }
        public int intAno { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblAccess_YearTypeYear> tblAccess_YearTypeYear { get; set; }
    }
}
