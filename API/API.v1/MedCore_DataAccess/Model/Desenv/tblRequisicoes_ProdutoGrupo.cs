using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_ProdutoGrupo
    {
        public tblRequisicoes_ProdutoGrupo()
        {
            tblRequisicoes_Produto = new HashSet<tblRequisicoes_Produto>();
        }

        public int intProdutoGrupoId { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }
        public bool? bitImportacao { get; set; }

        public virtual ICollection<tblRequisicoes_Produto> tblRequisicoes_Produto { get; set; }
    }
}
