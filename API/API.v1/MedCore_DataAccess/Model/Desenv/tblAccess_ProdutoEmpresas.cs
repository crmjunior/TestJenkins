using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_ProdutoEmpresas
    {
        public int intProdutoEmpresaId { get; set; }
        public string txtproduto { get; set; }
        public int? intEmpresaId { get; set; }
        public int? intProductGroupId { get; set; }
    }
}
