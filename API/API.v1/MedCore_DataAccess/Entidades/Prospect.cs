using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Prospect", Namespace = "a")]
    public class Prospect : Pessoa
    {
        [DataMember(Name = "Instituicao", EmitDefaultValue = false)]
        public string Instituicao { get; set; }
    }
}