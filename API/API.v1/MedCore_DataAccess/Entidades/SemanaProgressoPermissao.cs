using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "SemanaProgressoPermissao", Namespace = "a")]
    public class SemanaProgressoPermissao
    {
        [DataMember(Name = "MenuId", EmitDefaultValue = false)]
        public int MenuId { get; set; }

        [DataMember(Name = "ProgressoSemanas", EmitDefaultValue = false)]
        public List<ProgressoSemana> ProgressoSemanas { get; set; }

        [DataMember(Name = "PermissaoSemanas", EmitDefaultValue = false)]
        public List<Semana> PermissaoSemanas { get; set; }

    }
}