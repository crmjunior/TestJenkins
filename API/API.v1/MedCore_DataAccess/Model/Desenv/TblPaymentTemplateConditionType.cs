using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPaymentTemplateConditionType
    {
        public int intConditionTypeID { get; set; }
        public string txtDescription { get; set; }
        public bool? bitCheck { get; set; }
    }
}
