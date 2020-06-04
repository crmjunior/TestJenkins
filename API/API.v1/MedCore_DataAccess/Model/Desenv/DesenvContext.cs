using System.Linq;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using MedCore_DataAccess.Entidades;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.Util;
using System.Collections.Generic;
using MedCore_DataAccess.Model;
namespace MedCore_DataAccess.Model
{
    public partial class DesenvContext : DbContext
    {
        

        public DesenvContext()
        {
        }

        public DesenvContext(DbContextOptions<DesenvContext> options)
            : base(options)
        {
        }

        #region Dbset

        public virtual DbSet<tblTurmaExcecaoCpfBase> tblTurmaExcecaoCpfBase { get; set; }
        public virtual DbSet<ViewDireitoIntensivao_Chamados2018> ViewDireitoIntensivao_Chamados2018 { get; set; }
        public virtual DbSet<mview_ConcursoProvas_Recursos> mview_ConcursoProvas_Recursos { get; set; }
        public virtual DbSet<mview_CondicoesRegra> mview_CondicoesRegra { get; set; }
        public virtual DbSet<mview_CondicoesRegra_Group> mview_CondicoesRegra_Group { get; set; }
        public virtual DbSet<mview_Cronograma> mview_Cronograma { get; set; }
        public virtual DbSet<mview_ProdutosPorFilial> mview_ProdutosPorFilial { get; set; }
        public virtual DbSet<mview_ProductCombos> mview_ProductCombos { get; set; }
        public virtual DbSet<tblAPI_EntidadesValidas> tblAPI_EntidadesValidas { get; set; }
        public virtual DbSet<tblAPI_LiberacaoApostila> tblAPI_LiberacaoApostila { get; set; }
        public virtual DbSet<tblAPI_VisualizarAntecedencia> tblAPI_VisualizarAntecedencia { get; set; }
        public virtual DbSet<tblAccess> tblAccess { get; set; }
        public virtual DbSet<tblAccessLog> tblAccessLog { get; set; }
        public virtual DbSet<tblLessonsEvaluationProvaVideo> tblLessonsEvaluationProvaVideo { get; set; }
        public virtual DbSet<tblAccess_Action> tblAccess_Action { get; set; }
        public virtual DbSet<tblAccess_Application> tblAccess_Application { get; set; }
        public virtual DbSet<tblLogAvisos> tblLogAvisos { get; set; }
        public virtual DbSet<tblAccess_Button> tblAccess_Button { get; set; }
        public virtual DbSet<tblAccess_Combo> tblAccess_Combo { get; set; }
        public virtual DbSet<tblAccess_Condition> tblAccess_Condition { get; set; }
        public virtual DbSet<tblAccess_DataLimite> tblAccess_DataLimite { get; set; }
        public virtual DbSet<tblAccess_Detail> tblAccess_Detail { get; set; }
        public virtual DbSet<tblBloqueioQuestoes> tblBloqueioQuestoes { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_Autorizacao> tblConcursoQuestao_Classificacao_Autorizacao { get; set; }
        public virtual DbSet<tblSysRoles> tblSysRoles { get; set; }
        public virtual DbSet<tblAccess_Device> tblAccess_Device { get; set; }
        public virtual DbSet<tblAccess_Empresa_Application> tblAccess_Empresa_Application { get; set; }
        public virtual DbSet<tblAccess_Empresas> tblAccess_Empresas { get; set; }
        public virtual DbSet<tblAccess_Key> tblAccess_Key { get; set; }
        public virtual DbSet<tblAccess_Menu> tblAccess_Menu { get; set; }
        public virtual DbSet<tblAccess_MenuProduto> tblAccess_MenuProduto { get; set; }
        public virtual DbSet<tblAccess_Menu_Apagar> tblAccess_Menu_Apagar { get; set; }
        public virtual DbSet<tblNotificacaoEvento> tblNotificacaoEvento { get; set; }
        public virtual DbSet<tblAccess_Menu_ProductGroup> tblAccess_Menu_ProductGroup { get; set; }
        public virtual DbSet<tblAccess_Menu_Url> tblAccess_Menu_Url { get; set; }
        public virtual DbSet<tblAccess_Message> tblAccess_Message { get; set; }
        public virtual DbSet<tblAccess_Object> tblAccess_Object { get; set; }
        public virtual DbSet<tblLessonTitles> tblLessonTitles { get; set; }
        public virtual DbSet<tblLessonsEvaluationRevisaoAula> tblLessonsEvaluationRevisaoAula { get; set; }
        public virtual DbSet<tblProvaVideo> tblProvaVideo { get; set; }
        public virtual DbSet<tblProvaVideoIndice> tblProvaVideoIndice { get; set; }
        public virtual DbSet<tblAccess_ObjectType> tblAccess_ObjectType { get; set; }
        public virtual DbSet<tblAccess_Object_Application> tblAccess_Object_Application { get; set; }
        public virtual DbSet<tblAccess_PermissionNotification> tblAccess_PermissionNotification { get; set; }
        public virtual DbSet<tblAccess_PermissionObject> tblAccess_PermissionObject { get; set; }
        public virtual DbSet<tblAccess_Permission_Rule> tblAccess_Permission_Rule { get; set; }
        public virtual DbSet<tblAccess_Permission_Status> tblAccess_Permission_Status { get; set; }
        public virtual DbSet<tblAccess_PersonalException> tblAccess_PersonalException { get; set; }
        public virtual DbSet<tblAccess_ProdutoEmpresas> tblAccess_ProdutoEmpresas { get; set; }
        public virtual DbSet<tblAccess_Rule> tblAccess_Rule { get; set; }
        public virtual DbSet<tblAccess_RuleDetail> tblAccess_RuleDetail { get; set; }
        public virtual DbSet<tblAccess_Rule_Condition> tblAccess_Rule_Condition { get; set; }
        public virtual DbSet<tblAccess_Rule_Detail> tblAccess_Rule_Detail { get; set; }
        public virtual DbSet<tblAccess_Rule_Menu> tblAccess_Rule_Menu { get; set; }
        public virtual DbSet<tblAccess_Screen> tblAccess_Screen { get; set; }
        public virtual DbSet<tblAccess_Screen_Action> tblAccess_Screen_Action { get; set; }
        public virtual DbSet<tblAccess_YearType> tblAccess_YearType { get; set; }
        public virtual DbSet<tblAccess_YearTypeYear> tblAccess_YearTypeYear { get; set; }
        public virtual DbSet<tblAccess_Year_Type> tblAccess_Year_Type { get; set; }
        public virtual DbSet<tblAccountData> tblAccountData { get; set; }
        public virtual DbSet<tblAcess_Object_Validity> tblAcess_Object_Validity { get; set; }
        public virtual DbSet<tblAdaptaMedAulaIndice> tblAdaptaMedAulaIndice { get; set; }
        public virtual DbSet<tblAdaptaMedAulaTemaProfessorAssistido> tblAdaptaMedAulaTemaProfessorAssistido { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideo> tblAdaptaMedAulaVideo { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideoAprovacao> tblAdaptaMedAulaVideoAprovacao { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideoAprovacaoLog> tblAdaptaMedAulaVideoAprovacaoLog { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideoCorrigido> tblAdaptaMedAulaVideoCorrigido { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideoLogPosition> tblAdaptaMedAulaVideoLogPosition { get; set; }
        public virtual DbSet<tblAdaptaMedAulaVideoRelatorioReprovacaoLog> tblAdaptaMedAulaVideoRelatorioReprovacaoLog { get; set; }
        public virtual DbSet<tblAddressTypes> tblAddressTypes { get; set; }
        public virtual DbSet<tblAlunosAnoAtualMaisAnterior> tblAlunosAnoAtualMaisAnterior { get; set; }
        public virtual DbSet<tblAnexo> tblAnexo { get; set; }
        public virtual DbSet<tblAnoInscricao> tblAnoInscricao { get; set; }
        public virtual DbSet<tblApi_MensagemInadimplencia_Log> tblApi_MensagemInadimplencia_Log { get; set; }
        public virtual DbSet<tblApostilaAddOn> tblApostilaAddOn { get; set; }
        public virtual DbSet<tblApostilasUnificadas> tblApostilasUnificadas { get; set; }
        public virtual DbSet<tblApplication_AcessDenied> tblApplication_AcessDenied { get; set; }
        public virtual DbSet<tblAtualizacaoErrata_Imagens> tblAtualizacaoErrata_Imagens { get; set; }
        public virtual DbSet<tblAvaliacaoAluno> tblAvaliacaoAluno { get; set; }
        public virtual DbSet<tblAvaliacaoConteudoQuestao> tblAvaliacaoConteudoQuestao { get; set; }
        public virtual DbSet<tblAvaliacaoConteudoQuestaoAlternativas> tblAvaliacaoConteudoQuestaoAlternativas { get; set; }
        public virtual DbSet<tblAvatar> tblAvatar { get; set; }
        public virtual DbSet<tblAvatar_Category> tblAvatar_Category { get; set; }
        public virtual DbSet<tblAvatar_Types> tblAvatar_Types { get; set; }
        public virtual DbSet<tblAvisos> tblAvisos { get; set; }
        public virtual DbSet<tblAvisos_Chamados> tblAvisos_Chamados { get; set; }
        public virtual DbSet<tblBankAccounts> tblBankAccounts { get; set; }
        public virtual DbSet<tblBanks> tblBanks { get; set; }
        public virtual DbSet<tblBanners> tblBanners { get; set; }
        public virtual DbSet<tblBdNuvemLog> tblBdNuvemLog { get; set; }
        public virtual DbSet<tblBlackList_Anexo> tblBlackList_Anexo { get; set; }
        public virtual DbSet<tblBlackList_Categoria> tblBlackList_Categoria { get; set; }
        public virtual DbSet<tblBlackWords_DuvidasAcademicas> tblBlackWords_DuvidasAcademicas { get; set; }
        public virtual DbSet<tblBlacklistAprovacoes_Bloqueios> tblBlacklistAprovacoes_Bloqueios { get; set; }
        public virtual DbSet<tblBlacklist_Log> tblBlacklist_Log { get; set; }
        public virtual DbSet<tblProvaVideoLogPosition> tblProvaVideoLogPosition { get; set; }
        public virtual DbSet<tblBlacklist_Usuarios> tblBlacklist_Usuarios { get; set; }
        public virtual DbSet<tblBloqueioArea> tblBloqueioArea { get; set; }
        public virtual DbSet<tblBloqueioConcurso> tblBloqueioConcurso { get; set; }
        public virtual DbSet<tblContadorQuestoes_MontaProva> tblContadorQuestoes_MontaProva { get; set; }
        public virtual DbSet<tblFiltroAluno_MontaProva> tblFiltroAluno_MontaProva { get; set; }

        public virtual DbSet<tblBook_Imagens> tblBook_Imagens { get; set; }
        public virtual DbSet<tblBooks> tblBooks { get; set; }
        public virtual DbSet<tblBooksEntitiesProfessor> tblBooksEntitiesProfessor { get; set; }
        public virtual DbSet<tblBooks_Entities> tblBooks_Entities { get; set; }
        public virtual DbSet<tblBooks_Videos> tblBooks_Videos { get; set; }
        public virtual DbSet<tblCallCenterCalls> tblCallCenterCalls { get; set; }
        public virtual DbSet<tblCallCenterCallsInadimplencia> tblCallCenterCallsInadimplencia { get; set; }
        public virtual DbSet<tblCallCenterCallsInadimplenciaLog> tblCallCenterCallsInadimplenciaLog { get; set; }
        public virtual DbSet<tblCallCenterCallsOv> tblCallCenterCallsOv { get; set; }
        public virtual DbSet<tblCallCenterCategory> tblCallCenterCategory { get; set; }
        public virtual DbSet<tblCallCenterCategory_Classification> tblCallCenterCategory_Classification { get; set; }
        public virtual DbSet<tblCallCenterEvents> tblCallCenterEvents { get; set; }
        public virtual DbSet<tblCargaHorariaCurso> tblCargaHorariaCurso { get; set; }
        public virtual DbSet<tblCities> tblCities { get; set; }
        public virtual DbSet<tblClassRooms> tblClassRooms { get; set; }
        public virtual DbSet<tblClassificacaoTurmaConvidada> tblClassificacaoTurmaConvidada { get; set; }
        public virtual DbSet<tblClassificationAttributes> tblClassificationAttributes { get; set; }
        public virtual DbSet<tblClassifications> tblClassifications { get; set; }
        public virtual DbSet<tblCleanHtmlTags> tblCleanHtmlTags { get; set; }
        public virtual DbSet<tblClientClassifications> tblClientClassifications { get; set; }
        public virtual DbSet<tblClients> tblClients { get; set; }
        public virtual DbSet<tblClientsDocuments> tblClientsDocuments { get; set; }
        public virtual DbSet<tblClientsDocumentsStatus> tblClientsDocumentsStatus { get; set; }
        public virtual DbSet<tblClientsDocumentsTypes> tblClientsDocumentsTypes { get; set; }
        public virtual DbSet<tblClients_BlackList> tblClients_BlackList { get; set; }
        public virtual DbSet<tblClients_BlackListAprovacoes> tblClients_BlackListAprovacoes { get; set; }
        public virtual DbSet<tblClients_BlackListMotivos> tblClients_BlackListMotivos { get; set; }
        public virtual DbSet<tblClients_BlackListPre> tblClients_BlackListPre { get; set; }
        public virtual DbSet<tblCodigoCaracteres> tblCodigoCaracteres { get; set; }
        public virtual DbSet<tblCodigoComentario> tblCodigoComentario { get; set; }
        public virtual DbSet<tblComentario_Rascunho> tblComentario_Rascunho { get; set; }
        public virtual DbSet<tblCompanies> tblCompanies { get; set; }
        public virtual DbSet<tblCompanySectors> tblCompanySectors { get; set; }
        public virtual DbSet<tblConcursoCatologoDeClassificacoes> tblConcursoCatologoDeClassificacoes { get; set; }
        public virtual DbSet<tblConcursoPremium> tblConcursoPremium { get; set; }
        public virtual DbSet<tblConcursoPremium_DataLimite> tblConcursoPremium_DataLimite { get; set; }
        public virtual DbSet<tblConcursoQuestaoEmClassificacao> tblConcursoQuestaoEmClassificacao { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao> tblConcursoQuestao_Classificacao { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada> tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada_log> tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada_log { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_Autorizacao_log> tblConcursoQuestao_Classificacao_Autorizacao_log { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila> tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila { get; set; }
        public virtual DbSet<tblConcursoQuestao_Classificacao_log> tblConcursoQuestao_Classificacao_log { get; set; }
        public virtual DbSet<tblConcursoQuestoes> tblConcursoQuestoes { get; set; }
        public virtual DbSet<tblConcursoQuestoesGravacaoProtocolo_Catalogo> tblConcursoQuestoesGravacaoProtocolo_Catalogo { get; set; }
        public virtual DbSet<tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT> tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT { get; set; }
        public virtual DbSet<tblConcursoQuestoesGravacaoProtocolo_Codigos> tblConcursoQuestoesGravacaoProtocolo_Codigos { get; set; }
        public virtual DbSet<tblConcursoQuestoesGravacaoProtocolo_Questoes> tblConcursoQuestoesGravacaoProtocolo_Questoes { get; set; }
        public virtual DbSet<tblConcursoQuestoes_Alternativas> tblConcursoQuestoes_Alternativas { get; set; }
        public virtual DbSet<tblConcursoQuestoes_Aluno_Favoritas> tblConcursoQuestoes_Aluno_Favoritas { get; set; }
        public virtual DbSet<tblConcursoQuestoes_recursosComentario_Imagens> tblConcursoQuestoes_recursosComentario_Imagens { get; set; }
        public virtual DbSet<tblConcursoRecursoFavoritado> tblConcursoRecursoFavoritado { get; set; }

        public virtual DbSet<tblConcurso_ProvasLivesRecurso> tblConcurso_ProvasLivesRecurso { get; set; }

        public virtual DbSet<tblProvaAlunoConfiguracoes> tblProvaAlunoConfiguracoes { get; set; }

        public virtual DbSet<tblConcurso_Provas> tblConcurso_Provas { get; set; }
        public virtual DbSet<tblConcurso_Provas_Acertos> tblConcurso_Provas_Acertos { get; set; }
        public virtual DbSet<tblConcurso_Provas_Acertos_log> tblConcurso_Provas_Acertos_log { get; set; }
        public virtual DbSet<tblConcurso_Provas_ArquivosAluno> tblConcurso_Provas_ArquivosAluno { get; set; }
        public virtual DbSet<tblConcurso_Provas_Forum> tblConcurso_Provas_Forum { get; set; }
        public virtual DbSet<tblConcurso_Provas_Forum_Moderadas> tblConcurso_Provas_Forum_Moderadas { get; set; }
        public virtual DbSet<tblConcurso_Provas_Forum_log> tblConcurso_Provas_Forum_log { get; set; }
        public virtual DbSet<tblConcurso_Provas_Tipos> tblConcurso_Provas_Tipos { get; set; }
        public virtual DbSet<tblConcurso_Recurso_AccessDenied> tblConcurso_Recurso_AccessDenied { get; set; }
        public virtual DbSet<tblConcurso_Recurso_AccessDenied_LOG> tblConcurso_Recurso_AccessDenied_LOG { get; set; }
        public virtual DbSet<tblConcurso_Recurso_Aluno> tblConcurso_Recurso_Aluno { get; set; }
        public virtual DbSet<tblConcurso_Recurso_Funcionarios> tblConcurso_Recurso_Funcionarios { get; set; }
        public virtual DbSet<tblConcurso_Recurso_Status> tblConcurso_Recurso_Status { get; set; }
        public virtual DbSet<tblConcursos> tblConcursos { get; set; }
        public virtual DbSet<tblConcursosProvas> tblConcursosProvas { get; set; }
        public virtual DbSet<tblConcursosVagas> tblConcursosVagas { get; set; }
        public virtual DbSet<tblConteudo> tblConteudo { get; set; }
        public virtual DbSet<tblConteudoCategoria> tblConteudoCategoria { get; set; }
        public virtual DbSet<tblConteudoCategoriaLog> tblConteudoCategoriaLog { get; set; }
        public virtual DbSet<tblConteudoLabel> tblConteudoLabel { get; set; }
        public virtual DbSet<tblConteudoLabel_Item> tblConteudoLabel_Item { get; set; }
        public virtual DbSet<tblConteudoLog> tblConteudoLog { get; set; }
        public virtual DbSet<tblConteudoMaterialMed> tblConteudoMaterialMed { get; set; }
        public virtual DbSet<tblConteudoMaterialMedTipo> tblConteudoMaterialMedTipo { get; set; }
        public virtual DbSet<tblConteudoTipo> tblConteudoTipo { get; set; }
        public virtual DbSet<tblContrato> tblContrato { get; set; }
        public virtual DbSet<tblContratoAceite> tblContratoAceite { get; set; }
        public virtual DbSet<tblContratoAlunoAceite> tblContratoAlunoAceite { get; set; }
        public virtual DbSet<tblContratoImagem> tblContratoImagem { get; set; }
        public virtual DbSet<tblContratoIntensivo> tblContratoIntensivo { get; set; }
        public virtual DbSet<tblContribuicao> tblContribuicao { get; set; }
        public virtual DbSet<tblContribuicaoArquivo> tblContribuicaoArquivo { get; set; }
        public virtual DbSet<tblContribuicao_Encaminhadas> tblContribuicao_Encaminhadas { get; set; }
        public virtual DbSet<tblContribuicoes_Arquivadas> tblContribuicoes_Arquivadas { get; set; }
        public virtual DbSet<tblContribuicoes_Interacao> tblContribuicoes_Interacao { get; set; }
        public virtual DbSet<tblCountries> tblCountries { get; set; }
        public virtual DbSet<tblCourses> tblCourses { get; set; }
        public virtual DbSet<tblCriterioOrdenacao_BuscaTexto> tblCriterioOrdenacao_BuscaTexto { get; set; }
        public virtual DbSet<tblCronogramaConteudoRevalida> tblCronogramaConteudoRevalida { get; set; }
        public virtual DbSet<tblCronogramaPrateleira> tblCronogramaPrateleira { get; set; }
        public virtual DbSet<tblCronogramaPrateleira_LessonTitles> tblCronogramaPrateleira_LessonTitles { get; set; }
        public virtual DbSet<tblCtrlPanel_AccessControl_Persons_X_Roles> tblCtrlPanel_AccessControl_Persons_X_Roles { get; set; }
        public virtual DbSet<tblCtrlPanel_AccessControl_Roles> tblCtrlPanel_AccessControl_Roles { get; set; }
        public virtual DbSet<tblCtrlPanel_Link> tblCtrlPanel_Link { get; set; }
        public virtual DbSet<tblConcurso_ProvaCasoClinico> tblConcurso_ProvaCasoClinico { get; set; }
        public virtual DbSet<tblConcurso_Recurso_MEDGRUPO> tblConcurso_Recurso_MEDGRUPO { get; set; }

        public virtual DbSet<tblCtrlPanel_Link_X_Employees> tblCtrlPanel_Link_X_Employees { get; set; }
        public virtual DbSet<tblCtrlPanel_Relacao> tblCtrlPanel_Relacao { get; set; }
        public virtual DbSet<tblDeviceMovel> tblDeviceMovel { get; set; }
        public virtual DbSet<tblDeviceToken> tblDeviceToken { get; set; }
        public virtual DbSet<tblDocumento> tblDocumento { get; set; }
        public virtual DbSet<tblDownloadApostilaQuestao_Log> tblDownloadApostilaQuestao_Log { get; set; }
        public virtual DbSet<tblDownloadApostila_Log> tblDownloadApostila_Log { get; set; }
        public virtual DbSet<tblDuvida> tblDuvida { get; set; }
        public virtual DbSet<tblDuvidaImagem> tblDuvidaImagem { get; set; }
        public virtual DbSet<tblDuvidaRespostaProfessor> tblDuvidaRespostaProfessor { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Denuncia> tblDuvidasAcademicas_Denuncia { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_DuvidaApostila> tblDuvidasAcademicas_DuvidaApostila { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_DuvidaQuestao> tblDuvidasAcademicas_DuvidaQuestao { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Duvidas> tblDuvidasAcademicas_Duvidas { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_DuvidasEncaminhadas> tblDuvidasAcademicas_DuvidasEncaminhadas { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_DuvidasHistorico> tblDuvidasAcademicas_DuvidasHistorico { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Interacoes> tblDuvidasAcademicas_Interacoes { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Lidas> tblDuvidasAcademicas_Lidas { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Log> tblDuvidasAcademicas_Log { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Notificacao> tblDuvidasAcademicas_Notificacao { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_Resposta> tblDuvidasAcademicas_Resposta { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_RespostaHistorico> tblDuvidasAcademicas_RespostaHistorico { get; set; }
        public virtual DbSet<tblDuvidasAcademicas_TipoInteracao> tblDuvidasAcademicas_TipoInteracao { get; set; }

        public virtual DbSet<tblDuvidasAcademicas_DuvidasArquivadas> tblDuvidasAcademicas_DuvidasArquivadas { get; set; }
        public virtual DbSet<tblEditoras> tblEditoras { get; set; }
        public virtual DbSet<tblEditoras_GrupoProduto> tblEditoras_GrupoProduto { get; set; }
        public virtual DbSet<tblEmailConteudo> tblEmailConteudo { get; set; }
        public virtual DbSet<tblEmailNotificacaoDuvidasAcademicas> tblEmailNotificacaoDuvidasAcademicas { get; set; }
        public virtual DbSet<tblEmed_AccessDenied> tblEmed_AccessDenied { get; set; }
        public virtual DbSet<msp_API_ProgressoAulaRevisaoAluno_Result> msp_API_ProgressoAulaRevisaoAluno_Result { get; set; }
        
        public virtual DbSet<tblEmed_AccessDenied_LOG> tblEmed_AccessDenied_LOG { get; set; }
        public virtual DbSet<tblEmed_AccessGolden> tblEmed_AccessGolden { get; set; }
        public virtual DbSet<tblEmed_AccessGolden_log> tblEmed_AccessGolden_log { get; set; }
        public virtual DbSet<tblEmed_AcessoConcorrente_Log> tblEmed_AcessoConcorrente_Log { get; set; }
        public virtual DbSet<tblEmed_Duvidas> tblEmed_Duvidas { get; set; }
        public virtual DbSet<tblEmed_SessoesAtivas> tblEmed_SessoesAtivas { get; set; }
        public virtual DbSet<tblEmployeeCargos> tblEmployeeCargos { get; set; }
        public virtual DbSet<tblEmployee_Sector> tblEmployee_Sector { get; set; }
        public virtual DbSet<tblEmployees> tblEmployees { get; set; }
        public virtual DbSet<tblEnderecoEntregaCliente> tblEnderecoEntregaCliente { get; set; }
        public virtual DbSet<tblEspecialidadeProfessor> tblEspecialidadeProfessor { get; set; }
        public virtual DbSet<tblEspecialidades> tblEspecialidades { get; set; }
        public virtual DbSet<tblExercicio_MontaProva> tblExercicio_MontaProva { get; set; }
        public virtual DbSet<tblExpectedGraduationTermCatalog> tblExpectedGraduationTermCatalog { get; set; }
        public virtual DbSet<tblFTPConfig> tblFTPConfig { get; set; }
        public virtual DbSet<tblFormulas_Medme> tblFormulas_Medme { get; set; }
        public virtual DbSet<tblFuncionalidade> tblFuncionalidade { get; set; }
        public virtual DbSet<tblFuncionalidade1> tblFuncionalidade1 { get; set; }
        public virtual DbSet<tblGaleriaImagem> tblGaleriaImagem { get; set; }
        public virtual DbSet<tblGaleriaImagemApostila> tblGaleriaImagemApostila { get; set; }
        public virtual DbSet<tblGaleriaRelacaoImagem> tblGaleriaRelacaoImagem { get; set; }
        public virtual DbSet<tblGrupo> tblGrupo { get; set; }
        public virtual DbSet<tblImagemGaleria> tblImagemGaleria { get; set; }
        public virtual DbSet<tblImagemSemana> tblImagemSemana { get; set; }
        public virtual DbSet<tblImagemSemanaRespostaAluno> tblImagemSemanaRespostaAluno { get; set; }
        public virtual DbSet<tblImpostoDeRenda> tblImpostoDeRenda { get; set; }
        public virtual DbSet<tblImpostoDeRendaAliquota> tblImpostoDeRendaAliquota { get; set; }
        public virtual DbSet<tblInscricao_EadCadastro> tblInscricao_EadCadastro { get; set; }
        public virtual DbSet<tblInscricao_MedCadastro> tblInscricao_MedCadastro { get; set; }
        public virtual DbSet<tblInscricoesBloqueios> tblInscricoesBloqueios { get; set; }
        public virtual DbSet<tblInscricoesBloqueiosTipos> tblInscricoesBloqueiosTipos { get; set; }
        public virtual DbSet<tblInscricoesBloqueios_Log> tblInscricoesBloqueios_Log { get; set; }
        public virtual DbSet<tblInscricoesCampanhaMkt> tblInscricoesCampanhaMkt { get; set; }
        public virtual DbSet<tblInscricoesCampanhaMktTipo> tblInscricoesCampanhaMktTipo { get; set; }
        public virtual DbSet<tblInscricoesRessalvas> tblInscricoesRessalvas { get; set; }
        public virtual DbSet<tblInscricoes_Log> tblInscricoes_Log { get; set; }
        public virtual DbSet<tblInstrucaoPostagemCheque> tblInstrucaoPostagemCheque { get; set; }
        public virtual DbSet<tblIntensivaoLog> tblIntensivaoLog { get; set; }
        public virtual DbSet<tblIntensivao_Cronograma> tblIntensivao_Cronograma { get; set; }
        public virtual DbSet<tblLabelDetails> tblLabelDetails { get; set; }
        public virtual DbSet<tblLabelGroups> tblLabelGroups { get; set; }
        public virtual DbSet<tblLabel_SmartInfo> tblLabel_SmartInfo { get; set; }
        public virtual DbSet<tblLabels> tblLabels { get; set; }
        public virtual DbSet<tblLessonEvaluationVideoAula> tblLessonEvaluationVideoAula { get; set; }
        public virtual DbSet<tblLessonRessalva> tblLessonRessalva { get; set; }
        public virtual DbSet<tblLessonTeacherSubstituto> tblLessonTeacherSubstituto { get; set; }
        //public virtual DbSet<tblLessonTeachersByClassRoom> tblLessonTeachersByClassRoom { get; set; }
        public virtual DbSet<tblLessonTeachersByGroupAndTitle> tblLessonTeachersByGroupAndTitle { get; set; }
        public virtual DbSet<tblLessonTitleRevalida> tblLessonTitleRevalida { get; set; }
        public virtual DbSet<tblLessonTypes> tblLessonTypes { get; set; }
        public virtual DbSet<tblLesson_Material> tblLesson_Material { get; set; }
        public virtual DbSet<tblLessons> tblLessons { get; set; }
        public virtual DbSet<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
        public virtual DbSet<tblLessonsTotalEvaluationAuxiliar> tblLessonsTotalEvaluationAuxiliar { get; set; }
        public virtual DbSet<tblLessonxRessalva> tblLessonxRessalva { get; set; }

        public virtual DbSet<tblConcurso> tblConcurso { get; set; }
        public virtual DbSet<tblLiberacaoApostila> tblLiberacaoApostila { get; set; }
        public virtual DbSet<tblLiberacaoApostila1> tblLiberacaoApostila1 { get; set; }
        public virtual DbSet<tblLiberacaoApostilaAntecipada> tblLiberacaoApostilaAntecipada { get; set; }
        public virtual DbSet<tblLiberacaoApostila_Historico> tblLiberacaoApostila_Historico { get; set; }
        public virtual DbSet<tblLinkEsqueciSenha> tblLinkEsqueciSenha { get; set; }
        public virtual DbSet<tblLocaisRetiradaMaterial> tblLocaisRetiradaMaterial { get; set; }
        public virtual DbSet<tblLogAcessoLogin> tblLogAcessoLogin { get; set; }
        public virtual DbSet<tblLogAcoesSimuladoImpresso> tblLogAcoesSimuladoImpresso { get; set; }
        public virtual DbSet<tblLogClientInscricaoInadimplente> tblLogClientInscricaoInadimplente { get; set; }
        public virtual DbSet<tblLogConcursoQuestaoComentario> tblLogConcursoQuestaoComentario { get; set; }
        public virtual DbSet<tblLogGaleriaImagem> tblLogGaleriaImagem { get; set; }
        public virtual DbSet<tblLogMesesBlocoMaterialAnteriorAvulso> tblLogMesesBlocoMaterialAnteriorAvulso { get; set; }
        public virtual DbSet<tblLogOperacoesConcurso> tblLogOperacoesConcurso { get; set; }
        public virtual DbSet<tblLogOrdemVenda> tblLogOrdemVenda { get; set; }
        public virtual DbSet<tblLogRecursoAluno> tblLogRecursoAluno { get; set; }
        public virtual DbSet<tblLog_PrintApostila> tblLog_PrintApostila { get; set; }
        public virtual DbSet<tblLog_PrintApostilaMedsoftPro> tblLog_PrintApostilaMedsoftPro { get; set; }
        public virtual DbSet<tblMaterialApostila> tblMaterialApostila { get; set; }
        public virtual DbSet<tblMaterialApostilaAluno> tblMaterialApostilaAluno { get; set; }
        public virtual DbSet<tblMaterialApostilaAluno_Comentario> tblMaterialApostilaAluno_Comentario { get; set; }
        public virtual DbSet<tblMaterialApostilaAssets> tblMaterialApostilaAssets { get; set; }
        public virtual DbSet<tblMaterialApostilaComentario> tblMaterialApostilaComentario { get; set; }
        public virtual DbSet<tblMaterialApostilaConfig> tblMaterialApostilaConfig { get; set; }
        public virtual DbSet<tblMaterialApostilaInteracao> tblMaterialApostilaInteracao { get; set; }
        public virtual DbSet<tblMaterialApostilaProgresso> tblMaterialApostilaProgresso { get; set; }
        public virtual DbSet<tblMaterialOrdersGalpao> tblMaterialOrdersGalpao { get; set; }
        public virtual DbSet<tblMaterialOrdersGalpaoRomaneio> tblMaterialOrdersGalpaoRomaneio { get; set; }
        public virtual DbSet<tblMaterials> tblMaterials { get; set; }
        public virtual DbSet<tblMedCode_MarcasCompartilhadas> tblMedCode_MarcasCompartilhadas { get; set; }
        public virtual DbSet<tblMedNotas_Cliente> tblMedNotas_Cliente { get; set; }
        public virtual DbSet<tblMedNotas_Empresa> tblMedNotas_Empresa { get; set; }
        public virtual DbSet<tblMedNotas_Fornecedor> tblMedNotas_Fornecedor { get; set; }
        public virtual DbSet<tblMedNotas_Guia> tblMedNotas_Guia { get; set; }
        public virtual DbSet<tblMedNotas_LogGuia> tblMedNotas_LogGuia { get; set; }
        public virtual DbSet<tblMedNotas_LogNota> tblMedNotas_LogNota { get; set; }
        public virtual DbSet<tblMedNotas_Nota> tblMedNotas_Nota { get; set; }
        public virtual DbSet<tblMedNotas_PermissaoXSetor> tblMedNotas_PermissaoXSetor { get; set; }
        public virtual DbSet<tblMedNotas_Status> tblMedNotas_Status { get; set; }
        public virtual DbSet<tblMedNotas_TipoImposto> tblMedNotas_TipoImposto { get; set; }
        public virtual DbSet<tblMedNotas_TipoPermissao> tblMedNotas_TipoPermissao { get; set; }
        public virtual DbSet<tblMedNotas_Usuario> tblMedNotas_Usuario { get; set; }
        public virtual DbSet<tblMedSoft_AcessoEspecial> tblMedSoft_AcessoEspecial { get; set; }
        public virtual DbSet<tblMedSoft_VersaoAppPermissao> tblMedSoft_VersaoAppPermissao { get; set; }
        public virtual DbSet<tblMedcineVideos> tblMedcineVideos { get; set; }
        public virtual DbSet<tblMedcode_DataMatrix> tblMedcode_DataMatrix { get; set; }
        public virtual DbSet<tblMedcode_DataMatrix_Anexo> tblMedcode_DataMatrix_Anexo { get; set; }
        public virtual DbSet<tblMedcode_DataMatrix_Log> tblMedcode_DataMatrix_Log { get; set; }
        public virtual DbSet<tblMedcode_DataMatrix_Tipo> tblMedcode_DataMatrix_Tipo { get; set; }
        public virtual DbSet<tblMedmeAreas> tblMedmeAreas { get; set; }
        public virtual DbSet<tblMedmeVideoLogPosition> tblMedmeVideoLogPosition { get; set; }
        public virtual DbSet<tblMednet_AlunoClipping> tblMednet_AlunoClipping { get; set; }
        public virtual DbSet<tblMednet_AlunoComentario> tblMednet_AlunoComentario { get; set; }
        public virtual DbSet<tblMedsoftClipboardReport> tblMedsoftClipboardReport { get; set; }
        public virtual DbSet<tblMedsoftScreenshotReport> tblMedsoftScreenshotReport { get; set; }
        public virtual DbSet<tblMedsoft_AlunosOnline> tblMedsoft_AlunosOnline { get; set; }
        public virtual DbSet<tblMedsoft_Atualizacao> tblMedsoft_Atualizacao { get; set; }
        //public virtual DbSet<tblMedsoft_Atualizacao_Aluno> tblMedsoft_Atualizacao_Aluno { get; set; }
        public virtual DbSet<tblMedsoft_CacheConfig> tblMedsoft_CacheConfig { get; set; }
        public virtual DbSet<tblMedsoft_Especialidade_Classificacao> tblMedsoft_Especialidade_Classificacao { get; set; }
        public virtual DbSet<tblMedsoft_Especialidade_Old_to_New> tblMedsoft_Especialidade_Old_to_New { get; set; }
        public virtual DbSet<tblMedsoft_PermissaoLogin> tblMedsoft_PermissaoLogin { get; set; }
        public virtual DbSet<tblMedsoft_Questao_Especialidade> tblMedsoft_Questao_Especialidade { get; set; }
        public virtual DbSet<tblMedsoft_VideoMioloAssistido> tblMedsoft_VideoMioloAssistido { get; set; }
        public virtual DbSet<tblMeiodeAnoAlunosAnosAnteriores> tblMeiodeAnoAlunosAnosAnteriores { get; set; }
        public virtual DbSet<tblMensagens> tblMensagens { get; set; }
        public virtual DbSet<tblMensagensLogin> tblMensagensLogin { get; set; }
        public virtual DbSet<tblMenuItens> tblMenuItens { get; set; }
        public virtual DbSet<tblMenu_PerfilRegra> tblMenu_PerfilRegra { get; set; }
        public virtual DbSet<tblNacionalidade> tblNacionalidade { get; set; }
        public virtual DbSet<tblNotificacao> tblNotificacao { get; set; }
        public virtual DbSet<tblNotificacaoAluno> tblNotificacaoAluno { get; set; }
        public virtual DbSet<tblNotificacaoDeviceToken> tblNotificacaoDeviceToken { get; set; }
        public virtual DbSet<tblNotificacaoDuvidas> tblNotificacaoDuvidas { get; set; }
        public virtual DbSet<tblNotificacaoTipo> tblNotificacaoTipo { get; set; }
        public virtual DbSet<tblOfflineConfig> tblOfflineConfig { get; set; }
        public virtual DbSet<tblPaymentDocuments> tblPaymentDocuments { get; set; }
        public virtual DbSet<tblPaymentTemplateConditionType> tblPaymentTemplateConditionType { get; set; }
        public virtual DbSet<tblPaymentTemplateData> tblPaymentTemplateData { get; set; }
        public virtual DbSet<tblPaymentTemplatesCheques> tblPaymentTemplatesCheques { get; set; }
        public virtual DbSet<tblPaymentTypes> tblPaymentTypes { get; set; }
        public virtual DbSet<tblPerfil> tblPerfil { get; set; }
        public virtual DbSet<tblPerfil1> tblPerfil1 { get; set; }
        public virtual DbSet<tblPerfil_Area> tblPerfil_Area { get; set; }
        public virtual DbSet<tblPerfil_Employees> tblPerfil_Employees { get; set; }
        public virtual DbSet<tblPerfil_Employees1> tblPerfil_Employees1 { get; set; }
        public virtual DbSet<tblPerfil_Funcionalidade> tblPerfil_Funcionalidade { get; set; }
        public virtual DbSet<tblPerfil_Funcionalidade1> tblPerfil_Funcionalidade1 { get; set; }
        public virtual DbSet<tblPerfil_Regra> tblPerfil_Regra { get; set; }
        public virtual DbSet<tblPerfil_RegraEntidade> tblPerfil_RegraEntidade { get; set; }
        public virtual DbSet<tblPermissaoInadimplenciaConfiguracao> tblPermissaoInadimplenciaConfiguracao { get; set; }
        public virtual DbSet<tblPermissaoInadimplenciaLogAlerta> tblPermissaoInadimplenciaLogAlerta { get; set; }
        public virtual DbSet<tblPermissaoInadimplenciaLogConfirmacaoAlerta> tblPermissaoInadimplenciaLogConfirmacaoAlerta { get; set; }
        public virtual DbSet<tblPermissaoListaMateriais> tblPermissaoListaMateriais { get; set; }
        public virtual DbSet<tblPersons> tblPersons { get; set; }
        public virtual DbSet<tblPersonsAvatar> tblPersonsAvatar { get; set; }
        public virtual DbSet<tblPersonsPicture> tblPersonsPicture { get; set; }
        public virtual DbSet<tblPersons_Passwords> tblPersons_Passwords { get; set; }
        public virtual DbSet<tblPessoaGrupo> tblPessoaGrupo { get; set; }
        public virtual DbSet<tblPorcentagemDesconto> tblPorcentagemDesconto { get; set; }
        public virtual DbSet<tblPrestacaoContasGestorXsubordinados> tblPrestacaoContasGestorXsubordinados { get; set; }
        public virtual DbSet<tblProcedimentoEntrada> tblProcedimentoEntrada { get; set; }
        public virtual DbSet<tblProductCombos_Products> tblProductCombos_Products { get; set; }
        public virtual DbSet<tblProductGroup1XPacote> tblProductGroup1XPacote { get; set; }
        public virtual DbSet<tblProductGroups1> tblProductGroups1 { get; set; }
        public virtual DbSet<tblProducts> tblProducts { get; set; }
        public virtual DbSet<tblProfessor_GrandeArea> tblProfessor_GrandeArea { get; set; }
        public virtual DbSet<tblProspects> tblProspects { get; set; }
        public virtual DbSet<tblProspectsAdaptamed> tblProspectsAdaptamed { get; set; }
        public virtual DbSet<tblProtocoloGravacao_Emails> tblProtocoloGravacao_Emails { get; set; }
        public virtual DbSet<tblQuestaoConcurso_Imagem> tblQuestaoConcurso_Imagem { get; set; }
        public virtual DbSet<tblQuestao_Duvida> tblQuestao_Duvida { get; set; }
        public virtual DbSet<tblQuestao_Duvida_Encaminhamento> tblQuestao_Duvida_Encaminhamento { get; set; }
        public virtual DbSet<tblQuestao_Duvida_Imagem> tblQuestao_Duvida_Imagem { get; set; }
        public virtual DbSet<tblQuestao_Duvida_Lida> tblQuestao_Duvida_Lida { get; set; }
        public virtual DbSet<tblQuestao_Duvida_Moderada> tblQuestao_Duvida_Moderada { get; set; }
        public virtual DbSet<tblQuestao_Duvida_Resposta> tblQuestao_Duvida_Resposta { get; set; }
        public virtual DbSet<tblQuestao_Estatistica> tblQuestao_Estatistica { get; set; }
        public virtual DbSet<tblQuestao_Favoritas> tblQuestao_Favoritas { get; set; }
        public virtual DbSet<tblQuestao_Favoritas_Professores> tblQuestao_Favoritas_Professores { get; set; }
        public virtual DbSet<tblQuestao_MontaProva> tblQuestao_MontaProva { get; set; }
        public virtual DbSet<tblQuestionario> tblQuestionario { get; set; }
        public virtual DbSet<tblQuestionario_Cliente> tblQuestionario_Cliente { get; set; }
        public virtual DbSet<tblQuestionario_Detalhes> tblQuestionario_Detalhes { get; set; }
        public virtual DbSet<tblQuestionario_Questoes_Alternativas> tblQuestionario_Questoes_Alternativas { get; set; }
        public virtual DbSet<tblQuestionario_Tipo_Resposta> tblQuestionario_Tipo_Resposta { get; set; }
        public virtual DbSet<tblQuestoesConcursoImagem_Comentario> tblQuestoesConcursoImagem_Comentario { get; set; }
        public virtual DbSet<tblQuestoesConcursoImagem_ComentarioLog> tblQuestoesConcursoImagem_ComentarioLog { get; set; }

        public virtual DbSet<tblAlunoCrossPlataformaWhiteList> tblAlunoCrossPlataformaWhiteList { get; set; }

        public virtual DbSet<tblAlunoExcecaoAcessoTablet> tblAlunoExcecaoAcessoTablet { get; set; }

        public virtual DbSet<tblRPA> tblRPA { get; set; }

        public virtual DbSet<msp_API_ListaEntidades_Result> msp_API_ListaEntidades_Result { get; set; }

        public virtual DbSet<csp_ListaMaterialDireitoAluno_Result> csp_ListaMaterialDireitoAluno_Result { get; set; }
        

        public virtual DbSet<msp_LoadCP_MED_Choice_Result> msp_LoadCP_MED_Choice_Result { get; set; }
        

        public virtual DbSet<msp_Medsoft_SelectModulosPermitidos_Result> msp_Medsoft_SelectModulosPermitidos_Result { get; set; }

        public virtual DbSet<msp_API_ListaApostilas_Result> msp_API_ListaApostilas_Result { get; set; }

        
        public virtual DbSet<msp_HoraAulaTema_Result> msp_HoraAulaTema_Result { get; set; }
         

        public virtual DbSet<msp_API_LoadGrandeArea_Result> msp_API_LoadGrandeArea_Result { get; set; }

        public virtual DbSet<csp_CustomClient_PagamentosProdutosGeral_Result> csp_CustomClient_PagamentosProdutosGeral_Result { get; set; }
        
    
        public virtual DbSet<msp_GetDataLimite_ByApplication_Result> msp_GetDataLimite_ByApplication_Result { get; set; }

        public virtual DbSet<emed_CursosAnosStatus_Result> emed_CursosAnosStatus_Result { get; set; }
        
        public virtual DbSet<msp_Medsoft_SelectImagensComentProfessor_Result> msp_Medsoft_SelectImagensComentProfessor_Result { get; set; }

        public virtual DbSet<csp_getServerDate_Result> csp_getServerDate_Result { get; set; }

        public virtual DbSet<msp_Medsoft_SelectPermissaoExercicios_Result> msp_Medsoft_SelectPermissaoExercicios_Result { get; set; }

        public virtual DbSet<msp_API_NomeResumido_Result> msp_API_NomeResumido_Result { get; set; }

        

        
        
        
        public virtual DbSet<tblAcademicoVideoEmail> tblAcademicoVideoEmail { get; set; }
        public virtual DbSet<tblRPAGuia> tblRPAGuia { get; set; }
        public virtual DbSet<tblRPAGuia_Status> tblRPAGuia_Status { get; set; }
        public virtual DbSet<tblRPAGuia_Status_Historico> tblRPAGuia_Status_Historico { get; set; }
        public virtual DbSet<tblRPA_AcrescimoProduto> tblRPA_AcrescimoProduto { get; set; }
        public virtual DbSet<tblRPA_Ativo_Historico> tblRPA_Ativo_Historico { get; set; }
        public virtual DbSet<tblRPA_Cargos> tblRPA_Cargos { get; set; }
        public virtual DbSet<tblRPA_DadosBancarios> tblRPA_DadosBancarios { get; set; }
        public virtual DbSet<tblRPA_Observacao> tblRPA_Observacao { get; set; }
        public virtual DbSet<tblMapaMentalVideos> tblMapaMentalVideos { get; set; }
        public virtual DbSet<tblRPA_PermissaoStatus_Employee> tblRPA_PermissaoStatus_Employee { get; set; }
        public virtual DbSet<tblRPA_PermissaoStatus_Responsability> tblRPA_PermissaoStatus_Responsability { get; set; }
        public virtual DbSet<tblAlunoExcecaoSlideAulas> tblAlunoExcecaoSlideAulas { get; set; }
        public virtual DbSet<tblRPA_PermissaoXEmployee> tblRPA_PermissaoXEmployee { get; set; }
        public virtual DbSet<tblRPA_PermissaoXResponsability> tblRPA_PermissaoXResponsability { get; set; }
        public virtual DbSet<tblRPA_Status> tblRPA_Status { get; set; }
        public virtual DbSet<tblRPA_TipoPermissao> tblRPA_TipoPermissao { get; set; }
        public virtual DbSet<tblRegions> tblRegions { get; set; }
        public virtual DbSet<tblReplaceHtmlTags> tblReplaceHtmlTags { get; set; }
        public virtual DbSet<tblRequisicoes_Anexo> tblRequisicoes_Anexo { get; set; }
        public virtual DbSet<tblRequisicoes_Ativo> tblRequisicoes_Ativo { get; set; }
        public virtual DbSet<tblRequisicoes_AtivoMovimentacao> tblRequisicoes_AtivoMovimentacao { get; set; }
        public virtual DbSet<tblRequisicoes_Ativo_Historico> tblRequisicoes_Ativo_Historico { get; set; }
        public virtual DbSet<tblRequisicoes_Ativo_ProdutoCaracteristica> tblRequisicoes_Ativo_ProdutoCaracteristica { get; set; }
        public virtual DbSet<tblRequisicoes_Curso> tblRequisicoes_Curso { get; set; }
        public virtual DbSet<tblRequisicoes_Fornecedor> tblRequisicoes_Fornecedor { get; set; }
        public virtual DbSet<tblRequisicoes_FornecedorContato> tblRequisicoes_FornecedorContato { get; set; }
        public virtual DbSet<tblRequisicoes_FornecedorFormaPagamento> tblRequisicoes_FornecedorFormaPagamento { get; set; }
        public virtual DbSet<tblRequisicoes_Fornecedor_Produto> tblRequisicoes_Fornecedor_Produto { get; set; }
        public virtual DbSet<tblRequisicoes_IntranetLink> tblRequisicoes_IntranetLink { get; set; }
        public virtual DbSet<tblRequisicoes_Perfil> tblRequisicoes_Perfil { get; set; }
        public virtual DbSet<tblRequisicoes_PerfilItem> tblRequisicoes_PerfilItem { get; set; }
        public virtual DbSet<tblRequisicoes_Produto> tblRequisicoes_Produto { get; set; }
        public virtual DbSet<tblRequisicoes_ProdutoCaracteristica> tblRequisicoes_ProdutoCaracteristica { get; set; }
        public virtual DbSet<tblRequisicoes_ProdutoGrupo> tblRequisicoes_ProdutoGrupo { get; set; }
        public virtual DbSet<tblRequisicoes_Produto_Historico> tblRequisicoes_Produto_Historico { get; set; }
        public virtual DbSet<tblRequisicoes_Requisicao> tblRequisicoes_Requisicao { get; set; }
        public virtual DbSet<tblRequisicoes_RequisicaoCotacao> tblRequisicoes_RequisicaoCotacao { get; set; }
        public virtual DbSet<tblRequisicoes_RequisicaoHistorico> tblRequisicoes_RequisicaoHistorico { get; set; }
        public virtual DbSet<tblRequisicoes_RequisicaoItem> tblRequisicoes_RequisicaoItem { get; set; }
        public virtual DbSet<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica> tblRequisicoes_RequisicaoItem_ProdutoCaracteristica { get; set; }
        public virtual DbSet<tblRequisicoes_RequisicaoStatus> tblRequisicoes_RequisicaoStatus { get; set; }
        public virtual DbSet<tblRequisicoes_Setor> tblRequisicoes_Setor { get; set; }
        public virtual DbSet<tblRequisicoes_Unidade> tblRequisicoes_Unidade { get; set; }
        public virtual DbSet<tblRequisicoes_Workflow> tblRequisicoes_Workflow { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowAcao> tblRequisicoes_WorkflowAcao { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowAcao_Perfil> tblRequisicoes_WorkflowAcao_Perfil { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowBloqueio> tblRequisicoes_WorkflowBloqueio { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowCampo> tblRequisicoes_WorkflowCampo { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowCategoria> tblRequisicoes_WorkflowCategoria { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowEtapa> tblRequisicoes_WorkflowEtapa { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowEtapa_Perfil> tblRequisicoes_WorkflowEtapa_Perfil { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowHistorico> tblRequisicoes_WorkflowHistorico { get; set; }
        public virtual DbSet<tblRequisicoes_WorkflowRegra> tblRequisicoes_WorkflowRegra { get; set; }
        public virtual DbSet<tblRequisicoes_Workflow_Requisicao> tblRequisicoes_Workflow_Requisicao { get; set; }
        public virtual DbSet<tblResumoAulaIndice> tblResumoAulaIndice { get; set; }
        public virtual DbSet<tblResumoAulaTemaProfessorAssistido> tblResumoAulaTemaProfessorAssistido { get; set; }
        public virtual DbSet<tblResumoAulaVideo> tblResumoAulaVideo { get; set; }
        public virtual DbSet<tblResumoAulaVideoAprovacao> tblResumoAulaVideoAprovacao { get; set; }
        public virtual DbSet<tblResumoAulaVideoAprovacaoLog> tblResumoAulaVideoAprovacaoLog { get; set; }
        public virtual DbSet<tblResumoAulaVideoCorrigido> tblResumoAulaVideoCorrigido { get; set; }
        public virtual DbSet<tblResumoAulaVideoLogPosition> tblResumoAulaVideoLogPosition { get; set; }
        public virtual DbSet<tblResumoAulaVideoRelatorioReprovacaoLog> tblResumoAulaVideoRelatorioReprovacaoLog { get; set; }
        public virtual DbSet<tblRetiradaMaterialExtensivo> tblRetiradaMaterialExtensivo { get; set; }
        public virtual DbSet<tblRevalidaAulaIndice> tblRevalidaAulaIndice { get; set; }
        public virtual DbSet<tblRevalidaAulaTemaProfessorAssistido> tblRevalidaAulaTemaProfessorAssistido { get; set; }
        public virtual DbSet<tblRevalidaAulaVideo> tblRevalidaAulaVideo { get; set; }
        public virtual DbSet<tblRevalidaAulaVideoLogPosition> tblRevalidaAulaVideoLogPosition { get; set; }
        public virtual DbSet<tblRevisaoAulaIndice> tblRevisaoAulaIndice { get; set; }
        public virtual DbSet<tblRevisaoAulaTemaProfessorAssistido> tblRevisaoAulaTemaProfessorAssistido { get; set; }
        public virtual DbSet<tblRevisaoAulaVideo> tblRevisaoAulaVideo { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoAprovacao> tblRevisaoAulaVideoAprovacao { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoAprovacaoLog> tblRevisaoAulaVideoAprovacaoLog { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoCorrigido> tblRevisaoAulaVideoCorrigido { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoLog> tblRevisaoAulaVideoLog { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoLogPosition> tblRevisaoAulaVideoLogPosition { get; set; }
        public virtual DbSet<tblRevisaoAulaVideoRelatorioReprovacaoLog> tblRevisaoAulaVideoRelatorioReprovacaoLog { get; set; }
        public virtual DbSet<tblRevisaoAula_Slides> tblRevisaoAula_Slides { get; set; }
        public virtual DbSet<tblRodadaAluno> tblRodadaAluno { get; set; }
        public virtual DbSet<tblRodadaAvaliacao> tblRodadaAvaliacao { get; set; }
        public virtual DbSet<tblSchools> tblSchools { get; set; }
        public virtual DbSet<tblSeguranca> tblSeguranca { get; set; }
        public virtual DbSet<tblSeguranca_log> tblSeguranca_log { get; set; }
        public virtual DbSet<tblSellOrderDetails> tblSellOrderDetails { get; set; }
        public virtual DbSet<tblSellOrders> tblSellOrders { get; set; }
        public virtual DbSet<tblSellOrdersTemplate> tblSellOrdersTemplate { get; set; }
        public virtual DbSet<tblShippingCompanies> tblShippingCompanies { get; set; }
        public virtual DbSet<tblSimuladoTipos> tblSimuladoTipos { get; set; }
        public virtual DbSet<tblSitePage_AsTurmas_Enderecos> tblSitePage_AsTurmas_Enderecos { get; set; }
        public virtual DbSet<tblSitePage_AsTurmas_parametros> tblSitePage_AsTurmas_parametros { get; set; }
        public virtual DbSet<tblSitePagesGeral> tblSitePagesGeral { get; set; }
        public virtual DbSet<tblSmtpConfig> tblSmtpConfig { get; set; }
        public virtual DbSet<tblSoulMedicina_Tipo> tblSoulMedicina_Tipo { get; set; }
        public virtual DbSet<tblSpecialChars> tblSpecialChars { get; set; }
        public virtual DbSet<tblStates> tblStates { get; set; }
        public virtual DbSet<tblStore_CombosPaymentTemplate> tblStore_CombosPaymentTemplate { get; set; }
        public virtual DbSet<tblStore_Product_PaymentTemplate> tblStore_Product_PaymentTemplate { get; set; }
        public virtual DbSet<tblStores> tblStores { get; set; }
        public virtual DbSet<tblStores_Complement> tblStores_Complement { get; set; }
        public virtual DbSet<tblStores_New> tblStores_New { get; set; }
        public virtual DbSet<tblStores_Satelites> tblStores_Satelites { get; set; }
        public virtual DbSet<tblTeacher> tblTeacher { get; set; }
        public virtual DbSet<tblTelas> tblTelas { get; set; }
        public virtual DbSet<tblTelas1> tblTelas1 { get; set; }
        public virtual DbSet<tblTemplateDescontoTurmaCPMED> tblTemplateDescontoTurmaCPMED { get; set; }
        public virtual DbSet<tblTipoAcaoSimuladoImpresso> tblTipoAcaoSimuladoImpresso { get; set; }
        public virtual DbSet<tblTipoAcessoLogin> tblTipoAcessoLogin { get; set; }
        public virtual DbSet<tblTipoInteracao> tblTipoInteracao { get; set; }
        public virtual DbSet<tblTipoProcedimentoEntrada> tblTipoProcedimentoEntrada { get; set; }
        public virtual DbSet<tblTipoQuestaoPPT> tblTipoQuestaoPPT { get; set; }
        public virtual DbSet<tblTurmasCPMED_R> tblTurmasCPMED_R { get; set; }
        public virtual DbSet<tblVendasData> tblVendasData { get; set; }
        public virtual DbSet<tblVideoLog> tblVideoLog { get; set; }
        public virtual DbSet<tblVideoMedme> tblVideoMedme { get; set; }
        public virtual DbSet<tblVideoMedmeIndice> tblVideoMedmeIndice { get; set; }
        public virtual DbSet<tblVideoVote> tblVideoVote { get; set; }
        public virtual DbSet<tblVideo_Book> tblVideo_Book { get; set; }
        public virtual DbSet<tblVideo_Book_Intro> tblVideo_Book_Intro { get; set; }
        public virtual DbSet<tblVideo_Questao_Concurso> tblVideo_Questao_Concurso { get; set; }
        public virtual DbSet<tblVideo_SoulMedicina> tblVideo_SoulMedicina { get; set; }
        public virtual DbSet<tblVideos_Brutos> tblVideos_Brutos { get; set; }
        public virtual DbSet<tblVideos_Brutos_Busca> tblVideos_Brutos_Busca { get; set; }
        //public virtual DbSet<tblWarehouse_AntecipacaoMaterial> tblWarehouse_AntecipacaoMaterial { get; set; }
        public virtual DbSet<tblWarehouses> tblWarehouses { get; set; }
        public virtual DbSet<tblWarehousesClassRooms> tblWarehousesClassRooms { get; set; }
        public virtual DbSet<tblWarehousesClassRooms_Api> tblWarehousesClassRooms_Api { get; set; }
        public virtual DbSet<tbl_emed_access_business> tbl_emed_access_business { get; set; }
        public virtual DbSet<tbllogConcursoQuestoesGravacaoProtocolo_Codigos> tbllogConcursoQuestoesGravacaoProtocolo_Codigos { get; set; }
        public virtual DbSet<viewMedsoft_tblAluno> viewMedsoft_tblAluno { get; set; }

        public virtual DbSet<tblConcursoQuestaoCatologoDeClassificacoes> tblConcursoQuestaoCatologoDeClassificacoes { get; set; }

        public virtual DbSet<tblCronogramaExcecoesEntidades> tblCronogramaExcecoesEntidades { get; set; }
        public virtual DbSet<tblParametrosGenericos> tblParametrosGenericos { get; set; }
        public virtual DbSet<tblProductCodes> tblProductCodes { get; set; }

         public virtual DbSet<csp_loadMesesCursados_Result> csp_loadMesesCursados_Result { get; set; }

        public virtual DbSet<tblProdutoComboLiberado> tblProdutoComboLiberado { get; set; }
        
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connection = GetProfiledConnection();
                optionsBuilder.UseSqlServer(connection.ConnectionString);
                //optionsBuilder.UseSqlServer(@"Data Source=mbd.ordomederi.com;Initial Catalog=homologacaoCtrlPanel2012;Application Name=API;Persist Security Info=True;User ID=MateriaisDireito;Password=200t@bl7sL1m1t1;MultipleActiveResultSets=True");
            }
        }

        private DbConnection GetProfiledConnection()
        {
            var dbConnection = new System.Data.SqlClient.SqlConnection(ConfigurationProvider.Get("ConnectionStrings:DesenvConnection"));
            return new StackExchange.Profiling.Data.ProfiledDbConnection(dbConnection, MiniProfiler.Current);
        }

        #region modelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<tblProdutoComboLiberado>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblProdu__11B679328F8071ED");
            });

            modelBuilder.Entity<tblLogAvisos>(entity =>
            {
                entity.HasKey(e => e.intLogAvisoID);

                entity.HasIndex(e => new { e.bitConfirmaVisualizacao, e.intClientID, e.intAvisoID })
                    .HasName("_net_IX_tblLogAvisos_intClientID");

                entity.Property(e => e.dteVisualizacao).HasColumnType("datetime");
            });

            modelBuilder.Entity<ViewDireitoIntensivao_Chamados2018>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewDireitoIntensivao_Chamados2018");

                entity.Property(e => e.Acesso_ou_Retirada_no_Intensivão_2018)
                    .HasColumnName("Acesso ou Retirada no Intensivão 2018")
                    .HasMaxLength(10);

                entity.Property(e => e.Categoria).HasMaxLength(100);

                entity.Property(e => e.Chamado_criado_por)
                    .HasColumnName("Chamado criado por")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Data_chamado)
                    .HasColumnName("Data chamado")
                    .HasColumnType("datetime");

                entity.Property(e => e.Data_da_Ordem_de_Venda)
                    .HasColumnName("Data da Ordem de Venda")
                    .HasColumnType("datetime");

                entity.Property(e => e.Direito_Intensivão_Final)
                    .HasColumnName("Direito Intensivão Final")
                    .HasMaxLength(10);

                entity.Property(e => e.Direito_Intensivão_Regra)
                    .HasColumnName("Direito Intensivão Regra")
                    .HasMaxLength(10);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Filial).HasMaxLength(200);

                entity.Property(e => e.Intensivão_Cortesia_Anos_Anteriores_)
                    .IsRequired()
                    .HasColumnName("Intensivão Cortesia Anos Anteriores?")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Já_analisado_)
                    .IsRequired()
                    .HasColumnName("Já analisado?")
                    .HasMaxLength(255);

                entity.Property(e => e.Já_esteve_como_sim_no_passado_)
                    .IsRequired()
                    .HasColumnName("Já esteve como sim no passado?")
                    .HasMaxLength(50);

                entity.Property(e => e.Observação).HasMaxLength(1000);

                entity.Property(e => e.Ordem_de_Venda).HasColumnName("Ordem de Venda");

                entity.Property(e => e.Possui_OV_INTENSIVÃO_2018_)
                    .IsRequired()
                    .HasColumnName("Possui OV INTENSIVÃO 2018?")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Possui_chamado_de_TERMO_CONFORME_)
                    .IsRequired()
                    .HasColumnName("Possui chamado de TERMO CONFORME?")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Possui_venda_de_material_paga_no_ano_)
                    .IsRequired()
                    .HasColumnName("Possui venda de material paga no ano?")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Produto_Atual)
                    .HasColumnName("Produto Atual")
                    .HasMaxLength(200);

                entity.Property(e => e.Proutos_OVS_Anteriores)
                    .HasColumnName("Proutos OVS Anteriores")
                    .HasMaxLength(500);

                entity.Property(e => e.SALDO_ALUNO_ANOS_ANTERIORES).HasColumnName("SALDO ALUNO ANOS ANTERIORES");

                entity.Property(e => e.SALDO_ALUNO_NO_ANO).HasColumnName("SALDO ALUNO NO ANO");

                entity.Property(e => e.STATUS_DA_ORDEM_DE_VENDA_ATUAL)
                    .IsRequired()
                    .HasColumnName("STATUS DA ORDEM DE VENDA ATUAL")
                    .HasMaxLength(50);

                entity.Property(e => e.Status_do_Chamado_de_Termo)
                    .HasColumnName("Status do Chamado de Termo")
                    .HasMaxLength(255);

                entity.Property(e => e.Valor_Pago_INTENSIVÃO_2018).HasColumnName("Valor Pago INTENSIVÃO 2018");

                entity.Property(e => e.statusAluno)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcurso_ProvaCasoClinico>(entity =>
            {
                entity.HasKey(e => e.intCasoClinicoID)
                    .HasName("PK__tblConcu__9194E2EBA7A824D9");

                entity.Property(e => e.txtTexto).HasColumnType("text");
            });

            modelBuilder.Entity<tblConcurso_Recurso_MEDGRUPO>(entity =>
            {
                entity.Property(e => e.LoggedUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.bitActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.bitOpiniao).HasDefaultValueSql("((0))");

                entity.Property(e => e.dteCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtEditor).HasMaxLength(200);

                entity.Property(e => e.txtForum)
                    .HasMaxLength(30)
                    .HasComment("Q para forum de questão e P para forum de prova");

                entity.Property(e => e.txtPicturePath)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtRecurso_Comentario).IsRequired();
            });

            modelBuilder.Entity<mview_ConcursoProvas_Recursos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_ConcursoProvas_Recursos");

                entity.Property(e => e.CD_UF)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NM_CONCURSO)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SG_CONCURSO).HasMaxLength(350);

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeForumComentBlocked).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeLastUpdate).HasColumnType("datetime");

                entity.Property(e => e.dteExpiracao).HasColumnType("datetime");

                entity.Property(e => e.dteLightboxExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.txtBibliografia).HasMaxLength(1000);

                entity.Property(e => e.txtDescription).HasColumnType("text");

                entity.Property(e => e.txtGabaritoFinal).HasMaxLength(1000);

                entity.Property(e => e.txtGabaritoPreliminar).HasMaxLength(1000);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtProva)
                    .HasMaxLength(1000)
                    .IsFixedLength();
            });

            modelBuilder.Entity<mview_CondicoesRegra>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_CondicoesRegra");

                entity.Property(e => e.dteUltimaAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<mview_CondicoesRegra_Group>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_CondicoesRegra_Group");

                entity.Property(e => e.dteUltimaAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<mview_Cronograma>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.bitAllowedMaterial)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K13");

                entity.HasIndex(e => e.dteDateTime)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7");

                entity.HasIndex(e => e.intClassRoomID)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10");

                entity.HasIndex(e => e.intCourseID)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4");

                entity.HasIndex(e => e.intLessonID)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1");

                entity.HasIndex(e => e.intLessonTitleID)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6");

                entity.HasIndex(e => e.intLessonType)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11");

                entity.HasIndex(e => e.intSequence)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5");

                entity.HasIndex(e => e.intStoreID)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2");

                entity.HasIndex(e => e.intYear)
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4");

                entity.HasIndex(e => new { e.intClassRoomID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K2");

                entity.HasIndex(e => new { e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonSubjectID })
                    .HasName("_dta_stat_815873664_4_9");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K14");

                entity.HasIndex(e => new { e.intLessonID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_1");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence })
                    .HasName("_dta_stat_815873664_1_5");

                entity.HasIndex(e => new { e.intLessonID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14");

                entity.HasIndex(e => new { e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_6");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intYear })
                    .HasName("_dta_stat_815873664_6_14");

                entity.HasIndex(e => new { e.intSequence, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11");

                entity.HasIndex(e => new { e.intStoreID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K10");

                entity.HasIndex(e => new { e.intYear, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K4");

                entity.HasIndex(e => new { e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1");

                entity.HasIndex(e => new { e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11");

                entity.HasIndex(e => new { e.bitAllowedMaterial, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_13_4_6");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonTitleID })
                    .HasName("_net_IX_mview_Cronograma_intCourseID_intLessonTitleID");

                entity.HasIndex(e => new { e.dteDateTime, e.intDuration, e.intCourseID })
                    .HasName("_dta_stat_815873664_7_8_4");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intSequence, e.intStoreID })
                    .HasName("_dta_stat_815873664_7_5_2");

                entity.HasIndex(e => new { e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_10_4_6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intCourseID, e.intYear })
                    .HasName("_net_IX_mview_Cronograma_intCourseID_intYear");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K4");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11");

                entity.HasIndex(e => new { e.intCourseID, e.dteDateTime, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K7_K1");

                entity.HasIndex(e => new { e.intCourseID, e.dteDateTime, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_4_7");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonType })
                    .HasName("_dta_stat_815873664_4_5_11");

                entity.HasIndex(e => new { e.intCourseID, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K14_K1");

                entity.HasIndex(e => new { e.intLessonID, e.dteDateTime, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_1");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K14_1");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K11");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K4");

                entity.HasIndex(e => new { e.intLessonSubjectID, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_9_10");

                entity.HasIndex(e => new { e.intLessonTitleID, e.dteDateTime, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_6");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonSubjectID })
                    .HasName("_dta_stat_815873664_6_4_9");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intSequence, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_K11");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K14_K11");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_stat_815873664_11_6_1");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intYear })
                    .HasName("_dta_stat_815873664_11_6_14");

                entity.HasIndex(e => new { e.intSequence, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K4_K1");

                entity.HasIndex(e => new { e.intSequence, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K4");

                entity.HasIndex(e => new { e.intSequence, e.intLessonType, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K1");

                entity.HasIndex(e => new { e.intSequence, e.intLessonType, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K6");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intSequence })
                    .HasName("_dta_stat_815873664_2_4_5");

                entity.HasIndex(e => new { e.intYear, e.intCourseID, e.intLessonID })
                    .HasName("_dta_stat_815873664_14_4_1");

                entity.HasIndex(e => new { e.intYear, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_stat_815873664_14_1_7");

                entity.HasIndex(e => new { e.intYear, e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1_K4");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K10");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K6");

                entity.HasIndex(e => new { e.bitAllowedMaterial, e.intLessonID, e.intYear, e.intCourseID })
                    .HasName("_dta_stat_815873664_13_1_14_4");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K14_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_K6_K1");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K14_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intDuration, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K8_K4_K6");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intYear, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intYear, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K4_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intYear, e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1_K4_7");

                entity.HasIndex(e => new { e.intClassRoomID, e.dteDateTime, e.intLessonTitleID, e.intLessonType })
                    .HasName("_dta_stat_815873664_10_7_6_11");

                entity.HasIndex(e => new { e.intClassRoomID, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_10_4_9_6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K4_K1");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_stat_815873664_10_14_11_4");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K1_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K6_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K9_K6_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intDuration })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K7_K8");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K7_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intClassRoomID, e.intYear })
                    .HasName("_dta_stat_815873664_4_5_10_14");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K2");

                entity.HasIndex(e => new { e.intLessonID, e.dteDateTime, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_stat_815873664_1_7_10_4");

                entity.HasIndex(e => new { e.intLessonID, e.dteDateTime, e.intLessonTitleID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_1_7");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.dteDateTime, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K5_1_4");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.dteDateTime, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_1_4_7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_1_6_7_10");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K7_1_6");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K4_K2");

                entity.HasIndex(e => new { e.intLessonTitleID, e.dteDateTime, e.intDuration, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_6_7_8");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.dteDateTime, e.intDuration })
                    .HasName("_dta_stat_815873664_6_4_7_8");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.dteDateTime, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K7_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K10_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K10");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonSubjectID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K9_K10");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intSequence, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_K4_K1");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonID, e.dteDateTime, e.intYear })
                    .HasName("_dta_stat_815873664_11_1_7_14");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_stat_815873664_11_6_4_1");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intStoreID })
                    .HasName("_dta_stat_815873664_5_10_14_2");

                entity.HasIndex(e => new { e.intSequence, e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K4_K1_K6");

                entity.HasIndex(e => new { e.intSequence, e.intCourseID, e.intLessonID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K4_K1_K2");

                entity.HasIndex(e => new { e.intSequence, e.intLessonType, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_5_11_1_6");

                entity.HasIndex(e => new { e.intStoreID, e.intLessonID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_2_1_4_6");

                entity.HasIndex(e => new { e.intYear, e.intLessonTitleID, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_stat_815873664_14_6_1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intClassRoomID, e.intLessonID, e.intDuration, e.intLessonTitleID })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intLessonTitleID");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intClassRoomID, e.intLessonID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K1_K14_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonTitleID, e.intLessonID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_K6_K1_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K1_K4_K10_K6");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K1_K4_K6_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intYear, e.intClassRoomID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K10_K4_K1_7");

                entity.HasIndex(e => new { e.intClassRoomID, e.dteDateTime, e.intLessonTitleID, e.intLessonType, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K7_K6_K11_K14");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_stat_815873664_10_1_7_14_11");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear })
                    .HasName("_dta_stat_815873664_10_6_1_7_14");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K7_K6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intLessonType, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_4_10_11_14");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K11_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K2_K11");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10_K14_K11");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K6_K7_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K1_K7_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intLessonID, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K1_K10_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.dteDateTime, e.intCourseID, e.intClassRoomID, e.intYear })
                    .HasName("_net_IX_mview_Cronograma_intCourseID_intClassRoomID");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intCourseID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K4_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K11_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.dteDateTime, e.intClassRoomID, e.intYear })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_1_4_7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K10_K14_K11");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K6_K7_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K5_1_4_6");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intStoreID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K2_K5_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K7_K10_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intClassRoomID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_stat_815873664_1_6_10_4_7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K7_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intYear })
                    .HasName("_dta_stat_815873664_1_6_4_10_14");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K5_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_1_6_14_11_10");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_stat_815873664_1_5_10_14_11");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4");

                entity.HasIndex(e => new { e.intLessonTitleID, e.dteDateTime, e.intClassRoomID, e.intLessonType, e.intYear })
                    .HasName("_net_IX_mview_Cronograma_intClassRoomID_2");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_stat_815873664_6_10_14_11_4");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType })
                    .HasName("_dta_stat_815873664_6_10_14_2_11");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID, e.dteDateTime, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K7_K10");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intLessonID, e.intClassRoomID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K1_K10_K4_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonType, e.intClassRoomID, e.intYear, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K10_K14_K7_K6");

                entity.HasIndex(e => new { e.intLessonType, e.intClassRoomID, e.intYear, e.intStoreID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K10_K14_K2_K4");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear })
                    .HasName("_dta_stat_815873664_11_6_1_7_14");

                entity.HasIndex(e => new { e.intLessonType, e.intSequence, e.intLessonTitleID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_stat_815873664_11_5_6_4_1");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intClassRoomID, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K14_K10_K7_K6");

                entity.HasIndex(e => new { e.intStoreID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_stat_815873664_2_10_14_11_4");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intSequence, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K5_K1_K7");

                entity.HasIndex(e => new { e.intStoreID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_2_14_11_1_10");

                entity.HasIndex(e => new { e.intYear, e.intLessonID, e.intStoreID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_14_1_2_4_6");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K10_K7_K6");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID })
                    .HasName("PK_tblMview_Cronograma_DW")
                    .IsUnique()
                    .IsClustered();

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intLessonTitleID, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K6_K10_K7");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intClassRoomID, e.intLessonType, e.intSequence, e.intLessonID })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intLessonID_2");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_K1_K14_K11_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intSequence, e.intLessonType, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K11_K6_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K1_K4_K14_K11_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intSequence, e.intLessonType, e.intLessonTitleID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K11_K6_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intCourseID, e.intLessonID, e.intLessonType, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K11_K5_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intLessonID, e.intCourseID, e.intLessonType, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K1_K4_K11_K5_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intSequence, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_K11_K4_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intSequence, e.intLessonType, e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_K11_K1_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonType, e.intLessonTitleID, e.intCourseID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K6_K4_K1_K5_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonType, e.intLessonTitleID, e.intLessonID, e.intCourseID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K6_K1_K4_K5_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonType, e.intSequence, e.intLessonTitleID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K5_K6_K4_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonType, e.intSequence, e.intLessonTitleID, e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K5_K6_K1_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intSequence, e.intLessonType, e.intCourseID, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K4_K6_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intSequence, e.intLessonType, e.intLessonID, e.intLessonTitleID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K1_K6_K4_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intSequence, e.intLessonType, e.intLessonTitleID, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K6_K4_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intSequence, e.intLessonType, e.intLessonTitleID, e.intLessonID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K11_K6_K1_K4_7");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_stat_815873664_10_6_1_7_14_11");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1_K6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1_K2");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4_K1");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_10_14_2_11_4_6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4_K5");

                entity.HasIndex(e => new { e.intCourseID, e.dteDateTime, e.intClassRoomID, e.intLessonType, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_4_7_10_11_14");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intYear })
                    .HasName("_dta_stat_815873664_4_10_1_6_7_14");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K11_K1_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K11_K1_K2");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10_K14_K11_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intClassRoomID, e.intLessonType, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_4_6_10_11_14");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K1_K14_K11_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID, e.intLessonID })
                    .HasName("_dta_stat_815873664_4_11_9_6_10_1");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K10_K14_K11_K1");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K11_K4_K2");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K2_K11_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID, e.intStoreID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_net_IX_tblMview_Cronograma_DW_dteDateTime");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K6_K14_K11_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intStoreID, e.intSequence, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K2_K5_K7_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K2");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K10_K14_K11");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_1_6_4_14_11_10");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K10_K14_K11_K4");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonTitleID, e.intLessonType, e.intCourseID, e.dteDateTime })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intCourseID_dtedateTime");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_K6");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K10_K14_K11_K4_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K10_K14_K2_K11_K4");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K10_K14_K11_K1");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K10_K14_K11");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K1_K14_K11_K10_K4");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K14_K11_K1_K10_K4");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intLessonID, e.dteDateTime, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K7_K10_K4_11_14");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intLessonID, e.intLessonTitleID, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K10_K4_11_14");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K10_K14_K11_K4_K1");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K10_K14_K2_K11_K4");

                entity.HasIndex(e => new { e.intStoreID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K10_K14_K11_K4_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K1_K14_K11_K10");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_2_4_11_9_6_10");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intSequence, e.intLessonID, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K5_K1_K7_K6");

                entity.HasIndex(e => new { e.intStoreID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K14_K11_K1_K10_K4");

                entity.HasIndex(e => new { e.intYear, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_14_4_11_9_6_10");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K1_K10_K4_K2");

                entity.HasIndex(e => new { e.txtStoreName, e.intCourseID, e.dteDateTime, e.intClassRoomID, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intLessonSubjectID");

                entity.HasIndex(e => new { e.dteDateTime, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1_K6_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_7_4_1_14_11_10_6");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intLessonTitleID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K4_K6_K1_K14_K11_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K11_K4_K6_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K1_K4_K6_K14_K11_K10");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K1_K4_K14_K11_K10_K6");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_K6_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K10_K14_K11_K4_K1_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonType, e.intLessonID, e.intYear, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K1_K14_K10_K4_K6_7");

                entity.HasIndex(e => new { e.dteDateTime, e.intYear, e.intLessonID, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1_K11_K10_K4_K6_7");

                entity.HasIndex(e => new { e.intClassRoomID, e.intCourseID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K4_K6_K1_K7_K14_K11");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K1_K7_K14_K11_K4_K6");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K6_K1_K7_K14_K11_K4");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonTitleID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1_K6_K2");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K1_K6_K7_K14_K11");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K11_K1_K6_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_4_10_14_2_11_9_6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID, e.intLessonTitleID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10_K6_K7_K14_K11");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K10_K14_K11_K6_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K1_K14_K11_K10_K7_K6");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID, e.intLessonType, e.intYear, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_4_6_7_10_11_14");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K1_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K6_K14_K11_K1_K10_K2");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonSubjectID })
                    .HasName("_dta_stat_815873664_1_10_14_2_11_4_9");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID, e.intLessonType, e.intYear, e.dteDateTime, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K5_1_4_10_11_14");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K10_K14_K11_K7_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K10_K14_K11_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K5_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K10_K14_K11_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K14_K11_K10_K4_K2");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.dteDateTime, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_K7_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K14_K11_K10_K4_K6_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K10_K14_K11_K4_K1_K2");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intLessonID, e.dteDateTime, e.intYear, e.intLessonType })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K10_K1_K7_K14_K11");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K10_K14_K11_K1_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intCourseID, e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K4_K1_K10_K14_K11_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K1_K14_K11_K10_K4_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intSequence, e.intCourseID, e.intLessonID, e.intStoreID, e.intYear, e.intLessonType })
                    .HasName("_dta_stat_815873664_6_5_4_1_2_14_11");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K14_K11_K1_K10_K4_K2");

                entity.HasIndex(e => new { e.intLessonType, e.intCourseID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K4_K6_K1_K7_K14_K10");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonID, e.dteDateTime, e.intYear, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K1_K7_K14_K10_K4_K6");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intYear, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K6_K1_K7_K14_K10_K4");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonTitleID, e.intYear, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K6_K14_K1_K10_K4_K2");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intLessonID, e.dteDateTime, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K7_K10_K4_K6_11_14");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K7_K10_K4_11_14");

                entity.HasIndex(e => new { e.intLessonType, e.intYear, e.intLessonID, e.intLessonTitleID, e.intClassRoomID, e.intCourseID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K10_K4_K7_11_14");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intStoreID })
                    .HasName("_dta_stat_815873664_5_10_14_11_4_1_2");

                entity.HasIndex(e => new { e.intStoreID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K10_K14_K11_K4_K1_K6");

                entity.HasIndex(e => new { e.intStoreID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K6_K14_K11_K1_K10_K4");

                entity.HasIndex(e => new { e.intStoreID, e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K14_K11_K1_K10_K4_K6");

                entity.HasIndex(e => new { e.intYear, e.intCourseID, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K4_K6_K1_K7_K11_K10");

                entity.HasIndex(e => new { e.intYear, e.intLessonID, e.dteDateTime, e.intLessonType, e.intClassRoomID, e.intCourseID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1_K7_K11_K10_K4_K6");

                entity.HasIndex(e => new { e.intYear, e.intLessonTitleID, e.intLessonID, e.dteDateTime, e.intLessonType, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K6_K1_K7_K11_K10_K4");

                entity.HasIndex(e => new { e.intYear, e.intLessonTitleID, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K6_K11_K1_K10_K4_K2");

                entity.HasIndex(e => new { e.intYear, e.intLessonType, e.intLessonID, e.intClassRoomID, e.intCourseID, e.intStoreID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K11_K1_K10_K4_K2_K6");

                entity.HasIndex(e => new { e.txtStoreName, e.intCourseID, e.dteDateTime, e.intClassRoomID, e.intYear, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_mview_Cronograma_intYear_intLessonSubjectID");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonType, e.intYear, e.intLessonID, e.intCourseID, e.intStoreID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K2_K5_K7_10_11_14");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K6_K7_K14_K11_K10");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime, e.intStoreID })
                    .HasName("_dta_stat_815873664_4_5_1_14_11_10_7_2");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intClassRoomID, e.intLessonType, e.intYear, e.dteDateTime, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K5_1_4_6_10_11_14");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intStoreID, e.intSequence, e.dteDateTime, e.intLessonTitleID, e.intYear, e.intLessonType })
                    .HasName("_dta_stat_815873664_1_4_2_5_7_6_14_11");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intSequence, e.dteDateTime, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K5_K7_K2");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intSequence, e.dteDateTime, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_stat_815873664_1_6_4_5_7_14_11_10");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID })
                    .HasName("_dta_stat_815873664_5_10_14_2_11_4_9_6");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.dteDateTime, e.intLessonID, e.intLessonTitleID, e.intClassRoomID, e.intSequence, e.intLessonType })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intLessonType");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intCourseID, e.intLessonID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intStoreID, e.intSequence })
                    .HasName("_dta_stat_815873664_10_14_4_1_11_9_6_2_5");

                entity.HasIndex(e => new { e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K11_K4_K1_K2_K6_K5_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K11_K1_K2_K6_K5_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K10_K14_K11_K1_K2_K6_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intLessonTitleID, e.dteDateTime, e.intYear, e.intLessonType, e.intClassRoomID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K6_K7_K14_K11_K10_K2");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K2_K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime, e.intStoreID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K1_K14_K11_K10_K7_K2_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intStoreID, e.intLessonTitleID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_stat_815873664_1_10_14_11_4_2_6_5_7");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intLessonSubjectID, e.intClassRoomID, e.bitAllowedMaterial, e.intYear, e.intLessonType })
                    .HasName("_net_IX_mview_Cronograma_bitAllowedMaterial");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intLessonSubjectID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType })
                    .HasName("_net_IX_mview_Cronograma_intStoreID_intLessonType");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intClassRoomID, e.intLessonType, e.intYear, e.dteDateTime, e.intSequence, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K7_K5_K2_1_4_6_10_11_14");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intStoreID, e.intSequence, e.dteDateTime, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K2_K5_K7_K6_K14_K11_K10");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intSequence, e.dteDateTime, e.intStoreID, e.intLessonTitleID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K5_K7_K2_K6");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intStoreID, e.intLessonTitleID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K4_K14_K11_K10_K2_K6_K5_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intSequence, e.dteDateTime, e.intYear, e.intLessonType, e.intClassRoomID, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K5_K7_K14_K11_K10_K2");

                entity.HasIndex(e => new { e.intLessonID, e.intLessonTitleID, e.intCourseID, e.intYear, e.intLessonType, e.intClassRoomID, e.intStoreID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K6_K4_K14_K11_K10_K2_K5_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intStoreID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K10_K14_K11_K4_K2_K6_K7");

                entity.HasIndex(e => new { e.intLessonID, e.intSequence, e.intCourseID, e.intStoreID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K5_K4_K2_K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.intSequence, e.intCourseID, e.intLessonID, e.intStoreID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K5_K4_K1_K2_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonType, e.intLessonID, e.intStoreID, e.intCourseID, e.intLessonTitleID, e.intSequence, e.intYear, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K1_K2_K4_K6_K5_K14_K10_K7");

                entity.HasIndex(e => new { e.intSequence, e.intClassRoomID, e.intYear, e.intLessonType, e.intCourseID, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K10_K14_K11_K4_K1_K2_K6_K7");

                entity.HasIndex(e => new { e.intSequence, e.intCourseID, e.intLessonID, e.intLessonTitleID, e.intStoreID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K4_K1_K6_K2_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intSequence, e.intCourseID, e.intLessonID, e.intStoreID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K4_K1_K2_K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intStoreID, e.dteDateTime, e.intClassRoomID, e.intYear, e.intCourseID, e.intSequence, e.intLessonType, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K5_K11_K6_K1_2_7_10_14");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intLessonID, e.intYear, e.intLessonType, e.intClassRoomID, e.intLessonTitleID, e.intSequence, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K1_K14_K11_K10_K6_K5_K7");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intSequence, e.intLessonID, e.dteDateTime, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K5_K1_K7_K6_K14_K11_K10");

                entity.HasIndex(e => new { e.intStoreID, e.intCourseID, e.intSequence, e.intLessonID, e.intLessonTitleID, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K5_K1_K6_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intStoreID, e.intLessonID, e.intCourseID, e.intLessonTitleID, e.intSequence, e.intYear, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K1_K4_K6_K5_K14_K11_K10_K7");

                entity.HasIndex(e => new { e.intYear, e.intLessonID, e.intStoreID, e.intCourseID, e.intLessonTitleID, e.intSequence, e.intLessonType, e.intClassRoomID, e.dteDateTime })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K1_K2_K4_K6_K5_K11_K10_K7");

                entity.HasIndex(e => new { e.intLessonTitleID, e.dteDateTime, e.intLessonID, e.txtStoreName, e.intCourseID, e.intLessonSubjectID, e.intClassRoomID, e.intLessonType, e.intYear, e.intStoreID })
                    .HasName("_net_IX_mview_Cronograma_intYear_intStoreID");

                entity.HasIndex(e => new { e.intLessonID, e.txtStoreName, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intLessonSubjectID, e.intLessonType, e.bitAllowedAccess, e.bitAllowedMaterial, e.intSequence, e.intClassRoomID, e.intYear, e.intStoreID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K10_K14_K2_1_3_4_6_7_9_11_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intClassRoomID, e.intYear, e.intCourseID, e.intLessonID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intStoreID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K4_K1_K11_K9_K6_K2_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonSubjectID, e.intLessonTitleID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4_K1_K9_K6_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonTitleID, e.intLessonSubjectID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4_K6_K9_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intSequence, e.intLessonSubjectID, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K10_K14_K2_K11_K4_K5_K9_K6_K1_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intCourseID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K10_K14_K2_K11_K9_K6_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID, e.intLessonID, e.intYear, e.intStoreID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K4_K11_K9_K6_K10_K1_K14_K2_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intLessonID, e.intClassRoomID, e.intYear, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intStoreID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K4_K11_K9_K6_K2_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intLessonID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K1_K10_K14_K2_K11_K4_K9_K6_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonSubjectID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K6_K10_K14_K2_K11_K4_K9_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intLessonType, e.intClassRoomID, e.intYear, e.intStoreID, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K11_K10_K14_K2_K4_K9_K6_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intSequence, e.intClassRoomID, e.intYear, e.intStoreID, e.intLessonType, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID, e.intLessonID })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K5_K10_K14_K2_K11_K4_K9_K6_K1_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intStoreID, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID, e.intYear, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K2_K4_K11_K9_K6_K10_K14_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intYear, e.intCourseID, e.intLessonType, e.intLessonSubjectID, e.intLessonTitleID, e.intClassRoomID, e.intStoreID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K4_K11_K9_K6_K10_K2_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.txtStoreName, e.dteDateTime, e.bitAllowedAccess, e.bitAllowedMaterial, e.intYear, e.intStoreID, e.intClassRoomID, e.intLessonType, e.intCourseID, e.intLessonSubjectID, e.intLessonTitleID, e.intLessonID, e.intSequence })
                    .HasName("_dta_index_mview_Cronograma_5_815873664__K14_K2_K10_K11_K4_K9_K6_K1_K5_3_7_12_13");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonID, e.dteDateTime, e.intCourseID, e.txtStoreName, e.intSequence, e.intLessonTitleID, e.intDuration, e.intLessonSubjectID, e.intLessonType, e.bitAllowedAccess, e.bitAllowedMaterial, e.intStoreID, e.intYear })
                    .HasName("_net_IX_mview_Cronograma_intStoreID");

                entity.HasIndex(e => new { e.intLessonID, e.intStoreID, e.txtStoreName, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.bitAllowedAccess, e.bitAllowedMaterial, e.intClassRoomID, e.intYear, e.intSequence, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_mview_Cronograma_intClassRoomID");

                entity.HasIndex(e => new { e.intLessonID, e.intStoreID, e.txtStoreName, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.bitAllowedAccess, e.bitAllowedMaterial, e.intYear, e.intClassRoomID, e.intSequence, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_mview_Cronograma_intClassRoomID_intSequence");

                entity.HasIndex(e => new { e.intLessonID, e.intStoreID, e.txtStoreName, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.intClassRoomID, e.bitAllowedAccess, e.bitAllowedMaterial, e.intYear, e.intSequence, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_tblMview_Cronograma_DW_intYear");

                entity.HasIndex(e => new { e.intLessonID, e.intStoreID, e.txtStoreName, e.intSequence, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.intLessonSubjectID, e.intClassRoomID, e.bitAllowedAccess, e.bitAllowedMaterial, e.intCourseID, e.intLessonType, e.intYear })
                    .HasName("_net_IX_mview_Cronograma_intCourseID_intLessonType");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtStoreName).HasMaxLength(50);
            });

            modelBuilder.Entity<mview_ProductCombos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_ProductCombos");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAPI_EntidadesValidas>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<tblAPI_LiberacaoApostila>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoApostilaID);

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblAPI_LiberacaoApostila)
                    .HasForeignKey(d => d.intBookID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAPI_LiberacaoApostila_tblBooks");
            });

            modelBuilder.Entity<tblAPI_VisualizarAntecedencia>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tblAccess>(entity =>
            {
                entity.HasKey(e => e.intAccessID);

                entity.HasOne(d => d.intAction)
                    .WithMany(p => p.tblAccess)
                    .HasForeignKey(d => d.intActionID)
                    .HasConstraintName("FK_tblAccess_tblAccess_Action");

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblAccess)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblProductGroups1");

                entity.HasOne(d => d.intScreen)
                    .WithMany(p => p.tblAccess)
                    .HasForeignKey(d => d.intScreenID)
                    .HasConstraintName("FK_tblAccess_tblAccess_Screen");

                entity.HasOne(d => d.intYearType)
                    .WithMany(p => p.tblAccess)
                    .HasForeignKey(d => d.intYearTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblAccess_YearType");
            });

            modelBuilder.Entity<tblAccessLog>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.intPeopleID, e.dteDateTime, e.bitAccess })
                    .HasName("IX_tblAccessLog_Full");

                entity.HasIndex(e => new { e.dteDateTime, e.intPeopleID, e.intClassroomID, e.intAccessType })
                    .HasName("_net_ix_tblAccessLog_dteDateTime");

                entity.HasIndex(e => new { e.intPeopleID, e.intClassroomID, e.bitAccess, e.dteDateTime })
                    .HasName("IX_tblAccessLog_intClassroomID_bitAccess_dteDateTime_90F61");

                entity.HasIndex(e => new { e.intPeopleID, e.dteDateTime, e.intClassroomID, e.intEmployeeID, e.bitAccess })
                    .HasName("_net_ix_tblAccessLog_bitAccess");

                entity.HasIndex(e => new { e.intPeopleID, e.intAccessType, e.txtComment, e.intClassroomID, e.dteDateTime })
                    .HasName("_net_IX_tblAccessLog_intClassroomID");

                entity.HasIndex(e => new { e.dteDateTime, e.intClassroomID, e.bitEntrada, e.txtComment, e.intAccessType, e.txtOpCode, e.intPeopleID, e.bitAccess })
                    .HasName("_net_IX_tblAccessLog_intPeopleID_bitAccess");

                entity.HasIndex(e => new { e.dteDateTime, e.bitEntrada, e.intEmployeeID, e.intClassroomID, e.bitAccess, e.txtComment, e.intAccessType, e.txtOpCode, e.intPeopleID })
                    .HasName("IX_tblAccessLog_intPeopleID_6A955");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtOpCode).HasMaxLength(50);
            });

            modelBuilder.Entity<tblAccess_Action>(entity =>
            {
                entity.HasKey(e => e.intActionId);

                entity.Property(e => e.txtAction).HasMaxLength(10);
            });

            modelBuilder.Entity<tblAccess_Application>(entity =>
            {
                entity.HasKey(e => e.intApplicationID);

                entity.Property(e => e.txtApplication)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<tblAccess_Button>(entity =>
            {
                entity.HasKey(e => e.intButtonId)
                    .HasName("PK__tblAcces__9FADB0437987C02D");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrl)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Combo>(entity =>
            {
                entity.HasKey(e => e.intComboId)
                    .HasName("PK__tblAcces__66B7BA6567871506");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Condition>(entity =>
            {
                entity.HasKey(e => e.intConditionID);

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblAccess_Condition)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Condition_tblProductGroups1");

                entity.HasOne(d => d.intYearType)
                    .WithMany(p => p.tblAccess_Condition)
                    .HasForeignKey(d => d.intYearTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Condition_tblAccess_YearType");
            });

            modelBuilder.Entity<tblAccess_DataLimite>(entity =>
            {
                entity.HasKey(e => new { e.intAplicationID, e.intAlunoYear });

                entity.Property(e => e.dteDataLimite)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.intDataLimiteID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblAccess_Detail>(entity =>
            {
                entity.HasKey(e => e.intDetalheId)
                    .HasName("PK__tblAcces__914264C134C80977");

                entity.HasIndex(e => new { e.intProductGroupId, e.intTipoAnoId, e.intStatusOV, e.intStatusPagamento, e.intCallCategory, e.intStatusInterno, e.intClientId, e.intCourseID, e.intStoreID, e.intGroupId, e.intAnoMinimo })
                    .HasName("IX_tblAccess_Detail")
                    .IsUnique();

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteUltimaAlteracao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.intAnoMinimo).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intCallCategory).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intClientId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intCourseID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intEmployeeID).HasDefaultValueSql("((131220))");

                entity.Property(e => e.intGroupId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intProductGroupId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStatusInterno).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStatusOV).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStatusPagamento).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStoreID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intTipoAnoId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Device>(entity =>
            {
                entity.HasKey(e => e.intDeviceTipoID);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Empresa_Application>(entity =>
            {
                entity.HasKey(e => e.intEmpresaApplication)
                    .HasName("PK__tblAcces__C021C63871C1EAED");

                entity.Property(e => e.txtApplication)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Empresas>(entity =>
            {
                entity.HasKey(e => e.intEmpresaId)
                    .HasName("PK__tblAcces__DCC7636A3D4E22AE");

                entity.Property(e => e.txtEmpresa)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Key>(entity =>
            {
                entity.HasKey(e => e.intKeyID);

                entity.Property(e => e.txtPrivateKey)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.txtPublicKey)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.txtShortPublicKey)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Menu>(entity =>
            {
                entity.HasKey(e => e.intMenuId)
                    .HasName("PK__tblAcces__5A2F7F1B30F77893");

                entity.Property(e => e.txtExternalPageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtTarget)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrl)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_MenuProduto>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblAcces__11B679324011949A");

                entity.HasOne(d => d.intMenu)
                    .WithMany(p => p.tblAccess_MenuProduto)
                    .HasForeignKey(d => d.intMenuID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAccess__intMe__41F9DD0C");

                entity.HasOne(d => d.intProduto)
                    .WithMany(p => p.tblAccess_MenuProduto)
                    .HasForeignKey(d => d.intProdutoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAccess__intPr__42EE0145");
            });

            modelBuilder.Entity<tblAccess_Menu_Apagar>(entity =>
            {
                entity.HasKey(e => e.intMenuID)
                    .HasName("PK_tblAccess_Menu");

                entity.Property(e => e.txtDescription).HasMaxLength(50);

                entity.Property(e => e.txtLink).HasMaxLength(100);

                entity.HasOne(d => d.intApplication)
                    .WithMany(p => p.tblAccess_Menu_Apagar)
                    .HasForeignKey(d => d.intApplicationID)
                    .HasConstraintName("FK_tblAccess_Menu_tblAccess_Application");
            });

            modelBuilder.Entity<tblAccess_Menu_ProductGroup>(entity =>
            {
                entity.HasKey(e => e.intMenuProductGroup)
                    .HasName("PK__tblAcces__D68FB08618B6A605");

                entity.HasOne(d => d.intMenu)
                    .WithMany(p => p.tblAccess_Menu_ProductGroup)
                    .HasForeignKey(d => d.intMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Menu_ProductGroup_tblAccess_Menu");
            });

            modelBuilder.Entity<tblAccess_Menu_Url>(entity =>
            {
                entity.HasKey(e => e.intMenuId)
                    .HasName("PK__tblAcces__5A2F7F1B7E41103D");

                entity.Property(e => e.txtUrl)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Message>(entity =>
            {
                entity.HasKey(e => e.intMessageID);

                entity.Property(e => e.intMessageID).ValueGeneratedNever();

                entity.Property(e => e.txtMessage)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblAccess_Object>(entity =>
            {
                entity.HasKey(e => e.intObjectId)
                    .HasName("PK__tblAcces__985870EF0AD1CFAB");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.dteUltimaAlteracao).HasColumnType("datetime");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblAccess_Object)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Object_tblEmployees");

                entity.HasOne(d => d.intObjectType)
                    .WithMany(p => p.tblAccess_Object)
                    .HasForeignKey(d => d.intObjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Object_tblAccess_ObjectType");
            });

            modelBuilder.Entity<tblAccess_ObjectType>(entity =>
            {
                entity.HasKey(e => e.intObjectTypeId)
                    .HasName("PK__tblAcces__218149A32DC53AD6");

                entity.Property(e => e.txtType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Object_Application>(entity =>
            {
                entity.HasKey(e => e.intobjectApplication)
                    .HasName("PK__tblAcces__4A8431EB26839B36");

                entity.Property(e => e.txtMinVersion)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0.0.0')");

                entity.Property(e => e.txtMinVersionOffline)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0.0.0')");
            });

            modelBuilder.Entity<tblAccess_PermissionNotification>(entity =>
            {
                entity.HasKey(e => e.intPermissionNotificationId)
                    .HasName("PK__tblAcces__1F1CD7256B287A80");

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.HasOne(d => d.intNotificacao)
                    .WithMany(p => p.tblAccess_PermissionNotification)
                    .HasForeignKey(d => d.intNotificacaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Access_PermissionNotification_Notificacao");
            });

            modelBuilder.Entity<tblAccess_PermissionObject>(entity =>
            {
                entity.HasKey(e => e.intPermissionObject)
                    .HasName("PK__tblAcces__61864DAD5534D909");

                entity.HasIndex(e => new { e.intObjectId, e.intPermissaoRegraId, e.intApplicationID })
                    .HasName("IX_tblAccess_PermissionObject")
                    .IsUnique();

                entity.Property(e => e.dteDataAlteracao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.intApplicationID).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.intObject)
                    .WithMany(p => p.tblAccess_PermissionObject)
                    .HasForeignKey(d => d.intObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_PermissionObject_tblAccess_Object");

                entity.HasOne(d => d.intPermissaoRegra)
                    .WithMany(p => p.tblAccess_PermissionObject)
                    .HasForeignKey(d => d.intPermissaoRegraId)
                    .HasConstraintName("fk_tblAccess_PermissionObject_tblAccess_Permission_Rule");
            });

            modelBuilder.Entity<tblAccess_Permission_Rule>(entity =>
            {
                entity.HasKey(e => e.intPermissaoRegraId)
                    .HasName("PK__tblAcces__0AFF746F4C9F9308");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteUltimaAlteracao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteValidoAte).HasColumnType("datetime");

                entity.Property(e => e.dteValidoDe).HasColumnType("datetime");

                entity.Property(e => e.intEmployeeID).HasDefaultValueSql("((131220))");

                entity.Property(e => e.intInterruptorId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intMensagemId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intOrdem).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtDescricao).HasMaxLength(150);

                entity.HasOne(d => d.intAccesso)
                    .WithMany(p => p.tblAccess_Permission_Rule)
                    .HasForeignKey(d => d.intAccessoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Permission_Rule_tblAccess_Permission_Status");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblAccess_Permission_Rule)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Permission_Rule_tblEmployees");

                entity.HasOne(d => d.intRegra)
                    .WithMany(p => p.tblAccess_Permission_Rule)
                    .HasForeignKey(d => d.intRegraId)
                    .HasConstraintName("fk_tblAccess_Permission_Rule_tblAccess_Rule");
            });

            modelBuilder.Entity<tblAccess_Permission_Status>(entity =>
            {
                entity.HasKey(e => e.intAccessoId)
                    .HasName("PK__tblAcces__66D98BE835665C9E");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_PersonalException>(entity =>
            {
                entity.HasKey(e => e.intPersonalExceptionID);

                entity.Property(e => e.dteEnd).HasColumnType("datetime");

                entity.Property(e => e.dteStart).HasColumnType("datetime");

                entity.Property(e => e.txtDescription).HasMaxLength(500);

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblAccess_PersonalException)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_PersonalException_tblProductGroups1");

                entity.HasOne(d => d.intScreenAction)
                    .WithMany(p => p.tblAccess_PersonalException)
                    .HasForeignKey(d => d.intScreenActionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_PersonalException_tblAccess_Screen_Action");

                entity.HasOne(d => d.intYearType)
                    .WithMany(p => p.tblAccess_PersonalException)
                    .HasForeignKey(d => d.intYearTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_PersonalException_tblAccess_YearType");
            });

            modelBuilder.Entity<tblAccess_ProdutoEmpresas>(entity =>
            {
                entity.HasKey(e => e.intProdutoEmpresaId)
                    .HasName("PK__tblAcces__54F0046452493F94");

                entity.Property(e => e.txtproduto)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccess_Rule>(entity =>
            {
                entity.HasKey(e => e.intRegraId)
                    .HasName("PK__tblAcces__6D82FE3438989A5B");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteCriacao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteUltimaAlteracao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.intEmployeeID).HasDefaultValueSql("((131220))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblAccess_Rule)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblAccess_Rule_tblEmployees");
            });

            modelBuilder.Entity<tblAccess_RuleDetail>(entity =>
            {
                entity.HasKey(e => e.intRegraDetalheId)
                    .HasName("PK__tblAcces__A0F95EE43D5D4F78");

                entity.HasIndex(e => new { e.intRegraId, e.intDetalheId })
                    .HasName("IX_tblAccess_RuleDetail")
                    .IsUnique();

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intDetalhe)
                    .WithMany(p => p.tblAccess_RuleDetail)
                    .HasForeignKey(d => d.intDetalheId)
                    .HasConstraintName("fk_tblAccess_RuleDetail_tblAccess_Detail");

                entity.HasOne(d => d.intRegra)
                    .WithMany(p => p.tblAccess_RuleDetail)
                    .HasForeignKey(d => d.intRegraId)
                    .HasConstraintName("fk_tblAccess_RuleDetail_tblAccess_Rule");
            });

            modelBuilder.Entity<tblAccess_Rule_Condition>(entity =>
            {
                entity.HasKey(e => e.intRuleConditionID);

                entity.HasOne(d => d.intCondition)
                    .WithMany(p => p.tblAccess_Rule_Condition)
                    .HasForeignKey(d => d.intConditionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Rule_Condition_tblAccess_Condition");
            });

            modelBuilder.Entity<tblAccess_Rule_Detail>(entity =>
            {
                entity.HasKey(e => e.intRegraDetalheId)
                    .HasName("PK__tblAcces__A0F95EE43936ED82");

                entity.Property(e => e.dteUltimaAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblAccess_Rule_Menu>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasOne(d => d.intMenu)
                    .WithMany(p => p.tblAccess_Rule_Menu)
                    .HasForeignKey(d => d.intMenuID)
                    .HasConstraintName("FK_tblAccess_Rule_Menu_tblAccess_Menu");
            });

            modelBuilder.Entity<tblAccess_Screen>(entity =>
            {
                entity.HasKey(e => e.intScreenID);

                entity.Property(e => e.txtScreenName).HasMaxLength(50);

                entity.HasOne(d => d.intApplication)
                    .WithMany(p => p.tblAccess_Screen)
                    .HasForeignKey(d => d.intApplicationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Screen_tblAccess_Application");

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblAccess_Screen)
                    .HasForeignKey(d => d.intProductGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Screen_tblProductGroups1");
            });

            modelBuilder.Entity<tblAccess_Screen_Action>(entity =>
            {
                entity.HasKey(e => e.intScreenActionID);

                entity.HasOne(d => d.intAction)
                    .WithMany(p => p.tblAccess_Screen_Action)
                    .HasForeignKey(d => d.intActionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Screen_Action_tblAccess_Action");

                entity.HasOne(d => d.intScreen)
                    .WithMany(p => p.tblAccess_Screen_Action)
                    .HasForeignKey(d => d.intScreenID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Screen_Action_tblAccess_Screen");
            });

            modelBuilder.Entity<tblAccess_YearType>(entity =>
            {
                entity.HasKey(e => e.intYearTypeID);

                entity.Property(e => e.dteEnd).HasColumnType("datetime");

                entity.Property(e => e.dteStart).HasColumnType("datetime");

                entity.Property(e => e.txtValue)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblAccess_YearTypeYear>(entity =>
            {
                entity.HasKey(e => e.intAno_TipoAnoID);

                entity.Property(e => e.intAno_TipoAnoID).ValueGeneratedNever();

                entity.HasOne(d => d.intTipoAno)
                    .WithMany(p => p.tblAccess_YearTypeYear)
                    .HasForeignKey(d => d.intTipoAnoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_YearTypeYear_tblAccess_YearTypeYear");
            });

            modelBuilder.Entity<tblAccess_Year_Type>(entity =>
            {
                entity.HasKey(e => e.intTipoAnoId)
                    .HasName("PK__tblAcces__2FD7726007013EC7");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAccountData>(entity =>
            {
                entity.HasKey(e => e.intAccountingEntryID);

                entity.HasIndex(e => e.intDocumentID)
                    .HasName("_dta_index_tblAccountData_5_518397016__K7");

                entity.HasIndex(e => e.intSequence)
                    .HasName("_dta_index_tblAccountData_5_518397016__K16");

                entity.HasIndex(e => e.intStatusID)
                    .HasName("_dta_index_tblAccountData_5_518397016__K6");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K1_K7");

                entity.HasIndex(e => new { e.intDebitAccountID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K3_K6");

                entity.HasIndex(e => new { e.intDocumentID, e.intAccountingEntryID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_K1");

                entity.HasIndex(e => new { e.intDocumentID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_K6");

                entity.HasIndex(e => new { e.intOrderID, e.intSequence })
                    .HasName("_dta_stat_518397016_13_16");

                entity.HasIndex(e => new { e.intOrderID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K6");

                entity.HasIndex(e => new { e.intSequence, e.intStatusID })
                    .HasName("_dta_stat_518397016_16_6");

                entity.HasIndex(e => new { e.intStatusID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K3");

                entity.HasIndex(e => new { e.intStatusID, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K7");

                entity.HasIndex(e => new { e.intStatusID, e.intOrderID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K13");

                entity.HasIndex(e => new { e.dblValue, e.intDocumentID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_K6_5");

                entity.HasIndex(e => new { e.dblValue, e.intOrderID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_5_13");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K7_5");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intCreditAccountID, e.intPaymentOptionID })
                    .HasName("_dta_stat_518397016_1_2_14");

                entity.HasIndex(e => new { e.intDebitAccountID, e.intSequence, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K3_K16_K6");

                entity.HasIndex(e => new { e.intOrderID, e.intSequence, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K16_K6");

                entity.HasIndex(e => new { e.intOrderID, e.intStatusID, e.txtComment })
                    .HasName("_net_IX_tblAccountData_intStatusIDxtComment");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intDocumentID, e.intAccountingEntryID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K14_K7_K1");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intAccountingEntryID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K1_14_15");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_14_15");

                entity.HasIndex(e => new { e.intSequence, e.intStatusID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K16_K6_K3");

                entity.HasIndex(e => new { e.intSequence, e.intStatusID, e.intOrderID })
                    .HasName("_dta_stat_518397016_16_6_13");

                entity.HasIndex(e => new { e.intStatusID, e.dblValue, e.intDocumentID })
                    .HasName("_net_IX_tblAccountData_intStatusID_dblValue_intDocumentID");

                entity.HasIndex(e => new { e.intStatusID, e.dteDate, e.intOrderID })
                    .HasName("_dta_stat_518397016_6_4_13");

                entity.HasIndex(e => new { e.dblValue, e.intDebitAccountID, e.intOrderID, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K3_K13_K6_5");

                entity.HasIndex(e => new { e.dblValue, e.intDebitAccountID, e.intStatusID, e.intOrderID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K3_K6_K13_5");

                entity.HasIndex(e => new { e.dblValue, e.intOrderID, e.intStatusID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K6_K3_5");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.intDebitAccountID, e.intOrderID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K3_K13_5");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.intOrderID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K13_K3_5");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_1_14_15");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intSequence, e.intCreditAccountID, e.intPaymentOptionID })
                    .HasName("_dta_stat_518397016_1_16_2_14");

                entity.HasIndex(e => new { e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intAccountingEntryID })
                    .HasName("_dta_stat_518397016_2_14_15_1");

                entity.HasIndex(e => new { e.intDebitAccountID, e.intOrderID, e.intSequence, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K16_K6_3_13");

                entity.HasIndex(e => new { e.intDebitAccountID, e.intSequence, e.intStatusID, e.intOrderID })
                    .HasName("_dta_stat_518397016_3_16_6_13");

                entity.HasIndex(e => new { e.intDocumentID, e.intOrderID, e.dblValue, e.intSequence })
                    .HasName("_net_IX_tblAccountData_intOrderID_dblValue_intSequence_intDocumentID");

                entity.HasIndex(e => new { e.intOrderID, e.intDebitAccountID, e.intStatusID, e.intSequence })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K3_K6_K16");

                entity.HasIndex(e => new { e.intOrderID, e.intSequence, e.intStatusID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K16_K6_K3");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intAccountingEntryID, e.intDocumentID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K1_K7_14_15");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intDocumentID, e.intAccountingEntryID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K7_K1_14_15");

                entity.HasIndex(e => new { e.intSequence, e.intStatusID, e.intDebitAccountID, e.intOrderID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K16_K6_K3_K13");

                entity.HasIndex(e => new { e.intSequence, e.intStatusID, e.intOrderID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K16_K6_K13_K3");

                entity.HasIndex(e => new { e.intStatusID, e.intAccountingEntryID, e.dblValue, e.intDocumentID })
                    .HasName("_net_IX_tblAccountData_documentID");

                entity.HasIndex(e => new { e.intStatusID, e.intDebitAccountID, e.intOrderID, e.dteDate })
                    .HasName("_dta_stat_518397016_6_3_13_4");

                entity.HasIndex(e => new { e.intStatusID, e.intOrderID, e.intDebitAccountID, e.intSequence })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K13_K3_K16");

                entity.HasIndex(e => new { e.intStatusID, e.intSequence, e.intOrderID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K16_K13_K3");

                entity.HasIndex(e => new { e.txtComment, e.intOrderID, e.intPaymentOptionID, e.intStatusID })
                    .HasName("_net_IX_tblAccountData_ntOrderID_intPaymentOptionID");

                entity.HasIndex(e => new { e.dblValue, e.intDebitAccountID, e.intStatusID, e.intOrderID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K3_K6_K13_K4_5");

                entity.HasIndex(e => new { e.dblValue, e.intOrderID, e.intDebitAccountID, e.intStatusID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K3_K6_K4_5");

                entity.HasIndex(e => new { e.dblValue, e.intOrderID, e.intStatusID, e.intDebitAccountID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K6_K3_K4_5");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.dteDate, e.intOrderID, e.intDebitAccountID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K4_K13_K3_5");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.intDebitAccountID, e.intOrderID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K3_K13_K4_5");

                entity.HasIndex(e => new { e.dblValue, e.intStatusID, e.intOrderID, e.intDebitAccountID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K13_K3_K4_5");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intSequence, e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec })
                    .HasName("_dta_index_tblAccountData_5_518397016__K1_K16_K2_K14_K15");

                entity.HasIndex(e => new { e.intDebitAccountID, e.dblValue, e.intOrderID, e.dteDate, e.intStatusID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K4_K6_3_5_13");

                entity.HasIndex(e => new { e.intSequence, e.intAccountingEntryID, e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec })
                    .HasName("_dta_index_tblAccountData_5_518397016__K16_K1_K2_K14_K15");

                entity.HasIndex(e => new { e.intSequence, e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intAccountingEntryID })
                    .HasName("_dta_stat_518397016_16_2_14_15_1");

                entity.HasIndex(e => new { e.intStatusID, e.dblValue, e.intDebitAccountID, e.intPaymentOptionID, e.intOrderID })
                    .HasName("_net_IX_tblAccountData_intStatusID_dblValue");

                entity.HasIndex(e => new { e.dblValue, e.intDocumentID, e.intOrderID, e.intDebitAccountID, e.intStatusID, e.intSequence })
                    .HasName("_dta_index_tblAccountData_5_518397016__K13_K3_K6_K16_5_7");

                entity.HasIndex(e => new { e.intDocumentID, e.txtComment, e.intOrderID, e.intSequence, e.dteDate, e.dblValue })
                    .HasName("_net_IX_tblAccountData_intSequence");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.dteDate, e.intStatusID, e.intDocumentID, e.dblValue, e.txtComment })
                    .HasName("_net_IX_tblAccountData_teste1");

                entity.HasIndex(e => new { e.intDebitAccountID, e.intOrderID, e.intDocumentID, e.intAccountingEntryID, e.dblValue, e.intStatusID, e.dteDate })
                    .HasName("tblAccountData_intStatusID_dteDate");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intAccountingEntryID, e.intDocumentID, e.intStatusID, e.intPaymentOptionID, e.dblValue, e.intSequence })
                    .HasName("_net_IX_tblAccountData_intStatusID_intPaymentOptionID");

                entity.HasIndex(e => new { e.dteDate, e.intStatusID, e.intOrderID, e.dblValue, e.intAccountingEntryID, e.intDocumentID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intSequence })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K13_K5_K1_K7");

                entity.HasIndex(e => new { e.txtComment, e.intSequence, e.intStatusID, e.intDebitAccountID, e.intAccountingEntryID, e.dblValue, e.intPaymentOptionID, e.intOrderID, e.dteDate })
                    .HasName("_dta_index_tblAccountData_5_518397016__K6_K3_K1_K5_K14_K13_K4_8_16");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.dblValue, e.dteDate, e.intPaymentOptionID, e.txtComment, e.intSequence, e.intCreditAccountID, e.intDocumentID, e.intOrderID, e.intDebitAccountID, e.intStatusID })
                    .HasName("IX_tblAccountData_OVValueDebitStatus");

                entity.HasIndex(e => new { e.intDebitAccountID, e.dteDate, e.dblValue, e.intStatusID, e.intDocumentID, e.txtComment, e.intOrderID, e.intAccountingEntryID, e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec })
                    .HasName("_dta_index_tblAccountData_5_518397016__K1_K2_K14_K15_3_4_5_6_7_8_13");

                entity.HasIndex(e => new { e.intDebitAccountID, e.dteDate, e.dblValue, e.intStatusID, e.intDocumentID, e.txtComment, e.intOrderID, e.intCreditAccountID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intAccountingEntryID })
                    .HasName("_dta_index_tblAccountData_5_518397016__K2_K14_K15_K1_3_4_5_6_7_8_13");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intCreditAccountID, e.dteDate, e.intDocumentID, e.txtComment, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intOrderID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intDebitAccountID, e.intStatusID, e.dblValue })
                    .HasName("_net_IX_tblaccountdata_intDebitAccountID");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intCreditAccountID, e.dteDate, e.intOrderID, e.intPaymentOptionID, e.intDebitAccountID, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intPaymentOptionIDSec, e.intSequence, e.dblValue, e.intStatusID, e.txtComment, e.intDocumentID })
                    .HasName("_net_IX_tblAccountData_dblValue_intStatusID_2");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intCreditAccountID, e.intDebitAccountID, e.dteDate, e.intDocumentID, e.txtComment, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intOrderID, e.intPaymentOptionIDSec, e.intSequence, e.intStatusID, e.dblValue, e.intPaymentOptionID })
                    .HasName("_net_IX_tblAccountData_intAccountingEntryID_intCreditAccountID");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intCreditAccountID, e.intDebitAccountID, e.intStatusID, e.intDocumentID, e.txtComment, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intOrderID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intSequence, e.dteDate, e.dblValue })
                    .HasName("_net_IX_tblAccountData_dteDate_dblValue");

                entity.HasIndex(e => new { e.intAccountingEntryID, e.intDebitAccountID, e.dteDate, e.intStatusID, e.intDocumentID, e.txtComment, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intOrderID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intSequence, e.intCreditAccountID, e.dblValue })
                    .HasName("_net_IX_tblAccountData_intCreditAccountID");

                entity.HasIndex(e => new { e.intDocumentID, e.intAccountingEntryID, e.intCreditAccountID, e.intDebitAccountID, e.dteDate, e.intStatusID, e.txtComment, e.dblFine, e.dblInterest, e.dblDiscount, e.intStoreID, e.intOrderID, e.intPaymentOptionID, e.intPaymentOptionIDSec, e.intSequence, e.dblValue })
                    .HasName("_net_IX_tblAccountData_intSequence_dblValue");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.intPaymentOptionID).HasDefaultValueSql("((1))");

                entity.Property(e => e.intSequence).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtComment).HasMaxLength(150);

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblAccountData)
                    .HasForeignKey(d => d.intStoreID)
                    .HasConstraintName("FK_tblAccountData_tblStores");
            });

            modelBuilder.Entity<tblAcess_Object_Validity>(entity =>
            {
                entity.HasKey(e => e.intValidityId)
                    .HasName("PK__tblAcess__1E3CF4C87D585111");

                entity.Property(e => e.dteFim).HasColumnType("datetime");

                entity.Property(e => e.dteInicio).HasColumnType("datetime");

                entity.HasOne(d => d.intObject)
                    .WithMany(p => p.tblAcess_Object_Validity)
                    .HasForeignKey(d => d.intObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAcess___intOb__7F409983");
            });

            modelBuilder.Entity<tblAdaptaMedAulaIndice>(entity =>
            {
                entity.HasKey(e => e.intAdaptaMedIndiceId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblAdaptaMedAulaTemaProfessorAssistido>(entity =>
            {
                entity.HasKey(e => e.intTemaProfessorAssistidoID);

                entity.HasIndex(e => new { e.intProfessorID, e.intLessonTitleID, e.intClientID })
                    .HasName("IX_tblAdaptaMedAulaTemaProfessorAssistido")
                    .IsUnique();
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideo>(entity =>
            {
                entity.HasKey(e => e.intAdaptaMedVideoId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideoAprovacao>(entity =>
            {
                entity.HasKey(e => e.intId);
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideoAprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideoCorrigido>(entity =>
            {
                entity.HasKey(e => e.intVideoCorrigidoId);

                entity.Property(e => e.intVideoCorrigidoId).ValueGeneratedNever();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblAdapt__6DAE9E967BCF1B10");

                entity.Property(e => e.dteLastUpdate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblAdaptaMedAulaVideoRelatorioReprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblAdapt__11B678D277FE8A2C");

                entity.Property(e => e.dteReprovacao).HasColumnType("datetime");

                entity.Property(e => e.txtApostilaSigla)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtTema)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtVideoTitulo)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAddressTypes>(entity =>
            {
                entity.HasKey(e => e.intAddressTypeID);

                entity.Property(e => e.intAddressTypeID).ValueGeneratedNever();

                entity.Property(e => e.txtAddressTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblAlunosAnoAtualMaisAnterior>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<tblAnexo>(entity =>
            {
                entity.HasKey(e => e.intAnexoId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtLink)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblAnexo)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAnexo_tblEmployees");
            });

            modelBuilder.Entity<tblAnoInscricao>(entity =>
            {
                entity.HasKey(e => e.intAnoInscricaoId)
                    .HasName("PK__tblAnoIn__9A8EFCC2A29DFAB0");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.intApplication)
                    .WithMany(p => p.tblAnoInscricao)
                    .HasForeignKey(d => d.intApplicationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAnoIns__intAp__3F3C7EB9");
            });

            modelBuilder.Entity<tblApi_MensagemInadimplencia_Log>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.intClientID)
                    .HasName("_net_IX_tblApi_MensagemInadimplencia_Log_intClientID");

                entity.Property(e => e.dteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblApi_MensagemInadimplencia_Log)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblApi_MensagemInadimplencia_Log_tblPersons");
            });

            modelBuilder.Entity<tblApostilaAddOn>(entity =>
            {
                entity.HasKey(e => e.intApostilaAddOnID)
                    .HasName("PK_tblApostilaAddOn_1");

                entity.Property(e => e.txtConteudo).IsRequired();

                entity.Property(e => e.txtPosicao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblApostilaAddOn)
                    .HasForeignKey(d => d.intApostilaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblApostilaAddOn_tblBooks");
            });

            modelBuilder.Entity<tblApostilasUnificadas>(entity =>
            {
                entity.HasKey(e => e.intUnificacaoID);
            });

            modelBuilder.Entity<tblApplication_AcessDenied>(entity =>
            {
                entity.HasKey(e => e.intApplicationAcessDeniedId);

                entity.Property(e => e.txtMotivoDesbloqueio).IsUnicode(false);

                entity.Property(e => e.txtReason).IsUnicode(false);
            });

            modelBuilder.Entity<tblAtualizacaoErrata_Imagens>(entity =>
            {
                entity.HasKey(e => e.intAtualizacaoErrataID);

                entity.Property(e => e.dteCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtFileName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtPath)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAvaliacaoAluno>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblAvali__11B679325F624ECD");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDataAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.txtComentario)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.txtPlataforma)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.txtVersaoApp)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.txtVersaoPlataforma)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAvaliacaoConteudoQuestao>(entity =>
            {
                entity.HasKey(e => e.intAvaliacaoId);

                entity.HasIndex(e => e.intClientId)
                    .HasName("_net_IX_tblAvaliacaoConteudoQuestao_intClientId");

                entity.HasIndex(e => new { e.intQuestaoId, e.intTipoExercicioId, e.intClientId, e.intTipoComentario })
                    .HasName("_net_IX_tblAvaliacaoConteudoQuestao_intQuestaoId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtComentarioAvaliacao)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.HasOne(d => d.intAlternativaReprovaNavigation)
                    .WithMany(p => p.tblAvaliacaoConteudoQuestao)
                    .HasForeignKey(d => d.intAlternativaReprova)
                    .HasConstraintName("FK_tblAvaliacaoConteudoQuestaoAlternativas_AlternativaID");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblAvaliacaoConteudoQuestao)
                    .HasForeignKey(d => d.intClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAvaliacaoConteudoQuestao_tblPersons");
            });

            modelBuilder.Entity<tblAvaliacaoConteudoQuestaoAlternativas>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblAvali__11B6793207795C49");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAvatar>(entity =>
            {
                entity.HasKey(e => e.intAvatarID)
                    .HasName("PK_Avatar");

                entity.HasIndex(e => new { e.bitActive, e.intCategoryID })
                    .HasName("_net_IX_tblAvatar_bitActive_intCategoryID");

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtAvatarPath)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.HasOne(d => d.intAvatarType)
                    .WithMany(p => p.tblAvatar)
                    .HasForeignKey(d => d.intAvatarTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAvatar_tblAvatarTypes");

                entity.HasOne(d => d.intCategory)
                    .WithMany(p => p.tblAvatar)
                    .HasForeignKey(d => d.intCategoryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAvatar_tblAvatar_Category");
            });

            modelBuilder.Entity<tblAvatar_Category>(entity =>
            {
                entity.HasKey(e => e.intAvatarCategoryID);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblAvatar_Types>(entity =>
            {
                entity.HasKey(e => e.intAvatarTypeID);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblAvisos>(entity =>
            {
                entity.HasKey(e => e.intAvisoID);

                entity.Property(e => e.txtAviso).HasColumnType("text");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtTitulo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblAvisos_Chamados>(entity =>
            {
                entity.HasKey(e => e.intAvisoChamado)
                    .HasName("PK__tblAviso__D5D858D913F1F0E8");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intAviso)
                    .WithMany(p => p.tblAvisos_Chamados)
                    .HasForeignKey(d => d.intAvisoId)
                    .HasConstraintName("fk_tblAvisos_Chamados_tblAvisos");
            });

            modelBuilder.Entity<tblBankAccounts>(entity =>
            {
                entity.HasKey(e => e.intBankAccountID);

                entity.HasIndex(e => new { e.intBankID, e.txtAgency, e.txtAccount, e.txtCarteira })
                    .HasName("IX_tblBankAccounts")
                    .IsUnique();

                entity.Property(e => e.intBankAccountID).ValueGeneratedNever();

                entity.Property(e => e.strChaveEstabelecimento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.strIdEstabelecimento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtAccount)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.txtAccountDigit).HasMaxLength(3);

                entity.Property(e => e.txtAgency)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.txtCarteira).HasMaxLength(50);

                entity.Property(e => e.txtInstrucoes)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtNossoNumero).HasMaxLength(50);

                entity.Property(e => e.txtNossoNumeroFim).HasMaxLength(50);

                entity.Property(e => e.txtTipoConta).HasMaxLength(1);

                entity.HasOne(d => d.intBank)
                    .WithMany(p => p.tblBankAccounts)
                    .HasForeignKey(d => d.intBankID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankAccounts_tblBanks");

                entity.HasOne(d => d.intCompany)
                    .WithMany(p => p.tblBankAccounts)
                    .HasForeignKey(d => d.intCompanyID)
                    .HasConstraintName("FK_tblBankAccounts_tblCompanies");
            });

            modelBuilder.Entity<tblBanks>(entity =>
            {
                entity.HasKey(e => e.intBankID);

                entity.Property(e => e.txtCode).HasMaxLength(10);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblBanners>(entity =>
            {
                entity.HasKey(e => e.intBannerId)
                    .HasName("PK_Banner");

                entity.Property(e => e.txtClickAqui)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtImagem)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtLink)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.intObject)
                    .WithMany(p => p.tblBanners)
                    .HasForeignKey(d => d.intObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BannerObject");
            });

            modelBuilder.Entity<tblBdNuvemLog>(entity =>
            {
                entity.HasKey(e => e.intBdNuvemLogId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblBlackList_Anexo>(entity =>
            {
                entity.HasKey(e => e.intBlackListAnexoID)
                    .HasName("PK__tblBlack__29115CB5356DAC19");

                entity.Property(e => e.dteDateTimeAlteracao).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtAnexoDossie)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBlackList_Categoria>(entity =>
            {
                entity.HasKey(e => e.intCategoriaID)
                    .HasName("PK__tblBlack__A14104200359B0AB");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBlackWords_DuvidasAcademicas>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtBlackWordDA).IsUnicode(false);
            });

            modelBuilder.Entity<tblBlacklistAprovacoes_Bloqueios>(entity =>
            {
                entity.HasKey(e => e.intBlacklistAprovacoesBloqueiosID)
                    .HasName("PK__tblBlack__18E312E70AC0FB77");

                entity.Property(e => e.dteInclusaoBloqueio).HasColumnType("datetime");

                entity.Property(e => e.txtMotivo)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBlacklist_Log>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblBlack__11B678D27E5F0495");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtEmail).IsUnicode(false);

                entity.Property(e => e.txtFaculdade).IsUnicode(false);

                entity.Property(e => e.txtMotivo).IsUnicode(false);

                entity.Property(e => e.txtNome).IsUnicode(false);

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBlacklist_Usuarios>(entity =>
            {
                entity.HasKey(e => e.intBlackListPessoasID)
                    .HasName("PK_Blacklist_Usuarios");

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBloqueioArea>(entity =>
            {
                entity.HasKey(e => e.intBloqueioAreaId);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBloqueioConcurso>(entity =>
            {
                entity.HasKey(e => e.intBloqueioConcursoId);

                entity.HasIndex(e => new { e.intProvaId, e.intBloqueioAreaId })
                    .HasName("UQ__tblBloqu__CC210E2B331DBDE5")
                    .IsUnique();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.intBloqueioArea)
                    .WithMany(p => p.tblBloqueioConcurso)
                    .HasForeignKey(d => d.intBloqueioAreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBloqueioConcurso_tblBloqueioArea");

                entity.HasOne(d => d.intProva)
                    .WithMany(p => p.tblBloqueioConcurso)
                    .HasForeignKey(d => d.intProvaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBloqueioConcurso_tblConcurso_Provas");
            });

            modelBuilder.Entity<tblBook_Imagens>(entity =>
            {
                entity.HasKey(e => e.intBookImagemID);

                entity.Property(e => e.bitAtivo).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.Property(e => e.txtFileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrl).HasMaxLength(500);

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblBook_Imagens)
                    .HasForeignKey(d => d.intBookID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBook_Imagens_tblBooks");
            });

            modelBuilder.Entity<tblBooks>(entity =>
            {
                entity.HasKey(e => e.intBookID);

                entity.HasIndex(e => e.bitImprimeAtualizacoes)
                    .HasName("_dta_index_tblBooks_5_631217549__K22");

                entity.HasIndex(e => e.intBookEntityID)
                    .HasName("_dta_index_tblBooks_5_631217549__K28");

                entity.HasIndex(e => e.intBookID)
                    .HasName("_dta_index_tblBooks_5_631217549__K1");

                entity.HasIndex(e => e.intYear)
                    .HasName("_dta_index_tblBooks_5_631217549__K20");

                entity.HasIndex(e => new { e.bitImprimeAtualizacoes, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K22_K1");

                entity.HasIndex(e => new { e.intBookEntityID, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K28_K1");

                entity.HasIndex(e => new { e.intBookEntityID, e.txtGenericFile })
                    .HasName("_dta_stat_631217549_28_30");

                entity.HasIndex(e => new { e.intBookID, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K28_1");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3");

                entity.HasIndex(e => new { e.intBookID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_1");

                entity.HasIndex(e => new { e.intSubjectID, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_3");

                entity.HasIndex(e => new { e.intSubjectID, e.intYear })
                    .HasName("_net_IX_tblBooks_intSubjectID_intYear");

                entity.HasIndex(e => new { e.intYear, e.intBookID })
                    .HasName("_dta_stat_631217549_20_1");

                entity.HasIndex(e => new { e.txtArchiveLayer, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_13");

                entity.HasIndex(e => new { e.bitImprimeAtualizacoes, e.intBookID, e.intBookEntityID })
                    .HasName("_dta_stat_631217549_22_1_28");

                entity.HasIndex(e => new { e.intBookID, e.intBookEntityID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K28_K20_1");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_stat_631217549_1_3_12");

                entity.HasIndex(e => new { e.intBookID, e.intYear, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K20_K28");

                entity.HasIndex(e => new { e.intVolume, e.intSubjectID, e.intYear })
                    .HasName("_dta_stat_631217549_12_3_20");

                entity.HasIndex(e => new { e.intYear, e.intBookEntityID, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K28_K1");

                entity.HasIndex(e => new { e.intYear, e.intBookID, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K1_K28");

                entity.HasIndex(e => new { e.txtArchiveLayer, e.intYear, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_13_20");

                entity.HasIndex(e => new { e.txtGenericFile, e.txtGenericThumbnail, e.intBookEntityID })
                    .HasName("_dta_stat_631217549_30_31_28");

                entity.HasIndex(e => new { e.intBookEntityID, e.intBookID, e.txtGenericFile, e.txtGenericThumbnail })
                    .HasName("_dta_stat_631217549_28_1_30_31");

                entity.HasIndex(e => new { e.intBookID, e.intBookEntityID, e.txtGenericFile, e.txtGenericThumbnail })
                    .HasName("_dta_index_tblBooks_5_631217549__K28_K30_K31_1");

                entity.HasIndex(e => new { e.intBookID, e.intVolume, e.intYear, e.txtArchiveLayer })
                    .HasName("_net_IX_tblBooks_txtArchiveLayer");

                entity.HasIndex(e => new { e.intBookID, e.txtArchiveLayer, e.intYear, e.intBookEntityID })
                    .HasName("_net_IX_tblBooks_intBookEntityID");

                entity.HasIndex(e => new { e.intBookID, e.txtGenericFile, e.txtGenericThumbnail, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K30_K31_K28_1");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.intVolume, e.intSubjectID })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_1_11");

                entity.HasIndex(e => new { e.intVolume, e.intSubjectID, e.intBookEntityID, e.intYear })
                    .HasName("_dta_stat_631217549_12_3_28_20");

                entity.HasIndex(e => new { e.intVolume, e.intSubjectID, e.intBookID, e.intYear })
                    .HasName("_dta_stat_631217549_12_3_1_20");

                entity.HasIndex(e => new { e.intVolume, e.intSubjectID, e.txtArchiveUpdateSwf, e.intYear })
                    .HasName("_dta_stat_631217549_12_3_15_20");

                entity.HasIndex(e => new { e.txtArchiveLayer, e.intBookID, e.intBookEntityID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K28_K20_13");

                entity.HasIndex(e => new { e.txtArchiveLayer, e.intBookID, e.intYear, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K20_K28_13");

                entity.HasIndex(e => new { e.txtArchiveLayer, e.intYear, e.intBookEntityID, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K28_K1_13");

                entity.HasIndex(e => new { e.txtFullContentSwf, e.intBookID, e.intBookEntityID, e.intYear })
                    .HasName("_net_IX_tblBooks_intBookid");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intSubjectID, e.intBookID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K3_K1_K12_11");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID, e.intYear, e.txtArchiveBonus, e.intVolume })
                    .HasName("_dta_stat_631217549_1_3_20_17_12");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID, e.intYear, e.txtArchiveUpdateSwf, e.intVolume })
                    .HasName("_dta_stat_631217549_1_3_20_15_12");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.intVolume, e.intSubjectID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K20_1_11");

                entity.HasIndex(e => new { e.intVolume, e.intSubjectID, e.intBookID, e.intYear, e.intBookEntityID })
                    .HasName("_dta_stat_631217549_12_3_1_20_28");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intSubjectID, e.intYear, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3_K20_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intYear, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K20_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intVolume, e.intSubjectID, e.intBookID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K1_K20_11");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID, e.txtTitle, e.intVolume, e.intYear, e.txtArchiveUpdateSwf })
                    .HasName("_net_IX_tblBooks_intYear_txtArchiveUpdateSwf");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.intVolume, e.intSubjectID, e.intYear, e.txtArchiveUpdateSwf })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K20_K15_1_11");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.intVolume, e.intSubjectID, e.txtArchiveUpdateSwf, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K15_K20_1_11");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.txtArchiveBonus, e.intVolume, e.intSubjectID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K20_1_11_17");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.txtArchiveUpdateSwf, e.intYear, e.intVolume, e.intSubjectID })
                    .HasName("_dta_index_tblBooks_5_631217549__K15_K20_K12_K3_1_11");

                entity.HasIndex(e => new { e.intYear, e.intBookEntityID, e.txtArquiveGabaritoSwf, e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_stat_631217549_20_28_21_1_3_12");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intSubjectID, e.intYear, e.txtArchiveBonus, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3_K20_K17_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intSubjectID, e.intYear, e.txtArchiveUpdateSwf, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3_K20_K15_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intYear, e.txtArchiveBonus, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K20_K17_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intYear, e.txtArchiveBonus, e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K17_K1_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intYear, e.txtArchiveBonus, e.intSubjectID, e.intBookID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K17_K3_K1_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intYear, e.txtArchiveUpdateSwf, e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K15_K1_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.txtArchiveBonus, e.intVolume, e.intSubjectID, e.intBookID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K1_K20_11_17");

                entity.HasIndex(e => new { e.txtTitle, e.txtArquiveGabaritoSwf, e.intVolume, e.intSubjectID, e.intBookID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K1_K20_11_21");

                entity.HasIndex(e => new { e.intBookID, e.intSubjectID, e.txtTitle, e.intVolume, e.txtArquiveGabaritoSwf, e.intYear, e.intBookEntityID })
                    .HasName("_net_IX_tblBooks_intYear_intBookEntityID");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.intBookEntityID, e.intSubjectID, e.intVolume, e.intYear, e.txtArchiveBonus })
                    .HasName("_net_IX_tblBooks_intYear");

                entity.HasIndex(e => new { e.intBookID, e.txtTitle, e.txtArquiveGabaritoSwf, e.intVolume, e.intSubjectID, e.intBookEntityID, e.intYear })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K28_K20_1_11_21");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intSubjectID, e.intYear, e.intBookEntityID, e.txtArquiveGabaritoSwf, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K3_K20_K28_K21_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intBookID, e.intYear, e.intBookEntityID, e.txtArquiveGabaritoSwf, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_K20_K28_K21_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.intYear, e.intBookEntityID, e.txtArquiveGabaritoSwf, e.intBookID, e.intSubjectID, e.intVolume })
                    .HasName("_dta_index_tblBooks_5_631217549__K20_K28_K21_K1_K3_K12_11");

                entity.HasIndex(e => new { e.txtTitle, e.txtArquiveGabaritoSwf, e.intVolume, e.intSubjectID, e.intBookID, e.intYear, e.intBookEntityID })
                    .HasName("_dta_index_tblBooks_5_631217549__K12_K3_K1_K20_K28_11_21");

                entity.HasIndex(e => new { e.intBookID, e.intPages, e.intSubjectID, e.intQuestionNumber, e.dblWeight, e.dtePublication, e.txtPath, e.intMaterialID, e.intEspecialidadeAreaID, e.intEspecialidadeID, e.txtTitle, e.intVolume, e.txtArchiveLayer, e.txtArchiveIndice, e.txtArchiveUpdateSwf, e.txtArchiveUpdate, e.txtArchiveBonus, e.txtArchiveGabarito, e.intLessonTitleID, e.txtArquiveGabaritoSwf, e.bitImprimeAtualizacoes, e.bitImprimeBonus, e.bitImprimeGabaritos, e.bitImprimeIndice, e.txtFullContentSwf, e.bitVideoIntroLIberado, e.intBookEntityID, e.dteAlteracaoUpadteSwf, e.txtGenericFile, e.txtGenericThumbnail, e.intVirtualPages, e.intYear })
                    .HasName("IX_tblBooks_intYear_22DDF");

                entity.HasIndex(e => new { e.intPages, e.intSubjectID, e.intQuestionNumber, e.dblWeight, e.dtePublication, e.txtPath, e.intMaterialID, e.intEspecialidadeAreaID, e.intEspecialidadeID, e.txtTitle, e.intVolume, e.txtArchiveLayer, e.txtArchiveIndice, e.txtArchiveUpdateSwf, e.txtArchiveUpdate, e.txtArchiveBonus, e.txtArchiveGabarito, e.intLessonTitleID, e.intYear, e.txtArquiveGabaritoSwf, e.bitImprimeAtualizacoes, e.bitImprimeBonus, e.bitImprimeGabaritos, e.bitImprimeIndice, e.txtFullContentSwf, e.bitVideoIntroLIberado, e.intBookEntityID, e.dteAlteracaoUpadteSwf, e.txtGenericFile, e.txtGenericThumbnail, e.intVirtualPages, e.intBookID })
                    .HasName("_dta_index_tblBooks_5_631217549__K1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30_31_32");

                entity.Property(e => e.intBookID).ValueGeneratedNever();

                entity.Property(e => e.dteAlteracaoUpadteSwf).HasColumnType("datetime");

                entity.Property(e => e.dtePublication).HasColumnType("datetime");

                entity.Property(e => e.intLessonTitleID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtArchiveBonus).HasMaxLength(100);

                entity.Property(e => e.txtArchiveGabarito).HasMaxLength(100);

                entity.Property(e => e.txtArchiveIndice).HasMaxLength(100);

                entity.Property(e => e.txtArchiveLayer).HasMaxLength(100);

                entity.Property(e => e.txtArchiveUpdate).HasMaxLength(100);

                entity.Property(e => e.txtArchiveUpdateSwf).HasMaxLength(100);

                entity.Property(e => e.txtArquiveGabaritoSwf).HasMaxLength(100);

                entity.Property(e => e.txtFullContentSwf).HasMaxLength(100);

                entity.Property(e => e.txtGenericFile)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtGenericThumbnail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtPath).HasMaxLength(200);

                entity.Property(e => e.txtTitle).HasMaxLength(400);

                entity.HasOne(d => d.intBookEntity)
                    .WithMany(p => p.tblBooks)
                    .HasForeignKey(d => d.intBookEntityID)
                    .HasConstraintName("FK_tblBooks_tblBooks_Entities");

                entity.HasOne(d => d.intBook)
                    .WithOne(p => p.tblBooks)
                    .HasForeignKey<tblBooks>(d => d.intBookID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBooks_tblProducts");

                entity.HasOne(d => d.intEspecialidade)
                    .WithMany(p => p.tblBooks)
                    .HasForeignKey(d => d.intEspecialidadeID)
                    .HasConstraintName("FK_tblBooks_tblEspecialidades");
            });

            modelBuilder.Entity<tblBooksEntitiesProfessor>(entity =>
            {
                entity.HasKey(e => e.intBooksEntitiesProfessor);

                entity.Property(e => e.bitControlado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblBooksEntitiesProfessor)
                    .HasForeignKey(d => d.intContactId)
                    .HasConstraintName("FK_tblBooksEntitiesProfessor_tblPersons");
            });

            modelBuilder.Entity<tblBooks_Entities>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.txtName)
                    .HasName("UK_tblBooks_Entities_txtName")
                    .IsUnique();

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblBooks_Videos>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.txtVideoCode)
                    .HasName("_net_IX_tblBooks_Videos_txtVideoCode");

                entity.HasIndex(e => new { e.intMaterialID, e.txtVideoCode, e.txtVideoURL, e.txtExtension })
                    .HasName("_net_IX_tblBooks_Videos_txtExtension");

                entity.HasIndex(e => new { e.txtVideoURL, e.txtName, e.txtExtension, e.intMaterialID, e.txtVideoCode })
                    .HasName("_net_IX_tblBooks_Videos_intMaterialID");

                entity.Property(e => e.dteAutorizationDateTime).HasColumnType("datetime");

                entity.Property(e => e.dtePublishDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteUpdate).HasColumnType("datetime");

                entity.Property(e => e.txtExtension)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.txtName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.txtVideoCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtVideoURL).HasMaxLength(400);
            });

            modelBuilder.Entity<tblCallCenterCallsInadimplencia>(entity =>
            {
                entity.HasKey(e => e.intCallCenterInadimplenciaID);

                entity.HasIndex(e => e.intCallCenterCallsID)
                    .HasName("IX_tblCallCenterCallsInadimplencia");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallCenterCallsIDRef })
                    .HasName("_net_IX_tblCallCenterCallsInadimplencia_intCallCenterCallsIDRef");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intOrderID, e.intCallCenterCallsIDRef })
                    .HasName("IX_tblCallCenterCallsInadimplencia_1");

                entity.HasOne(d => d.intOrder)
                    .WithMany(p => p.tblCallCenterCallsInadimplencia)
                    .HasForeignKey(d => d.intOrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCallCenterCallsInadimplencia_tblSellOrders");
            });

            modelBuilder.Entity<tblCallCenterCallsInadimplenciaLog>(entity =>
            {
                entity.HasKey(e => e.intCallCenterCallsID);

                entity.Property(e => e.intCallCenterCallsID).ValueGeneratedNever();

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.HasOne(d => d.intAplicativo)
                    .WithMany(p => p.tblCallCenterCallsInadimplenciaLog)
                    .HasForeignKey(d => d.intAplicativoID)
                    .HasConstraintName("FK_tblCallCenterCallsInadimplenciaLog_tblAccess_Application");
            });

            modelBuilder.Entity<tblCallCenterCallsOv>(entity =>
            {
                entity.HasKey(e => e.intCallCenterCallsOvID);
            });

            modelBuilder.Entity<tblCallCenterCategory>(entity =>
            {
                entity.HasKey(e => e.intCallCategoryID)
                    .HasName("PK_tblCallCenterGroups");

                entity.HasIndex(e => new { e.txtDescription, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCategory_5_481436789__K1_2");

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblCallCenterCategory_Classification>(entity =>
            {
                entity.HasKey(e => e.intCategoryClassificationID);
            });

            modelBuilder.Entity<tblCallCenterEvents>(entity =>
            {
                entity.HasKey(e => new { e.intCallEventsID, e.intCallCenterCallsID });

                entity.HasIndex(e => e.intCallCenterCallsID)
                    .HasName("_net_IX_tblCallCenterEvents_intCallCenterCallsID");

                entity.HasIndex(e => e.intCallEventsID)
                    .HasName("IX_tblCallCenterEvents")
                    .IsUnique();

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteDate, e.intCallStatusID })
                    .HasName("_net_IX_tblCallCenterEvents_intCallStatusID");

                entity.HasIndex(e => new { e.intCallEventsID, e.intCallCenterCallsID, e.dteDate, e.bitInternalInformation, e.intCallStatusID, e.txtSubject, e.txtDetails, e.intEmployeeID, e.intSeverityID, e.txtFilename, e.intSectorComplementID, e.intSectorID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterEvents_intStatusInternoID");

                entity.Property(e => e.intCallEventsID).ValueGeneratedOnAdd();

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtDetails).HasMaxLength(4000);

                entity.Property(e => e.txtFilename)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.txtSubject)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblCallCenterEvents)
                    .HasForeignKey(d => d.intEmployeeID)
                    .HasConstraintName("FK_tblCallCenterEvents_tblEmployees");
            });

            modelBuilder.Entity<tblCargaHorariaCurso>(entity =>
            {
                entity.HasKey(e => e.intCargaHorariaCursoID)
                    .HasName("PK__tblCarga__4733F4BF098FB6DB");

                entity.HasIndex(e => new { e.intProductGroup1, e.txtMesAnoVigenciaInicio, e.txtMesAnoVigenciaFim })
                    .HasName("tblCargaHorariaCurso_Unique")
                    .IsUnique();

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtMesAnoVigenciaFim)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtMesAnoVigenciaInicio)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblCities>(entity =>
            {
                entity.HasKey(e => e.intCityID);

                entity.HasIndex(e => e.txtCodigoIBGE)
                    .HasName("_net_IX_tblCities_txtCodigoIBGE");

                entity.HasIndex(e => new { e.intCityID, e.intState, e.intAreaCode, e.bitCapital, e.txtCodigoIBGE, e.txtName })
                    .HasName("_net_IX_tblCities_txtName");

                entity.HasIndex(e => new { e.intCityID, e.txtName, e.intAreaCode, e.txtCodigoIBGE, e.intState, e.bitCapital })
                    .HasName("_net_IX_tblCities_intState");

                entity.HasIndex(e => new { e.intCityID, e.txtName, e.intState, e.intAreaCode, e.txtCodigoIBGE, e.bitCapital })
                    .HasName("_net_IX_tblCities_bitCapital");

                entity.Property(e => e.txtCodigoIBGE).HasMaxLength(7);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.intStateNavigation)
                    .WithMany(p => p.tblCities)
                    .HasForeignKey(d => d.intState)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCities_tblStates");
            });

            modelBuilder.Entity<tblClassRooms>(entity =>
            {
                entity.HasKey(e => e.intClassRoomID);

                entity.HasIndex(e => new { e.intClassRoomID, e.txtDescription })
                    .HasName("_net_IX_tblClassRooms_tblClassRooms");

                entity.Property(e => e.txtAddress1).HasMaxLength(100);

                entity.Property(e => e.txtAddress2).HasMaxLength(100);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(50);

                entity.Property(e => e.txtZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblClassRooms)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblClassRooms_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblClassRooms)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblClassRooms_tblCities");

                entity.HasOne(d => d.intCompany)
                    .WithMany(p => p.tblClassRooms)
                    .HasForeignKey(d => d.intCompanyID)
                    .HasConstraintName("FK_tblClassRooms_tblCompanies");
            });

            modelBuilder.Entity<tblClassificacaoTurmaConvidada>(entity =>
            {
                entity.HasKey(e => e.intClassificacaoTurmaConvidadaId)
                    .HasName("PK__tblClass__5BEB788BE2D5A79D");

                entity.HasOne(d => d.intAnoInscricao)
                    .WithMany(p => p.tblClassificacaoTurmaConvidada)
                    .HasForeignKey(d => d.intAnoInscricaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblClassi__intAn__636E8216");

                entity.HasOne(d => d.intClassification)
                    .WithMany(p => p.tblClassificacaoTurmaConvidada)
                    .HasForeignKey(d => d.intClassificationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblClassi__intCl__627A5DDD");
            });

            modelBuilder.Entity<tblClassificationAttributes>(entity =>
            {
                entity.HasKey(e => e.intAttributeID);

                entity.HasIndex(e => new { e.intAttributeID, e.txtDescription, e.intClassificationID })
                    .HasName("_net_IX_tblClassificationAttributes_intClassificationID");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblClassifications>(entity =>
            {
                entity.HasKey(e => e.intClassificationID);

                entity.Property(e => e.intActionPermissionID).HasComment("tblSysAction");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblCleanHtmlTags>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.txtTagFinal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtTagInicio)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblClientClassifications>(entity =>
            {
                entity.HasKey(e => new { e.intPersonID, e.intClassificationID, e.intAttributeID });

                entity.HasIndex(e => new { e.intPersonID, e.intAttributeID })
                    .HasName("_net_IX_tblClientClassifications_intAttributeID");

                entity.HasIndex(e => new { e.intPersonID, e.dteDate, e.intEmployeeID, e.intClassificationID, e.intAttributeID })
                    .HasName("_net_IX_tblClientClassifications_intClassificationID_intAttributeID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.intEmployeeID).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.intPerson)
                    .WithMany(p => p.tblClientClassifications)
                    .HasForeignKey(d => d.intPersonID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientClassifications_tblPersons");
            });

            modelBuilder.Entity<tblClients>(entity =>
            {
                entity.HasKey(e => e.intClientID)
                    .HasName("PK_tblClient");

                entity.HasIndex(e => e.intAccountID)
                    .HasName("IX_tblClientsAccount")
                    .IsUnique();

                entity.HasIndex(e => new { e.intClientID, e.intAccountID })
                    .HasName("_net_IX_tblClientsintClientID")
                    .IsUnique();

                entity.HasIndex(e => new { e.intClientID, e.intExpectedGraduationTermID })
                    .HasName("_net_IX_tblClients_intExpectedGraduationTermID");

                entity.HasIndex(e => new { e.intEspecialidadeID, e.intClientID })
                    .HasName("_dta_index_tblClients_5_1478296326__K1_5");

                entity.HasIndex(e => new { e.intClientID, e.intAccountID, e.intSchoolID })
                    .HasName("_net_IX_tblClients_intSchoolID");

                entity.HasIndex(e => new { e.txtSubscriptionCode, e.intAccountID, e.intClientStatusID, e.txtArea, e.intEspecialidadeID, e.intExpectedGraduationTermID, e.intSchoolID, e.intClientID })
                    .HasName("_net_IX_tblClients_intEspecialidadeID");

                entity.Property(e => e.intClientID).ValueGeneratedNever();

                entity.Property(e => e.txtArea).HasMaxLength(50);

                entity.Property(e => e.txtSubscriptionCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.intClient)
                    .WithOne(p => p.tblClients)
                    .HasForeignKey<tblClients>(d => d.intClientID)
                    .HasConstraintName("FK_tblClients_tblPersons");

                entity.HasOne(d => d.intEspecialidade)
                    .WithMany(p => p.tblClients)
                    .HasForeignKey(d => d.intEspecialidadeID)
                    .HasConstraintName("FK_tblClients_tblEspecialidades");

                entity.HasOne(d => d.intExpectedGraduationTerm)
                    .WithMany(p => p.tblClients)
                    .HasForeignKey(d => d.intExpectedGraduationTermID)
                    .HasConstraintName("FK_tblClients_tblExpectedGraduationTermCatalog");

                entity.HasOne(d => d.intSchool)
                    .WithMany(p => p.tblClients)
                    .HasForeignKey(d => d.intSchoolID)
                    .HasConstraintName("FK_tblClients_tblSchools");
            });

            modelBuilder.Entity<tblClientsDocuments>(entity =>
            {
                entity.HasKey(e => e.intDocumentID);

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intStatusID, e.intTypeID })
                    .HasName("_net_IX_tblClientsDocuments_intClientID_intStatusID");

                entity.HasIndex(e => new { e.intDocumentID, e.intClientID, e.txtDocument, e.intStatusID, e.intEmployeeID, e.bitExibirPainel, e.intTypeID, e.dteDate })
                    .HasName("_net_IX_tblClientsDocuments_intTypeID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtDocument)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblClientsDocuments)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientsDocuments_tblPersons");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblClientsDocuments)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientsDocuments_tblEmployees");

                entity.HasOne(d => d.intStatus)
                    .WithMany(p => p.tblClientsDocuments)
                    .HasForeignKey(d => d.intStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientsDocuments_tblClientsDocumentsStatus");

                entity.HasOne(d => d.intType)
                    .WithMany(p => p.tblClientsDocuments)
                    .HasForeignKey(d => d.intTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientsDocuments_tblClientsDocumentsTypes");
            });

            modelBuilder.Entity<tblClientsDocumentsStatus>(entity =>
            {
                entity.HasKey(e => e.intStatusID);

                entity.Property(e => e.txtDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<tblClientsDocumentsTypes>(entity =>
            {
                entity.HasKey(e => e.intTypeID);

                entity.Property(e => e.intTamanhoMaximo).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<tblClients_BlackList>(entity =>
            {
                entity.HasKey(e => e.intClientBlackListID);

                entity.Property(e => e.dteDateTimeInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtAnexoDossie)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtMessage).HasMaxLength(500);

                entity.Property(e => e.txtNome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtType)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasComment("E = emed | I = inscrição");

                entity.HasOne(d => d.intBlackListCategoria)
                    .WithMany(p => p.tblClients_BlackList)
                    .HasForeignKey(d => d.intBlackListCategoriaID)
                    .HasConstraintName("fk_BlackList_Categoria");
            });

            modelBuilder.Entity<tblClients_BlackListAprovacoes>(entity =>
            {
                entity.HasKey(e => e.intClientBlackListID)
                    .HasName("PK_intClientBlackListID");

                entity.Property(e => e.dteDateTimeInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtAnexoDossie)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtMessage).HasMaxLength(500);

                entity.Property(e => e.txtNome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtType)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblClients_BlackListMotivos>(entity =>
            {
                entity.HasKey(e => e.intClientBlackListID);

                entity.Property(e => e.dteDataMotivo).HasColumnType("datetime");

                entity.Property(e => e.txtMessage)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.txtRegister)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblClients_BlackListPre>(entity =>
            {
                entity.HasKey(e => e.intClientBlackListID)
                    .HasName("PK_intClientBlackListIDPre");

                entity.Property(e => e.txtEmail)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtFaculdade)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intSchool)
                    .WithMany(p => p.tblClients_BlackListPre)
                    .HasForeignKey(d => d.intSchoolID)
                    .HasConstraintName("ID_FK_intSchoolID");
            });

            modelBuilder.Entity<tblCodigoCaracteres>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.txtCodigoCaracter)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblCodigoComentario>(entity =>
            {
                entity.HasKey(e => e.IntID);

                entity.HasIndex(e => e.intQuestaoID)
                    .HasName("_net_IX_tblCodigoComentario_intQuestaoID");

                entity.HasIndex(e => new { e.intEmployeeID, e.dteDataGravacao, e.intApostilaID })
                    .HasName("_net_IX_tblCodigoComentario_01");

                entity.Property(e => e.Gabarito)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.GabaritoOriginal)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.dteDataEdicao).HasColumnType("datetime");

                entity.Property(e => e.dteDataGravacao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblComentario_Rascunho>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblComen__11B679324D667940");

                entity.HasIndex(e => new { e.intEmployeeID, e.intQuestaoID })
                    .HasName("_net_IX_tblComentario_Rascunho_intEmployeeID");
            });

            modelBuilder.Entity<tblCompanies>(entity =>
            {
                entity.HasKey(e => e.intCompanyID);

                entity.Property(e => e.bitBranch)
                    .IsRequired()
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.bitVendor)
                    .IsRequired()
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.txtAddress1).HasMaxLength(100);

                entity.Property(e => e.txtAddress2).HasMaxLength(100);

                entity.Property(e => e.txtCNPJ)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.txtCertificado)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtCodigoCustodiaCheques)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.txtContract).HasMaxLength(4000);

                entity.Property(e => e.txtDirectory)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtFantasyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtLicencaCobreBem).HasMaxLength(300);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(50);

                entity.Property(e => e.txtNomeChave).HasMaxLength(100);

                entity.Property(e => e.txtRegisterCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtRegistrationName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtSenha).HasMaxLength(100);

                entity.Property(e => e.txtZipCode).HasMaxLength(50);

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblCompanies)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblCompanies_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblCompanies)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblCompanies_tblCities");
            });

            modelBuilder.Entity<tblCompanySectors>(entity =>
            {
                entity.HasKey(e => e.intCompanySectorID);

                entity.HasIndex(e => new { e.txtDescription, e.intCompanySectorID })
                    .HasName("_net_IX_tblCompanySectors_intCompanySectorID");

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intResponsible)
                    .WithMany(p => p.tblCompanySectorsintResponsible)
                    .HasForeignKey(d => d.intResponsibleID)
                    .HasConstraintName("FK_tblCompanySectors_tblPersons");

                entity.HasOne(d => d.intSubstitute)
                    .WithMany(p => p.tblCompanySectorsintSubstitute)
                    .HasForeignKey(d => d.intSubstituteID)
                    .HasConstraintName("FK_tblCompanySectors_tblPersons1");
            });

            modelBuilder.Entity<tblConcursoCatologoDeClassificacoes>(entity =>
            {
                entity.HasKey(e => e.intClassificacaoID)
                    .HasName("PK_tblConcursoClassificacoes");

                entity.Property(e => e.txtDescricaoClassificacao).HasMaxLength(1000);

                entity.Property(e => e.txtSubTipoDeClassificacao).HasMaxLength(100);

                entity.Property(e => e.txtTipoDeClassificacao)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblConcursoPremium>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblConcu__11B6793247E5F4B9");

                entity.Property(e => e.txtSigla)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<tblConcursoPremium_DataLimite>(entity =>
            {
                entity.Property(e => e.dteDataLimite).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblConcursoQuestaoEmClassificacao>(entity =>
            {
                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblConcu__11B67932172EA388");

                entity.HasIndex(e => new { e.intQuestaoID, e.intClassificacaoID, e.intTipoDeClassificacao })
                    .HasName("_dta_index_tblConcursoQuestao_Classificacao_5_1032703077__K2_K4_K3");

                entity.HasIndex(e => new { e.intQuestaoID, e.intEmployeeID, e.intID })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_intID");

                entity.HasIndex(e => new { e.intQuestaoID, e.intTipoDeClassificacao, e.intClassificacaoID })
                    .HasName("_dta_index_tblConcursoQuestao_Classificacao_5_1032703077__K2_K3_K4");

                entity.HasIndex(e => new { e.intTipoDeClassificacao, e.intQuestaoID, e.intClassificacaoID })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_teste");

                entity.HasIndex(e => new { e.intID, e.intEmployeeID, e.txtRegister, e.dteDate, e.intClassificacaoID, e.intTipoDeClassificacao, e.intQuestaoID })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_intClassificacaoID");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intEmployeeID, e.txtRegister, e.dteDate, e.intTipoDeClassificacao, e.intClassificacaoID })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtRegister).HasMaxLength(50);

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblConcursoQuestao_Classificacao)
                    .HasForeignKey(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestao_Classificacao_tblConcursoQuestoes");
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoID);

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.tipoquestao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.txtIP)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.txtMotivo)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblBloqueioQuestoes>(entity =>
            {
                entity.HasKey(e => e.intBloqueioConcursoId)
                    .HasName("PK__tblBloqu__5C78D0CF5E3B1BF4");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_Autorizacao>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.intQuestaoID, e.intMaterialID })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_Autorizacao_intQuestaoID_intMaterialID");

                entity.HasIndex(e => new { e.intQuestaoID, e.intMaterialID, e.dteDateTime, e.bitAutorizacao })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_bitAutorizacao");

                entity.HasIndex(e => new { e.intQuestaoID, e.intEmployeeID, e.intMaterialAno, e.intMaterialID, e.bitAutorizacao })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_Autorizacao_intMaterialID_bitAutorizacao");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteEnvio).HasColumnType("datetime");

                entity.Property(e => e.txtComentarioCordendador).HasColumnType("text");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblConcursoQuestao_Classificacao_Autorizacao)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestao_Classificacao_Autorizacao_tblEmployees");

                entity.HasOne(d => d.tblProducts)
                    .WithMany(p => p.tblConcursoQuestao_Classificacao_Autorizacao)
                    .HasForeignKey(d => d.intMaterialID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestao_Classificacao_Autorizacao_tblProducts");

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblConcursoQuestao_Classificacao_Autorizacao)
                    .HasForeignKey(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestao_Classificacao_Autorizacao_tblConcursoQuestoes");
            });

            
            modelBuilder.Entity<tblSysRoles>(entity =>
            {
                entity.HasKey(e => e.intResponsabilityID);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada_log>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoLogID);

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtIP)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.txtMotivo)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_Autorizacao_log>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteEnvio).HasColumnType("datetime");

                entity.Property(e => e.txtComentarioCordendador).HasColumnType("text");
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila>(entity =>
            {
                entity.HasKey(e => new { e.intClassificacaoID, e.intProductGroupID })
                    .HasName("PK_tblQuestao_Classificacao_ProductGroup_Validacao");

                entity.HasIndex(e => e.intProductGroupID)
                    .HasName("IX_tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila");
            });

            modelBuilder.Entity<tblConcursoQuestao_Classificacao_log>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblConcu__11B679321FC3E989");

                entity.HasIndex(e => new { e.intQuestaoID, e.intTipoDeClassificacao })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_log_intQuestaoID_intTipoDeClassificacao");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intTipoDeClassificacao, e.intClassificacaoID, e.intEmployeeID, e.txtRegister, e.dteDate, e.dteDateLog, e.intIDOrigial, e.txtOperacao })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_log__txtOperacao");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intTipoDeClassificacao, e.intEmployeeID, e.txtRegister, e.dteDate, e.dteDateLog, e.intIDOrigial, e.intClassificacaoID, e.txtOperacao })
                    .HasName("_net_IX_tblConcursoQuestao_Classificacao_log_intClassificacaoID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dteDateLog)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtOperacao)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtRegister).HasMaxLength(50);
            });

            modelBuilder.Entity<tblConcursoQuestoes>(entity =>
            {
                entity.HasKey(e => e.intQuestaoID);

                entity.HasIndex(e => e.bitDiscursiva)
                    .HasName("_net_IX_tblConcursoQuestoes_bitDiscursiva");

                entity.HasIndex(e => e.intEmployeeComentarioID)
                    .HasName("_dta_index_tblConcursoQuestoes_5_1344320149__K27");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitAnulada })
                    .HasName("_net_IX_tblConcursoQuestoes_bitAnulada");

                entity.HasIndex(e => new { e.intQuestaoID, e.dteQuestao })
                    .HasName("_net_IX_tblConcursoQuestoes_dteQuestao");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitAnuladaPosRecurso, e.intProvaID })
                    .HasName("_net_IX_tblConcursoQuestoes_intQuestaoID_bitAnuladaPosConcurso");

                entity.HasIndex(e => new { e.intQuestaoID, e.intOrder, e.ID_CONCURSO_RECURSO_STATUS, e.bitComentarioAtivo, e.intProvaID, e.intStatus_Banca_Recurso })
                    .HasName("_net_IX_tblConcursoQuestoes_intProvaID");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtEnunciado, e.intProvaID, e.intOrder, e.txtComentario, e.bitAnulada, e.intYear })
                    .HasName("_net_IX_tblConcursoQuestoes_intYear_2");

                entity.HasIndex(e => new { e.txtEnunciado, e.txtComentario, e.intQuestaoID, e.intProvaID, e.intOrder, e.ID_CONCURSO_RECURSO_STATUS, e.intStatus_Banca_Recurso, e.intEmployeeID, e.bitAnulada, e.bitAnuladaPosRecurso })
                    .HasName("IX_tblConcursoQuestoesGabarito");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtEnunciado, e.ID_CONCURSO_RECURSO_STATUS, e.bitComentarioAtivo, e.txtEnunciadoConcursoSiteRecursos, e.intStatus_Banca_Recurso, e.bitGabaritoPosRecursoLiberado, e.txtComentario, e.intYear, e.bitAnulada, e.bitAnuladaPosRecurso, e.intProvaID, e.intOrder })
                    .HasName("_net_IX_tblConcursoQuestoes_intProvaID_intOrder");

                entity.Property(e => e.ID_CONCURSO_RECURSO_STATUS).HasDefaultValueSql("('')");

                entity.Property(e => e.bitCasoClinico)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.bitConceitual)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.dteQuestao).HasColumnType("datetime");

                entity.Property(e => e.guidQuestaoID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.txtComentario).HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtComentario_banca_recurso).HasColumnType("text");

                entity.Property(e => e.txtEnunciado)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtEnunciadoConcursoSiteRecursos).HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtFonteTipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtLetraAlternativaSugerida)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtObservacao).HasMaxLength(4000);

                entity.Property(e => e.txtRecurso).HasColumnType("text");
            });

            modelBuilder.Entity<tblConcursoQuestoesGravacaoProtocolo_Catalogo>(entity =>
            {
                entity.HasKey(e => e.intProtocoloID)
                    .HasName("PK_tblProtocolo_Material");

                entity.Property(e => e.dteDataProtocolo).HasColumnType("datetime");

                entity.Property(e => e.intTypeID)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.timestamp).IsRowVersion();

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT>(entity =>
            {
                entity.HasKey(e => e.intPPTControleID);

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtNamePPT)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.intProtocolo)
                    .WithMany(p => p.tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT)
                    .HasForeignKey(d => d.intProtocoloID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT_tblConcursoQuestoesGravacaoProtocolo_Catalogo");
            });

            modelBuilder.Entity<tblConcursoQuestoesGravacaoProtocolo_Codigos>(entity =>
            {
                entity.HasKey(e => new { e.intQuestaoID, e.intTypeID })
                    .HasName("PK_tblConcursoQuestoesGravacaoProtocolo_Codigos_1");

                entity.HasIndex(e => new { e.intQuestaoID, e.intTypeID, e.bitActive })
                    .HasName("_net_IX_tblConcursoQuestoesGravacaoProtocolo_Codigos_01");

                entity.HasIndex(e => new { e.txtCode, e.intQuestaoID, e.dteDateTime })
                    .HasName("_dta_index_tblConcursoQuestoesGravacaoProto_5_454552953__K1_K5_3");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteDateTimeUpload).HasColumnType("datetime");

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcursoQuestoesGravacaoProtocolo_Questoes>(entity =>
            {
                entity.HasKey(e => new { e.intProtocoloID, e.intQuestaoID });

                entity.HasIndex(e => new { e.intQuestaoID, e.intProtocoloID })
                    .HasName("_net_IX_tblConcursoQuestoesGravacaoProtocolo_Questoes_intQuestaoID_intProtocoloID");

                entity.HasIndex(e => new { e.intProtocoloID, e.intQuestaoID, e.intEmployeeID, e.bitValida })
                    .HasName("_net_IX_tblConcursoQuestoesGravacaoProtocolo_Questoes_bitValida");

                entity.HasIndex(e => new { e.intProtocoloID, e.intQuestaoID, e.bitValida, e.bitReutilizar, e.dteValidationQuest, e.intEmployeeID })
                    .HasName("_net_IX_tblConcursoQuestoesGravacaoProtocolo_Questoes_intEmployeeID");

                entity.Property(e => e.dteDistribuitionQuest).HasColumnType("datetime");

                entity.Property(e => e.dteValidationQuest).HasColumnType("datetime");

                entity.Property(e => e.timestamp).IsRowVersion();

                entity.Property(e => e.txtCodeCopiar).IsUnicode(false);

                entity.Property(e => e.txtCodeDeletar).IsUnicode(false);

                entity.HasOne(d => d.intProtocolo)
                    .WithMany(p => p.tblConcursoQuestoesGravacaoProtocolo_Questoes)
                    .HasForeignKey(d => d.intProtocoloID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestoesGravacaoProtocolo_Questoes_tblConcursoQuestoesGravacaoProtocolo_Catalogo");
            });

            modelBuilder.Entity<tblConcursoQuestoes_Alternativas>(entity =>
            {
                entity.HasKey(e => new { e.intQuestaoID, e.txtLetraAlternativa });

                entity.HasIndex(e => e.intAlternativaID)
                    .HasName("_net_IX_tblConcursoQuestoes_Alternativas_intAlternativaID")
                    .IsUnique();

                entity.HasIndex(e => new { e.txtLetraAlternativa, e.bitCorretaPreliminar })
                    .HasName("_net_IX_tblConcursoQuestoes_Alternativas_bitCorretaPreliminar");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitCorreta, e.bitCorretaPreliminar })
                    .HasName("_net_IX_tblConcursoQuestoes_Alternativas_bitCorreta_bitCorretaPreliminar");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtLetraAlternativa, e.bitCorreta })
                    .HasName("IXQuestaoAlternativaCorreta_tblConcursoQuestoes_Alternativas");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtLetraAlternativa, e.bitCorreta, e.bitCorretaPreliminar })
                    .HasName("_net_IX_tblConcursoQuestoes_Alternativas_bitCorreta");

                entity.Property(e => e.txtLetraAlternativa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.intAlternativaID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtImagem).HasMaxLength(1000);

                entity.Property(e => e.txtImagemOtimizada).HasMaxLength(1000);

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblConcursoQuestoes_Alternativas)
                    .HasForeignKey(d => d.intQuestaoID)
                    .HasConstraintName("FK_tblConcursoQuestoes_Alternativas_tblConcursoQuestoes");
            });

            modelBuilder.Entity<tblConcursoQuestoes_Aluno_Favoritas>(entity =>
            {
                entity.HasKey(e => new { e.intQuestaoID, e.intClientID, e.dteDate });

                entity.HasIndex(e => new { e.intQuestaoID, e.charResposta })
                    .HasName("_net_IX_tblConcursoQuestoes_Aluno_Favoritas_intQuestaoID_charResposta");

                entity.HasIndex(e => new { e.dteDate, e.charResposta, e.bitResultadoResposta, e.charRespostaNova, e.intClientID, e.bitActive, e.bitDuvida, e.bitVideo, e.intQuestaoID })
                    .HasName("_net_IX_tblConcursoQuestoes_Aluno_Favoritas_IntClientID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.charResposta)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.charRespostaNova)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblConcursoQuestoes_Aluno_Favoritas)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestoes_Aluno_Favoritas_tblPersons");

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblConcursoQuestoes_Aluno_Favoritas)
                    .HasForeignKey(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConcursoQuestoes_Aluno_Favoritas_tblConcursoQuestoes");
            });

            modelBuilder.Entity<tblConcursoQuestoes_recursosComentario_Imagens>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK_tblImagens_recursosComentario");

                entity.Property(e => e.intClassificacao).IsUnicode(false);

                entity.Property(e => e.txtLabel).IsUnicode(false);

                entity.Property(e => e.txtName).IsUnicode(false);

                entity.Property(e => e.txtPath)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcursoRecursoFavoritado>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.HasIndex(e => e.intClienteId)
                    .HasName("IX_tblConcursoRecursoFavoritado_intClienteId_0FB97");

                entity.HasIndex(e => new { e.intConcursoId, e.intClienteId })
                    .HasName("IX_tblConcursoRecursoFavoritado_intConcursoId_intClienteId_D7D7B");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblConcurso_Provas>(entity =>
            {
                entity.HasKey(e => e.intProvaID)
                    .HasName("PK_tblConcursoProvas");

                entity.HasIndex(e => e.guidProvaID)
                    .HasName("_net_IX_tblConcurso_Provas_guidProvaID");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.ID_CONCURSO_RECURSO_STATUS })
                    .HasName("IX_tblConcurso_Provas_1");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.intProvaID })
                    .HasName("_dta_index_tblConcurso_Provas_5_201155862__K2_K1");

                entity.HasIndex(e => new { e.ID_CONCURSO_RECURSO_STATUS, e.ID_CONCURSO })
                    .HasName("_dta_index_tblConcurso_Provas_5_201155862__K5_K2");

                entity.HasIndex(e => new { e.intProvaID, e.ID_CONCURSO, e.intProvaTipoID })
                    .HasName("_net_IX_tblConcurso_Provas_intProvaTipoID");

                entity.HasIndex(e => new { e.intProvaID, e.intProvaTipoID, e.ID_CONCURSO })
                    .HasName("IX_tblConcurso_Provas_ID_CONCURSO_71643");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.intProvaID, e.bitVendaLiberada, e.intProvaTipoID })
                    .HasName("_net_IX_tblConcurso_Provas_bitVendaLiberada");

                entity.HasIndex(e => new { e.intProvaTipoID, e.intProvaID, e.ID_CONCURSO, e.txtName })
                    .HasName("_net_IX_tblConcursosProvas_ID_Concurso_intProvaID");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.txtName, e.intProvaID, e.intDVDID, e.bitVendaLiberada, e.bitAtiva })
                    .HasName("_dta_index_tblConcurso_Provas_5_201155862__K1_K12_K13_K14_2_3");

                entity.HasIndex(e => new { e.txtName, e.bitRecursoForumAcertosLiberado, e.intRecursoForumAcertosQtdQuestoes, e.ID_CONCURSO_RECURSO_STATUS, e.intProvaID, e.ID_CONCURSO })
                    .HasName("_dta_index_tblConcurso_Provas_5_201155862__K5_K1_K2_3_18_19");

                entity.HasIndex(e => new { e.txtName, e.bitRecursoForumAcertosLiberado, e.intRecursoForumAcertosQtdQuestoes, e.intProvaID, e.ID_CONCURSO, e.ID_CONCURSO_RECURSO_STATUS })
                    .HasName("_dta_index_tblConcurso_Provas_5_201155862__K1_K2_K5_3_18_19");

                entity.HasIndex(e => new { e.intProvaID, e.ID_CONCURSO, e.txtName, e.ID_CONCURSO_RECURSO_STATUS, e.dteDate, e.intProvaTipoID, e.bitAtivo })
                    .HasName("_net_IX_tblConcurso_Provas_bitAtivo");

                entity.Property(e => e.bitAtivo).HasDefaultValueSql("((1))");

                entity.Property(e => e.bitUploadAluno).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeForumComentBlocked).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeLastUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteExpiracao).HasColumnType("datetime");

                entity.Property(e => e.dteLibGabaritoDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteLibGabaritoPosDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteLibProvaDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteLightboxExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.dtePrevLibGabaritoDateTime).HasColumnType("datetime");

                entity.Property(e => e.dtePrevLibGabaritoPosDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteProvaFimDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteProvaInicioDateTime).HasColumnType("datetime");

                entity.Property(e => e.guidProvaID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.intProvaTipoID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtBibliografia).HasMaxLength(1000);

                entity.Property(e => e.txtDescription).HasColumnType("text");

                entity.Property(e => e.txtDestaquePainelAvisos)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtEditaLiberacaoProva).HasMaxLength(50);

                entity.Property(e => e.txtEditaRecurso).HasColumnType("text");

                entity.Property(e => e.txtGabaritoFinal).HasMaxLength(1000);

                entity.Property(e => e.txtGabaritoPreliminar).HasMaxLength(1000);

                entity.Property(e => e.txtLocalLibGabarito).HasMaxLength(200);

                entity.Property(e => e.txtLocalLibGabaritoPos).HasMaxLength(200);

                entity.Property(e => e.txtLocalLiberacao).HasMaxLength(200);

                entity.Property(e => e.txtName).HasMaxLength(50);

                entity.Property(e => e.txtObs).HasColumnType("text");

                entity.Property(e => e.txtProva)
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.Property(e => e.txtTipoProva).HasMaxLength(50);

                entity.Property(e => e.txtTituloPainelAvisos).HasMaxLength(300);
            });

            modelBuilder.Entity<tblConcurso_Provas_Acertos>(entity =>
            {
                entity.HasKey(e => e.intProvaAcertosID);

                entity.HasIndex(e => new { e.intContactID, e.intProvaID })
                    .HasName("_net_IX_tblConcurso_Provas_Acertos_intContactID_intProvaID");

                entity.HasIndex(e => new { e.intEspecialidadeID, e.intProvaAcertosID, e.intContactID, e.dteCadastro, e.bitActive, e.intProvaID, e.intAcertos })
                    .HasName("_net_IX_tblConcurso_Provas_Acertos_intProvaID");

                entity.HasIndex(e => new { e.intProvaAcertosID, e.intContactID, e.dteCadastro, e.intAcertos, e.intEspecialidadeID, e.bitActive, e.intProvaID })
                    .HasName("_net_IX_tblConcurso_Provas_Acertos_bitActive_intProvaID");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.txtIP_Usuario)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtNickname)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcurso_Provas_Acertos_log>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.txtIP_Usuario)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtNickname)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcurso_Provas_ArquivosAluno>(entity =>
            {
                entity.HasKey(e => e.intArquivoAlunoID)
                    .HasName("PK__tblConcu__3957D1AD5CAC9243");

                entity.Property(e => e.bitAtivo).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtHashNomeArquivo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtNomeArquivo)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtPasta)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblConcurso_Provas_ArquivosAluno)
                    .HasForeignKey(d => d.intContactID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblConcur__intCo__0E8FD726");

                entity.HasOne(d => d.intProva)
                    .WithMany(p => p.tblConcurso_Provas_ArquivosAluno)
                    .HasForeignKey(d => d.intProvaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblConcur__intPr__0D9BB2ED");
            });

            modelBuilder.Entity<tblConcurso_Provas_Forum>(entity =>
            {
                entity.HasKey(e => e.intProvaForumID);

                entity.HasIndex(e => e.intContactID)
                    .HasName("_net_IX_tblConcurso_Provas_Forum_intContactID");

                entity.HasIndex(e => new { e.intProvaForumID, e.intContactID, e.txtComentario, e.bitActive, e.intEspecialidadeID, e.intProvaID, e.dteCadastro })
                    .HasName("_net_IX_tblConcurso_Provas_Forum_intProvaID_intProvaID");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtIP_Usuario)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtNickname)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcurso_Provas_Forum_Moderadas>(entity =>
            {
                entity.HasKey(e => e.intProvaForumModeradaID)
                    .HasName("PK__tblConcu__E4CA4B7B4F9E2B7F");

                entity.HasIndex(e => new { e.intProvaForumId, e.bitModerado })
                    .HasName("_net_IX_tblConcurso_Provas_Forum_Moderadas_intProvaForumId");
            });

            modelBuilder.Entity<tblConcurso_Provas_Forum_log>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.intProvaAcertosId, e.bitActive, e.intProvaID })
                    .HasName("_net_IX_tblConcurso_Provas_Forum_log");

                entity.HasIndex(e => new { e.txtComentario, e.bitActive, e.intContactID, e.intProvaID, e.dteCadastro, e.intProvaForumId })
                    .HasName("_net_IX_tblConcurso_Provas_Forum_log_intContactID_intProvaID_dteCadastro_intProvaForumId_txtComentario_bitActive");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.intProvaForumId).HasDefaultValueSql("((0))");

                entity.Property(e => e.intProvaForumLogID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtIP_Usuario)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtNickname)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcurso_Provas_Tipos>(entity =>
            {
                entity.HasKey(e => e.intProvaTipoID);

                entity.Property(e => e.IntOrder).HasDefaultValueSql("((0))");

                entity.Property(e => e.bitDiscursiva).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblConcurso_Recurso_AccessDenied>(entity =>
            {
                entity.HasKey(e => e.intConcursoRecursoId);

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.txtAutorizador).HasMaxLength(50);

                entity.Property(e => e.txtLoggedUser).HasMaxLength(50);

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(2000);

                entity.Property(e => e.txtSolicitador).HasMaxLength(50);
            });

            modelBuilder.Entity<tblConcurso_Recurso_AccessDenied_LOG>(entity =>
            {
                entity.HasKey(e => e.intAccessDeniedId);

                entity.Property(e => e.dteDateTimeChangeLog).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.txtAutorizador).HasMaxLength(50);

                entity.Property(e => e.txtLoggedUser).HasMaxLength(50);

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(2000);

                entity.Property(e => e.txtSolicitador).HasMaxLength(50);
            });

            modelBuilder.Entity<tblConcurso_Recurso_Aluno>(entity =>
            {
                entity.HasKey(e => e.ID_CONCURSO_RECURSO_ALUNO);

                entity.HasIndex(e => new { e.intQuestaoID, e.intTipo })
                    .HasName("_net_IX_tblConcurso_Recurso_Aluno_intQuestaoID_intTipo");

                entity.HasIndex(e => new { e.intTipo, e.bitActive })
                    .HasName("_net_IX_tblConcurso_Recurso_Aluno_intTipo");

                entity.HasIndex(e => new { e.intContactID, e.bitOpiniao, e.dteCadastro, e.bitActive, e.intTipo })
                    .HasName("_net_IX_tblConcurso_Recurso_Aluno_dteCadastro");

                entity.HasIndex(e => new { e.txtRecurso_Comentario, e.dteCadastro, e.txtAlternativa_Sugerida, e.bitOpiniao, e.txtIP_Usuario, e.bitActive, e.intQuestaoID, e.intTipo, e.intContactID })
                    .HasName("_dta_index_tblConcurso_Recurso_Aluno_5_119879694__K8_K3_K10_K2");

                entity.HasIndex(e => new { e.ID_CONCURSO_RECURSO_ALUNO, e.intQuestaoID, e.txtRecurso_Comentario, e.txtAlternativa_Sugerida, e.txtIP_Usuario, e.bitActive, e.bitOpiniao, e.intContactID, e.intTipo, e.dteCadastro })
                    .HasName("_net_IX_tblConcurso_Recurso_Aluno_intContactID_intTipo");

                entity.Property(e => e.bitActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.intTipo)
                    .HasDefaultValueSql("((1))")
                    .HasComment("1=pre | 2=pos");

                entity.Property(e => e.txtAlternativa_Sugerida)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtIP_Usuario)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtRecurso_Comentario)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<tblConcurso_Recurso_Funcionarios>(entity =>
            {
                entity.HasKey(e => e.intEmployeeID);

                entity.Property(e => e.intEmployeeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblConcurso_Recurso_Status>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.ID_CONCURSO_RECURSO_STATUS, e.txtConcursoQuestao_Status })
                    .HasName("IX_tblConcurso_Recurso_Status");

                entity.Property(e => e.txtCategoria)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtConcursoQuestao_Status).HasMaxLength(50);
            });

            modelBuilder.Entity<tblConcursos>(entity =>
            {
                entity.HasKey(e => e.intConcursoID);

                entity.HasIndex(e => new { e.intConcursoID, e.bitEnviarEmail })
                    .HasName("_net_IX_tblConcursos_bitEnviarEmail");

                entity.HasIndex(e => new { e.intConcursoID, e.intAno, e.bitProducao, e.intStateID })
                    .HasName("_net_IX_tblConcursos_04");

                entity.HasIndex(e => new { e.intConcursoID, e.txtSigla, e.intAno, e.dblValorInscricao })
                    .HasName("_net_IX_tblConcursos_02");

                entity.HasIndex(e => new { e.intStateID, e.txtSigla, e.intAno, e.bitProducao })
                    .HasName("IX_tblConcursos_PesquisaPrincipal");

                entity.HasIndex(e => new { e.txtSigla, e.intAno, e.intStateID, e.bitProducao })
                    .HasName("_net_IX_tblConcursos_05");

                entity.HasIndex(e => new { e.intConcursoID, e.intStateID, e.intAno, e.bitProducao, e.bitExibirSiteRecursos })
                    .HasName("_net_IX_tblConcursos_intAno_bitProducao");

                entity.HasIndex(e => new { e.intConcursoID, e.txtDescription, e.txtImagem, e.intStateID, e.strInscricaoPeriodo })
                    .HasName("IX_tblConcursos_BuscaConcursos2");

                entity.HasIndex(e => new { e.intConcursoID, e.txtSigla, e.intCity, e.intStateID, e.bitProducao, e.intAno, e.txtDescription })
                    .HasName("IX_tblConcursos_BuscaConcursos1");

                entity.HasIndex(e => new { e.intConcursoID, e.txtDescription, e.txtSigla, e.txtImagem, e.strInscricaoPeriodo, e.strProvasObs, e.intAno, e.intStateID, e.bitProducao })
                    .HasName("_net_IX_tblConcursos_06");

                entity.HasIndex(e => new { e.intConcursoID, e.intStateID, e.intAno, e.intEmployeeID, e.bitEditalDivulgado, e.dteEditalDivulgado, e.dtePrevisaoEditalDivulgado, e.bitProducao, e.bitEnviarEmail, e.txtDescription, e.txtSigla })
                    .HasName("_net_IX_tblConcursos_01");

                entity.HasIndex(e => new { e.txtDescription, e.txtSigla, e.txtImagem, e.intCity, e.intStateID, e.intEmployeeID, e.dblInscricaoTaxa, e.strInscricaoObs, e.strInscricaoPeriodo, e.strAvaliacaoMedgrupo, e.dteDataProva, e.bitCadernoLiberado, e.intNroQuestoes, e.strBibliografia, e.dtePrazoRecurso, e.dblValorRecurso, e.strListaClassificados, e.strForumProvaAnoPassado, e.intNroQuestoesRecurso, e.intNroQuestoesAlteradas, e.strResumo, e.strRecursoPrazoTexto, e.dteRecursoPrazoReal, e.strRecursoObs, e.strProvasObs, e.strResultadosObs, e.strGabaritosObs, e.intConcursoIDOrigem, e.bitProducao, e.bitValidadoFull, e.intConcursoIDDestino, e.bitEnviarEmail, e.bitEditalDivulgado, e.dteEditalDivulgado, e.dtePrevisaoEditalDivulgado, e.dblValorInscricao, e.strBibliografiaTexto, e.bitExibirSiteRecursos, e.intAno, e.intConcursoID })
                    .HasName("_dta_index_tblConcursos_5_2903223__K4_K1_2_3_5_6_7_8_9_14_15_16_17_18_19_20_21_22_23_24_25_26_27_30_31_32_47_48_49_51_52_53_54_");

                entity.Property(e => e.dblInscricaoTaxa).HasMaxLength(100);

                entity.Property(e => e.dblValorInscricao).HasMaxLength(100);

                entity.Property(e => e.dblValorRecurso).HasMaxLength(100);

                entity.Property(e => e.dteDataProva).HasMaxLength(100);

                entity.Property(e => e.dteEditalDivulgado).HasMaxLength(100);

                entity.Property(e => e.dtePrazoRecurso).HasMaxLength(100);

                entity.Property(e => e.dtePrevisaoEditalDivulgado).HasMaxLength(100);

                entity.Property(e => e.dteRecursoPrazoReal).HasMaxLength(100);

                entity.Property(e => e.strAvaliacaoMedgrupo).HasMaxLength(100);

                entity.Property(e => e.strBibliografia).HasMaxLength(300);

                entity.Property(e => e.strBibliografiaTexto).HasColumnType("nvarchar(max)");

                entity.Property(e => e.strForumProvaAnoPassado).HasMaxLength(50);

                entity.Property(e => e.strGabaritosObs).HasMaxLength(300);

                entity.Property(e => e.strInscricaoObs).HasMaxLength(200);

                entity.Property(e => e.strInscricaoPeriodo).HasMaxLength(100);

                entity.Property(e => e.strListaClassificados).HasMaxLength(300);

                entity.Property(e => e.strProvasObs).HasMaxLength(300);

                entity.Property(e => e.strRecursoObs).HasMaxLength(300);

                entity.Property(e => e.strRecursoPrazoTexto).HasMaxLength(100);

                entity.Property(e => e.strResultadosObs).HasMaxLength(300);

                entity.Property(e => e.strResumo).HasMaxLength(300);

                entity.Property(e => e.txtDescription).HasMaxLength(200);

                entity.Property(e => e.txtImagem).HasMaxLength(100);

                entity.Property(e => e.txtSigla)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcursosProvas>(entity =>
            {
                entity.HasKey(e => e.intProvaID);

                entity.HasIndex(e => e.intEditalID)
                    .HasName("_net_IX_tblConcursosProvas_intEditalID");

                entity.HasIndex(e => new { e.dteDataInicio, e.dteDataTermino, e.bitALiberar, e.intNumQuestoes, e.intConcursoID, e.bitBuscarProva })
                    .HasName("_net_IX_tblConcursosProvas_01");

                entity.HasIndex(e => new { e.dteDataInicio, e.intConcursoID, e.intEditalID, e.dteDataTermino, e.strDiaSemana, e.bitALiberar, e.intNumQuestoes, e.bitBuscarProva })
                    .HasName("_net_IX_tblConcursosProvas_bitBuscarProva");

                entity.Property(e => e.dteDataInicio).HasMaxLength(100);

                entity.Property(e => e.dteDataTermino).HasMaxLength(100);

                entity.Property(e => e.dteUltimaAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.strDiaSemana).HasMaxLength(100);

                entity.Property(e => e.strObs).HasMaxLength(300);

                entity.Property(e => e.strPeriodo).HasMaxLength(100);

                entity.Property(e => e.txtConcursoR1)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<tblConcursosVagas>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.intVagaID)
                    .HasName("_net_IX_tblConcursosVagas_06");

                entity.HasIndex(e => new { e.intConcursoID, e.intEspecialidadeID, e.intInstituicaoID })
                    .HasName("_net_IX_intConcursoID_intEspecialidadeID");

                entity.HasIndex(e => new { e.intVagaID, e.intConcursoID, e.intInstituicaoID, e.intEspecialidadeID, e.intQuantidade, e.intOrigemVagaID, e.intOrigemSituacaoID, e.dblNotaCorte, e.intOrigemNotaCorteID, e.txtR1R3, e.intCandidatos, e.dteUltimaAtualizacao, e.dblVagasCandidato, e.bitExibirInstSite, e.intOrigemCandVaga, e.intSituacaoID })
                    .HasName("_net_IX_tblConcursosVagas_intSituacaoID");

                entity.HasIndex(e => new { e.intVagaID, e.intEspecialidadeID, e.intQuantidade, e.intOrigemVagaID, e.intSituacaoID, e.intOrigemSituacaoID, e.dblNotaCorte, e.intOrigemNotaCorteID, e.txtR1R3, e.intCandidatos, e.dteUltimaAtualizacao, e.dblVagasCandidato, e.bitExibirInstSite, e.intOrigemCandVaga, e.dblNotaCorte1, e.intOrigemNotaCorte1ID, e.intConcursoID, e.intInstituicaoID })
                    .HasName("_net_IX_tblConcursosVagas_intConcursoID_intInstituicaoID");

                entity.Property(e => e.dteUltimaAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.intVagaID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtR1R3).HasMaxLength(100);
            });

            modelBuilder.Entity<tblConteudo>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtHierarquia).HasMaxLength(50);

                entity.Property(e => e.txtTitulo).IsRequired();
            });

            modelBuilder.Entity<tblConteudoCategoria>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtAlias).IsRequired();

                entity.Property(e => e.txtTitulo).IsRequired();
            });

            modelBuilder.Entity<tblConteudoCategoriaLog>(entity =>
            {
                entity.HasKey(e => e.intLogID);

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtAlias).IsRequired();

                entity.Property(e => e.txtTitulo).IsRequired();
            });

            modelBuilder.Entity<tblConteudoLabel>(entity =>
            {
                entity.HasKey(e => e.intConteudoLabelId)
                    .HasName("PK__tblConte__CFA17BB249B346EC");

                entity.Property(e => e.txtConteudoLabel)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConteudoLabel_Item>(entity =>
            {
                entity.HasKey(e => e.intConteudoLabelItemId)
                    .HasName("PK__tblConte__59B766AA4D83D7D0");
            });

            modelBuilder.Entity<tblConteudoLog>(entity =>
            {
                entity.HasKey(e => e.intLogID);

                entity.HasIndex(e => new { e.intLogID, e.intID, e.bitAtivo, e.intCategoria })
                    .HasName("IX_tblConteudoLog_bitAtivo_intCategoria_601C8");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtHierarquia).HasMaxLength(50);

                entity.Property(e => e.txtTitulo).IsRequired();
            });

            modelBuilder.Entity<tblConteudoMaterialMed>(entity =>
            {
                entity.HasKey(e => e.intConteudoMaterialMedID);

                entity.HasIndex(e => new { e.data, e.imgPequena, e.titulo, e.intConteudoMaterialMedTipoID })
                    .HasName("_net_IX_tblConteudoMaterialMed_intConteudoMaterialMedTipoID_data_imgPequena_titulo");

                entity.Property(e => e.conteudo)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.data).HasColumnType("datetime");

                entity.Property(e => e.dataInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.imgGrande)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.imgPequena)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.titulo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.intConteudoMaterialMedTipo)
                    .WithMany(p => p.tblConteudoMaterialMed)
                    .HasForeignKey(d => d.intConteudoMaterialMedTipoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConteudoMaterialMed_tblConteudoMaterialMedTipo");
            });

            modelBuilder.Entity<tblConteudoMaterialMedTipo>(entity =>
            {
                entity.HasKey(e => e.intConteudoMaterialMedTipoID);

                entity.Property(e => e.ConteudoMaterialMedTipo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConteudoTipo>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtDescricao).IsRequired();

                entity.Property(e => e.txtValor).IsRequired();
            });

            modelBuilder.Entity<tblContrato>(entity =>
            {
                entity.HasKey(e => e.intContratoCPMEDId)
                    .HasName("PK__tblContr__8DC7975188612BB8");

                entity.Property(e => e.txtlinkPDF)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intDocumentos)
                    .WithMany(p => p.tblContrato)
                    .HasForeignKey(d => d.intDocumentosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblContra__intDo__5DB1B86B");
            });

            modelBuilder.Entity<tblContratoAceite>(entity =>
            {
                entity.HasKey(e => e.intContratoAceite)
                    .HasName("PK__tblContr__EF407C90FA5F51E5");

                entity.Property(e => e.intContratoId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtCriptoDadosAluno)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtCriptoPDF)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlPDFContrato)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.intContratoAlunoAceiteNavigation)
                    .WithMany(p => p.tblContratoAceite)
                    .HasForeignKey(d => d.intContratoAlunoAceite)
                    .HasConstraintName("FK__tblContra__intCo__4C01CFD2");
            });

            modelBuilder.Entity<tblContratoAlunoAceite>(entity =>
            {
                entity.HasKey(e => e.intContratoAlunoAceite)
                    .HasName("PK__tblContr__CF92AE52BBCC8127");

                entity.Property(e => e.txtDataAceiteAluno).HasColumnType("datetime");

                entity.Property(e => e.txtIpAluno)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtNomeAluno)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegistro)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblContratoImagem>(entity =>
            {
                entity.HasKey(e => e.intContratoImagemId)
                    .HasName("PK__tblContr__D1F901D712D0D10D");

                entity.Property(e => e.txtlinkImagem)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblContratoIntensivo>(entity =>
            {
                entity.HasKey(e => e.intContratoIntensivoId)
                    .HasName("PK__tblContr__F6DABE3D912B2B1F");
            });

            modelBuilder.Entity<tblContribuicao>(entity =>
            {
                entity.HasKey(e => e.intContribuicaoID);

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtCodigoMarcacao).IsUnicode(false);

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtOrigem).IsUnicode(false);

                entity.Property(e => e.txtOrigemSubnivel).IsUnicode(false);

                entity.Property(e => e.txtTrechoSelecionado).IsUnicode(false);

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblContribuicao)
                    .HasForeignKey(d => d.intApostilaID)
                    .HasConstraintName("FK_tblContribuicao_tblMaterialApostila");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblContribuicao)
                    .HasForeignKey(d => d.intClientID)
                    .HasConstraintName("FK_tblContribuicao_tblPersons");
            });

            modelBuilder.Entity<tblContribuicaoArquivo>(entity =>
            {
                entity.HasKey(e => e.intContribuicaoArquivoID);

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtContribuicaoArquivo).IsUnicode(false);

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtDuracao)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrl).IsUnicode(false);

                entity.HasOne(d => d.intContribuicao)
                    .WithMany(p => p.tblContribuicaoArquivo)
                    .HasForeignKey(d => d.intContribuicaoID)
                    .HasConstraintName("FK_tblContribuicaoArquivo_tblContribuicao");
            });

            modelBuilder.Entity<tblContribuicao_Encaminhadas>(entity =>
            {
                entity.HasKey(e => e.intContribuicaoEncaminhadaID)
                    .HasName("PK__tblContr__A7ED80C8A76E734A");

                entity.Property(e => e.dteDataEncaminhamento).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblContribuicao_EncaminhadasintClient)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicao_Encaminhada_tblEmployees");

                entity.HasOne(d => d.intContribuicao)
                    .WithMany(p => p.tblContribuicao_Encaminhadas)
                    .HasForeignKey(d => d.intContribuicaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicao_Encaminhada_tblContribuicao");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblContribuicao_EncaminhadasintEmployee)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicao_Encaminhada_tblEmployees2");
            });

            modelBuilder.Entity<tblContribuicoes_Arquivadas>(entity =>
            {
                entity.HasKey(e => e.intContribuicaoArquivadaID)
                    .HasName("PK__tblContr__DCC715E947903979");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblContribuicoes_Arquivadas)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicao_Arquivadas_tblPersons");

                entity.HasOne(d => d.intContribuicao)
                    .WithMany(p => p.tblContribuicoes_Arquivadas)
                    .HasForeignKey(d => d.intContribuicaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicao_Arquivadas_tblContribuicao");
            });

            modelBuilder.Entity<tblContribuicoes_Interacao>(entity =>
            {
                entity.HasKey(e => e.intContribuicaoInteracaoID)
                    .HasName("PK__tblContr__F216FB97E9C6E7B5");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblContribuicoes_Interacao)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicoes_Interacao_tblPersons");

                entity.HasOne(d => d.intContribuicao)
                    .WithMany(p => p.tblContribuicoes_Interacao)
                    .HasForeignKey(d => d.intContribuicaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblContribuicoes_Interacao_tblContribuicao");
            });

            modelBuilder.Entity<tblCountries>(entity =>
            {
                entity.HasKey(e => e.intCountryID);

                entity.Property(e => e.intCountryID).ValueGeneratedNever();

                entity.Property(e => e.txtCodigoBacen).HasMaxLength(10);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblCourses>(entity =>
            {
                entity.HasKey(e => e.intCourseID);

                entity.HasIndex(e => e.bitZeraVagasDisponiveis)
                    .HasName("_dta_index_tblCourses_5_1753981525__K6");

                entity.HasIndex(e => e.intCourseID)
                    .HasName("_dta_index_tblCourses_5_1753981525__K1");

                entity.HasIndex(e => e.intYear)
                    .HasName("_dta_index_tblCourses_5_1753981525__K2");

                entity.HasIndex(e => new { e.bitZeraVagasDisponiveis, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K6_K1");

                entity.HasIndex(e => new { e.intCourseID, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_1");

                entity.HasIndex(e => new { e.intCourseID, e.intYear })
                    .HasName("_net_IX_tblCourses_intCourseID_intYear");

                entity.HasIndex(e => new { e.intLastEmployeeID, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_14");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_3");

                entity.HasIndex(e => new { e.intYear, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K1");

                entity.HasIndex(e => new { e.bitZeraVagasDisponiveis, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K6_K1_K2");

                entity.HasIndex(e => new { e.bitZeraVagasDisponiveis, e.intYear, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K6_K2_K1");

                entity.HasIndex(e => new { e.dteStartDateTime, e.intYear, e.intCourseID })
                    .HasName("_dta_stat_1753981525_7_2_1");

                entity.HasIndex(e => new { e.intCourseID, e.intPrincipalClassRoomID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3");

                entity.HasIndex(e => new { e.intCourseID, e.intYear, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K2_K3");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.dteStartDateTime, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K7_K2_3");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1_K2");

                entity.HasIndex(e => new { e.intYear, e.intCourseID, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K1_K3");

                entity.HasIndex(e => new { e.intYear, e.intPrincipalClassRoomID, e.dteStartDateTime })
                    .HasName("_dta_stat_1753981525_2_3_7");

                entity.HasIndex(e => new { e.intYear, e.intPrincipalClassRoomID, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K3_K1");

                entity.HasIndex(e => new { e.intCourseID, e.dteStartDateTime, e.intYear, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K7_K2_K3_1");

                entity.HasIndex(e => new { e.intCourseID, e.intPrincipalClassRoomID, e.intYear, e.dteStartDateTime })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K3_K2_K7");

                entity.HasIndex(e => new { e.intCourseID, e.intYear, e.intPrincipalClassRoomID, e.dteStartDateTime })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K3_K7_1");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.dteStartDateTime, e.intYear, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K7_K2_K1_3");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intCourseID, e.intYear, e.dteStartDateTime })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1_K2_K7");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intYear, e.intCourseID, e.dteStartDateTime })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K2_K1_K7");

                entity.HasIndex(e => new { e.intYear, e.intCourseID, e.intPrincipalClassRoomID, e.dteStartDateTime })
                    .HasName("_dta_stat_1753981525_2_1_3_7");

                entity.HasIndex(e => new { e.intCourseID, e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_1_4_5_6");

                entity.HasIndex(e => new { e.intCourseID, e.intPrincipalClassRoomID, e.intOriginalCapacity, e.intOverbookingCapacity, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_1_3_4_5");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K2_4_5_6");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intYear, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K1_4_5_6");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.intCourseID, e.intPrincipalClassRoomID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K3_K2_4_5");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.intCourseID, e.intYear, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K2_K3_4_5");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.intYear, e.intCourseID, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K1_K3_4_5");

                entity.HasIndex(e => new { e.intCourseID, e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intFixedChairs, e.intExtraChairs, e.intPrincipalClassRoomID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K2_1_4_5_6_12_13");

                entity.HasIndex(e => new { e.intCourseID, e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intFixedChairs, e.intExtraChairs, e.intYear, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K3_1_4_5_6_12_13");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intFixedChairs, e.intExtraChairs, e.intCourseID, e.intPrincipalClassRoomID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K3_K2_4_5_6_12_13");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intFixedChairs, e.intExtraChairs, e.intPrincipalClassRoomID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1_K2_4_5_6_12_13");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.intFixedChairs, e.intExtraChairs, e.intYear, e.intPrincipalClassRoomID, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K3_K1_4_5_6_12_13");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intPrincipalClassRoomID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1_K2_4_5_6_7_8_10_11_12_13");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intYear, e.intPrincipalClassRoomID, e.intCourseID })
                    .HasName("_net_IX_tblCourses_intYear");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intLastEmployeeID, e.intCourseID, e.intYear, e.intPrincipalClassRoomID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K1_K2_K3_4_5_6_7_8_10_11_12_13_14");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intLastEmployeeID, e.intPrincipalClassRoomID, e.intCourseID, e.intYear })
                    .HasName("_dta_index_tblCourses_5_1753981525__K3_K1_K2_4_5_6_7_8_10_11_12_13_14");

                entity.HasIndex(e => new { e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intLastEmployeeID, e.intYear, e.intPrincipalClassRoomID, e.intCourseID })
                    .HasName("_dta_index_tblCourses_5_1753981525__K2_K3_K1_4_5_6_7_8_10_11_12_13_14");

                entity.HasIndex(e => new { e.intPrincipalClassRoomID, e.intOriginalCapacity, e.intOverbookingCapacity, e.bitZeraVagasDisponiveis, e.dteStartDateTime, e.dteEndDateTime, e.bitQuorumMinimo, e.intFixedChairs, e.intExtraChairs, e.intLastEmployeeID, e.intQuorumMinimo, e.intYear, e.intCourseID })
                    .HasName("_net_IX_tblCourses_intYear_intCourseID");

                entity.Property(e => e.intCourseID).ValueGeneratedNever();

                entity.Property(e => e.bitQuorumMinimo).HasDefaultValueSql("((0))");

                entity.Property(e => e.bitZeraVagasDisponiveis).HasDefaultValueSql("((0))");

                entity.Property(e => e.dteEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.intOriginalCapacity).HasDefaultValueSql("((0))");

                entity.Property(e => e.intOverbookingCapacity).HasDefaultValueSql("((0))");

                entity.Property(e => e.intQuorumMinimo).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.intCourse)
                    .WithOne(p => p.tblCourses)
                    .HasForeignKey<tblCourses>(d => d.intCourseID)
                    .HasConstraintName("FK_tblCourses_tblProducts");

                entity.HasOne(d => d.intPrincipalClassRoom)
                    .WithMany(p => p.tblCourses)
                    .HasForeignKey(d => d.intPrincipalClassRoomID)
                    .HasConstraintName("FK_tblCourses_tblClassRooms");
            });

            modelBuilder.Entity<tblCriterioOrdenacao_BuscaTexto>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtTexto)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblCronogramaConteudoRevalida>(entity =>
            {
                entity.HasKey(e => e.intRevalidaId)
                    .HasName("PK__tblCrono__BCB5F55A5E6A782C");

                entity.Property(e => e.dteInicio).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblCronogramaPrateleira>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblCrono__11B6793280A97643");

                entity.Property(e => e.dteDataInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblCronogramaPrateleira)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployees_CronogramaPrateleira");

                entity.HasOne(d => d.intMenu)
                    .WithMany(p => p.tblCronogramaPrateleira)
                    .HasForeignKey(d => d.intMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_Object_CronogramaPrateleira");
            });

            modelBuilder.Entity<tblCronogramaPrateleira_LessonTitles>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblCrono__11B67932DE8C8A53");

                entity.HasOne(d => d.intPrateleiraCronograma)
                    .WithMany(p => p.tblCronogramaPrateleira_LessonTitles)
                    .HasForeignKey(d => d.intPrateleiraCronogramaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCronogramaPrateleira_tblCronogramaPrateleira_LessonTitles");
            });

            modelBuilder.Entity<tblCtrlPanel_AccessControl_Persons_X_Roles>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.HasOne(d => d.intRole)
                    .WithMany(p => p.tblCtrlPanel_AccessControl_Persons_X_Roles)
                    .HasForeignKey(d => d.intRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCtrlPanel_AccessControl_Persons_X_Roles_tblCtrlPanel_AccessControl_Roles");
            });

            modelBuilder.Entity<tblCtrlPanel_AccessControl_Roles>(entity =>
            {
                entity.HasKey(e => e.intRoleId);

                entity.Property(e => e.txtAlias)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblCtrlPanel_Link>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intLinkID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtLink).HasMaxLength(50);

                entity.Property(e => e.txtUrl).HasMaxLength(150);
            });

            modelBuilder.Entity<tblCtrlPanel_Link_X_Employees>(entity =>
            {
                entity.HasKey(e => new { e.intEmployeeID, e.intLinkID });

                entity.Property(e => e.boolPermissaoNegada).HasDefaultValueSql("((0))");

                entity.Property(e => e.intLinkEmployeeID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblCtrlPanel_Relacao>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intRelacaoID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblDeviceMovel>(entity =>
            {
                entity.HasKey(e => e.intDeviceId)
                    .HasName("PK__tblDevic__314DC3F950A221C8");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblDeviceToken>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblDevic__11B679321B5F32D8");

                entity.HasIndex(e => e.intClientID)
                    .HasName("_net_IX_tblDeviceToken_intClientID");

                entity.HasIndex(e => new { e.intClientID, e.txtOneSignalToken, e.bitAtivo, e.intApplicationId })
                    .HasName("IX_tblDeviceToken_bitAtivo_intApplicationId_F3C62");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtOneSignalToken)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDeviceToken)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDevice__intCl__1D477B4A");
            });

            modelBuilder.Entity<tblDocumento>(entity =>
            {
                entity.HasKey(e => e.intDocumentoId)
                    .HasName("PK__tblDocum__3F16AAA03C25E228");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intApplication)
                    .WithMany(p => p.tblDocumento)
                    .HasForeignKey(d => d.intApplicationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDocume__intAp__6A32C6D7");

                entity.HasOne(d => d.intObjectType)
                    .WithMany(p => p.tblDocumento)
                    .HasForeignKey(d => d.intObjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDocume__intOb__693EA29E");
            });

            modelBuilder.Entity<tblDownloadApostilaQuestao_Log>(entity =>
            {
                entity.HasKey(e => new { e.intDownloadApostilaID, e.intQuestaoID });
            });

            modelBuilder.Entity<tblDownloadApostila_Log>(entity =>
            {
                entity.HasKey(e => e.intDownloadApostilaID);

                entity.Property(e => e.dteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtNomeArquivo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblDuvida>(entity =>
            {
                entity.HasKey(e => e.intDuvidaId)
                    .HasName("PK__tblDuvid__4526083A00E94E67");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDuvida)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblDuvidaImagem>(entity =>
            {
                entity.HasKey(e => e.intDuvidaImagemId)
                    .HasName("PK__tblDuvid__8A16C45607964BF6");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtNomeImagem)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblDuvidaRespostaProfessor>(entity =>
            {
                entity.HasKey(e => e.intDuvidaRespostaProfessorId)
                    .HasName("PK__tblDuvid__03CA1F690B66DCDA");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtResposta)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Denuncia>(entity =>
            {
                entity.HasKey(e => e.intDenunciaID);

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtComplemento).IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblDuvidasAcademicas_Denuncia)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Denuncia_tblPersons");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_Denuncia)
                    .HasForeignKey(d => d.intDuvidaID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Denuncia_tblDuvidas");

                entity.HasOne(d => d.intResposta)
                    .WithMany(p => p.tblDuvidasAcademicas_Denuncia)
                    .HasForeignKey(d => d.intRespostaID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Denuncia_tblRespostas");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_DuvidaApostila>(entity =>
            {
                entity.HasKey(e => e.intDuvidaApostilaId);

                entity.HasIndex(e => e.intDuvidaId)
                    .HasName("IX_tblDuvidasAcademicas_DuvidaApostila_intDuvidaId_D3032");

                entity.HasIndex(e => e.intMaterialApostilaId)
                    .HasName("IX_tblDuvidasAcademicas_DuvidaApostila_intMaterialApostilaId_E67AA");

                entity.Property(e => e.txtCodigoMarcacao).IsUnicode(false);

                entity.Property(e => e.txtTrecho).IsUnicode(false);

                entity.HasOne(d => d.tblDuvidasAcademicas_Duvidas)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidaApostila)
                    .HasForeignKey(d => d.intDuvidaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_DuvidaApostila_tblDuvidasAcademicas_Duvidas");

                entity.HasOne(d => d.intMaterialApostila)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidaApostila)
                    .HasForeignKey(d => d.intMaterialApostilaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_DuvidaApostila_tblMaterialApostila");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_DuvidaQuestao>(entity =>
            {
                entity.HasKey(e => e.intDuvidaQuestaoId)
                    .HasName("PK__tblDuvid__C37299DC57C33BC8");

                entity.Property(e => e.txtOrigemQuestaoConcurso)
                    .HasMaxLength(150);

                entity.HasIndex(e => new { e.intQuestaoId, e.intEspecialidadeID })
                    .HasName("idx_tblDuvidasAcademicas_DuvidaQuest99");

                entity.HasIndex(e => new { e.intDuvidaQuestaoId, e.intQuestaoId, e.intTipoQuestao, e.intExercicioId, e.intTipoExercicioID, e.intNumQuestao, e.intDuvidaID })
                    .HasName("IX_tblDuvidasAcademicas_DuvidaQuestao_intDuvidaID_8DA6F");

                entity.HasOne(d => d.tblDuvidasAcademicas_Duvidas)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidaQuestao)
                    .HasForeignKey(d => d.intDuvidaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intDu__6675784F");

                entity.HasOne(d => d.intEspecialidade)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidaQuestao)
                    .HasForeignKey(d => d.intEspecialidadeID)
                    .HasConstraintName("FK_tblduvidasacademicas_duvidaquestao_Especialidade");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Duvidas>(entity =>
            {
                entity.HasKey(e => e.intDuvidaID)
                    .HasName("PK__tblDuvid__4526085A6EF041CE");

                entity.HasIndex(e => new { e.intDuvidaID, e.bitAtiva })
                    .HasName("<IDX_DuvidasAcademicas_tblDuvidasAcademicas_Duvidas>");

                entity.HasIndex(e => new { e.intClientID, e.bitAtiva, e.bitAtivaDesenv, e.dteDataCriacao })
                    .HasName("idx_tblDuvidasAcademicas_Duvidas99");

                entity.Property(e => e.bitAtiva)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtCidadeFilial)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtCurso).IsUnicode(false);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtEstadoFake)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEstadoFilial)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtNomeFake)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtOrigem)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtOrigemSubnivel)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                
                entity.Property(e => e.txtOrigemProduto)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.intAcademico)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidasintAcademico)
                    .HasForeignKey(d => d.intAcademicoID)
                    .HasConstraintName("FKtblDuvidasAcademicas_Duvidas_tblPersonsAcademico");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidasintClient)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intCl__70D88A40");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_DuvidasEncaminhadas>(entity =>
            {
                entity.HasKey(e => e.intDuvidaEncaminhadaID);

                entity.Property(e => e.dteDataEncaminhamento).HasColumnType("datetime");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidasEncaminhadasintEmployee)
                    .HasForeignKey(d => d.intEmployeeID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_DuvidasEncaminhadas_tblEmployees2");

                entity.HasOne(d => d.intGestor)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidasEncaminhadasintGestor)
                    .HasForeignKey(d => d.intGestorID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_DuvidasEncaminhadas_tblEmployees");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_DuvidasHistorico>(entity =>
            {
                entity.HasKey(e => e.intDuvidaHistoricoID)
                    .HasName("PK_tblDuvidasAcademicas_HistoricoDuvidas");

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_DuvidasHistorico)
                    .HasForeignKey(d => d.intDuvidaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_HistoricoDuvidas_tblDuvidasAcademicas_Duvidas");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Interacoes>(entity =>
            {
                entity.HasKey(e => e.intInteracaoId)
                    .HasName("PK__tblDuvid__D7F2629A520A6272");

                entity.HasIndex(e => e.intClientID)
                    .HasName("IX_tblDuvidasAcademicas_Interacoes_intClientID_07BF4");

                entity.HasIndex(e => new { e.intDuvidaId, e.intVote })
                    .HasName("IX_tblDuvidasAcademicas_Interacoes_intDuvidaId_intVote_56634");

                entity.HasIndex(e => new { e.intRespostaId, e.intClientID, e.bitDenuncia })
                    .HasName("idx_tblDuvidasAcademicas_Interacoes98");

                entity.HasIndex(e => new { e.intRespostaId, e.intVote, e.intTipoInteracaoId })
                    .HasName("idx_tblDuvidasAcademicas_Interacoes97");

                entity.HasIndex(e => new { e.intRespostaId, e.intClientID, e.intVote, e.intTipoInteracaoId })
                    .HasName("idx_tblDuvidasAcademicas_Interacoes99");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_Interacoes)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intCl__53F2AAE4");

                entity.HasOne(d => d.intTipoInteracao)
                    .WithMany(p => p.tblDuvidasAcademicas_Interacoes)
                    .HasForeignKey(d => d.intTipoInteracaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intTi__67057F58");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Lidas>(entity =>
            {
                entity.HasKey(e => e.intLidaID)
                    .HasName("PK__tblDuvid__6B32C9E41421C67D");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_Lidas)
                    .HasForeignKey(d => d.intClientID)
                    .HasConstraintName("FK__tblDuvida__intCl__17F25761");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_Lidas)
                    .HasForeignKey(d => d.intDuvidaID)
                    .HasConstraintName("FK__tblDuvida__intDu__160A0EEF");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Log>(entity =>
            {
                entity.HasKey(e => e.intIDLog)
                    .HasName("PK__tblDuvid__C998CFC2001ACDD0");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtAcao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_Log)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intCl__03EB5EB4");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_Log)
                    .HasForeignKey(d => d.intDuvidaID)
                    .HasConstraintName("FK__tblDuvida__intDu__02031642");

                entity.HasOne(d => d.intResposta)
                    .WithMany(p => p.tblDuvidasAcademicas_Log)
                    .HasForeignKey(d => d.intRespostaID)
                    .HasConstraintName("FK__tblDuvida__intRe__02F73A7B");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Notificacao>(entity =>
            {
                entity.HasKey(e => e.intDuvidaAcademicaNotificacaoId);

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_Notificacao)
                    .HasForeignKey(d => d.intClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Notificacao_tblPersons");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_Notificacao)
                    .HasForeignKey(d => d.intDuvidaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Notificacao_tblDuvidasAcademicas_Duvidas");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_Resposta>(entity =>
            {
                entity.HasKey(e => e.intRespostaID)
                    .HasName("PK__tblDuvid__603D8C9176916396");

                entity.HasIndex(e => e.intParentRespostaID)
                    .HasName("idx_tblDuvidasAcademicas_Resposta99");

                entity.HasIndex(e => new { e.intDuvidaID, e.intParentRespostaID })
                    .HasName("idx_tblDuvidasAcademicas_Resposta98");

                entity.Property(e => e.bitAprovacaoMedgrupo).HasDefaultValueSql("((0))");

                entity.Property(e => e.bitAtiva)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.intMedGrupoID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtCidadeFilial)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtCurso).IsUnicode(false);

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtEstadoFake)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEstadoFilial)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtNomeFake)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intAcademico)
                    .WithMany(p => p.tblDuvidasAcademicas_RespostaintAcademico)
                    .HasForeignKey(d => d.intAcademicoID)
                    .HasConstraintName("FKtblDuvidasAcademicas_Resposta_tblPersonsAcademico");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblDuvidasAcademicas_RespostaintClient)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intCl__796DD041");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblDuvidasAcademicas_Resposta)
                    .HasForeignKey(d => d.intDuvidaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDuvida__intDu__7879AC08");

                entity.HasOne(d => d.intParentResposta)
                    .WithMany(p => p.InverseintParentResposta)
                    .HasForeignKey(d => d.intParentRespostaID)
                    .HasConstraintName("FK_tblDuvidasAcademicas_Resposta_tblDuvidasAcademicas_Resposta");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_RespostaHistorico>(entity =>
            {
                entity.HasKey(e => e.intRespostaHistoricoID);

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.intResposta)
                    .WithMany(p => p.tblDuvidasAcademicas_RespostaHistorico)
                    .HasForeignKey(d => d.intRespostaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDuvidasAcademicas_RespostaHistorico_tblDuvidasAcademicas_Resposta");
            });

            modelBuilder.Entity<tblDuvidasAcademicas_TipoInteracao>(entity =>
            {
                entity.HasKey(e => e.intTipoInteracaoId)
                    .HasName("PK__tblDuvid__B51D75BA6334EE74");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblEditoras>(entity =>
            {
                entity.Property(e => e.txtEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtSite)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtTelefone)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblEmailConteudo>(entity =>
            {
                entity.HasKey(e => e.intEmailConteudoID)
                    .HasName("PK__tblEmail__E2789B73040CE037");

                entity.Property(e => e.intAplicacaoId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtConteudo).IsUnicode(false);
            });

            modelBuilder.Entity<tblEmailNotificacaoDuvidasAcademicas>(entity =>
            {
                entity.HasKey(e => e.intEmailDuvidasID);

                entity.Property(e => e.txtEmail).IsUnicode(false);

                entity.Property(e => e.txtName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblEmailNotificacaoDuvidasAcademicas)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblEmailNotificacaoDuvidasAcademicas_tblPersons");
            });

            modelBuilder.Entity<tblEmed_AccessDenied>(entity =>
            {
                entity.HasKey(e => e.intEmedId);

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(500);
            });

            modelBuilder.Entity<tblEmed_AccessDenied_LOG>(entity =>
            {
                entity.HasKey(e => e.intAccessDeniedId)
                    .HasName("PK_intEmedAccessDeniedId")
                    .IsClustered(false);

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(500);
            });

            modelBuilder.Entity<tblEmed_AccessGolden>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.txtCPF)
                    .HasName("_net_IX_tblEmed_AccessGolden_txtCPF");

                entity.HasIndex(e => new { e.intID, e.txtCPF, e.txtComment, e.intEmployeeID, e.dteDateTime, e.bitEterno })
                    .HasName("_net_IX_tblEmed_AccessGolden_bitEterno");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtCPF)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.txtComment)
                    .HasMaxLength(300)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblEmed_AccessGolden_log>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtAcao)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('INCLUSAO')");

                entity.Property(e => e.txtCPF)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.txtComment)
                    .HasMaxLength(300)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblEmed_AcessoConcorrente_Log>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblEmed___11B67932518D2255");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtBrowser)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtIP)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.txtSessionID)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblEmed_Duvidas>(entity =>
            {
                entity.HasKey(e => e.idEmed_Duvida)
                    .HasName("PK_tblEmed_Duvidas_dsv");

                entity.Property(e => e.bitAtiva).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDuvida_Enviada).HasColumnType("datetime");

                entity.Property(e => e.txtTexto_Duvida).IsRequired();

                entity.Property(e => e.txtUID).HasMaxLength(200);

                entity.Property(e => e.txtURLImagem).HasMaxLength(255);
            });

            modelBuilder.Entity<tblEmed_SessoesAtivas>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblEmed___11B679325FD4D9FD");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtBrowser)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtIP)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.txtSessionID)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblEmployeeCargos>(entity =>
            {
                entity.HasKey(e => e.intCargoID);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblEmployee_Sector>(entity =>
            {
                entity.HasKey(e => new { e.intEmployeeID, e.intSectorID });

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblEmployee_Sector)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployee_Sector_tblEmployees");
            });

            modelBuilder.Entity<tblEmployees>(entity =>
            {
                entity.HasKey(e => e.intEmployeeID)
                    .HasName("PK_tblEmployee");

                entity.HasIndex(e => e.intEmployeeID)
                    .HasName("_dta_index_tblEmployees_8_1660077200__K1");

                entity.HasIndex(e => e.intStoreID)
                    .HasName("_net_IX_tblEmployees_003");

                entity.HasIndex(e => new { e.intEmployeeID, e.intCargo })
                    .HasName("_net_IX_tblEmployees_intCargo");

                entity.HasIndex(e => new { e.txtLogin, e.txtPassword })
                    .HasName("_net_IX_tblEmployees_txtLogin_txtPassword");

                entity.HasIndex(e => new { e.intEmployeeID, e.intGestorID, e.intStatus })
                    .HasName("IX_tblEmployees_intStatus_5A1D2");

                entity.HasIndex(e => new { e.intEmployeeID, e.intResponsabilityID, e.intStatus })
                    .HasName("_net_IX_tblEmployees_001");

                entity.HasIndex(e => new { e.intEmployeeID, e.intSectorID, e.bitActiveEmployee })
                    .HasName("_net_IX_tblEmployees_002");

                entity.HasIndex(e => new { e.intEmployeeID, e.txtLogin, e.intResponsabilityID })
                    .HasName("_net_IX_tblEmployees_intResponsabilityID");

                entity.HasIndex(e => new { e.intEmployeeID, e.intSectorID, e.txtLogin, e.intStatus })
                    .HasName("_net_IX_tblEmployees_intSectorID");

                entity.HasIndex(e => new { e.intEmployeeID, e.intStoreID, e.intSectorID, e.intStatus })
                    .HasName("_net_IX_tblEmployees_intSectorID_intStatus");

                entity.Property(e => e.intEmployeeID)
                    .HasComment("Relação com tblPersons")
                    .ValueGeneratedNever();

                entity.Property(e => e.dteDateContrato).HasColumnType("datetime");

                entity.Property(e => e.dteDateDesligamento).HasColumnType("datetime");

                entity.Property(e => e.dteDateExame).HasColumnType("datetime");

                entity.Property(e => e.dteDateFirstDay).HasColumnType("datetime");

                entity.Property(e => e.dteDateSavePassword).HasColumnType("datetime");

                entity.Property(e => e.dteDateTCEMaturity).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.dteDateTwoYearsCompletion).HasColumnType("datetime");

                entity.Property(e => e.txtBankAccountInformation).HasMaxLength(500);

                entity.Property(e => e.txtBankAccountInformation2).HasMaxLength(500);

                entity.Property(e => e.txtCollegeConclusion).HasMaxLength(500);

                entity.Property(e => e.txtCollegeName).HasMaxLength(500);

                entity.Property(e => e.txtCollegePeriod).HasMaxLength(500);

                entity.Property(e => e.txtConscriptionDocument).HasMaxLength(500);

                entity.Property(e => e.txtCourse).HasMaxLength(500);

                entity.Property(e => e.txtLogin)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtMatriculationStatement).HasMaxLength(500);

                entity.Property(e => e.txtPIS).HasMaxLength(40);

                entity.Property(e => e.txtPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtSerie).HasMaxLength(50);

                entity.Property(e => e.txtSocialCard).HasMaxLength(30);

                entity.Property(e => e.txtVoterRegistrationCardDocument).HasMaxLength(500);

                entity.Property(e => e.txtWorkCardDocument).HasMaxLength(500);

                entity.HasOne(d => d.intGestor)
                    .WithMany(p => p.InverseintGestor)
                    .HasForeignKey(d => d.intGestorID)
                    .HasConstraintName("FK_tblEmployees_tblEmployees1");

                entity.HasOne(d => d.intResponsability)
                    .WithMany(p => p.tblEmployees)
                    .HasForeignKey(d => d.intResponsabilityID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployees_tblSysRoles");

                entity.HasOne(d => d.intCargoNavigation)
                    .WithMany(p => p.tblEmployees)
                    .HasForeignKey(d => d.intCargo)
                    .HasConstraintName("FK_tblEmployees_tblEmployeeCargos");
            });

            modelBuilder.Entity<tblEnderecoEntregaCliente>(entity =>
            {
                entity.HasKey(e => e.IdEnderecoEntrega)
                    .HasName("PK__tblEnder__F008D16071BDF204");

                entity.Property(e => e.txtBairro).HasMaxLength(100);

                entity.Property(e => e.txtComplementoEndereco).HasMaxLength(100);

                entity.Property(e => e.txtEndereco).HasMaxLength(100);

                entity.Property(e => e.txtZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.tblEnderecoEntregaCliente)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdCliente");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblEnderecoEntregaCliente)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_intCityID");
            });

            modelBuilder.Entity<tblEspecialidadeProfessor>(entity =>
            {
                entity.HasKey(e => e.intEspecialidadeProfessorID);

                entity.Property(e => e.txtEspecialidade)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblEspecialidadeProfessor)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblEspecialidadeProfessor_tblPersons");

                entity.HasOne(d => d.intEspecialidade)
                    .WithMany(p => p.tblEspecialidadeProfessor)
                    .HasForeignKey(d => d.intEspecialidadeID)
                    .HasConstraintName("FK_tblEspecialidadeProfessor_tblEspecialidade");
            });

            modelBuilder.Entity<tblEspecialidades>(entity =>
            {
                entity.HasKey(e => e.intEspecialidadeID)
                    .HasName("PK_tblEspecialidades_1");

                entity.HasIndex(e => e.CD_ESPECIALIDADE)
                    .HasName("IX_tblEspecialidades_unique")
                    .IsUnique();

                entity.Property(e => e.CD_AREA)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CD_ESPECIALIDADE)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CONCURSO)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DE_ESPECIALIDADE)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.INSCRICAO)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblExercicio_MontaProva>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblExerc__11B67932326C87C7");

                entity.HasIndex(e => new { e.intID, e.intFiltroId })
                    .HasName("IX_tblExercicio_MontaProva_intFiltroId_60E37");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtNome)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblExpectedGraduationTermCatalog>(entity =>
            {
                entity.HasKey(e => e.intGraduationPeriodID);

                entity.Property(e => e.intGraduationPeriodID).ValueGeneratedNever();

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblFTPConfig>(entity =>
            {
                entity.HasKey(e => e.intFtpID);

                entity.Property(e => e.txtApplication).HasMaxLength(50);

                entity.Property(e => e.txtFtpPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtFtpPath).HasMaxLength(50);

                entity.Property(e => e.txtFtpServer)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtFtpUser)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtFtpVirtualPath)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.txtUrl).HasMaxLength(50);
            });

            modelBuilder.Entity<tblFormulas_Medme>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtCondicao).HasMaxLength(400);

                entity.Property(e => e.txtFormula).HasMaxLength(400);
            });

            modelBuilder.Entity<tblFuncionalidade>(entity =>
            {
                entity.HasKey(e => e.intFuncionalidadeID)
                    .HasName("PK__tblFunci__2EAEAFAB25CA26D9");

                entity.Property(e => e.txtAliasFuncionalidade)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.txtURI)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.intTela)
                    .WithMany(p => p.tblFuncionalidade)
                    .HasForeignKey(d => d.intTelaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFuncionalidade_tblTelas");
            });

            modelBuilder.Entity<tblFuncionalidade1>(entity =>
            {
                entity.HasKey(e => e.intFuncionalidadeID)
                    .HasName("PK__tblFunci__2EAEAFAB04422B62");

                entity.ToTable("tblFuncionalidade", "MEDBARRA\\alysson.silva");

                entity.Property(e => e.txtAliasFuncionalidade)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.txtURI)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.intTela)
                    .WithMany(p => p.tblFuncionalidade1)
                    .HasForeignKey(d => d.intTelaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFuncionalidade_tblTelas");
            });

            modelBuilder.Entity<tblGaleriaImagem>(entity =>
            {
                entity.HasKey(e => e.intGaleriaImagemId)
                    .HasName("PK__tblGaler__D27755E33BE64169");

                entity.Property(e => e.dteCadastro).HasColumnType("smalldatetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(1000);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblGaleriaImagem)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intEm__3D51DD74");
            });

            modelBuilder.Entity<tblGaleriaImagemApostila>(entity =>
            {
                entity.HasKey(e => e.intGaleriaImagemApostilaId)
                    .HasName("PK__tblGaler__9CD832A840AAF686");

                entity.Property(e => e.dteCadastro).HasColumnType("smalldatetime");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblGaleriaImagemApostila)
                    .HasForeignKey(d => d.intBookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intBo__3E4601AD");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblGaleriaImagemApostila)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intEm__3F3A25E6");

                entity.HasOne(d => d.intGaleriaImagem)
                    .WithMany(p => p.tblGaleriaImagemApostila)
                    .HasForeignKey(d => d.intGaleriaImagemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intGa__402E4A1F");
            });

            modelBuilder.Entity<tblGaleriaRelacaoImagem>(entity =>
            {
                entity.HasKey(e => e.intRelacaoId)
                    .HasName("PK__tblGaler__221B712173016C29");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblGaleriaRelacaoImagem)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intEm__41226E58");

                entity.HasOne(d => d.intGaleriaImagem)
                    .WithMany(p => p.tblGaleriaRelacaoImagem)
                    .HasForeignKey(d => d.intGaleriaImagemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intGa__42169291");

                entity.HasOne(d => d.intImagem)
                    .WithMany(p => p.tblGaleriaRelacaoImagem)
                    .HasForeignKey(d => d.intImagemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGaleri__intIm__430AB6CA");
            });

            modelBuilder.Entity<tblGrupo>(entity =>
            {
                entity.HasKey(e => e.intGrupoID);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblGrupo)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblGrupo_tblPersons");
            });

            modelBuilder.Entity<tblImagemGaleria>(entity =>
            {
                entity.HasKey(e => e.intImagemId)
                    .HasName("PK__tblImage__7203AAA06E3CB70C");

                entity.Property(e => e.dteCadastro).HasColumnType("smalldatetime");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtFilename)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblImagemGaleria)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblImagem__intEm__43FEDB03");
            });

            modelBuilder.Entity<tblImagemSemana>(entity =>
            {
                entity.HasKey(e => e.intImagemSemanaID);

                entity.Property(e => e.dteImagemSemana).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.txtImagemGrande)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtImagemPequena)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtImagemResposta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtResposta)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.txtThumb)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblImagemSemanaRespostaAluno>(entity =>
            {
                entity.HasKey(e => e.intImagemSemanaRespostaAlunoID);

                entity.HasIndex(e => new { e.bitLiberarResposta, e.intImagemSemanaID })
                    .HasName("_net_IX_tblImagemSemanaRespostaAluno_bitLiberarResposta_intImagemSemanaID");

                entity.HasIndex(e => new { e.intImagemSemanaRespostaAlunoID, e.intContactID, e.dteRespostaAluno, e.txtDescricao, e.bitLiberarResposta, e.intImagemSemanaID })
                    .HasName("_net_IX_tblImagemSemanaRespostaAluno_intImagemSemanaID");

                entity.Property(e => e.dteRespostaAluno).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<tblImpostoDeRenda>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dbBaseCalculo).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.dbDeducao).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.intTipoAliquotaNavigation)
                    .WithMany(p => p.tblImpostoDeRenda)
                    .HasForeignKey(d => d.intTipoAliquota)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblImpostoDeRenda_tblImpostoDeRendaAliquota");
            });

            modelBuilder.Entity<tblImpostoDeRendaAliquota>(entity =>
            {
                entity.HasKey(e => e.intTipoAliquota);

                entity.Property(e => e.txtAliquota)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<tblInscricao_EadCadastro>(entity =>
            {
                entity.HasKey(e => e.intEadEmailId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.guidSession)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblInscricao_MedCadastro>(entity =>
            {
                entity.HasKey(e => e.intMedEmailId);

                entity.HasIndex(e => e.guidSession)
                    .HasName("_net_IX_tblInscricao_MedCadastro_guidSession");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.guidSession)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblInscricoesBloqueios>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteDateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.dteInclusaoBloqueio)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtMotivo).IsUnicode(false);

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(500);

                entity.Property(e => e.txtRegister)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.HasOne(d => d.intType)
                    .WithMany(p => p.tblInscricoesBloqueios)
                    .HasForeignKey(d => d.intTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblInscricoesBloqueios_tblInscricoesBloqueiosTipos");
            });

            modelBuilder.Entity<tblInscricoesBloqueiosTipos>(entity =>
            {
                entity.HasKey(e => e.intTypeID);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtDetails).IsRequired();
            });

            modelBuilder.Entity<tblInscricoesBloqueios_Log>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblInscr__11B6793218AAF614");

                entity.Property(e => e.dteDateTimeEnd)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteInclusaoBloqueio).HasColumnType("datetime");

                entity.Property(e => e.txtMotivoDesbloqueio).HasMaxLength(500);

                entity.Property(e => e.txtRegister)
                    .IsRequired()
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<tblInscricoesCampanhaMkt>(entity =>
            {
                entity.HasKey(e => e.intInscricoesCampanhaMktID)
                    .HasName("PK__tblInscr__5561610F63A32916");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.HasOne(d => d.intTipoCampanhaMktNavigation)
                    .WithMany(p => p.tblInscricoesCampanhaMkt)
                    .HasForeignKey(d => d.intTipoCampanhaMkt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoCampanhaMkt");
            });

            modelBuilder.Entity<tblInscricoesCampanhaMktTipo>(entity =>
            {
                entity.HasKey(e => e.intTipoCampanhaMktID)
                    .HasName("PK__tblInscr__207ACC2F5FD29832");

                entity.Property(e => e.txtTipo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblInscricoesRessalvas>(entity =>
            {
                entity.HasKey(e => e.intMsgID);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtMsgText).IsRequired();

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblInscricoesRessalvas)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblInscricoesRessalvas_tblProductGroups1");

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblInscricoesRessalvas)
                    .HasForeignKey(d => d.intStoreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblInscricoesRessalvas_tblStores");
            });

            modelBuilder.Entity<tblInscricoes_Log>(entity =>
            {
                entity.HasKey(e => e.intInscricoes_LogID);

                entity.HasOne(d => d.intDeviceTipo)
                    .WithMany(p => p.tblInscricoes_Log)
                    .HasForeignKey(d => d.intDeviceTipoID)
                    .HasConstraintName("FK_tblInscricoes_Log_tblAccess_Device");
            });

            modelBuilder.Entity<tblInstrucaoPostagemCheque>(entity =>
            {
                entity.HasKey(e => e.intInstrucaoPostagemChequeId)
                    .HasName("PK__tblInstr__D293F927B5B63502");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtLinkPDF)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblIntensivaoLog>(entity =>
            {
                entity.HasKey(e => e.intIntensivaoLogID);

                entity.Property(e => e.dteTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.txtPerfil)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblIntensivao_Cronograma>(entity =>
            {
                entity.HasKey(e => e.codIntensivao)
                    .HasName("PK__tblIntensivao_Cr__1B7714D5");

                entity.HasIndex(e => e.intStoreID)
                    .HasName("UQ__tblIntensivao_Cr__1C6B390E")
                    .IsUnique();

                entity.Property(e => e.dia_semana).HasMaxLength(30);

                entity.Property(e => e.endereco).IsUnicode(false);

                entity.Property(e => e.formato).HasMaxLength(50);

                entity.Property(e => e.horario)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(N'09:00h às 21:00h')");

                entity.Property(e => e.intLocal).HasMaxLength(50);

                entity.Property(e => e.localprova).HasMaxLength(50);

                entity.Property(e => e.periodo).HasColumnType("text");

                entity.Property(e => e.txtCidade).HasMaxLength(100);

                entity.Property(e => e.txtObs)
                    .HasMaxLength(3500)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblLabelDetails>(entity =>
            {
                entity.HasKey(e => e.intLabelDetailID);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intLabel)
                    .WithMany(p => p.tblLabelDetails)
                    .HasForeignKey(d => d.intLabelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLabelDetails_tbl_Labels");
            });

            modelBuilder.Entity<tblLabelGroups>(entity =>
            {
                entity.HasKey(e => e.intLabelGroupID);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblLabel_SmartInfo>(entity =>
            {
                entity.HasKey(e => e.intLabelSmartInfoId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtRegraEntrada).IsUnicode(false);

                entity.Property(e => e.txtRegraSaida).IsUnicode(false);
            });

            modelBuilder.Entity<tblLabels>(entity =>
            {
                entity.HasKey(e => e.intLabelID);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtColor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblLabels)
                    .HasForeignKey(d => d.intContactID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLabel_tblPersons");

                entity.HasOne(d => d.intLabelGroup)
                    .WithMany(p => p.tblLabels)
                    .HasForeignKey(d => d.intLabelGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLabel_tblLabelGroups");
            });

            modelBuilder.Entity<tblLessonsEvaluationProvaVideo>(entity =>
            {
                entity.HasKey(e => e.intLessonsEvaluationId)
                    .HasName("PK__tblLesso__1D841FBE2552521D");

                entity.Property(e => e.dteEvaluation).HasColumnType("datetime");

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.intProvaVideo)
                    .WithMany(p => p.tblLessonsEvaluationProvaVideo)
                    .HasForeignKey(d => d.intProvaVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblLesson__intPr__28486567");
            });

            modelBuilder.Entity<tblLessonEvaluationVideoAula>(entity =>
            {
                entity.HasKey(e => e.intLessonEvaluationVideoAulaId)
                    .HasName("PK_tblLessonsEvaluationVideoAula");

                entity.HasIndex(e => new { e.intClientId, e.intTipoVideo })
                    .HasName("_net_IX_tblLessonEvaluationVideoAula_intClientId_intTipoVideo");

                entity.HasIndex(e => new { e.intClientId, e.intEmployeeId, e.intTipoVideo })
                    .HasName("_net_IX_tblLessonEvaluationVideoAula_intClientId");

                entity.Property(e => e.dteEvaluation).HasColumnType("datetime");

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblLessonRessalva>(entity =>
            {
                entity.HasKey(e => e.intLessonRessalvaID);

                entity.Property(e => e.txtAssunto)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtRessalva)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.intLesson)
                    .WithMany(p => p.tblLessonRessalva)
                    .HasForeignKey(d => d.intLessonID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_tblLessonRessalva_tblLessons1");
            });

            modelBuilder.Entity<tblLessonTeacherSubstituto>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblLessonTeacherSubstituto)
                    .HasForeignKey(d => d.intEmployeeID)
                    .HasConstraintName("FK_tblLessonTeacherSubstituto_tblEmployees");

                entity.HasOne(d => d.intLesson)
                    .WithMany(p => p.tblLessonTeacherSubstituto)
                    .HasForeignKey(d => d.intLessonID)
                    .HasConstraintName("FK_tblLessonTeacherSubstituto_tblLessons");
            });

            modelBuilder.Entity<tblLessonTitleRevalida>(entity =>
            {
                entity.HasKey(e => e.intLessonTitleRevalidaId);

                entity.Property(e => e.txtName).HasMaxLength(200);
            });

            modelBuilder.Entity<tblLessonsEvaluationProvaVideo>(entity =>
            {
                entity.HasKey(e => e.intLessonsEvaluationId)
                    .HasName("PK__tblLesso__1D841FBE2552521D");

                entity.Property(e => e.dteEvaluation).HasColumnType("datetime");

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.intProvaVideo)
                    .WithMany(p => p.tblLessonsEvaluationProvaVideo)
                    .HasForeignKey(d => d.intProvaVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblLesson__intPr__28486567");
            });

            modelBuilder.Entity<tblLessonTitles>(entity =>
            {
                entity.HasKey(e => e.intLessonTitleID);

                entity.HasIndex(e => new { e.intLessonTitleID, e.txtLessonTitleName, e.intSemana })
                    .HasName("_net_IX_tblLessonTitles_intSemana");

                entity.HasIndex(e => new { e.txtLessonTitleName, e.intLessonSubjectID, e.intAno, e.intSemana, e.intLessonTitleID })
                    .HasName("_dta_index_tblLessonTitles_8_76565544__K1_2_3_4_5");

                entity.HasIndex(e => new { e.intLessonTitleID, e.txtLessonTitleName, e.intAno, e.intSemana, e.intLessonTitleEntityID, e.intLessonSubjectID })
                    .HasName("_net_IX_tblLessonTitles_intLessonSubjectID_2");

                entity.HasIndex(e => new { e.intLessonTitleID, e.txtLessonTitleName, e.intLessonSubjectID, e.intSemana, e.intLessonTitleEntityID, e.intAno })
                    .HasName("_net_IX_tblLessonTitles_intAno");

                entity.Property(e => e.txtLessonTitleName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<tblLessonTypes>(entity =>
            {
                entity.HasKey(e => e.intLessonType);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblLesson_Material>(entity =>
            {
                entity.HasKey(e => new { e.intLessonID, e.intMaterialID });

                entity.HasIndex(e => e.intLessonID)
                    .HasName("_dta_index_tblLesson_Material_5_574625090__K1");

                entity.HasIndex(e => e.intMaterialID)
                    .HasName("_dta_index_tblLesson_Material_5_574625090__K2");

                entity.HasIndex(e => new { e.intLessonID, e.intMaterialID })
                    .HasName("tblLesson_Material_intLessonId_Include_Queryprocessor");

                entity.HasIndex(e => new { e.intMaterialID, e.intLessonID })
                    .HasName("_dta_index_tblLesson_Material_5_574625090__K2_K1");

                entity.HasOne(d => d.intLesson)
                    .WithMany(p => p.tblLesson_Material)
                    .HasForeignKey(d => d.intLessonID)
                    .HasConstraintName("FK_tblLesson_Material_tblLessons");

                entity.HasOne(d => d.intMaterial)
                    .WithMany(p => p.tblLesson_Material)
                    .HasForeignKey(d => d.intMaterialID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLesson_Material_tblProducts");
            });

            modelBuilder.Entity<tblLessons>(entity =>
            {
                entity.HasKey(e => e.intLessonID);

                entity.HasIndex(e => new { e.bitInvitedCoursesReplication, e.dteDateTime, e.intLessonID, e.intClassRoomID, e.intCourseID })
                    .HasName("_dta_index_tblLessons_5_847394138__K5_K1_K8_K2");

                entity.HasIndex(e => new { e.intClassRoomID, e.intCourseID, e.dteDateTime, e.intLessonType, e.intLessonID })
                    .HasName("_dta_index_tblLessons_5_847394138__K2_K5_K10_K1");

                entity.HasIndex(e => new { e.dteDateTime, e.intCourseID, e.intClassRoomID, e.intLessonSubjectID, e.intLessonID, e.intLessonTitleID, e.intSequence })
                    .HasName("_net_IX_tblLessons_intCourseID_intClassRoomID_intLessonSubjectID");

                entity.HasIndex(e => new { e.dteDateTime, e.intLessonID, e.intCourseID, e.intClassRoomID, e.intLessonSubjectID, e.intLessonTitleID, e.intSequence })
                    .HasName("_net_IX_tblLessons_intLessonID_intCourseID_intClassRoomID");

                entity.HasIndex(e => new { e.intClassRoomID, e.intLessonType, e.bitInvitedCoursesReplication, e.dteDateTime, e.intCourseID, e.intLessonTitleID, e.intLessonID })
                    .HasName("_net_IX_tblLessons_intClassRoomID_intLessonType");

                entity.HasIndex(e => new { e.intSequence, e.dteDateTime, e.intDuration, e.intLessonSubjectID, e.bitInvitedCoursesReplication, e.intLessonType, e.intCourseID, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_tblLessons_5_847394138__K2_K1_K4_3_5_6_7_9_10");

                entity.HasIndex(e => new { e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intLessonID, e.bitInvitedCoursesReplication, e.intDuration, e.intClassRoomID, e.intSequence, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_tblLessons_intClassRoomID");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.dteDateTime, e.intDuration, e.intLessonSubjectID, e.intClassRoomID, e.bitInvitedCoursesReplication, e.intLessonType, e.intLessonID, e.intLessonTitleID })
                    .HasName("_dta_index_tblLessons_8_847394138__K1_K4_2_3_5_6_7_8_9_10");

                entity.HasIndex(e => new { e.intCourseID, e.intSequence, e.intLessonTitleID, e.dteDateTime, e.intClassRoomID, e.intLessonID, e.intDuration, e.intLessonSubjectID, e.bitInvitedCoursesReplication, e.intLessonType })
                    .HasName("_net_IX_tblLessons_intLessonType");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.dteDateTime, e.intClassRoomID, e.bitInvitedCoursesReplication, e.intLessonTitleID, e.intSequence, e.intDuration, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_intLessonType_intLessonSubjectID_intLessonType");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.bitInvitedCoursesReplication, e.intClassRoomID, e.intSequence, e.intLessonSubjectID, e.intLessonType })
                    .HasName("_net_IX_tblLessons_intSequence_intLessonSubjectID");

                entity.HasIndex(e => new { e.intLessonID, e.intCourseID, e.intSequence, e.dteDateTime, e.intDuration, e.intLessonSubjectID, e.intClassRoomID, e.bitInvitedCoursesReplication, e.intLessonTitleID, e.intLessonType })
                    .HasName("_net_IX_tblLessons_intLessonTitleID_intLessonType");

                entity.HasIndex(e => new { e.intSequence, e.intLessonTitleID, e.dteDateTime, e.intDuration, e.intLessonSubjectID, e.bitInvitedCoursesReplication, e.intLessonType, e.intCourseID, e.intClassRoomID, e.intLessonID })
                    .HasName("_net_IX_tblLessons_intCourseID_1");

                entity.Property(e => e.bitInvitedCoursesReplication).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.intClassRoom)
                    .WithMany(p => p.tblLessons)
                    .HasForeignKey(d => d.intClassRoomID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLessons_tblClassRooms");

                entity.HasOne(d => d.intCourse)
                    .WithMany(p => p.tblLessons)
                    .HasForeignKey(d => d.intCourseID)
                    .HasConstraintName("FK_tblLessons_tblCourses");
            });

            modelBuilder.Entity<tblLessonsEvaluation>(entity =>
            {
                entity.HasKey(e => e.intEvaluationID);

                entity.HasIndex(e => new { e.intEvaluationID, e.dteEvaluationDate, e.intLessonID, e.intClientID })
                    .HasName("_net_IX_tblLessonsEvaluation_intLessonID");

                entity.HasIndex(e => new { e.intLessonID, e.intClientID, e.intEmployeedID, e.intProductGroup1ID, e.txtObservacao, e.intNota })
                    .HasName("IX_tblLessonsEvaluation_intNota_E1DBC");

                entity.HasIndex(e => new { e.intClassroomID, e.intNota, e.intApplicationID, e.dteEvaluationDate, e.txtObservacao, e.intClientID, e.intEvaluationID, e.intBookID, e.intProductGroup1ID, e.intLessonID, e.intEmployeedID })
                    .HasName("_dta_index_tblLessonsEvaluation_8_1751085511__K5_K1_K2_K8_K3_K7_4_6_9_10_11");

                entity.HasIndex(e => new { e.intLessonID, e.intClientID, e.intEmployeedID, e.intProductGroup1ID, e.txtObservacao, e.intEvaluationID, e.intClassroomID, e.intApplicationID, e.dteEvaluationDate, e.intBookID, e.intNota })
                    .HasName("_net_IX_tblLessonsEvaluation_intBookID_intNota");

                entity.Property(e => e.dteEvaluationDate).HasColumnType("datetime");

                entity.Property(e => e.txtObservacao).HasColumnType("varchar(max)");

                entity.HasOne(d => d.intApplication)
                    .WithMany(p => p.tblLessonsEvaluation)
                    .HasForeignKey(d => d.intApplicationID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblAccess_Application");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblLessonsEvaluation)
                    .HasForeignKey(d => d.intBookID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblBooks");

                entity.HasOne(d => d.intClassroom)
                    .WithMany(p => p.tblLessonsEvaluation)
                    .HasForeignKey(d => d.intClassroomID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblClassRooms");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblLessonsEvaluationintClient)
                    .HasForeignKey(d => d.intClientID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblPersons");

                entity.HasOne(d => d.intEmployeed)
                    .WithMany(p => p.tblLessonsEvaluationintEmployeed)
                    .HasForeignKey(d => d.intEmployeedID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblPersons1");

                entity.HasOne(d => d.intLesson)
                    .WithMany(p => p.tblLessonsEvaluation)
                    .HasForeignKey(d => d.intLessonID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblLessons");

                entity.HasOne(d => d.intProductGroup1)
                    .WithMany(p => p.tblLessonsEvaluation)
                    .HasForeignKey(d => d.intProductGroup1ID)
                    .HasConstraintName("FK_tblLessonsEvaluation_tblProductGroups1");
            });

            modelBuilder.Entity<tblLessonsEvaluationRevisaoAula>(entity =>
            {
                entity.HasKey(e => e.intLessonsEvaluationRaId);

                entity.HasIndex(e => new { e.intNota, e.intVideoId })
                    .HasName("IX_tblLessonsEvaluationRevisaoAula_intVideoId_9ACA2");

                entity.HasIndex(e => new { e.intNota, e.intEmployeeId, e.intRevisaoAulaIndiceId, e.intClientId })
                    .HasName("_net_IX_tblLessonsEvaluationRevisaoAula_intRevisaoAulaIndiceId");

                entity.Property(e => e.dteEvaluation).HasColumnType("datetime");

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblLessonsTotalEvaluationAuxiliar>(entity =>
            {
                entity.HasKey(e => e.intId);
            });

            modelBuilder.Entity<tblLessonxRessalva>(entity =>
            {
                entity.HasKey(e => e.intLessonxRessalvaID);

                entity.Property(e => e.dteDataInicio)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2017-04-24 00:00:00.000')");

                entity.HasOne(d => d.intLesson)
                    .WithMany(p => p.tblLessonxRessalva)
                    .HasForeignKey(d => d.intLessonID)
                    .HasConstraintName("FK_tblLessonxRessalva_tblLessons");

                entity.HasOne(d => d.intLessonRessalva)
                    .WithMany(p => p.tblLessonxRessalva)
                    .HasForeignKey(d => d.intLessonRessalvaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLessonxRessalva_tblLessonRessalva");
            });

            modelBuilder.Entity<tblLiberacaoApostila>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoApostilaId)
                    .HasName("PK_intLiberacaoApostilaId");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblLiberacaoApostila)
                    .HasForeignKey(d => d.intBookId)
                    .HasConstraintName("FK_intBookId");
            });

            modelBuilder.Entity<tblLiberacaoApostila1>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoApostilaId)
                    .HasName("PK_intLiberacaoApostilaId");

                entity.ToTable("tblLiberacaoApostila", "MEDBARRA\\murilo.filho");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblLiberacaoApostila1)
                    .HasForeignKey(d => d.intBookId)
                    .HasConstraintName("FK_intBookId");
            });

            modelBuilder.Entity<tblLiberacaoApostilaAntecipada>(entity =>
            {
                entity.HasKey(e => e.intLiberacaoApostilaAntecipadaID);

                entity.Property(e => e.dteDataCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblLiberacaoApostilaAntecipada)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblLiberacaoApostilaAntecipada_tblPersons");
            });

            modelBuilder.Entity<tblLiberacaoApostila_Historico>(entity =>
            {
                entity.HasKey(e => e.IntID)
                    .HasName("PK_dbo.tbl_LiberacaoApostila_Historico");

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.txtAcao)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.txtUsuarioBD)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblLinkEsqueciSenha>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblLinkE__11B678D2BA85A1E2");

                entity.Property(e => e.txtLink).HasMaxLength(400);
            });

            modelBuilder.Entity<tblLocaisRetiradaMaterial>(entity =>
            {
                entity.Property(e => e.dteDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblLogAcessoLogin>(entity =>
            {
                entity.HasKey(e => e.intLogAcessoLoginID);

                entity.Property(e => e.dteDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblLogAcoesSimuladoImpresso>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblLogAc__11B679322DE07A51");

                entity.HasIndex(e => new { e.intClientID, e.intSimuladoID, e.intAplicationID })
                    .HasName("IX_tblLogAcoesSimuladoImpresso_intSimuladoID_intAplicationID_intClientID");

                entity.Property(e => e.dteData)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tblLogClientInscricaoInadimplente>(entity =>
            {
                entity.HasKey(e => e.intLogID);

                entity.HasIndex(e => e.intClientID)
                    .HasName("_net_IX_tblLogClientInscricaoInadimplente_intClientID");

                entity.Property(e => e.DataInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataSellOrder).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblLogConcursoQuestaoComentario>(entity =>
            {
                entity.HasIndex(e => new { e.dteDateAlteracao, e.intEmployeeAlterou, e.intQuestaoID })
                    .HasName("_net_IX_tblLogConcursoQuestaoComentario_02");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtComentarioAntigo, e.txtComentarioNovo, e.dteDateAlteracao, e.intEmployeeAntigo, e.txtLoginName, e.bitDuplicado, e.intEmployeeAlterou })
                    .HasName("_net_IX_tblLogConcursoQuestaoComentario_01");

                entity.Property(e => e.dteDateAlteracao).HasColumnType("datetime");

                entity.Property(e => e.txtComentarioAntigo).HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtComentarioNovo).HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtLoginName).HasMaxLength(200);
            });

                        modelBuilder.Entity<tblProvaVideo>(entity =>
            {
                entity.HasKey(e => e.intProvaVideoId)
                    .HasName("PK__tblProva__F31E9D851D03E0E6");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.dteLiberacao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intProvaVideoIndice)
                    .WithMany(p => p.tblProvaVideo)
                    .HasForeignKey(d => d.intProvaVideoIndiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblProvaV__intPr__256BF8BC");
            });

            modelBuilder.Entity<tblProvaVideoIndice>(entity =>
            {
                entity.HasKey(e => e.intProvaVideoIndiceId)
                    .HasName("PK__tblProva__BCFE7AA0A6C8AEAF");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.intLessonTitle)
                    .WithMany(p => p.tblProvaVideoIndice)
                    .HasForeignKey(d => d.intLessonTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblProvaV__intLe__228F8C11");
            });

            modelBuilder.Entity<tblLogGaleriaImagem>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblLogGaleriaImagem)
                    .HasForeignKey(d => d.intEmployeeID)
                    .HasConstraintName("fk_LogEmployee");
            });

            modelBuilder.Entity<tblLogMesesBlocoMaterialAnteriorAvulso>(entity =>
            {
                entity.HasKey(e => new { e.intID, e.intOrderID, e.intPaymentDocumentID });

                entity.HasIndex(e => e.intMes)
                    .HasName("_dta_index_tblLogMesesBlocoMaterialAnterior_5_702065687__K5");

                entity.HasIndex(e => new { e.intMes, e.intOrderID })
                    .HasName("_dta_index_tblLogMesesBlocoMaterialAnterior_5_702065687__K5_K2");

                entity.HasIndex(e => new { e.intOrderID, e.intMes })
                    .HasName("_dta_stat_702065687_2_5");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.dteDate, e.intMes, e.intProductGroupID })
                    .HasName("_net_IX_tblLogMesesBlocoMaterialAnteriorAvulso_intOrderID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.dteDate, e.intMes, e.intProductGroupID, e.intEmployeeID, e.intOrderID })
                    .HasName("_net_IX_tblLogMesesBlocoMaterialAnteriorAvulso_IntOrderID_intPaymentDocumentID");

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.HasOne(d => d.intOrder)
                    .WithMany(p => p.tblLogMesesBlocoMaterialAnteriorAvulso)
                    .HasForeignKey(d => d.intOrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogMesesBlocoMaterialAnteriorAvulso_tblSellOrders");

                entity.HasOne(d => d.intPaymentDocument)
                    .WithMany(p => p.tblLogMesesBlocoMaterialAnteriorAvulso)
                    .HasForeignKey(d => d.intPaymentDocumentID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogMesesBlocoMaterialAnteriorAvulso_tblPaymentDocuments");
            });

            modelBuilder.Entity<tblLogOperacoesConcurso>(entity =>
            {
                entity.HasKey(e => e.intLogOperacoesConcurso)
                    .HasName("PK__tblLogOp__1D7C3E81F4CEE29C");

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);
            });

            modelBuilder.Entity<tblLogOrdemVenda>(entity =>
            {
                entity.HasKey(e => e.intLogOrdemVendaID);

                entity.HasIndex(e => e.intClientID)
                    .HasName("_net_IX_tblLogOrdemVenda_01");

                entity.Property(e => e.dataInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tblLogRecursoAluno>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblLogRe__11B679323326D56E");

                entity.HasIndex(e => new { e.intClientID, e.intQuestaoID })
                    .HasName("_dta_stat_826182908_2_3");

                entity.HasIndex(e => new { e.intID, e.dteDataCriacao, e.bitVisualizadoBanca, e.bitVisualizadoMedgrupo, e.intClientID, e.intQuestaoID })
                    .HasName("_dta_index_tblLogRecursoAluno_5_826182908__K2_K3_1_4_5_6");

                entity.HasIndex(e => new { e.intID, e.dteDataCriacao, e.bitVisualizadoBanca, e.bitVisualizadoMedgrupo, e.intQuestaoID, e.intClientID })
                    .HasName("_dta_index_tblLogRecursoAluno_5_826182908__K3_K2_1_4_5_6");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblLog_PrintApostila>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblLog_P__11B6793207B5BAF1");

                entity.Property(e => e.cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.data).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblLog_PrintApostilaMedsoftPro>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblLog_P__11B679322C8D258C");

                entity.Property(e => e.cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.data).HasColumnType("smalldatetime");

                entity.Property(e => e.numPorcentagem).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<tblMaterialApostila>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B6793210D4239E");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblMaterialApostilaAluno>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B679321598D8BB");

                entity.HasIndex(e => new { e.intClientID, e.bitAtiva })
                    .HasName("ix_tblMaterialApostilaAluno_intClientID_bitAtiva");

                entity.HasIndex(e => new { e.intMaterialApostilaID, e.intClientID })
                    .HasName("ix_tblMaterialApostilaAluno_intMaterialApostilaID_intClientID");

                entity.Property(e => e.bitAtiva).HasDefaultValueSql("((0))");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtApostilaNameId).HasMaxLength(300);

                entity.Property(e => e.txtConteudo)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMaterialApostilaAluno)
                    .HasForeignKey(d => d.intClientID)
                    .HasConstraintName("FK__tblMateri__intCl__18754566");

                entity.HasOne(d => d.intMaterialApostila)
                    .WithMany(p => p.tblMaterialApostilaAluno)
                    .HasForeignKey(d => d.intMaterialApostilaID)
                    .HasConstraintName("FK__tblMateri__intMa__1781212D");
            });

            modelBuilder.Entity<tblMaterialApostilaAluno_Comentario>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B6793221FEAFA0");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblMaterialApostilaAluno_Comentario)
                    .HasForeignKey(d => d.intApostilaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intAp__23E6F812");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMaterialApostilaAluno_Comentario)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intCl__24DB1C4B");
            });

            modelBuilder.Entity<tblMaterialApostilaAssets>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblMater__11B678D2795E06A3");

                entity.Property(e => e.txtUrl)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMaterialApostilaComentario>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B679323F662440");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.txtComentarioID)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblMaterialApostilaComentario)
                    .HasForeignKey(d => d.intApostilaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intAp__414E6CB2");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMaterialApostilaComentario)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intCl__424290EB");
            });

            modelBuilder.Entity<tblMaterialApostilaConfig>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B67932453EE43C");

                entity.Property(e => e.dteDataAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMaterialApostilaInteracao>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B679327B138564");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.txtConteudo).HasColumnType("text");

                entity.Property(e => e.txtInteracaoID)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.txtLinkMedia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblMaterialApostilaInteracao)
                    .HasForeignKey(d => d.intApostilaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intAp__7CFBCDD6");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMaterialApostilaInteracao)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMateri__intCl__7DEFF20F");
            });

            modelBuilder.Entity<tblMaterialApostilaProgresso>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMater__11B679321AF3B9A0");

                entity.HasIndex(e => new { e.intApostilaID, e.intClientID })
                    .HasName("IX_tblMaterialApostilaProgresso_intApostilaID_intClientID_0CF82");

                entity.HasIndex(e => new { e.intClientID, e.dblPercentProgresso })
                    .HasName("ix_tblMaterialApostilaProgresso_intClientID_dblPercentProgresso");

                entity.Property(e => e.dblPercentProgresso).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.HasOne(d => d.intApostila)
                    .WithMany(p => p.tblMaterialApostilaProgresso)
                    .HasForeignKey(d => d.intApostilaID)
                    .HasConstraintName("FK__tblMateri__intAp__1CDC0212");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMaterialApostilaProgresso)
                    .HasForeignKey(d => d.intClientID)
                    .HasConstraintName("FK__tblMateri__intCl__1DD0264B");
            });

            modelBuilder.Entity<tblMaterialOrdersGalpao>(entity =>
            {
                entity.HasKey(e => e.intOrderId);

                entity.HasIndex(e => new { e.intMaterialID, e.intWarehouseid, e.intWarehouseOrigemID })
                    .HasName("IX_tblMaterialOrdersGalpao_intWarehouseid_intWarehouseOrigemID_intMaterialID");

                entity.HasIndex(e => new { e.intOrderId, e.intMaterialID, e.IntQuantidade, e.intWarehouseOrigemID })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intWarehouseOrigemID");

                entity.HasIndex(e => new { e.intOrderId, e.IntQuantidade, e.intMaterialID, e.intWarehouseid, e.dteDateAula })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intMaterialID_intWarehouseid");

                entity.HasIndex(e => new { e.intMaterialID, e.intWarehouseOrigemID, e.intQuantidadeRecebida, e.IntQuantidade, e.intWarehouseid, e.dteDateAula })
                    .HasName("IX_tblMaterialOrdersGalpao_intWarehouseid_intMaterialID_intWarehouseOrigemID");

                entity.HasIndex(e => new { e.intOrderId, e.intMaterialID, e.intProductGroupID, e.IntQuantidade, e.bitComplemento, e.intWarehouseOrigemID, e.intWarehouseid, e.intStatusID })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intWarehouseid_intStatusID");

                entity.HasIndex(e => new { e.intOrderId, e.intMaterialID, e.IntQuantidade, e.intWarehouseid, e.dteDatePrazoEnvio, e.intProductGroupID, e.intAno, e.dteDatePrazoChegada, e.intStatusID })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intStatusID");

                entity.HasIndex(e => new { e.intOrderId, e.intMaterialID, e.intProductGroupID, e.intWarehouseid, e.dteDatePrazoEnvio, e.dteDatePrazoChegada, e.dteDatePrazoConferencia, e.bitComplemento, e.dteDateAula, e.intStatusID })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_bitComplemento");

                entity.HasIndex(e => new { e.intOrderId, e.dteDatePrazoEnvio, e.dteDatePrazoChegada, e.dteDatePrazoConferencia, e.dteDateAula, e.intEmployeeID, e.bitComplemento, e.intMaterialID, e.intProductGroupID, e.intWarehouseid, e.intStatusID })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intMaterialID");

                entity.HasIndex(e => new { e.intOrderId, e.intMaterialID, e.intProductGroupID, e.IntQuantidade, e.intWarehouseid, e.dteDatePrazoEnvio, e.dteDatePrazoChegada, e.dteDatePrazoConferencia, e.dteDateAula, e.intStatusID, e.bitComplemento, e.intTipoComplementoID, e.intWarehouseOrigemID, e.intQuantidadeRecebida, e.intAno })
                    .HasName("_net_IX_tblMaterialOrdersGalpao_intAno");

                entity.Property(e => e.dteDateAula).HasColumnType("datetime");

                entity.Property(e => e.dteDatePrazoChegada).HasColumnType("datetime");

                entity.Property(e => e.dteDatePrazoConferencia).HasColumnType("datetime");

                entity.Property(e => e.dteDatePrazoEnvio).HasColumnType("datetime");

                entity.Property(e => e.intWarehouseOrigemID).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intMaterial)
                    .WithMany(p => p.tblMaterialOrdersGalpao)
                    .HasForeignKey(d => d.intMaterialID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaterialOrdersGalpao_tblMaterials");

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblMaterialOrdersGalpao)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaterialOrdersGalpao_tblProductGroups1");

                entity.HasOne(d => d.intWarehouseOrigem)
                    .WithMany(p => p.tblMaterialOrdersGalpaointWarehouseOrigem)
                    .HasForeignKey(d => d.intWarehouseOrigemID)
                    .HasConstraintName("FK_tblMaterialOrdersGalpao_tblWarehouses1");

                entity.HasOne(d => d.intWarehouse)
                    .WithMany(p => p.tblMaterialOrdersGalpaointWarehouse)
                    .HasForeignKey(d => d.intWarehouseid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaterialOrdersGalpao_tblWarehouses");
            });

            modelBuilder.Entity<tblMaterialOrdersGalpaoRomaneio>(entity =>
            {
                entity.HasKey(e => e.intRomaneioID);

                entity.HasIndex(e => new { e.intRomaneioID, e.intStatus })
                    .HasName("_net_IX_tblMaterialOrdersGalpaoRomaneio_intStatus");

                entity.HasIndex(e => new { e.intRomaneioID, e.intWareHouseID })
                    .HasName("_net_IX_tblMaterialOrdersGalpaoRomaneio_intWareHouseID");

                entity.HasIndex(e => new { e.intRomaneioID, e.intShippingCompanyID, e.intStatus })
                    .HasName("_net_IX_tblMaterialOrdersGalpaoRomaneio_intShippingCompanyID");

                entity.Property(e => e.dteDate).HasColumnType("date");

                entity.Property(e => e.dteDateExpedicao).HasColumnType("datetime");

                entity.Property(e => e.txtAddress1).HasMaxLength(300);

                entity.Property(e => e.txtAddress2).HasMaxLength(300);

                entity.Property(e => e.txtDocumento).HasMaxLength(50);

                entity.Property(e => e.txtMotorista).HasMaxLength(300);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(200);

                entity.Property(e => e.txtZipCode).HasMaxLength(50);

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblMaterialOrdersGalpaoRomaneio)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblMaterialOrdersGalpaoRomaneio_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblMaterialOrdersGalpaoRomaneio)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblMaterialOrdersGalpaoRomaneio_tblCities");

                entity.HasOne(d => d.intResponsavelRecebimentoNavigation)
                    .WithMany(p => p.tblMaterialOrdersGalpaoRomaneio)
                    .HasForeignKey(d => d.intResponsavelRecebimento)
                    .HasConstraintName("FK_tblMaterialOrdersGalpaoRomaneio_tblEmployees");

                entity.HasOne(d => d.intShippingCompany)
                    .WithMany(p => p.tblMaterialOrdersGalpaoRomaneio)
                    .HasForeignKey(d => d.intShippingCompanyID)
                    .HasConstraintName("FK_tblMaterialOrdersGalpaoRomaneio_tblShippingCompanies");

                entity.HasOne(d => d.intWareHouse)
                    .WithMany(p => p.tblMaterialOrdersGalpaoRomaneio)
                    .HasForeignKey(d => d.intWareHouseID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaterialOrdersGalpaoRomaneio_tblWarehouses");
            });

            modelBuilder.Entity<tblMaterials>(entity =>
            {
                entity.HasKey(e => e.intMaterialID)
                    .HasName("PK_tblStockItens");

                entity.HasIndex(e => e.txtBarCode)
                    .HasName("_net_IX_tblMaterials_txtBarCode");

                entity.HasIndex(e => new { e.intMaterialID, e.intVendorID })
                    .HasName("_net_IX_tblMaterials_intVendorID");

                entity.HasIndex(e => new { e.intMaterialID, e.txtName, e.txtBarCode, e.intMaterialGroupID, e.intOrderUnitID, e.intVendorID, e.intMaterialSubGroupID, e.txtShortName, e.txtStatus, e.intPackingID, e.dblWeight, e.intUnitsPacking, e.intMaterialTypeID })
                    .HasName("_net_IX_tblMaterials_intMaterialTypeID");

                entity.Property(e => e.intMaterialID).ValueGeneratedNever();

                entity.Property(e => e.txtBarCode).HasMaxLength(100);

                entity.Property(e => e.txtName).HasMaxLength(150);

                entity.Property(e => e.txtShortName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intVendor)
                    .WithMany(p => p.tblMaterials)
                    .HasForeignKey(d => d.intVendorID)
                    .HasConstraintName("FK_tblMaterials_tblCompanies");
            });

            modelBuilder.Entity<tblMedCode_MarcasCompartilhadas>(entity =>
            {
                entity.HasKey(e => e.intMarcasCompartilhadasId);
            });

            modelBuilder.Entity<tblMedNotas_Cliente>(entity =>
            {
                entity.HasKey(e => e.intClienteID);

                entity.HasOne(d => d.intEmpresa)
                    .WithMany(p => p.tblMedNotas_Cliente)
                    .HasForeignKey(d => d.intEmpresaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Cliente_tblMedNotas_Empresa");
            });

            modelBuilder.Entity<tblMedNotas_Empresa>(entity =>
            {
                entity.HasKey(e => e.intEmpresaID);

                entity.Property(e => e.txtCNPJ)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedNotas_Fornecedor>(entity =>
            {
                entity.HasKey(e => e.intFornecedorID);

                entity.HasOne(d => d.intEmpresa)
                    .WithMany(p => p.tblMedNotas_Fornecedor)
                    .HasForeignKey(d => d.intEmpresaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Fornecedor_tblMedNotas_Empresa");
            });

            modelBuilder.Entity<tblMedNotas_Guia>(entity =>
            {
                entity.HasKey(e => e.intGuiaID);

                entity.Property(e => e.dteDataAlteracao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteDataPagamento).HasColumnType("datetime");

                entity.Property(e => e.dteDataVencimento).HasColumnType("datetime");

                entity.Property(e => e.intUsuarioID).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intNota)
                    .WithMany(p => p.tblMedNotas_Guia)
                    .HasForeignKey(d => d.intNotaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Guia_tblMedNotas_Nota");

                entity.HasOne(d => d.intTipoImposto)
                    .WithMany(p => p.tblMedNotas_Guia)
                    .HasForeignKey(d => d.intTipoImpostoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Guia_tblMedNotas_TipoImposto");
            });

            modelBuilder.Entity<tblMedNotas_LogGuia>(entity =>
            {
                entity.HasKey(e => e.intLogGuiaID);

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.dteDataPagamento).HasColumnType("datetime");

                entity.Property(e => e.dteDataVencimento).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblMedNotas_LogNota>(entity =>
            {
                entity.HasKey(e => e.intLogNotaID);

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.dteDataPagamento).HasColumnType("datetime");

                entity.Property(e => e.dteEmissao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtNumero)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedNotas_Nota>(entity =>
            {
                entity.HasKey(e => e.intNotaID);

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.dteDataPagamento).HasColumnType("datetime");

                entity.Property(e => e.dteEmissao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtNumero)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.intCliente)
                    .WithMany(p => p.tblMedNotas_Nota)
                    .HasForeignKey(d => d.intClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Nota_tblMedNotas_Cliente");

                entity.HasOne(d => d.intFornecedor)
                    .WithMany(p => p.tblMedNotas_Nota)
                    .HasForeignKey(d => d.intFornecedorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedNotas_Nota_tblMedNotas_Fornecedor");
            });

            modelBuilder.Entity<tblMedNotas_PermissaoXSetor>(entity =>
            {
                entity.HasKey(e => e.intPermissaoXSetorID);
            });

            modelBuilder.Entity<tblMedNotas_Status>(entity =>
            {
                entity.HasKey(e => e.intStatusID);

                entity.Property(e => e.txtStatusNome)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblMedNotas_TipoImposto>(entity =>
            {
                entity.HasKey(e => e.intTipoImpostoID);

                entity.Property(e => e.txtTipoImpostoNome)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblMedNotas_TipoPermissao>(entity =>
            {
                entity.HasKey(e => e.intTipoPermissaoID);

                entity.Property(e => e.txtTipoPermissaoNome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedNotas_Usuario>(entity =>
            {
                entity.HasKey(e => e.intUsuarioID);

                entity.Property(e => e.txtUsuarioNome)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedSoft_AcessoEspecial>(entity =>
            {
                entity.HasKey(e => e.intAcessoEspecialID);

                entity.Property(e => e.dteDataHoraAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblMedSoft_VersaoAppPermissao>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblMedSo__11B678D21BA05087");

                entity.Property(e => e.txtVersaoApp)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedcineVideos>(entity =>
            {
                entity.HasKey(e => e.intVideoID);

                entity.HasIndex(e => new { e.intLessonTitleID, e.txtVideoName, e.txtVideoPassword, e.dteDuracao, e.dteExpirationDate, e.txtVideoDescription })
                    .HasName("_net_IX_tblMedcineVideos_txtVideoDescription");

                entity.HasIndex(e => new { e.txtVideoName, e.txtVideoPassword, e.dteDuracao, e.dteExpirationDate, e.intLessonTitleID, e.txtVideoDescription })
                    .HasName("_net_IX_tblMedcineVideos_intLessonTitleID");

                entity.Property(e => e.bitEncripted).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteDuracao).HasColumnType("datetime");

                entity.Property(e => e.dteExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.txtBarCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtOBS).HasMaxLength(1000);

                entity.Property(e => e.txtVideoDescription).HasMaxLength(200);

                entity.Property(e => e.txtVideoName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtVideoPassword).HasMaxLength(50);
            });

            modelBuilder.Entity<tblMedcode_DataMatrix>(entity =>
            {
                entity.HasKey(e => e.intDataMatrixID);

                entity.HasIndex(e => new { e.intDataMatrixID, e.intBookID, e.intProductGroup, e.intMediaID, e.dteDateTime, e.txtDescription, e.intIndex, e.txtMediaCode, e.intEmployeeId, e.intMediaTipo })
                    .HasName("_net_IX_edcode_DataMatrix_intMediaTipo");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtArquivo).HasMaxLength(200);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtMediaCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblMedcode_DataMatrix)
                    .HasForeignKey(d => d.intEmployeeId)
                    .HasConstraintName("FK_tblMedcode_DataMatrix_tblEmployees");

                entity.HasOne(d => d.intMediaTipoNavigation)
                    .WithMany(p => p.tblMedcode_DataMatrix)
                    .HasForeignKey(d => d.intMediaTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMedcode_DataMatrix_tblMedcode_DataMatrix_Tipo");
            });

            modelBuilder.Entity<tblMedcode_DataMatrix_Anexo>(entity =>
            {
                entity.HasKey(e => e.intAnexoID)
                    .HasName("PK__tblMedco__17CC5E4A1A70F142");

                entity.Property(e => e.dteDataInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricaoAnexo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedcode_DataMatrix_Log>(entity =>
            {
                entity.HasKey(e => e.intDataMatrixLogID);

                entity.Property(e => e.dteDataAlteracao).HasColumnType("datetime");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtArquivo).HasMaxLength(200);

                entity.Property(e => e.txtDescription).HasMaxLength(500);

                entity.Property(e => e.txtMediaCode).HasMaxLength(50);
            });

            modelBuilder.Entity<tblMedcode_DataMatrix_Tipo>(entity =>
            {
                entity.HasKey(e => e.intMediaTipoID);

                entity.Property(e => e.txtMediaDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedmeAreas>(entity =>
            {
                entity.HasKey(e => e.intAreaId)
                    .HasName("PK_intAreaId");
            });

            modelBuilder.Entity<tblMedmeVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblMedme__6DAE9E9638976A9C");

                entity.Property(e => e.dteLastUpdate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblMednet_AlunoClipping>(entity =>
            {
                entity.HasKey(e => e.intMnClipping);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtTitulo)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblMednet_AlunoComentario>(entity =>
            {
                entity.HasKey(e => e.intMnComentarioId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtComentario).HasMaxLength(600);

                entity.Property(e => e.txtTitulo).HasMaxLength(100);
            });

            modelBuilder.Entity<tblMedsoftClipboardReport>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMedso__11B67932225209C2");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtDeviceID)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedsoftScreenshotReport>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMedso__11B679324891E8E1");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtDeviceID)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblMedsoftScreenshotReport)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblMedsof__intCl__4A7A3153");
            });

            modelBuilder.Entity<tblMedsoft_AlunosOnline>(entity =>
            {
                entity.HasKey(e => e.intAlunosOnlineID);

                entity.HasIndex(e => new { e.intAlunosOnlineID, e.txtMachineToken, e.dteTimeStamp, e.intClientID })
                    .HasName("_net_IX_tblMedsoft_AlunosOnline_intClientID");

                entity.Property(e => e.dteTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.txtMachineToken).HasMaxLength(50);
            });

            modelBuilder.Entity<tblMedsoft_Atualizacao>(entity =>
            {
                entity.HasKey(e => e.intAtualizacaoID)
                    .HasName("PK_tblMedsoft_Atualizacao_");

                entity.HasIndex(e => new { e.intAtualizacaoID, e.intAtualizacaoTipoID, e.bitActive, e.guidColunaValue })
                    .HasName("_net_IX_tblMedsoft_Atualizacao_bitActive");

                entity.HasIndex(e => new { e.intAtualizacaoID, e.dteRowDateTime, e.bitActive, e.intAtualizacaoTipoID, e.guidColunaValue })
                    .HasName("_net_IX_tblMedsoft_Atualizacao_guidColunaValue");

                entity.HasIndex(e => new { e.intAtualizacaoID, e.bitActive, e.guidColunaValue, e.bitSincronia, e.intAtualizacaoTipoID, e.dteRowDateTime })
                    .HasName("_net_IX_tblMedsoft_Atualizacao_intAtualizacaoID");

                entity.HasIndex(e => new { e.intAtualizacaoID, e.guidColunaValue, e.bitSincronia, e.bitActive, e.intAtualizacaoTipoID, e.dteRowDateTime })
                    .HasName("_net_IX_tblMedsoft_Atualizacao_bitActive_intAtualizacaoTipoID");

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteRowDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            // modelBuilder.Entity<tblMedsoft_Atualizacao_Aluno>(entity =>
            // {
            //     entity.HasNoKey();

            //     entity.Property(e => e.bitActive)
            //         .IsRequired()
            //         .HasDefaultValueSql("((1))");

            //     entity.Property(e => e.dteDateTime).HasColumnType("datetime");

            //     entity.Property(e => e.txtMachineToken)
            //         .IsRequired()
            //         .HasMaxLength(300)
            //         .IsFixedLength();

            //     entity.HasOne(d => d.intAtualizacao)
            //         .WithMany(p => p.tblMedsoft_Atualizacao_Aluno)
            //         .HasForeignKey(d => d.intAtualizacaoID)
            //         .OnDelete(DeleteBehavior.ClientSetNull)
            //         .HasConstraintName("FK_tblMedsoft_Atualizacao_Aluno_tblMedsoft_Atualizacao_HD");
            // });

            modelBuilder.Entity<tblMedsoft_CacheConfig>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblMedso__11B6793263F8E178");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMedsoft_Especialidade_Classificacao>(entity =>
            {
                entity.HasKey(e => new { e.intClassificacaoID, e.intEspecialidadeID });
            });

            modelBuilder.Entity<tblMedsoft_Especialidade_Old_to_New>(entity =>
            {
                entity.HasKey(e => new { e.intOldEspecialidadeId, e.intNewEspecialidadeId });
            });

            modelBuilder.Entity<tblMedsoft_PermissaoLogin>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.intClientID);

                entity.Property(e => e.dteCadastro).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblMedsoft_Questao_Especialidade>(entity =>
            {
                entity.HasKey(e => new { e.intExercicioTipo, e.intQuestaoID, e.guidQuestaoID, e.intEspecialidadeID });

                entity.HasIndex(e => e.guidQuestaoID)
                    .HasName("_net_IX_tblMedsoft_Questao_Especialidade_guidQuestaoID");

                entity.HasIndex(e => e.intQuestaoID)
                    .HasName("_net_IX_tblMedsoft_Questao_Especialidade_intQuestaoID");

                entity.HasIndex(e => new { e.intExercicioTipo, e.intQuestaoID, e.intEspecialidadeID })
                    .HasName("_net_IX_intEspecialidadeID_intEspecialidadeID");
            });

            modelBuilder.Entity<tblMedsoft_VideoMioloAssistido>(entity =>
            {
                entity.HasKey(e => e.idId);

                entity.Property(e => e.dteTimestamp).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblMeiodeAnoAlunosAnosAnteriores>(entity =>
            {
                entity.HasKey(e => e.intMeioDeAnoAnterioresID);
            });

            modelBuilder.Entity<tblMensagens>(entity =>
            {
                entity.HasKey(e => e.intMensagemId)
                    .HasName("PK__tblMensa__38A2A37CCD2F80D2");

                entity.Property(e => e.dteInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtMensagem)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMensagensLogin>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intId).ValueGeneratedOnAdd();

                entity.Property(e => e.txtMensagem)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblMenuItens>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblMenuI__11B678D27A212DD2");

                entity.Property(e => e.txtLink).IsUnicode(false);

                entity.Property(e => e.txtNome).IsUnicode(false);
            });

            modelBuilder.Entity<tblMenu_PerfilRegra>(entity =>
            {
                entity.HasKey(e => e.intMenuRegraId);

                entity.HasOne(d => d.intMenu)
                    .WithMany(p => p.tblMenu_PerfilRegra)
                    .HasForeignKey(d => d.intMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMenu_PerfilRegra_tblMenuItens");

                entity.HasOne(d => d.intRegra)
                    .WithMany(p => p.tblMenu_PerfilRegra)
                    .HasForeignKey(d => d.intRegraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMenu_PerfilRegra_tblPerfil_Regra");
            });

            modelBuilder.Entity<tblNacionalidade>(entity =>
            {
                entity.HasKey(e => e.intNacionalidadeID);

                entity.Property(e => e.txtNacionalidade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblNotificacao>(entity =>
            {
                entity.HasKey(e => e.intNotificacaoId)
                    .HasName("PK__tblNotif__18A8958430F8EDC9");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.dteLiberacao).HasColumnType("datetime");

                entity.Property(e => e.intClientID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStatusEnvio).HasDefaultValueSql("((0))");

                entity.Property(e => e.intTipoEnvio).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtInfoAdicional)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtTexto)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtTitulo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblNotificacaoAluno>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblNotif__11B678D2389A0F91");

                entity.Property(e => e.dteLida).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblNotificacaoDeviceToken>(entity =>
            {
                entity.HasKey(e => e.intNotificacaoDeviceToken)
                    .HasName("PK__tblNotif__08115F54D313E5EB");

                entity.Property(e => e.dteEnvio).HasColumnType("datetime");

                entity.Property(e => e.txtInfoAdicional)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtMensagem)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.txtOneSignalToken)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtTitulo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblNotificacaoDuvidas>(entity =>
            {
                entity.HasKey(e => e.intNotificacaoDuvidaId);

                entity.HasIndex(e => e.intContactId)
                    .HasName("ix_tblNotificacaoDuvidas_intContactId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblNotificacaoDuvidas)
                    .HasForeignKey(d => d.intContactId)
                    .HasConstraintName("FK_tblNotificacaoDuvidas_tblPersons");

                entity.HasOne(d => d.intDuvida)
                    .WithMany(p => p.tblNotificacaoDuvidas)
                    .HasForeignKey(d => d.intDuvidaId)
                    .HasConstraintName("FK_tblNotificacaoDuvidas_tblDuvidasAcademicas");

                entity.HasOne(d => d.intNotificacao)
                    .WithMany(p => p.tblNotificacaoDuvidas)
                    .HasForeignKey(d => d.intNotificacaoId)
                    .HasConstraintName("FK_tblNotificacaoDuvidas_tblNotificacao");
            });

            modelBuilder.Entity<tblNotificacaoTipo>(entity =>
            {
                entity.HasKey(e => e.intNotificacaoTipoId)
                    .HasName("PK__tblNotif__4115F7ED34C97EAD");

                entity.Property(e => e.txtAlias).HasMaxLength(50);

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblOfflineConfig>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblOffli__11B67932CC0D2775");

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblPaymentDocuments>(entity =>
            {
                entity.HasKey(e => e.intPaymentDocumentID);

                entity.HasIndex(e => new { e.intCounterpartyID, e.intCreditAccountID })
                    .HasName("_net_IX_tblPaymentDocuments_intCounterpartyID_intCreditAccountID");

                entity.HasIndex(e => new { e.intCounterpartyID, e.intPaymentDocumentID, e.txtDescription })
                    .HasName("_dta_index_tblPaymentDocuments_5_678397586__K4_K1_K5");

                entity.HasIndex(e => new { e.intCounterpartyID, e.intPaymentStatusID, e.dblValue })
                    .HasName("_net_IX_tblPaymentDocuments_intCounterpartyID_intPaymentStatusID");

                entity.HasIndex(e => new { e.intPaymentStatusID, e.intCounterpartyID, e.intCreditAccountID })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentStatusID_intCounterpartyID");

                entity.HasIndex(e => new { e.intPaymentStatusID, e.intSellOrderID, e.intPaymentDocumentID, e.dblValue })
                    .HasName("_dta_index_tblPaymentDocuments_5_678397586__K3_K19_K1_K7");

                entity.HasIndex(e => new { e.txtDescription, e.intPaymentStatusID, e.intSellOrderID, e.dblValue })
                    .HasName("ix_tblPaymentDocuments_intPaymentStatusID_intSellOrderID_dblValue_includes");

                entity.HasIndex(e => new { e.dblValue, e.intPaymentDocumentID, e.intSellOrderID, e.txtDescription, e.txtDocumentNumber })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentDocumentID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.txtDescription, e.intPaymentStatusID, e.intSellOrderID, e.dteDate, e.dblValue })
                    .HasName("ix_tblPaymentDocuments_intPaymentStatusID_intSellOrderID_dteDate_dblValue_includes");

                entity.HasIndex(e => new { e.dteDate, e.intPaymentMethodID, e.intCounterpartyID, e.intPaymentStatusID, e.dblValue, e.intSellOrderID, e.intPaymentDocumentID })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentMethodID_intCounterpartyID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intCounterpartyID, e.txtDescription, e.intSellOrderID, e.intPaymentStatusID, e.intTranche, e.dblValue })
                    .HasName("_net_IX_");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.dteDate, e.dblValue, e.intCounterpartyID, e.intSellOrderID, e.intPaymentStatusID })
                    .HasName("_net_IX_tblPaymentDocuments_OVStatus");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.intSellOrderID, e.intPaymentStatusID, e.dblValue })
                    .HasName("_net_IX_tblPaymentDocuments");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentStatusID, e.txtDescription, e.intTranche, e.intSellOrderID, e.intPaymentMethodID, e.dteDate, e.dblValue })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentMethodID_dteDate");

                entity.HasIndex(e => new { e.txtDescription, e.intPaymentMethodID, e.intCounterpartyID, e.intPaymentStatusID, e.intSellOrderID, e.dblValue, e.intPaymentDocumentID, e.dteDate })
                    .HasName("_dta_index_tblPaymentDocuments_5_678397586__K2_K4_K3_K19_K7_K1_K6_5");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.dblValue, e.txtDocumentNumber, e.intSellOrderID, e.intCounterpartyID, e.txtDescription, e.intCreditAccountID, e.intPaymentStatusID, e.dteDate })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentStatusID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.dblValue, e.txtDocumentNumber, e.intDebitAccountID, e.intTranche, e.intSellOrderID, e.intCreditAccountID })
                    .HasName("_net_IX_tblPaymentDocuments_intCreditAccountID");

                entity.HasIndex(e => new { e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.dblValue, e.txtDocumentNumber, e.intCreditAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.intPaymentMethodID, e.intDebitAccountID })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentMethodID_intDebitAccountID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.txtDocumentNumber, e.intDebitAccountID, e.intTranche, e.intSellOrderID, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.dblPaidValue, e.dtePaymentDate, e.intPaymentMethodID, e.intPaymentStatusID, e.intCreditAccountID, e.dblValue })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentMethodID_intPaymentStatusID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intCounterpartyID, e.txtDescription, e.txtDocumentNumber, e.intCreditAccountID, e.intDebitAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.dblValue, e.intPaymentStatusID, e.dteDate })
                    .HasName("_net_IX_tblPaymentDocuments_dblValue");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intPaymentStatusID, e.intCounterpartyID, e.dblValue, e.txtDocumentNumber, e.intCreditAccountID, e.intDebitAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.txtDescription, e.dteDate })
                    .HasName("_net_IX_tblPaymentDocuments_txtDescription");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.dblValue, e.txtDocumentNumber, e.intCreditAccountID, e.intDebitAccountID, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.intTranche })
                    .HasName("_net_IX_tblPaymentDocuments_intTranche");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.dblValue, e.txtDocumentNumber, e.intCreditAccountID, e.intDebitAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.dblPaidValue, e.dtePaymentDate, e.intSellOrderID })
                    .HasName("IX_tblPaymentDocuments_intSellOrderID_65A7A");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentMethodID, e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.dblValue, e.txtDocumentNumber, e.intCreditAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.intDebitAccountID })
                    .HasName("_net_IX_tblPaymentDocuments_intDebitAccountID");

                entity.HasIndex(e => new { e.intPaymentDocumentID, e.intPaymentStatusID, e.intCounterpartyID, e.txtDescription, e.dteDate, e.txtDocumentNumber, e.intDebitAccountID, e.intTranche, e.bitPreview, e.intDocumentTypeID, e.txtComplement, e.intStoreID, e.txtBancoPagador, e.txtAgenciaPagadora, e.txtContaCorrentePagadora, e.intSellOrderID, e.dblPaidValue, e.dtePaymentDate, e.intPaymentMethodID, e.intCreditAccountID, e.dblValue })
                    .HasName("_net_IX_tblPaymentDocuments_intPaymentMethodID_intCreditAccountID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dtePaymentDate).HasColumnType("datetime");

                entity.Property(e => e.txtAgenciaPagadora).HasMaxLength(50);

                entity.Property(e => e.txtBancoPagador).HasMaxLength(50);

                entity.Property(e => e.txtComplement).HasMaxLength(1000);

                entity.Property(e => e.txtContaCorrentePagadora).HasMaxLength(50);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.txtDocumentNumber).HasMaxLength(50);

                entity.HasOne(d => d.intCounterparty)
                    .WithMany(p => p.tblPaymentDocuments)
                    .HasForeignKey(d => d.intCounterpartyID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPaymentDocuments_tblPersons");

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblPaymentDocuments)
                    .HasForeignKey(d => d.intStoreID)
                    .HasConstraintName("FK_tblPaymentDocuments_tblStores");
            });

            modelBuilder.Entity<tblPaymentTemplateConditionType>(entity =>
            {
                entity.HasKey(e => e.intConditionTypeID);

                entity.Property(e => e.intConditionTypeID).ValueGeneratedNever();

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblPaymentTemplateData>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK_tblPaymentTemplateData_3");

                entity.HasIndex(e => e.dblValue)
                    .HasName("_net_IX_tblPaymentTemplateData_dblValue");

                entity.HasIndex(e => new { e.intPaymentTemplateID, e.dblSpecialDiscountBaseValue })
                    .HasName("_net_IX_tblPaymentTemplateData_dblSpecialDiscountBaseValue");

                entity.HasIndex(e => new { e.dblValue, e.dblValueExClient, e.intPaymentTemplateID, e.intSubscription })
                    .HasName("_net_IX_tblPaymentTemplateData_intPaymentTemplateID");

                entity.HasIndex(e => new { e.intPaymentTemplateID, e.dblValue, e.dblValueExClient, e.intSubscription, e.intPaymentTypeID })
                    .HasName("_net_IX_tblPaymentTemplateData_intSubscription_intPaymentTypeID");

                entity.HasIndex(e => new { e.intPaymentTemplateID, e.dblValue, e.dblValueExClient, e.intSubscription, e.intSequence })
                    .HasName("_net_IX_tblPaymentTemplateData_intSubscription_intSequence");

                entity.HasIndex(e => new { e.intPaymentTemplateID, e.dblValue, e.intPaymentTypeID, e.dblValueExClient, e.dblSpecialDiscountBaseValue, e.intSubscription })
                    .HasName("IX_tblPaymentTemplateData_intSubscription_FF394");

                entity.HasIndex(e => new { e.intID, e.intPaymentTemplateID, e.dteDate, e.dblValue, e.txtDescription, e.intAccountID, e.intSubscription, e.intPaymentTypeID, e.dblMEDCURSOExClientValue, e.dblMEDCURSOExClientPercDisc, e.dblMEDCURSOExClientValueDisc, e.bitMEDCURSOExClientSumOfPrevYearsDisc, e.dblMEDCURSODiscount, e.dblMEDExClientValue, e.dblMEDExClientPercDisc, e.dblMEDExClientValueDisc, e.bitMEDExClientSumOfPrevYearsDisc, e.dblMEDDiscount, e.dblCOMBOExClientValue, e.dblCOMBOExClientPercDisc, e.dblCOMBOExClientValueDisc, e.bitCOMBOExClientSumOfPrevYearsDisc, e.dblCOMBODiscount, e.dblValueExClient, e.dblSpecialDiscountBaseValue, e.dblSpecialDiscountBaseValue_ExClient, e.intSequence })
                    .HasName("_net_IX_tblPaymentTemplateData_intSequence");

                entity.Property(e => e.dteDate).HasColumnType("smalldatetime");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblPaymentTemplatesCheques>(entity =>
            {
                entity.HasKey(e => new { e.intPaymentTemplateID, e.intMesInscricao });
            });

            modelBuilder.Entity<tblPaymentTypes>(entity =>
            {
                entity.HasKey(e => e.intPaymentTypeID);

                entity.Property(e => e.txtCarteira).HasMaxLength(10);

                entity.Property(e => e.txtCodigoEscritural).HasMaxLength(50);

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblPerfil>(entity =>
            {
                entity.HasKey(e => e.intPerfilID)
                    .HasName("PK__tblPerfi__5CEF5FA33E0DA027");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblPerfil1>(entity =>
            {
                entity.HasKey(e => e.intPerfilID)
                    .HasName("PK__tblPerfi__5CEF5FA3EE7B31C8");

                entity.ToTable("tblPerfil", "MEDBARRA\\alysson.silva");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblPerfil_Area>(entity =>
            {
                entity.HasKey(e => e.intPerfilAreaId)
                    .HasName("PK__tblPerfi__BB980116766986F1");

                entity.Property(e => e.txtArea)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblPerfil_Employees>(entity =>
            {
                entity.HasKey(e => e.intPerfilEmployeeID)
                    .HasName("PK__tblPerfi__7E3558FE58375E11");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblPerfil_Employees)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Employees_tblEmployee");

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblPerfil_Employees)
                    .HasForeignKey(d => d.intPerfilID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Employees_tblPerfil");
            });

            modelBuilder.Entity<tblPerfil_Employees1>(entity =>
            {
                entity.HasKey(e => e.intPerfilEmployeeID)
                    .HasName("PK__tblPerfi__7E3558FE434E0EC8");

                entity.ToTable("tblPerfil_Employees", "MEDBARRA\\alysson.silva");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblPerfil_Employees1)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Employees_tblEmployee");

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblPerfil_Employees1)
                    .HasForeignKey(d => d.intPerfilID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Employees_tblPerfil");
            });

            modelBuilder.Entity<tblPerfil_Funcionalidade>(entity =>
            {
                entity.HasKey(e => e.intPerfilFuncionalidadeID)
                    .HasName("PK__tblPerfi__28E790FA7A7389AD");

                entity.Property(e => e.txtTipoPermissao)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.intFuncionalidade)
                    .WithMany(p => p.tblPerfil_Funcionalidade)
                    .HasForeignKey(d => d.intFuncionalidadeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Funcionalidade_tblFuncionalidade");

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblPerfil_Funcionalidade)
                    .HasForeignKey(d => d.intPerfilID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Funcionalidade_tblPerfil");
            });

            modelBuilder.Entity<tblPerfil_Funcionalidade1>(entity =>
            {
                entity.HasKey(e => e.intPerfilFuncionalidadeID)
                    .HasName("PK__tblPerfi__28E790FA11365A11");

                entity.ToTable("tblPerfil_Funcionalidade", "MEDBARRA\\alysson.silva");

                entity.Property(e => e.txtTipoPermissao)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.intFuncionalidade)
                    .WithMany(p => p.tblPerfil_Funcionalidade1)
                    .HasForeignKey(d => d.intFuncionalidadeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Funcionalidade_tblFuncionalidade");

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblPerfil_Funcionalidade1)
                    .HasForeignKey(d => d.intPerfilID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPerfil_Funcionalidade_tblPerfil");
            });

            modelBuilder.Entity<tblPerfil_Regra>(entity =>
            {
                entity.HasKey(e => e.intPerfilRegraId)
                    .HasName("PK__tblPerfi__0D08E5387A3A17D5");

                entity.Property(e => e.txtAlias).IsUnicode(false);

                entity.Property(e => e.txtCategoria).IsUnicode(false);

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtNome).IsUnicode(false);
            });

            modelBuilder.Entity<tblPerfil_RegraEntidade>(entity =>
            {
                entity.HasKey(e => e.intPerfilRegraEntidadeId)
                    .HasName("PK__tblPerfi__F3A34AFD7FF2F12B");

                entity.HasIndex(e => new { e.intPerfilRegraId, e.intTipo, e.intEntidadeId })
                    .HasName("index_tblPerfil_RegraEntidade")
                    .IsUnique();
            });

            modelBuilder.Entity<tblPermissaoInadimplenciaConfiguracao>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.txtMensagemBloqueio).HasColumnType("text");

                entity.Property(e => e.txtMensagemDeAcordo).HasColumnType("text");

                entity.Property(e => e.txtMensagemSemOv)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblPermissaoInadimplenciaLogAlerta>(entity =>
            {
                entity.HasKey(e => e.intLogAlertaId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblPermissaoInadimplenciaLogConfirmacaoAlerta>(entity =>
            {
                entity.HasKey(e => e.intLogConfirmacaoId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblPermissaoListaMateriais>(entity =>
            {
                entity.HasKey(e => e.intID);
            });

            modelBuilder.Entity<tblPersons>(entity =>
            {
                entity.HasKey(e => e.intContactID)
                    .HasName("PK_tblContact");

                entity.HasIndex(e => e.bitActive)
                    .HasName("_dta_index_tblPersons_5_859918185__K24");

                entity.HasIndex(e => e.dteDate)
                    .HasName("_net_IX_tblPersons_dteDate");

                entity.HasIndex(e => e.intCityID)
                    .HasName("_net_IX_tblPersons_intCityID");

                entity.HasIndex(e => e.intContactID)
                    .HasName("_dta_index_tblPersons_5_859918185__K1");

                entity.HasIndex(e => e.txtClientLogin)
                    .IsUnique()
                    .HasFilter("([txtClientLogin] IS NOT NULL)");

                entity.HasIndex(e => e.txtName)
                    .HasName("_dta_index_tblPersons_5_859918185__K2");

                entity.HasIndex(e => new { e.bitActive, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K24_K1");

                entity.HasIndex(e => new { e.bitActive, e.txtRegister })
                    .HasName("_net_IX_tblPersons_bitactive");

                entity.HasIndex(e => new { e.intCityID, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K10_K1");

                entity.HasIndex(e => new { e.intCityID, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_10");

                entity.HasIndex(e => new { e.intContactID, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K10");

                entity.HasIndex(e => new { e.intContactID, e.txtEmail1 })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K16");

                entity.HasIndex(e => new { e.intContactID, e.txtEmail2 })
                    .HasName("_net_IX_tblPersons_txtEmail2");

                entity.HasIndex(e => new { e.intContactID, e.txtIDDocument })
                    .HasName("_net_IX_tblPersons_txtIDDocument");

                entity.HasIndex(e => new { e.intContactID, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K2");

                entity.HasIndex(e => new { e.intContactID, e.txtPassport })
                    .HasName("_net_IX_tblPersons_txtPassport");

                entity.HasIndex(e => new { e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_1");

                entity.HasIndex(e => new { e.txtEmail1, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_16");

                entity.HasIndex(e => new { e.txtName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_K1");

                entity.HasIndex(e => new { e.txtRegister, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1");

                entity.HasIndex(e => new { e.intCityID, e.txtName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_K1_10");

                entity.HasIndex(e => new { e.intContactID, e.txtEmail1, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K16_K2");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.intSex })
                    .HasName("_net_IX_tblPersons_intSex");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.txtEmail1 })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K2_K16");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.txtNickName })
                    .HasName("IX_tblPersonNickName");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_K3_1");

                entity.HasIndex(e => new { e.intContactID, e.txtRegister, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_1_3");

                entity.HasIndex(e => new { e.txtEmail1, e.intContactID, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K2_16");

                entity.HasIndex(e => new { e.txtEmail1, e.txtName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K2_K1_16");

                entity.HasIndex(e => new { e.txtName, e.intContactID, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K10_2");

                entity.HasIndex(e => new { e.txtName, e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_2");

                entity.HasIndex(e => new { e.txtName, e.txtEmail1, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_2_16");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.dteDate })
                    .HasName("_net_IX_tblPersons_txtRegister");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_2");

                entity.HasIndex(e => new { e.txtRegister, e.intContactID, e.txtName })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_K2");

                entity.HasIndex(e => new { e.intCityID, e.intContactID, e.txtName, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K2_K3_10");

                entity.HasIndex(e => new { e.txtEmail1, e.txtEmail2, e.txtEmail3, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_16_18_19");

                entity.HasIndex(e => new { e.txtName, e.dteDate, e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_2_22");

                entity.HasIndex(e => new { e.txtName, e.dteDate, e.txtRegister, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_2_22");

                entity.HasIndex(e => new { e.txtName, e.txtEmail1, e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_2_16");

                entity.HasIndex(e => new { e.txtName, e.txtEmail1, e.txtRegister, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_2_16");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.txtEmail1, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_2_3_16");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.txtRegister, e.dteBirthday, e.txtAddress1, e.txtPhone1, e.txtEmail1 })
                    .HasName("_net_IX_tblPersons_txtEmail1");

                entity.HasIndex(e => new { e.txtEmail1, e.txtRegister, e.dteBirthday, e.txtAddress1, e.txtPhone1, e.intContactID, e.txtName })
                    .HasName("_net_IX_tblpersons_intContactID_txtEmail1");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.dteBirthday, e.txtAddress1, e.txtPhone1, e.txtEmail1, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_2_3_5_7_12_16");

                entity.HasIndex(e => new { e.txtRegister, e.dteBirthday, e.intContactID, e.txtEmail1, e.txtName, e.txtPhone1, e.txtPhone2, e.txtCel })
                    .HasName("_net_IX_tblPersons_intContactID_txtRegister");

                entity.HasIndex(e => new { e.intContactID, e.txtName, e.txtRegister, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K10_1_2_3_12_13_14_15_16");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.intCityID, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_2_3_10_12_13_14_15_16");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.intCityID, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K10_K1_2_3_12_13_14_15_16");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.intContactID, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K10_2_3_12_13_14_15_16");

                entity.HasIndex(e => new { e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.intContactID, e.txtRegister, e.dteBirthday, e.txtAddress1, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtName })
                    .HasName("IX_tblPersonsName");

                entity.HasIndex(e => new { e.intSex, e.intAddressType, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone2, e.txtCel, e.txtFax, e.txtSite, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_4_6_8_9_10_11_13_14_15_17_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.bitActive, e.txtNickName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_4_5_6_7_8_9_10_11_12_13_14_15_16_18_19_20_21_24_28");

                entity.HasIndex(e => new { e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.bitActive, e.txtNickName, e.intContactID, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K10_4_5_6_7_8_9_11_12_13_14_15_16_18_19_20_21_24_28");

                entity.HasIndex(e => new { e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtSite, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.bitActive, e.txtNickName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_2_4_5_6_7_8_9_10_11_12_13_14_15_16_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID, e.intCityID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K10_K3_2_4_5_6_7_8_9_11_12_13_14_15_16_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID, e.txtRegister, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_K10_2_4_5_6_7_8_9_11_12_13_14_15_16_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.txtRegister, e.intContactID, e.intCityID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_K10_2_4_5_6_7_8_9_11_12_13_14_15_16_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtSite, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID, e.txtRegister })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_K3_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtSite, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.txtRegister, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K3_K1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_24_28");

                entity.HasIndex(e => new { e.txtName, e.txtRegister, e.intSex, e.dteBirthday, e.intAddressType, e.txtAddress1, e.txtAddress2, e.txtNeighbourhood, e.intCityID, e.txtZipCode, e.txtPhone1, e.txtPhone2, e.txtCel, e.txtFax, e.txtEmail1, e.txtSite, e.txtEmail2, e.txtEmail3, e.txtPassport, e.txtIDDocument, e.dteDate, e.bitActive, e.txtNickName, e.intContactID })
                    .HasName("_dta_index_tblPersons_5_859918185__K1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_24_28");

                entity.Property(e => e.dteBirthday).HasColumnType("datetime");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.imgFingerPrint).HasColumnType("image");

                entity.Property(e => e.intCityID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtAddress1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtAddress2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtCel)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtClientLogin).HasMaxLength(400);

                entity.Property(e => e.txtEmail1).HasMaxLength(100);

                entity.Property(e => e.txtEmail2).HasMaxLength(100);

                entity.Property(e => e.txtEmail3).HasMaxLength(100);

                entity.Property(e => e.txtFax)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtIDDocument).HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtNeighbourhood)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtNickName)
                    .HasMaxLength(60)
                    .IsFixedLength();

                entity.Property(e => e.txtPassport).HasMaxLength(50);

                entity.Property(e => e.txtPhone1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtPhone2)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtSite).HasMaxLength(100);

                entity.Property(e => e.txtZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblPersons)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblPersons_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblPersons)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblPersons_tblCities");
            });

            modelBuilder.Entity<tblPersonsAvatar>(entity =>
            {
                entity.HasKey(e => new { e.intContactID, e.intAvatarID });

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tblPersonsPicture>(entity =>
            {
                entity.HasKey(e => new { e.intContactID, e.intPictureTypeID });

                entity.HasIndex(e => new { e.intContactID, e.txtPicturePath, e.dteDateTime, e.intPictureTypeID, e.bitActive })
                    .HasName("_net_IX_tblPersonsPicture_intPictureTypeID");

                entity.Property(e => e.bitActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtPicturePath)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblPersonsPicture)
                    .HasForeignKey(d => d.intContactID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonsPicture_tblPersons");
            });

            modelBuilder.Entity<tblPersons_Passwords>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.txtPassword, e.intContactID, e.intChave, e.dteDataLimite })
                    .HasName("_net_IX_tblPersons_Passwords_intContactID");

                entity.Property(e => e.dteDataLimite).HasColumnType("datetime");

                entity.Property(e => e.dteDatePassword).HasColumnType("datetime");

                entity.Property(e => e.intAplicacaoId).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtPassword)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblPessoaGrupo>(entity =>
            {
                entity.HasKey(e => e.intPessoaGrupoID);

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblPessoaGrupo)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblPessoaGrupo_tblPersons");

                entity.HasOne(d => d.intGrupo)
                    .WithMany(p => p.tblPessoaGrupo)
                    .HasForeignKey(d => d.intGrupoID)
                    .HasConstraintName("FK_tblPessoaGrupo_tblGrupo");
            });

            modelBuilder.Entity<tblPorcentagemDesconto>(entity =>
            {
                entity.HasKey(e => e.intPorcentagemDescontoId)
                    .HasName("PK__tblPorce__08A0CF77FE3675A3");
            });

            modelBuilder.Entity<tblPrestacaoContasGestorXsubordinados>(entity =>
            {
                entity.HasKey(e => new { e.intGestorID, e.intSubordinadoID });

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.HasOne(d => d.intGestor)
                    .WithMany(p => p.tblPrestacaoContasGestorXsubordinadosintGestor)
                    .HasForeignKey(d => d.intGestorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrestacaoContasGestorXsubordinados_tblEmployees");

                entity.HasOne(d => d.intSubordinado)
                    .WithMany(p => p.tblPrestacaoContasGestorXsubordinadosintSubordinado)
                    .HasForeignKey(d => d.intSubordinadoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrestacaoContasGestorXsubordinados_tblEmployees1");
            });

            modelBuilder.Entity<tblProcedimentoEntrada>(entity =>
            {
                entity.HasKey(e => e.intProcedimentoEntradaId);

                entity.Property(e => e.Fim).HasColumnType("datetime");

                entity.Property(e => e.Inicio).HasColumnType("datetime");

                entity.HasOne(d => d.intTipoProcedimentoEntrada)
                    .WithMany(p => p.tblProcedimentoEntrada)
                    .HasForeignKey(d => d.intTipoProcedimentoEntradaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProcedimentoEntrada_tblTipoProcedimentoEntrada");
            });

            modelBuilder.Entity<tblProductCombos_Products>(entity =>
            {
                entity.HasKey(e => new { e.intComboID, e.intProductID })
                    .HasName("PK_tblStore_ProductGroup_PaymentTemplateItems");

                entity.HasIndex(e => new { e.intComboID, e.intProductID })
                    .HasName("_net_IX_tblProductCombos_Products_intProductID");

                entity.HasIndex(e => new { e.intProductID, e.intComboID })
                    .HasName("_net_IX_tblProductCombos_Products_intComboID");

                entity.HasOne(d => d.intProduct)
                    .WithMany(p => p.tblProductCombos_Products)
                    .HasForeignKey(d => d.intProductID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStore_CombosProducts_tblProducts");
            });

            modelBuilder.Entity<tblProductGroup1XPacote>(entity =>
            {
                entity.HasKey(e => e.intProductGroup1XPacoteID)
                    .HasName("PK_tblProductGroup1XPacote_1");
            });

            modelBuilder.Entity<tblProductGroups1>(entity =>
            {
                entity.HasKey(e => e.intProductGroup1ID);

                entity.HasIndex(e => e.txtDescription)
                    .HasName("IX_tblProductGroups1Description");

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblProducts>(entity =>
            {
                entity.HasKey(e => e.intProductID);

                entity.HasIndex(e => e.bitIsCombo)
                    .HasName("_dta_index_tblProducts_5_105819489__K8");

                entity.HasIndex(e => e.guidProductID)
                    .HasName("_net_IX_tblProducts_guidProductID");

                entity.HasIndex(e => e.intProductID)
                    .HasName("_dta_index_tblProducts_5_105819489__K1");

                entity.HasIndex(e => e.txtCode)
                    .HasName("_dta_index_tblProducts_5_105819489__K7");

                entity.HasIndex(e => new { e.intProductGroup2, e.intProductGroup1 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_K3");

                entity.HasIndex(e => new { e.intProductGroup2, e.intProductID })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_K1");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_1");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup3 })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K5");

                entity.HasIndex(e => new { e.intProductID, e.txtName })
                    .HasName("_dta_index_tblProducts_5_105819489__K2_1");

                entity.HasIndex(e => new { e.txtCode, e.intProductID })
                    .HasName("_dta_index_tblProducts_5_105819489__K7_K1");

                entity.HasIndex(e => new { e.intProductGroup1, e.intProductGroup2, e.intProductGroup3 })
                    .HasName("_dta_index_tblProducts_5_105819489__K3_K4_K5_9987");

                entity.HasIndex(e => new { e.intProductGroup1, e.intProductID, e.txtName })
                    .HasName("_dta_index_tblProducts_5_105819489__K3_K1_K2");

                entity.HasIndex(e => new { e.intProductGroup2, e.intProductGroup1, e.intProductGroup3 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_K3_K5");

                entity.HasIndex(e => new { e.intProductGroup3, e.intProductGroup1, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K5_K3_K4");

                entity.HasIndex(e => new { e.intProductGroup3, e.intProductID, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K4_5");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup1, e.txtName })
                    .HasName("_dta_index_tblProducts_5_105819489__K2_1_3");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup2, e.intProductGroup1 })
                    .HasName("_net_IX_tblProducts_intProductGroup2_intProductGroup1");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup3, e.txtCode })
                    .HasName("_net_IX_tblProducts_txtCode");

                entity.HasIndex(e => new { e.intProductID, e.txtCode, e.txtName })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K7_K2");

                entity.HasIndex(e => new { e.txtName, e.intProductGroup1, e.intProductID })
                    .HasName("_dta_index_tblProducts_5_105819489__K3_K1_2");

                entity.HasIndex(e => new { e.txtName, e.intProductID, e.intProductGroup1 })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K3_2");

                entity.HasIndex(e => new { e.txtName, e.txtCode, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_2_7");

                entity.HasIndex(e => new { e.bitIsCombo, e.intProductGroup3, e.intProductGroup1, e.intProductGroup2 })
                    .HasName("_dta_stat_105819489_8_5_3_4");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup3, e.txtCode, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_1_5_7");

                entity.HasIndex(e => new { e.txtCode, e.intProductGroup2, e.intProductID, e.intProductGroup3 })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_K1_K5_7");

                entity.HasIndex(e => new { e.txtCode, e.intProductID, e.intProductGroup1, e.txtName })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K3_K2_7");

                entity.HasIndex(e => new { e.txtCode, e.intProductID, e.intProductGroup3, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K5_K4_7");

                entity.HasIndex(e => new { e.txtName, e.txtCode, e.intProductGroup2, e.intProductID })
                    .HasName("_dta_index_tblProducts_5_105819489__K4_K1_2_7");

                entity.HasIndex(e => new { e.txtName, e.txtCode, e.intProductID, e.intProductGroup2 })
                    .HasName("_dta_index_tblProducts_5_105819489__K1_K4_2_7");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup2, e.txtCode, e.intProductGroup1, e.intProductGroup3 })
                    .HasName("IX_tblProducts_intProductGroup1_intProductGroup3_intProductID");

                entity.HasIndex(e => new { e.txtName, e.txtCode, e.intProductGroup2, e.intProductGroup3, e.intProductID })
                    .HasName("_net_IX_med_tblProducts_intProductGroup2_intProductGroup3");

                entity.HasIndex(e => new { e.intProductID, e.txtCode, e.intProductGroup3, e.intProductGroup1, e.txtName, e.intProductGroup2 })
                    .HasName("_net_IX_tblProducts_intProductGroup1_txtName");

                entity.HasIndex(e => new { e.intProductID, e.intProductGroup3, e.txtCode, e.txtShortName, e.txtName, e.intProductGroup1, e.intProductGroup2 })
                    .HasName("_net_IX_tblProducts_intProductGroup1_intProductGroup2");

                entity.HasIndex(e => new { e.intProductID, e.txtName, e.guidProductID, e.intProductGroup1, e.intProductGroup2, e.intProductGroup3, e.txtCode })
                    .HasName("_net_IX_tblProducts_intProductGroup3_txtCode_1");

                entity.HasIndex(e => new { e.intProductID, e.txtName, e.intProductGroup2, e.intProductGroup3, e.intType, e.bitIsCombo, e.txtShortName, e.guidProductID, e.intProductGroup1, e.txtCode })
                    .HasName("_net_IX_tblProducts_intProductGroup1_txtCode");

                entity.HasIndex(e => new { e.txtCode, e.intProductGroup2, e.intProductGroup3, e.intType, e.bitIsCombo, e.txtShortName, e.guidProductID, e.txtName, e.intProductID, e.intProductGroup1 })
                    .HasName("_net_IX_tblProducts_txtName_intProductID");

                entity.Property(e => e.guidProductID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.txtCode).HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.txtShortName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.intProductGroup1Navigation)
                    .WithMany(p => p.tblProductsintProductGroup1Navigation)
                    .HasForeignKey(d => d.intProductGroup1)
                    .HasConstraintName("FK_tblProducts_tblProductGroups1");

                entity.HasOne(d => d.intProductGroup2Navigation)
                    .WithMany(p => p.tblProductsintProductGroup2Navigation)
                    .HasForeignKey(d => d.intProductGroup2)
                    .HasConstraintName("FK_tblProducts_tblProductGroups2");

                entity.HasOne(d => d.intProductGroup3Navigation)
                    .WithMany(p => p.tblProductsintProductGroup3Navigation)
                    .HasForeignKey(d => d.intProductGroup3)
                    .HasConstraintName("FK_tblProducts_tblProductGroups3");
            });

            modelBuilder.Entity<tblProfessor_GrandeArea>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblProfe__11B679320B63FACC");
            });

            modelBuilder.Entity<tblProspects>(entity =>
            {
                entity.HasKey(e => e.intProspectID)
                    .HasName("PK_tblProspect");

                entity.Property(e => e.intProspectID).ValueGeneratedNever();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtAddress)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtAddressComplement)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtCel)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.txtCity)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtEnderecoReferencia)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.txtInstitution)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtNeighbourhood)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtNumero)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtZipCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblProspectsAdaptamed>(entity =>
            {
                entity.HasKey(e => e.intProspectID);

                entity.Property(e => e.intProspectID).ValueGeneratedNever();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtBairro)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtCep)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.txtCidade)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEndereco)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtEnderecoComplemento)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtFaculdade)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtTelefone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intPais)
                    .WithMany(p => p.tblProspectsAdaptamed)
                    .HasForeignKey(d => d.intPaisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProspectsAdaptamed_tblCountries");
            });

            modelBuilder.Entity<tblProtocoloGravacao_Emails>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblProto__11B678D25EABFA43");

                entity.Property(e => e.txtDestinatario)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblQuestaoConcurso_Imagem>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblQuest__11B6793222A05634");

                entity.HasIndex(e => e.intQuestaoID)
                    .HasName("_net_IX_tblQuestaoConcurso_Imagem_01");

                entity.Property(e => e.imgImagem).HasColumnType("image");

                entity.Property(e => e.imgImagemOtimizada).HasColumnType("image");

                entity.Property(e => e.txtLabel).HasMaxLength(200);

                entity.Property(e => e.txtName).HasMaxLength(200);
            });

            modelBuilder.Entity<tblQuestao_Duvida>(entity =>
            {
                entity.HasKey(e => e.intQuestaoDuvidaId);

                entity.Property(e => e.dtePergunta).HasColumnType("datetime");

                entity.Property(e => e.dteResposta).HasColumnType("datetime");

                entity.Property(e => e.txtPergunta)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.txtResposta)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblQuestao_Duvida_Encaminhamento>(entity =>
            {
                entity.HasKey(e => e.intQuestaoDuvidaEncaminhamentoID);

                entity.Property(e => e.dteEncaminhamento).HasColumnType("datetime");

                entity.HasOne(d => d.intDestinatario)
                    .WithMany(p => p.tblQuestao_Duvida_EncaminhamentointDestinatario)
                    .HasForeignKey(d => d.intDestinatarioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestao_Duvida_Encaminhamento_tblPersons_Destinatario");

                entity.HasOne(d => d.intQuestaoDuvida)
                    .WithMany(p => p.tblQuestao_Duvida_Encaminhamento)
                    .HasForeignKey(d => d.intQuestaoDuvidaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestao_Duvida_Encaminhamento_tblQuestao_Duvida");

                entity.HasOne(d => d.intRemetente)
                    .WithMany(p => p.tblQuestao_Duvida_EncaminhamentointRemetente)
                    .HasForeignKey(d => d.intRemetenteID)
                    .HasConstraintName("FK_tblQuestao_Duvida_Encaminhamento_tblPersons");
            });

            modelBuilder.Entity<tblQuestao_Duvida_Imagem>(entity =>
            {
                entity.HasKey(e => e.intDuvidaImagemId);

                entity.HasIndex(e => e.intQuestaoDuvidaId)
                    .HasName("PK_tblQuestao_Duvida");

                entity.Property(e => e.dteCadastro).HasColumnType("smalldatetime");

                entity.Property(e => e.txtNomeImagem)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblQuestao_Duvida_Lida>(entity =>
            {
                entity.HasKey(e => e.intDuvidaQuestaoLidaID);
            });

            modelBuilder.Entity<tblQuestao_Duvida_Moderada>(entity =>
            {
                entity.HasKey(e => e.intQuestaoDuvidaModeradaId)
                    .HasName("PK_tblQuestao_Duvida_moderada");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblQuestao_Duvida_Moderada)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestao_Duvida_moderada_tblEmployees");
            });

            modelBuilder.Entity<tblQuestao_Duvida_Resposta>(entity =>
            {
                entity.HasKey(e => e.intQuestaoDuvidaRespostaId)
                    .HasName("PK__tblQuest__9978F5F5061FCF80");

                entity.Property(e => e.dteResposta).HasColumnType("datetime");

                entity.Property(e => e.txtResposta)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblQuestao_Estatistica>(entity =>
            {
                entity.HasKey(e => new { e.intExercicioTipo, e.intQuestaoID, e.txtLetraAlternativa });

                entity.Property(e => e.txtLetraAlternativa)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.dblPercent).HasColumnType("decimal(9, 4)");
            });

            modelBuilder.Entity<tblQuestao_Favoritas>(entity =>
            {
                entity.HasKey(e => e.intQuestaoFavoritaID)
                    .HasName("PK__tblQuest__EEA2F59D01686102");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitAtivo })
                    .HasName("_net_IX_tblQuestao_Favoritas_intQuestaoID_bitAtivo");

                entity.Property(e => e.dteDataCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtJustificativa).HasColumnType("text");
            });

            modelBuilder.Entity<tblQuestao_Favoritas_Professores>(entity =>
            {
                entity.HasKey(e => e.intProfessorID)
                    .HasName("PK__tblQuest__2741A3190444CDAD");

                entity.Property(e => e.intProfessorID).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblQuestao_MontaProva>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblQuest__11B67932363D18AB");

                entity.HasIndex(e => e.intFiltroId);

                entity.HasIndex(e => new { e.intID, e.intQuestaoId, e.intFiltroId })
                    .HasName("_net_IX_tblQuestao_MontaProva_intQuestaoId_intFiltroId");

                entity.HasIndex(e => new { e.intProvaId, e.intQuestaoId, e.intTipoExercicioId })
                    .HasName("_net_IX_tblQuestao_MontaProva_intProvaId_intQuestaoId");

                entity.HasIndex(e => new { e.intQuestaoId, e.intProvaId, e.intTipoExercicioId })
                    .HasName("_net_IX_tblQuestao_MontaProva_intProvaId_intTipoExercicioId");

                entity.HasIndex(e => new { e.intID, e.intQuestaoId, e.intTipoExercicioId, e.intProvaId, e.intFiltroId })
                    .HasName("_net_IX_tblQuestao_MontaProva_intProvaId_intFiltroId");

                entity.HasIndex(e => new { e.intQuestaoId, e.intTipoExercicioId, e.intFiltroId, e.intProvaId, e.intID })
                    .HasName("_net_IX_tblQuestao_MontaProva_intProvaId_intID");

                entity.HasOne(d => d.intProva)
                    .WithMany(p => p.tblQuestao_MontaProva)
                    .HasForeignKey(d => d.intProvaId)
                    .HasConstraintName("FK__tblQuesta__intPr__3825611D");
            });

            modelBuilder.Entity<tblQuestionario>(entity =>
            {
                entity.HasKey(e => e.intQuestionarioID);

                entity.Property(e => e.dteCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<tblTurmaExcecaoCpfBase>(entity =>
            {
                entity.HasKey(e => e.intTurmaExcecaoCpfBaseId)
                    .HasName("PK__tblTurma__4439BFE5EF3786AC");

                entity.HasIndex(e => e.intCourseId)
                    .HasName("ix_tblTurmaExcecaoCpfBase_intTurmaExcecaoCpfBaseId")
                    .IsUnique();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblQuestionario_Cliente>(entity =>
            {
                entity.HasKey(e => e.intQuestionarioClienteID);

                entity.HasIndex(e => e.intClientID)
                    .HasName("_net_IX_tblQuestionario_Cliente_intClientID");

                entity.HasIndex(e => e.txtEmail)
                    .HasName("IX_tblQuestionario_Cliente_txtEmail_E6693");

                entity.HasIndex(e => new { e.intQuestionarioClienteID, e.intClientID, e.txtEmail, e.txtRegister, e.intQuestionarioID })
                    .HasName("_net_IX_tblQuestionario_Cliente_intQuestionarioID");

                entity.Property(e => e.IPAluno)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.dteInicio)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtEmail).HasMaxLength(200);

                entity.Property(e => e.txtRegister)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.intQuestionario)
                    .WithMany(p => p.tblQuestionario_Cliente)
                    .HasForeignKey(d => d.intQuestionarioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestionario_Cliente_tblQuestionario");
            });

            modelBuilder.Entity<tblQuestionario_Detalhes>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.intQuestionarioClienteID)
                    .HasName("_net_IX_tblQuestionario_Detalhes_intQuestionarioClienteID");

                entity.Property(e => e.dteResposta)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtComentario)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.intQuestionarioCliente)
                    .WithMany(p => p.tblQuestionario_Detalhes)
                    .HasForeignKey(d => d.intQuestionarioClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestionario_Detalhes_tblQuestionario_Cliente");
            });

            modelBuilder.Entity<tblQuestionario_Questoes_Alternativas>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtAlternativa).HasMaxLength(255);

                entity.HasOne(d => d.intQuestionario)
                    .WithMany(p => p.tblQuestionario_Questoes_Alternativas)
                    .HasForeignKey(d => d.intQuestionarioID)
                    .HasConstraintName("FK_tblQuestionario_Questoes_Alternativas_tblQuestionario");
            });

            modelBuilder.Entity<tblQuestionario_Tipo_Resposta>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblQuestoesConcursoImagem_Comentario>(entity =>
            {
                entity.HasKey(e => e.intImagemComentarioID)
                    .HasName("PK__tblQuest__EAFCDFFD257CC2DF");

                entity.HasIndex(e => new { e.intImagemComentarioID, e.txtName, e.intQuestaoID })
                    .HasName("_net_IX_tblQuestoesConcursoImagem_Comentario_intQuestaoID");

                entity.Property(e => e.imgImagem)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.txtLabel).HasMaxLength(50);

                entity.Property(e => e.txtName).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<tblQuestoesConcursoImagem_ComentarioLog>(entity =>
            {
                entity.HasKey(e => e.IntComentarioLogId)
                    .HasName("PK__tblQuest__43B445766DCFD028");

                entity.Property(e => e.dtDataAcao).HasColumnType("datetime");

                entity.Property(e => e.txtName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRPA>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.intID, e.txtDadosBancarios, e.bitAtivo })
                    .HasName("_net_IX_tblRPA_bitAtivo");

                entity.HasIndex(e => new { e.intID, e.txtName, e.txtLogin, e.intCargoID, e.txtCelular, e.txtResidencial, e.txtEndereco, e.txtComplemento, e.txtBairro, e.txtCEP, e.txtEmail, e.txtRG, e.txtCPF, e.txtCTPS, e.txtINSS, e.txtDadosBancarios, e.intStatus, e.txtObservacoes, e.txtSenha, e.intCityID, e.bitAtivo, e.dteDataCriacao, e.intUsuarioInclusao, e.intQtdDependentes, e.txtSerieCTPS, e.dteDataCTPS, e.dteDataCadastramentoPis, e.dteDataEmissaoRG, e.dteDataNascimento, e.intEstadoCivil, e.txtFuncao, e.txtGrauInstrucao, e.txtNacionalidade, e.txtRaca, e.txtSexo, e.txtTituloEleitor, e.txtSecaoTituloEleitor, e.txtZonaTituloEleitor, e.txtCertificadoReservista, e.intWarehouseID })
                    .HasName("IX_tblRPA_intWarehouseID_5B500");

                entity.Property(e => e.dteDataCTPS).HasColumnType("datetime");

                entity.Property(e => e.dteDataCadastramentoPis).HasColumnType("datetime");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.dteDataEmissaoRG).HasColumnType("datetime");

                entity.Property(e => e.dteDataNascimento).HasColumnType("datetime");

                entity.Property(e => e.dteDesligamento).HasColumnType("datetime");

                entity.Property(e => e.dteInicio).HasColumnType("datetime");

                entity.Property(e => e.txtBairro).HasMaxLength(50);

                entity.Property(e => e.txtCEP)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.txtCPF).HasMaxLength(11);

                entity.Property(e => e.txtCTPS).HasMaxLength(30);

                entity.Property(e => e.txtCelular).HasMaxLength(15);

                entity.Property(e => e.txtCertificadoReservista).HasMaxLength(15);

                entity.Property(e => e.txtComplemento)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.txtDadosBancarios).HasMaxLength(30);

                entity.Property(e => e.txtEmail).HasMaxLength(100);

                entity.Property(e => e.txtEndereco).HasMaxLength(100);

                entity.Property(e => e.txtFuncao).HasMaxLength(50);

                entity.Property(e => e.txtGrauInstrucao).HasMaxLength(50);

                entity.Property(e => e.txtINSS).HasMaxLength(30);

                entity.Property(e => e.txtLogin).HasMaxLength(30);

                entity.Property(e => e.txtNacionalidade).HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtObservacoes).HasColumnType("nvarchar(max)");

                entity.Property(e => e.txtRG).HasMaxLength(50);

                entity.Property(e => e.txtRaca).HasMaxLength(20);

                entity.Property(e => e.txtResidencial).HasMaxLength(15);

                entity.Property(e => e.txtSecaoTituloEleitor).HasMaxLength(10);

                entity.Property(e => e.txtSenha)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtSerieCTPS).HasMaxLength(20);

                entity.Property(e => e.txtSexo).HasMaxLength(1);

                entity.Property(e => e.txtTituloEleitor).HasMaxLength(20);

                entity.Property(e => e.txtZonaTituloEleitor).HasMaxLength(10);

                entity.HasOne(d => d.intCargo)
                    .WithMany(p => p.tblRPA)
                    .HasForeignKey(d => d.intCargoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPA_tblRPA_Cargos");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblRPA)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblRPA_tblCities");

                entity.HasOne(d => d.intStatusNavigation)
                    .WithMany(p => p.tblRPA)
                    .HasForeignKey(d => d.intStatus)
                    .HasConstraintName("FK_tblRPA_tblRPA_Status");
            });

            modelBuilder.Entity<tblRPAGuia>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteGuia)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteOriginalRecebido).HasColumnType("datetime");

                entity.Property(e => e.dtePagamento).HasColumnType("datetime");

                entity.Property(e => e.txtOBS)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.HasOne(d => d.intStatus)
                    .WithMany(p => p.tblRPAGuia)
                    .HasForeignKey(d => d.intStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPAGuia_tblRPAGuia_Status");
            });

            modelBuilder.Entity<tblProvaVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblProva__6DAE9E969BB0468A");

                entity.Property(e => e.dteLastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRPAGuia_Status>(entity =>
            {
                entity.HasKey(e => e.intStatusID);

                entity.Property(e => e.txtStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRPAGuia_Status_Historico>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.intRPAGuiaID)
                    .HasName("_net_IX_tblRPAGuia_Status_Historico_intRPAGuiaID");

                entity.Property(e => e.dteGuia)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteInclusao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtObservacao)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.HasOne(d => d.intRPAGuia)
                    .WithMany(p => p.tblRPAGuia_Status_Historico)
                    .HasForeignKey(d => d.intRPAGuiaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPAGuia_Status_Historico_tblRPAGuia");

                entity.HasOne(d => d.intStatus)
                    .WithMany(p => p.tblRPAGuia_Status_Historico)
                    .HasForeignKey(d => d.intStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPAGuia_Status_Historico_tblRPAGuia_Status");
            });

            modelBuilder.Entity<tblRPA_AcrescimoProduto>(entity =>
            {
                entity.HasKey(e => e.intID);
            });

            modelBuilder.Entity<tblRPA_Ativo_Historico>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.intRPAID, e.intEmployeeID })
                    .HasName("IX_tblRPA_Ativo_Historico_intRPAID_intEmployeeID_A6E5E");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtMudancas).IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRPA_Ativo_Historico)
                    .HasForeignKey(d => d.intEmployeeID)
                    .HasConstraintName("FK_Employee_Requisicao");

                entity.HasOne(d => d.intRPA)
                    .WithMany(p => p.tblRPA_Ativo_Historico)
                    .HasForeignKey(d => d.intRPAID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPA_Ativo_Historico_tblRPA");
            });

            modelBuilder.Entity<tblRPA_Cargos>(entity =>
            {
                entity.HasKey(e => e.intCargoID);

                entity.Property(e => e.txtCargo)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<tblRPA_DadosBancarios>(entity =>
            {
                entity.HasKey(e => e.intRPADadosBancariosId);

                entity.Property(e => e.txtAgencia)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.txtConta)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.txtTipoConta)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intBanco)
                    .WithMany(p => p.tblRPA_DadosBancarios)
                    .HasForeignKey(d => d.intBancoId)
                    .HasConstraintName("FK_tblRPA_DadosBancarios_tblBanks");

                entity.HasOne(d => d.intRPA)
                    .WithMany(p => p.tblRPA_DadosBancarios)
                    .HasForeignKey(d => d.intRPAId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRPA_DadosBancarios_tblRPA");
            });

            modelBuilder.Entity<tblRPA_Observacao>(entity =>
            {
                entity.HasKey(e => e.intRPAObservacao);

                entity.Property(e => e.txtDescricao).IsRequired();
            });

            modelBuilder.Entity<tblRPA_PermissaoStatus_Employee>(entity =>
            {
                entity.HasKey(e => e.intPermissaoStatusxEmployeeId)
                    .HasName("PK__tblRPA_P__F527916819886CA8");
            });

            modelBuilder.Entity<tblRPA_PermissaoStatus_Responsability>(entity =>
            {
                entity.HasKey(e => e.intPermissaoStatusXResponsabilityID);
            });

            modelBuilder.Entity<tblRPA_PermissaoXEmployee>(entity =>
            {
                entity.HasKey(e => e.intPermissaoxEmployeeId)
                    .HasName("PK__tblRPA_P__BA699C501D58FD8C");
            });

            modelBuilder.Entity<tblRPA_PermissaoXResponsability>(entity =>
            {
                entity.HasKey(e => e.intPermissaoXResponsabilityID)
                    .HasName("PK_tblRPA_PermissaoXSetor");
            });

            modelBuilder.Entity<tblRPA_Status>(entity =>
            {
                entity.HasKey(e => e.intStatusID);

                entity.Property(e => e.txtStatus)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<tblRPA_TipoPermissao>(entity =>
            {
                entity.HasKey(e => e.intTipoPermissaoID);

                entity.Property(e => e.intTipoPermissaoID).ValueGeneratedNever();

                entity.Property(e => e.txtTipoPermissaoNome)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRegions>(entity =>
            {
                entity.HasKey(e => e.intRegionID);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblReplaceHtmlTags>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.txtReplace)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtTag)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblRequisicoes_Anexo>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoAnexoId);

                entity.HasIndex(e => new { e.intEntidadeId, e.intAnexoTipoId, e.bitAtivo })
                    .HasName("_net_IX_");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteAnexoData).HasColumnType("datetime");

                entity.Property(e => e.txtAnexoLink)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRequisicoes_Anexo)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Anexo_tblEmployees");
            });

            modelBuilder.Entity<tblRequisicoes_Ativo>(entity =>
            {
                entity.HasKey(e => e.intAtivoId);

                entity.HasIndex(e => e.intRequisicaoId)
                    .HasName("_net_IX_tblRequisicoes_Ativo_intRequisicaoId");

                entity.HasIndex(e => new { e.intAtivoId, e.txtNumeroSerie, e.bitAtivo, e.txtModelo, e.txtNumeroPatrimonio, e.intStatus, e.txtAcessorio, e.dblValor, e.dteDataCompra, e.txtObservacao, e.intRequisicaoId, e.intProdutoId, e.txtMarca })
                    .HasName("_net_IX_tblRequisicoes_Ativo_intProdutoId_txtMarca");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitImportacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.dblValor)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.dteDataCompra).HasColumnType("datetime");

                entity.Property(e => e.intRequisicaoId).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtAcessorio).HasColumnType("varchar(max)");

                entity.Property(e => e.txtMarca)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.txtModelo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtNumeroPatrimonio)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtNumeroSerie).HasMaxLength(100);

                entity.Property(e => e.txtObservacao).HasColumnType("varchar(max)");

                entity.HasOne(d => d.intCompany)
                    .WithMany(p => p.tblRequisicoes_AtivointCompany)
                    .HasForeignKey(d => d.intCompanyId)
                    .HasConstraintName("FK_tblRequisicoes_Ativo_tblCompanies");

                entity.HasOne(d => d.intCompanyIdOriginalNavigation)
                    .WithMany(p => p.tblRequisicoes_AtivointCompanyIdOriginalNavigation)
                    .HasForeignKey(d => d.intCompanyIdOriginal)
                    .HasConstraintName("FK_tblRequisicoes_Ativo_tblCompanies1");

                entity.HasOne(d => d.intProduto)
                    .WithMany(p => p.tblRequisicoes_Ativo)
                    .HasForeignKey(d => d.intProdutoId)
                    .HasConstraintName("FK_tblRequisicoes_Ativo_tblRequisicoes_Produto");
            });

            modelBuilder.Entity<tblRequisicoes_AtivoMovimentacao>(entity =>
            {
                entity.HasKey(e => e.intAtivoMovimentacaoId);

                entity.HasIndex(e => new { e.dteMovimentacao, e.intTipoMovimentacao, e.intRequisicaoUnidadeId, e.intAtivoId })
                    .HasName("IX_tblRequisicoes_AtivoMovimentacao_intAtivoId_dteMovimentacao_intTipoMovimentacao_intRequisicaoUnidadeId");

                entity.Property(e => e.dteMovimentacao).HasColumnType("datetime");

                entity.Property(e => e.intTipoMovimentacao).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.intAtivo)
                    .WithMany(p => p.tblRequisicoes_AtivoMovimentacao)
                    .HasForeignKey(d => d.intAtivoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_AtivoMovimentacao_tblRequisicoes_Ativo");

                entity.HasOne(d => d.intRequisicaoSetor)
                    .WithMany(p => p.tblRequisicoes_AtivoMovimentacao)
                    .HasForeignKey(d => d.intRequisicaoSetorId)
                    .HasConstraintName("FK_tblRequisicoes_AtivoMovimentacao_tblRequisicoes_Setor");

                entity.HasOne(d => d.intRequisicaoUnidade)
                    .WithMany(p => p.tblRequisicoes_AtivoMovimentacao)
                    .HasForeignKey(d => d.intRequisicaoUnidadeId)
                    .HasConstraintName("FK_tblRequisicoes_AtivoMovimentacao_tblRequisicoes_Unidade");

                entity.HasOne(d => d.intResponsavel)
                    .WithMany(p => p.tblRequisicoes_AtivoMovimentacao)
                    .HasForeignKey(d => d.intResponsavelId)
                    .HasConstraintName("FK_tblRequisicoes_AtivoMovimentacao_tblEmployees");
            });

            modelBuilder.Entity<tblRequisicoes_Ativo_Historico>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoAtivoHistoricoId);

                entity.HasIndex(e => e.intAtivoId)
                    .HasName("IX_tblRequisicoes_Ativo_Historico_intAtivoId_64A68");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.intTipo).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtMudancas).IsUnicode(false);

                entity.HasOne(d => d.intAtivo)
                    .WithMany(p => p.tblRequisicoes_Ativo_Historico)
                    .HasForeignKey(d => d.intAtivoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Ativo_Historico_tblRequisicoes_Ativo");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRequisicoes_Ativo_Historico)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Ativo_Historico_tblEmployees");
            });

            modelBuilder.Entity<tblRequisicoes_Ativo_ProdutoCaracteristica>(entity =>
            {
                entity.HasKey(e => e.intAtivoProdutoCaracteristicaId)
                    .HasName("PK_tblRequisicoesAtivo_ProdutoCaracteristicas");

                entity.HasIndex(e => new { e.intAtivoId, e.intProdutoCaracteristicaId })
                    .HasName("_net_IX_tblRequisicoes_Ativo_ProdutoCaracteristica_intAtivoId");

                entity.Property(e => e.txtValor).IsUnicode(false);

                entity.HasOne(d => d.intAtivo)
                    .WithMany(p => p.tblRequisicoes_Ativo_ProdutoCaracteristica)
                    .HasForeignKey(d => d.intAtivoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoesAtivo_ProdutoCaracteristicas_tblRequisicoes_Ativo");

                entity.HasOne(d => d.intProdutoCaracteristica)
                    .WithMany(p => p.tblRequisicoes_Ativo_ProdutoCaracteristica)
                    .HasForeignKey(d => d.intProdutoCaracteristicaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoesAtivo_ProdutoCaracteristicas_tblRequisicoes_ProdutoCaracteristica");
            });

            modelBuilder.Entity<tblLessonTeachersByGroupAndTitle>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblRequisicoes_Curso>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoCursoId)
                    .HasName("PK_tblRequisicoesCurso");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_Fornecedor>(entity =>
            {
                entity.HasKey(e => e.intFornecedorId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitPessoaFisica)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtAgencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtBairro)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtCEP)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.txtCPFCNPJ)
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.txtCidade)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtComplemento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtConta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtEmail).IsUnicode(false);

                entity.Property(e => e.txtEndereco).IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtNomeFantasia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtObservacao).IsUnicode(false);

                entity.Property(e => e.txtSite).IsUnicode(false);

                entity.Property(e => e.txtTelefone).IsUnicode(false);

                entity.Property(e => e.txtUF)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_FornecedorContato>(entity =>
            {
                entity.HasKey(e => e.intFornecedorContatoId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtTelefone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intFornecedor)
                    .WithMany(p => p.tblRequisicoes_FornecedorContato)
                    .HasForeignKey(d => d.intFornecedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_FornecedorContato_tblRequisicoes_Fornecedor");
            });

            modelBuilder.Entity<tblRequisicoes_FornecedorFormaPagamento>(entity =>
            {
                entity.HasKey(e => e.intFornecedorFormaPagamentoId);

                entity.HasOne(d => d.intFornecedor)
                    .WithMany(p => p.tblRequisicoes_FornecedorFormaPagamento)
                    .HasForeignKey(d => d.intFornecedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_FornecedorFormaPagamento_tblRequisicoes_Fornecedor");
            });

            modelBuilder.Entity<tblRequisicoes_Fornecedor_Produto>(entity =>
            {
                entity.HasKey(e => e.intFornecedorProdutoId);

                entity.HasOne(d => d.intFornecedor)
                    .WithMany(p => p.tblRequisicoes_Fornecedor_Produto)
                    .HasForeignKey(d => d.intFornecedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Fornecedor_Produto_tblRequisicoes_Fornecedor");

                entity.HasOne(d => d.intProduto)
                    .WithMany(p => p.tblRequisicoes_Fornecedor_Produto)
                    .HasForeignKey(d => d.intProdutoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Fornecedor_Produto_tblRequisicoes_Produto");
            });

            modelBuilder.Entity<tblRequisicoes_IntranetLink>(entity =>
            {
                entity.HasKey(e => e.intIntranetLinkId);

                entity.Property(e => e.txtIntranetId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.txtIntranetLink)
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.HasOne(d => d.intRequisicao)
                    .WithMany(p => p.tblRequisicoes_IntranetLink)
                    .HasForeignKey(d => d.intRequisicaoId)
                    .HasConstraintName("FK_tblRequisicoes_IntranetLink_tblRequisicoes_Requisicao");
            });

            modelBuilder.Entity<tblRequisicoes_Perfil>(entity =>
            {
                entity.HasKey(e => e.intPerfilId);

                entity.Property(e => e.bitAprovadorRequisitante).HasDefaultValueSql("((0))");

                entity.Property(e => e.bitAprovadorSubordinado).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_PerfilItem>(entity =>
            {
                entity.HasKey(e => e.intPerfilItemId);

                entity.Property(e => e.bitAutoAprovador).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.intCargo)
                    .WithMany(p => p.tblRequisicoes_PerfilItem)
                    .HasForeignKey(d => d.intCargoId)
                    .HasConstraintName("FK_tblRequisicoes_PerfilItem_Responsabilidade_tblEmployeeCargos");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRequisicoes_PerfilItem)
                    .HasForeignKey(d => d.intEmployeeId)
                    .HasConstraintName("FK_tblRequisicoes_PerfilItem_tblEmployees");

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblRequisicoes_PerfilItem)
                    .HasForeignKey(d => d.intPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_PerfilItem_tblRequisicoes_Perfil");
            });

            modelBuilder.Entity<tblRequisicoes_Produto>(entity =>
            {
                entity.HasKey(e => e.intProdutoId)
                    .HasName("PK_tblRequisioces_Produto");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitImportacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intProdutoGrupo)
                    .WithMany(p => p.tblRequisicoes_Produto)
                    .HasForeignKey(d => d.intProdutoGrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisioces_Produto_tblRequisicoes_ProdutoGrupo");
            });

            modelBuilder.Entity<tblRequisicoes_ProdutoCaracteristica>(entity =>
            {
                entity.HasKey(e => e.intProdutoCaracteristicaId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitImportacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtUnidade)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtValor).IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_ProdutoGrupo>(entity =>
            {
                entity.HasKey(e => e.intProdutoGrupoId)
                    .HasName("PK_tblRequisioces_ProdutoGrupo");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitImportacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_Produto_Historico>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoProdutoHistoricoId);

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.intTipo).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtMudancas).IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRequisicoes_Produto_Historico)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Produto_Historico_tblEmployees");

                entity.HasOne(d => d.intProduto)
                    .WithMany(p => p.tblRequisicoes_Produto_Historico)
                    .HasForeignKey(d => d.intProdutoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Produto_Historico_tblRequisicoes_Produto");
            });

            modelBuilder.Entity<tblRequisicoes_Requisicao>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoId);

                entity.HasIndex(e => new { e.intRequisicaoId, e.intRequisicaoCursoId, e.bitAtivo })
                    .HasName("IX_tblRequisicoes_Requisicao_intRequisicaoCursoId_bitAtivo_F0077");

                entity.HasIndex(e => new { e.intRequisicaoId, e.intRequisicaoSetorId, e.bitAtivo })
                    .HasName("IX_tblRequisicoes_Requisicao_intRequisicaoSetorId_bitAtivo_A2682");

                entity.HasIndex(e => new { e.intRequisicaoId, e.intRequisicaoUnidadeId, e.bitAtivo })
                    .HasName("IX_tblRequisicoes_Requisicao_intRequisicaoUnidadeId_bitAtivo_BEDDE");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dblValor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dtePrevisaoConclusao).HasColumnType("datetime");

                entity.Property(e => e.dteRequisicao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtIntranetLink).IsUnicode(false);

                entity.Property(e => e.txtJustificativaCotacao).IsUnicode(false);

                entity.HasOne(d => d.intAtribuido)
                    .WithMany(p => p.tblRequisicoes_RequisicaointAtribuido)
                    .HasForeignKey(d => d.intAtribuidoId)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblEmployeeAtribuido");

                entity.HasOne(d => d.intCompany)
                    .WithMany(p => p.tblRequisicoes_RequisicaointCompany)
                    .HasForeignKey(d => d.intCompanyId)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblCompanies");

                entity.HasOne(d => d.intCompanyIdOriginalNavigation)
                    .WithMany(p => p.tblRequisicoes_RequisicaointCompanyIdOriginalNavigation)
                    .HasForeignKey(d => d.intCompanyIdOriginal)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblCompanies1");

                entity.HasOne(d => d.intRequisicaoCurso)
                    .WithMany(p => p.tblRequisicoes_Requisicao)
                    .HasForeignKey(d => d.intRequisicaoCursoId)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblRequisicoes_Curso");

                entity.HasOne(d => d.intRequisicaoStatus)
                    .WithMany(p => p.tblRequisicoes_Requisicao)
                    .HasForeignKey(d => d.intRequisicaoStatusId)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblRequisicoes_RequisicaoStatus");

                entity.HasOne(d => d.intRequisitante)
                    .WithMany(p => p.tblRequisicoes_RequisicaointRequisitante)
                    .HasForeignKey(d => d.intRequisitanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblEmployeeRequisitante");

                entity.HasOne(d => d.intResponsavel)
                    .WithMany(p => p.tblRequisicoes_RequisicaointResponsavel)
                    .HasForeignKey(d => d.intResponsavelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Requisicao_tblEmployeeResponsavel");
            });

            modelBuilder.Entity<tblRequisicoes_RequisicaoCotacao>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoCotacaoId);

                entity.HasIndex(e => new { e.intRequisicaoId, e.bitAtivo })
                    .HasName("IX_tblRequisicoes_RequisicaoCotacao_intRequisicaoId_bitAtivo_0BD47");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dblValor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dblValorDesconto).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dblValorFrete).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dblValorUnitario).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dteEscolhido).HasColumnType("datetime");

                entity.Property(e => e.dteVencimento)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtLink).IsUnicode(false);

                entity.Property(e => e.txtObservacao).IsUnicode(false);

                entity.Property(e => e.txtPrazoEntrega)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEscolhidoPor)
                    .WithMany(p => p.tblRequisicoes_RequisicaoCotacao)
                    .HasForeignKey(d => d.intEscolhidoPorId)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoCotacao_tblEmployeeAtribuido");

                entity.HasOne(d => d.intFornecedor)
                    .WithMany(p => p.tblRequisicoes_RequisicaoCotacao)
                    .HasForeignKey(d => d.intFornecedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoCotacao_tblRequisicoes_Fornecedor");

                entity.HasOne(d => d.intRequisicao)
                    .WithMany(p => p.tblRequisicoes_RequisicaoCotacao)
                    .HasForeignKey(d => d.intRequisicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoCotacao_tblRequisicoes_Requisicao");
            });

            modelBuilder.Entity<tblRequisicoes_RequisicaoHistorico>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoHistoricoId);

                entity.HasIndex(e => new { e.intRequisicaoId, e.bitAtivo })
                    .HasName("_net_IX_tblRequisicoes_RequisicaoHistorico_intRequisicaoId");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.intTipo).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtTitulo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRequisicoes_RequisicaoHistorico)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoHistorico_tblEmployees");

                entity.HasOne(d => d.intRequisicao)
                    .WithMany(p => p.tblRequisicoes_RequisicaoHistorico)
                    .HasForeignKey(d => d.intRequisicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoHistorico_tblRequisicoes_Requisicao");
            });

            modelBuilder.Entity<tblRequisicoes_RequisicaoItem>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoItemId);

                entity.HasIndex(e => new { e.intRequisicaoItemId, e.intProdutoId })
                    .HasName("IX_tblRequisicoes_RequisicaoItem_intProdutoId_AE8E4");

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao).IsUnicode(false);

                entity.Property(e => e.txtResumo).IsUnicode(false);

                entity.HasOne(d => d.intProduto)
                    .WithMany(p => p.tblRequisicoes_RequisicaoItem)
                    .HasForeignKey(d => d.intProdutoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_RequisicaoItem_tblRequisicoes_Produto");
            });

            modelBuilder.Entity<tblRequisicoes_RequisicaoItem_ProdutoCaracteristica>(entity =>
            {
                entity.HasKey(e => e.intItemProdutoCaracteristicaId)
                    .HasName("PK_tblRequisicoesRequisicaoItem_ProdutoCaracteristica");

                entity.HasIndex(e => new { e.intRequisicaoItemId, e.intProdutoCaracteristicaId })
                    .HasName("IX_tblRequisicoes_RequisicaoItem_ProdutoCaracteristica_intRequisicaoItemId_intProdutoCaracteristicaId_D692E");

                entity.Property(e => e.txtValor).IsUnicode(false);

                entity.HasOne(d => d.intProdutoCaracteristica)
                    .WithMany(p => p.tblRequisicoes_RequisicaoItem_ProdutoCaracteristica)
                    .HasForeignKey(d => d.intProdutoCaracteristicaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoesRequisicaoItem_ProdutoCaracteristica_tblRequisicoes_ProdutoCaracteristica");

                entity.HasOne(d => d.intRequisicaoItem)
                    .WithMany(p => p.tblRequisicoes_RequisicaoItem_ProdutoCaracteristica)
                    .HasForeignKey(d => d.intRequisicaoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoesRequisicaoItem_ProdutoCaracteristica_tblRequisicoes_RequisicaoItem");
            });

            modelBuilder.Entity<tblRequisicoes_RequisicaoStatus>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoStatusId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_Setor>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoSetorId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_Unidade>(entity =>
            {
                entity.HasKey(e => e.intRequisicaoUnidadeId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.bitMatriz)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_Workflow>(entity =>
            {
                entity.HasKey(e => e.intWorkflowId);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intWorkflowCategoria)
                    .WithMany(p => p.tblRequisicoes_Workflow)
                    .HasForeignKey(d => d.intWorkflowCategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Workflow_tblRequisicoes_WorkflowCategoria");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowAcao>(entity =>
            {
                entity.HasKey(e => e.intWorkflowAcaoId);

                entity.Property(e => e.txtAcao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtCor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtIcone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intWorkflowEtapa)
                    .WithMany(p => p.tblRequisicoes_WorkflowAcaointWorkflowEtapa)
                    .HasForeignKey(d => d.intWorkflowEtapaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowAcao_tblRequisicoes_WorkflowEtapa");

                entity.HasOne(d => d.intWorkflowEtapaProximo)
                    .WithMany(p => p.tblRequisicoes_WorkflowAcaointWorkflowEtapaProximo)
                    .HasForeignKey(d => d.intWorkflowEtapaProximoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowAcao_tblRequisicoes_WorkflowEtapaProximo");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowAcao_Perfil>(entity =>
            {
                entity.HasKey(e => e.intWorkflowAcaoPerfilId);

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblRequisicoes_WorkflowAcao_Perfil)
                    .HasForeignKey(d => d.intPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowAcao_Perfil_tblRequisicoes_Perfil");

                entity.HasOne(d => d.intWorkflowAcao)
                    .WithMany(p => p.tblRequisicoes_WorkflowAcao_Perfil)
                    .HasForeignKey(d => d.intWorkflowAcaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowAcao_Perfil_tblRequisicoes_WorkflowAcao");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowBloqueio>(entity =>
            {
                entity.HasKey(e => e.intWorkflowBloqueioId);

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intWorkflowEtapa)
                    .WithMany(p => p.tblRequisicoes_WorkflowBloqueio)
                    .HasForeignKey(d => d.intWorkflowEtapaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowBloqueio_tblRequisicoes_WorkflowEtapa");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowCampo>(entity =>
            {
                entity.HasKey(e => e.intWorkflowCampoId)
                    .HasName("FK_tblRequisicoes_WorkflowCampo");

                entity.Property(e => e.txtAtributoReferenciado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtEntidadeReferenciada)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtMetdoAPI)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtObjetoMapeado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtPropriedadeMapeada)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowCategoria>(entity =>
            {
                entity.HasKey(e => e.intWorkflowCategoriaId);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowEtapa>(entity =>
            {
                entity.HasKey(e => e.intWorkflowEtapaId);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtStatus)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intWorkflow)
                    .WithMany(p => p.tblRequisicoes_WorkflowEtapa)
                    .HasForeignKey(d => d.intWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowEtapa_tblRequisicoes_Workflow");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowEtapa_Perfil>(entity =>
            {
                entity.HasKey(e => e.intWorkflowEtapaPerfilId);

                entity.HasOne(d => d.intPerfil)
                    .WithMany(p => p.tblRequisicoes_WorkflowEtapa_Perfil)
                    .HasForeignKey(d => d.intPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowEtapa_Perfil_tblRequisicoes_Perfil");

                entity.HasOne(d => d.intWorkflowEtapa)
                    .WithMany(p => p.tblRequisicoes_WorkflowEtapa_Perfil)
                    .HasForeignKey(d => d.intWorkflowEtapaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowEtapa_Perfil_tblRequisicoes_WorkflowEtapa");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowHistorico>(entity =>
            {
                entity.HasKey(e => e.intWorkflowHistoricoId);

                entity.HasIndex(e => new { e.intWorkflowEtapaPosteriorId, e.intWorkflowHistoricoId, e.dteHistorico, e.intWorkflowInstanciaId })
                    .HasName("_net_IX_tblRequisicoes_WorkflowHistorico_intWorkflowInstanciaId");

                entity.Property(e => e.dteHistorico).HasColumnType("datetime");

                entity.Property(e => e.txtJustificativa).IsUnicode(false);

                entity.HasOne(d => d.intWorkflowAcao)
                    .WithMany(p => p.tblRequisicoes_WorkflowHistorico)
                    .HasForeignKey(d => d.intWorkflowAcaoId)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowHistorico_tblRequisicoes_WorkflowAcao");

                entity.HasOne(d => d.intWorkflowEtapaAnterior)
                    .WithMany(p => p.tblRequisicoes_WorkflowHistoricointWorkflowEtapaAnterior)
                    .HasForeignKey(d => d.intWorkflowEtapaAnteriorId)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowHistorico_tblRequisicoes_WorkflowEtapaAnterior");

                entity.HasOne(d => d.intWorkflowEtapaPosterior)
                    .WithMany(p => p.tblRequisicoes_WorkflowHistoricointWorkflowEtapaPosterior)
                    .HasForeignKey(d => d.intWorkflowEtapaPosteriorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowHistorico_tblRequisicoes_WorkflowEtapaPosterior");
            });

            modelBuilder.Entity<tblRequisicoes_WorkflowRegra>(entity =>
            {
                entity.HasKey(e => e.intWorkflowRegraId)
                    .HasName("FK_tblRequisicoes_WorkflowRegra");

                entity.Property(e => e.txtValor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.intWorkflowAcao)
                    .WithMany(p => p.tblRequisicoes_WorkflowRegra)
                    .HasForeignKey(d => d.intWorkflowAcaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowRegra_tblRequisicoes_WorkflowAcao");

                entity.HasOne(d => d.intWorkflowCampo)
                    .WithMany(p => p.tblRequisicoes_WorkflowRegra)
                    .HasForeignKey(d => d.intWorkflowCampoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowRegra_tblRequisicoes_WorkflowCampo");

                entity.HasOne(d => d.intWorkflowEtapa)
                    .WithMany(p => p.tblRequisicoes_WorkflowRegra)
                    .HasForeignKey(d => d.intWorkflowEtapaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_WorkflowRegra_tblRequisicoes_WorkflowEtapa");
            });

            modelBuilder.Entity<tblRequisicoes_Workflow_Requisicao>(entity =>
            {
                entity.HasKey(e => e.intWorkflowRequisicaoId)
                    .HasName("PK_tblRequisicoes__Workflow_Requisicao");

                entity.HasOne(d => d.intRequisicao)
                    .WithMany(p => p.tblRequisicoes_Workflow_Requisicao)
                    .HasForeignKey(d => d.intRequisicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Workflow_Requisicao_tblRequisicoes_Requisicao");

                entity.HasOne(d => d.intWorkflow)
                    .WithMany(p => p.tblRequisicoes_Workflow_Requisicao)
                    .HasForeignKey(d => d.intWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRequisicoes_Workflow_Requisicao_tblRequisicoes_Workflow");
            });

            modelBuilder.Entity<tblResumoAulaIndice>(entity =>
            {
                entity.HasKey(e => e.intResumoAulaIndiceId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblResumoAulaTemaProfessorAssistido>(entity =>
            {
                entity.HasKey(e => e.intTemaProfessorAssistidoID);

                entity.HasIndex(e => new { e.intProfessorID, e.intLessonTitleID, e.intClientID })
                    .HasName("IX_tblResumoAulaTemaProfessorAssistido")
                    .IsUnique();
            });

            modelBuilder.Entity<tblResumoAulaVideo>(entity =>
            {
                entity.HasKey(e => e.intResumoAulaVideoId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);
            });

            modelBuilder.Entity<tblResumoAulaVideoAprovacao>(entity =>
            {
                entity.HasKey(e => e.intId);
            });

            modelBuilder.Entity<tblResumoAulaVideoAprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblResumoAulaVideoCorrigido>(entity =>
            {
                entity.HasKey(e => e.intVideoCorrigidoId);

                entity.Property(e => e.intVideoCorrigidoId).ValueGeneratedNever();

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblResumoAulaVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblResum__6DAE9E9618747E3E");

                entity.Property(e => e.dteLastUpdate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblResumoAulaVideoRelatorioReprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblResum__11B678D228A48B4F");

                entity.Property(e => e.dteReprovacao).HasColumnType("datetime");

                entity.Property(e => e.txtApostilaSigla)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtTema)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtVideoTitulo)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRetiradaMaterialExtensivo>(entity =>
            {
                entity.HasKey(e => e.intRetiradaMaterialExtensivoId);

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRevalidaAulaIndice>(entity =>
            {
                entity.HasKey(e => e.intRevalidaAulaIndiceId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRevalidaAulaTemaProfessorAssistido>(entity =>
            {
                entity.HasKey(e => e.intTemaProfessorAssistidoID);

                entity.HasIndex(e => new { e.intClientID, e.intPercentVisualizado })
                    .HasName("ix_tblRevalidaAulaTemaProfessorAssistido_intClientID_intPercentVisualizado");

                entity.HasIndex(e => new { e.intProfessorID, e.intLessonTitleID, e.intClientID })
                    .HasName("IX_tblRevalidaAulaTemaProfessorAssistido")
                    .IsUnique();
            });

            modelBuilder.Entity<tblRevalidaAulaVideo>(entity =>
            {
                entity.HasKey(e => e.intRevalidaAulaVideoId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);
            });

            modelBuilder.Entity<tblRevalidaAulaVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblReval__6DAE9E9622F20CB1");

                entity.Property(e => e.dteLastUpdate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblRevisaoAulaIndice>(entity =>
            {
                entity.HasKey(e => e.intRevisaoAulaIndiceId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRevisaoAulaTemaProfessorAssistido>(entity =>
            {
                entity.HasKey(e => e.intTemaProfessorAssistidoID);

                entity.HasIndex(e => e.intClientID);

                entity.HasIndex(e => new { e.intLessonTitleID, e.intClientID });

                entity.HasIndex(e => new { e.intProfessorID, e.intLessonTitleID, e.intClientID })
                    .HasName("IX_tblRevisaoAulaTemaProfessorAssistido")
                    .IsUnique();
            });

            modelBuilder.Entity<tblRevisaoAulaVideo>(entity =>
            {
                entity.HasKey(e => e.intRevisaoAulaId);

                entity.HasIndex(e => new { e.intRevisaoAulaId, e.intRevisaoAulaIdPai, e.intProfessorId, e.intRevisaoAulaIndiceId, e.txtDescricao, e.intOrdem, e.intCuePoint, e.intVideoId })
                    .HasName("_net_IX_tblRevisaoAulaVideo_intVideoId");

                entity.HasIndex(e => new { e.intRevisaoAulaId, e.intRevisaoAulaIdPai, e.intProfessorId, e.txtDescricao, e.intOrdem, e.intCuePoint, e.intVideoId, e.intRevisaoAulaIndiceId })
                    .HasName("_net_IX_tblRevisaoAulaVideo_intRevisaoAulaIndiceId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);

                entity.Property(e => e.txtTemp_nomeVideo)
                    .HasMaxLength(600)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRevisaoAulaVideoAprovacao>(entity =>
            {
                entity.HasKey(e => e.intId);
            });

            modelBuilder.Entity<tblRevisaoAulaVideoAprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.dteCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblRevisaoAulaVideoAprovacaoLog)
                    .HasForeignKey(d => d.intEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRevisaoAulaVideoAprovacaoLog_tblEmployees");

                entity.HasOne(d => d.intRevisaoAulaVideoAprovacao)
                    .WithMany(p => p.tblRevisaoAulaVideoAprovacaoLog)
                    .HasForeignKey(d => d.intRevisaoAulaVideoAprovacaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRevisaoAulaVideoAprovacaoLog_tblRevisaoAulaVideoAprovacao");
            });

            modelBuilder.Entity<tblRevisaoAulaVideoCorrigido>(entity =>
            {
                entity.HasKey(e => e.intVideoCorrigidoId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                // entity.HasOne(d => d.intRevisaoAulaVideo)
                //     .WithMany(p => p.tblRevisaoAulaVideoCorrigido)
                //     .HasForeignKey(d => d.intRevisaoAulaVideoId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_tblRevisaoAulaVideoCorrigido_tblRevisaoAulaVideo");
            });

            modelBuilder.Entity<tblRevisaoAulaVideoLog>(entity =>
            {
                entity.HasKey(e => e.intRevisaoAulaVideoLogId);

                entity.HasIndex(e => new { e.dteCadastro, e.intRevisaoAulaId, e.intClientId })
                    .HasName("_net_IX_tblRevisaoAulaVideoLogintRevisaoAulaId");

                entity.HasIndex(e => new { e.intRevisaoAulaId, e.dteCadastro, e.intClientId })
                    .HasName("_net_IX_tblRevisaoAulaVideoLog_intClientId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRevisaoAulaVideoLogPosition>(entity =>
            {
                entity.HasKey(e => e.intLogPositionId)
                    .HasName("PK__tblRevis__6DAE9E967A902BD7");

                entity.HasIndex(e => e.intClientId)
                    .HasName("_net_IX_tblRevisaoAulaVideoLogPosition_intClientId");

                entity.HasIndex(e => new { e.intLogPositionId, e.intSecond, e.dteLastUpdate, e.intLastSecondViewed, e.intClientId, e.intRevisaoAulaVideoId })
                    .HasName("IX_tblRevisaoAulaVideoLogPosition_intClientId_intRevisaoAulaVideoId_B4474");

                entity.Property(e => e.dteLastUpdate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblAlunoCrossPlataformaWhiteList>(entity =>
            {
                entity.HasKey(e => e.intAlunoCrossPlataformaWhiteListId)
                    .HasName("PK__tblAluno__0DB73D1BDA7B6733");

                entity.Property(e => e.dteDataInclusao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblAlunoExcecaoAcessoTablet>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intAlunoExcecaoTabletId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblRevisaoAulaVideoRelatorioReprovacaoLog>(entity =>
            {
                entity.HasKey(e => e.intId)
                    .HasName("PK__tblRevis__11B678D276A71484");

                entity.Property(e => e.dteReprovacao).HasColumnType("datetime");

                entity.Property(e => e.txtApostilaSigla)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtJustificativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtTema)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.txtVideoTitulo)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRevisaoAula_Slides>(entity =>
            {
                entity.HasKey(e => e.intSlideAulaID);

                entity.HasIndex(e => new { e.intLessonTitleID, e.intProfessorID });

                entity.HasIndex(e => new { e.intSlideAulaID, e.intLessonTitleID, e.intOrder, e.intProfessorID })
                    .HasName("_net_IX_tblRevisaoAula_Slides_intProfessorID");

                entity.Property(e => e.guidSlide).HasDefaultValueSql("(newid())");

                entity.Property(e => e.intOrder).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intProfessor)
                    .WithMany(p => p.tblRevisaoAula_Slides)
                    .HasForeignKey(d => d.intProfessorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SlideAula_tblEmployees");
            });

            modelBuilder.Entity<tblRodadaAluno>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblRodad__11B6793256CD08CC");

                entity.HasIndex(e => new { e.intID, e.intClientID, e.intRodadaId })
                    .HasName("IX_tblRodadaAluno_intRodadaId_43672");

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblRodadaAluno)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRodada__intCl__58B5513E");

                entity.HasOne(d => d.intRodada)
                    .WithMany(p => p.tblRodadaAluno)
                    .HasForeignKey(d => d.intRodadaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRodada__intRo__5C85E222");
            });

            modelBuilder.Entity<tblAcademicoVideoEmail>(entity =>
            {
                entity.HasKey(e => e.intAcademicoVideoEmail);

                entity.Property(e => e.dteOcorrencia).HasColumnType("datetime");

                entity.Property(e => e.txtName).IsUnicode(false);

                entity.Property(e => e.txtVideoStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<msp_API_ListaEntidades_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<msp_API_ProgressoAulaRevisaoAluno_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<csp_ListaMaterialDireitoAluno_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<msp_LoadCP_MED_Choice_Result>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<csp_loadMesesCursados_Result>(entity =>
            {
                entity.HasNoKey();
            });
            

            modelBuilder.Entity<msp_Medsoft_SelectModulosPermitidos_Result>(entity =>
            {
                entity.HasNoKey();
            });      

            modelBuilder.Entity<msp_API_ListaApostilas_Result>(entity =>
            {
                entity.HasNoKey();
            });    
            

            modelBuilder.Entity<msp_API_NomeResumido_Result>(entity =>
            {
                entity.HasNoKey();
            });     

            modelBuilder.Entity<msp_HoraAulaTema_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<emed_CursosAnosStatus_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<msp_GetDataLimite_ByApplication_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<csp_getServerDate_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<msp_API_LoadGrandeArea_Result>(entity =>
            {
                entity.HasNoKey();
            });            

            modelBuilder.Entity<csp_CustomClient_PagamentosProdutosGeral_Result>(entity =>
            {
                entity.HasNoKey();
            });         

            modelBuilder.Entity<msp_Medsoft_SelectPermissaoExercicios_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<tblRodadaAvaliacao>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblRodad__11B6793252FC77E8");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");
            });            

            modelBuilder.Entity<msp_Medsoft_SelectImagensComentProfessor_Result>(entity =>
            {
                entity.HasNoKey();
            });

            

            modelBuilder.Entity<tblSchools>(entity =>
            {
                entity.HasKey(e => e.intSchoolID);

                entity.Property(e => e.intStateId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.txtSigla)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<tblSeguranca>(entity =>
            {
                entity.HasKey(e => e.intMsMovelSegurancaId)
                    .HasName("PK__tblSegur__219F2FC64CD190E4");

                entity.HasIndex(e => new { e.intClientId, e.intApplicationId, e.intDeviceId })
                    .HasName("_net_IX_tblSeguranca_intClientId");

                entity.HasIndex(e => new { e.intDeviceId, e.intApplicationId, e.dteCadastro })
                    .HasName("IX_tblSeguranca_intApplicationId_dteCadastro_intDeviceId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDeviceToken)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblSeguranca_log>(entity =>
            {
                entity.HasKey(e => e.intSegurancaLogId)
                    .HasName("PK__tblSegur__957CD1E364AEF021");

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDeviceToken)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.txtJustificativa).IsUnicode(false);
            });

            modelBuilder.Entity<tblSellOrderDetails>(entity =>
            {
                entity.HasKey(e => e.intOrderRecordID)
                    .HasName("PK_tblSellOrderRecords");

                entity.HasIndex(e => e.intMaterialID)
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K7");

                entity.HasIndex(e => e.intOrderID)
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K2");

                entity.HasIndex(e => e.intProductID)
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K3");

                entity.HasIndex(e => new { e.intOrderID, e.intProductID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K3_2");

                entity.HasIndex(e => new { e.intProductID, e.intOrderID })
                    .HasName("_net_IX_tblSellOrderDetails_intOrderID");

                entity.HasIndex(e => new { e.intProductID, e.intOrderRecordID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K1_3");

                entity.HasIndex(e => new { e.intMaterialID, e.intOrderID, e.intProductID })
                    .HasName("_dta_stat_1649440950_7_2_3");

                entity.HasIndex(e => new { e.intOrderID, e.intOrderRecordID, e.intProductID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K1_K3_2");

                entity.HasIndex(e => new { e.intOrderID, e.intProductID, e.intOrderRecordID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K2_K3_K1");

                entity.HasIndex(e => new { e.intProductID, e.intOrderID, e.intOrderRecordID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K3_K2_K1");

                entity.HasIndex(e => new { e.intProductID, e.intOrderRecordID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K1_K2_3");

                entity.HasIndex(e => new { e.dblAmount, e.dblPrice, e.intMaterialID, e.intOrderID, e.intProductID })
                    .HasName("IX_tblSellOrderDetails_1")
                    .IsUnique();

                entity.HasIndex(e => new { e.dblAmount, e.dblPrice, e.intMaterialID, e.intProductID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrderDetails_5_1649440950__K3_K2");

                entity.HasOne(d => d.intOrder)
                    .WithMany(p => p.tblSellOrderDetails)
                    .HasForeignKey(d => d.intOrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSellOrderDetails_tblSellOrders");

                entity.HasOne(d => d.intProduct)
                    .WithMany(p => p.tblSellOrderDetails)
                    .HasForeignKey(d => d.intProductID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSellOrderDetails_tblProducts");
            });

            modelBuilder.Entity<tblSellOrders>(entity =>
            {
                entity.HasKey(e => e.intOrderID);

                entity.HasIndex(e => e.intClientID)
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2");

                entity.HasIndex(e => e.intConditionTypeID)
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K13");

                entity.HasIndex(e => e.intOrderID)
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1");

                entity.HasIndex(e => e.intStatus)
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_2");

                entity.HasIndex(e => new { e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_2");

                entity.HasIndex(e => new { e.intClientID, e.intStoreID })
                    .HasName("_dta_stat_1680725040_2_3");

                entity.HasIndex(e => new { e.intClientID, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_2");

                entity.HasIndex(e => new { e.intConditionTypeID, e.intClientID })
                    .HasName("_dta_stat_1680725040_13_2");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_1");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_1");

                entity.HasIndex(e => new { e.intOrderID, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K5");

                entity.HasIndex(e => new { e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_3");

                entity.HasIndex(e => new { e.txtComment, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K2");

                entity.HasIndex(e => new { e.txtComment, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K1");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K4_K2_K1");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_4");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K4");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K12");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K3");

                entity.HasIndex(e => new { e.intClientID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K1");

                entity.HasIndex(e => new { e.intClientID, e.intStoreID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K3_K1");

                entity.HasIndex(e => new { e.intClientID, e.txtComment, e.intConditionTypeID })
                    .HasName("_dta_stat_1680725040_2_5_13");

                entity.HasIndex(e => new { e.intConditionTypeID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K13_K1_K6");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K4_1_2");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_1");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K12");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K3");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_1");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K3");

                entity.HasIndex(e => new { e.intOrderID, e.intStoreID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K3_K2");

                entity.HasIndex(e => new { e.intStatus, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K1");

                entity.HasIndex(e => new { e.intStatus, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K1_K2");

                entity.HasIndex(e => new { e.intStatus, e.intStoreID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K3_K1");

                entity.HasIndex(e => new { e.intStatus2, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K12_K2_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K1_K2");

                entity.HasIndex(e => new { e.intStoreID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K6_K1");

                entity.HasIndex(e => new { e.txtComment, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_5");

                entity.HasIndex(e => new { e.txtComment, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_5");

                entity.HasIndex(e => new { e.txtComment, e.intStatus, e.intClientID })
                    .HasName("_dta_stat_1680725040_5_6_2");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K12_4");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intStatus2, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K12_K1_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.intStatus, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K1_K2_4");

                entity.HasIndex(e => new { e.intClientID, e.dteDate, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_2_4");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStatus, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_K12");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_K3");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStatus, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_K5");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStatus2, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K12_K6");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStoreID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K3_K4");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intStoreID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K3_K6");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K5_K6");

                entity.HasIndex(e => new { e.intClientID, e.intStatus, e.intOrderID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K1_K3");

                entity.HasIndex(e => new { e.intClientID, e.intStatus, e.intStatus2, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K12_K1");

                entity.HasIndex(e => new { e.intClientID, e.intStoreID, e.intOrderID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K3_K1_K4");

                entity.HasIndex(e => new { e.intClientID, e.intStoreID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K3_K6_K1");

                entity.HasIndex(e => new { e.intClientID, e.txtComment, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K5_K1_K6");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K12_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intConditionTypeID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_1_2_13");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_K12");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K3_1");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K5_1_2");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStatus2, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K12_K6_1");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K3_K4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K3_K6");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K5_K6");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus, e.intClientID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K2_K3");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus, e.intClientID, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K5_1");

                entity.HasIndex(e => new { e.intOrderID, e.intStatus, e.intStoreID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K3_K2");

                entity.HasIndex(e => new { e.intOrderID, e.intStoreID, e.dteDate, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K4D_K2_1_3");

                entity.HasIndex(e => new { e.intStatus, e.intClientID, e.intOrderID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K1_K3");

                entity.HasIndex(e => new { e.intStatus, e.intClientID, e.intOrderID, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K1_K5");

                entity.HasIndex(e => new { e.intStatus, e.intOrderID, e.intClientID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K1_K2_K3");

                entity.HasIndex(e => new { e.intStatus, e.intStoreID, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_8_1680725040__K6_K3_K2_K1");

                entity.HasIndex(e => new { e.intStatus, e.intStoreID, e.intClientID, e.intStatus2 })
                    .HasName("_dta_stat_1680725040_6_3_2_12");

                entity.HasIndex(e => new { e.intStatus, e.intStoreID, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K3_K1_K2");

                entity.HasIndex(e => new { e.intStatus2, e.intStatus, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K12_K6_K2_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_net_IX_tblSellOrders_intOrderID_intStatus");

                entity.HasIndex(e => new { e.intStoreID, e.intClientID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K2_K6_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intOrderID, e.intClientID, e.dteDate })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K1_K2_K4");

                entity.HasIndex(e => new { e.intStoreID, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_3");

                entity.HasIndex(e => new { e.intStoreID, e.intStatus, e.intOrderID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K6_K1_K2");

                entity.HasIndex(e => new { e.txtComment, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K2_K1_K6");

                entity.HasIndex(e => new { e.txtComment, e.intClientID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K1_5");

                entity.HasIndex(e => new { e.txtComment, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_5");

                entity.HasIndex(e => new { e.txtComment, e.intStatus, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K6_K2_K1");

                entity.HasIndex(e => new { e.txtComment, e.intStatus, e.intConditionTypeID, e.intClientID })
                    .HasName("_dta_stat_1680725040_5_6_13_2");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_K3_4");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K5_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.txtComment, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K5_K1_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intClientID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_K3_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intClientID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K5_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intStatus, e.intStoreID, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K3_K2_4");

                entity.HasIndex(e => new { e.dteDate, e.intStatus, e.intClientID, e.intOrderID, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K1_K3_4");

                entity.HasIndex(e => new { e.dteDate, e.txtComment, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K2_K1_K6_4");

                entity.HasIndex(e => new { e.dteDate, e.txtComment, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_4_5");

                entity.HasIndex(e => new { e.dteDate, e.txtComment, e.intStatus, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_K1_4_5");

                entity.HasIndex(e => new { e.intClientID, e.intOrderID, e.intConditionTypeID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K13_K5_K6");

                entity.HasIndex(e => new { e.intClientID, e.intStatus2, e.intOrderID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K3_2_12");

                entity.HasIndex(e => new { e.intClientID, e.txtComment, e.intConditionTypeID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K5_K13_K1_K6");

                entity.HasIndex(e => new { e.intConditionTypeID, e.intClientID, e.intOrderID, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K13_K2_K1_K5_K6");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K3_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID, e.intStatus2, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K12_K6_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.txtComment, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_1_4_5");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.txtComment, e.intStatus, e.intClientID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K2_1_4_5");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.dteDate, e.intStatus, e.intStatus2 })
                    .HasName("IX_tblSellOrders_intStatus_intStatus2_A28A8");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.dteDate, e.intStatus, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K5_1_2_4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.dteDate, e.txtComment, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K6_1_2_4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intConditionTypeID, e.intStatus, e.txtComment })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K5_1_2_13");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intConditionTypeID, e.txtComment, e.intStatus })
                    .HasName("_dta_stat_1680725040_1_2_13_5_6");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.txtComment, e.intStatus2, e.intStatus })
                    .HasName("net_IX_tblSellOrders_Status2");

                entity.HasIndex(e => new { e.intOrderID, e.txtComment, e.intClientID, e.intStoreID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K3_K6_1_5");

                entity.HasIndex(e => new { e.intStoreID, e.intStatus2, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_3_12");

                entity.HasIndex(e => new { e.intStoreID, e.intStatus2, e.intClientID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K1_3_12");

                entity.HasIndex(e => new { e.intStoreID, e.intStatus2, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_3_12");

                entity.HasIndex(e => new { e.txtComment, e.intClientID, e.intConditionTypeID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K2_K13_K1_K6");

                entity.HasIndex(e => new { e.txtComment, e.intClientID, e.intStoreID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K3_K1_K6_5");

                entity.HasIndex(e => new { e.txtComment, e.intOrderID, e.intClientID, e.intStoreID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K3_K6_5");

                entity.HasIndex(e => new { e.txtComment, e.intStatus, e.intConditionTypeID, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K5_K6_K13_K2_K1");

                entity.HasIndex(e => new { e.txtComment, e.intStoreID, e.intStatus, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K3_K6_K2_K1_5");

                entity.HasIndex(e => new { e.dteDate, e.intClientID, e.intOrderID, e.intStatus, e.intStoreID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_K3_K12_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intClientID, e.intStatus, e.intStoreID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K2_K6_K3_K12_4");

                entity.HasIndex(e => new { e.dteDate, e.intOrderID, e.intStatus, e.intStoreID, e.intClientID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1_K6_K3_K2_K12_4");

                entity.HasIndex(e => new { e.dteDate, e.intStatus, e.intStoreID, e.intClientID, e.intStatus2, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K3_K2_K12_K1_4");

                entity.HasIndex(e => new { e.dteDate, e.intStatus, e.intStoreID, e.intOrderID, e.intClientID, e.intStatus2 })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K6_K3_K1_K2_K12_4");

                entity.HasIndex(e => new { e.intOrderID, e.dteDate, e.intClientID, e.intStatus2, e.intStatus, e.intStoreID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K12_K6_K3_1_4");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.intConditionTypeID, e.dteDate, e.intStatus })
                    .HasName("_net_IX_tblSellOrders_intConditionTypeID_dteDate");

                entity.HasIndex(e => new { e.intStoreID, e.txtComment, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intClientID, e.intOrderID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_K6_3_5_7_8_10_12_13");

                entity.HasIndex(e => new { e.intStoreID, e.txtComment, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intClientID, e.intStatus, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K6_K1_3_5_7_8_10_12_13");

                entity.HasIndex(e => new { e.intStoreID, e.txtComment, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intOrderID, e.intClientID, e.intStatus })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K1D_K2_K6_3_5_7_8_10_12_13");

                entity.HasIndex(e => new { e.intStoreID, e.txtComment, e.intStatus, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intClientID, e.intOrderID })
                    .HasName("_dta_index_tblSellOrders_5_1680725040__K2_K1_3_5_6_7_8_10_12_13");

                entity.HasIndex(e => new { e.dteDate, e.intStoreID, e.txtComment, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intConditionTypeID, e.intOrderID, e.intClientID, e.intStatus, e.intStatus2 })
                    .HasName("IX_tblSellOrdersClientStatus");

                entity.HasIndex(e => new { e.intClientID, e.intStoreID, e.intOrderID, e.txtComment, e.intSellerID, e.intShippingMethodID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intStatus, e.dteDate })
                    .HasName("_net_IX_tblSellOrders_intStatus");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.dteDate, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.txtComment, e.intStatus })
                    .HasName("_net_IX_tblSellOrders_txtComment_intStatus");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.dteDate, e.txtComment, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intStatus })
                    .HasName("_net_IX_tblSellOrders_intConditionTypeID");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.dteDate, e.txtComment, e.intStatus, e.intShippingMethodID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intSellerID })
                    .HasName("_net_IX_tblSellOrders_intSellerID");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.intStoreID, e.txtComment, e.intStatus, e.intShippingMethodID, e.intSellerID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.dteDate })
                    .HasName("_net_IX_tblSellOrders_dteDate");

                entity.HasIndex(e => new { e.intOrderID, e.intClientID, e.txtComment, e.dteDate, e.intSellerID, e.intShippingMethodID, e.intAgreementID, e.intStatus2, e.intConditionTypeID, e.intStoreID, e.intStatus })
                    .HasName("_net_IX_tblSellOrders_intStoreID_intStatus");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.intConditionTypeID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStatus2).HasDefaultValueSql("(0)");

                entity.Property(e => e.txtComment).HasMaxLength(200);

                entity.HasOne(d => d.intClient)
                    .WithMany(p => p.tblSellOrders)
                    .HasForeignKey(d => d.intClientID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSellOrders_tblClients");

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblSellOrders)
                    .HasForeignKey(d => d.intStoreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSellOrders_tblStores");
            });

            modelBuilder.Entity<tblSellOrdersTemplate>(entity =>
            {
                entity.HasKey(e => e.intOrderId);

                entity.Property(e => e.intOrderId).ValueGeneratedNever();

                entity.HasOne(d => d.intOrder)
                    .WithOne(p => p.tblSellOrdersTemplate)
                    .HasForeignKey<tblSellOrdersTemplate>(d => d.intOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSellOrdersTemplate_tblSellOrders");
            });

            modelBuilder.Entity<tblShippingCompanies>(entity =>
            {
                entity.HasKey(e => e.intCompanyID);

                entity.Property(e => e.txtAddress1).HasMaxLength(50);

                entity.Property(e => e.txtAddress2).HasMaxLength(50);

                entity.Property(e => e.txtComment).HasMaxLength(300);

                entity.Property(e => e.txtFantasyName).HasMaxLength(100);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(50);

                entity.Property(e => e.txtPhone).HasMaxLength(50);

                entity.Property(e => e.txtRegisterCode).HasMaxLength(50);

                entity.Property(e => e.txtRegistrationName).HasMaxLength(100);

                entity.Property(e => e.txtZipCode).HasMaxLength(50);

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblShippingCompanies)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblShippingCompanies_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblShippingCompanies)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblShippingCompanies_tblCities");
            });

            modelBuilder.Entity<tblSimuladoTipos>(entity =>
            {
                entity.HasKey(e => e.intTipoSimuladoID);

                entity.Property(e => e.intTipoSimuladoID).ValueGeneratedNever();

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblSitePage_AsTurmas_Enderecos>(entity =>
            {
                entity.HasKey(e => e.intEnderecoID);

                entity.HasIndex(e => e.intStoreID)
                    .HasName("_net_IX_tblSitePage_AsTurmas_Enderecos_intStoreID");

                entity.Property(e => e.txtDetalhe)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.txtGoogleMapLink).HasMaxLength(3000);

                entity.Property(e => e.txtLat)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.txtLng)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.txtLocal).HasMaxLength(3000);
            });

            modelBuilder.Entity<tblSitePage_AsTurmas_parametros>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblSiteP__11B679327719B3CB");

                entity.HasIndex(e => new { e.intStoreID, e.bitReprise, e.bitActive });

                entity.HasIndex(e => new { e.txtTurmaDiaSemana, e.dteTimeInicio, e.dteTimeFim, e.intCourseID })
                    .HasName("_net_IX_tblSitePage_AsTurmas_parametros_intCourseID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dteTimeFim).HasColumnType("datetime");

                entity.Property(e => e.dteTimeInicio).HasColumnType("datetime");

                entity.Property(e => e.txtCourseName).HasMaxLength(100);

                entity.Property(e => e.txtCourseType).HasMaxLength(100);

                entity.Property(e => e.txtProductName).HasMaxLength(4000);

                entity.Property(e => e.txtStoreName).HasMaxLength(100);

                entity.Property(e => e.txtTurmaDiaSemana).HasMaxLength(100);
            });

            modelBuilder.Entity<tblSitePagesGeral>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.dteEndDate).HasColumnType("datetime");

                entity.Property(e => e.dteStartDate).HasColumnType("datetime");

                entity.Property(e => e.intPageID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtCountry).HasMaxLength(50);

                entity.Property(e => e.txtPageDescription).HasMaxLength(400);

                entity.Property(e => e.txtPageName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtPage_ImgFileName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtState).HasMaxLength(50);
            });

            modelBuilder.Entity<tblDuvidasAcademicas_DuvidasArquivadas>(entity => 
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<tblSmtpConfig>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.displayFrom)
                    .HasMaxLength(32)
                    .IsFixedLength();

                entity.Property(e => e.profile_id)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsFixedLength()
                    .HasDefaultValueSql("(N'ses')");

                entity.Property(e => e.smtpFrom)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.smtpHost)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.smtpID).ValueGeneratedOnAdd();

                entity.Property(e => e.smtpPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.smtpUsername)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblSoulMedicina_Tipo>(entity =>
            {
                entity.HasKey(e => e.intSoulMedicina_TipoId)
                    .HasName("PK__tblSoulM__0E78F581690D14E5");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblSpecialChars>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.txtChar).HasMaxLength(30);
            });

            modelBuilder.Entity<tblStates>(entity =>
            {
                entity.HasKey(e => e.intStateID);

                entity.Property(e => e.txtCaption).HasMaxLength(50);

                entity.Property(e => e.txtCodigoIBGE)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.txtDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.intCountry)
                    .WithMany(p => p.tblStates)
                    .HasForeignKey(d => d.intCountryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStates_tblCountries");

                entity.HasOne(d => d.intRegion)
                    .WithMany(p => p.tblStates)
                    .HasForeignKey(d => d.intRegionID)
                    .HasConstraintName("FK_tblStates_tblRegions");
            });

            modelBuilder.Entity<tblConcurso>(entity =>
            {
                entity.HasKey(e => e.ID_CONCURSO)
                    .HasName("PK_tblConcurso_1");

                entity.HasIndex(e => e.DT_ULTIMA_ATUALIZACAO)
                    .HasName("_net_IX_tblConcurso_DT_ULTIMA_ATUALIZACAO");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.CD_UF })
                    .HasName("IX_tblConcurso_CD_UF_ID_CONCURSO");

                entity.HasIndex(e => new { e.NM_CONCURSO, e.VL_ANO_CONCURSO })
                    .HasName("_net_IX_tblConcurso_NM_Concurso_VL_ANO_CONCURSO");

                entity.HasIndex(e => new { e.DT_ULTIMA_ATUALIZACAO, e.VL_ANO_CONCURSO, e.CD_UF })
                    .HasName("_net_IX_tblConcurso_VL_ANO_CONCURSO_CD_UF");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.NM_CONCURSO, e.VL_ANO_CONCURSO, e.SG_CONCURSO })
                    .HasName("_net_IX_tblConcurso_SG_CONCURSO");

                entity.HasIndex(e => new { e.ID_CONCURSO, e.NM_CONCURSO, e.VL_ANO_CONCURSO, e.SG_CONCURSO, e.CD_UF })
                    .HasName("IXFull_tblConcurso");

                entity.HasIndex(e => new { e.SG_CONCURSO, e.CD_UF, e.NM_CONCURSO, e.VL_ANO_CONCURSO, e.ID_CONCURSO })
                    .HasName("_dta_index_tblConcurso_5_677837727__K3_K1_14_15");

                entity.Property(e => e.CD_UF)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DH_ATUALIZACAO_SITE).HasColumnType("datetime");

                entity.Property(e => e.DIVULGACAO_GABARITO).HasColumnType("datetime");

                entity.Property(e => e.DT_EDITAL)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DT_GERAL)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DT_INSCRICAO)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DT_ULTIMA_ATUALIZACAO).HasColumnType("datetime");

                entity.Property(e => e.DT_VCTO_LINK1).HasColumnType("datetime");

                entity.Property(e => e.DT_VCTO_LINK2).HasColumnType("datetime");

                entity.Property(e => e.DT_VCTO_LINK3).HasColumnType("datetime");

                entity.Property(e => e.DT_VCTO_LINK4).HasColumnType("datetime");

                entity.Property(e => e.DT_VCTO_LINK5).HasColumnType("datetime");

                entity.Property(e => e.EDITAL2)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EDITAL_DIVULGADO)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FIM_INSCRICOES).HasColumnType("datetime");

                entity.Property(e => e.FL_ENVIAR_EMAIL)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NM_ARQ_PAGINA)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NM_CIDADE)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NM_CONCURSO)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NR_TELEFONE_1)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NR_TELEFONE_2)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PRAZO_RECURSO).HasColumnType("datetime");

                entity.Property(e => e.PRAZO_RECURSO_ATE).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_1).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_1_ATE).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_2).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_2_ATE).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_3).HasColumnType("datetime");

                entity.Property(e => e.PROVA_FASE_3_ATE).HasColumnType("datetime");

                entity.Property(e => e.RESULTADO_FASE_1).HasColumnType("datetime");

                entity.Property(e => e.RESULTADO_FASE_2).HasColumnType("datetime");

                entity.Property(e => e.RESULTADO_FASE_3).HasColumnType("datetime");

                entity.Property(e => e.ROTULO_EDITAL1)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ROTULO_EDITAL2)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.SG_CONCURSO)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.TX_EMAIL_1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TX_EMAIL_2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TX_OBSERVACAO).HasColumnType("text");

                entity.Property(e => e.TX_ROTULO_LINK1)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_ROTULO_LINK2)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_ROTULO_LINK3)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_ROTULO_LINK4)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_ROTULO_LINK5)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_SITE_APROVADOS)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_SITE_CONCURSO)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_SITE_EDITAL)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TX_TEXTO_LINK1)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_TEXTO_LINK2)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_TEXTO_LINK3)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_TEXTO_LINK4)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_TEXTO_LINK5)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TX_URL_LINK1)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TX_URL_LINK2)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TX_URL_LINK3)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TX_URL_LINK4)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TX_URL_LINK5)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblStore_CombosPaymentTemplate>(entity =>
            {
                entity.HasKey(e => new { e.intStoreID, e.intComboID, e.intPaymentTemplateID })
                    .HasName("PK_tblStore_Product_PaymentOption");

                entity.HasIndex(e => new { e.intComboID, e.bitInternet })
                    .HasName("_net_IX_tblStore_CombosPaymentTemplate_bitInternet");

                entity.HasIndex(e => new { e.intPaymentTemplateID, e.intComboID })
                    .HasName("_net_IX_tblStore_CombosPaymentTemplate_intPaymentTemplateID");

                entity.HasIndex(e => new { e.bitActive, e.intComboID, e.intStoreID })
                    .HasName("_dta_index_tblStore_CombosPaymentTemplate_5_1854629650__K4_K2_K1");

                entity.HasIndex(e => new { e.bitActive, e.intStoreID, e.intComboID })
                    .HasName("_dta_index_tblStore_CombosPaymentTemplate_5_1854629650__K4_K1_K2");

                entity.HasIndex(e => new { e.intComboID, e.intStoreID, e.bitActive })
                    .HasName("_dta_index_tblStore_CombosPaymentTemplate_5_1854629650__K1_K4_2");

                entity.Property(e => e.bitActive).HasDefaultValueSql("(0)");

                entity.Property(e => e.bitInternet).HasDefaultValueSql("(0)");

                entity.Property(e => e.bitProducao)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblStore_CombosPaymentTemplate)
                    .HasForeignKey(d => d.intStoreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStore_CombosPaymentTemplate_tblStores");
            });

            modelBuilder.Entity<tblStore_Product_PaymentTemplate>(entity =>
            {
                entity.HasKey(e => new { e.intStoreID, e.intProductID, e.intPaymentOptionID });

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intProductID })
                    .HasName("_net_IX_tblStore_Product_PaymentTemplate_intPaymentOptionID");

                entity.HasIndex(e => new { e.intProductID, e.bitActive, e.intStoreID })
                    .HasName("_dta_index_tblStore_Product_PaymentTemplate_5_1485300401__K2_K4_K1");

                entity.HasIndex(e => new { e.intStoreID, e.intProductID, e.bitActive })
                    .HasName("_dta_index_tblStore_Product_PaymentTemplate_5_1485300401__K1_K2_K4");

                entity.HasIndex(e => new { e.intPaymentOptionID, e.intStoreID, e.bitActive, e.intProductID })
                    .HasName("_dta_index_tblStore_Product_PaymentTemplate_5_1485300401__K1_K4_K2_3");

                entity.HasIndex(e => new { e.intProductID, e.intStoreID, e.intPaymentOptionID, e.bitActive, e.bitInternet })
                    .HasName("_dta_index_tblStore_Product_PaymentTemplate_5_1485300401__K2_K1_K3_K4_K5");

                entity.HasIndex(e => new { e.intStoreID, e.intProductID, e.intPaymentOptionID, e.bitActive, e.bitProducao, e.bitInternet })
                    .HasName("_net_IX_tblStore_Product_PaymentTemplate_bitInternet");

                entity.HasIndex(e => new { e.intStoreID, e.intProductID, e.intPaymentOptionID, e.bitInternet, e.bitProducao, e.bitActive })
                    .HasName("_net_IX_tblStore_Product_PaymentTemplate_bitactive");

                entity.Property(e => e.bitProducao)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblStores>(entity =>
            {
                entity.HasKey(e => e.intStoreID)
                    .HasName("PK_tblStored");

                entity.HasIndex(e => new { e.txtStoreName, e.intStoreID })
                    .HasName("_net_IX_tblStores_intStoreID");

                entity.Property(e => e.bitEnableInternetSales).HasDefaultValueSql("(0)");

                entity.Property(e => e.txtAddress1).HasMaxLength(100);

                entity.Property(e => e.txtAddress2).HasMaxLength(100);

                entity.Property(e => e.txtContract).HasMaxLength(4000);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(50);

                entity.Property(e => e.txtStoreName).HasMaxLength(50);

                entity.Property(e => e.txtZipCode).HasMaxLength(50);

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblStores)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblStores_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblStores)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblStores_tblCities");
            });

            modelBuilder.Entity<tblStores_Complement>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.txtGoogleMapLink).HasMaxLength(4000);

                entity.Property(e => e.txtInfoAviso).HasMaxLength(4000);

                entity.Property(e => e.txtInfoTitle).HasMaxLength(300);

                entity.Property(e => e.txtLat)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.txtLng)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.txtRevisoesAviso).HasMaxLength(4000);

                entity.Property(e => e.txtRevisoesTitle).HasMaxLength(300);

                entity.Property(e => e.txtSateliteAviso).HasMaxLength(4000);
            });

            modelBuilder.Entity<tblStores_New>(entity =>
            {
                entity.HasKey(e => new { e.intStoreID, e.intYear });
            });

            modelBuilder.Entity<tblStores_Satelites>(entity =>
            {
                entity.HasKey(e => e.intStoreID);

                entity.Property(e => e.intStoreID).ValueGeneratedNever();

                entity.Property(e => e.txtState)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intStore)
                    .WithOne(p => p.tblStores_Satelites)
                    .HasForeignKey<tblStores_Satelites>(d => d.intStoreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStores_Satelites_tblStores");
            });

            modelBuilder.Entity<tblTeacher>(entity =>
            {
                entity.HasKey(e => e.intTeacherID);

                entity.Property(e => e.intTeacherID).ValueGeneratedNever();

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblTelas>(entity =>
            {
                entity.HasKey(e => e.intTelaID)
                    .HasName("PK__tblTelas__5F67EBFC29BD5690");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.txtURL)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<tblTelas1>(entity =>
            {
                entity.HasKey(e => e.intTelaID)
                    .HasName("PK__tblTelas__5F67EBFC0912C242");

                entity.ToTable("tblTelas", "MEDBARRA\\alysson.silva");

                entity.Property(e => e.txtNome)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.txtURL)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<tblTemplateDescontoTurmaCPMED>(entity =>
            {
                entity.HasKey(e => e.intTemplateCPMEDId)
                    .HasName("PK__tblTempl__B1B3FE3437A7C2B4");

                entity.HasOne(d => d.intPorcentagemDesconto)
                    .WithMany(p => p.tblTemplateDescontoTurmaCPMED)
                    .HasForeignKey(d => d.intPorcentagemDescontoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTempla__intPo__59842C37");
            });

            modelBuilder.Entity<tblTipoAcaoSimuladoImpresso>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblTipoA__11B6793232A52F6E");

                entity.Property(e => e.txtAcao)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblTipoAcessoLogin>(entity =>
            {
                entity.HasKey(e => e.intTipoAcessoLoginID);

                entity.Property(e => e.intTipoAcessoLoginID).ValueGeneratedNever();

                entity.Property(e => e.txtTipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblTipoInteracao>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intTipoInteracaoId).ValueGeneratedOnAdd();

                entity.Property(e => e.txtDescricao)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblTipoProcedimentoEntrada>(entity =>
            {
                entity.HasKey(e => e.intTipoProcedimentoEntradaId);

                entity.Property(e => e.strTipoProcedimentoEntrada)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblTipoQuestaoPPT>(entity =>
            {
                entity.HasKey(e => e.intTipoQuestaoID);

                entity.Property(e => e.txtTipoNome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<tblVendasData>(entity =>
            {
                entity.Property(e => e.dteDate).HasColumnType("date");
            });

            modelBuilder.Entity<tblVideoLog>(entity =>
            {
                entity.HasKey(e => e.intVideoLogId);

                entity.HasIndex(e => new { e.intVideoLogId, e.intClientId, e.dteCadastro, e.intTipoVideo, e.intOrigemVideoId })
                    .HasName("IX_tblVideoLog_intTipoVideo_intOrigemVideoId_intVideoLogId_intClientId_dteCadastro");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblVideoMedme>(entity =>
            {
                entity.HasKey(e => e.intVideoMedmeID)
                    .HasName("PK__tblVideo__C72C39562A992D91");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.intVideoMedmeIndice)
                    .WithMany(p => p.tblVideoMedme)
                    .HasForeignKey(d => d.intVideoMedmeIndiceID)
                    .HasConstraintName("FK__tblVideoM__intVi__2C817603");
            });

            modelBuilder.Entity<tblVideoMedmeIndice>(entity =>
            {
                entity.HasKey(e => e.intVideoIndiceID)
                    .HasName("PK__tblVideo__6EE4E588150F0AF2");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

              modelBuilder.Entity<mview_ProdutosPorFilial>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_ProdutosPorFilial");
            });


            modelBuilder.Entity<tblVideoVote>(entity =>
            {
                entity.HasKey(e => e.intVideoVoteID);

                entity.HasIndex(e => new { e.intVideoID, e.intContactID, e.intTipoInteracaoId })
                    .HasName("ix_tblVideoVote_intVideoID_intContactID_intTipoInteracaoId");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblVideoVote)
                    .HasForeignKey(d => d.intBookID)
                    .HasConstraintName("FK_tblVideoVote_tblBooks");

                entity.HasOne(d => d.intContact)
                    .WithMany(p => p.tblVideoVote)
                    .HasForeignKey(d => d.intContactID)
                    .HasConstraintName("FK_tblVideoVote_tblPersons");
            });

            modelBuilder.Entity<tblVideo_Book>(entity =>
            {
                entity.HasKey(e => new { e.intBookID, e.txtVideoCode });

                entity.HasIndex(e => e.intVideoID)
                    .HasName("_net_IX_tblVideo_Book_intVideoID");

                entity.HasIndex(e => e.txtVideoCode)
                    .HasName("_net_IX_tblVideo_Book_txtVideoCode");

                entity.Property(e => e.txtVideoCode).HasMaxLength(50);

                entity.Property(e => e.bitAutoStart)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteAutorizationDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteLastModifyDate).HasColumnType("datetime");

                entity.Property(e => e.dtePublishDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblVideo_Book_Intro>(entity =>
            {
                entity.HasKey(e => e.intBookID)
                    .HasName("PK_tblVideo_Book_intro");

                entity.Property(e => e.intBookID).ValueGeneratedNever();

                entity.Property(e => e.dteCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteLastModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.intBook)
                    .WithOne(p => p.tblVideo_Book_Intro)
                    .HasForeignKey<tblVideo_Book_Intro>(d => d.intBookID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVideo_Book_Intro_tblVideo_Book_Intro");
            });

            modelBuilder.Entity<tblVideo_Questao_Concurso>(entity =>
            {
                entity.HasKey(e => e.intQuestaoID);

                entity.HasIndex(e => e.intVideoID)
                    .HasName("_net_IX_tblVideo_Questao_Concurso_intVideoID");

                entity.Property(e => e.intQuestaoID).ValueGeneratedNever();

                entity.Property(e => e.dteCreationDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteLastModifyDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<tblVideo_SoulMedicina>(entity =>
            {
                entity.HasKey(e => e.intSoulMedicinaId)
                    .HasName("PK__tblVideo__7C68E989B6D9E7DC");

                entity.HasOne(d => d.intSoulMedicina_Tipo)
                    .WithMany(p => p.tblVideo_SoulMedicina)
                    .HasForeignKey(d => d.intSoulMedicina_TipoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblVideo___intSo__07F4326D");
            });

            modelBuilder.Entity<tblVideos_Brutos>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.txtCompleteFileName)
                    .HasName("_net_IX_tblVideos_Brutos_txtCompleteFileName");

                entity.HasIndex(e => new { e.txtFileName, e.dteCriacao })
                    .HasName("IX_tblVideos_Brutos");

                entity.HasIndex(e => new { e.dteDateTime, e.txtCompleteFileName, e.txtFileName, e.txtDirectoryName, e.txtExtension, e.dteCriacao, e.intID, e.intQuestaoID })
                    .HasName("_net_IX_tblVideos_Brutos_intQuestaoID");

                entity.Property(e => e.OperadorANYCAST).IsUnicode(false);

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.txtCompleteFileName)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.txtDirectoryName)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsFixedLength();

                entity.Property(e => e.txtExtension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.txtFileName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblVideos_Brutos_Busca>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK_tblVideos_Brutos_Busca_HD2");

                entity.HasIndex(e => e.intQuestaoID)
                    .HasName("IX_tblVideos_Brutos_Busca");

                entity.HasIndex(e => new { e.txtCompleteFileName, e.intQuestaoID, e.intVideoTipo })
                    .HasName("tblVideos_Brutos_Busca-intVideoTipo");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.txtCompleteFileName)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.txtFileName)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.txtVideoCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.intBook)
                    .WithMany(p => p.tblVideos_Brutos_Busca)
                    .HasForeignKey(d => d.intBookID)
                    .HasConstraintName("FK_tblVideos_Brutos_Busca_HD2_tblBooks");

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblVideos_Brutos_Busca)
                    .HasForeignKey(d => d.intQuestaoID)
                    .HasConstraintName("FK_tblVideos_Brutos_Busca_HD2_tblConcursoQuestoes");
            });

            modelBuilder.Entity<tblWarehouses>(entity =>
            {
                entity.HasKey(e => e.intWarehouseID);

                entity.Property(e => e.intEmployeeID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intStoreID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.txtAddress1).HasMaxLength(50);

                entity.Property(e => e.txtAddress2).HasMaxLength(50);

                entity.Property(e => e.txtComplementoNome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.txtNeighbourhood).HasMaxLength(50);

                entity.Property(e => e.txtZipCode).HasMaxLength(50);

                entity.HasOne(d => d.intAddressTypeNavigation)
                    .WithMany(p => p.tblWarehouses)
                    .HasForeignKey(d => d.intAddressType)
                    .HasConstraintName("FK_tblWarehouses_tblAddressTypes");

                entity.HasOne(d => d.intCity)
                    .WithMany(p => p.tblWarehouses)
                    .HasForeignKey(d => d.intCityID)
                    .HasConstraintName("FK_tblWarehouses_tblCities");

                entity.HasOne(d => d.intEmployee)
                    .WithMany(p => p.tblWarehouses)
                    .HasForeignKey(d => d.intEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehouses_tblEmployees");

                entity.HasOne(d => d.intStore)
                    .WithMany(p => p.tblWarehouses)
                    .HasForeignKey(d => d.intStoreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehouses_tblStores");
            });

            modelBuilder.Entity<tblWarehousesClassRooms>(entity =>
            {
                entity.HasKey(e => new { e.intWarehouseID, e.intClassRoomID });

                entity.HasIndex(e => e.intClassRoomID)
                    .HasName("_net_IX_tblWarehousesClassRooms_intClassRoomID")
                    .IsUnique();

                entity.HasIndex(e => new { e.intWarehouseID, e.intProductGroupID })
                    .HasName("IX_tblWarehousesClassRooms");

                entity.HasOne(d => d.intClassRoom)
                    .WithOne(p => p.tblWarehousesClassRooms)
                    .HasForeignKey<tblWarehousesClassRooms>(d => d.intClassRoomID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehousesClassRooms_tblClassRooms");

                entity.HasOne(d => d.intProductGroup)
                    .WithMany(p => p.tblWarehousesClassRooms)
                    .HasForeignKey(d => d.intProductGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehousesClassRooms_tblProductGroups1");

                entity.HasOne(d => d.intWarehouse)
                    .WithMany(p => p.tblWarehousesClassRooms)
                    .HasForeignKey(d => d.intWarehouseID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehouses_Courses_tblWarehouses_Courses");
            });

            modelBuilder.Entity<tblWarehousesClassRooms_Api>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblWareh__11B67932679021AA");

                entity.HasIndex(e => new { e.intID, e.intWarehouseID, e.intProductID, e.intYear, e.intClassRoomID })
                    .HasName("IX_tblWarehousesClassRooms_Api_intClassRoomID_intID_intWarehouseID_intProductID_intYear");
            });

            modelBuilder.Entity<tbl_emed_access_business>(entity =>
            {
                entity.HasKey(e => e.intAccessBusinessId);

                entity.HasIndex(e => new { e.txtItemDetail, e.intModuleItemId, e.intClientID })
                    .HasName("_net_IX_tbl_emed_access_business_intModuleItemID");

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtIP)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.txtItemDetail)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbllogConcursoQuestoesGravacaoProtocolo_Codigos>(entity =>
            {
                entity.HasKey(e => e.intLogID)
                    .HasName("PK__tbllogCo__A6C747B43AF81025");

                entity.Property(e => e.dteDateTime).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeUpload).HasColumnType("datetime");

                entity.Property(e => e.dteExclude).HasColumnType("datetime");

                entity.Property(e => e.intTypeID)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.txtJutificativa)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<viewMedsoft_tblAluno>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewMedsoft_tblAluno");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.dteBirthday).HasColumnType("datetime");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.imgFingerPrint).HasColumnType("image");

                entity.Property(e => e.txtAddress1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtAddress2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtArea)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtCel)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtEmail1).HasMaxLength(100);

                entity.Property(e => e.txtEmail2).HasMaxLength(100);

                entity.Property(e => e.txtEmail3).HasMaxLength(100);

                entity.Property(e => e.txtEspecialidade)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.txtFax)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtIDDocument).HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtNeighbourhood)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtNickName)
                    .HasMaxLength(60)
                    .IsFixedLength();

                entity.Property(e => e.txtPassport).HasMaxLength(50);

                entity.Property(e => e.txtPhone1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtPhone2)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtRegister)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtSubscriptionCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblConcursoQuestaoCatologoDeClassificacoes>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.intTipoDeClassificacao)
                    .HasName("_net_IX_tblConcursoQuestaoCatologoDeClassificacoes");

                entity.HasIndex(e => new { e.intClassificacaoID, e.intTipoDeClassificacao })
                    .HasName("_net_IX_tblConcursoQuestaoCatologoDeClassificacoes01");

                entity.Property(e => e.intClassificacaoID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtDescricaoClassificacao).HasMaxLength(1000);

                entity.Property(e => e.txtSubTipoDeClassificacao).HasMaxLength(100);

                entity.Property(e => e.txtTipoDeClassificacao)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<tblCallCenterCalls>(entity =>
            {
                entity.HasKey(e => e.intCallCenterCallsID);

                entity.HasIndex(e => e.bitNotify)
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K11");

                entity.HasIndex(e => e.intStatusInternoID)
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15");

                entity.HasIndex(e => new { e.dteOpen, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_2");

                entity.HasIndex(e => new { e.intCallCategoryID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K4");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID })
                    .HasName("IX_tblCallCenterCallsxCliente");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intStatusID");

                entity.HasIndex(e => new { e.intCallSectorID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K1");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K1");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4");

                entity.HasIndex(e => new { e.bitNotify, e.intClientID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K11_K4_K6");

                entity.HasIndex(e => new { e.dteOpen, e.intCallCategoryID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K4_2");

                entity.HasIndex(e => new { e.dteOpen, e.intClientID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_2");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intClientID })
                    .HasName("_dta_stat_1429632186_2_3_4");

                entity.HasIndex(e => new { e.intCallCategoryID, e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_6");

                entity.HasIndex(e => new { e.intCallCategoryID, e.intCourseID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallCategoryID_intCourseID_intStatusID");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallCategoryID, e.intStatusInternoID })
                    .HasName("_dta_stat_1429632186_1_6_15");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID })
                    .HasName("_dta_stat_1429632186_1_4_3");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCourseID, e.intCallCenterCallsID })
                    .HasName("_dta_stat_1429632186_5_20_1");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intClientID_intCallCategoryID");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_4_5_6");

                entity.HasIndex(e => new { e.intCourseID, e.intClientID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_20_4_6");

                entity.HasIndex(e => new { e.bitNotify, e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_stat_1429632186_11_4_6_3");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intCallGroupID })
                    .HasName("_dta_stat_1429632186_18_19_20_5");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intStatusID, e.intCallGroupID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K5_18_19");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K6_K3");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_dta_stat_1429632186_1_4_3_15");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_5_6_3_12");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.dteOpen, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallCategoryID_dteOpen");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K1");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_4_6_3_12");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCourseID })
                    .HasName("_dta_stat_1429632186_4_6_3_20");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intCallCategoryID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterCalls_intClientID_intStatusID");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_4_15_5_6");

                entity.HasIndex(e => new { e.intCourseID, e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K20_K4_K6_K3");

                entity.HasIndex(e => new { e.intCourseID, e.intStatusID, e.intClientID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K6_20");

                entity.HasIndex(e => new { e.intStatusID, e.intCallCategoryID, e.intCourseID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallCategoryID_intCourseID");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.dteOpen })
                    .HasName("_dta_stat_1429632186_3_4_15_2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.dteDataPrevisao2, e.intClientID, e.intStatusID })
                    .HasName("IX_tblCallCenterCalls_intClientID_intStatusID_CCE65");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_15_6_3_12");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K3_K4_15");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intStatusID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K3_K6");

                entity.HasIndex(e => new { e.txtSubject, e.txtDetail, e.txtFile, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_7_8_9");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCallCategoryID, e.intCourseID, e.intStatusID })
                    .HasName("IX_tblCallCenterCalls_intCallCategoryID_intCourseID_intStatusID_21FD5");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intCallGroupID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K18_K19_K20_K5_K3");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intStatusID, e.intCallGroupID })
                    .HasName("_net_IX_tblCallCenterCalls_intCourseID_intStatusID");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intStatusID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K5_K20_18_19");

                entity.HasIndex(e => new { e.dteOpen, e.intClientID, e.intCallSectorID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallSectorID");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCourseID, e.intCallCategoryID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K4_2_3_20");

                entity.HasIndex(e => new { e.intCallCategoryID, e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K3_K15_6");

                entity.HasIndex(e => new { e.intCallCategoryID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K1_6");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallCategoryID, e.intStatusID, e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_1_6");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K3_K15_K6");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K3_K18_K19_K20");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K15_K6_K3_K4");

                entity.HasIndex(e => new { e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_12_4_5_15_6");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_stat_1429632186_4_6_3_5_20");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K12_K15");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_4_1_15_5_6");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K15");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_4_3_6_5_12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K1");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K5");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.dteOpen, e.intStatusID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_4_15_2_3_6");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K5_K6_K3");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K5");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K12");

                entity.HasIndex(e => new { e.intStatusID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intCallGroupID })
                    .HasName("_net_IX_tblCallCenterCalls_intCourseID_intCallGroupID");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K5");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K12");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K6_K3_K12_K4");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_15_4_6_3_12");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K5_K6_K3");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K20_18_19");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K20_K4_K6_K3_18_19");

                entity.HasIndex(e => new { e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intClientID, e.intStatusID, e.intCallCategoryID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K6_18_19_20");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K15_K6_K12");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteDataPrevisao2, e.intClientID, e.intCallCategoryID, e.intStatusInternoID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallCategoryID_intStatusInternoID");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.txtSubject, e.intCallCategoryID, e.intStatusID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K3_K4_1_5_7");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.txtSubject, e.intClientID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_1_5_7");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID })
                    .HasName("_dta_stat_1429632186_1_12_4_5_15_6");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.dteOpen, e.intFirstEmployeeID, e.intStatusInternoID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intStatusInternoID_intStatusID");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K15_K6_K3_K12_K4");

                entity.HasIndex(e => new { e.intClientID, e.dteDataPrevisao2, e.intCallCategoryID, e.intStatusInternoID, e.intStatusID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K15_K3_K1_4_19");

                entity.HasIndex(e => new { e.intClientID, e.dteDataPrevisao2, e.intCallCenterCallsID, e.intCallCategoryID, e.intStatusInternoID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K6_K15_K3_4_19");

                entity.HasIndex(e => new { e.intClientID, e.dteOpen, e.intCallCategoryID, e.intCallGroupID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K2_K6_K5_K3_K15");

                entity.HasIndex(e => new { e.intClientID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_4_2_3_6_5_12");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallCenterCallsID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_stat_1429632186_4_6_3_1_5_20");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K12_K15_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K15");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intStatusInternoID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K15_K12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_4_3_15_6_5_12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K12_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_4_15_2_3_6_12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K5_K6_K3_K12");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K5_K12");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K5_K12");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K12_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K6_K3_K12_K4_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K6_K3_K12_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K5_K6_K3_K12");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intFirstEmployeeID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_stat_1429632186_15_4_16_5_6_3");

                entity.HasIndex(e => new { e.bitNotify, e.intClientID, e.dteOpen, e.intCallCategoryID, e.intCallGroupID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_dta_stat_1429632186_11_4_2_6_5_3_15");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K15_K6_K5_K12");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K4_K5_K15_K6_K3_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K5_K2_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusInternoID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallGroupID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K2_K3_K6_K5_1");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K6_K3_K12_K4_K16_K2");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intCallCenterCallsID })
                    .HasName("_dta_stat_1429632186_5_15_6_3_12_4_1");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intClientID, e.intCallCenterCallsID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K15_K6_K3_K4_K1_K2");

                entity.HasIndex(e => new { e.intCallSectorID, e.intCallCenterCallsID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K1_K4_K5_K15_K6_K3");

                entity.HasIndex(e => new { e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intFirstEmployeeID })
                    .HasName("_dta_stat_1429632186_12_4_5_15_6_3_16");

                entity.HasIndex(e => new { e.intCallSectorID, e.intSeverity, e.intStatusID, e.intCallCategoryID, e.intClientID, e.intCallGroupID, e.dteDataPrevisao1 })
                    .HasName("_dta_stat_1429632186_12_10_3_6_4_5_18");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.intCallGroupID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K12_K15_K5_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K1_K15_K5_K6_K3_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K15_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intStatusInternoID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K15_K12_K2");

                entity.HasIndex(e => new { e.intClientID, e.intFirstEmployeeID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_stat_1429632186_4_16_5_6_3_12_2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12_K5_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K5_K6_K3_K12_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K5_K12_K2");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.dteOpen, e.intStatusID, e.intClientID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K6_K5_K12_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intClientID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K2_K3_K6_K5_K12_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intClientID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K6_K5_K12_K2_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intStatusID, e.intClientID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K6_K5_K12_K2_16");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K5_K12_K2");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.intCallGroupID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K12_K5_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K6_K3_K12_K5_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K5_K6_K3_K12_K2");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteOpen, e.intClientID, e.intCallCategoryID, e.intStatusInternoID, e.intCallGroupID, e.intStatusID, e.intCourseID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID_revisar");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteOpen, e.intStatusID, e.intClientID, e.intCallCategoryID, e.intCallGroupID, e.intStatusInternoID, e.intCourseID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID_revisar_02");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K4_K5_K15_K6_K3_K2_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.dteOpen, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K5_K2_K12_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCourseID, e.intClientID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCallGroupID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID_intStatusID");

                entity.HasIndex(e => new { e.intCallGroupID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCallSectorID, e.intSeverity, e.intStatusID, e.intCallCategoryID, e.intClientID })
                    .HasName("_dta_stat_1429632186_5_18_19_12_10_3_6_4");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intFirstEmployeeID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K6_K3_K12_K4_K16_K2_K15");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intClientID, e.intCallCenterCallsID, e.dteOpen, e.intCallSectorID })
                    .HasName("_dta_stat_1429632186_5_15_6_3_4_1_2_12");

                entity.HasIndex(e => new { e.intCallSectorID, e.intCallCenterCallsID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K1_K4_K5_K15_K6_K3_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.intCallGroupID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K12_K15_K5_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.dteOpen, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K1_K15_K5_K6_K3_K2_K12");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intFirstEmployeeID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K16_K2_K15");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K15_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intStatusInternoID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K12_K15_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intStatusInternoID, e.intCallSectorID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K5_K6_K3_K15_K12_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intFirstEmployeeID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K16_K5_K6_K3_K12_K2_K15");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.dteOpen, e.intCallSectorID, e.intFirstEmployeeID })
                    .HasName("_dta_stat_1429632186_4_3_15_6_5_2_12_16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.dteOpen, e.intCallGroupID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K12_K2_K5_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.intCallGroupID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K12_K5_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallSectorID, e.intCallGroupID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K2_K3_K6_K12_K5_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen, e.intCallGroupID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12_K2_K5_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12_K5_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K6_K3_K12_K5_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K5_K6_K3_K12_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K5_K6_K3_K12_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K5_K12_K2_K16");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K5_K12_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intStatusInternoID, e.intStatusID, e.intCallCategoryID, e.intCallSectorID, e.intCallGroupID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K3_K6_K12_K5_K16_K2");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.dteOpen, e.intStatusID, e.intClientID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K6_K5_K12_K15_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.dteOpen, e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K15_K6_K5_K12_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intClientID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K2_K3_K6_K5_K12_K15_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intClientID, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K6_K5_K12_K2_K15_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intStatusID, e.intClientID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K6_K5_K12_K2_K15_16");

                entity.HasIndex(e => new { e.intFirstEmployeeID, e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K5_K12_K2_16");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intStatusInternoID, e.intCallCategoryID, e.intCallSectorID, e.intCallGroupID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K6_K12_K5_K2_K16");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.dteOpen, e.intCallGroupID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K6_K3_K12_K4_K2_K5_K16");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K6_K3_K12_K4_K5_K16_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen, e.intCallGroupID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K6_K3_K12_K2_K5_K16");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K6_K3_K12_K5_K2_K16");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intCallGroupID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K6_K3_K12_K5_K16_K2");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K5_K6_K3_K12_K2_K16");

                entity.HasIndex(e => new { e.intStatusInternoID, e.intClientID, e.intFirstEmployeeID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K15_K4_K16_K5_K6_K3_K12_K2");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intFirstEmployeeID, e.dteOpen, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K6_K3_K12_K4_K16_K2_K15_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K4_K5_K15_K6_K3_K2_K16_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallSectorID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K4_K5_K15_K6_K3_K16_K2_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.dteOpen, e.intCallSectorID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K5_K2_K12_K16_1");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intFirstEmployeeID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K6_K5_K12_K2_1_16");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intFirstEmployeeID, e.intClientID, e.intStatusInternoID, e.dteOpen, e.intStatusID, e.intCallCategoryID, e.intCallGroupID, e.intCallSectorID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K2_K3_K6_K5_K12_1_16");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.intClientID, e.intCallCenterCallsID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K15_K6_K3_K12_K4_K1_K16_K2");

                entity.HasIndex(e => new { e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intClientID, e.intCallCenterCallsID, e.dteOpen, e.intCallSectorID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K15_K6_K3_K4_K1_K2_K12_K16");

                entity.HasIndex(e => new { e.intCallSectorID, e.intCallCenterCallsID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.dteOpen, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K1_K4_K5_K15_K6_K3_K2_K16");

                entity.HasIndex(e => new { e.intCallSectorID, e.intCallCenterCallsID, e.intClientID, e.intCallGroupID, e.intStatusInternoID, e.intCallCategoryID, e.intStatusID, e.intFirstEmployeeID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K1_K4_K5_K15_K6_K3_K16_K2");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.dteOpen, e.intCallSectorID, e.intFirstEmployeeID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K1_K15_K5_K6_K3_K2_K12_K16");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.intStatusInternoID, e.intFirstEmployeeID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID, e.intCallSectorID, e.dteOpen })
                    .HasName("_dta_stat_1429632186_4_1_15_16_5_6_3_12_2");

                entity.HasIndex(e => new { e.txtSubject, e.intCallGroupID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCallSectorID, e.intSeverity, e.intStatusID, e.intCallCategoryID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K18_K19_K12_K10_K3_K6_K4_7");

                entity.HasIndex(e => new { e.txtSubject, e.intCallSectorID, e.intSeverity, e.intStatusID, e.intCallCategoryID, e.intClientID, e.intCallGroupID, e.dteDataPrevisao1, e.dteDataPrevisao2 })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K12_K10_K3_K6_K4_K5_K18_K19_7");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteOpen, e.intClientID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intStatusInternoID, e.intCallGroupID, e.intCallCategoryID, e.intStatusID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID_intCallCategoryID");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.txtSubject, e.intSeverity, e.intCallSectorID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCallCategoryID, e.intStatusID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K6_K3_K4_1_5_7_10_12_18_19");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intSeverity, e.intCallSectorID, e.intStatusInternoID, e.intCourseID, e.intLastEmployeeID, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.intClientID, e.intCallCenterCallsID })
                    .HasName("_net_IX_tblCallCenterCalls_intSeverity_intCallSectorID");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.dteOpen, e.intStatusID, e.intClientID, e.intCallCategoryID, e.txtSubject, e.intCallSectorID, e.intStatusInternoID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intCallGroupID, e.intSeverity })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID_intSeverity");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_1_5_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.intCallGroupID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K2_5_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.bitNotify, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intCallSectorID, e.intCallCenterCallsID, e.intSeverity, e.intCallGroupID, e.intCallCategoryID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K12_K1_K10_K5_K6_K15_2_3_11_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K15_2_3_5_6_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intStatusInternoID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K1_2_3_5_6_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_2_3_5_6_10_11_12_13_14_15_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K1_2_3_5_6_10_11_12_13_14_15_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intDepartmentID, e.intCallCenterCallsID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K5_K20_2_7_8_9_10_11_12_13_14_15_16_17_18_19_21");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intDepartmentID, e.intCallGroupID, e.intCourseID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K5_K20_K1_2_7_8_9_10_11_12_13_14_15_16_17_18_19_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K15_2_3_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intStatusInternoID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K15_K1_2_3_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_2_3_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.intStatusID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K1_2_3_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K6_K3_K5_K20_2_7_8_9_10_11_12_13_14_15_16_17_18_19_21");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intDepartmentID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallCenterCallsID, e.intCallGroupID, e.intCourseID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K1_K5_K20_2_7_8_9_10_11_12_13_14_15_16_17_18_19_21");

                entity.HasIndex(e => new { e.dteOpen, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intDepartmentID, e.intClientID, e.intCallCategoryID, e.intStatusID, e.intCallGroupID, e.intCourseID, e.intCallCenterCallsID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K6_K3_K5_K20_K1_2_7_8_9_10_11_12_13_14_15_16_17_18_19_21");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.dteOpen, e.intStatusID, e.intClientID, e.intStatusInternoID })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K2_K3_K4_K15_1_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.intCallCenterCallsID, e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intStatusID, e.intClientID, e.intStatusInternoID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K3_K4_K15_K2_1_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCenterCallsID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K1_K4_K3_K15_K2_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.intCallGroupID, e.intCallCategoryID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intClientID, e.intStatusID, e.intStatusInternoID, e.intCallCenterCallsID, e.dteOpen })
                    .HasName("_dta_index_tblCallCenterCalls_5_1429632186__K4_K3_K15_K1_K2_5_6_7_8_9_10_11_12_13_14_16_17_18_19_20_21");

                entity.HasIndex(e => new { e.intClientID, e.intCallCenterCallsID, e.dteOpen, e.intCallGroupID, e.txtSubject, e.txtDetail, e.txtFile, e.intSeverity, e.bitNotify, e.intCallSectorID, e.dteSolutionDays, e.intSectorComplementID, e.intFirstEmployeeID, e.intLastEmployeeID, e.dteDataPrevisao1, e.dteDataPrevisao2, e.intCourseID, e.intDepartmentID, e.intCallCategoryID, e.intStatusID, e.intStatusInternoID })
                    .HasName("_net_IX_tblCallCenterCalls_intCallCategoryID");

                entity.HasIndex(e => new { e.intStatusID, e.intClientID, e.intCallCategoryID, e.txtSubject, e.intSeverity, e.intCallSectorID, e.intCallCenterCallsID, e.dteOpen, e.txtDetail, e.txtFile, e.bitNotify, e.intSectorComplementID, e.intStatusInternoID, e.intFirstEmployeeID, e.intLastEmployeeID, e.intCourseID, e.dteSolutionDays, e.intDepartmentID, e.intCallGroupID, e.dteDataPrevisao1, e.dteDataPrevisao2 })
                    .HasName("_net_IX_tblCallCenterCalls_intCallGroupID");

                entity.Property(e => e.dteDataPrevisao1).HasColumnType("datetime");

                entity.Property(e => e.dteDataPrevisao2).HasColumnType("datetime");

                entity.Property(e => e.dteOpen).HasColumnType("datetime");

                entity.Property(e => e.dteSolutionDays).HasColumnType("datetime");

                entity.Property(e => e.txtDetail).HasMaxLength(4000);

                entity.Property(e => e.txtFile).HasMaxLength(200);

                entity.Property(e => e.txtSubject)
                    .IsRequired()
                    .HasMaxLength(50);
            });            

                        modelBuilder.Entity<tblCronogramaExcecoesEntidades>(entity =>
            {
                entity.HasKey(e => e.intExcecaoEntidadeId)
                    .HasName("PK__tblCrono__8CC05D20147EC76E");

                entity.Property(e => e.dteDataInclusao).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblParametrosGenericos>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => e.txtName)
                    .HasName("IX_tblParametrosGenericos");

                entity.HasIndex(e => new { e.txtValue, e.txtModule, e.txtName })
                    .HasName("_net_tblParametrosGenericos_txtModule_TxtName");

                entity.Property(e => e.txtModule)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Nome do parametro para ser utilizado em filtros especificos");

                entity.Property(e => e.txtValue)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasComment("Valor para o parametro especifico");
            });

            modelBuilder.Entity<tblProductCodes>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.intID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblRevisaoAulaIndice>(entity =>
            {
                entity.HasKey(e => e.intRevisaoAulaIndiceId);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRevisaoAulaVideo>(entity =>
            {
                entity.HasKey(e => e.intRevisaoAulaId);

                entity.HasIndex(e => new { e.intRevisaoAulaId, e.intRevisaoAulaIdPai, e.intProfessorId, e.intRevisaoAulaIndiceId, e.txtDescricao, e.intOrdem, e.intCuePoint, e.intVideoId })
                    .HasName("_net_IX_tblRevisaoAulaVideo_intVideoId");

                entity.HasIndex(e => new { e.intRevisaoAulaId, e.intRevisaoAulaIdPai, e.intProfessorId, e.txtDescricao, e.intOrdem, e.intCuePoint, e.intVideoId, e.intRevisaoAulaIndiceId })
                    .HasName("_net_IX_tblRevisaoAulaVideo_intRevisaoAulaIndiceId");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao).HasMaxLength(200);

                entity.Property(e => e.txtTemp_nomeVideo)
                    .HasMaxLength(600)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblNotificacaoEvento>(entity =>
            {
                entity.HasKey(e => e.intNotificacaoEvento)
                    .HasName("PK__tblNotif__80EA3898B3371A06");

                entity.HasIndex(e => new { e.Metadados, e.txtTitulo, e.txtDescricao, e.dteCadastro, e.intStatusLeitura, e.intNotificacaoId, e.intContactId, e.bitAtivo })
                    .HasName("IX_tblNotificacaoEvento_intNotificacaoId_intContactId_bitAtivo_839EF");

                entity.HasIndex(e => new { e.intNotificacaoId, e.Metadados, e.txtTitulo, e.txtDescricao, e.dteCadastro, e.intStatusLeitura, e.intContactId, e.bitAtivo })
                    .HasName("IX_tblNotificacaoEvento_intContactId_bitAtivo_9DA4D");

                entity.HasIndex(e => new { e.intNotificacaoEvento, e.Metadados, e.intContactId, e.txtTitulo, e.txtDescricao, e.intStatus, e.bitAtivo, e.intNotificacaoId, e.dteCadastro })
                    .HasName("IX_tblNotificacaoEvento_intStatus_bitAtivo_intNotificacaoId_dteCadastro_66715");

                entity.Property(e => e.Metadados)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtDescricao)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.txtTitulo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblConcurso_ProvasLivesRecurso>(entity =>
            {
                entity.HasKey(e => e.intConcursoProvasLivesRecursoID)
                    .HasName("PK__tblConcu__CFCD1FAA53E40B8D");

                entity.Property(e => e.dteData).HasColumnType("datetime");

                entity.Property(e => e.txtAliasUrlProvaGabaritada).HasMaxLength(100);

                entity.Property(e => e.txtUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlOtimizadaProvaGabaritada)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlProvaGabaritada)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<tblProvaAlunoConfiguracoes>(entity =>
            {
                entity.HasKey(e => e.intComunicadoAlunoId)
                    .HasName("PK__tblProva__F11770F5F3D6CD0C");

                entity.Property(e => e.bitComunicadoAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.dteAtualizacao).HasColumnType("datetime");

                entity.Property(e => e.dteCriacao).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblContadorQuestoes_MontaProva>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.HasIndex(e => new { e.intId, e.intQuantidadeQuestoes, e.intNaoRealizadas, e.intAcertos, e.intErros, e.intProvaId, e.intClientId })
                    .HasName("intIDIndex");

                entity.HasIndex(e => new { e.intId, e.intProvaId, e.intQuantidadeQuestoes, e.intNaoRealizadas, e.intAcertos, e.intErros, e.dteDataCriacao, e.intClientId })
                    .HasName("IX_tblContadorQuestoes_MontaProva_intClientId_37B34");

                entity.Property(e => e.dteDataCriacao).HasColumnType("date");
            });

            modelBuilder.Entity<tblFiltroAluno_MontaProva>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblFiltr__11B6793215A65982");

                entity.HasIndex(e => new { e.intClientId, e.bitAtivo })
                    .HasName("IX_tblFiltroAluno_MontaProva_intClientId_bitAtivo_4BDC0");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.Property(e => e.txtAnos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.txtConcursos).IsUnicode(false);

                entity.Property(e => e.txtEspecialidades)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.txtFiltrosEspeciais)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.txtJsonFiltro).IsUnicode(false);

                entity.Property(e => e.txtNome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtPalavraChave)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblMapaMentalVideos>(entity =>
            {
                entity.HasKey(e => e.intMapaMentalVideoID)
                    .HasName("PK__tblMapaM__8D31D78A7C832CE9");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblAlunoExcecaoSlideAulas>(entity =>
            {
                entity.HasKey(e => e.intAlunoExcecaoSlideId)
                    .HasName("PK__tblAluno__12A52581E7C16B5D");

                entity.HasIndex(e => e.intClientID)
                    .HasName("ix_tblAlunoExcecaoSlideAulas_intAlunoExcecaoSlideId")
                    .IsUnique();

                entity.Property(e => e.dteCadatastro).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        #endregion

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual List<csp_ListaMaterialDireitoAluno_Result> csp_ListaMaterialDireitoAluno(int? clientID, int? intYear, int? intProductGroup1)
        {
            var clientIDParameter = clientID.HasValue ? clientID : null;
            var intYearParameter = intYear.HasValue ? $",{intYear}" : null;
            var intProductGroup1Parameter = intProductGroup1.HasValue ? $",{intProductGroup1}" : null;

            var query = $"exec dbo.csp_ListaMaterialDireitoAluno {clientIDParameter}{intYearParameter}{intProductGroup1Parameter}";
            var retornoRows = new DBQuery().ExecuteQuery(query).Tables[0].Rows;

            var retornoList = new List<csp_ListaMaterialDireitoAluno_Result>();

            for(int i = 0; i < retornoRows.Count; i++)
            {
                var row = retornoRows[i];

                var temp = new csp_ListaMaterialDireitoAluno_Result(){
                    intMaterialID = (row["intMaterialID"] == DBNull.Value) ? null : (int?)row["intMaterialID"],
                    intBookEntityID = (row["intBookEntityID"] == DBNull.Value) ? null : (long?)row["intBookEntityID"],
                    intSemana = (row["intSemana"] == DBNull.Value) ? null : (int?)row["intSemana"],
                    dataInicio = row["dataInicio"].ToString(),
                    datafim = row["datafim"].ToString(),
                    horaInicio = row["horaInicio"].ToString(),
                    anoCronograma = (row["anoCronograma"] == DBNull.Value) ? null : (int?)row["anoCronograma"],
                    anoCursado = (row["anoCursado"] == DBNull.Value) ? null : (int?)row["anoCursado"],
                    blnPermitido = (int)row["blnPermitido"],
                    txtName = row["txtName"].ToString(),
                    intLessonTitleID = (row["intLessonTitleID"] == DBNull.Value) ? null : (int?)row["intLessonTitleID"]
                };

                retornoList.Add(temp);     
            }

            return retornoList;
        }

        public virtual List<DateTime?> msp_GetDataLimite_ByApplication(int? intYear, int? intApplicatoinID)
        {
            var result = this.msp_GetDataLimite_ByApplication_Result.FromSqlInterpolated($"exec dbo.msp_GetDataLimite_ByApplication {intYear}, {intApplicatoinID}");
            return result.Select(_ => _.dteDataLimite).ToList();
        }

        

        public virtual List<msp_Medsoft_SelectPermissaoExercicios_Result> msp_Medsoft_SelectPermissaoExercicios(bool? bitVisitanteExpirado, bool? bitAlunoVisitante, int? intClientID)
        {
            var result = this.msp_Medsoft_SelectPermissaoExercicios_Result.FromSqlInterpolated($"exec dbo.msp_Medsoft_SelectPermissaoExercicios {bitVisitanteExpirado}, {bitAlunoVisitante}, {intClientID}");
            return result.ToList();
        }

        public void SetCommandTimeOut(int Timeout)
        {
            this.Database.SetCommandTimeout(Timeout);
        }


        public virtual List<csp_loadMesesCursados_Result> csp_loadMesesCursados(int? matricula, int? idProduto)
        {
            var result = this.csp_loadMesesCursados_Result.FromSqlInterpolated($"exec dbo.csp_loadMesesCursados {matricula}");
            return result.AsEnumerable().Where(x => x.intProductGroup == idProduto).ToList();


        }

        
    }
}
