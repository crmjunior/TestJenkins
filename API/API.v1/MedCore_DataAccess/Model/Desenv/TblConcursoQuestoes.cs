using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoes
    {
        public tblConcursoQuestoes()
        {
            tblConcursoQuestao_Classificacao = new HashSet<tblConcursoQuestao_Classificacao>();
            tblConcursoQuestao_Classificacao_Autorizacao = new HashSet<tblConcursoQuestao_Classificacao_Autorizacao>();
            tblConcursoQuestoes_Alternativas = new HashSet<tblConcursoQuestoes_Alternativas>();
            tblConcursoQuestoes_Aluno_Favoritas = new HashSet<tblConcursoQuestoes_Aluno_Favoritas>();
            tblVideos_Brutos_Busca = new HashSet<tblVideos_Brutos_Busca>();
        }

        public int intQuestaoID { get; set; }
        public int? intQuestaoIDOld { get; set; }
        public int? intInstituicaoID { get; set; }
        public int? intNivelDeDificuladeID { get; set; }
        public string bitCasoClinico { get; set; }
        public string bitConceitual { get; set; }
        public int? intProvaID { get; set; }
        public string txtEnunciado { get; set; }
        public string txtEnunciadoConcursoSiteRecursos { get; set; }
        public string txtComentario { get; set; }
        public string txtLetraAlternativaSugerida { get; set; }
        public string txtObservacao { get; set; }
        public string txtRecurso { get; set; }
        public DateTime? dteQuestao { get; set; }
        public int? intFonteID { get; set; }
        public string txtFonteTipo { get; set; }
        public int? intYear { get; set; }
        public int? intOrder { get; set; }
        public int? ID_CONCURSO_RECURSO_STATUS { get; set; }
        public int? intStatus_Banca_Recurso { get; set; }
        public string txtComentario_banca_recurso { get; set; }
        public bool? bitComentarioAtivo { get; set; }
        public int? intEmployeeID { get; set; }
        public bool bitAnulada { get; set; }
        public bool? bitAnuladaPosRecurso { get; set; }
        public bool? bitGabaritoPosRecursoLiberado { get; set; }
        public int? intEmployeeComentarioID { get; set; }
        public Guid guidQuestaoID { get; set; }
        public bool bitSemGabarito { get; set; }
        public bool bitSemGabaritoDiscursiva { get; set; }
        public bool bitDiscursiva { get; set; }
        public bool bitAprovadaRevisao1 { get; set; }
        public bool bitAprovadaRevisao2 { get; set; }
        public bool bitAprovadaRevisaoGeral { get; set; }
        public int? intCasoClinicoID { get; set; }

        public virtual ICollection<tblConcursoQuestao_Classificacao> tblConcursoQuestao_Classificacao { get; set; }
        public virtual ICollection<tblConcursoQuestao_Classificacao_Autorizacao> tblConcursoQuestao_Classificacao_Autorizacao { get; set; }
        public virtual ICollection<tblConcursoQuestoes_Alternativas> tblConcursoQuestoes_Alternativas { get; set; }
        public virtual ICollection<tblConcursoQuestoes_Aluno_Favoritas> tblConcursoQuestoes_Aluno_Favoritas { get; set; }
        public virtual ICollection<tblVideos_Brutos_Busca> tblVideos_Brutos_Busca { get; set; }
    }
}
