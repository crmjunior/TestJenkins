using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContratoAceite
    {
        public int intContratoAceite { get; set; }
        public int? intContratoAlunoAceite { get; set; }
        public string txtCriptoPDF { get; set; }
        public string txtUrlPDFContrato { get; set; }
        public string txtCriptoDadosAluno { get; set; }
        public int intContratoId { get; set; }

        public virtual tblContratoAlunoAceite intContratoAlunoAceiteNavigation { get; set; }
    }
}
