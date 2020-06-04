using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_PermissaoXResponsability
    {
        public int intPermissaoXResponsabilityID { get; set; }
        public int intTipoPermissaoID { get; set; }
        public int intResponsabilityId { get; set; }
    }
}
