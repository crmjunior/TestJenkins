using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ApostilaUnificada", Namespace = "a")]
    public class ApostilaUnificada
    {
        [DataMember(Name = "BookAtualId", EmitDefaultValue = false)]
        public long BookAtualId { get; set; }

        [DataMember(Name = "BookAnteriorId", EmitDefaultValue = false)]
        public long BookAnteriorId { get; set; }

        [DataMember(Name = "AnoUnificacao", EmitDefaultValue = false)]
        public int AnoUnificacao { get; set; }

		[DataMember(Name = "EntityAtualId", EmitDefaultValue = false)]
		public long EntityAtualId { get; set; }

        [DataMember(Name = "EntityAtual", EmitDefaultValue = false)]
        public string EntityAtual { get; set; }

        [DataMember(Name = "EntityAnteriorId", EmitDefaultValue = false)]
        public long EntityAnteriorId { get; set; }

        [DataMember(Name = "EntityAnterior", EmitDefaultValue = false)]
        public string EntityAnterior { get; set; }
    }
}