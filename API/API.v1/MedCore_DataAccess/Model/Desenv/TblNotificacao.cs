using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacao
    {
        public tblNotificacao()
        {
            tblAccess_PermissionNotification = new HashSet<tblAccess_PermissionNotification>();
            tblNotificacaoDuvidas = new HashSet<tblNotificacaoDuvidas>();
        }

        public int intNotificacaoId { get; set; }
        public int intNotificacaoTipoId { get; set; }
        public int intEmployeeId { get; set; }
        public string txtTexto { get; set; }
        public int intApplicationId { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intClientID { get; set; }
        public DateTime dteLiberacao { get; set; }
        public string txtTitulo { get; set; }
        public string txtInfoAdicional { get; set; }
        public int? intTipoEnvio { get; set; }
        public int? intStatusEnvio { get; set; }
        public bool bitDestaque { get; set; }

        public virtual ICollection<tblAccess_PermissionNotification> tblAccess_PermissionNotification { get; set; }
        public virtual ICollection<tblNotificacaoDuvidas> tblNotificacaoDuvidas { get; set; }
    }
}
