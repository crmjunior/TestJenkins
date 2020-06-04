using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialOrdersGalpao
    {
        public int intOrderId { get; set; }
        public int intMaterialID { get; set; }
        public int intProductGroupID { get; set; }
        public int? IntQuantidade { get; set; }
        public int intWarehouseid { get; set; }
        public int? intAno { get; set; }
        public DateTime? dteDatePrazoEnvio { get; set; }
        public DateTime? dteDatePrazoChegada { get; set; }
        public DateTime? dteDatePrazoConferencia { get; set; }
        public DateTime? dteDateAula { get; set; }
        public int intStatusID { get; set; }
        public int? intEmployeeID { get; set; }
        public bool? bitComplemento { get; set; }
        public int? intQuantidadeRecebida { get; set; }
        public int? intTipoComplementoID { get; set; }
        public int? intWarehouseOrigemID { get; set; }

        public virtual tblProducts intMaterial { get; set; }
        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblWarehouses intWarehouse { get; set; }
        public virtual tblWarehouses intWarehouseOrigem { get; set; }
    }
}
