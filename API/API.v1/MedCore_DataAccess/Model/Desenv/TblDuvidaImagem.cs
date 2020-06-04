using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidaImagem
    {
        public int intDuvidaImagemId { get; set; }
        public int intDuvidaId { get; set; }
        public string txtNomeImagem { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
    }
}
