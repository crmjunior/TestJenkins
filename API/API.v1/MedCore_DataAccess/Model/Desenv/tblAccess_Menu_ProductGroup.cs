using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Menu_ProductGroup
    {
        public int intMenuProductGroup { get; set; }
        public int intMenuId { get; set; }
        public int intProductGroup { get; set; }

        public virtual tblAccess_Menu intMenu { get; set; }
    }
}
