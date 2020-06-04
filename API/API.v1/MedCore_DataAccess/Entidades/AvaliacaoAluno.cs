using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AvaliacaoAluno", Namespace = "a")]
    public class AvaliacaoAluno
    {
        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "Nota", EmitDefaultValue = false)]
        public string Nota { get; set; }

        [DataMember(Name = "Comentario", EmitDefaultValue = false)]
        public string Comentario { get; set; }

        [DataMember(Name = "VersaoApp", EmitDefaultValue = false)]
        public string VersaoApp { get; set; }

        [DataMember(Name = "VersaoPlataforma", EmitDefaultValue = false)]
        public string VersaoPlataforma { get; set; }

        [DataMember(Name = "Plataforma", EmitDefaultValue = false)]
        public string Plataforma { get; set; }

        [DataMember(Name = "AvaliarMaisTarde", EmitDefaultValue = false)]
        public bool AvaliarMaisTarde { get; set; }
    }
}