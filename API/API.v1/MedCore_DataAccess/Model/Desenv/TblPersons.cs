using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPersons
    {
        public tblPersons()
        {
            tblApi_MensagemInadimplencia_Log = new HashSet<tblApi_MensagemInadimplencia_Log>();
            tblAvaliacaoConteudoQuestao = new HashSet<tblAvaliacaoConteudoQuestao>();
            tblBooksEntitiesProfessor = new HashSet<tblBooksEntitiesProfessor>();
            tblClientClassifications = new HashSet<tblClientClassifications>();
            tblClientsDocuments = new HashSet<tblClientsDocuments>();
            tblCompanySectorsintResponsible = new HashSet<tblCompanySectors>();
            tblCompanySectorsintSubstitute = new HashSet<tblCompanySectors>();
            tblConcursoQuestoes_Aluno_Favoritas = new HashSet<tblConcursoQuestoes_Aluno_Favoritas>();
            tblConcurso_Provas_ArquivosAluno = new HashSet<tblConcurso_Provas_ArquivosAluno>();
            tblContribuicao = new HashSet<tblContribuicao>();
            tblContribuicoes_Arquivadas = new HashSet<tblContribuicoes_Arquivadas>();
            tblContribuicoes_Interacao = new HashSet<tblContribuicoes_Interacao>();
            tblDeviceToken = new HashSet<tblDeviceToken>();
            tblDuvidasAcademicas_Denuncia = new HashSet<tblDuvidasAcademicas_Denuncia>();
            tblDuvidasAcademicas_DuvidasintAcademico = new HashSet<tblDuvidasAcademicas_Duvidas>();
            tblDuvidasAcademicas_DuvidasintClient = new HashSet<tblDuvidasAcademicas_Duvidas>();
            tblDuvidasAcademicas_Interacoes = new HashSet<tblDuvidasAcademicas_Interacoes>();
            tblDuvidasAcademicas_Log = new HashSet<tblDuvidasAcademicas_Log>();
            tblDuvidasAcademicas_Notificacao = new HashSet<tblDuvidasAcademicas_Notificacao>();
            tblDuvidasAcademicas_RespostaintAcademico = new HashSet<tblDuvidasAcademicas_Resposta>();
            tblDuvidasAcademicas_RespostaintClient = new HashSet<tblDuvidasAcademicas_Resposta>();
            tblEmailNotificacaoDuvidasAcademicas = new HashSet<tblEmailNotificacaoDuvidasAcademicas>();
            tblEnderecoEntregaCliente = new HashSet<tblEnderecoEntregaCliente>();
            tblEspecialidadeProfessor = new HashSet<tblEspecialidadeProfessor>();
            tblGrupo = new HashSet<tblGrupo>();
            tblLabels = new HashSet<tblLabels>();
            tblLessonsEvaluationintClient = new HashSet<tblLessonsEvaluation>();
            tblLessonsEvaluationintEmployeed = new HashSet<tblLessonsEvaluation>();
            tblLiberacaoApostilaAntecipada = new HashSet<tblLiberacaoApostilaAntecipada>();
            tblMaterialApostilaAluno = new HashSet<tblMaterialApostilaAluno>();
            tblMaterialApostilaAluno_Comentario = new HashSet<tblMaterialApostilaAluno_Comentario>();
            tblMaterialApostilaComentario = new HashSet<tblMaterialApostilaComentario>();
            tblMaterialApostilaInteracao = new HashSet<tblMaterialApostilaInteracao>();
            tblMaterialApostilaProgresso = new HashSet<tblMaterialApostilaProgresso>();
            tblMedsoftScreenshotReport = new HashSet<tblMedsoftScreenshotReport>();
            tblNotificacaoDuvidas = new HashSet<tblNotificacaoDuvidas>();
            tblPaymentDocuments = new HashSet<tblPaymentDocuments>();
            tblPersonsPicture = new HashSet<tblPersonsPicture>();
            tblPessoaGrupo = new HashSet<tblPessoaGrupo>();
            tblQuestao_Duvida_EncaminhamentointDestinatario = new HashSet<tblQuestao_Duvida_Encaminhamento>();
            tblQuestao_Duvida_EncaminhamentointRemetente = new HashSet<tblQuestao_Duvida_Encaminhamento>();
            tblRodadaAluno = new HashSet<tblRodadaAluno>();
            tblVideoVote = new HashSet<tblVideoVote>();
        }

        public int intContactID { get; set; }
        public string txtName { get; set; }
        public string txtRegister { get; set; }
        public int? intSex { get; set; }
        public DateTime? dteBirthday { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtZipCode { get; set; }
        public string txtPhone1 { get; set; }
        public string txtPhone2 { get; set; }
        public string txtCel { get; set; }
        public string txtFax { get; set; }
        public string txtEmail1 { get; set; }
        public string txtSite { get; set; }
        public string txtEmail2 { get; set; }
        public string txtEmail3 { get; set; }
        public string txtPassport { get; set; }
        public string txtIDDocument { get; set; }
        public DateTime? dteDate { get; set; }
        public byte[] imgFingerPrint { get; set; }
        public bool? bitActive { get; set; }
        public string txtNickName { get; set; }
        public string txtClientLogin { get; set; }
        public int? intNacionalidadeID { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblClients tblClients { get; set; }
        public virtual tblEmployees tblEmployees { get; set; }
        public virtual ICollection<tblApi_MensagemInadimplencia_Log> tblApi_MensagemInadimplencia_Log { get; set; }
        public virtual ICollection<tblAvaliacaoConteudoQuestao> tblAvaliacaoConteudoQuestao { get; set; }
        public virtual ICollection<tblBooksEntitiesProfessor> tblBooksEntitiesProfessor { get; set; }
        public virtual ICollection<tblClientClassifications> tblClientClassifications { get; set; }
        public virtual ICollection<tblClientsDocuments> tblClientsDocuments { get; set; }
        public virtual ICollection<tblCompanySectors> tblCompanySectorsintResponsible { get; set; }
        public virtual ICollection<tblCompanySectors> tblCompanySectorsintSubstitute { get; set; }
        public virtual ICollection<tblConcursoQuestoes_Aluno_Favoritas> tblConcursoQuestoes_Aluno_Favoritas { get; set; }
        public virtual ICollection<tblConcurso_Provas_ArquivosAluno> tblConcurso_Provas_ArquivosAluno { get; set; }
        public virtual ICollection<tblContribuicao> tblContribuicao { get; set; }
        public virtual ICollection<tblContribuicoes_Arquivadas> tblContribuicoes_Arquivadas { get; set; }
        public virtual ICollection<tblContribuicoes_Interacao> tblContribuicoes_Interacao { get; set; }
        public virtual ICollection<tblDeviceToken> tblDeviceToken { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Denuncia> tblDuvidasAcademicas_Denuncia { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Duvidas> tblDuvidasAcademicas_DuvidasintAcademico { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Duvidas> tblDuvidasAcademicas_DuvidasintClient { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Interacoes> tblDuvidasAcademicas_Interacoes { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Log> tblDuvidasAcademicas_Log { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Notificacao> tblDuvidasAcademicas_Notificacao { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Resposta> tblDuvidasAcademicas_RespostaintAcademico { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Resposta> tblDuvidasAcademicas_RespostaintClient { get; set; }
        public virtual ICollection<tblEmailNotificacaoDuvidasAcademicas> tblEmailNotificacaoDuvidasAcademicas { get; set; }
        public virtual ICollection<tblEnderecoEntregaCliente> tblEnderecoEntregaCliente { get; set; }
        public virtual ICollection<tblEspecialidadeProfessor> tblEspecialidadeProfessor { get; set; }
        public virtual ICollection<tblGrupo> tblGrupo { get; set; }
        public virtual ICollection<tblLabels> tblLabels { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluationintClient { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluationintEmployeed { get; set; }
        public virtual ICollection<tblLiberacaoApostilaAntecipada> tblLiberacaoApostilaAntecipada { get; set; }
        public virtual ICollection<tblMaterialApostilaAluno> tblMaterialApostilaAluno { get; set; }
        public virtual ICollection<tblMaterialApostilaAluno_Comentario> tblMaterialApostilaAluno_Comentario { get; set; }
        public virtual ICollection<tblMaterialApostilaComentario> tblMaterialApostilaComentario { get; set; }
        public virtual ICollection<tblMaterialApostilaInteracao> tblMaterialApostilaInteracao { get; set; }
        public virtual ICollection<tblMaterialApostilaProgresso> tblMaterialApostilaProgresso { get; set; }
        public virtual ICollection<tblMedsoftScreenshotReport> tblMedsoftScreenshotReport { get; set; }
        public virtual ICollection<tblNotificacaoDuvidas> tblNotificacaoDuvidas { get; set; }
        public virtual ICollection<tblPaymentDocuments> tblPaymentDocuments { get; set; }
        public virtual ICollection<tblPersonsPicture> tblPersonsPicture { get; set; }
        public virtual ICollection<tblPessoaGrupo> tblPessoaGrupo { get; set; }
        public virtual ICollection<tblQuestao_Duvida_Encaminhamento> tblQuestao_Duvida_EncaminhamentointDestinatario { get; set; }
        public virtual ICollection<tblQuestao_Duvida_Encaminhamento> tblQuestao_Duvida_EncaminhamentointRemetente { get; set; }
        public virtual ICollection<tblRodadaAluno> tblRodadaAluno { get; set; }
        public virtual ICollection<tblVideoVote> tblVideoVote { get; set; }
    }
}
