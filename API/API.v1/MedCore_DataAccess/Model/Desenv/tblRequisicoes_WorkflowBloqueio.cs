using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowBloqueio
    {
        public int intWorkflowBloqueioId { get; set; }
        public int intWorkflowEtapaId { get; set; }
        public int intBloqueioTipoId { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapa { get; set; }
    }
}
