using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContribuicoes_Interacao
    {
        public int intContribuicaoInteracaoID { get; set; }
        public int intContribuicaoID { get; set; }
        public int intClientID { get; set; }
        public int intContribuicaoTipo { get; set; }
        public DateTime dteDataCriacao { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblContribuicao intContribuicao { get; set; }
    }
}
