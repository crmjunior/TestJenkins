using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestaoAlternativas
    {
        public int intQuestaoID { get; set; }
        public string txtLetraAlternativa { get; set; }
        public string txtAlternativa { get; set; }
        public bool? bitCorreta { get; set; }
        public string txtResposta { get; set; }
        public int intAlternativaID { get; set; }
    }
}
