using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLiberacaoApostila_Historico
    {
        public int IntID { get; set; }
        public int intEmployeeID { get; set; }
        public int bitLiberado { get; set; }
        public int intBookID { get; set; }
        public DateTime dteDataAlteracao { get; set; }
        public string txtAcao { get; set; }
        public string txtUsuarioBD { get; set; }
    }
}
