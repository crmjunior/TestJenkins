using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContrato
    {
        public int intContratoCPMEDId { get; set; }
        public int intDocumentosId { get; set; }
        public int intContratoTipoId { get; set; }
        public string txtlinkPDF { get; set; }

        public virtual tblDocumento intDocumentos { get; set; }
    }
}
