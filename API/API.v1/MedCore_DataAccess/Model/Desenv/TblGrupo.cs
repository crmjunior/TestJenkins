using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblGrupo
    {
        public tblGrupo()
        {
            tblPessoaGrupo = new HashSet<tblPessoaGrupo>();
        }

        public int intGrupoID { get; set; }
        public int? intContactID { get; set; }
        public int? bitAtivo { get; set; }
        public int? intTipoGrupo { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual ICollection<tblPessoaGrupo> tblPessoaGrupo { get; set; }
    }
}
