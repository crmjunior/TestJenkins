using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblQuestoes
    {
        public tblQuestoes()
        {
            tblQuestao_CometariosLog = new HashSet<tblQuestao_CometariosLog>();
            tblQuestao_Simulado = new HashSet<tblQuestao_Simulado>();
        }

        public int intQuestaoID { get; set; }
        public int? intOldQuestaoID { get; set; }
        public int? intInstituicaoID { get; set; }
        public int? intNivelDeDificuladeID { get; set; }
        public string bitCasoClinico { get; set; }
        public string bitConceitual { get; set; }
        public int? intGrandeAreaID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public int? intSubEspecialidadeID { get; set; }
        public int? ID_CONCURSO { get; set; }
        public string txtEnunciado { get; set; }
        public string txtComentario { get; set; }
        public string txtObservacao { get; set; }
        public string txtRecurso { get; set; }
        public DateTime? dteQuestao { get; set; }
        public int? intFonteID { get; set; }
        public string txtFonteTipo { get; set; }
        public string txtOrigem { get; set; }
        public int? intYear { get; set; }
        public int? intProfessorAutor { get; set; }
        public int? intProfessorFilmagem { get; set; }
        public DateTime? dteFilmagem { get; set; }
        public Guid guidQuestaoID { get; set; }
        public bool bitAnulada { get; set; }
        public int? intEmployeeComentarioID { get; set; }
        public DateTime? dteLimite { get; set; }
        public int? intQuestaoConcursoID { get; set; }

        public virtual tblVideo_Questao_Simulado tblVideo_Questao_Simulado { get; set; }
        public virtual ICollection<tblQuestao_CometariosLog> tblQuestao_CometariosLog { get; set; }
        public virtual ICollection<tblQuestao_Simulado> tblQuestao_Simulado { get; set; }
    }
}
