using System;

namespace MedCore_DataAccess.Entidades
{
    public partial class csp_ListaMaterialDireitoAluno
    {
        public Nullable<int> intMaterialID { get; set; }
        public Nullable<long> intBookEntityID { get; set; }
        public Nullable<int> intSemana { get; set; }
        public string dataInicio { get; set; }
        public string datafim { get; set; }
        public string horaInicio { get; set; }
        public Nullable<int> anoCronograma { get; set; }
        public Nullable<int> anoCursado { get; set; }
        public int blnPermitido { get; set; }
        public string txtName { get; set; }
        public Nullable<int> intLessonTitleID { get; set; }
    }
}