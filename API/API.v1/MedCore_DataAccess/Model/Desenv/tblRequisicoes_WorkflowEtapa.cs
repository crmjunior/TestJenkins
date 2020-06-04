using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowEtapa
    {
        public tblRequisicoes_WorkflowEtapa()
        {
            tblRequisicoes_WorkflowAcaointWorkflowEtapa = new HashSet<tblRequisicoes_WorkflowAcao>();
            tblRequisicoes_WorkflowAcaointWorkflowEtapaProximo = new HashSet<tblRequisicoes_WorkflowAcao>();
            tblRequisicoes_WorkflowBloqueio = new HashSet<tblRequisicoes_WorkflowBloqueio>();
            tblRequisicoes_WorkflowEtapa_Perfil = new HashSet<tblRequisicoes_WorkflowEtapa_Perfil>();
            tblRequisicoes_WorkflowHistoricointWorkflowEtapaAnterior = new HashSet<tblRequisicoes_WorkflowHistorico>();
            tblRequisicoes_WorkflowHistoricointWorkflowEtapaPosterior = new HashSet<tblRequisicoes_WorkflowHistorico>();
            tblRequisicoes_WorkflowRegra = new HashSet<tblRequisicoes_WorkflowRegra>();
        }

        public int intWorkflowEtapaId { get; set; }
        public int intWorkflowId { get; set; }
        public int intEtapaTipoId { get; set; }
        public string txtDescricao { get; set; }
        public string txtStatus { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblRequisicoes_Workflow intWorkflow { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowAcao> tblRequisicoes_WorkflowAcaointWorkflowEtapa { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowAcao> tblRequisicoes_WorkflowAcaointWorkflowEtapaProximo { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowBloqueio> tblRequisicoes_WorkflowBloqueio { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowEtapa_Perfil> tblRequisicoes_WorkflowEtapa_Perfil { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowHistorico> tblRequisicoes_WorkflowHistoricointWorkflowEtapaAnterior { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowHistorico> tblRequisicoes_WorkflowHistoricointWorkflowEtapaPosterior { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowRegra> tblRequisicoes_WorkflowRegra { get; set; }
    }
}
