using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPessoaGrupo
    {
        public int intPessoaGrupoID { get; set; }
        public int? intGrupoID { get; set; }
        public int? intContactID { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblGrupo intGrupo { get; set; }
    }
}
