using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_ProvaCasoClinico
    {
        public int intCasoClinicoID { get; set; }
        public int intQuestoesDe { get; set; }
        public int intQuestoesAte { get; set; }
        public string txtTexto { get; set; }
        public int intProvaID { get; set; }
    }
}
