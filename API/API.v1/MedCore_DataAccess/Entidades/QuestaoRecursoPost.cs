using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoRecursosPost", Namespace = "a")]
    public class QuestaoRecursosPost
    {
        [DataMember(Name = "IdQuestao", EmitDefaultValue = false)]
        public int IdQuestao { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "AlternativaSelecionada", EmitDefaultValue = false)]
        public string AlternativaSelecionada { get; set; }

        [DataMember(Name = "AlternativaCorreta", EmitDefaultValue = false)]
        public bool AlternativaCorreta { get; set; }
    }
}