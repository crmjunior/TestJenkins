using System;

namespace MedCore_DataAccess.DTO
{
    public class ProdutoComboLiberadoDTO
    {
        public int intCurso { get; set; }
        public int intYear { get; set; }
        public String Nome { get; set; }
        public int Id { get; set; }
        public int Ordem { get; set; }
        public bool ProdutoFake { get; set; }
    }
}