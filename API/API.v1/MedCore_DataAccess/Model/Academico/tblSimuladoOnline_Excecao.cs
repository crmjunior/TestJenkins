using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoOnline_Excecao
    {
        public int intSimuladoExcecaoID { get; set; }
        public int intSimuladoID { get; set; }
        public int intClientID { get; set; }
        public int intCourseID { get; set; }
        public DateTime? dteDataHoraInicioWEB { get; set; }
        public DateTime? dteDataHoraTerminoWEB { get; set; }
        public bool bitRefazer { get; set; }
        public DateTime? dteInicioConsultaRanking { get; set; }
    }
}
