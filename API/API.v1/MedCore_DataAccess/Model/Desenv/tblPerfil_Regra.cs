using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil_Regra
    {
        public tblPerfil_Regra()
        {
            tblMenu_PerfilRegra = new HashSet<tblMenu_PerfilRegra>();
        }

        public int intPerfilRegraId { get; set; }
        public int intPerfilAreaId { get; set; }
        public string txtAlias { get; set; }
        public string txtNome { get; set; }
        public string txtDescricao { get; set; }
        public string txtCategoria { get; set; }

        public virtual ICollection<tblMenu_PerfilRegra> tblMenu_PerfilRegra { get; set; }
    }
}
