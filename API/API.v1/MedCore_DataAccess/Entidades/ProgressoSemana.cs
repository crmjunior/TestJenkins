using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ProgressoSemana", Namespace = "a")]
    public class ProgressoSemana
    {
        [DataMember(Name = "IdEntidade", EmitDefaultValue = false)]
        public long IdEntidade { get; set; }

        [DataMember(Name = "PercentLido", EmitDefaultValue = false)]
        public int PercentLido { get; set; }
    }
}