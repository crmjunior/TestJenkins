using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblFuncionalidade
    {
        public tblFuncionalidade()
        {
            tblPerfil_Funcionalidade = new HashSet<tblPerfil_Funcionalidade>();
        }

        public int intFuncionalidadeID { get; set; }
        public string txtURI { get; set; }
        public string txtAliasFuncionalidade { get; set; }
        public string txtDescricao { get; set; }
        public int intTelaID { get; set; }

        public virtual tblTelas intTela { get; set; }
        public virtual ICollection<tblPerfil_Funcionalidade> tblPerfil_Funcionalidade { get; set; }
    }
}
