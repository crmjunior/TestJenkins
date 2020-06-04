using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTeachersByClassRoom
    {
        public int intID { get; set; }
        public int intEmployeeID { get; set; }
        public int intClassRoomID { get; set; }

        public virtual tblClassRooms intClassRoom { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
    }
}
