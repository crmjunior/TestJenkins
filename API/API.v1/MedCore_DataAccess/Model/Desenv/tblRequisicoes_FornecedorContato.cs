using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_FornecedorContato
    {
        public int intFornecedorContatoId { get; set; }
        public int intFornecedorId { get; set; }
        public string txtNome { get; set; }
        public string txtTelefone { get; set; }
        public string txtEmail { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblRequisicoes_Fornecedor intFornecedor { get; set; }
    }
}
