using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRodadaAluno
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public int intRodadaId { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblRodadaAvaliacao intRodada { get; set; }
    }
}
