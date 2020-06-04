using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AulaTema", Namespace = "")]
    public class AulaTema
    {
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int AulaID { get; set; }

        [DataMember(Name = "Data", EmitDefaultValue = false)]
        public string Data { get; set; }

        [DataMember(Name = "IsAvaliado", EmitDefaultValue = false)]
        public bool IsAvaliado { get; set; }

        [DataMember(Name = "PodeAvaliar", EmitDefaultValue = true)]
        public bool PodeAvaliar { get; set; }

        [DataMember(Name = "ProfessorNome", EmitDefaultValue = false)]
        public string ProfessorNome { get; set; }

        [DataMember(Name = "ProfessorID", EmitDefaultValue = false)]
        public int ProfessorID { get; set; }

        [DataMember(Name = "ProfessorFoto", EmitDefaultValue = false)]
        public string ProfessorFoto { get; set; }

        [DataMember(Name = "Rotulo", EmitDefaultValue = false)]
        public string Rotulo { get; set; }

        [DataMember(Name = "Slides", EmitDefaultValue = false)]
        public List<string> Slides { get; set; }

        [DataMember(Name = "AvaliacaoID", EmitDefaultValue = false)]
        public int AvaliacaoID { get; set; }

        [DataMember(Name = "TemaID", EmitDefaultValue = true)]
        public int TemaID { get; set; }
    }
}