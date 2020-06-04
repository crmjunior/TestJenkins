using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Rule_Detail
    {
        public int intRegraDetalheId { get; set; }
        public int? intRegraId { get; set; }
        public int? intProductGroupId { get; set; }
        public int? intTipoAnoId { get; set; }
        public int? intStatusOV { get; set; }
        public int? intStatusPagamento { get; set; }
        public int? intCallCategory { get; set; }
        public int? intStatusInterno { get; set; }
        public int? intClientId { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public int? intEmployeeID { get; set; }
        public bool bitAtivo { get; set; }
    }
}
