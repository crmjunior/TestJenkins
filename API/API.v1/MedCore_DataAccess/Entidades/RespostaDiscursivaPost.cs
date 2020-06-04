using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Questao", Namespace = "a")]
    public class RespostaDiscursivaPost
    {
        [DataMember(Name = "HistoricoId", EmitDefaultValue = false)]
        public int HistoricoId { get; set; }

        [DataMember(Name = "QuestaoId", EmitDefaultValue = false)]
        public int QuestaoId { get; set; }

        [DataMember(Name = "DiscursivaId", EmitDefaultValue = false)]
        public int DiscursivaId { get; set; }

        [DataMember(Name = "ExercicioId", EmitDefaultValue = false)]
        public int ExercicioId { get; set; }

        [DataMember(Name = "ExercicioTipoId", EmitDefaultValue = false)]
        public int ExercicioTipoId { get; set; }

        [DataMember(Name = "Resposta", EmitDefaultValue = false)]
        public string Resposta { get; set; }


    }
}