using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblTipoProcedimentoEntrada
    {
        public tblTipoProcedimentoEntrada()
        {
            tblProcedimentoEntrada = new HashSet<tblProcedimentoEntrada>();
        }

        public int intTipoProcedimentoEntradaId { get; set; }
        public string strTipoProcedimentoEntrada { get; set; }

        public virtual ICollection<tblProcedimentoEntrada> tblProcedimentoEntrada { get; set; }
    }
}
