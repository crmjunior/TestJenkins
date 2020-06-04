using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogOperacoesConcurso
    {
        public int intLogOperacoesConcurso { get; set; }
        public int intEmployeeID { get; set; }
        public int? intProvaID { get; set; }
        public int? intQuestaoID { get; set; }
        public int? intAlternativaID { get; set; }
        public DateTime dteDataAlteracao { get; set; }
        public string txtDescricao { get; set; }
        public int? intAndamentoCadastro { get; set; }
    }
}
