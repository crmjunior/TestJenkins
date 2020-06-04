using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPermissaoInadimplenciaLogConfirmacaoAlerta
    {
        public int intLogConfirmacaoId { get; set; }
        public int intClientId { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intApplicationId { get; set; }
    }
}
