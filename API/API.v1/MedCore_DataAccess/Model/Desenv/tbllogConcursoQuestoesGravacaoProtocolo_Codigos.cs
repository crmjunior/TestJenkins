using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tbllogConcursoQuestoesGravacaoProtocolo_Codigos
    {
        public int intQuestaoID { get; set; }
        public string intTypeID { get; set; }
        public string txtCode { get; set; }
        public bool? bitActive { get; set; }
        public DateTime? dteDateTime { get; set; }
        public int? intEmployeeID { get; set; }
        public int? intID { get; set; }
        public bool? bitPPT { get; set; }
        public bool? bitVideo { get; set; }
        public DateTime? dteDateTimeUpload { get; set; }
        public string txtJutificativa { get; set; }
        public int intEmployeeResponseExclude { get; set; }
        public DateTime dteExclude { get; set; }
        public int intLogID { get; set; }
    }
}
