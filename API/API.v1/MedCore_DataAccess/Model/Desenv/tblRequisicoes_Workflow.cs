using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Workflow
    {
        public tblRequisicoes_Workflow()
        {
            tblRequisicoes_WorkflowEtapa = new HashSet<tblRequisicoes_WorkflowEtapa>();
            tblRequisicoes_Workflow_Requisicao = new HashSet<tblRequisicoes_Workflow_Requisicao>();
        }

        public int intWorkflowId { get; set; }
        public string txtDescricao { get; set; }
        public int intWorkflowCategoriaId { get; set; }
        public int intStatusId { get; set; }

        public virtual tblRequisicoes_WorkflowCategoria intWorkflowCategoria { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowEtapa> tblRequisicoes_WorkflowEtapa { get; set; }
        public virtual ICollection<tblRequisicoes_Workflow_Requisicao> tblRequisicoes_Workflow_Requisicao { get; set; }
    }
}
