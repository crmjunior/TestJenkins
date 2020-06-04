using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialOrdersGalpaoRomaneio
    {
        public int intRomaneioID { get; set; }
        public DateTime? dteDate { get; set; }
        public int intWareHouseID { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtZipCode { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public int? intResponsavelRecebimento { get; set; }
        public int? intVolumes { get; set; }
        public int? intShippingCompanyID { get; set; }
        public string txtMotorista { get; set; }
        public string txtDocumento { get; set; }
        public DateTime? dteDateExpedicao { get; set; }
        public int intStatus { get; set; }
        public int intTypeID { get; set; }
        public int intLocalEnvioID { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblEmployees intResponsavelRecebimentoNavigation { get; set; }
        public virtual tblShippingCompanies intShippingCompany { get; set; }
        public virtual tblWarehouses intWareHouse { get; set; }
    }
}
