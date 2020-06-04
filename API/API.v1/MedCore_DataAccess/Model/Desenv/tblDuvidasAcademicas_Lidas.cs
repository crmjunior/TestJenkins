using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Lidas
    {
        public int intLidaID { get; set; }
        public int? intDuvidaID { get; set; }
        public int? intClientID { get; set; }
        public DateTime dteCriacao { get; set; }

        public virtual tblClients intClient { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
    }
}
