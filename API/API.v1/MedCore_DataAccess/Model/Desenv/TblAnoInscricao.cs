using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAnoInscricao
    {
        public tblAnoInscricao()
        {
            tblClassificacaoTurmaConvidada = new HashSet<tblClassificacaoTurmaConvidada>();
        }

        public int intAnoInscricaoId { get; set; }
        public int intAno { get; set; }
        public int intApplicationID { get; set; }
        public string txtDescricao { get; set; }

        public virtual tblAccess_Application intApplication { get; set; }
        public virtual ICollection<tblClassificacaoTurmaConvidada> tblClassificacaoTurmaConvidada { get; set; }
    }
}
