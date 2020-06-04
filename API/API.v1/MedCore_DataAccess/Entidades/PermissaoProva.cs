using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PermissaoProva", Namespace = "a")]
    public class PermissaoProva
    {
        [DataMember(Name = "Favorito")]
        public int Favorito { get; set; }

        [DataMember(Name = "Gabarito")]
        public int Gabarito { get; set; }

        [DataMember(Name = "Estatística")]
        public int Estatística { get; set; }

        [DataMember(Name = "ComentarioVideo")]
        public int ComentarioVideo { get; set; }

        [DataMember(Name = "ComentarioTexto")]
        public int ComentarioTexto { get; set; }

        [DataMember(Name = "Recursos")]
        public int Recursos { get; set; }

        [DataMember(Name = "Cronometro")]
        public int Cronometro { get; set; }

        [DataMember(Name = "Impressao")]
        public int Impressao { get; set; }

    }
}