using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTypes
    {
        public int intLessonType { get; set; }
        public string txtName { get; set; }
        public string txtDescription { get; set; }
        public bool bitAulaPresencialMedicine { get; set; }
    }
}
