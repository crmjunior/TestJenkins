using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProcedimentoEntrada
    {
        public int intProcedimentoEntradaId { get; set; }
        public int intTipoProcedimentoEntradaId { get; set; }
        public int intCursoId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public virtual tblTipoProcedimentoEntrada intTipoProcedimentoEntrada { get; set; }
    }
}
