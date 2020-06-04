using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAdaptaMedAulaVideoCorrigido
    {
        public int intVideoCorrigidoId { get; set; }
        public int intAdaptaMedAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitVerificado { get; set; }
    }
}
