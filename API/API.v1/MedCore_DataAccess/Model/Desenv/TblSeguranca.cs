using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSeguranca
    {
        public int intMsMovelSegurancaId { get; set; }
        public int intDeviceId { get; set; }
        public int intClientId { get; set; }
        public string txtDeviceToken { get; set; }
        public int intApplicationId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool? bitDevicePrincipal { get; set; }
    }
}
