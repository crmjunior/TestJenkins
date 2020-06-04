using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Menu
    {
        public tblAccess_Menu()
        {
            tblAccess_Menu_ProductGroup = new HashSet<tblAccess_Menu_ProductGroup>();
        }

        public int intMenuId { get; set; }
        public int intObjectId { get; set; }
        public string txtNome { get; set; }
        public string txtUrl { get; set; }
        public string txtTarget { get; set; }
        public bool bitAutenticacao { get; set; }
        public bool bitNovo { get; set; }
        public string txtExternalPageUrl { get; set; }

        public virtual ICollection<tblAccess_Menu_ProductGroup> tblAccess_Menu_ProductGroup { get; set; }
    }
}
