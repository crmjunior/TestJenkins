using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_LogGuia
    {
        public int intLogGuiaID { get; set; }
        public int intGuiaID { get; set; }
        public int intTipoImpostoID { get; set; }
        public int intNotaID { get; set; }
        public DateTime dteDataVencimento { get; set; }
        public DateTime? dteDataPagamento { get; set; }
        public int intStatusID { get; set; }
        public double dblValor { get; set; }
        public int? intUsuarioId { get; set; }
        public DateTime dteDataAlteracao { get; set; }
    }
}
