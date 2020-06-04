using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAlunoCrossPlataformaWhiteList
    {
        public int intAlunoCrossPlataformaWhiteListId { get; set; }
        public int intClientId { get; set; }
        public DateTime dteDataInclusao { get; set; }
        public int intQuantidadeDispositivos { get; set; }
    }
}
