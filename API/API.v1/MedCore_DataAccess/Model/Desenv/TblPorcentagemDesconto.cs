using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPorcentagemDesconto
    {
        public tblPorcentagemDesconto()
        {
            tblTemplateDescontoTurmaCPMED = new HashSet<tblTemplateDescontoTurmaCPMED>();
        }

        public int intPorcentagemDescontoId { get; set; }
        public int txtPorcentagem { get; set; }

        public virtual ICollection<tblTemplateDescontoTurmaCPMED> tblTemplateDescontoTurmaCPMED { get; set; }
    }
}
