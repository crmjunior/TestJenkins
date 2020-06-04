using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblApplication_AcessDenied
    {
        public int intApplicationAcessDeniedId { get; set; }
        public int intEmedID { get; set; }
        public int intApplicationID { get; set; }
        public string txtReason { get; set; }
        public string txtMotivoDesbloqueio { get; set; }
    }
}
