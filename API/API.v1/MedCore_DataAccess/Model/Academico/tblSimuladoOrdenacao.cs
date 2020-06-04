using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoOrdenacao
    {
        public int intSimuladoID { get; set; }
        public int intQuestaoID { get; set; }
        public int? intOrdem { get; set; }

        public virtual tblQuestao_Simulado _int { get; set; }
    }
}
