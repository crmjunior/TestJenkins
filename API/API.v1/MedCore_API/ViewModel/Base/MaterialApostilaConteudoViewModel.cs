using System;

namespace MedCoreAPI.ViewModel.Base
{
    public class MaterialApostilaConteudoViewModel
    {
        public int ID { get; set; }
        public int? ProductId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}