using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestoesConcursoImagem_Comentario
    {
        public int intImagemComentarioID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtLabel { get; set; }
        public string txtName { get; set; }
        public byte[] imgImagem { get; set; }
    }
}
