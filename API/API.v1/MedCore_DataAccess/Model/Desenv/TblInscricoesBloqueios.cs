using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoesBloqueios
    {
        public int intID { get; set; }
        public string txtRegister { get; set; }
        public int intTypeID { get; set; }
        public DateTime? dteInclusaoBloqueio { get; set; }
        public string txtMotivo { get; set; }
        public int? intSolicitadorId { get; set; }
        public int? intAutorizadorId { get; set; }
        public string txtMotivoDesbloqueio { get; set; }
        public DateTime? dteDateTimeEnd { get; set; }
        public int? intBlackListCategoriaID { get; set; }

        public virtual tblInscricoesBloqueiosTipos intType { get; set; }
    }
}
