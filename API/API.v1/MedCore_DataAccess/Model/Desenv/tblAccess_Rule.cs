using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Rule
    {
        public tblAccess_Rule()
        {
            tblAccess_Permission_Rule = new HashSet<tblAccess_Permission_Rule>();
            tblAccess_RuleDetail = new HashSet<tblAccess_RuleDetail>();
        }

        public int intRegraId { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteCriacao { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public int intEmployeeID { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual ICollection<tblAccess_Permission_Rule> tblAccess_Permission_Rule { get; set; }
        public virtual ICollection<tblAccess_RuleDetail> tblAccess_RuleDetail { get; set; }
    }
}
