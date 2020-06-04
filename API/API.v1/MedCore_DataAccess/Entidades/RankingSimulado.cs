using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "RankingSimulado", Namespace = "a")]
    public class RankingSimulado
    {
        [DataMember(Name = "P")]
        public string Posicao { get; set; }

        [DataMember(Name = "N")]
        public double NotaFinal { get; set; }

        [DataMember(Name = "Nk")]
        public string NickName { get; set; }

        [DataMember(Name = "Curso", EmitDefaultValue = false)]
        public string Curso { get; set; }

        [DataMember(Name = "E")]
        public string Especialidade { get; set; }

        [DataMember(Name = "F")]
        public string Filial { get; set; }

        [DataMember(Name = "A")]
        public int Acertos { get; set; }

        [DataMember(Name = "IU")]
        public int IdUf { get; set; }

        [DataMember(Name = "U")]
        public string Uf { get; set; }

        [DataMember(Name = "NotaObjetiva")]
        public double NotaObjetiva { get; set; }

        [DataMember(Name = "NotaDiscursiva")]
        public double NotaDiscursiva { get; set; }

        [DataMember(Name = "Local", EmitDefaultValue = false)]
        public string Local { get; set; }

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }
    }
}