using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Setor
    {
        public tblRequisicoes_Setor()
        {
            tblRequisicoes_AtivoMovimentacao = new HashSet<tblRequisicoes_AtivoMovimentacao>();
        }

        public int intRequisicaoSetorId { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual ICollection<tblRequisicoes_AtivoMovimentacao> tblRequisicoes_AtivoMovimentacao { get; set; }
    }
}
