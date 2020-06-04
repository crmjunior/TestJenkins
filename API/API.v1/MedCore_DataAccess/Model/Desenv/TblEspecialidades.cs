using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEspecialidades
    {
        public tblEspecialidades()
        {
            tblBooks = new HashSet<tblBooks>();
            tblClients = new HashSet<tblClients>();
            tblDuvidasAcademicas_DuvidaQuestao = new HashSet<tblDuvidasAcademicas_DuvidaQuestao>();
            tblEspecialidadeProfessor = new HashSet<tblEspecialidadeProfessor>();
        }

        public int intEspecialidadeID { get; set; }
        public string CD_ESPECIALIDADE { get; set; }
        public string CD_AREA { get; set; }
        public string DE_ESPECIALIDADE { get; set; }
        public int? VL_TEMPO_PRE { get; set; }
        public string INSCRICAO { get; set; }
        public string CONCURSO { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual ICollection<tblBooks> tblBooks { get; set; }
        public virtual ICollection<tblClients> tblClients { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidaQuestao> tblDuvidasAcademicas_DuvidaQuestao { get; set; }
        public virtual ICollection<tblEspecialidadeProfessor> tblEspecialidadeProfessor { get; set; }
    }
}
