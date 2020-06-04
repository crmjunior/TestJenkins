using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_TipoInteracao
    {
        public tblDuvidasAcademicas_TipoInteracao()
        {
            tblDuvidasAcademicas_Interacoes = new HashSet<tblDuvidasAcademicas_Interacoes>();
        }

        public int intTipoInteracaoId { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblDuvidasAcademicas_Interacoes> tblDuvidasAcademicas_Interacoes { get; set; }
    }
}
