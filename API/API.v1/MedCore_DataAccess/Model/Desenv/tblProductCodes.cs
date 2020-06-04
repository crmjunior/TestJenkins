using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProductCodes
    {
        public int intID { get; set; }
        public int intProductId { get; set; }
        public int intApplicationId { get; set; }
        public string txtCode { get; set; }
        public int intLessonTitleID { get; set; }
    }
}
