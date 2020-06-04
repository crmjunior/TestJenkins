using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMensagensLogin
    {
        public int intId { get; set; }
        public int intAplicacaoId { get; set; }
        public int intTipoMensagemId { get; set; }
        public string txtMensagem { get; set; }
    }
}
