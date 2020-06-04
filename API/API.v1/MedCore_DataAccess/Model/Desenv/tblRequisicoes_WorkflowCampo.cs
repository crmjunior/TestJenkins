using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_WorkflowCampo
    {
        public tblRequisicoes_WorkflowCampo()
        {
            tblRequisicoes_WorkflowRegra = new HashSet<tblRequisicoes_WorkflowRegra>();
        }

        public int intWorkflowCampoId { get; set; }
        public int intCampoTipoId { get; set; }
        public int intOrdem { get; set; }
        public string txtNome { get; set; }
        public string txtObjetoMapeado { get; set; }
        public string txtEntidadeReferenciada { get; set; }
        public string txtPropriedadeMapeada { get; set; }
        public string txtAtributoReferenciado { get; set; }
        public string txtMetdoAPI { get; set; }
        public bool bitAtivo { get; set; }

        public virtual ICollection<tblRequisicoes_WorkflowRegra> tblRequisicoes_WorkflowRegra { get; set; }
    }
}
