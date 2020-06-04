using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblLogSimuladoAlunoTurma
    {
        public int? intSimuladoID { get; set; }
        public int? intClientID { get; set; }
        public int? intOrderID { get; set; }
        public string txtUnidade { get; set; }
        public int? intState { get; set; }
        public string txtTurma { get; set; }
        public string txtEspecialidade { get; set; }
    }
}
