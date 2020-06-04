using System.Collections.Generic;

namespace MedCore_API.ViewModel.Base
{
    public class AulaAvaliacaoTemaViewModel
    {
        public int ID { get; set; }
        public int TemaId { get; set; }
        public int AvaliacaoId { get; set; }
        public string Data { get; set; }
        public bool IsAvaliado { get; set; }
        public string Nome { get; set; }
        public bool PodeAvaliar { get; set; }
        public int ProfessorId { get; set; }
        public string ProfessorFoto { get; set; }
        public string ProfessorNome { get; set; }
        public string Rotulo { get; set; }
        public List<AulaAvaliacaoSlideViewModel> Slides { get; set; }


    }
}