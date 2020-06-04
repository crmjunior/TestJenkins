using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class AddOnApostila
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Posicao")]
        public string Posicao { get; set; }

        [DataMember(Name = "Conteudo")]
        public string Conteudo { get; set; }

        [DataMember(Name = "IdApostila")]
        public int IdApostila { get; set; }
    }
}