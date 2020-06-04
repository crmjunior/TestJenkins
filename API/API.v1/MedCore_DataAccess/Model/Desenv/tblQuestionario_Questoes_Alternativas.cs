using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestionario_Questoes_Alternativas
    {
        public int intID { get; set; }
        public int? intQuestionarioQuestoesID { get; set; }
        public int? intQuestionarioID { get; set; }
        public int? intAlternativa { get; set; }
        public string txtAlternativa { get; set; }

        public virtual tblQuestionario intQuestionario { get; set; }
    }
}
