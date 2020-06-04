using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoApostilaDownload", Namespace = "a")]
    public class QuestaoApostilaDownload
    {
        [DataMember(Name = "Questao", EmitDefaultValue = false)]
        public Questao Questao { get; set; }

        [DataMember(Name = "ForumRecurso", EmitDefaultValue = false)]
        public ForumQuestaoRecurso ForumRecurso { get; set; }

        [DataMember(Name = "Estatistica", EmitDefaultValue = false)]
        public Dictionary<string, string> Estatistica { get; set; }
    }
}