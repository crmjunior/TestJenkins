using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
     [DataContract(Name = "OneSignalNotification", Namespace = "a")]
    public class OneSignalNotification
    {
        [DataMember(Name = "include_player_ids")]
        public List<string> Devices { get; set; }

        [DataMember(Name = "app_id")]
        public string AppId { get; set; }

        [DataMember(Name = "gcm_key")]
        public string GcmKey { get; set; }

        [DataMember(Name = "headings")]
        public OneSignalMessage Title { get; set; }

        [DataMember(Name = "contents")]
        public OneSignalMessage Message { get; set; }

		[DataMember(Name = "data")]
		public IDictionary<string, string> Data { get; set; }

        [DataMember(Name = "delayed_option")]
        public string DelayedOption { get; set; }

    }
}