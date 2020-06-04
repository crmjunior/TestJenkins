using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Requisicao
    {
        public tblRequisicoes_Requisicao()
        {
            tblRequisicoes_IntranetLink = new HashSet<tblRequisicoes_IntranetLink>();
            tblRequisicoes_RequisicaoCotacao = new HashSet<tblRequisicoes_RequisicaoCotacao>();
            tblRequisicoes_RequisicaoHistorico = new HashSet<tblRequisicoes_RequisicaoHistorico>();
            tblRequisicoes_Workflow_Requisicao = new HashSet<tblRequisicoes_Workflow_Requisicao>();
        }

        public int intRequisicaoId { get; set; }
        public int intRequisicaoTipoId { get; set; }
        public int? intRequisicaoStatusId { get; set; }
        public DateTime dteRequisicao { get; set; }
        public int intRequisicaoUnidadeId { get; set; }
        public int intRequisicaoSetorId { get; set; }
        public int? intRequisicaoCursoId { get; set; }
        public int intRequisitanteId { get; set; }
        public int intResponsavelId { get; set; }
        public bool? bitAtivo { get; set; }
        public int? intRequisicaoItemId { get; set; }
        public decimal? dblValor { get; set; }
        public int? intIntranetId { get; set; }
        public string txtIntranetLink { get; set; }
        public DateTime? dtePrevisaoConclusao { get; set; }
        public int? intAtribuidoId { get; set; }
        public string txtDescricao { get; set; }
        public string txtJustificativaCotacao { get; set; }
        public bool? bitRequisicaoEstoque { get; set; }
        public int? intRequisicaoUsuarioId { get; set; }
        public int? intCompanyIdOriginal { get; set; }
        public int? intCompanyId { get; set; }

        public virtual tblEmployees intAtribuido { get; set; }
        public virtual tblCompanies intCompany { get; set; }
        public virtual tblCompanies intCompanyIdOriginalNavigation { get; set; }
        public virtual tblRequisicoes_Curso intRequisicaoCurso { get; set; }
        public virtual tblRequisicoes_RequisicaoStatus intRequisicaoStatus { get; set; }
        public virtual tblEmployees intRequisitante { get; set; }
        public virtual tblEmployees intResponsavel { get; set; }
        public virtual ICollection<tblRequisicoes_IntranetLink> tblRequisicoes_IntranetLink { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoCotacao> tblRequisicoes_RequisicaoCotacao { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoHistorico> tblRequisicoes_RequisicaoHistorico { get; set; }
        public virtual ICollection<tblRequisicoes_Workflow_Requisicao> tblRequisicoes_Workflow_Requisicao { get; set; }
    }
}
