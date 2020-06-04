using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBlacklistAprovacoes_Bloqueios
    {
        public int intBlacklistAprovacoesBloqueiosID { get; set; }
        public int intClientBlackListID { get; set; }
        public int intTipo { get; set; }
        public string txtMotivo { get; set; }
        public DateTime? dteInclusaoBloqueio { get; set; }
        public int? intSolicitadorId { get; set; }
    }
}
