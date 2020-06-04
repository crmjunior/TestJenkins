using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContratoImagem
    {
        public int intContratoImagemId { get; set; }
        public int intDocumentosId { get; set; }
        public int intContratoCPMEDId { get; set; }
        public string txtlinkImagem { get; set; }
    }
}
