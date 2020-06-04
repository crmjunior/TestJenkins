using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevalidaAulaIndice
    {
        public int intRevalidaAulaIndiceId { get; set; }
        public int intLessonTitleRevalidaId { get; set; }
        public int? intOrdem { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
