using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_RequisicaoItem_ProdutoCaracteristica
    {
        public int intItemProdutoCaracteristicaId { get; set; }
        public int intRequisicaoItemId { get; set; }
        public int intProdutoCaracteristicaId { get; set; }
        public string txtValor { get; set; }

        public virtual tblRequisicoes_ProdutoCaracteristica intProdutoCaracteristica { get; set; }
        public virtual tblRequisicoes_RequisicaoItem intRequisicaoItem { get; set; }
    }
}
