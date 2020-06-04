using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAddressTypes
    {
        public tblAddressTypes()
        {
            tblClassRooms = new HashSet<tblClassRooms>();
            tblCompanies = new HashSet<tblCompanies>();
            tblMaterialOrdersGalpaoRomaneio = new HashSet<tblMaterialOrdersGalpaoRomaneio>();
            tblPersons = new HashSet<tblPersons>();
            tblShippingCompanies = new HashSet<tblShippingCompanies>();
            tblStores = new HashSet<tblStores>();
            tblWarehouses = new HashSet<tblWarehouses>();
        }

        public int intAddressTypeID { get; set; }
        public string txtAddressTypeName { get; set; }

        public virtual ICollection<tblClassRooms> tblClassRooms { get; set; }
        public virtual ICollection<tblCompanies> tblCompanies { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
        public virtual ICollection<tblPersons> tblPersons { get; set; }
        public virtual ICollection<tblShippingCompanies> tblShippingCompanies { get; set; }
        public virtual ICollection<tblStores> tblStores { get; set; }
        public virtual ICollection<tblWarehouses> tblWarehouses { get; set; }
    }
}
