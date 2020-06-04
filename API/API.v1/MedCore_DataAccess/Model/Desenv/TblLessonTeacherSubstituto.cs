using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTeacherSubstituto
    {
        public int intID { get; set; }
        public int? intLessonID { get; set; }
        public int? intEmployeeID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblLessons intLesson { get; set; }
    }
}
