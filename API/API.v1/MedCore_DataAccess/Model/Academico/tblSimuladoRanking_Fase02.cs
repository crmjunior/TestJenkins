using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoRanking_Fase02
    {
        public int? intSimuladoID { get; set; }
        public string txtPosicao { get; set; }
        public int? intAcertos { get; set; }
        public double? dblNotaProvaDiscursiva { get; set; }
        public double? dblNotaObjetiva { get; set; }
        public double? dblNotaDiscursiva { get; set; }
        public double? dblNotaFinal { get; set; }
        public int? intClientID { get; set; }
        public string txtUnidade { get; set; }
        public string txtLocal { get; set; }
        public string txtName { get; set; }
        public string txtEspecialidade { get; set; }
        public int? intStateID { get; set; }
    }
}
