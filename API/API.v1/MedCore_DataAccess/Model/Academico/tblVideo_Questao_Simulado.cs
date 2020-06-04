using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblVideo_Questao_Simulado
    {
        public int intQuestaoID { get; set; }
        public DateTime? dteLastModifyDate { get; set; }
        public DateTime? dteCreationDate { get; set; }
        public int intEmployeeID { get; set; }
        public int intVideoID { get; set; }

        public virtual tblQuestoes intQuestao { get; set; }
        public virtual tblVideo intVideo { get; set; }
    }
}
