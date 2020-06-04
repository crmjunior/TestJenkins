using System;

namespace MedCore_DataAccess.Entidades
{
    public partial class msp_API_ListaEntidades_Result
    {
        public long intID { get; set; }
        public string entidade { get; set; }
        public Nullable<int> intSemana { get; set; }
        public Nullable<int> intYear { get; set; }
        public string dataInicio { get; set; }
        public string datafim { get; set; }
        public int intLessonTitleID { get; set; }
        public string txtName { get; set; }
        public int intMaterialID { get; set; }
        public string txtCode { get; set; }
        public string txtLessonTitleName { get; set; }
        public string txtDescription { get; set; }
        public int intLessonSubjectID { get; set; }
    }
}