using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBloqueioQuestoes
    {
        public int intBloqueioConcursoId { get; set; }
        public int intQuestaoId { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
