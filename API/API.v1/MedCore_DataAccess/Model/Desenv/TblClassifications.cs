using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClassifications
    {
        public tblClassifications()
        {
            tblClassificacaoTurmaConvidada = new HashSet<tblClassificacaoTurmaConvidada>();
        }

        public int intClassificationID { get; set; }
        public string txtDescription { get; set; }
        public bool? bitEdit { get; set; }
        public int? intActionPermissionID { get; set; }

        public virtual ICollection<tblClassificacaoTurmaConvidada> tblClassificacaoTurmaConvidada { get; set; }
    }
}
