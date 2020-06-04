using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestao_Classificacao
    {
        public int intID { get; set; }
        public int intQuestaoID { get; set; }
        public int intTipoDeClassificacao { get; set; }
        public int intClassificacaoID { get; set; }
        public int? intEmployeeID { get; set; }
        public string txtRegister { get; set; }
        public DateTime dteDate { get; set; }

        public virtual tblConcursoQuestoes intQuestao { get; set; }
    }
}
