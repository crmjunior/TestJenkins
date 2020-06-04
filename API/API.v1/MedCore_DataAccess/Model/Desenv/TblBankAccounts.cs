using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBankAccounts
    {
        public int intBankAccountID { get; set; }
        public int intBankID { get; set; }
        public string txtAgency { get; set; }
        public string txtAccount { get; set; }
        public string txtNossoNumero { get; set; }
        public string txtCarteira { get; set; }
        public string txtAccountDigit { get; set; }
        public string txtNossoNumeroFim { get; set; }
        public double? dblTaxaJuro { get; set; }
        public int? intDiasAtraso { get; set; }
        public string txtInstrucoes { get; set; }
        public bool? bitProtesto { get; set; }
        public bool? bitPagamentoParcial { get; set; }
        public int? intCompanyID { get; set; }
        public string txtTipoConta { get; set; }
        public string strIdEstabelecimento { get; set; }
        public string strChaveEstabelecimento { get; set; }
        public bool? bitModoSandbox { get; set; }

        public virtual tblBanks intBank { get; set; }
        public virtual tblCompanies intCompany { get; set; }
    }
}
