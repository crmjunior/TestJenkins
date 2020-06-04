using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class CartaoRespostaSimuladoAgendadoDTO
    {
        public CartaoRespostaSimuladoAgendadoDTO()
        {
            Questoes = new List<QuestaoSimuladoAgendadoDTO>();
        }
        public List<QuestaoSimuladoAgendadoDTO> Questoes { get; set; }

        public int ClientID { get; set; }

        public int HistoricoId { get; set; }
    }
}