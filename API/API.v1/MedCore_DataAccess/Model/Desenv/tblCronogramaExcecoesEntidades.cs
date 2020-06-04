using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCronogramaExcecoesEntidades
    {
        public int intExcecaoEntidadeId { get; set; }
        public long intBookEntityId { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteDataInclusao { get; set; }
    }
}
