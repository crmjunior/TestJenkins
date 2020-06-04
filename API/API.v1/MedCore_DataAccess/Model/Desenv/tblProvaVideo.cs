using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProvaVideo
    {
        public tblProvaVideo()
        {
            tblLessonsEvaluationProvaVideo = new HashSet<tblLessonsEvaluationProvaVideo>();
        }

        public int intProvaVideoId { get; set; }
        public int intProvaVideoIndiceId { get; set; }
        public int intProfessorId { get; set; }
        public string txtDescricao { get; set; }
        public int intOrdem { get; set; }
        public int intVideoId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
        public DateTime dteLiberacao { get; set; }
        public bool bitPossuiAnexo { get; set; }

        public virtual tblProvaVideoIndice intProvaVideoIndice { get; set; }
        public virtual ICollection<tblLessonsEvaluationProvaVideo> tblLessonsEvaluationProvaVideo { get; set; }
    }
}
