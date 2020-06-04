using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestionario_Detalhes
    {
        public int intID { get; set; }
        public int intQuestionarioClienteID { get; set; }
        public DateTime dteResposta { get; set; }
        public int intNumAlternativa { get; set; }
        public int intQuestionarioQuestoesID { get; set; }
        public string txtComentario { get; set; }

        public virtual tblQuestionario_Cliente intQuestionarioCliente { get; set; }
    }
}
