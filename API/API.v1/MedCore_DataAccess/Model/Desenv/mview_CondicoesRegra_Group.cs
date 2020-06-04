using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class mview_CondicoesRegra_Group
    {
        public int intDetalheId { get; set; }
        public int intRegraId { get; set; }
        public int intClientId { get; set; }
        public int intTipoAnoId { get; set; }
        public int intProductGroupId { get; set; }
        public int intStatusOV { get; set; }
        public int intStatusPagamento { get; set; }
        public int intCallCategory { get; set; }
        public int intStatusInterno { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public bool bitAtivo { get; set; }
        public int? intEmpresaId { get; set; }
        public int intStoreID { get; set; }
        public int intCourseID { get; set; }
        public int intGroupId { get; set; }
        public int intGroupClientId { get; set; }
        public int intAnoMinimo { get; set; }
    }
}
