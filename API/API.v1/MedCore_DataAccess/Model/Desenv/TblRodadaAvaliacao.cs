using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRodadaAvaliacao
    {
        public tblRodadaAvaliacao()
        {
            tblRodadaAluno = new HashSet<tblRodadaAluno>();
        }

        public int intID { get; set; }
        public DateTime dteDataCriacao { get; set; }

        public virtual ICollection<tblRodadaAluno> tblRodadaAluno { get; set; }
    }
}
