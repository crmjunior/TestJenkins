using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCtrlPanel_AccessControl_Roles
    {
        public tblCtrlPanel_AccessControl_Roles()
        {
            tblCtrlPanel_AccessControl_Persons_X_Roles = new HashSet<tblCtrlPanel_AccessControl_Persons_X_Roles>();
        }

        public long intRoleId { get; set; }
        public string txtAlias { get; set; }
        public string txtName { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblCtrlPanel_AccessControl_Persons_X_Roles> tblCtrlPanel_AccessControl_Persons_X_Roles { get; set; }
    }
}
