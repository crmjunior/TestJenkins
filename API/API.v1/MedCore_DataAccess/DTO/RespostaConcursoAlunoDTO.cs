using System;

namespace MedCore_DataAccess.DTO
{
    public class RespostaConcursoAlunoDTO
    {
        public int QuestaoId { get; set; }

        public string Alternativa { get; set; }

        public DateTime? Ultimoregistro { get; set; }

    }
}