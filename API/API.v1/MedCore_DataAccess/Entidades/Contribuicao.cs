using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Contribuicao", Namespace = "a")]
    public class Contribuicao
    {
        [DataMember(Name = "ContribuicaoId", EmitDefaultValue = false)]
        public int ContribuicaoId { get; set; }

        [DataMember(Name = "ClientId", EmitDefaultValue = false)]
        public int ClientId { get; set; }

        [DataMember(Name = "MedGrupoID", EmitDefaultValue = false)]
        public int MedGrupoID { get; set; }

        [DataMember(Name = "ApostilaId", EmitDefaultValue = false)]
        public int ApostilaId { get; set; }

        [DataMember(Name = "DataCriacao", EmitDefaultValue = false)]
        public DateTime DataCriacao { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "BitAtivo", EmitDefaultValue = false)]
        public bool BitAtivo { get; set; }

        [DataMember(Name = "Origem", EmitDefaultValue = false)]
        public string Origem { get; set; }

        [DataMember(Name = "IsEditado", EmitDefaultValue = false)]
        public bool IsEditado { get; set; }

        [DataMember(Name = "BitAprovacaoMedgrupo")]
        public bool BitAprovacaoMedgrupo { get; set; }

        [DataMember(Name = "Estado")]
        public string Estado { get; set; }


        [DataMember(Name = "NumeroCapitulo", EmitDefaultValue = false)]
        private string _NumeroCapitulo;
        public dynamic NumeroCapitulo { 
            get => _NumeroCapitulo; 
            set => this._NumeroCapitulo = value.ToString();            
        }

        [DataMember(Name = "TrechoApostila", EmitDefaultValue = false)]
        public string TrechoApostila { get; set; }

        [DataMember(Name = "CodigoMarcacao", EmitDefaultValue = false)]
        public string CodigoMarcacao { get; set; }

        [DataMember(Name = "Arquivos", EmitDefaultValue = true)]
        public IList<ContribuicaoArquivo> Arquivos { get; set; }

        [DataMember(Name = "OrigemSubnivel", EmitDefaultValue = false)]
        public string OrigemSubnivel { get; set; }

        [DataMember(Name = "ProfessoresSelecionados")]
        public List<int> ProfessoresSelecionados { get; set; }

        [DataMember(Name = "BitAprovarMaisTarde")]
        public bool BitAprovarMaisTarde { get; set; }

        [DataMember(Name = "BitMedGrupo")]
        public bool BitMedGrupo { get; set; }

        [DataMember(Name = "CursoAluno")]
        public string CursoAluno { get; set; }

        [DataMember(Name = "TipoCategoria")]
        public EnumTipoCategoriaContribuicao TipoCategoria { get; set; }

        [DataMember(Name = "AprovacaoID")]
        public int? AprovacaoID { get; set; }

        [DataMember(Name = "TipoContribuicao")]
        public EnumTipoArquivoContribuicao TipoContribuicao { get; set; }

        [DataMember(Name = "OpcaoPrivacidade")]
        public TipoOpcaoPrivacidade OpcaoPrivacidade { get; set; }
    }


    public enum EnumTipoCategoriaContribuicao
    {
        Indefinido = 0,
        Capitulo = 1,
        Hypotesis = 2,
        Conteudo = 3
    }

    public enum TipoOpcaoPrivacidade
    {
        Indefinido = 0,
        Publica = 1,
        PrivadaFimContrato = 2
    }

    public enum EnumTipoArquivoContribuicao
    {
        Indefinido = 0,
        Imagem = 1,
        Audio = 2,
        Video = 3,
        Texto = 4,
        Mnemonica = 5
    }

    public enum EnumTipoInteracaoContribuicao
    {
        Indefinido = 0,
        Curtir = 1,
        Favorito = 2
    }
}