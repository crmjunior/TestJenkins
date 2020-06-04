using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_RuleDetail
    {
        public int intRegraDetalheId { get; set; }
        public int intRegraId { get; set; }
        public int intDetalheId { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblAccess_Detail intDetalhe { get; set; }
        public virtual tblAccess_Rule intRegra { get; set; }
    }
}
