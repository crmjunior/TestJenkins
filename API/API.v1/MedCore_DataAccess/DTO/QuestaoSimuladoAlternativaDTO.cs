namespace MedCore_DataAccess.DTO
{
    public class QuestaoSimuladoAlternativaDTO
    {
        public int intQuestaoID { get; set; }
        public string txtLetraAlternativa { get; set; }
        public string txtAlternativa { get; set; }
        public bool? bitCorreta { get; set; }
        public string txtResposta { get; set; }
        public int intAlternativaID { get; set; }
    }
}