using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AlunoConcursoEstatistica", Namespace = "a")]
    public class AlunoConcursoEstatistica
    {
        [DataMember(Name = "Acertos")]
        public int Acertos { get; set; }

        [DataMember(Name = "Erros")]
        public int Erros { get; set; }

        [DataMember(Name = "NaoRealizadas")]
        public int NaoRealizadas { get; set; }

        [DataMember(Name = "TotalQuestoes")]
        public int TotalQuestoes { get; set; }

        [DataMember(Name = "Nota")]
        public int Nota { get; set; }

    }
}