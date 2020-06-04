using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Rule_Menu
    {
        public int intID { get; set; }
        public int? intRuleID { get; set; }
        public int? intMenuID { get; set; }

        public virtual tblAccess_Menu_Apagar intMenu { get; set; }
    }
}
