using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblImagemSemanaRespostaAluno
    {
        public int intImagemSemanaRespostaAlunoID { get; set; }
        public int intImagemSemanaID { get; set; }
        public int intContactID { get; set; }
        public DateTime dteRespostaAluno { get; set; }
        public string txtDescricao { get; set; }
        public bool bitLiberarResposta { get; set; }
    }
}
