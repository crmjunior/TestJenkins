using System;

namespace MedCore_DataAccess.DTO
{
    public class TurmaMatriculaBaseDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int Ano { get; set; }

        public int MatriculaBase { get; set; }

        public DateTime DataCadastro { get; set; }

        public int DiasLimite { get; set; }

        public int ProdutoId { get; set; }
    }
}