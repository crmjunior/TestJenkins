using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil_Funcionalidade1
    {
        public int intPerfilFuncionalidadeID { get; set; }
        public int intPerfilID { get; set; }
        public int intFuncionalidadeID { get; set; }
        public string txtTipoPermissao { get; set; }

        public virtual tblFuncionalidade1 intFuncionalidade { get; set; }
        public virtual tblPerfil1 intPerfil { get; set; }
    }
}
