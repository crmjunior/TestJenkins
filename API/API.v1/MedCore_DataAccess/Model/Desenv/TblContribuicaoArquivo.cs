using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContribuicaoArquivo
    {
        public int intContribuicaoArquivoID { get; set; }
        public int? intContribuicaoID { get; set; }
        public DateTime? dteDataCriacao { get; set; }
        public string txtContribuicaoArquivo { get; set; }
        public bool? bitAtivo { get; set; }
        public bool? bitAprovacaoMedgrupo { get; set; }
        public string txtUrl { get; set; }
        public string txtDescricao { get; set; }
        public int? intTipoArquivo { get; set; }
        public string txtDuracao { get; set; }

        public virtual tblContribuicao intContribuicao { get; set; }
    }
}
