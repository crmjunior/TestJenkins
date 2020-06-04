using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_Atualizacao_Aluno
    {
        public int intAtualizacaoID { get; set; }
        public int intClientID { get; set; }
        public string txtMachineToken { get; set; }
        public DateTime dteDateTime { get; set; }
        public bool? bitActive { get; set; }

        public virtual tblMedsoft_Atualizacao intAtualizacao { get; set; }
    }
}
