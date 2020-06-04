using System.Collections.Generic;

namespace MedCore_API.ViewModel.Base
{
    public class AulaAvaliacaoViewModel
    {
        public int ID { get; set; }
        public int Ano { get; set; }
        public bool Aprovada { get; set; }
        public int IdGrandeArea { get; set; }
        public int IdProduto { get; set; }
        public int IdSubEspecialidade { get; set; }
        public string NomeCompleto { get; set; }
        public string Titulo { get; set; }
        public int IdApostila { get; set; }
        public List<AulaAvaliacaoTemaViewModel> Temas { get; set; }

    }
}