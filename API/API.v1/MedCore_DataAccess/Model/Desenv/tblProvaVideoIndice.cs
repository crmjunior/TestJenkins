using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProvaVideoIndice
    {
        public tblProvaVideoIndice()
        {
            tblProvaVideo = new HashSet<tblProvaVideo>();
        }

        public int intProvaVideoIndiceId { get; set; }
        public int intLessonTitleId { get; set; }
        public int intOrdem { get; set; }
        public DateTime dteCadastro { get; set; }

        public virtual tblLessonTitles intLessonTitle { get; set; }
        public virtual ICollection<tblProvaVideo> tblProvaVideo { get; set; }
    }
}
