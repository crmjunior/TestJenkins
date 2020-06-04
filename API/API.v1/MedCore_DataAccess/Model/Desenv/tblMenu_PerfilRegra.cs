using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMenu_PerfilRegra
    {
        public int intMenuRegraId { get; set; }
        public int intMenuId { get; set; }
        public int intRegraId { get; set; }

        public virtual tblMenuItens intMenu { get; set; }
        public virtual tblPerfil_Regra intRegra { get; set; }
    }
}
