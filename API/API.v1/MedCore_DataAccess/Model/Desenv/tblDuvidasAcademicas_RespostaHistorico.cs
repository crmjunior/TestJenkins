using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_RespostaHistorico
    {
        public int intRespostaHistoricoID { get; set; }
        public int intRespostaID { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteAtualizacao { get; set; }

        public virtual tblDuvidasAcademicas_Resposta intResposta { get; set; }
    }
}
