using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ApostilaEntidade", Namespace = "a")]
    public class ApostilaEntidade
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int64 ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "IdProduto", EmitDefaultValue = false)]
        public int IdProduto { get; set; }
    }
}