using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonxRessalva
    {
        public int intLessonID { get; set; }
        public int intLessonRessalvaID { get; set; }
        public int intLessonxRessalvaID { get; set; }
        public DateTime dteDataInicio { get; set; }

        public virtual tblLessons intLesson { get; set; }
        public virtual tblLessonRessalva intLessonRessalva { get; set; }
    }
}
