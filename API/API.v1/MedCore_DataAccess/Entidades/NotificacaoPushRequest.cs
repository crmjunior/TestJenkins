using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "NotificacaoPushPost", Namespace = "")]
    public class NotificacaoPushRequest
    {
        [DataMember(Name = "ParametrosAvaliacaoAula")]
        public ParametrosAvaliacaoAula ParametrosAvaliacaoAula { get; set; }

        [DataMember(Name = "ParametrosPrimeiraAula")]
        public ParametrosPrimeiraAula ParametrosPrimeiraAula { get; set; }
    }
}