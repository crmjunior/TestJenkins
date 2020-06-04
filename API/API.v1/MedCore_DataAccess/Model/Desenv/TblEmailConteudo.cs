using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmailConteudo
    {
        public int intEmailConteudoID { get; set; }
        public string txtConteudo { get; set; }
        public int intProdutoID { get; set; }
        public int? intTipoEnvioDeEmailID { get; set; }
        public int intAplicacaoId { get; set; }
    }
}
