using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "LogLogin", Namespace = "a")]
    public class LogLogin
    {
        [DataMember(Name = "Matricula")]
        public int Matricula { get; set; }

        [DataMember(Name = "AplicacaoId")]
        public int AplicacaoId { get; set; }

        [DataMember(Name = "AcessoId")]
        public int AcessoId { get; set; }
    }
}