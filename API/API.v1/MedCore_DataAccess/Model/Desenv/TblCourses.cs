using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCourses
    {
        public tblCourses()
        {
            tblLessons = new HashSet<tblLessons>();
        }

        public int intCourseID { get; set; }
        public int? intYear { get; set; }
        public int? intPrincipalClassRoomID { get; set; }
        public int? intOriginalCapacity { get; set; }
        public int? intOverbookingCapacity { get; set; }
        public bool? bitZeraVagasDisponiveis { get; set; }
        public DateTime? dteStartDateTime { get; set; }
        public DateTime? dteEndDateTime { get; set; }
        public bool? bitQuorumMinimo { get; set; }
        public int? intQuorumMinimo { get; set; }
        public int? intFixedChairs { get; set; }
        public int? intExtraChairs { get; set; }
        public int? intLastEmployeeID { get; set; }

        public virtual tblProducts intCourse { get; set; }
        public virtual tblClassRooms intPrincipalClassRoom { get; set; }
        public virtual ICollection<tblLessons> tblLessons { get; set; }
    }
}
