using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAdaptaMedAulaIndice
    {
        public int intAdaptaMedIndiceId { get; set; }
        public int intBookId { get; set; }
        public int intLessonTitleId { get; set; }
        public int intSemana { get; set; }
        public int? intOrdem { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
