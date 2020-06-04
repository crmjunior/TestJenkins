using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Fornecedor_Produto
    {
        public int intFornecedorProdutoId { get; set; }
        public int intFornecedorId { get; set; }
        public int intProdutoId { get; set; }

        public virtual tblRequisicoes_Fornecedor intFornecedor { get; set; }
        public virtual tblRequisicoes_Produto intProduto { get; set; }
    }
}
