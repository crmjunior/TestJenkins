using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBloqueioArea
    {
        public tblBloqueioArea()
        {
            tblBloqueioConcurso = new HashSet<tblBloqueioConcurso>();
        }

        public int intBloqueioAreaId { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblBloqueioConcurso> tblBloqueioConcurso { get; set; }
    }
}
