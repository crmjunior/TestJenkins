using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestao_Classificacao_Autorizacao_log
    {
        public int intQuestaoID { get; set; }
        public int intMaterialID { get; set; }
        public bool? bitAutorizacao { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDateTime { get; set; }
        public int intMaterialAno { get; set; }
        public bool? bitPendente { get; set; }
        public string txtComentarioCordendador { get; set; }
        public DateTime? dteEnvio { get; set; }
        public int? intCordenadorID { get; set; }
        public int intID { get; set; }
    }
}
