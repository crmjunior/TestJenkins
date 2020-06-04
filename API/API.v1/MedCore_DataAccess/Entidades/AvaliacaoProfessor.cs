using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AvaliacaoProfessor", Namespace = "")]
    public class AvaliacaoProfessor
    {
        [DataMember(Name = "Professor", EmitDefaultValue = false)]
        public Pessoa Professor { get; set; }

        [DataMember(Name = "Nota1")]
        public int Nota1 { get; set; }

        [DataMember(Name = "Nota2")]
        public int Nota2 { get; set; }

        [DataMember(Name = "Nota3")]
        public int Nota3 { get; set; }

        [DataMember(Name = "Nota4")]
        public int Nota4 { get; set; }

        [DataMember(Name = "TotalNotas")]
        public int TotalNotas { get; set; }

        [DataMember(Name = "NotaFinal")]
        public int NotaFinal { get; set; }
    }
}