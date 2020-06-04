using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Produto
    {
        public tblRequisicoes_Produto()
        {
            tblRequisicoes_Ativo = new HashSet<tblRequisicoes_Ativo>();
            tblRequisicoes_Fornecedor_Produto = new HashSet<tblRequisicoes_Fornecedor_Produto>();
            tblRequisicoes_Produto_Historico = new HashSet<tblRequisicoes_Produto_Historico>();
            tblRequisicoes_RequisicaoItem = new HashSet<tblRequisicoes_RequisicaoItem>();
        }

        public int intProdutoId { get; set; }
        public int intProdutoGrupoId { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }
        public bool? bitImportacao { get; set; }

        public virtual tblRequisicoes_ProdutoGrupo intProdutoGrupo { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo> tblRequisicoes_Ativo { get; set; }
        public virtual ICollection<tblRequisicoes_Fornecedor_Produto> tblRequisicoes_Fornecedor_Produto { get; set; }
        public virtual ICollection<tblRequisicoes_Produto_Historico> tblRequisicoes_Produto_Historico { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoItem> tblRequisicoes_RequisicaoItem { get; set; }
    }
}
