using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Notificacao
    {
        public int intDuvidaAcademicaNotificacaoId { get; set; }
        public int intDuvidaId { get; set; }
        public int intClientId { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public bool bitAtiva { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
    }
}
