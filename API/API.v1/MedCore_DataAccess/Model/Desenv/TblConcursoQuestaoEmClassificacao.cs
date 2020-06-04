using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestaoEmClassificacao
    {
        public int intQuestaoID { get; set; }
        public int intEmployeeID { get; set; }
        public int intProcessoDeClassificacao { get; set; }
        public DateTime dteDateTime { get; set; }
        public int id { get; set; }
    }
}
