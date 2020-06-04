using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCriterioOrdenacao_BuscaTexto
    {
        public int intID { get; set; }
        public int intTipoOrdenacao { get; set; }
        public string txtTexto { get; set; }
        public int intOrdem { get; set; }
    }
}
