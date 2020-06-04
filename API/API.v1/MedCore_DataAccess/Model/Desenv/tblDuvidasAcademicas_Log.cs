using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Log
    {
        public int intIDLog { get; set; }
        public int? intDuvidaID { get; set; }
        public int? intRespostaID { get; set; }
        public int intClientID { get; set; }
        public int? intTipoInteracao { get; set; }
        public string txtAcao { get; set; }
        public DateTime dteData { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
        public virtual tblDuvidasAcademicas_Resposta intResposta { get; set; }
    }
}
