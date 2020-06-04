using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoImagem", Namespace = "a")]
    public class QuestaoImagem
    {
        [DataMember(Name = "QuestaoId", EmitDefaultValue = false)]
        public Int32 QuestaoId { get; set; }

        [DataMember(Name = "Label", EmitDefaultValue = false)]
        public String Label { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public String Nome { get; set; }

        [DataMember(Name = "Imagem", EmitDefaultValue = false)]
        public byte[] Imagem { get; set; }

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int32 Id { get; set; }
    }
}