using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAtualizacaoErrata_Imagens
    {
        public int intAtualizacaoErrataID { get; set; }
        public int intBookID { get; set; }
        public string txtPath { get; set; }
        public int intSequence { get; set; }
        public DateTime dteCadastro { get; set; }
        public string txtDescricao { get; set; }
        public string txtFileName { get; set; }
    }
}
