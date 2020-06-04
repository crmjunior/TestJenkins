using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Permission_Status
    {
        public tblAccess_Permission_Status()
        {
            tblAccess_Permission_Rule = new HashSet<tblAccess_Permission_Rule>();
        }

        public int intAccessoId { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblAccess_Permission_Rule> tblAccess_Permission_Rule { get; set; }
    }
}
