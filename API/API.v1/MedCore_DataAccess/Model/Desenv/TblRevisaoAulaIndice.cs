using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaIndice
    {
        public int intRevisaoAulaIndiceId { get; set; }
        public int intBookId { get; set; }
        public int intLessonTitleId { get; set; }
        public int intOrdem { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
