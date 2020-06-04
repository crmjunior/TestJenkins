using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
     [DataContract(Name = "OneSignalMessage", Namespace = "a")]
    public class OneSignalMessage
    {
        public OneSignalMessage(string msg)
        {
            Message = msg;
        }

        [DataMember(Name = "en")]
        public string Message { get; set; }
    }
}