using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonsEvaluationRevisaoAula
    {
        public int intLessonsEvaluationRaId { get; set; }
        public int intRevisaoAulaIndiceId { get; set; }
        public int intClientId { get; set; }
        public int intNota { get; set; }
        public int intEmployeeId { get; set; }
        public int intApplicationId { get; set; }
        public DateTime dteEvaluation { get; set; }
        public string txtObservacao { get; set; }
        public int? intVideoId { get; set; }
    }
}
