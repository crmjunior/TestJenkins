using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCargaHorariaCurso
    {
        public int intCargaHorariaCursoID { get; set; }
        public int intProductGroup1 { get; set; }
        public int intCargaHorariaMensal { get; set; }
        public string txtMesAnoVigenciaInicio { get; set; }
        public string txtMesAnoVigenciaFim { get; set; }
        public DateTime dteCriacao { get; set; }
    }
}
