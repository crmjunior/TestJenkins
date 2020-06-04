using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MaterialApostilaAluno", Namespace = "")]
    public class MaterialApostilaAluno : MaterialApostila
    {
        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "Ativa")]
        public bool Ativa { get; set; }

        [DataMember(Name = "ApostilaId")]
        public int ApostilaId { get; set; }

        [DataMember(Name = "Versao")]
        public long Versao { get; set; }

        [DataMember(Name = "DataRelease")]
        public long DataRelease { get; set; }

        [DataMember(Name = "Configs")]
        public List<MaterialApostilaConfig> Configs { get; set; }

        [DataMember(Name = "Bloqueado")]
        public bool Bloqueado { get; set; }
        [DataMember(Name = "NaoInterageDuvidas")]
        public bool NaoInterageDuvidas { get; set; }
    }
}