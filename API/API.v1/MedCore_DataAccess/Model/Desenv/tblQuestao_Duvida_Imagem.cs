using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida_Imagem
    {
        public int intDuvidaImagemId { get; set; }
        public string txtNomeImagem { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intQuestaoDuvidaId { get; set; }
    }
}
