using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPermissaoInadimplenciaConfiguracao
    {
        public int intId { get; set; }
        public string txtMensagemDeAcordo { get; set; }
        public string txtMensagemBloqueio { get; set; }
        public int intLimiteDias { get; set; }
        public string txtMensagemSemOv { get; set; }
    }
}
