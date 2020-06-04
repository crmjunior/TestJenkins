namespace MedCore_DataAccess.DTO
{
    public class QuestaoFiltroDTO
    {
        public int QuestaoId { get; set; }

        public string ConcursoSigla { get; set; }

        public int Ano { get; set; }

        public string Estado { get; set; }

        public string Anotacao { get; set; }

        public bool Impressa { get; set; }

        public bool Favorita { get; set; }

        public bool Anotada { get; set; }

        public bool Incorreta { get; set; }

        public bool NaoRespondida { get; set; }

        public int TipoExercicioId { get; set; }
    }
}