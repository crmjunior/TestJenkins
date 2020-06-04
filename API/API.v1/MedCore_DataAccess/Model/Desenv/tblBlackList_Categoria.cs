using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBlackList_Categoria
    {
        public tblBlackList_Categoria()
        {
            tblClients_BlackList = new HashSet<tblClients_BlackList>();
        }

        public int intCategoriaID { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblClients_BlackList> tblClients_BlackList { get; set; }
    }
}
