using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_RequisicaoItem
    {
        public tblRequisicoes_RequisicaoItem()
        {
            tblRequisicoes_RequisicaoItem_ProdutoCaracteristica = new HashSet<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica>();
        }

        public int intRequisicaoItemId { get; set; }
        public int intProdutoId { get; set; }
        public string txtResumo { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }
        public int intQuantidade { get; set; }

        public virtual tblRequisicoes_Produto intProduto { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica> tblRequisicoes_RequisicaoItem_ProdutoCaracteristica { get; set; }
    }
}
