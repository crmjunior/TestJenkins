using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_ProdutoCaracteristica
    {
        public tblRequisicoes_ProdutoCaracteristica()
        {
            tblRequisicoes_Ativo_ProdutoCaracteristica = new HashSet<tblRequisicoes_Ativo_ProdutoCaracteristica>();
            tblRequisicoes_RequisicaoItem_ProdutoCaracteristica = new HashSet<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica>();
        }

        public int intProdutoCaracteristicaId { get; set; }
        public int intProdutoId { get; set; }
        public string txtNome { get; set; }
        public int intTipo { get; set; }
        public int intOrdem { get; set; }
        public string txtValor { get; set; }
        public bool bitObrigatorio { get; set; }
        public string txtUnidade { get; set; }
        public bool? bitAtivo { get; set; }
        public bool? bitImportacao { get; set; }

        public virtual ICollection<tblRequisicoes_Ativo_ProdutoCaracteristica> tblRequisicoes_Ativo_ProdutoCaracteristica { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica> tblRequisicoes_RequisicaoItem_ProdutoCaracteristica { get; set; }
    }
}
