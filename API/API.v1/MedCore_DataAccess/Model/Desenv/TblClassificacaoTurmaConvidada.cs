using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClassificacaoTurmaConvidada
    {
        public int intClassificacaoTurmaConvidadaId { get; set; }
        public int intClassificationID { get; set; }
        public int intAnoInscricaoId { get; set; }

        public virtual tblAnoInscricao intAnoInscricao { get; set; }
        public virtual tblClassifications intClassification { get; set; }
    }
}
