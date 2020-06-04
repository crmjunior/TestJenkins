using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowEtapa_Perfil
    {
        public int intWorkflowEtapaPerfilId { get; set; }
        public int intWorkflowEtapaId { get; set; }
        public int intPerfilId { get; set; }

        public virtual tblRequisicoes_Perfil intPerfil { get; set; }
        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapa { get; set; }
    }
}
