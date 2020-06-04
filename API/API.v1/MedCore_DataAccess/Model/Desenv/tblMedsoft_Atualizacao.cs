using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_Atualizacao
    {
        public tblMedsoft_Atualizacao()
        {
            //tblMedsoft_Atualizacao_Aluno = new HashSet<tblMedsoft_Atualizacao_Aluno>();
        }

        public int intAtualizacaoID { get; set; }
        public DateTime dteRowDateTime { get; set; }
        public bool? bitActive { get; set; }
        public int intAtualizacaoTipoID { get; set; }
        public Guid guidColunaValue { get; set; }
        public bool bitSincronia { get; set; }

        //public virtual ICollection<tblMedsoft_Atualizacao_Aluno> tblMedsoft_Atualizacao_Aluno { get; set; }
    }
}
