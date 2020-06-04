using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideoCorrigido
    {
        public int intVideoCorrigidoId { get; set; }
        public int intRevisaoAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitVerificado { get; set; }

        public virtual tblRevisaoAulaVideo intRevisaoAulaVideo { get; set; }
    }
}
