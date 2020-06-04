using System;

namespace MedCore_DataAccess.Entidades
{
    public partial class csp_ListaMaterialDireitoAluno_Result
    {
        public int? intMaterialID { get; set; }
        public long? intBookEntityID { get; set; }
        public int? intSemana { get; set; }
        public string dataInicio { get; set; }
        public string datafim { get; set; }
        public string horaInicio { get; set; }
        public int? anoCronograma { get; set; }
        public int? anoCursado { get; set; }
        public int blnPermitido { get; set; }
        public string txtName { get; set; }
        public int? intLessonTitleID { get; set; }
    }
}