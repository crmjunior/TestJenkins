using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCronogramaPrateleira_LessonTitles
    {
        public int intID { get; set; }
        public int intPrateleiraCronogramaID { get; set; }
        public int intLessonTitleID { get; set; }

        public virtual tblLessonTitles intLessonTitle { get; set; }
        public virtual tblCronogramaPrateleira intPrateleiraCronograma { get; set; }
    }
}
