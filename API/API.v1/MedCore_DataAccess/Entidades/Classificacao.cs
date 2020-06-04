using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Classificacao", Namespace = "a")]
    public class Classificacao
    {
        public enum TipoClassificacao
        {
            TurmaConvidada2015 = 127,
            TurmaConvidada2016 = 134,
            TurmaConvidada2017 = 144,
            TurmaConvidada2018 = 149,
            TurmaConvidada2019 = 156
        }

        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Atributo")]
        public int Atributo { get; set; }

        [DataMember(Name = "Cliente")]
        public Cliente Cliente { get; set; }

        [DataMember(Name = "DescricaoAtributo")]
        public string DescricaoAtributo { get; set; }

        [DataMember(Name = "Ano")]
        public int Ano { get; set; }
    }
}