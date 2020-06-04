namespace MedCore_DataAccess.DTO
{
    public class CronogramaPlaylistVideosDTO
    {
        public int ID { get; set; }
        public bool Ativo { get; set; }
        public bool Assistido { get; set; }
        public string Descricao { get; set; }
        public string PalavrasChaves { get; set; }
        public int Progresso { get; set; }
        public string DuracaoFormatada { get; set; }
        public string Thumb { get; set; }
        public int IdOrigemVideo { get; set; }

    }
}