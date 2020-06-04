using System.ComponentModel;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "RecursoQuestao", Namespace = "a")]
    public class QuestaoRecurso : Questao
    {
        [DataMember(Name = "ComentarioAtivo", EmitDefaultValue = false)]
        public bool ComentarioAtivo { get; set; }

        [DataMember(Name = "IdConcursoQuestaoStatus")]
        public int IdConcursoQuestaoStatus { get; set; }

        [DataMember(Name = "ConcursoQuestaoStatus", EmitDefaultValue = false)]
        public string ConcursoQuestaoStatus { get; set; }

        [DataMember(Name = "ProvaId", EmitDefaultValue = false)]
        public int ProvaId { get; set; }

        [DataMember(Name = "StatusConcurso", EmitDefaultValue = false)]
        public int StatusConcurso { get; set; }

        [DataMember(Name = "StatusBanca", EmitDefaultValue = false)]
        public int StatusBanca { get; set; }

        [DataMember(Name = "TemVereditoBanca", EmitDefaultValue = false)]
        public bool TemVereditoBanca { get; set; }

        //[DataMember(Name = "Recurso", EmitDefaultValue = false)]
        //public string Recurso { get; set; }

        //[DataMember(Name = "Professor", EmitDefaultValue = false)]
        //public Pessoa Professor { get; set; }

        public enum StatusBancaAvaliadora
        {
            Indefinido = 0,
            EmAnalise = 10,
            Sim = 11,
            Nao = 12,
        }

        public enum StatusQuestao
        {
            [Description("I")]
            Invalido = -1,

            [Description("X")]
            NaoExiste = 0,

            [Description("N")]
            NaoCabeRecurso = 3,

            [Description("C")]
            CabeRecurso = 4,

            [Description("E")]
            EmAnalise = 5,

            [Description("B")]
            EmAnaliseBloqueada = 7,

            [Description("R")]
            NaoSolicitado = 8,

            [Description("A")]
            AlteradaPelaBanca = 11
        }

        public enum StatusProva
        {
            Bloqueada = 2,
        }

        public enum StatusGabarito
        {
            [Description("A")]
            Anulada = 1,

            [Description("R")]
            AnuladaAposRecurso = 2,

            [Description("P")]
            GabaritoPreliminar = 3,

            [Description("O")]
            GabaritoOficial = 4
        }

        public enum TipoForumRecurso
        {
            Pre = 1,
            Pos = 2,
        }

        public enum StatusOpiniao
        {
            [Description("F")]
            Favor = 1,

            [Description("C")]
            Contra = 2
        }
    }
}