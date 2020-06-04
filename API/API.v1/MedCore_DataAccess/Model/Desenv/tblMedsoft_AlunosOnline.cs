using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedsoft_AlunosOnline
    {
        public int intAlunosOnlineID { get; set; }
        public int? intClientID { get; set; }
        public string txtMachineToken { get; set; }
        public DateTime? dteTimeStamp { get; set; }
    }
}
