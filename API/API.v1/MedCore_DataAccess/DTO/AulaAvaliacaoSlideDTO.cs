using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class AulaAvaliacaoSlideDTO
    {
        public int Tipo { get; set; }
        public List<AulaAvaliacaoConteudoDTO> Conteudo { get; set; }
    }
}