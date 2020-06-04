using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "OfflineConfig", Namespace = "a")]
    public class OfflineConfig
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Minutos")]
        public int Minutos { get; set; }
    }
}