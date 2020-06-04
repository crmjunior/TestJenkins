using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada_log
    {
        public int intProductID { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDateTime { get; set; }
        public bool bitActive { get; set; }
        public long intLiberacaoLogID { get; set; }
        public string txtIP { get; set; }
        public long? intLiberacaoID { get; set; }
        public string txtMotivo { get; set; }
        public bool? bitRevisar { get; set; }
    }
}
