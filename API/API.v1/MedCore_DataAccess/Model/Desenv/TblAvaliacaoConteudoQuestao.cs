using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvaliacaoConteudoQuestao
    {
        public int intAvaliacaoId { get; set; }
        public int intQuestaoId { get; set; }
        public int intTipoExercicioId { get; set; }
        public int intClientId { get; set; }
        public int intNota { get; set; }
        public int intTipoComentario { get; set; }
        public DateTime dteCadastro { get; set; }
        public string txtComentarioAvaliacao { get; set; }
        public bool bitActive { get; set; }
        public int? intComentarioLogId { get; set; }
        public int? intAlternativaReprova { get; set; }

        public virtual tblAvaliacaoConteudoQuestaoAlternativas intAlternativaReprovaNavigation { get; set; }
        public virtual tblPersons intClient { get; set; }
    }
}
