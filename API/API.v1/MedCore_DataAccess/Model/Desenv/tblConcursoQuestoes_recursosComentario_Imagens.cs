using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoes_recursosComentario_Imagens
    {
        public int intID { get; set; }
        public int intQuestao { get; set; }
        public string txtLabel { get; set; }
        public string txtName { get; set; }
        public string intClassificacao { get; set; }
        public string txtPath { get; set; }
    }
}
