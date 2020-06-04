using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideoAprovacaoLog
    {
        public int intId { get; set; }
        public int intRevisaoAulaVideoAprovacaoId { get; set; }
        public int intEmployeeId { get; set; }
        public string txtJustificativa { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAprovado { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblRevisaoAulaVideoAprovacao intRevisaoAulaVideoAprovacao { get; set; }
    }
}
