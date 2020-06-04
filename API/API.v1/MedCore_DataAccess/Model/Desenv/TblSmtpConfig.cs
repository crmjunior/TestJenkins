using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSmtpConfig
    {
        public int smtpID { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public string smtpFrom { get; set; }
        public string smtpUsername { get; set; }
        public string smtpPassword { get; set; }
        public bool smtpActive { get; set; }
        public bool? smtpDefaultCredentials { get; set; }
        public bool? smtpInterno { get; set; }
        public string profile_id { get; set; }
        public string displayFrom { get; set; }
    }
}
