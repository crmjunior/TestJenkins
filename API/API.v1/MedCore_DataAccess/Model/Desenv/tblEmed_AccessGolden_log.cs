using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmed_AccessGolden_log
    {
        public int intID { get; set; }
        public string txtCPF { get; set; }
        public string txtComment { get; set; }
        public int? intEmployeeID { get; set; }
        public DateTime? dteDateTime { get; set; }
        public bool? bitEterno { get; set; }
        public string txtAcao { get; set; }
    }
}
