using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContratoIntensivo
    {
        public int intContratoIntensivoId { get; set; }
        public int? intContratoTipoId { get; set; }
        public int idConteudoContratacao { get; set; }
        public int intContratoCPMEDId { get; set; }
    }
}
