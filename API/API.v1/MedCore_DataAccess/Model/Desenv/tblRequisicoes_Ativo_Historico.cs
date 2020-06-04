using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Ativo_Historico
    {
        public int intRequisicaoAtivoHistoricoId { get; set; }
        public int intAtivoId { get; set; }
        public DateTime dteData { get; set; }
        public int intEmployeeId { get; set; }
        public string txtMudancas { get; set; }
        public int intTipo { get; set; }

        public virtual tblRequisicoes_Ativo intAtivo { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
    }
}
