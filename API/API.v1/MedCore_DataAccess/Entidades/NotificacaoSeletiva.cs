using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "NotificacaoSeletiva", Namespace = "a")]
    public class NotificacaoSeletiva
    {
		[DataMember(Name = "NotificacaoTipoId")]
		public TipoNotificacao NotificacaoTipoId { get; set; }

		[DataMember(Name = "DevicesUsuarios")]
        public List<string> DevicesUsuarios { get; set; }

        [DataMember(Name = "Titulo")]
        public string Titulo { get; set; }

        [DataMember(Name = "Mensagem")]
        public string Mensagem { get; set; }

		[DataMember(Name = "InfoAdicional")]
		public string InfoAdicional { get; set; }
	}
}