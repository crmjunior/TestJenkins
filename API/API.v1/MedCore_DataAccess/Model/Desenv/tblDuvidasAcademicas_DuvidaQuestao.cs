using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_DuvidaQuestao
    {
        public int intDuvidaQuestaoId { get; set; }
        public int intDuvidaID { get; set; }
        public int intQuestaoId { get; set; }
        public int intTipoQuestao { get; set; }
        public int? intExercicioId { get; set; }
        public int intTipoExercicioID { get; set; }
        public int? intNumQuestao { get; set; }
        public int? intQuestaoConcursoID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public string txtOrigemQuestaoConcurso { get; set; }

        public virtual tblDuvidasAcademicas_Duvidas tblDuvidasAcademicas_Duvidas { get; set; }
        public virtual tblEspecialidades intEspecialidade { get; set; }
    }
}
