using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInstrucaoPostagemCheque
    {
        public int intInstrucaoPostagemChequeId { get; set; }
        public int intProdutoId { get; set; }
        public int intAno { get; set; }
        public string txtDescricao { get; set; }
        public string txtLinkPDF { get; set; }
        public int? intTemplateId { get; set; }
    }
}
