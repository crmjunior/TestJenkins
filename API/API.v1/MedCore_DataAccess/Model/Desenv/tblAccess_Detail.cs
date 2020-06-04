using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Detail
    {
        public tblAccess_Detail()
        {
            tblAccess_RuleDetail = new HashSet<tblAccess_RuleDetail>();
        }

        public int intDetalheId { get; set; }
        public string txtDescricao { get; set; }
        public int intProductGroupId { get; set; }
        public int intTipoAnoId { get; set; }
        public int intStatusOV { get; set; }
        public int intStatusPagamento { get; set; }
        public int intCallCategory { get; set; }
        public int intStatusInterno { get; set; }
        public int intClientId { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public int intEmployeeID { get; set; }
        public bool? bitAtivo { get; set; }
        public int? intEmpresaId { get; set; }
        public int intCourseID { get; set; }
        public int intStoreID { get; set; }
        public int intGroupId { get; set; }
        public int intAnoMinimo { get; set; }

        public virtual ICollection<tblAccess_RuleDetail> tblAccess_RuleDetail { get; set; }
    }
}
