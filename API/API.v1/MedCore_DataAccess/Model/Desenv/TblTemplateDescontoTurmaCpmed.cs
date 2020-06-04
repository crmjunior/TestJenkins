using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblTemplateDescontoTurmaCPMED
    {
        public int intTemplateCPMEDId { get; set; }
        public int intTemplateId { get; set; }
        public int intPorcentagemDescontoId { get; set; }

        public virtual tblPorcentagemDesconto intPorcentagemDesconto { get; set; }
    }
}
