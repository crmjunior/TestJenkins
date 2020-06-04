using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblTelas
    {
        public tblTelas()
        {
            tblFuncionalidade = new HashSet<tblFuncionalidade>();
        }

        public int intTelaID { get; set; }
        public string txtURL { get; set; }
        public string txtNome { get; set; }

        public virtual ICollection<tblFuncionalidade> tblFuncionalidade { get; set; }
    }
}
