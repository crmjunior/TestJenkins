using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "RevisaoAprovacao", Namespace = "a")]
	public class RevisaoAprovacao
	{
		[DataMember(Name = "Aprovado", EmitDefaultValue = false)]
		public bool Aprovado { get; set; }

		[DataMember(Name = "IDProfessor", EmitDefaultValue = false)]
		public int IDProfessor { get; set; }

		[DataMember(Name = "NomeProfessor", EmitDefaultValue = false)]
		public string NomeProfessor { get; set; }

		[DataMember(Name = "RevisaoAulaID", EmitDefaultValue = false)]
		public int RevisaoAulaID { get; set; }

		[DataMember(Name = "Motivo", EmitDefaultValue = false)]
		public string Motivo { get; set; }
	}
}