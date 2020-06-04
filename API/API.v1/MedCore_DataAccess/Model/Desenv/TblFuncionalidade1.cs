using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblFuncionalidade1
    {
        public tblFuncionalidade1()
        {
            tblPerfil_Funcionalidade1 = new HashSet<tblPerfil_Funcionalidade1>();
        }

        public int intFuncionalidadeID { get; set; }
        public string txtURI { get; set; }
        public string txtAliasFuncionalidade { get; set; }
        public string txtDescricao { get; set; }
        public int intTelaID { get; set; }

        public virtual tblTelas1 intTela { get; set; }
        public virtual ICollection<tblPerfil_Funcionalidade1> tblPerfil_Funcionalidade1 { get; set; }
    }
}
