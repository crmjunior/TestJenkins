using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessons
    {
        public tblLessons()
        {
            tblLessonRessalva = new HashSet<tblLessonRessalva>();
            tblLessonTeacherSubstituto = new HashSet<tblLessonTeacherSubstituto>();
            tblLesson_Material = new HashSet<tblLesson_Material>();
            tblLessonsEvaluation = new HashSet<tblLessonsEvaluation>();
            tblLessonxRessalva = new HashSet<tblLessonxRessalva>();
        }

        public int intLessonID { get; set; }
        public int intCourseID { get; set; }
        public int? intSequence { get; set; }
        public int intLessonTitleID { get; set; }
        public DateTime dteDateTime { get; set; }
        public int intDuration { get; set; }
        public int intLessonSubjectID { get; set; }
        public int intClassRoomID { get; set; }
        public int bitInvitedCoursesReplication { get; set; }
        public int intLessonType { get; set; }

        public virtual tblClassRooms intClassRoom { get; set; }
        public virtual tblCourses intCourse { get; set; }
        public virtual tblLessonTitles intLessonTitle { get; set; }
        public virtual ICollection<tblLessonRessalva> tblLessonRessalva { get; set; }
        public virtual ICollection<tblLessonTeacherSubstituto> tblLessonTeacherSubstituto { get; set; }
        public virtual ICollection<tblLesson_Material> tblLesson_Material { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
        public virtual ICollection<tblLessonxRessalva> tblLessonxRessalva { get; set; }
    }
}
