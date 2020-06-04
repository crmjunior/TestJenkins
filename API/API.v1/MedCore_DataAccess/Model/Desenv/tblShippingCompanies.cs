using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblShippingCompanies
    {
        public tblShippingCompanies()
        {
            tblMaterialOrdersGalpaoRomaneio = new HashSet<tblMaterialOrdersGalpaoRomaneio>();
        }

        public int intCompanyID { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtZipCode { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtComment { get; set; }
        public string txtRegistrationName { get; set; }
        public string txtFantasyName { get; set; }
        public string txtRegisterCode { get; set; }
        public string txtPhone { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
    }
}
