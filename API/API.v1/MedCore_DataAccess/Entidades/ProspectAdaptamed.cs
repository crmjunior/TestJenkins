using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "ProspectAdaptamed", Namespace = "a")]
    public class ProspectAdaptamed : Pessoa
	{
		[DataMember( Name = "IdPais", EmitDefaultValue = false )]
		public int IdPais { get; set; }

		[DataMember( Name = "AnoFormatura", EmitDefaultValue = false )]
		public int AnoFormatura { get; set; }
	}
}