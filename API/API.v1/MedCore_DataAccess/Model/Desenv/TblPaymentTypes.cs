using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPaymentTypes
    {
        public int intPaymentTypeID { get; set; }
        public string txtDescription { get; set; }
        public int? intAccountID { get; set; }
        public int intPaymentMethodID { get; set; }
        public string txtCodigoEscritural { get; set; }
        public string txtCarteira { get; set; }
        public int? intPaymentTemplateProdutoID { get; set; }
    }
}
