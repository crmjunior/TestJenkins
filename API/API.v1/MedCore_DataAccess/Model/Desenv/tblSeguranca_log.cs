using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSeguranca_log
    {
        public int intSegurancaLogId { get; set; }
        public int intDeviceId { get; set; }
        public int intClientId { get; set; }
        public string txtDeviceToken { get; set; }
        public int intApplicationId { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime dteAtualizacao { get; set; }
        public string txtJustificativa { get; set; }
    }
}
