using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClients_BlackListMotivos
    {
        public int intClientBlackListID { get; set; }
        public string txtRegister { get; set; }
        public string txtMessage { get; set; }
        public DateTime dteDataMotivo { get; set; }
    }
}
