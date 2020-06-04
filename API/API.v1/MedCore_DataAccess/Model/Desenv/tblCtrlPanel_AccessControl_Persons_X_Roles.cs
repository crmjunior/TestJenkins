using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCtrlPanel_AccessControl_Persons_X_Roles
    {
        public long intId { get; set; }
        public long intContactId { get; set; }
        public long intRoleId { get; set; }

        public virtual tblCtrlPanel_AccessControl_Roles intRole { get; set; }
    }
}
