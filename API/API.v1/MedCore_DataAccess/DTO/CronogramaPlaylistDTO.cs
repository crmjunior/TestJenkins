using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class CronogramaPlaylistDTO
    {
        public int IdTema { get; set; }
        public int Ativa { get; set; }
        public string Nome { get; set; }
        public int PercentMedia { get; set; }
        public List<CronogramaPlaylistVideosDTO> Videos { get; set; } 
    }
}