using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Menu_Apagar
    {
        public tblAccess_Menu_Apagar()
        {
            tblAccess_Rule_Menu = new HashSet<tblAccess_Rule_Menu>();
        }

        public int intMenuID { get; set; }
        public string txtDescription { get; set; }
        public string txtLink { get; set; }
        public int? intOrder { get; set; }
        public int? intApplicationID { get; set; }
        public bool? bitGlobal { get; set; }

        public virtual tblAccess_Application intApplication { get; set; }
        public virtual ICollection<tblAccess_Rule_Menu> tblAccess_Rule_Menu { get; set; }
    }
}
