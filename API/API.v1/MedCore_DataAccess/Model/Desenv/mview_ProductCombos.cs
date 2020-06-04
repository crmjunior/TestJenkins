using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class mview_ProductCombos
    {
        public int intComboID { get; set; }
        public string txtDescription { get; set; }
        public bool? bitActive { get; set; }
        public int intProductID { get; set; }
    }
}
