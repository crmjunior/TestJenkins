using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class RespostaConcursoDTO
    {
        public int QuestaoId { get; set; }

        public string Alternativa { get; set; }

        public bool Correta { get; set; }

        public string AlternativaRespondida { get; set; }

        public bool Anulada { get; set; }

        public DateTime? DteCadastro { get; set; }

        public List<RespostaConcursoDTO> AcademicoObjecto { get; set; }
    }
}