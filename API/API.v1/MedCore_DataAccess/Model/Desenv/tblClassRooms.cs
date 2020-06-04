using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClassRooms
    {
        public tblClassRooms()
        {
            tblCourses = new HashSet<tblCourses>();
            //tblLessonTeachersByClassRoom = new HashSet<tblLessonTeachersByClassRoom>();
            tblLessons = new HashSet<tblLessons>();
            tblLessonsEvaluation = new HashSet<tblLessonsEvaluation>();
        }

        public int intClassRoomID { get; set; }
        public int intCapacity { get; set; }
        public string txtDescription { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtZipCode { get; set; }
        public int? intCompanyID { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblCompanies intCompany { get; set; }
        public virtual tblWarehousesClassRooms tblWarehousesClassRooms { get; set; }
        public virtual ICollection<tblCourses> tblCourses { get; set; }
        //public virtual ICollection<tblLessonTeachersByClassRoom> tblLessonTeachersByClassRoom { get; set; }
        public virtual ICollection<tblLessons> tblLessons { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
    }
}
