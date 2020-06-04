using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_PermissaoStatus_Employee
    {
        public int intPermissaoStatusxEmployeeId { get; set; }
        public int intGuiaStatusId { get; set; }
        public int intEmployeeId { get; set; }
        public bool? bitVisualiza { get; set; }
        public bool? bitTroca { get; set; }
    }
}
