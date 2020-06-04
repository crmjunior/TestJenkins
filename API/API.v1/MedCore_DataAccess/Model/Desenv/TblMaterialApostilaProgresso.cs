using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaProgresso
    {
        public int intID { get; set; }
        public int? intApostilaID { get; set; }
        public int? intClientID { get; set; }
        public decimal dblPercentProgresso { get; set; }
        public DateTime dteDataAlteracao { get; set; }

        public virtual tblMaterialApostila intApostila { get; set; }
        public virtual tblPersons intClient { get; set; }
    }
}
