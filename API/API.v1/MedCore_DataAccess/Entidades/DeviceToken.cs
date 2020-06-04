using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DeviceToken", Namespace = "a")]
    public class DeviceToken
    {
        [DataMember(Name = "Token")]
        public string Token { get; set; }

        [DataMember(Name = "Register")]
        public int Register { get; set; }

		[DataMember(Name = "dteDataCriacao")]
		public System.DateTime? dteDataCriacao { get; set; }

		[DataMember(Name = "bitIsTablet")]
		public bool? bitIsTablet { get; set; }

		[DataMember(Name = "bitAtivo")]
		public bool? bitAtivo { get; set; }

        [DataMember(Name = "AplicacaoId")]
        public int AplicacaoId { get; set; }
    }
}