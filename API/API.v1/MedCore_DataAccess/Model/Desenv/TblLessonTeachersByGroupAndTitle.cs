using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTeachersByGroupAndTitle
    {
        public int intID { get; set; }
        public int intEmployeeID { get; set; }
        public int intYear { get; set; }
        public int intProductGroupID { get; set; }
        public int intLessonTitleID { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblLessonTitles intLessonTitle { get; set; }
        public virtual tblProductGroups1 intProductGroup { get; set; }
    }
}
