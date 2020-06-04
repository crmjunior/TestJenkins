using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Action
    {
        public tblAccess_Action()
        {
            tblAccess = new HashSet<tblAccess>();
            tblAccess_Screen_Action = new HashSet<tblAccess_Screen_Action>();
        }

        public int intActionId { get; set; }
        public string txtAction { get; set; }

        public virtual ICollection<tblAccess> tblAccess { get; set; }
        public virtual ICollection<tblAccess_Screen_Action> tblAccess_Screen_Action { get; set; }
    }
}
