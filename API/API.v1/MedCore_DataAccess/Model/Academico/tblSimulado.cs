using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimulado
    {
        public tblSimulado()
        {
            //tblEspecialidadesSimulado = new HashSet<tblEspecialidadesSimulado>();
            tblQuestao_Simulado = new HashSet<tblQuestao_Simulado>();
            tblSimuladoOnline_Consolidado = new HashSet<tblSimuladoOnline_Consolidado>();
            tblSimuladoRespostas = new HashSet<tblSimuladoRespostas>();
            tblSimuladoResultados = new HashSet<tblSimuladoResultados>();
            tblSimuladoResultadosDiscursivas = new HashSet<tblSimuladoResultadosDiscursivas>();
        }

        public int intSimuladoID { get; set; }
        public int? intLessonTitleID { get; set; }
        public int? intBookID { get; set; }
        public string txtOrigem { get; set; }
        public string txtSimuladoName { get; set; }
        public string txtSimuladoDescription { get; set; }
        public int? intSimuladoOrdem { get; set; }
        public int intDuracaoSimulado { get; set; }
        public int? intConcursoID { get; set; }
        public int? intAno { get; set; }
        public bool? bitParaWEB { get; set; }
        public DateTime? dteDataHoraInicioWEB { get; set; }
        public DateTime? dteDataHoraTerminoWEB { get; set; }
        public DateTime? dteReleaseSimuladoWeb { get; set; }
        public DateTime? dteReleaseGabarito { get; set; }
        public DateTime? dteReleaseComentario { get; set; }
        public DateTime? dteInicioConsultaRanking { get; set; }
        public DateTime? dteLimiteParaRanking { get; set; }
        public bool? bitIsDemo { get; set; }
        public string CD_ESPECIALIDADE { get; set; }
        public int? ID_INSTITUICAO { get; set; }
        public string txtPathGabarito { get; set; }
        public int? intQtdQuestoes { get; set; }
        public bool? bitRankingWeb { get; set; }
        public bool? bitGabaritoWeb { get; set; }
        public bool? bitRankingFinalWeb { get; set; }
        public string txtCodQuestoes { get; set; }
        public bool? bitVideoComentariosWeb { get; set; }
        public int? intQtdQuestoesCasoClinico { get; set; }
        public Guid guidSimuladoID { get; set; }
        public DateTime dteDateTimeLastUpdate { get; set; }
        public bool bitCronogramaAprovado { get; set; }
        public int intTipoSimuladoID { get; set; }
        public bool bitSimuladoGeral { get; set; }
        public bool bitOnline { get; set; }
        public int? intPesoProvaObjetiva { get; set; }
        public DateTime? dteDateInicio { get; set; }
        public DateTime? dteDateFim { get; set; }

        //public virtual ICollection<tblEspecialidadesSimulado> tblEspecialidadesSimulado { get; set; }
        public virtual ICollection<tblQuestao_Simulado> tblQuestao_Simulado { get; set; }
        public virtual ICollection<tblSimuladoOnline_Consolidado> tblSimuladoOnline_Consolidado { get; set; }
        public virtual ICollection<tblSimuladoRespostas> tblSimuladoRespostas { get; set; }
        public virtual ICollection<tblSimuladoResultados> tblSimuladoResultados { get; set; }
        public virtual ICollection<tblSimuladoResultadosDiscursivas> tblSimuladoResultadosDiscursivas { get; set; }
    }
}
