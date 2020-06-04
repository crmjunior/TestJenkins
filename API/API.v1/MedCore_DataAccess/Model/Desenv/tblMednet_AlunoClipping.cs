using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMednet_AlunoClipping
    {
        public int intMnClipping { get; set; }
        public int intClientId { get; set; }
        public int intRevisaoAulaId { get; set; }
        public int intTempoIni { get; set; }
        public int intTempoFim { get; set; }
        public string txtTitulo { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
