using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblWarehouse_AntecipacaoMaterial
    {
        public int intAtencipacaoMaterialID { get; set; }
        public int intWarehouseID { get; set; }
        public bool bitMaterialEntregueNaFilial { get; set; }
        public int intYear { get; set; }
    }
}
