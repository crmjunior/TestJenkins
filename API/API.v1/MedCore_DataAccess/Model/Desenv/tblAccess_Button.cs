using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Button
    {
        public int intButtonId { get; set; }
        public int intObjectId { get; set; }
        public string txtNome { get; set; }
        public string txtUrl { get; set; }
    }
}
