using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_RequisicaoCotacao
    {
        public int intRequisicaoCotacaoId { get; set; }
        public int intRequisicaoId { get; set; }
        public int intFornecedorId { get; set; }
        public string txtPrazoEntrega { get; set; }
        public string txtLink { get; set; }
        public decimal dblValor { get; set; }
        public bool? bitAtivo { get; set; }
        public decimal dblValorUnitario { get; set; }
        public decimal? dblValorFrete { get; set; }
        public decimal? dblValorDesconto { get; set; }
        public int intFormaPagamentoId { get; set; }
        public DateTime dteVencimento { get; set; }
        public bool? bitEscolhido { get; set; }
        public int? intEscolhidoPorId { get; set; }
        public string txtObservacao { get; set; }
        public DateTime? dteEscolhido { get; set; }

        public virtual tblEmployees intEscolhidoPor { get; set; }
        public virtual tblRequisicoes_Fornecedor intFornecedor { get; set; }
        public virtual tblRequisicoes_Requisicao intRequisicao { get; set; }
    }
}
