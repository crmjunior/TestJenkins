using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacaoAluno
    {
        public int intId { get; set; }
        public int intClientId { get; set; }
        public int intNotificacaoId { get; set; }
        public DateTime? dteLida { get; set; }
    }
}
