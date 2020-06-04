using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Prova", Namespace = "a")]
    public class Prova
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "TipoProva", EmitDefaultValue = false)]
        public TipoProva TipoProva { get; set; }

        [DataMember(Name = "SobDemanda", EmitDefaultValue = false)]
        public bool SobDemanda { get; set; }
    }
}