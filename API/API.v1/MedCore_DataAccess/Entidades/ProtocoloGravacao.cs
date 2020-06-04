using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "ProtocoloGravacao", Namespace = "a")]
	public class ProtocoloGravacao
	{
		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "Nome", EmitDefaultValue = false)]
		public string Nome { get; set; }

		[DataMember(Name = "IdFuncionario", EmitDefaultValue = false)]
		public int IdFuncionario { get; set; }

		[DataMember(Name = "Data", EmitDefaultValue = false)]
		public double Data { get; set; }

		[DataMember(Name = "IdOrigem", EmitDefaultValue = false)]
		public int IdOrigem { get; set; }

		[DataMember(Name = "Tipo")]
		public TipoProtocolo Tipo { get; set; }

		[DataMember(Name = "Questoes", EmitDefaultValue = false)]
		public List<PPQuestao> Questoes { get; set; }

		[DataMember(Name = "Professores", EmitDefaultValue = false)]
		public List<Professor> Professores { get; set; }

		[DataMember(Name = "Distribuicoes", EmitDefaultValue = false)]
		public List<DistribuicaoProtocolo> Distribuicoes { get; set; }
	}

	[DataContract(Name = "TipoProtocolo", Namespace = "a")]
	public enum TipoProtocolo
	{
		Apostila = 0,
		Prova = 1,
        GrupoQuestoes = 2
	}
}