using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ListaProdutosDTO
    {
        public List<int> ID { get; set; }

        public string Descricao { get; set; }

        public int Ordenacao { get; set; }
    }
}