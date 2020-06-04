using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "StatusRecurso", Namespace = "a")]
	public class StatusRecurso
	{
		[DataMember(Name = "ID", EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(Name = "Descricao", EmitDefaultValue = false)]
		public string Descricao { get; set; }
	}
}