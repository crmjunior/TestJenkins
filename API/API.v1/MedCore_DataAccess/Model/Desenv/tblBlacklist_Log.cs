using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBlacklist_Log
    {
        public int intId { get; set; }
        public int? intClientId { get; set; }
        public string txtRegister { get; set; }
        public int? intTipoBloqueio { get; set; }
        public string txtMotivo { get; set; }
        public bool? bitBloqueio { get; set; }
        public DateTime? dteData { get; set; }
        public int? intEmployeeId { get; set; }
        public string txtNome { get; set; }
        public string txtEmail { get; set; }
        public string txtFaculdade { get; set; }
        public int? intTipo { get; set; }
    }
}
