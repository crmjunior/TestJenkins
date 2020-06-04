using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_Guia
    {
        public int intGuiaID { get; set; }
        public int intTipoImpostoID { get; set; }
        public int intNotaID { get; set; }
        public int intStatusID { get; set; }
        public DateTime dteDataVencimento { get; set; }
        public DateTime? dteDataPagamento { get; set; }
        public double dblValor { get; set; }
        public int intUsuarioID { get; set; }
        public DateTime dteDataAlteracao { get; set; }

        public virtual tblMedNotas_Nota intNota { get; set; }
        public virtual tblMedNotas_TipoImposto intTipoImposto { get; set; }
    }
}
