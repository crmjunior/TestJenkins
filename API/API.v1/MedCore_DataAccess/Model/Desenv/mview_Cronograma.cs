using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class mview_Cronograma
    {
        public int intLessonID { get; set; }
        public int? intStoreID { get; set; }
        public string txtStoreName { get; set; }
        public int intCourseID { get; set; }
        public int? intSequence { get; set; }
        public int intLessonTitleID { get; set; }
        public DateTime dteDateTime { get; set; }
        public int intDuration { get; set; }
        public int intLessonSubjectID { get; set; }
        public int intClassRoomID { get; set; }
        public int intLessonType { get; set; }
        public bool bitAllowedAccess { get; set; }
        public bool bitAllowedMaterial { get; set; }
        public int? intYear { get; set; }
    }
}
