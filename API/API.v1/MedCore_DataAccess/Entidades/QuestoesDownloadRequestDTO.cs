using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestoesDownloadRequest", Namespace = "")]
    public class QuestoesDownloadRequestDTO
    {
        [DataMember(Name = "ClientId", EmitDefaultValue = false)]
        public int ClientId { get; set; }

        [DataMember(Name = "ApplicationId", EmitDefaultValue = false)]
        public int ApplicationId { get; set; }

        [DataMember(Name = "QuestoesIds", EmitDefaultValue = false)]
        public List<int> QuestoesIds { get; set; }
    }
}