using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MaterialApostilaProgresso", Namespace = "")]
    public class MaterialApostilaProgresso
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "ApostilaId")]
        public int ApostilaId { get; set; }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "Progresso")]
        public decimal Progresso { get; set; }
    }
}