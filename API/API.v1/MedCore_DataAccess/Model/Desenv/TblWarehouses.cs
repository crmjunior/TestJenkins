using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblWarehouses
    {
        public tblWarehouses()
        {
            tblMaterialOrdersGalpaoRomaneio = new HashSet<tblMaterialOrdersGalpaoRomaneio>();
            tblMaterialOrdersGalpaointWarehouse = new HashSet<tblMaterialOrdersGalpao>();
            tblMaterialOrdersGalpaointWarehouseOrigem = new HashSet<tblMaterialOrdersGalpao>();
            tblWarehousesClassRooms = new HashSet<tblWarehousesClassRooms>();
        }

        public int intWarehouseID { get; set; }
        public string txtComplementoNome { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtZipCode { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public int intEmployeeID { get; set; }
        public string txtObservacao { get; set; }
        public int intStoreID { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblStores intStore { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpao> tblMaterialOrdersGalpaointWarehouse { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpao> tblMaterialOrdersGalpaointWarehouseOrigem { get; set; }
        public virtual ICollection<tblWarehousesClassRooms> tblWarehousesClassRooms { get; set; }
    }
}
