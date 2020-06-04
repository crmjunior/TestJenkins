using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Cliente", Namespace = "a")]
    public class Cliente : Pessoa
    {
        [NotMapped]
        [DataMember(Name = "IdEspecialidade")]
        public int IdEspecialidade { get; set; }

        [NotMapped]
        [DataMember(Name = "Especialidade")]
        public string Especialidade { get; set; }

        [DataMember(Name = "Area")]
        public string Area { get; set; }

        [NotMapped]
        [DataMember(Name = "Pagamentos")]
        public List<PagamentosCliente> Pagamentos { get; set; }

        [DataMember(Name = "Retorno")]
        public StatusRetorno RetornoStatus { get; set; }

        [DataMember(Name = "Produtos")]
        public Produtos Produtos { get; set; }

        [NotMapped]
        public PermissoesApostilas ExerciciosPermitidos { get; set; }

        [DataMember(Name = "AnosPermitidos")]
        public List<int> AnosPermitidos { get; set; }

        [DataMember(Name = "ProdutosContratados")]
        public List<int> ProdutosContratados { get; set; }

        [NotMapped]
        [DataMember(Name = "Avatar")]
        public string Avatar { get; set; }

        public enum StatusRetorno
        {
            Sucesso = 0,
            SemAcesso = 13,
            Inexistente = 14,
            Inadimplente = 15,
            Cancelado = 16,
            SemAcessoCpMedR = 17
        }

        public enum TipoFoto
        {
            Padrao = 1,
        }

        [NotMapped]
        [DataMember(Name = "TituloMensagemRetorno")]
        public string TituloMensagemRetorno { get; set; }

        [NotMapped]
        [DataMember(Name = "MensagemRetorno")]
        public string MensagemRetorno { get; set; }

        [NotMapped]
        [DataMember(Name = "TipoErro")]
        public string TipoErro { get; set; }

        [NotMapped]
        [DataMember(Name = "IdFilial")]
        public int IdFilial { get; set; }

        [NotMapped]
        [DataMember(Name = "Filial")]
        public string Filial { get; set; }

        [NotMapped]
        [DataMember(Name = "Key")]
        public int Key { get; set; }

        [NotMapped]
        [DataMember(Name = "AcessoGolden")]
        public bool AcessoGolden { get; set; }

        [DataMember(Name = "SituacaoAluno")]
        public int SituacaoAluno { get; set; }

        [DataMember(Name = "Login")]
        public string Login { get; set; }

        [DataMember(Name = "LstOrdemVendaMsg")]
        public List<PermissaoAcessoItem> LstOrdemVendaMsg { get; set; }

        public TipoErroAcesso ETipoErro { get; set; }

    }
}