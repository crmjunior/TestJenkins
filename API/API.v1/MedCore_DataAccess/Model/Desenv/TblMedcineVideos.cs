using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedcineVideos
    {
        public int intVideoID { get; set; }
        public DateTime dteDate { get; set; }
        public int intLessonTitleID { get; set; }
        public int intSequence { get; set; }
        public string txtVideoName { get; set; }
        public string txtVideoDescription { get; set; }
        public string txtOBS { get; set; }
        public string txtVideoPassword { get; set; }
        public DateTime? dteDuracao { get; set; }
        public bool? bitEncripted { get; set; }
        public DateTime? dteExpirationDate { get; set; }
        public int? intFileSize { get; set; }
        public string txtBarCode { get; set; }
    }
}
