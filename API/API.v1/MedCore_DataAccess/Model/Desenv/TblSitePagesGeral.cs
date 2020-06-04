using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSitePagesGeral
    {
        public int intPageID { get; set; }
        public int? intStoreID { get; set; }
        public DateTime dteStartDate { get; set; }
        public DateTime? dteEndDate { get; set; }
        public string txtPageName { get; set; }
        public string txtPageDescription { get; set; }
        public string txtPage_ImgFileName { get; set; }
        public string txtState { get; set; }
        public int? intInternetOrder { get; set; }
        public string txtCountry { get; set; }
    }
}
