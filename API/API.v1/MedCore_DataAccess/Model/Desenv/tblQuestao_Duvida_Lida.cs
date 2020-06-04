using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida_Lida
    {
        public int intDuvidaQuestaoLidaID { get; set; }
        public int intEmployeeID { get; set; }
        public int intQuestaoDuvidaID { get; set; }
        public bool bitLido { get; set; }
    }
}
