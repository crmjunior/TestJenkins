using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Avatar", Namespace = "a")]
    public class Avatar
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "Caminho", EmitDefaultValue = false)]
        public string Caminho { get; set; }

        [DataMember(Name = "CaminhoImagemPadrao", EmitDefaultValue = false)]
        public string CaminhoImagemPadrao { get; set; }

        [DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
        public DateTime DataCadastro { get; set; }

        [DataMember(Name = "Imagem", EmitDefaultValue = false)]
        public string Imagem { get; set; }
    }
}