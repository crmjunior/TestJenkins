using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClients_BlackListAprovacoes
    {
        public int intClientBlackListID { get; set; }
        public int intClientID { get; set; }
        public string txtType { get; set; }
        public string txtMessage { get; set; }
        public DateTime? dteDateTimeInclusao { get; set; }
        public string txtAnexoDossie { get; set; }
        public int? intBlackListCategoriaID { get; set; }
        public string txtRegister { get; set; }
        public string txtNome { get; set; }
        public string txtEmail { get; set; }
    }
}
