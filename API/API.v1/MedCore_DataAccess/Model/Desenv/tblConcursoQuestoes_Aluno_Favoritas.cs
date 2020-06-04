using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoes_Aluno_Favoritas
    {
        public int intQuestaoID { get; set; }
        public int intClientID { get; set; }
        public bool bitActive { get; set; }
        public DateTime dteDate { get; set; }
        public bool? bitVideo { get; set; }
        public bool? bitDuvida { get; set; }
        public string charResposta { get; set; }
        public bool? bitResultadoResposta { get; set; }
        public string charRespostaNova { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblConcursoQuestoes intQuestao { get; set; }
    }
}
