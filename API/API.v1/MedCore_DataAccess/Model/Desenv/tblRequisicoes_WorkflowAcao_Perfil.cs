using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowAcao_Perfil
    {
        public int intWorkflowAcaoPerfilId { get; set; }
        public int intWorkflowAcaoId { get; set; }
        public int intPerfilId { get; set; }

        public virtual tblRequisicoes_Perfil intPerfil { get; set; }
        public virtual tblRequisicoes_WorkflowAcao intWorkflowAcao { get; set; }
    }
}
