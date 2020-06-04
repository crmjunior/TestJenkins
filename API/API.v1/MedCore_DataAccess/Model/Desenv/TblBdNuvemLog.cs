using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBdNuvemLog
    {
        public int intBdNuvemLogId { get; set; }
        public int intUltimaAtualizacaoId { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
