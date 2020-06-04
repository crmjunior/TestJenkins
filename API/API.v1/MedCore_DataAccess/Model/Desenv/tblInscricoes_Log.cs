using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoes_Log
    {
        public int intInscricoes_LogID { get; set; }
        public int intSellOrderID { get; set; }
        public int? intDeviceTipoID { get; set; }

        public virtual tblAccess_Device intDeviceTipo { get; set; }
    }
}
