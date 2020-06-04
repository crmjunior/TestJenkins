using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Setor", Namespace = "a")]
    public class Setor
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }
    }
}