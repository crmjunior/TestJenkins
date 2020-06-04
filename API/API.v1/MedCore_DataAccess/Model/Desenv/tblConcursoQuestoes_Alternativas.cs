using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoes_Alternativas
    {
        public int intAlternativaID { get; set; }
        public int intQuestaoID { get; set; }
        public int? intQuestaoIDOld { get; set; }
        public string txtLetraAlternativa { get; set; }
        public string txtAlternativa { get; set; }
        public bool? bitCorreta { get; set; }
        public bool? bitCorretaPreliminar { get; set; }
        public string txtResposta { get; set; }
        public string txtImagem { get; set; }
        public string txtImagemOtimizada { get; set; }

        public virtual tblConcursoQuestoes intQuestao { get; set; }
    }
}
