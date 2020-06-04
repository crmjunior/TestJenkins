using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccountData
    {
        public int intAccountingEntryID { get; set; }
        public int intCreditAccountID { get; set; }
        public int intDebitAccountID { get; set; }
        public DateTime dteDate { get; set; }
        public double dblValue { get; set; }
        public int intStatusID { get; set; }
        public int intDocumentID { get; set; }
        public string txtComment { get; set; }
        public double? dblFine { get; set; }
        public double? dblInterest { get; set; }
        public double? dblDiscount { get; set; }
        public int? intStoreID { get; set; }
        public int? intOrderID { get; set; }
        public int intPaymentOptionID { get; set; }
        public int? intPaymentOptionIDSec { get; set; }
        public int intSequence { get; set; }

        public virtual tblStores intStore { get; set; }
    }
}
