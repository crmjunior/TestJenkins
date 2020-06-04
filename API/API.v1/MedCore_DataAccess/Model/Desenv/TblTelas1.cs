using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblTelas1
    {
        public tblTelas1()
        {
            tblFuncionalidade1 = new HashSet<tblFuncionalidade1>();
        }

        public int intTelaID { get; set; }
        public string txtURL { get; set; }
        public string txtNome { get; set; }

        public virtual ICollection<tblFuncionalidade1> tblFuncionalidade1 { get; set; }
    }
}
