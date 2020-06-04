using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestao_CometariosLog
    {
        public int intID { get; set; }
        public int? intQuestaoID { get; set; }
        public string txtComentario { get; set; }
        public DateTime? dteDate { get; set; }
        public int? intEmployeeID { get; set; }
        public string txtEnunciado { get; set; }
        public string txtAlternativa { get; set; }
        public bool? bitComentarioAtivo { get; set; }

        public virtual tblQuestoes intQuestao { get; set; }
    }
}
