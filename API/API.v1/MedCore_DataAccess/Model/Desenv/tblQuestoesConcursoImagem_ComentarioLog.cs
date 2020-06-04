using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestoesConcursoImagem_ComentarioLog
    {
        public int IntComentarioLogId { get; set; }
        public int intImagemComentarioID { get; set; }
        public int intQuestaoId { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime? dtDataAcao { get; set; }
        public string txtName { get; set; }
        public int intAcao { get; set; }
    }
}
