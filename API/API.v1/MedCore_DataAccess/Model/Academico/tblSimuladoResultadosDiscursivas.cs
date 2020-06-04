using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoResultadosDiscursivas
    {
        public int intSimuladoID { get; set; }
        public int intClientID { get; set; }
        public int? intAcertos { get; set; }
        public double? dblNota { get; set; }
        public DateTime dteDateTime { get; set; }

        public virtual tblSimulado intSimulado { get; set; }
    }
}
