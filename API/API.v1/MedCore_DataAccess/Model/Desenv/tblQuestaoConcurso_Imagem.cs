using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestaoConcurso_Imagem
    {
        public int intQuestaoID { get; set; }
        public string txtLabel { get; set; }
        public string txtName { get; set; }
        public int? intClassification { get; set; }
        public byte[] imgImagem { get; set; }
        public int intID { get; set; }
        public byte[] imgImagemOtimizada { get; set; }
    }
}
