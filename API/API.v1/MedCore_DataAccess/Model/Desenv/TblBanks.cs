using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBanks
    {
        public tblBanks()
        {
            tblBankAccounts = new HashSet<tblBankAccounts>();
            tblRPA_DadosBancarios = new HashSet<tblRPA_DadosBancarios>();
        }

        public int intBankID { get; set; }
        public string txtCode { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblBankAccounts> tblBankAccounts { get; set; }
        public virtual ICollection<tblRPA_DadosBancarios> tblRPA_DadosBancarios { get; set; }
    }
}
