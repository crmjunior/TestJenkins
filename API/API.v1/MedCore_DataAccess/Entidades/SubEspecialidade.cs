using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "SubEspecialidade", Namespace = "a")]
	public class SubEspecialidade
	{

		[DataMember(Name = "ID", EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(Name = "Nome", EmitDefaultValue = false)]
		public string Nome { get; set; }

		[DataMember(Name = "GrandeArea", EmitDefaultValue = false)]
		public int GrandeArea { get; set; }

		[DataMember(Name = "SubEspecialidades", EmitDefaultValue = false)]
		public List<SubEspecialidade> SubEspecialidades { get; set; }

	}
}