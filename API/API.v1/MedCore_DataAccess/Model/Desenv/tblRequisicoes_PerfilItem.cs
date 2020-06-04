using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_PerfilItem
    {
        public int intPerfilItemId { get; set; }
        public int intPerfilId { get; set; }
        public int intTipoPerfil { get; set; }
        public int? intEmployeeId { get; set; }
        public int? intResponsabilityId { get; set; }
        public int? intCargoId { get; set; }
        public bool? bitAutoAprovador { get; set; }

        public virtual tblEmployeeCargos intCargo { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblRequisicoes_Perfil intPerfil { get; set; }
    }
}
