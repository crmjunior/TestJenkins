using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AvaliacaoVideoApostila", Namespace = "a")]
    public class AvaliacaoVideoApostila
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "MaterialID")]
        public int MaterialID { get; set; }

        [DataMember(Name = "VideoID")]
        public int VideoID { get; set; }

        [DataMember(Name = "Matricula")]
        public int Matricula { get; set; }

        [DataMember(Name = "TipoVote")]
        public int TipoVote { get; set; }
    }
}