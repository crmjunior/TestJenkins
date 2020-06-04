using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Rascunho", Namespace = "a")]
    public class Rascunho
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "EmployeeId", EmitDefaultValue = false)]
        public int EmployeeId { get; set; }

        [DataMember(Name = "QuestaoId", EmitDefaultValue = false)]
        public int QuestaoId { get; set; }

        [DataMember(Name = "TextoRascunho", EmitDefaultValue = false)]
        public string TextoRascunho { get; set; }
    }
}