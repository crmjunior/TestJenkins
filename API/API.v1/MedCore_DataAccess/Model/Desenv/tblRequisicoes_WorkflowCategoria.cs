using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowCategoria
    {
        public tblRequisicoes_WorkflowCategoria()
        {
            tblRequisicoes_Workflow = new HashSet<tblRequisicoes_Workflow>();
        }

        public int intWorkflowCategoriaId { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblRequisicoes_Workflow> tblRequisicoes_Workflow { get; set; }
    }
}
