using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowAcao
    {
        public tblRequisicoes_WorkflowAcao()
        {
            tblRequisicoes_WorkflowAcao_Perfil = new HashSet<tblRequisicoes_WorkflowAcao_Perfil>();
            tblRequisicoes_WorkflowHistorico = new HashSet<tblRequisicoes_WorkflowHistorico>();
            tblRequisicoes_WorkflowRegra = new HashSet<tblRequisicoes_WorkflowRegra>();
        }

        public int intWorkflowAcaoId { get; set; }
        public int intWorkflowEtapaId { get; set; }
        public int intWorkflowEtapaProximoId { get; set; }
        public int intAcaoTipoId { get; set; }
        public string txtAcao { get; set; }
        public string txtIcone { get; set; }
        public string txtCor { get; set; }
        public bool bitAtivo { get; set; }
        public bool bitJustificativa { get; set; }

        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapa { get; set; }
        public virtual tblRequisicoes_WorkflowEtapa intWorkflowEtapaProximo { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowAcao_Perfil> tblRequisicoes_WorkflowAcao_Perfil { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowHistorico> tblRequisicoes_WorkflowHistorico { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowRegra> tblRequisicoes_WorkflowRegra { get; set; }
    }
}
