using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblCartaoResposta_objetiva_Simulado_Online
    {
        public int intID { get; set; }
        public int intQuestaoID { get; set; }
        public int intHistoricoExercicioID { get; set; }
        public string txtLetraAlternativa { get; set; }
        public Guid guidQuestao { get; set; }
        public int intExercicioTipoId { get; set; }
        public DateTime? dteCadastro { get; set; }

        public virtual tblExercicio_Historico intHistoricoExercicio { get; set; }
    }
}
