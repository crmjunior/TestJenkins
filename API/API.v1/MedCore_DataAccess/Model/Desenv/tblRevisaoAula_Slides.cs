using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAula_Slides
    {
        public int intSlideAulaID { get; set; }
        public int intLessonTitleID { get; set; }
        public int intProfessorID { get; set; }
        public int intOrder { get; set; }
        public Guid guidSlide { get; set; }

        public virtual tblLessonTitles intLessonTitle { get; set; }
        public virtual tblEmployees intProfessor { get; set; }
    }
}
