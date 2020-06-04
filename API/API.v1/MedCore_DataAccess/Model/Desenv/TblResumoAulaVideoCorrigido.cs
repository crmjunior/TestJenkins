using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblResumoAulaVideoCorrigido
    {
        public int intVideoCorrigidoId { get; set; }
        public int intResumoAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitVerificado { get; set; }
    }
}
