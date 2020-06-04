using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestoesSimuladoImagem_Comentario
    {
        public int intImagemComentarioID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtLabel { get; set; }
        public string txtName { get; set; }
        public byte[] imgImagem { get; set; }
    }
}
