using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "LogSimuladoImpresso", Namespace = "a")]
    public class LogSimuladoImpresso
    {
        [DataMember(Name = "matricula")]
        public int Matricula { get; set; }

        [DataMember(Name = "AplicacaoID")]
        public int AplicacaoId { get; set; }

        [DataMember(Name = "SimuladoId")]
        public int SimuladoId { get; set; }

        [DataMember(Name = "AcaoId")]
        public int AcaoId { get; set; }
    }
}