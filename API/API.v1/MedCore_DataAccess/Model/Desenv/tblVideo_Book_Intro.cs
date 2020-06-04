using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideo_Book_Intro
    {
        public int intBookID { get; set; }
        public DateTime dteLastModifyDate { get; set; }
        public DateTime dteCreationDate { get; set; }
        public int? intEmployeeID { get; set; }
        public int intVideoID { get; set; }

        public virtual tblBooks intBook { get; set; }
    }
}
