using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "RecursoAlunoLog", Namespace = "a")]
    public class RecursoAlunoLog
    {
        [DataMember(Name = "RecursoConcedidoPelaBanca")]
        public bool RecursoConcedidoPelaBanca { get; set; }

        [DataMember(Name = "StatusAnaliseAcademica")]
        public int StatusAnaliseAcademica { get; set; }

        [DataMember(Name = "QuestaoId")]
        public int QuestaoId { get; set; }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

    }
}