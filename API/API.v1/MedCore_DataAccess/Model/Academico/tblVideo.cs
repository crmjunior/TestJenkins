using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblVideo
    {
        public tblVideo()
        {
            tblVideo_Questao_Simulado = new HashSet<tblVideo_Questao_Simulado>();
        }

        public int intVideoID { get; set; }
        public int intStatusID { get; set; }
        public string intDuracao { get; set; }
        public string txtSubject { get; set; }
        public string txtDescription { get; set; }
        public string txtName { get; set; }
        public string txtPath { get; set; }
        public string txtFileName { get; set; }
        public DateTime dteCreationDate { get; set; }
        public DateTime dteLastModifyDate { get; set; }
        public int intEmployeeID { get; set; }
        public string txtUploadSource { get; set; }
        public int? intSequence { get; set; }
        public string txtReferenceName { get; set; }
        public Guid guidVideoID { get; set; }
        public bool? bitActive { get; set; }
        public int? intVimeoID { get; set; }
        public string txtUrlVimeo { get; set; }
        public string txtUrlThumbVimeo { get; set; }
        public string txtUrlStreamVimeo { get; set; }
        public string txtVideoInfo { get; set; }

        public virtual ICollection<tblVideo_Questao_Simulado> tblVideo_Questao_Simulado { get; set; }
    }
}
