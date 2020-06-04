using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCodigoComentario
    {
        public int IntID { get; set; }
        public int IntNumQuestaoID { get; set; }
        public int intQuestaoID { get; set; }
        public string Gabarito { get; set; }
        public int intComentarioID { get; set; }
        public int intApostilaID { get; set; }
        public int? intEmployeeID { get; set; }
        public DateTime? dteDataGravacao { get; set; }
        public int? intEmployeeEdicaoID { get; set; }
        public DateTime? dteDataEdicao { get; set; }
        public string GabaritoOriginal { get; set; }
    }
}
