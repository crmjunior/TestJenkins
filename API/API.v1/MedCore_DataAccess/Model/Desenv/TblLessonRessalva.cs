using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLessonRessalva
    {
        public tblLessonRessalva()
        {
            tblLessonxRessalva = new HashSet<tblLessonxRessalva>();
        }

        public int intLessonRessalvaID { get; set; }
        public int? intLessonID { get; set; }
        public string txtAssunto { get; set; }
        public string txtRessalva { get; set; }
        public int intEnderecoID { get; set; }
        public bool bitExibirTodoCurso { get; set; }

        public virtual tblLessons intLesson { get; set; }
        public virtual ICollection<tblLessonxRessalva> tblLessonxRessalva { get; set; }
    }
}
