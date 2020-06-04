using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestionario
    {
        public tblQuestionario()
        {
            tblQuestionario_Cliente = new HashSet<tblQuestionario_Cliente>();
            tblQuestionario_Questoes_Alternativas = new HashSet<tblQuestionario_Questoes_Alternativas>();
        }

        public int intQuestionarioID { get; set; }
        public int intAnoQuestionario { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
        public int intProductGroup1 { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblQuestionario_Cliente> tblQuestionario_Cliente { get; set; }
        public virtual ICollection<tblQuestionario_Questoes_Alternativas> tblQuestionario_Questoes_Alternativas { get; set; }
    }
}
