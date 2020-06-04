using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmployees
    {
        public tblEmployees()
        {
            InverseintGestor = new HashSet<tblEmployees>();
            tblAccess_Object = new HashSet<tblAccess_Object>();
            tblAccess_Permission_Rule = new HashSet<tblAccess_Permission_Rule>();
            tblAccess_Rule = new HashSet<tblAccess_Rule>();
            tblAnexo = new HashSet<tblAnexo>();
            tblCallCenterEvents = new HashSet<tblCallCenterEvents>();
            tblClientsDocuments = new HashSet<tblClientsDocuments>();
            tblConcursoQuestao_Classificacao_Autorizacao = new HashSet<tblConcursoQuestao_Classificacao_Autorizacao>();
            tblContribuicao_EncaminhadasintClient = new HashSet<tblContribuicao_Encaminhadas>();
            tblContribuicao_EncaminhadasintEmployee = new HashSet<tblContribuicao_Encaminhadas>();
            tblCronogramaPrateleira = new HashSet<tblCronogramaPrateleira>();
            tblDuvidasAcademicas_DuvidasEncaminhadasintEmployee = new HashSet<tblDuvidasAcademicas_DuvidasEncaminhadas>();
            tblDuvidasAcademicas_DuvidasEncaminhadasintGestor = new HashSet<tblDuvidasAcademicas_DuvidasEncaminhadas>();
            tblEmployee_Sector = new HashSet<tblEmployee_Sector>();
            tblGaleriaImagem = new HashSet<tblGaleriaImagem>();
            tblGaleriaImagemApostila = new HashSet<tblGaleriaImagemApostila>();
            tblGaleriaRelacaoImagem = new HashSet<tblGaleriaRelacaoImagem>();
            tblImagemGaleria = new HashSet<tblImagemGaleria>();
            tblLessonTeacherSubstituto = new HashSet<tblLessonTeacherSubstituto>();
            //tblLessonTeachersByClassRoom = new HashSet<tblLessonTeachersByClassRoom>();
            //tblLessonTeachersByGroupAndTitle = new HashSet<tblLessonTeachersByGroupAndTitle>();
            tblLogGaleriaImagem = new HashSet<tblLogGaleriaImagem>();
            tblMaterialOrdersGalpaoRomaneio = new HashSet<tblMaterialOrdersGalpaoRomaneio>();
            tblMedcode_DataMatrix = new HashSet<tblMedcode_DataMatrix>();
            tblPerfil_Employees = new HashSet<tblPerfil_Employees>();
            tblPerfil_Employees1 = new HashSet<tblPerfil_Employees1>();
            tblPrestacaoContasGestorXsubordinadosintGestor = new HashSet<tblPrestacaoContasGestorXsubordinados>();
            tblPrestacaoContasGestorXsubordinadosintSubordinado = new HashSet<tblPrestacaoContasGestorXsubordinados>();
            tblQuestao_Duvida_Moderada = new HashSet<tblQuestao_Duvida_Moderada>();
            tblRPA_Ativo_Historico = new HashSet<tblRPA_Ativo_Historico>();
            tblRequisicoes_Anexo = new HashSet<tblRequisicoes_Anexo>();
            tblRequisicoes_AtivoMovimentacao = new HashSet<tblRequisicoes_AtivoMovimentacao>();
            tblRequisicoes_Ativo_Historico = new HashSet<tblRequisicoes_Ativo_Historico>();
            tblRequisicoes_PerfilItem = new HashSet<tblRequisicoes_PerfilItem>();
            tblRequisicoes_Produto_Historico = new HashSet<tblRequisicoes_Produto_Historico>();
            tblRequisicoes_RequisicaoCotacao = new HashSet<tblRequisicoes_RequisicaoCotacao>();
            tblRequisicoes_RequisicaoHistorico = new HashSet<tblRequisicoes_RequisicaoHistorico>();
            tblRequisicoes_RequisicaointAtribuido = new HashSet<tblRequisicoes_Requisicao>();
            tblRequisicoes_RequisicaointRequisitante = new HashSet<tblRequisicoes_Requisicao>();
            tblRequisicoes_RequisicaointResponsavel = new HashSet<tblRequisicoes_Requisicao>();
            tblRevisaoAulaVideoAprovacaoLog = new HashSet<tblRevisaoAulaVideoAprovacaoLog>();
            tblRevisaoAula_Slides = new HashSet<tblRevisaoAula_Slides>();
            tblWarehouses = new HashSet<tblWarehouses>();
        }

        public int intEmployeeID { get; set; }
        public int? intPositionID { get; set; }
        public int? intAccess { get; set; }
        public string txtLogin { get; set; }
        public string txtPassword { get; set; }
        public int? intCompanyID { get; set; }
        public int? intSectorID { get; set; }
        public string txtSocialCard { get; set; }
        public int? intPaymentMethodID { get; set; }
        public string txtPIS { get; set; }
        public string txtSerie { get; set; }
        public double? dblBaseSalary { get; set; }
        public double? dblGratuity { get; set; }
        public double? dblGrossSalary { get; set; }
        public double? dblModifySalary { get; set; }
        public int? intContractType { get; set; }
        public double? dblHourValue { get; set; }
        public int intResponsabilityID { get; set; }
        public DateTime? dteDateSavePassword { get; set; }
        public bool? bitActiveEmployee { get; set; }
        public string txtObs { get; set; }
        public int? intStatus { get; set; }
        public DateTime? dteDateFirstDay { get; set; }
        public DateTime? dteDateTimeStart { get; set; }
        public DateTime? dteDateTimeEnd { get; set; }
        public string txtConscriptionDocument { get; set; }
        public string txtWorkCardDocument { get; set; }
        public string txtVoterRegistrationCardDocument { get; set; }
        public string txtBankAccountInformation { get; set; }
        public int? intCargo { get; set; }
        public bool bitTrainee { get; set; }
        public string txtMatriculationStatement { get; set; }
        public int? intCTPSstatus { get; set; }
        public string txtCollegeName { get; set; }
        public string txtCollegePeriod { get; set; }
        public string txtCollegeConclusion { get; set; }
        public string txtCourse { get; set; }
        public bool? bitTCESigned { get; set; }
        public DateTime? dteDateTCEMaturity { get; set; }
        public DateTime? dteDateTwoYearsCompletion { get; set; }
        public bool? bitLifeInsurance { get; set; }
        public string txtBankAccountInformation2 { get; set; }
        public bool? bitExame { get; set; }
        public DateTime? dteDateExame { get; set; }
        public int? intTipoExame { get; set; }
        public DateTime? dteDateDesligamento { get; set; }
        public int? intStoreID { get; set; }
        public bool bitIsRecebeMaterial { get; set; }
        public bool bitTijuca { get; set; }
        public int? intGestorID { get; set; }
        public bool? bitOptanteVT { get; set; }
        public double? dblValeTransporte { get; set; }
        public bool? bitContratoDeterminado { get; set; }
        public DateTime? dteDateContrato { get; set; }

        public virtual tblEmployeeCargos intCargoNavigation { get; set; }
        public virtual tblCompanies intCompany { get; set; }
        public virtual tblPersons intEmployee { get; set; }
        public virtual tblEmployees intGestor { get; set; }
        public virtual tblSysRoles intResponsability { get; set; }
        public virtual tblCompanySectors intSector { get; set; }
        public virtual tblStores intStore { get; set; }
        public virtual ICollection<tblEmployees> InverseintGestor { get; set; }
        public virtual ICollection<tblAccess_Object> tblAccess_Object { get; set; }
        public virtual ICollection<tblAccess_Permission_Rule> tblAccess_Permission_Rule { get; set; }
        public virtual ICollection<tblAccess_Rule> tblAccess_Rule { get; set; }
        public virtual ICollection<tblAnexo> tblAnexo { get; set; }
        public virtual ICollection<tblCallCenterEvents> tblCallCenterEvents { get; set; }
        public virtual ICollection<tblClientsDocuments> tblClientsDocuments { get; set; }
        public virtual ICollection<tblConcursoQuestao_Classificacao_Autorizacao> tblConcursoQuestao_Classificacao_Autorizacao { get; set; }
        public virtual ICollection<tblContribuicao_Encaminhadas> tblContribuicao_EncaminhadasintClient { get; set; }
        public virtual ICollection<tblContribuicao_Encaminhadas> tblContribuicao_EncaminhadasintEmployee { get; set; }
        public virtual ICollection<tblCronogramaPrateleira> tblCronogramaPrateleira { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidasEncaminhadas> tblDuvidasAcademicas_DuvidasEncaminhadasintEmployee { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidasEncaminhadas> tblDuvidasAcademicas_DuvidasEncaminhadasintGestor { get; set; }
        public virtual ICollection<tblEmployee_Sector> tblEmployee_Sector { get; set; }
        public virtual ICollection<tblGaleriaImagem> tblGaleriaImagem { get; set; }
        public virtual ICollection<tblGaleriaImagemApostila> tblGaleriaImagemApostila { get; set; }
        public virtual ICollection<tblGaleriaRelacaoImagem> tblGaleriaRelacaoImagem { get; set; }
        public virtual ICollection<tblImagemGaleria> tblImagemGaleria { get; set; }
        public virtual ICollection<tblLessonTeacherSubstituto> tblLessonTeacherSubstituto { get; set; }
        //public virtual ICollection<tblLessonTeachersByClassRoom> tblLessonTeachersByClassRoom { get; set; }
        //public virtual ICollection<tblLessonTeachersByGroupAndTitle> tblLessonTeachersByGroupAndTitle { get; set; }
        public virtual ICollection<tblLogGaleriaImagem> tblLogGaleriaImagem { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
        public virtual ICollection<tblMedcode_DataMatrix> tblMedcode_DataMatrix { get; set; }
        public virtual ICollection<tblPerfil_Employees> tblPerfil_Employees { get; set; }
        public virtual ICollection<tblPerfil_Employees1> tblPerfil_Employees1 { get; set; }
        public virtual ICollection<tblPrestacaoContasGestorXsubordinados> tblPrestacaoContasGestorXsubordinadosintGestor { get; set; }
        public virtual ICollection<tblPrestacaoContasGestorXsubordinados> tblPrestacaoContasGestorXsubordinadosintSubordinado { get; set; }
        public virtual ICollection<tblQuestao_Duvida_Moderada> tblQuestao_Duvida_Moderada { get; set; }
        public virtual ICollection<tblRPA_Ativo_Historico> tblRPA_Ativo_Historico { get; set; }
        public virtual ICollection<tblRequisicoes_Anexo> tblRequisicoes_Anexo { get; set; }
        public virtual ICollection<tblRequisicoes_AtivoMovimentacao> tblRequisicoes_AtivoMovimentacao { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo_Historico> tblRequisicoes_Ativo_Historico { get; set; }
        public virtual ICollection<tblRequisicoes_PerfilItem> tblRequisicoes_PerfilItem { get; set; }
        public virtual ICollection<tblRequisicoes_Produto_Historico> tblRequisicoes_Produto_Historico { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoCotacao> tblRequisicoes_RequisicaoCotacao { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoHistorico> tblRequisicoes_RequisicaoHistorico { get; set; }
        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_RequisicaointAtribuido { get; set; }
        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_RequisicaointRequisitante { get; set; }
        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_RequisicaointResponsavel { get; set; }
        public virtual ICollection<tblRevisaoAulaVideoAprovacaoLog> tblRevisaoAulaVideoAprovacaoLog { get; set; }
        public virtual ICollection<tblRevisaoAula_Slides> tblRevisaoAula_Slides { get; set; }
        public virtual ICollection<tblWarehouses> tblWarehouses { get; set; }
    }
}
