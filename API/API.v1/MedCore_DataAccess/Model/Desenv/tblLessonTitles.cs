using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTitles
    {
        public tblLessonTitles()
        {
            tblProvaVideoIndice = new HashSet<tblProvaVideoIndice>();
        }

        public int intLessonTitleID { get; set; }
        public string txtLessonTitleName { get; set; }
        public int? intLessonSubjectID { get; set; }
        public int? intAno { get; set; }
        public int? intSemana { get; set; }
        public int? intLessonTitleEntityID { get; set; }

        public virtual ICollection<tblProvaVideoIndice> tblProvaVideoIndice { get; set; }
    }
}
