using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAcademicoVideoEmail
    {
        public int intAcademicoVideoEmail { get; set; }
        public int? intVideoId { get; set; }
        public int? intVimeoId { get; set; }
        public string txtName { get; set; }
        public string txtVideoStatus { get; set; }
        public DateTime? dteOcorrencia { get; set; }
        public bool bitEmailEviado { get; set; }
    }
}
