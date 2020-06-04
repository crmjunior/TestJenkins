using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoOnline_Consolidado
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public int intSimuladoID { get; set; }
        public int intErradas { get; set; }
        public int intCertas { get; set; }
        public int intNaoRealizadas { get; set; }

        public virtual tblSimulado intSimulado { get; set; }
    }
}
