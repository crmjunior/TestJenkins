using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMensagens
    {
        public int intMensagemId { get; set; }
        public int intAplication { get; set; }
        public string txtMensagem { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteInclusao { get; set; }
    }
}
