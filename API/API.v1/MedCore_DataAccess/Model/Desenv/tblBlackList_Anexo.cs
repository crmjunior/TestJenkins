using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBlackList_Anexo
    {
        public int intBlackListAnexoID { get; set; }
        public int intContactID { get; set; }
        public string txtAnexoDossie { get; set; }
        public DateTime dteDateTimeInclusao { get; set; }
        public int intUsuarioInclusaoID { get; set; }
        public DateTime? dteDateTimeAlteracao { get; set; }
        public int? intUsuarioAlteracaoID { get; set; }
        public bool bitAtivo { get; set; }
        public string txtRegister { get; set; }
    }
}
