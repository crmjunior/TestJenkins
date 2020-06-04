using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoVersao
    {
        public int intSimuladoID { get; set; }
        public int intQuestaoID { get; set; }
        public int intVersaoID { get; set; }
        public int intQuestao { get; set; }
        public bool bitAnulada { get; set; }

        public virtual tblQuestao_Simulado _int { get; set; }
    }
}
