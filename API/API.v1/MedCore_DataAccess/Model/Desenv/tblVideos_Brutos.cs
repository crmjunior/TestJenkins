using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideos_Brutos
    {
        public DateTime dteDateTime { get; set; }
        public string txtCompleteFileName { get; set; }
        public string txtFileName { get; set; }
        public string txtDirectoryName { get; set; }
        public string txtExtension { get; set; }
        public int? intQuestaoID { get; set; }
        public DateTime? dteCriacao { get; set; }
        public int intID { get; set; }
        public int? ANYCAST { get; set; }
        public string OperadorANYCAST { get; set; }
    }
}
