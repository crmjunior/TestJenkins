using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_RequisicaoHistorico
    {
        public int intRequisicaoHistoricoId { get; set; }
        public int intRequisicaoId { get; set; }
        public string txtTitulo { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteData { get; set; }
        public int intEmployeeId { get; set; }
        public int intTipo { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblRequisicoes_Requisicao intRequisicao { get; set; }
    }
}
