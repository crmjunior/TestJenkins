using System.Collections.Generic;

namespace MedCore_API.ViewModel.Base
{
    public class AulaAvaliacaoSlideViewModel
    {
        public int Tipo { get; set; }
        public List<AulaAvaliacaoConteudoViewModel> Conteudo { get; set; }
    }
}