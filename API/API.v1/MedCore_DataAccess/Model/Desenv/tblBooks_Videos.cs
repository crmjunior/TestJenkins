using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBooks_Videos
    {
        public int intMaterialID { get; set; }
        public string txtVideoCode { get; set; }
        public string txtVideoURL { get; set; }
        public bool bitAutoStart { get; set; }
        public DateTime dteUpdate { get; set; }
        public string txtName { get; set; }
        public string txtExtension { get; set; }
        public string txtDescription { get; set; }
        public DateTime? dtePublishDateTime { get; set; }
        public int? intAutorizationEmployeeID { get; set; }
        public DateTime? dteAutorizationDateTime { get; set; }
        public int? intPublishEmployeeID { get; set; }
        public int? intLastEmployeeID { get; set; }
        public bool? bitMedi { get; set; }
    }
}
