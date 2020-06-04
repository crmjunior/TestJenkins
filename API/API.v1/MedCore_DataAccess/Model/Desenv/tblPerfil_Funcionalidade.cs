using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil_Funcionalidade
    {
        public int intPerfilFuncionalidadeID { get; set; }
        public int intPerfilID { get; set; }
        public int intFuncionalidadeID { get; set; }
        public string txtTipoPermissao { get; set; }

        public virtual tblFuncionalidade intFuncionalidade { get; set; }
        public virtual tblPerfil intPerfil { get; set; }
    }
}
