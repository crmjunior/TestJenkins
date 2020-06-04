using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblApi_MensagemInadimplencia_Log
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteDate { get; set; }
        public bool bitMensagemBloqueio { get; set; }

        public virtual tblPersons intClient { get; set; }
    }
}
