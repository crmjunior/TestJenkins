using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class VideoMapaMentalDTO
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int ProfessorId { get; set; }
        public int TemaId { get; set; }

        public List<AulaAvaliacaoConteudoDTO> Links { get; set; }

    }
}