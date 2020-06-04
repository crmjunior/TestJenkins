using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPaymentTemplateData
    {
        public int intID { get; set; }
        public int intPaymentTemplateID { get; set; }
        public int? intSequence { get; set; }
        public DateTime? dteDate { get; set; }
        public double dblValue { get; set; }
        public string txtDescription { get; set; }
        public int intAccountID { get; set; }
        public int intSubscription { get; set; }
        public int intPaymentTypeID { get; set; }
        public double? dblMEDCURSOExClientValue { get; set; }
        public double? dblMEDCURSOExClientPercDisc { get; set; }
        public double? dblMEDCURSOExClientValueDisc { get; set; }
        public bool? bitMEDCURSOExClientSumOfPrevYearsDisc { get; set; }
        public double? dblMEDCURSODiscount { get; set; }
        public double? dblMEDExClientValue { get; set; }
        public double? dblMEDExClientPercDisc { get; set; }
        public double? dblMEDExClientValueDisc { get; set; }
        public bool? bitMEDExClientSumOfPrevYearsDisc { get; set; }
        public double? dblMEDDiscount { get; set; }
        public double? dblCOMBOExClientValue { get; set; }
        public double? dblCOMBOExClientPercDisc { get; set; }
        public double? dblCOMBOExClientValueDisc { get; set; }
        public bool? bitCOMBOExClientSumOfPrevYearsDisc { get; set; }
        public double? dblCOMBODiscount { get; set; }
        public double? dblValueExClient { get; set; }
        public double? dblSpecialDiscountBaseValue { get; set; }
        public double? dblSpecialDiscountBaseValue_ExClient { get; set; }
    }
}
