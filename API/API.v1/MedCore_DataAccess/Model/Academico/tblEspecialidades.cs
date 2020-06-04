using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblEspecialidades
    {
        public tblEspecialidades()
        {
//tblEspecialidadesSimulado = new HashSet<tblEspecialidadesSimulado>();
        }

        public int intEspecialidadeID { get; set; }
        public string CD_ESPECIALIDADE { get; set; }
        public string CD_AREA { get; set; }
        public string DE_ESPECIALIDADE { get; set; }
        public int? VL_TEMPO_PRE { get; set; }
        public string INSCRICAO { get; set; }
        public string CONCURSO { get; set; }
        public bool? bitAtivo { get; set; }

        //public virtual ICollection<tblEspecialidadesSimulado> tblEspecialidadesSimulado { get; set; }
    }
}
