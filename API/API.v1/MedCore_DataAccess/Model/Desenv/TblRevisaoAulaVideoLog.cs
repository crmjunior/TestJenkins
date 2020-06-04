using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideoLog
    {
        public int intRevisaoAulaVideoLogId { get; set; }
        public int intRevisaoAulaId { get; set; }
        public int intClientId { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
