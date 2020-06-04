using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Workflow_Requisicao
    {
        public int intWorkflowRequisicaoId { get; set; }
        public int intWorkflowId { get; set; }
        public int intRequisicaoId { get; set; }

        public virtual tblRequisicoes_Requisicao intRequisicao { get; set; }
        public virtual tblRequisicoes_Workflow intWorkflow { get; set; }
    }
}
