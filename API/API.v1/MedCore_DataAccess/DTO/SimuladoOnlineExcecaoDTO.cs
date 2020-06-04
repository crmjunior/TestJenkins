using System;

namespace MedCore_DataAccess.DTO
{
    public class SimuladoOnlineExcecaoDTO
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