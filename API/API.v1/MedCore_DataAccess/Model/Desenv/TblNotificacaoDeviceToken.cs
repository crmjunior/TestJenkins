using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacaoDeviceToken
    {
        public int intNotificacaoDeviceToken { get; set; }
        public string txtOneSignalToken { get; set; }
        public int intNotificacaoId { get; set; }
        public int? intStatusEnvio { get; set; }
        public DateTime? dteEnvio { get; set; }
        public string txtInfoAdicional { get; set; }
        public string txtTitulo { get; set; }
        public string txtMensagem { get; set; }
        public int? intIdentificacaoId { get; set; }
    }
}
