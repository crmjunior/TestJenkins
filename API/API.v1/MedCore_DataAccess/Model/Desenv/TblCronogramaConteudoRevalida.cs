using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCronogramaConteudoRevalida
    {
        public int intRevalidaId { get; set; }
        public int? GrupoId { get; set; }
        public DateTime? dteInicio { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
