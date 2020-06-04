using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Ativo
    {
        public tblRequisicoes_Ativo()
        {
            tblRequisicoes_AtivoMovimentacao = new HashSet<tblRequisicoes_AtivoMovimentacao>();
            tblRequisicoes_Ativo_Historico = new HashSet<tblRequisicoes_Ativo_Historico>();
            tblRequisicoes_Ativo_ProdutoCaracteristica = new HashSet<tblRequisicoes_Ativo_ProdutoCaracteristica>();
        }

        public int intAtivoId { get; set; }
        public int? intProdutoId { get; set; }
        public string txtNumeroSerie { get; set; }
        public bool? bitAtivo { get; set; }
        public string txtModelo { get; set; }
        public string txtMarca { get; set; }
        public string txtNumeroPatrimonio { get; set; }
        public int? intStatus { get; set; }
        public string txtAcessorio { get; set; }
        public decimal? dblValor { get; set; }
        public DateTime? dteDataCompra { get; set; }
        public string txtObservacao { get; set; }
        public int? intRequisicaoId { get; set; }
        public bool? bitImportacao { get; set; }
        public int? intCompanyId { get; set; }
        public int? intCompanyIdOriginal { get; set; }

        public virtual tblCompanies intCompany { get; set; }
        public virtual tblCompanies intCompanyIdOriginalNavigation { get; set; }
        public virtual tblRequisicoes_Produto intProduto { get; set; }
        public virtual ICollection<tblRequisicoes_AtivoMovimentacao> tblRequisicoes_AtivoMovimentacao { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo_Historico> tblRequisicoes_Ativo_Historico { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo_ProdutoCaracteristica> tblRequisicoes_Ativo_ProdutoCaracteristica { get; set; }
    }
}
