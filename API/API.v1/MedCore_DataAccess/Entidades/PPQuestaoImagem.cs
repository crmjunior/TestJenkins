using System.Runtime.Serialization;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PPQuestaoImagem", Namespace = "a")]
	public class PPQuestaoImagem : QuestaoImagem
	{
		[DataMember(Name = "Url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "ByteArrayImagem", EmitDefaultValue = false)]
		public string ByteArrayImagem { get; set; }

        public AndamentoCadastroQuestao AndamentoCadastro { get; set; }
    }
}