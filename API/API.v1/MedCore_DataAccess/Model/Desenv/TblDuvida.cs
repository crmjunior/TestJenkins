using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvida
    {
        public int intDuvidaId { get; set; }
        public int intClientId { get; set; }
        public string txtDuvida { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
        public int intApplicationId { get; set; }
    }
}
