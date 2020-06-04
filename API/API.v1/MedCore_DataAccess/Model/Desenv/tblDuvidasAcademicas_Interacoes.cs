using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Interacoes
    {
        public int intInteracaoId { get; set; }
        public int? intDuvidaId { get; set; }
        public int? intRespostaId { get; set; }
        public int intClientID { get; set; }
        public DateTime dteCriacao { get; set; }
        public int? intVote { get; set; }
        public bool? bitFavorita { get; set; }
        public bool? bitDenuncia { get; set; }
        public int intTipoInteracaoId { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblDuvidasAcademicas_TipoInteracao intTipoInteracao { get; set; }
    }
}
