using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_PermissionObject
    {
        public int intPermissionObject { get; set; }
        public int intObjectId { get; set; }
        public int intPermissaoRegraId { get; set; }
        public int intOrdem { get; set; }
        public DateTime? dteDataAlteracao { get; set; }
        public int intApplicationID { get; set; }

        public virtual tblAccess_Object intObject { get; set; }
        public virtual tblAccess_Permission_Rule intPermissaoRegra { get; set; }
    }
}
