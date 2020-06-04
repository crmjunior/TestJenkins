using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblParametrosGenericos
    {
        public int intID { get; set; }
        public string txtModule { get; set; }
        public string txtName { get; set; }
        public string txtValue { get; set; }
        public bool? bitMGEWeb { get; set; }
    }
}
