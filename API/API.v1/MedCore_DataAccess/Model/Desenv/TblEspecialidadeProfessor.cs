using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEspecialidadeProfessor
    {
        public int intEspecialidadeProfessorID { get; set; }
        public int? intContactID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public string txtEspecialidade { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblEspecialidades intEspecialidade { get; set; }
    }
}
