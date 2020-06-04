using System;

namespace MedCore_DataAccess.DTO
{
    public class MaterialApostilaDTO
    {
        public int ID { get; set; }
        public int? ProductId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}