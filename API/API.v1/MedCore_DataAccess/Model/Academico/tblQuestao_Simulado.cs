using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestao_Simulado
    {
        public tblQuestao_Simulado()
        {
            tblSimuladoOrdenacao = new HashSet<tblSimuladoOrdenacao>();
            tblSimuladoVersao = new HashSet<tblSimuladoVersao>();
        }

        public int intQuestaoID { get; set; }
        public int intSimuladoID { get; set; }
        public string txtCodigoCorrecao { get; set; }
        public bool bitAnulada { get; set; }

        public virtual tblQuestoes intQuestao { get; set; }
        public virtual tblSimulado intSimulado { get; set; }
        public virtual ICollection<tblSimuladoOrdenacao> tblSimuladoOrdenacao { get; set; }
        public virtual ICollection<tblSimuladoVersao> tblSimuladoVersao { get; set; }
    }
}
