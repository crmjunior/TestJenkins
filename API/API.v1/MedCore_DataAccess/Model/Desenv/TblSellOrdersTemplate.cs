using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSellOrdersTemplate
    {
        public int intOrderId { get; set; }
        public int? intPaymentTemplateID { get; set; }
        public double? dblValorFrete { get; set; }

        public virtual tblSellOrders intOrder { get; set; }
    }
}
