using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblResumoAulaVideoAprovacaoLog
    {
        public int intId { get; set; }
        public int intResumoAulaVideoAprovacaoId { get; set; }
        public int intEmployeeId { get; set; }
        public string txtJustificativa { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAprovado { get; set; }
    }
}
