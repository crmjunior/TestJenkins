using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class TemaAula
    {

        [DataMember(Name = "NomeTema", EmitDefaultValue = false)]
        public string NomeTema { get; set; }

        [DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public string Titulo { get; set; }

        [DataMember(Name = "Hora", EmitDefaultValue = false)]
        public string Hora { get; set; }

        [DataMember(Name = "IdProduto", EmitDefaultValue = false)]
        public int IdProduto { get; set; }

        [DataMember(Name = "Tempo", EmitDefaultValue = false)]
        public string Tempo { get; set; }

    }
}