using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Provas_Tipos
    {
        public int intProvaTipoID { get; set; }
        public string txtDescription { get; set; }
        public int? IntOrder { get; set; }
        public bool? bitDiscursiva { get; set; }
    }
}
