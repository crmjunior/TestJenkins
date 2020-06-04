using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideo
    {
        public int intRevisaoAulaId { get; set; }
        public int intRevisaoAulaIdPai { get; set; }
        public int intProfessorId { get; set; }
        public int intRevisaoAulaIndiceId { get; set; }
        public string txtDescricao { get; set; }
        public int? intOrdem { get; set; }
        public int? intCuePoint { get; set; }
        public DateTime dteCadastro { get; set; }
        public int? intVideoId { get; set; }
        public string txtTemp_nomeVideo { get; set; }
    }
}
