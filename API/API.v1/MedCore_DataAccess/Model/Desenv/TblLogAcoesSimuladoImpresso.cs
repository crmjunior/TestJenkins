using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogAcoesSimuladoImpresso
    {
        public int intID { get; set; }
        public int intAplicationID { get; set; }
        public int intSimuladoID { get; set; }
        public int intAcaoID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteData { get; set; }
    }
}
