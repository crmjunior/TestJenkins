using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_FornecedorFormaPagamento
    {
        public int intFornecedorFormaPagamentoId { get; set; }
        public int intFornecedorId { get; set; }
        public int intFormaPagamentoId { get; set; }

        public virtual tblRequisicoes_Fornecedor intFornecedor { get; set; }
    }
}
