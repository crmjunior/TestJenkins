using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Denuncia
    {
        public int intDenunciaID { get; set; }
        public int? intContactID { get; set; }
        public int? intDuvidaID { get; set; }
        public int? intRespostaID { get; set; }
        public int? intTipoDenuncia { get; set; }
        public string txtComplemento { get; set; }
        public DateTime? dteDataCriacao { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
        public virtual tblDuvidasAcademicas_Resposta intResposta { get; set; }
    }
}
