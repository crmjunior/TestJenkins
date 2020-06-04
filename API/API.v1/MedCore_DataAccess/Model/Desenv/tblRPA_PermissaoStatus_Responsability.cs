using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_PermissaoStatus_Responsability
    {
        public int intPermissaoStatusXResponsabilityID { get; set; }
        public int intStatusID { get; set; }
        public int intResponsabilityId { get; set; }
        public bool boolVisualiza { get; set; }
        public bool boolTroca { get; set; }
    }
}
