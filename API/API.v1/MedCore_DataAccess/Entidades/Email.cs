using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Email", Namespace = "a")]
    public class Email
    {
        [DataMember(Name = "profile_name")]
        public String profile_name { get; set; }

        [DataMember(Name = "mailTo")]
        public String mailTo { get; set; }

        [DataMember(Name = "mailSubject")]
        public String mailSubject { get; set; }

        [DataMember(Name = "mailBody")]
        public String mailBody { get; set; }

        [DataMember(Name = "copyRecipients")]
        public String copyRecipients { get; set; }

        [DataMember(Name = "BlindCopyRecipients")]
        public String BlindCopyRecipients { get; set; }

        [DataMember(Name = "mailFrom")]
        public string mailFrom { get; set; }
    }
}