using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideoAprovacao
    {
        public tblRevisaoAulaVideoAprovacao()
        {
            tblRevisaoAulaVideoAprovacaoLog = new HashSet<tblRevisaoAulaVideoAprovacaoLog>();
        }

        public int intId { get; set; }
        public int intRevisaoAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public int intRevisaoAulaVideoTipoAprovadorId { get; set; }
        public bool bitAprovado { get; set; }

        public virtual ICollection<tblRevisaoAulaVideoAprovacaoLog> tblRevisaoAulaVideoAprovacaoLog { get; set; }
    }
}
