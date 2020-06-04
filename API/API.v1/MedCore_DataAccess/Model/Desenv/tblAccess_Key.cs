using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Key
    {
        public int intKeyID { get; set; }
        public string txtPublicKey { get; set; }
        public string txtPrivateKey { get; set; }
        public bool bitActive { get; set; }
        public int intApplicationID { get; set; }
        public string txtShortPublicKey { get; set; }
    }
}
