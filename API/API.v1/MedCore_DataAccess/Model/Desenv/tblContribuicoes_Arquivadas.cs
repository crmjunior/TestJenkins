using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContribuicoes_Arquivadas
    {
        public int intContribuicaoArquivadaID { get; set; }
        public int intContribuicaoID { get; set; }
        public int intClientID { get; set; }
        public bool bitAprovarMaisTarde { get; set; }
        public DateTime dteDataCriacao { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblContribuicao intContribuicao { get; set; }
    }
}
