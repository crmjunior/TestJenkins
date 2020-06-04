using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Alternativa", Namespace = "a")]
	public class Alternativa
	{
		[DataMember(Name = "Letra", EmitDefaultValue = false)]
		public Char Letra { get; set; }

		[DataMember(Name = "LetraStr", EmitDefaultValue = false)]
		public string LetraStr { get; set; }

		[DataMember(Name = "Nome", EmitDefaultValue = false)]
		public String Nome { get; set; }

		[DataMember(Name = "Correta", EmitDefaultValue = false)]
		public Boolean Correta { get; set; }

		[DataMember(Name = "CorretaPreliminar", EmitDefaultValue = false)]
		public Boolean CorretaPreliminar { get; set; }

		[DataMember(Name = "Selecionada", EmitDefaultValue = false)]
		public Boolean Selecionada { get; set; }

        [DataMember(Name = "Editar")]
        public Boolean Editar { get; set; }

		[DataMember(Name = "Resposta", EmitDefaultValue = false)]
		public String Resposta { get; set; }

		[DataMember(Name = "Gabarito", EmitDefaultValue = false)]
		public String Gabarito { get; set; }

		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

        [DataMember(Name = "IdQuestao", EmitDefaultValue = false)]
        public int IdQuestao { get; set; }

        [DataMember(Name = "Nota", EmitDefaultValue = false)]
        public double? Nota { get; set; }

        [DataMember(Name = "Imagem", EmitDefaultValue = false)]
        public String Imagem { get; set; }

        [DataMember(Name = "ImagemOtimizada", EmitDefaultValue = false)]
        public String ImagemOtimizada { get; set; }
	}
}