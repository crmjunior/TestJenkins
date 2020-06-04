using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideoLog
    {
        public int intVideoLogId { get; set; }
        public int intOrigemVideoId { get; set; }
        public int intTipoVideo { get; set; }
        public int intClientId { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
