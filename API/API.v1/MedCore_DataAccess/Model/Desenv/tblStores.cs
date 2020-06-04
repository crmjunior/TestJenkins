using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStores
    {
        public tblStores()
        {
            tblAccountData = new HashSet<tblAccountData>();
            tblEmployees = new HashSet<tblEmployees>();
            tblInscricoesRessalvas = new HashSet<tblInscricoesRessalvas>();
            tblPaymentDocuments = new HashSet<tblPaymentDocuments>();
            tblSellOrders = new HashSet<tblSellOrders>();
            tblStore_CombosPaymentTemplate = new HashSet<tblStore_CombosPaymentTemplate>();
            tblWarehouses = new HashSet<tblWarehouses>();
        }

        public int intStoreID { get; set; }
        public string txtStoreName { get; set; }
        public int intStoreType { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtZipCode { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtContract { get; set; }
        public bool? bitEnableInternetSales { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblStores_Satelites tblStores_Satelites { get; set; }
        public virtual ICollection<tblAccountData> tblAccountData { get; set; }
        public virtual ICollection<tblEmployees> tblEmployees { get; set; }
        public virtual ICollection<tblInscricoesRessalvas> tblInscricoesRessalvas { get; set; }
        public virtual ICollection<tblPaymentDocuments> tblPaymentDocuments { get; set; }
        public virtual ICollection<tblSellOrders> tblSellOrders { get; set; }
        public virtual ICollection<tblStore_CombosPaymentTemplate> tblStore_CombosPaymentTemplate { get; set; }
        public virtual ICollection<tblWarehouses> tblWarehouses { get; set; }
    }
}
