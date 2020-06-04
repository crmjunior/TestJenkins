using System;

namespace MedCore_DataAccess.DTO
{
    public class TblSimuladoDTO
    {
        public int intSimuladoID { get; set; }
        public Nullable<int> intLessonTitleID { get; set; }
        public Nullable<int> intBookID { get; set; }
        public string txtOrigem { get; set; }
        public string txtSimuladoName { get; set; }
        public string txtSimuladoDescription { get; set; }
        public Nullable<int> intSimuladoOrdem { get; set; }
        public int intDuracaoSimulado { get; set; }
        public Nullable<int> intConcursoID { get; set; }
        public Nullable<int> intAno { get; set; }
        public Nullable<bool> bitParaWEB { get; set; }
        public Nullable<DateTime> dteDataHoraInicioWEB { get; set; }
        public Nullable<DateTime> dteDataHoraTerminoWEB { get; set; }
        public Nullable<DateTime> dteReleaseSimuladoWeb { get; set; }
        public Nullable<DateTime> dteReleaseGabarito { get; set; }
        public Nullable<DateTime> dteReleaseComentario { get; set; }
        public Nullable<DateTime> dteInicioConsultaRanking { get; set; }
        public Nullable<DateTime> dteLimiteParaRanking { get; set; }
        public Nullable<bool> bitIsDemo { get; set; }
        public string CD_ESPECIALIDADE { get; set; }
        public Nullable<int> ID_INSTITUICAO { get; set; }
        public string txtPathGabarito { get; set; }
        public Nullable<int> intQtdQuestoes { get; set; }
        public Nullable<bool> bitRankingWeb { get; set; }
        public Nullable<bool> bitGabaritoWeb { get; set; }
        public Nullable<bool> bitRankingFinalWeb { get; set; }
        public string txtCodQuestoes { get; set; }
        public Nullable<bool> bitVideoComentariosWeb { get; set; }
        public Nullable<int> intQtdQuestoesCasoClinico { get; set; }
        public Guid guidSimuladoID { get; set; }
        public DateTime dteDateTimeLastUpdate { get; set; }
        public bool bitCronogramaAprovado { get; set; }
        public int intTipoSimuladoID { get; set; }
        public bool bitSimuladoGeral { get; set; }
        public bool bitOnline { get; set; }
        public Nullable<int> intPesoProvaObjetiva { get; set; }
        public Nullable<DateTime> dteDateInicio { get; set; }
        public Nullable<DateTime> dteDateFim { get; set; }
    }
}