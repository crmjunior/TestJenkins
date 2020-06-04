using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClassificationAttributes
    {
        public int intAttributeID { get; set; }
        public string txtDescription { get; set; }
        public int intClassificationID { get; set; }
    }
}
