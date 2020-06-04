using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Recurso_Aluno
    {
        public int ID_CONCURSO_RECURSO_ALUNO { get; set; }
        public int intContactID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtRecurso_Comentario { get; set; }
        public DateTime dteCadastro { get; set; }
        public string txtAlternativa_Sugerida { get; set; }
        public string txtIP_Usuario { get; set; }
        public bool? bitActive { get; set; }
        public bool? bitOpiniao { get; set; }
        public byte? intTipo { get; set; }
        public bool? bitDeferido { get; set; }
    }
}
