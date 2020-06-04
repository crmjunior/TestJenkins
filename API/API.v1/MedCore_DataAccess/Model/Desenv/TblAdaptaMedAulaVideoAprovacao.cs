using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAdaptaMedAulaVideoAprovacao
    {
        public int intId { get; set; }
        public int intAdaptaMedAulaVideoId { get; set; }
        public int? intVideoId { get; set; }
        public int intAdaptaMedAulaVideoTipoAprovadorId { get; set; }
        public bool bitAprovado { get; set; }
    }
}
