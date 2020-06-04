using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoImportacaoCartaoResposta
    {
        public int intLeituraID { get; set; }
        public string txtClientID { get; set; }
        public string txtSimuladoID { get; set; }
        public string txtColVazia { get; set; }
        public string txtVersaoID { get; set; }
        public string txtResposta { get; set; }
        public int? intArquivoID { get; set; }
        public string txtTurma { get; set; }

        public virtual tblLogLeituraCartaoRespostaSimulados intArquivo { get; set; }
    }
}
