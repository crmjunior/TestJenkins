using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogMesesBlocoMaterialAnteriorAvulso
    {
        public int intID { get; set; }
        public int intOrderID { get; set; }
        public int intPaymentDocumentID { get; set; }
        public DateTime dteDate { get; set; }
        public int intMes { get; set; }
        public int intProductGroupID { get; set; }
        public int? intEmployeeID { get; set; }

        public virtual tblSellOrders intOrder { get; set; }
        public virtual tblPaymentDocuments intPaymentDocument { get; set; }
    }
}
