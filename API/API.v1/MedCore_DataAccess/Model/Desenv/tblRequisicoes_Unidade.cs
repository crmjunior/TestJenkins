using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Unidade
    {
        public tblRequisicoes_Unidade()
        {
            tblRequisicoes_AtivoMovimentacao = new HashSet<tblRequisicoes_AtivoMovimentacao>();
        }

        public int intRequisicaoUnidadeId { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }
        public bool? bitMatriz { get; set; }

        public virtual ICollection<tblRequisicoes_AtivoMovimentacao> tblRequisicoes_AtivoMovimentacao { get; set; }
    }
}
