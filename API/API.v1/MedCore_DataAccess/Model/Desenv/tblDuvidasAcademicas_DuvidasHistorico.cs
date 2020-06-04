using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_DuvidasHistorico
    {
        public int intDuvidaHistoricoID { get; set; }
        public int intDuvidaID { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteAtualizacao { get; set; }

        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
    }
}
