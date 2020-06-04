using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestoesMontaProvaPost", Namespace = "a")]
    public class QuestoesMontaProvaPost
    {
        [DataMember(Name = "Quantidade", EmitDefaultValue = false)]
        public int Quantidade { get; set; }
    }
}