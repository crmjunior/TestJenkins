using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblReplaceHtmlTags
    {
        public int intId { get; set; }
        public string txtTag { get; set; }
        public string txtReplace { get; set; }
    }
}
