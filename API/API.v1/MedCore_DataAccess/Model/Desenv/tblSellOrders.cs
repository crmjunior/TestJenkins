using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSellOrders
    {
        public tblSellOrders()
        {
            tblCallCenterCallsInadimplencia = new HashSet<tblCallCenterCallsInadimplencia>();
            tblLogMesesBlocoMaterialAnteriorAvulso = new HashSet<tblLogMesesBlocoMaterialAnteriorAvulso>();
            tblSellOrderDetails = new HashSet<tblSellOrderDetails>();
        }

        public int intOrderID { get; set; }
        public int intClientID { get; set; }
        public int intStoreID { get; set; }
        public DateTime? dteDate { get; set; }
        public string txtComment { get; set; }
        public int? intStatus { get; set; }
        public int? intShippingMethodID { get; set; }
        public int? intSellerID { get; set; }
        public int? intAgreementID { get; set; }
        public int? intStatus2 { get; set; }
        public int? intConditionTypeID { get; set; }

        public virtual tblClients intClient { get; set; }
        public virtual tblStores intStore { get; set; }
        public virtual tblSellOrdersTemplate tblSellOrdersTemplate { get; set; }
        public virtual ICollection<tblCallCenterCallsInadimplencia> tblCallCenterCallsInadimplencia { get; set; }
        public virtual ICollection<tblLogMesesBlocoMaterialAnteriorAvulso> tblLogMesesBlocoMaterialAnteriorAvulso { get; set; }
        public virtual ICollection<tblSellOrderDetails> tblSellOrderDetails { get; set; }
    }
}
