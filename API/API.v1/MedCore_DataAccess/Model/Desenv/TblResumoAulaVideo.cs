using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblResumoAulaVideo
    {
        public int intResumoAulaVideoId { get; set; }
        public int intResumoAulaIndiceId { get; set; }
        public int intProfessorId { get; set; }
        public string txtDescricao { get; set; }
        public int? intOrdem { get; set; }
        public int intVideoId { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
