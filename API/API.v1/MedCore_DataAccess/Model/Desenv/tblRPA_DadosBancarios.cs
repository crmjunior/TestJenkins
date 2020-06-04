using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_DadosBancarios
    {
        public int intRPADadosBancariosId { get; set; }
        public int intRPAId { get; set; }
        public int? intBancoId { get; set; }
        public string txtAgencia { get; set; }
        public string txtConta { get; set; }
        public string txtTipoConta { get; set; }

        public virtual tblBanks intBanco { get; set; }
        public virtual tblRPA intRPA { get; set; }
    }
}
