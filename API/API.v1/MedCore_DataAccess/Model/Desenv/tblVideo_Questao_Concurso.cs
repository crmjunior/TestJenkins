using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideo_Questao_Concurso
    {
        public int intQuestaoID { get; set; }
        public int intType { get; set; }
        public DateTime? dteLastModifyDate { get; set; }
        public DateTime? dteCreationDate { get; set; }
        public int intEmployeeID { get; set; }
        public int intVideoID { get; set; }
    }
}
