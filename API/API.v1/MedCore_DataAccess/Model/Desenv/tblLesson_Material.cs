using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLesson_Material
    {
        public int intLessonID { get; set; }
        public int intMaterialID { get; set; }

        public virtual tblLessons intLesson { get; set; }
        public virtual tblProducts intMaterial { get; set; }
    }
}
