using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblEspecialidadesSimulado
    {
        public int intEspecialidadeID { get; set; }
        public int intSimuladoID { get; set; }
        public int intOrdem { get; set; }

        public virtual tblEspecialidades intEspecialidade { get; set; }
        public virtual tblSimulado intSimulado { get; set; }
    }
}
