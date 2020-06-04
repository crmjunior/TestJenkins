using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Produto_Historico
    {
        public int intRequisicaoProdutoHistoricoId { get; set; }
        public int intProdutoId { get; set; }
        public DateTime dteData { get; set; }
        public int intEmployeeId { get; set; }
        public string txtMudancas { get; set; }
        public int intTipo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblRequisicoes_Produto intProduto { get; set; }
    }
}
