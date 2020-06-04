using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Contribuicao", Namespace = "a")]
    public class ContribuicaoInteracao
    {
        [DataMember(Name = "ContribuicaoInteracaoId", EmitDefaultValue = false)]
        public int ContribuicaoInteracaoId { get; set; }

        [DataMember(Name = "ContribuicaoId", EmitDefaultValue = false)]
        public int ContribuicaoId { get; set; }

        [DataMember(Name = "ClientId", EmitDefaultValue = false)]
        public int ClientId { get; set; }

        [DataMember(Name = "TipoInteracao", EmitDefaultValue = false)]
        public EnumTipoInteracao TipoInteracao { get; set; }

        [DataMember(Name = "DataCriacao", EmitDefaultValue = false)]
        public DateTime DataCriacao { get; set; }
    }

    public enum EnumTipoInteracao
    {
        Curtir = 1,
        Favoritar = 2,
        Denunciar = 3
    }
}