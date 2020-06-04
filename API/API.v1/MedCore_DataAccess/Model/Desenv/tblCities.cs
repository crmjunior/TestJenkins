using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCities
    {
        public tblCities()
        {
            tblClassRooms = new HashSet<tblClassRooms>();
            tblCompanies = new HashSet<tblCompanies>();
            tblEnderecoEntregaCliente = new HashSet<tblEnderecoEntregaCliente>();
            tblMaterialOrdersGalpaoRomaneio = new HashSet<tblMaterialOrdersGalpaoRomaneio>();
            tblPersons = new HashSet<tblPersons>();
            tblRPA = new HashSet<tblRPA>();
            tblShippingCompanies = new HashSet<tblShippingCompanies>();
            tblStores = new HashSet<tblStores>();
            tblWarehouses = new HashSet<tblWarehouses>();
        }

        public int intCityID { get; set; }
        public string txtName { get; set; }
        public int intState { get; set; }
        public int? intAreaCode { get; set; }
        public bool? bitCapital { get; set; }
        public string txtCodigoIBGE { get; set; }

        public virtual tblStates intStateNavigation { get; set; }
        public virtual ICollection<tblClassRooms> tblClassRooms { get; set; }
        public virtual ICollection<tblCompanies> tblCompanies { get; set; }
        public virtual ICollection<tblEnderecoEntregaCliente> tblEnderecoEntregaCliente { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
        public virtual ICollection<tblPersons> tblPersons { get; set; }
        public virtual ICollection<tblRPA> tblRPA { get; set; }
        public virtual ICollection<tblShippingCompanies> tblShippingCompanies { get; set; }
        public virtual ICollection<tblStores> tblStores { get; set; }
        public virtual ICollection<tblWarehouses> tblWarehouses { get; set; }
    }
}
