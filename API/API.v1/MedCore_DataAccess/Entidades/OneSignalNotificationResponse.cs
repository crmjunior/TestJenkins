using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "OneSignalNotificationResponse", Namespace = "a")]
    public class OneSignalNotificationResponse
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Status")]
        public bool Sucesso { get; set; }

    }
}