using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogAvisos
    {
        public int intLogAvisoID { get; set; }
        public int intClientID { get; set; }
        public int intAvisoID { get; set; }
        public DateTime dteVisualizacao { get; set; }
        public bool bitConfirmaVisualizacao { get; set; }
    }
}
