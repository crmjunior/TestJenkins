using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAdaptaMedAulaTemaProfessorAssistido
    {
        public int intTemaProfessorAssistidoID { get; set; }
        public int intLessonTitleID { get; set; }
        public int intProfessorID { get; set; }
        public int intClientID { get; set; }
        public int intPercentVisualizado { get; set; }
    }
}
