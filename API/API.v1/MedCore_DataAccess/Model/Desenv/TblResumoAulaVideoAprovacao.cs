using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblResumoAulaVideoAprovacao
    {
        public int intId { get; set; }
        public int intResumoAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public int intResumoAulaVideoTipoAprovadorId { get; set; }
        public bool bitAprovado { get; set; }
    }
}
