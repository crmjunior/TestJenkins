using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_Questao_Especialidade
    {
        public int intExercicioTipo { get; set; }
        public int intQuestaoID { get; set; }
        public Guid guidQuestaoID { get; set; }
        public int intEspecialidadeID { get; set; }
    }
}
