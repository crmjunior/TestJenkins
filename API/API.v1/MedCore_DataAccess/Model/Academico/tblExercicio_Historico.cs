using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblExercicio_Historico
    {
        public tblExercicio_Historico()
        {
            tblCartaoResposta_Discursiva = new HashSet<tblCartaoResposta_Discursiva>();
            tblCartaoResposta_objetiva = new HashSet<tblCartaoResposta_objetiva>();
            tblCartaoResposta_objetiva_Simulado_Online = new HashSet<tblCartaoResposta_objetiva_Simulado_Online>();
        }

        public int intHistoricoExercicioID { get; set; }
        public int intExercicioID { get; set; }
        public int intExercicioTipo { get; set; }
        public DateTime dteDateInicio { get; set; }
        public DateTime? dteDateFim { get; set; }
        public bool bitRanqueado { get; set; }
        public int? intTempoExcedido { get; set; }
        public int intClientID { get; set; }
        public int intApplicationID { get; set; }
        public bool? bitRealizadoOnline { get; set; }
        public bool bitPresencial { get; set; }
        public int intVersaoID { get; set; }
        public int? intTipoProva { get; set; }

        public virtual ICollection<tblCartaoResposta_Discursiva> tblCartaoResposta_Discursiva { get; set; }
        public virtual ICollection<tblCartaoResposta_objetiva> tblCartaoResposta_objetiva { get; set; }
        public virtual ICollection<tblCartaoResposta_objetiva_Simulado_Online> tblCartaoResposta_objetiva_Simulado_Online { get; set; }
    }
}
