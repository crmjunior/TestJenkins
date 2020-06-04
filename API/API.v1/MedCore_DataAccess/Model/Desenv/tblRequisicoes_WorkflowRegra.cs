using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowRegra
    {
        public int intWorkflowRegraId { get; set; }
        public int intWorkflowCampoId { get; set; }
        public int intOperadorTipoId { get; set; }
        public int intOrdem { get; set; }
        public string txtValor { get; set; }
        public int intWorkflowEtapaId { get; set; }
        public int intWorkflowAcaoId { get; set; }

        public virtual tblRequisicoes_WorkflowAcao intWorkflowAcao { get; set; }
        public virtual tblRequisicoes_WorkflowCampo intWorkflowCampo { get; set; }
        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapa { get; set; }
    }
}
