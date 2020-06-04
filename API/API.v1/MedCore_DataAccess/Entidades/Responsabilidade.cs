using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Responsabilidade")]
    public class Responsabilidade
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }
        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }
        [DataMember(Name = "Visualiza")]
        public bool Visualiza { get; set; }
        [DataMember(Name = "Troca")]
        public bool Troca { get; set; }
    }
}