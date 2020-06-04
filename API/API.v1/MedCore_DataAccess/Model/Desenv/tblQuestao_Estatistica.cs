using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Estatistica
    {
        public int intExercicioTipo { get; set; }
        public int intQuestaoID { get; set; }
        public string txtLetraAlternativa { get; set; }
        public int? intCountTotal { get; set; }
        public bool? bitCorreta { get; set; }
        public decimal? dblPercent { get; set; }
    }
}
