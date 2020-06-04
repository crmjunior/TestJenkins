using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_MenuProduto
    {
        public int intID { get; set; }
        public int intMenuID { get; set; }
        public int intProdutoID { get; set; }

        public virtual tblAccess_Object intMenu { get; set; }
        public virtual tblProductGroups1 intProduto { get; set; }
    }
}
