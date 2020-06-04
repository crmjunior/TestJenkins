using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideo_Book
    {
        public int intBookID { get; set; }
        public string txtVideoCode { get; set; }
        public DateTime dteLastModifyDate { get; set; }
        public DateTime dteCreationDate { get; set; }
        public DateTime? dtePublishDateTime { get; set; }
        public int? intAutorizationEmployeeID { get; set; }
        public DateTime? dteAutorizationDateTime { get; set; }
        public int? intPublishEmployeeID { get; set; }
        public int? intEmployeeID { get; set; }
        public int? intVideoID { get; set; }
        public bool? bitAutoStart { get; set; }
    }
}
