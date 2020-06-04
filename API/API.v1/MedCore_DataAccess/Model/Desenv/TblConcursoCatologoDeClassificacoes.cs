using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoCatologoDeClassificacoes
    {
        public int intClassificacaoID { get; set; }
        public int? intTipoDeClassificacao { get; set; }
        public string txtTipoDeClassificacao { get; set; }
        public string txtSubTipoDeClassificacao { get; set; }
        public string txtDescricaoClassificacao { get; set; }
    }
}
