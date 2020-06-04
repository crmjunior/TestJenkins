namespace MedCore_DataAccess.DTO
{
    public class AulaAvaliacaoConteudoDTO
    {
        public string Qualidade { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public string Link { get; set; }
        public string[] videosUrl { get; set; }
    }
}