using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DistribuicaoProtocolo", Namespace = "a")]
    public class DistribuicaoProtocolo
    {
        [DataMember(Name = "Professores", EmitDefaultValue = false)]
        public List<Professor> Professores { get; set; }

        [DataMember(Name = "Questoes", EmitDefaultValue = false)]
        public List<PPQuestao> Questoes { get; set; }
    }
}