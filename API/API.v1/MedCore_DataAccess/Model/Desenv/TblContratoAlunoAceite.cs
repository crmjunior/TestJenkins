using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContratoAlunoAceite
    {
        public tblContratoAlunoAceite()
        {
            tblContratoAceite = new HashSet<tblContratoAceite>();
        }

        public int intContratoAlunoAceite { get; set; }
        public int? intMatricula { get; set; }
        public string txtRegistro { get; set; }
        public string txtNomeAluno { get; set; }
        public string txtIpAluno { get; set; }
        public DateTime? txtDataAceiteAluno { get; set; }

        public virtual ICollection<tblContratoAceite> tblContratoAceite { get; set; }
    }
}
