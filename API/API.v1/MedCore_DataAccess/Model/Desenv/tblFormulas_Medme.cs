using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblFormulas_Medme
    {
        public int intID { get; set; }
        public int intFormulaID { get; set; }
        public int intPrescricaoID { get; set; }
        public string txtFormula { get; set; }
        public string txtCondicao { get; set; }
    }
}
