using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Perfil
    {
        public tblRequisicoes_Perfil()
        {
            tblRequisicoes_PerfilItem = new HashSet<tblRequisicoes_PerfilItem>();
            tblRequisicoes_WorkflowAcao_Perfil = new HashSet<tblRequisicoes_WorkflowAcao_Perfil>();
            tblRequisicoes_WorkflowEtapa_Perfil = new HashSet<tblRequisicoes_WorkflowEtapa_Perfil>();
        }

        public int intPerfilId { get; set; }
        public string txtNome { get; set; }
        public bool bitAtivo { get; set; }
        public bool bitMaster { get; set; }
        public bool? bitAprovadorSubordinado { get; set; }
        public bool? bitAprovadorRequisitante { get; set; }

        public virtual ICollection<tblRequisicoes_PerfilItem> tblRequisicoes_PerfilItem { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowAcao_Perfil> tblRequisicoes_WorkflowAcao_Perfil { get; set; }
        public virtual ICollection<tblRequisicoes_WorkflowEtapa_Perfil> tblRequisicoes_WorkflowEtapa_Perfil { get; set; }
    }
}
