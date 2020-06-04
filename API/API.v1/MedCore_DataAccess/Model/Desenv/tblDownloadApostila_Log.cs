using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDownloadApostila_Log
    {
        public int intDownloadApostilaID { get; set; }
        public int intContactId { get; set; }
        public DateTime dteDate { get; set; }
        public int intProductID { get; set; }
        public string txtNomeArquivo { get; set; }
        public bool bitAutorizada { get; set; }
        public int bitQuestoesOriginais { get; set; }
        public bool bitConcluido { get; set; }
    }
}
