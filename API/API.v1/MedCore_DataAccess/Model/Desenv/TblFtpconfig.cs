using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblFTPConfig
    {
        public int intFtpID { get; set; }
        public string txtFtpServer { get; set; }
        public string txtFtpUser { get; set; }
        public string txtFtpPassword { get; set; }
        public string txtFtpVirtualPath { get; set; }
        public string txtFtpPath { get; set; }
        public string txtUrl { get; set; }
        public string txtApplication { get; set; }
    }
}
