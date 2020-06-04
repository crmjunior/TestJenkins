using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonsTotalEvaluationAuxiliar
    {
        public int intId { get; set; }
        public int? intEmployeeId { get; set; }
        public int? intNota1 { get; set; }
        public int? intNota2 { get; set; }
        public int? intNota3 { get; set; }
        public int? intNota4 { get; set; }
        public int? intLessonTitleId { get; set; }
    }
}
