using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Screen
    {
        public tblAccess_Screen()
        {
            tblAccess = new HashSet<tblAccess>();
            tblAccess_Screen_Action = new HashSet<tblAccess_Screen_Action>();
        }

        public int intScreenID { get; set; }
        public int intApplicationID { get; set; }
        public string txtScreenName { get; set; }
        public int intProductGroupId { get; set; }

        public virtual tblAccess_Application intApplication { get; set; }
        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual ICollection<tblAccess> tblAccess { get; set; }
        public virtual ICollection<tblAccess_Screen_Action> tblAccess_Screen_Action { get; set; }
    }
}
