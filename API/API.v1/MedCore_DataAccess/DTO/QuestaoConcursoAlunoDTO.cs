namespace MedCore_DataAccess.DTO
{
    public class QuestaoConcursoAlunoDTO
    {
        public int QuestaoId { get; set; }
        public string Alternativa { get; set; }
        public bool AlternativaCorreta { get; set; }
        public bool Anulada { get; set; }
    }
}