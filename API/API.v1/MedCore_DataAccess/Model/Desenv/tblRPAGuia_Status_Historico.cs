using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPAGuia_Status_Historico
    {
        public int intID { get; set; }
        public int intRPAGuiaID { get; set; }
        public int intStatusID { get; set; }
        public DateTime dteInclusao { get; set; }
        public int intUsuarioInclusao { get; set; }
        public string txtObservacao { get; set; }
        public DateTime dteGuia { get; set; }

        public virtual tblRPAGuia intRPAGuia { get; set; }
        public virtual tblRPAGuia_Status intStatus { get; set; }
    }
}
