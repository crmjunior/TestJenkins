using System;

namespace MedCore_DataAccess.DTO
{
    public class AulaAvaliacaoConsultaDTO
    {
        public int ID { get; set; }

        public string ProfessorNome { get; set; }

        public int ProfessorID { get; set; }

        public string ProdutoNome { get; set; }

        public int ProdutoID { get; set; }

        public string TemaNome { get; set; }

        public int AulaID { get; set; }

        public int TemaID { get; set; }

        public DateTime TemaData { get; set; }

        public int AlunoID { get; set; }

        public int? AlunoStatus { get; set; }

        public int SalaAula { get; set; }

        public DateTime? DataPrevisao1 { get; set; }

        public DateTime? DataPrevisao2 { get; set; }
    }
}