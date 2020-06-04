using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Ativo_ProdutoCaracteristica
    {
        public int intAtivoProdutoCaracteristicaId { get; set; }
        public int intAtivoId { get; set; }
        public int intProdutoCaracteristicaId { get; set; }
        public string txtValor { get; set; }

        public virtual tblRequisicoes_Ativo intAtivo { get; set; }
        public virtual tblRequisicoes_ProdutoCaracteristica intProdutoCaracteristica { get; set; }
    }
}
