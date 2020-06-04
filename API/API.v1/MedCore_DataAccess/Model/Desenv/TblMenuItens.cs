using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMenuItens
    {
        public tblMenuItens()
        {
            tblMenu_PerfilRegra = new HashSet<tblMenu_PerfilRegra>();
        }

        public int intId { get; set; }
        public int intIdPai { get; set; }
        public int intPosicao { get; set; }
        public string txtNome { get; set; }
        public string txtLink { get; set; }
        public bool bitAtivo { get; set; }
        public int? intRegraId { get; set; }

        public virtual ICollection<tblMenu_PerfilRegra> tblMenu_PerfilRegra { get; set; }
    }
}
