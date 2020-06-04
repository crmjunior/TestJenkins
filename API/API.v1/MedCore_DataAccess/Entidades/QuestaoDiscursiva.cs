using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoDiscursiva", Namespace = "a")]
    public class QuestaoDiscursiva
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int32 Id { get; set; }

        [DataMember(Name = "QuestaoId", EmitDefaultValue = false)]
        public Int32 QuestaoId { get; set; }

        [DataMember(Name = "Enunciado", EmitDefaultValue = false)]
        public String Enunciado { get; set; }

        [DataMember(Name = "Resposta", EmitDefaultValue = false)]
        public String Resposta { get; set; }

        [DataMember(Name = "ExercicioTipo", EmitDefaultValue = false)]
        public Int32 ExercicioTipo { get; set; }

        [DataMember(Name = "Tempo", EmitDefaultValue = false)]
        public Int32 Tempo { get; set; }

        [DataMember(Name = "Imagem", EmitDefaultValue = false)]
        public String Imagem { get; set; }

        [DataMember(Name = "ImagemOtimizada", EmitDefaultValue = false)]
        public String ImagemOtimizada { get; set; }

        public QuestaoDiscursiva()
        {

        }
    }
}