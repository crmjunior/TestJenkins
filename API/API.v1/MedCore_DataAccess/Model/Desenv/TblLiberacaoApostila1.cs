using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLiberacaoApostila1
    {
        public int intLiberacaoApostilaId { get; set; }
        public bool bitLiberado { get; set; }
        public int IntEmployeeId { get; set; }
        public DateTime dteDateTime { get; set; }
        public int? intBookId { get; set; }

        public virtual tblBooks intBook { get; set; }
    }
}
