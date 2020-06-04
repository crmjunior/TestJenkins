using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogConcursoQuestaoComentario
    {
        public int? intQuestaoID { get; set; }
        public string txtComentarioAntigo { get; set; }
        public string txtComentarioNovo { get; set; }
        public DateTime? dteDateAlteracao { get; set; }
        public int? intEmployeeAntigo { get; set; }
        public int? intEmployeeAlterou { get; set; }
        public string txtLoginName { get; set; }
        public bool? bitDuplicado { get; set; }
        public int id { get; set; }
    }
}
