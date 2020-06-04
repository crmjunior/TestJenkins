using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Permission_Rule
    {
        public tblAccess_Permission_Rule()
        {
            tblAccess_PermissionObject = new HashSet<tblAccess_PermissionObject>();
        }

        public int intPermissaoRegraId { get; set; }
        public string txtDescricao { get; set; }
        public int intRegraId { get; set; }
        public int intAccessoId { get; set; }
        public int intOrdem { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public int intEmployeeID { get; set; }
        public int? intMensagemId { get; set; }
        public bool bitDataLimite { get; set; }
        public int? intInterruptorId { get; set; }
        public bool? bitAtivo { get; set; }
        public DateTime? dteValidoDe { get; set; }
        public DateTime? dteValidoAte { get; set; }

        public virtual tblAccess_Permission_Status intAccesso { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblAccess_Rule intRegra { get; set; }
        public virtual ICollection<tblAccess_PermissionObject> tblAccess_PermissionObject { get; set; }
    }
}
