using System.Collections.Generic;

namespace MedCoreAPI.ViewModel.Base
{
    public class CartaoRespostaSimuladoAgendadoViewModel
    {
        public CartaoRespostaSimuladoAgendadoViewModel()
        {
            Questoes = new List<QuestaoSimuladoAgendadoCartaoRespostaViewModel>();
        }
        public List<QuestaoSimuladoAgendadoCartaoRespostaViewModel> Questoes { get; set; }

        public int ClientID { get; set; }

        public int HistoricoId { get; set; }
    }
}