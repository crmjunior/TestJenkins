using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowHistorico
    {
        public int intWorkflowHistoricoId { get; set; }
        public int intWorkflowInstanciaId { get; set; }
        public int? intWorkflowEtapaAnteriorId { get; set; }
        public int? intWorkflowAcaoId { get; set; }
        public int intWorkflowEtapaPosteriorId { get; set; }
        public DateTime dteHistorico { get; set; }
        public int intUsuarioId { get; set; }
        public string txtJustificativa { get; set; }

        public virtual tblRequisicoes_WorkflowAcao intWorkflowAcao { get; set; }
        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapaAnterior { get; set; }
        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapaPosterior { get; set; }
    }
}
