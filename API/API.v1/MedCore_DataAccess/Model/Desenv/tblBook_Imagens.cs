using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBook_Imagens
    {
        public int intBookImagemID { get; set; }
        public int intBookID { get; set; }
        public string txtUrl { get; set; }
        public string txtDescription { get; set; }
        public string txtCode { get; set; }
        public string txtFileName { get; set; }
        public int? intCapitulo { get; set; }
        public int? intCaso { get; set; }
        public int? intSubCaso { get; set; }
        public int? bitAtivo { get; set; }

        public virtual tblBooks intBook { get; set; }
    }
}
