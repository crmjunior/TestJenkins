using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonsEvaluation
    {
        public int intEvaluationID { get; set; }
        public int? intBookID { get; set; }
        public int? intLessonID { get; set; }
        public int? intClassroomID { get; set; }
        public int? intClientID { get; set; }
        public int? intNota { get; set; }
        public int? intEmployeedID { get; set; }
        public int? intProductGroup1ID { get; set; }
        public int? intApplicationID { get; set; }
        public DateTime? dteEvaluationDate { get; set; }
        public string txtObservacao { get; set; }

        public virtual tblAccess_Application intApplication { get; set; }
        public virtual tblBooks intBook { get; set; }
        public virtual tblClassRooms intClassroom { get; set; }
        public virtual tblPersons intClient { get; set; }
        public virtual tblPersons intEmployeed { get; set; }
        public virtual tblLessons intLesson { get; set; }
        public virtual tblProductGroups1 intProductGroup1 { get; set; }
    }
}
