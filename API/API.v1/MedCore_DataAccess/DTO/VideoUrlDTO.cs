namespace MedCore_DataAccess.DTO
{
    public class VideoUrlDTO
    {
        public string Url { get; set; }
        public int UltimaPosicaoAluno { get; set; }
        public VideoQualidadeDTO[] Links { get; set; }
        public int Duracao { get; set; }
    }
}