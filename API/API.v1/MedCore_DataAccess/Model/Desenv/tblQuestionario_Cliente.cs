using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestionario_Cliente
    {
        public tblQuestionario_Cliente()
        {
            tblQuestionario_Detalhes = new HashSet<tblQuestionario_Detalhes>();
        }

        public int intQuestionarioClienteID { get; set; }
        public int intQuestionarioID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteInicio { get; set; }
        public string txtEmail { get; set; }
        public string txtRegister { get; set; }
        public int intTurmaID { get; set; }
        public int? intAplicacaoId { get; set; }
        public string IPAluno { get; set; }

        public virtual tblQuestionario intQuestionario { get; set; }
        public virtual ICollection<tblQuestionario_Detalhes> tblQuestionario_Detalhes { get; set; }
    }
}
