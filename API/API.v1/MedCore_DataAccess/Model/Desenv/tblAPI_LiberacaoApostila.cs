using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAPI_LiberacaoApostila
    {
        public int intBookID { get; set; }
        public bool bitLiberado { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDateTime { get; set; }
        public int intLiberacaoApostilaID { get; set; }

        public virtual tblBooks intBook { get; set; }
    }
}
