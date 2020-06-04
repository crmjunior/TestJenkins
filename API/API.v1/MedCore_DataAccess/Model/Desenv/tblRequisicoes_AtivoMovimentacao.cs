using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_AtivoMovimentacao
    {
        public int intAtivoMovimentacaoId { get; set; }
        public int intAtivoId { get; set; }
        public int? intResponsavelId { get; set; }
        public DateTime dteMovimentacao { get; set; }
        public int? intRequisicaoUnidadeId { get; set; }
        public int? intRequisicaoSetorId { get; set; }
        public int? intTipoMovimentacao { get; set; }
        public bool? bitRequisicaoEstoque { get; set; }

        public virtual tblRequisicoes_Ativo intAtivo { get; set; }
        public virtual tblRequisicoes_Setor intRequisicaoSetor { get; set; }
        public virtual tblRequisicoes_Unidade intRequisicaoUnidade { get; set; }
        public virtual tblEmployees intResponsavel { get; set; }
    }
}
