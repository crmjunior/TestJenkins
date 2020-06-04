using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida_Moderada
    {
        public int intQuestaoDuvidaModeradaId { get; set; }
        public int intQuestaoDuvidaId { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitActive { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
