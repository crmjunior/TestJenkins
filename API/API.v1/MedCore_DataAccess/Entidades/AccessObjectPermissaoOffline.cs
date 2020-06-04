using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AccessObjectPermissaoOffline", Namespace = "a")]
    public class AccessObjectPermissaoOffline
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "VersaoMinimaOffline")]
        public string VersaoMinimaOffline { get; set; }

        [DataMember(Name = "PermiteOffline")]
        public int PermiteOffline { get; set; }
    }
}