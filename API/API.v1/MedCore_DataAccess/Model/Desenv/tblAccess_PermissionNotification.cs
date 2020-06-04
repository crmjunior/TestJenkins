using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_PermissionNotification
    {
        public int intPermissionNotificationId { get; set; }
        public int intNotificacaoId { get; set; }
        public int intEmployeeId { get; set; }
        public int intPermissaoRegra { get; set; }
        public int intOrdem { get; set; }
        public DateTime? dteDataAlteracao { get; set; }

        public virtual tblNotificacao intNotificacao { get; set; }
    }
}
