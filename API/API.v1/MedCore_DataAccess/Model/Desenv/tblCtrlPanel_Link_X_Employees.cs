using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCtrlPanel_Link_X_Employees
    {
        public int intEmployeeID { get; set; }
        public int intLinkID { get; set; }
        public bool? boolPermissaoNegada { get; set; }
        public int intLinkEmployeeID { get; set; }
    }
}
