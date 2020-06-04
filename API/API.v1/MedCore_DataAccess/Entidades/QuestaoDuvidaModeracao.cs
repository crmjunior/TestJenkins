using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoDuvidaModeracao", Namespace = "a")]
    public class QuestaoDuvidaModeracao
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "QuestaoDuvidaID", EmitDefaultValue = false)]
        public int QuestaoDuvidaID { get; set; }

        [DataMember(Name = "EmployeeID", EmitDefaultValue = false)]
        public int EmployeeID { get; set; }

        [DataMember(Name = "DataModeracao", EmitDefaultValue = false)]
        public DateTime DataModeracao { get; set; }

        [DataMember(Name = "TextoModeracao", EmitDefaultValue = false)]
        public string TextoModeracao { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool Ativo { get; set; }
    }
}