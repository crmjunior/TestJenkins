using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoftScreenshotReport
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteCriacao { get; set; }
        public int? intApplicationId { get; set; }
        public string txtDeviceID { get; set; }
        public int? intScreenshotCounter { get; set; }

        public virtual tblPersons intClient { get; set; }
    }
}
