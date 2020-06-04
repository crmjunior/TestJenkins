using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MaterialApostilaConfig", Namespace = "")]
    public class MaterialApostilaConfig
    {
        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Ativa")]
        public bool Ativa { get; set; }
    }
}