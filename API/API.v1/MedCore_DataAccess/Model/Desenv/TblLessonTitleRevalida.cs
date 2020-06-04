using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonTitleRevalida
    {
        public int intLessonTitleRevalidaId { get; set; }
        public string txtName { get; set; }
        public int? intEspecialidadeId { get; set; }
        public int? GrupoId { get; set; }
    }
}
