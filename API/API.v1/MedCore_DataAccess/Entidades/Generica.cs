using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Generica", Namespace = "a")]
    public class Generica
    {
        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Valor", EmitDefaultValue = false)]
        public int? Valor { get; set; }
    }
}