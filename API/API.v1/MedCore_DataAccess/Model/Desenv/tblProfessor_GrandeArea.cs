using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProfessor_GrandeArea
    {
        public int intID { get; set; }
        public int intProfessorID { get; set; }
        public int intClassificacaoID { get; set; }
    }
}
