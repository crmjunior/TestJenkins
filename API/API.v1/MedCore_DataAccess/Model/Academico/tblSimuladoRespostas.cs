using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoRespostas
    {
        public int intClientID { get; set; }
        public int intSimuladoID { get; set; }
        public int intVersaoID { get; set; }
        public int intQuestao { get; set; }
        public string txtLetraResposta { get; set; }
        public int? intArquivoID { get; set; }

        public virtual tblLogLeituraCartaoRespostaSimulados intArquivo { get; set; }
        public virtual tblSimulado intSimulado { get; set; }
    }
}
