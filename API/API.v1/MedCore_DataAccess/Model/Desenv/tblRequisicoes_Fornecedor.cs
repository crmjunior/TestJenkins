using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Fornecedor
    {
        public tblRequisicoes_Fornecedor()
        {
            tblRequisicoes_FornecedorContato = new HashSet<tblRequisicoes_FornecedorContato>();
            tblRequisicoes_FornecedorFormaPagamento = new HashSet<tblRequisicoes_FornecedorFormaPagamento>();
            tblRequisicoes_Fornecedor_Produto = new HashSet<tblRequisicoes_Fornecedor_Produto>();
            tblRequisicoes_RequisicaoCotacao = new HashSet<tblRequisicoes_RequisicaoCotacao>();
        }

        public int intFornecedorId { get; set; }
        public string txtNome { get; set; }
        public string txtNomeFantasia { get; set; }
        public string txtCPFCNPJ { get; set; }
        public bool? bitPessoaFisica { get; set; }
        public string txtEndereco { get; set; }
        public string txtComplemento { get; set; }
        public string txtBairro { get; set; }
        public string txtCidade { get; set; }
        public string txtUF { get; set; }
        public string txtCEP { get; set; }
        public int? intBancoId { get; set; }
        public string txtAgencia { get; set; }
        public string txtConta { get; set; }
        public string txtObservacao { get; set; }
        public int? intQualificacao { get; set; }
        public string txtTelefone { get; set; }
        public string txtEmail { get; set; }
        public string txtSite { get; set; }
        public bool? bitAtivo { get; set; }
        public int? intPlanilha { get; set; }

        public virtual ICollection<tblRequisicoes_FornecedorContato> tblRequisicoes_FornecedorContato { get; set; }
        public virtual ICollection<tblRequisicoes_FornecedorFormaPagamento> tblRequisicoes_FornecedorFormaPagamento { get; set; }
        public virtual ICollection<tblRequisicoes_Fornecedor_Produto> tblRequisicoes_Fornecedor_Produto { get; set; }
        public virtual ICollection<tblRequisicoes_RequisicaoCotacao> tblRequisicoes_RequisicaoCotacao { get; set; }
    }
}
