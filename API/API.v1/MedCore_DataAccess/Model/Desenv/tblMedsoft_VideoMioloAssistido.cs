using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_VideoMioloAssistido
    {
        public int idId { get; set; }
        public int intClientId { get; set; }
        public int intVideoId { get; set; }
        public DateTime dteTimestamp { get; set; }
    }
}
