using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "TipoProva", Namespace = "a")]
    public class TipoProva
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public int Ordem { get; set; }

        [DataMember(Name = "Discursiva", EmitDefaultValue = false)]
        public bool Discursiva { get; set; }

        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public string Tipo { get; set; }
    }
}