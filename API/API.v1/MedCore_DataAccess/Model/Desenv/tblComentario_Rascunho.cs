using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblComentario_Rascunho
    {
        public int intID { get; set; }
        public int intEmployeeID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtRascunho { get; set; }
    }
}
