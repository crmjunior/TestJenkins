using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidaRespostaProfessor
    {
        public int intDuvidaRespostaProfessorId { get; set; }
        public int intDuvidaId { get; set; }
        public int intEmployeeId { get; set; }
        public string txtResposta { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
    }
}
