using System;

namespace MedCore_DataAccess.DTO
{
    public class ExercicioHistoricoDTO
    {
        public int intHistoricoExercicioID { get; set; }
        public int intExercicioID { get; set; }
        public int intExercicioTipo { get; set; }
        public System.DateTime dteDateInicio { get; set; }
        public Nullable<DateTime> dteDateFim { get; set; }
        public bool bitRanqueado { get; set; }
        public Nullable<int> intTempoExcedido { get; set; }
        public int intClientID { get; set; }
        public int intApplicationID { get; set; }
        public Nullable<bool> bitRealizadoOnline { get; set; }
        public bool bitPresencial { get; set; }
        public int intVersaoID { get; set; }
        public Nullable<int> intTipoProva { get; set; }
    }
}