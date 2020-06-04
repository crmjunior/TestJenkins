using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PermissaoDevice", Namespace = "a")]
    public class PermissaoDevice
    {
        [DataMember(Name = "PermiteAcesso")]
        public int PermiteAcesso { get; set; }

        [DataMember(Name = "PermiteTroca")]
        public int PermiteTroca { get; set; }
    }
}