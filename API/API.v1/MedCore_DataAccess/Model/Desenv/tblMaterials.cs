using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterials
    {
        public int intMaterialID { get; set; }
        public string txtName { get; set; }
        public string txtBarCode { get; set; }
        public int intMaterialGroupID { get; set; }
        public int intOrderUnitID { get; set; }
        public int intMaterialTypeID { get; set; }
        public int? intVendorID { get; set; }
        public int? intMaterialSubGroupID { get; set; }
        public string txtShortName { get; set; }
        public string txtStatus { get; set; }
        public int? intPackingID { get; set; }
        public double? dblWeight { get; set; }
        public int? intUnitsPacking { get; set; }

        public virtual tblCompanies intVendor { get; set; }
    }
}
