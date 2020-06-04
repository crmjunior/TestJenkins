using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogOrdemVenda
    {
        public int intLogOrdemVendaID { get; set; }
        public int intClientID { get; set; }
        public DateTime dataInclusao { get; set; }
        public int? GroupId { get; set; }
    }
}
