using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBloqueioConcurso
    {
        public int intBloqueioConcursoId { get; set; }
        public int intProvaId { get; set; }
        public int intBloqueioAreaId { get; set; }
        public DateTime dteCadastro { get; set; }

        public virtual tblBloqueioArea intBloqueioArea { get; set; }
        public virtual tblConcurso_Provas intProva { get; set; }
    }
}
