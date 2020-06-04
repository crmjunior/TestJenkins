using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideos_Brutos_Busca
    {
        public string Tipo { get; set; }
        public string txtFileName { get; set; }
        public string txtCompleteFileName { get; set; }
        public int? intBookID { get; set; }
        public int? intQuestaoID { get; set; }
        public string txtVideoCode { get; set; }
        public int intID { get; set; }
        public int? intVideoTipo { get; set; }

        public virtual tblBooks intBook { get; set; }
        public virtual tblConcursoQuestoes intQuestao { get; set; }
    }
}
