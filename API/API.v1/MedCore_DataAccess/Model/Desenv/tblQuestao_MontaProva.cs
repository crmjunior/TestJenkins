using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_MontaProva
    {
        public int intID { get; set; }
        public int? intProvaId { get; set; }
        public int intQuestaoId { get; set; }
        public int? intTipoExercicioId { get; set; }
        public int? intFiltroId { get; set; }

        public virtual tblExercicio_MontaProva intProva { get; set; }
    }
}
