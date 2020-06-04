using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Provas_ArquivosAluno
    {
        public int intArquivoAlunoID { get; set; }
        public int intProvaID { get; set; }
        public int intContactID { get; set; }
        public string txtPasta { get; set; }
        public string txtHashNomeArquivo { get; set; }
        public string txtNomeArquivo { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblConcurso_Provas intProva { get; set; }
    }
}
