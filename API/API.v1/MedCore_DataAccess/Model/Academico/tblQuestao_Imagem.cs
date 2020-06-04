using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestao_Imagem
    {
        public int intID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtLabel { get; set; }
        public string txtName { get; set; }
        public int? intClassification { get; set; }
        public byte[] imgImagem { get; set; }
        public byte[] imgImagemOtimizada { get; set; }
    }
}
